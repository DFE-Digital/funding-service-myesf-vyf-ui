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
    /// The National Layout Action class.
    /// </summary>
    public class NationalLayoutAction : PreviewLayoutActionBase, IPreviewLayoutAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalLayoutAction"/> class.
        /// </summary>
        /// <param name="adminSettingsService">The admin settings service.</param>
        public NationalLayoutAction(
            IAdminSettingsService adminSettingsService)
        : base(adminSettingsService)
        {
        }

        /// <inheritdoc />
        public bool AppliesTo(FundingViewScope fundingViewScope)
        {
            return fundingViewScope == FundingViewScope;
        }

        /// <inheritdoc />
        public FundingViewScope FundingViewScope => FundingViewScope.National;

        /// <inheritdoc />
        public async Task<(string RouteName, object routeValues)> GetRouteNameAndValues(
            PreviewLayoutViewModel previewLayoutModel)
        {
            var fundingStream = await GetFundingStreamById(
                previewLayoutModel.FundingStreamId,
                new[] { FetchData.SettingValues_Setting, FetchData.Publications });

            var activeFundingPeriodCodes = FundingPeriodHelper.GetActiveFundingPeriodCodes(fundingStream, true);

            var (yearFrom, yearTo) = FundingPeriodHelper.GetLatestYears(
                fundingStream.SettingValues, activeFundingPeriodCodes).First();

            var publishedDate = GetPublishedDatePathFormatted(fundingStream);
            var yearSettingCode = FundingPeriodHelper.GetYearSettingCode(fundingStream.SettingValues);

            object routeValues = null;

            if (fundingStream != null)
            {
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
                        previewLayoutModel.LayoutId,
                        previewLayoutModel.FundingViewScope,
                        previewLayoutModel.FundingViewType,
                        previewLayoutModel.FundingStreamId,
                    };

                    return (ViewYourFundingConstants.RouteName_PreviewNationalSpreadsheetDownload, routeValues);
                }

                routeValues = new
                {
                    previewLayoutModel.LayoutId,
                    previewLayoutModel.FundingViewScope,
                    previewLayoutModel.FundingStreamId,
                    fundingStream.FundingStreamCode,
                    fundingStream.FundingStreamName,
                    yearFrom,
                    yearTo,
                    preview = true,
                };
            }

            return (ViewYourFundingConstants.RouteName_NationalFundingAllocation, routeValues);
        }
    }
}
