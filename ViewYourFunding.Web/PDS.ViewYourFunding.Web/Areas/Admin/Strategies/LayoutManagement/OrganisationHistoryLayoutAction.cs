using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.LayoutManagement
{
    /// <summary>
    /// The Organisation History Layout Action class.
    /// </summary>
    public class OrganisationHistoryLayoutAction : PreviewLayoutActionBase, IPreviewLayoutAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationHistoryLayoutAction"/> class.
        /// </summary>
        /// <param name="adminSettingsService">The admin settings service.</param>
        public OrganisationHistoryLayoutAction(IAdminSettingsService adminSettingsService)
            : base(adminSettingsService)
        {
        }

        /// <inheritdoc />
        public bool AppliesTo(FundingViewScope fundingViewScope)
        {
            return fundingViewScope == FundingViewScope;
        }

        /// <inheritdoc />
        public FundingViewScope FundingViewScope => FundingViewScope.OrganisationHistory;

        /// <inheritdoc />
        public async Task<(string RouteName, object routeValues)> GetRouteNameAndValues(
            PreviewLayoutViewModel previewLayoutModel)
        {
            var fundingStream = await GetFundingStreamById(previewLayoutModel.FundingStreamId);
            object routeValues = null;

            if (fundingStream != null)
            {
                routeValues = new
                {
                    fundingStreamName = fundingStream.FundingStreamName.ToUIPathComponent(),
                    previewLayoutModel.LocalAuthorityCode,
                    previewLayoutModel.LayoutId,
                    previewLayoutModel.FundingViewScope,
                    previewLayoutModel.FundingStreamId,
                    preview = true
                };
            }

            return (ViewYourFundingConstants.RouteName_LocalAuthorityHistory, routeValues);
        }
    }
}
