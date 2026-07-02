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
    /// The Provider History Single Year Layout Action class.
    /// </summary>
    public class ProviderHistorySingleYearLayoutAction : PreviewLayoutActionBase, IPreviewLayoutAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderHistorySingleYearLayoutAction"/> class.
        /// </summary>
        /// <param name="adminSettingsService">The admin settings service.</param>
        public ProviderHistorySingleYearLayoutAction(IAdminSettingsService adminSettingsService)
            : base(adminSettingsService)
        {
        }

        /// <inheritdoc />
        public bool AppliesTo(FundingViewScope fundingViewScope)
        {
            return fundingViewScope == FundingViewScope;
        }

        /// <inheritdoc />
        public FundingViewScope FundingViewScope => FundingViewScope.ProviderHistorySingleYear;

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

                var (year1, year2) = FundingPeriodHelper.GetLatestYears(
                    fundingStream.SettingValues, activeFundingPeriodCodes).First();

                routeValues = new
                {
                    fundingStreamName = fundingStream.FundingStreamName.ToUIPathComponent(),
                    previewLayoutModel.OrganisationUkprn,
                    previewLayoutModel.LayoutId,
                    previewLayoutModel.FundingViewScope,
                    previewLayoutModel.FundingStreamId,
                    year1 = --year1,
                    year2 = --year2,
                    preview = true
                };
            }

            return (ViewYourFundingConstants.RouteName_ProviderHistorySingleYear, routeValues);
        }
    }
}