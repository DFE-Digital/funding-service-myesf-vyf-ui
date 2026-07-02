using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.LayoutManagement
{
    /// <summary>
    /// The Organisation Summary Layout Action class.
    /// </summary>
    public class OrganisationSummaryLayoutAction : IPreviewLayoutAction
    {
        /// <inheritdoc />
        public bool AppliesTo(FundingViewScope fundingViewScope)
        {
            return fundingViewScope == FundingViewScope;
        }

        /// <inheritdoc />
        public FundingViewScope FundingViewScope => FundingViewScope.OrganisationSummary;

        /// <inheritdoc />
        public Task<(string RouteName, object routeValues)> GetRouteNameAndValues(
            PreviewLayoutViewModel previewLayoutModel)
        {
            object routeValues = new
            {
                previewLayoutModel.LocalAuthorityCode,
                previewLayoutModel.LayoutId,
                previewLayoutModel.FundingStreamId,
                previewLayoutModel.FundingViewScope,
                preview = true
            };
            var result = (ViewYourFundingConstants.RouteName_LocalAuthorityStatement, routeValues);

            return Task.FromResult(result);
        }
    }
}
