using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Helpers
{
    public static class ProviderFundingHelper
    {
        public static List<KeyValuePair<(int yearFrom, int yearTo), List<ProviderFundingViewModel>>> GetFundingsGroupedByPeriod(List<IFundingApiSearchProviderFunding> providerFundings, FundingStream fundingStream, bool previewEnabled)
        {
            var activeFundingPeriods = FundingPeriodHelper.GetActiveFundingPeriodCodes(fundingStream, previewEnabled);
            var latestFundingPeriodCodes = FundingPeriodHelper.GetLatestFundingPeriodCodes_FundingPeriodFormat(fundingStream.SettingValues, activeFundingPeriods);
            var latestFunding = providerFundings.GetLatestFunding();
            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(latestFunding.FundingPeriodCode);
            var years = FundingPeriodHelper.GetCurrentAndHistoricYears(yearFrom, yearTo, ViewYourFundingConstants.NumberOfYearsOfHistoricAllocationsToShow);
            var fundingsByYear = GroupFundingsByYear(years, providerFundings, fundingStream);

            // Remove entries for years where no funding records were found.
            fundingsByYear.RemoveAll(keyValuePair => keyValuePair.Value.Count == 0);

            var fundingModel = new List<KeyValuePair<(int, int), List<ProviderFundingViewModel>>>();

            foreach (var funding in fundingsByYear)
            {
                var latestFundingForYear = funding.Value.GetLatestFunding();

                // Convert to the view model
                fundingModel.Add(new KeyValuePair<(int, int), List<ProviderFundingViewModel>>(funding.Key, funding.Value.Select(v =>
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
        /// Convert a services provider funding to a web model provider funding.
        /// </summary>
        /// <param name="providerFunding">The services provider funding to convert.</param>
        /// <returns>A web model provider funding.</returns>
        public static ProviderFundingViewModel AsWebModel(this IFundingApiSearchProviderFunding providerFunding)
        {
            if (providerFunding == null)
            {
                return null;
            }

            var webProviderFundingModel = new ProviderFundingViewModel
            {
                FundingPeriodCode = providerFunding.FundingPeriodCode,
                StatusChangedDate = providerFunding.StatusChangedDate,
                VariationReason = providerFunding.VariationReason,
                GroupingReason = providerFunding.GroupingReason
            };

            return webProviderFundingModel;
        }

        public static List<KeyValuePair<(int yearFrom, int yearTo), List<IFundingApiSearchProviderFunding>>>
            GroupFundingsByYear(
                List<(int yearFrom, int yearTo)> yearsToInclude,
                List<IFundingApiSearchProviderFunding> providerFundings,
                FundingStream fundingStream)
        {
            var fundingsByYear = new List<KeyValuePair<(int, int), List<IFundingApiSearchProviderFunding>>>();

            foreach (var (yearFrom, yearTo) in yearsToInclude)
            {
                var yearTypeCode = FundingPeriodHelper.GetYearSettingCode(fundingStream.SettingValues);
                var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(yearFrom, yearTo, yearTypeCode);

                var fundingsForYear = providerFundings
                    .Where(p => p.FundingPeriodCode == fundingPeriodCode)
                    .OrderByDescending(x => x.StatusChangedDate)
                    .ToList();

                var fundingCounter = fundingsForYear.Count - 1;
                foreach (var funding in fundingsForYear)
                {
                    if (funding.VariationReason == null || !funding.VariationReason.Contains("allocation", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        var isIndicative = funding.GroupingReason.Equals(GroupingReason.Indicative);
                        if (isIndicative)
                        {
                            funding.VariationReason = fundingCounter == 0 ? "Indicative allocation." : "Revised indicative allocation.";
                        }
                        else
                        {
                            funding.VariationReason = fundingCounter == 0 ? "Initial allocation." : "Revised allocation.";
                        }

                        fundingCounter--;
                    }
                }

                fundingsByYear.Add(
                    new KeyValuePair<(int, int), List<IFundingApiSearchProviderFunding>>((yearFrom, yearTo), fundingsForYear));
            }

            return fundingsByYear;
        }

        /// <summary>
        /// Get the latest funding by status date.
        /// </summary>
        /// <param name="providerFundings">The provider fundings.</param>
        /// <returns>The latest publication.</returns>
        private static IFundingApiSearchProviderFunding GetLatestFunding(this List<IFundingApiSearchProviderFunding> providerFundings)
        {
            return providerFundings
                .OrderByDescending(p => p.StatusChangedDate)
                .FirstOrDefault();
        }
    }
}