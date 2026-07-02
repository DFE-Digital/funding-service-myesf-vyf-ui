using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Shared;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStream
{
    /// <summary>
    /// The funding stream page view model.
    /// </summary>
    public abstract class FundingStreamPageViewModel : AdminPageBaseViewModel
    {
        /// <summary>
        /// Gets the breadcrumb for the funding streams list page.
        /// </summary>
        /// <param name="isCurrentPage">A boolean indicating whether or not this is the current page.</param>
        /// <returns>The breadcrumb for the funding streams list page.</returns>
        protected BreadCrumbViewModel SettingsListBreadCrumb(bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "View Your Funding settings",
                        RouteName = ViewYourFundingConstants.RouteName_AdminSettingsHome
                    }
                };

        /// <summary>
        /// Gets the breadcrumb for the funding stream settings list page.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream ID.</param>
        /// <param name="fundingStreamName">The funding stream name.</param>
        /// <param name="isCurrentPage">A boolean indicating whether or not this is the current page.</param>
        /// <returns>The breadcrumb for the funding stream settings list page.</returns>
        protected BreadCrumbViewModel SettingsFundingStreamBreadCrumb(
            int? fundingStreamId,
            string fundingStreamName,
            bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $"{fundingStreamName} settings",
                        RouteName = ViewYourFundingConstants.RouteName_AdminSettingsFundingStream,
                        Parameters = new
                        {
                            FundingStreamId = fundingStreamId
                        }
                    }
                };

        /// <summary>
        /// Gets the breadcrumb for the funding stream action page.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream ID.</param>
        /// <param name="fundingStreamName">The funding stream name.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <param name="isCurrentPage">A boolean indicating whether or not this is the current page.</param>
        /// <returns>The breadcrumb for the funding stream settings action page.</returns>
        protected BreadCrumbViewModel FundingStreamActionModeBreadCrumb(
            int? fundingStreamId,
            string fundingStreamName,
            ActionMode actionMode,
            bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $"{actionMode} Funding Stream",
                        RouteName = ViewYourFundingConstants.RouteName_AdminFundingStreamAction,
                        Parameters = new
                        {
                            fundingStreamId,
                            actionMode
                        }
                    }
                };

        /// <summary>
        /// Gets the breadcrumb for the funding stream are you sure page.
        /// </summary>
        /// <param name="isCurrentPage">A boolean indicating whether or not this is the current page.</param>
        /// <returns>The breadcrumb for the funding stream are you sure page.</returns>
        protected BreadCrumbViewModel FundingStreamAreYouSureBreadCrumb(bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = " Are you sure?"
                    }
                };
    }
}
