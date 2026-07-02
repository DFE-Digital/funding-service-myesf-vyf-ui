using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Shared;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting
{
    /// <summary>
    /// Base class for the View Your Funding Settings pages' view models.
    /// </summary>
    public abstract class FundingStreamSettingsPageViewModel : AdminPageBaseViewModel
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
                    LinkText = "Funding stream settings",
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
        /// <param name="actionMode">the action mode.</param>
        /// <param name="isCurrentPage">if set to <c>true</c> if it is the current page.</param>
        /// <returns>The breadcrumb for the settings edit page.</returns>
        protected BreadCrumbViewModel SettingsFundingStreamEditBreadCrumb(
            int fundingStreamId,
            string fundingStreamName,
            string actionMode,
            bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $" {actionMode} {fundingStreamName} settings",
                        RouteName = ViewYourFundingConstants.RouteName_AdminSettingsFundingStream,
                        Parameters = new { FundingStreamId = fundingStreamId }
                    }
                };

        /// <summary>
        /// Gets the breadcrumb for the funding stream setting  edit page.
        /// </summary>
        /// <param name="settingsDescription">Name of the funding stream setting .</param>
        /// <param name="isCurrentPage">if set to <c>true</c> if it is the current page.</param>
        /// <returns>The breadcrumb for the settings edit page.</returns>
        protected BreadCrumbViewModel SettingsFundingStreamEditBreadCrumb(
            string settingsDescription,
            bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $" Edit {settingsDescription} settings",
                    }
                };

        /// <summary>
        /// Gets the breadcrumb for the funding stream setting delete page.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> if it is the current page.</param>
        /// <returns>The breadcrumb for the settings delete page.</returns>
        protected BreadCrumbViewModel SettingsFundingStreamDeleteBreadCrumb(
            bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Delete setting",
                    }
                };
    }
}