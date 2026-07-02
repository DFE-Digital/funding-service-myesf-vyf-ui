using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.LayoutManagement
{
    /// <summary>
    /// The Provider Summary Layout Action class.
    /// </summary>
    public class ProviderSummaryLayoutAction : IPreviewLayoutAction
    {
        /// <inheritdoc />
        public bool AppliesTo(FundingViewScope fundingViewScope)
        {
            return fundingViewScope == FundingViewScope;
        }

        /// <inheritdoc />
        public FundingViewScope FundingViewScope => FundingViewScope.ProviderSummary;

        /// <inheritdoc />
        public Task<(string RouteName, object routeValues)> GetRouteNameAndValues(
            PreviewLayoutViewModel previewLayoutModel)
        {
            object routeValues = new
            {
                previewLayoutModel.OrganisationUkprn,
                previewLayoutModel.LayoutId,
                previewLayoutModel.FundingStreamId,
                previewLayoutModel.FundingViewScope,
                preview = true
            };
            var result = (ViewYourFundingConstants.RouteName_ProviderStatement, routeValues);

            return Task.FromResult(result);
        }
    }
}