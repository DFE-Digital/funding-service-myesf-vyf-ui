using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Shared;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication
{
    /// <summary>
    /// Base class for the View Your Funding Settings pages' view models.
    /// </summary>
    public abstract class PublicationPageViewModel : AdminPageBaseViewModel
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
                        RouteName = ViewYourFundingConstants.RouteName_AdminSettingsHome
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
                        RouteName = ViewYourFundingConstants.RouteName_AdminSettingsFundingStream,
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
                        RouteName = ViewYourFundingConstants.RouteName_AdminSettingsFundingStream,
                        Parameters = new { FundingStreamId = fundingStreamId }
                    }
                };

        /// <summary>
        /// Gets the breadcrumb for the funding stream action page.
        /// </summary>
        /// <param name="publicationId">The publication identifier.</param>
        /// <param name="fundingStreamId">The funding stream ID.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <param name="isCurrentPage">A bool indicating whether or not this is the current page.</param>
        /// <returns>The breadcrumb for the funding stream settings action page.</returns>
        protected BreadCrumbViewModel SettingsActionModeBreadCrumb(
            int? publicationId,
            int fundingStreamId,
            ActionMode actionMode,
            bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $"{actionMode} Publication",
                        RouteName = ViewYourFundingConstants.RouteName_AdminPublicationAction,
                        Parameters = new { publicationId = publicationId, FundingStreamId = fundingStreamId, actionMode = actionMode }
                    }
                };

        /// <summary>
        /// Gets the breadcrumb for the funding stream are you sure page.
        /// </summary>
        /// <param name="isCurrentPage">A bool indicating whether or not this is the current page.</param>
        /// <returns>The breadcrumb for the funding stream setting are you sure page.</returns>
        protected BreadCrumbViewModel SettingsAreYouSureBreadCrumb(bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $" Are you sure?"
                    }
                };
    }
}