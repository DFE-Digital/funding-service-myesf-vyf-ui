using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Shared;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home
{
    /// <summary>
    /// Base class for the Home Page page' view models.
    /// </summary>
    public abstract class HomePageViewModel : AdminPageBaseViewModel
    {
        /// <summary>
        /// Gets the breadcrumb for the funding streams list page.
        /// </summary>
        /// <param name="isCurrentPage">A bool indicating whether or not this is the current page.</param>
        /// <returns>The breadcrumb for the funding streams list page.</returns>
        protected BreadCrumbViewModel SettingsListBreadCrumb(bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "View Your Funding settings",
                        RouteName = "Index",
                    }
                };

        /// <summary>
        /// Gets the breadcrumb for the funding stream settings list page.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream ID.</param>
        /// <param name="fundingStreamName">The funding stream name.</param>
        /// <param name="isCurrentPage">A bool indicating whether or not this is the current page.</param>
        /// <returns>The breadcrumb for the funding stream settings list page.</returns>
        protected BreadCrumbViewModel SettingsFundingStreamBreadCrumb(
            int fundingStreamId,
            string fundingStreamName,
            bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $"{fundingStreamName} settings",
                        RouteName = "FundingStream",
                        Parameters = new { FundingStreamId = fundingStreamId }
                    }
                };

        /// <summary>
        /// Gets the breadcrumb for the funding stream stream edit page.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="fundingStreamName">Name of the funding stream.</param>
        /// <param name="isCurrentPage">if set to <c>true</c> if it is the current page.</param>
        /// <returns>The breadcrumb for the settings edit page.</returns>
        protected BreadCrumbViewModel SettingsFundingStreamEditBreadCrumb(
            int fundingStreamId,
            string fundingStreamName,
            bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $" Edit {fundingStreamName} settings",
                        RouteName = "FundingStream",
                        Parameters = new { FundingStreamId = fundingStreamId }
                    }
                };
    }
}