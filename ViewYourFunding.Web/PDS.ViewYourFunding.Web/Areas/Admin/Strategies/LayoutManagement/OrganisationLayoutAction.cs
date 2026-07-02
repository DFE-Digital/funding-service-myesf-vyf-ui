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
    /// The Organisation Layout Action class.
    /// </summary>
    public class OrganisationLayoutAction : PreviewLayoutActionBase, IPreviewLayoutAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationLayoutAction"/> class.
        /// </summary>
        /// <param name="adminSettingsService">The admin settings service.</param>
        public OrganisationLayoutAction(IAdminSettingsService adminSettingsService)
            : base(adminSettingsService)
        {
        }

        /// <inheritdoc />
        public bool AppliesTo(FundingViewScope fundingViewScope)
        {
            return fundingViewScope == FundingViewScope;
        }

        /// <inheritdoc />
        public FundingViewScope FundingViewScope => FundingViewScope.Organisation;

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

                if (previewLayoutModel.FundingViewType == FundingViewType.Spreadsheet)
                {
                    routeValues = new
                    {
                        fundingStream.FundingStreamCode,
                        yearFrom,
                        yearTo,
                        YearTypeCode = yearSettingCode,
                        Format = "ods",
                        publishedDate,
                        IsPreview = true,
                        previewLayoutModel.LocalAuthorityCode,
                        previewLayoutModel.LayoutId,
                        previewLayoutModel.FundingViewScope,
                        previewLayoutModel.FundingStreamId,
                        previewLayoutModel.FundingViewType
                    };
                    return (ViewYourFundingConstants.RouteName_LocalAuthoritySpreadsheetDownload, routeValues);
                }

                routeValues = new
                {
                    yearFrom,
                    yearTo,
                    fundingStreamName = fundingStream.FundingStreamName.ToUIPathComponent(),
                    fundingStream.FundingStreamCode,
                    previewLayoutModel.LocalAuthorityCode,
                    previewLayoutModel.LayoutId,
                    previewLayoutModel.FundingViewScope,
                    previewLayoutModel.FundingStreamId,
                    publishedDate,
                    preview = true
                };
            }

            return (ViewYourFundingConstants.RouteName_LocalAuthorityFundingBreakdown, routeValues);
        }
    }
}