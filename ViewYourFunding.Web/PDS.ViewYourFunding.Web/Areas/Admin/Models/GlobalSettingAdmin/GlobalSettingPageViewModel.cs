using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Shared;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.GlobalSettingAdmin
{
    /// <summary>
    /// Base class for Global Settings admin pages' view models.
    /// </summary>
    public abstract class GlobalSettingPageViewModel : AdminPageBaseViewModel
    {
        /// <summary>
        /// The global setting edit page bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for the global setting edit page.</returns>
        protected BreadCrumbViewModel GeneralSettingsListBreadCrumb(bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "General Settings List",
                        RouteName = ViewYourFundingConstants.RouteName_AdminGeneralSettingHome,
                    }
                };

        /// <summary>
        /// The global setting edit page bread crumb.
        /// </summary>
        /// <param name="globalSettingId">The global setting identifier.</param>
        /// <param name="globalSettingName">Name of the global setting.</param>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for the global setting edit page.</returns>
        protected BreadCrumbViewModel GeneralSettingEditBreadCrumb(
            int globalSettingId,
            string globalSettingName,
            bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $" Edit {globalSettingName}",
                        RouteName = ViewYourFundingConstants.RouteName_AdminGeneralSettingEdit,
                        Parameters = new { globalSettingId }
                    }
                };

        /// <summary>
        /// The global setting are you sure bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">if set to <c>true</c> [is current page].</param>
        /// <returns>The breadcrumb for the global setting are you sure.</returns>
        protected BreadCrumbViewModel GeneralSettingsAreYouSureBreadCrumb(bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Are you sure?",
                    }
                };
    }
}