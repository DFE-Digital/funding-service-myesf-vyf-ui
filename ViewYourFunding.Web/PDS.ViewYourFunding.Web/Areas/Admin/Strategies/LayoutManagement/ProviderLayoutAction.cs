using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.LayoutManagement
{
    /// <summary>
    /// The Provider layout Action class.
    /// </summary>
    public class ProviderLayoutAction : PreviewLayoutActionBase, IPreviewLayoutAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderLayoutAction"/> class.
        /// </summary>
        /// <param name="adminSettingsService">The admin settings service.</param>
        public ProviderLayoutAction(IAdminSettingsService adminSettingsService)
            : base(adminSettingsService)
        {
        }

        /// <inheritdoc />
        public FundingViewScope FundingViewScope => FundingViewScope.Provider;

        /// <inheritdoc />
        public bool AppliesTo(FundingViewScope fundingViewScope)
        {
            return fundingViewScope == FundingViewScope;
        }

        /// <inheritdoc />
        public async Task<(string RouteName, object routeValues)> GetRouteNameAndValues(
            PreviewLayoutViewModel previewLayoutModel)
        {
            var fundingStream = await GetFundingStreamById(
                previewLayoutModel.FundingStreamId,
                new[] { FetchData.SettingValues_Setting, FetchData.Publications });
            object routeValues = null;
            if (fundingStream != null)
            {
                var activeFundingPeriodCodes = FundingPeriodHelper.GetActiveFundingPeriodCodes(fundingStream, true);

                var (yearFrom, yearTo) = FundingPeriodHelper.GetLatestYears(
               fundingStream.SettingValues, activeFundingPeriodCodes).First();
                var publishedDate = GetPublishedDatePathFormatted(fundingStream);
                var yearSettingCode = FundingPeriodHelper.GetYearSettingCode(fundingStream.SettingValues);
                routeValues = new
                {
                    fundingStream.FundingStreamCode,
                    yearFrom,
                    yearTo,
                    YearTypeCode = yearSettingCode,
                    Format = "ods",
                    publishedDate,
                    IsPreview = true,
                    Ukprn = previewLayoutModel.OrganisationUkprn,
                    previewLayoutModel.LayoutId,
                    previewLayoutModel.FundingViewScope,
                    previewLayoutModel.FundingStreamId,
                    previewLayoutModel.FundingViewType
                };
            }

            return (ViewYourFundingConstants.RouteName_ProviderSpreadsheetDownload, routeValues);
        }
    }
}
