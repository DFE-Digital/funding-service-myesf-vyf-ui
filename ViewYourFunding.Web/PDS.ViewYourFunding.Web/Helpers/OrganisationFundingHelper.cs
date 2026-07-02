using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Helpers
{
    /// <summary>
    /// Helper methods when working with organisations, like for e.g.to group fundings based on a period etc.
    /// </summary>
    public static class OrganisationFundingHelper
    {
        /// <summary>
        /// Group fundings by period like for example (20 to 21, 21 to 22),
        /// remove entries for any years  where there  are no entries
        /// and return a list of LocalAuthorityFundingViewModel for each period.
        /// </summary>
        /// <param name="fundings"> Fundings to group by period.</param>
        /// <param name="fundingStream"> Funding stream to use for period etc.</param>
        /// <param name="previewEnabled">Preview enabled.</param>
        /// <returns> A list of LocalAuthorityFundingViewModel for each period.</returns>
        public static List<KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>> GetFundingsGroupedByPeriod(List<IFundingApiSearchFunding> fundings, FundingStream fundingStream, bool previewEnabled)
        {
            var activeFundingPeriodCodes = FundingPeriodHelper.GetActiveFundingPeriodCodes(fundingStream, previewEnabled);
            var latestFundingPeriodCodes = FundingPeriodHelper.GetLatestFundingPeriodCodes_FundingPeriodFormat(fundingStream.SettingValues, activeFundingPeriodCodes);
            var latestFunding = fundings.GetLatestFunding();
            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(latestFunding.FundingPeriodCode);
            var years = FundingPeriodHelper.GetCurrentAndHistoricYears(yearFrom, yearTo, ViewYourFundingConstants.NumberOfYearsOfHistoricAllocationsToShow);
            var fundingsByYear = GroupFundingsByYear(years, fundings, fundingStream);

            // Remove entries for years where no funding records were found.
            fundingsByYear.RemoveAll(keyValuePair => keyValuePair.Value.Count == 0);

            var fundingModel = new List<KeyValuePair<(int, int), List<LocalAuthorityFundingViewModel>>>();

            foreach (var funding in fundingsByYear)
            {
                var latestFundingForYear = funding.Value.GetLatestFunding();

                // Convert to the view model
                fundingModel.Add(new KeyValuePair<(int, int), List<LocalAuthorityFundingViewModel>>(funding.Key, funding.Value.Select(v =>
                {
                    var model = v.AsWebModel();
                    var isLatestFundingPeriodCode = latestFundingPeriodCodes.Contains(model.FundingPeriodCode);
                    var hasLatestStatusChangeDate = model.StatusChangedDate == latestFundingForYear.StatusChangedDate;
                    model.IsLatest = isLatestFundingPeriodCode && hasLatestStatusChangeDate;
                    model.IsFinal = !isLatestFundingPeriodCode && hasLatestStatusChangeDate;
                    return model;
                }).ToList()));
            }

            return fundingModel.ToList();
        }

        /// <summary>
        /// Convert a services funding to a web model organisation funding.
        /// </summary>
        /// <param name="funding">The services funding to convert.</param>
        /// <returns>A web model organisation funding.</returns>
        public static LocalAuthorityFundingViewModel AsWebModel(this IFundingApiSearchFunding funding)
        {
            if (funding == null)
            {
                return null;
            }

            var webFundingModel = new LocalAuthorityFundingViewModel
            {
                FundingPeriodCode = funding.FundingPeriodCode,
                StatusChangedDate = funding.StatusChangedDate,
                VariationReason = funding.VariationReason
            };

            return webFundingModel;
        }

        /// <summary>
        /// methodto group fundings  by the year.
        /// </summary>
        /// <param name="yearsToInclude">years  for which fundings will be grouped. </param>
        /// <param name="fundings">fundings to group.</param>
        /// <param name="fundingStream">funding stream.</param>
        /// <returns>fundings grouped by year.</returns>
        private static List<KeyValuePair<(int yearFrom, int yearTo), List<IFundingApiSearchFunding>>>
            GroupFundingsByYear(
                List<(int yearFrom, int yearTo)> yearsToInclude,
                List<IFundingApiSearchFunding> fundings,
                FundingStream fundingStream)
        {
            var fundingsByYear = new List<KeyValuePair<(int, int), List<IFundingApiSearchFunding>>>();

            foreach (var (yearFrom, yearTo) in yearsToInclude)
            {
                var yearTypeCode = FundingPeriodHelper.GetYearSettingCode(fundingStream.SettingValues);
                var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(yearFrom, yearTo, yearTypeCode);

                var fundingsForYear = fundings
                    .Where(p => p.FundingPeriodCode == fundingPeriodCode)
                    .OrderByDescending(x => x.StatusChangedDate)
                    .ToList();

                var fundingCounter = fundingsForYear.Count - 1;
                foreach (var funding in fundingsForYear)
                {
                    funding.VariationReason = fundingCounter == 0 ? "Initial allocation." : "Revised allocation.";
                    fundingCounter--;
                }

                fundingsByYear.Add(
                    new KeyValuePair<(int, int), List<IFundingApiSearchFunding>>((yearFrom, yearTo), fundingsForYear));
            }

            return fundingsByYear;
        }

        /// <summary>
        /// Get the latest funding by status date.
        /// </summary>
        /// <param name="fundings">The fundings.</param>
        /// <returns>The latest publication.</returns>
        private static IFundingApiSearchFunding GetLatestFunding(this List<IFundingApiSearchFunding> fundings)
        {
            return fundings
                .OrderByDescending(p => p.StatusChangedDate)
                .FirstOrDefault();
        }
    }
}