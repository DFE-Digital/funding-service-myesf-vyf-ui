using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.LayoutManagement
{
    /// <summary>
    /// The preview layout action interface.
    /// </summary>
    public interface IPreviewLayoutAction
    {
        /// <summary>
        /// Checks if it Applies to.
        /// </summary>
        /// <param name="fundingViewScope">The funding view scope.</param>
        /// <returns>True if it applies to the funding view scope.</returns>
        bool AppliesTo(FundingViewScope fundingViewScope);

        /// <summary>
        /// Gets the funding view scope.
        /// </summary>
        /// <value>
        /// The funding view scope.
        /// </value>
        FundingViewScope FundingViewScope { get; }

        /// <summary>
        /// Gets the route name and values.
        /// </summary>
        /// <param name="previewLayoutModel">The preview layout model.</param>
        /// <returns>The route name and route values.</returns>
        Task<(string RouteName, object routeValues)> GetRouteNameAndValues(PreviewLayoutViewModel previewLayoutModel);
    }
}