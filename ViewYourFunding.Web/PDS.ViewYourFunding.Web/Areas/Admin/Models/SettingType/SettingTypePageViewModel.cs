using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Shared;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType
{
    /// <summary>
    /// Class for the setting types pages view models.
    /// </summary>
    public class SettingTypePageViewModel : AdminPageBaseViewModel
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
                        LinkText = "Setting Types",
                        RouteName = ViewYourFundingConstants.RouteName_AdminSettingTypesHome,
                    }
                };

        /// <summary>
        /// Gets the breadcrumb for the setting types list page.
        /// </summary>
        /// <param name="isCurrentPage">A boolean indicating whether or not this is the current page.</param>
        /// <returns>The breadcrumb for the setting types list page.</returns>
        protected BreadCrumbViewModel SettingTypesBreadCrumb(bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $"Setting Types",
                        RouteName = ViewYourFundingConstants.RouteName_AdminNextPaymentTypeIndex,
                    }
                };

        /// <summary>
        /// Gets the breadcrumb for the funding stream action page.
        /// </summary>
        /// <param name="settingTypeId">The setting type identifier.</param>
        /// <param name="isSettingTypeInUse">Is setting type in use.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <param name="isCurrentPage">A boolean indicating whether or not this is the current page.</param>
        /// <returns>The breadcrumb for the funding stream settings action page.</returns>
        protected BreadCrumbViewModel SettingTypeActionModeBreadCrumb(
            int? settingTypeId,
            bool? isSettingTypeInUse,
            ActionMode actionMode,
            bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $"{actionMode} Setting Type",
                        RouteName = ViewYourFundingConstants.RouteName_AdminSettingTypeAction,
                        Parameters = new { SettingTypeId = settingTypeId, IsSettingTypeInUse = isSettingTypeInUse, actionMode = actionMode }
                    }
                };

        /// <summary>
        /// Gets the breadcrumb for the funding stream are you sure page.
        /// </summary>
        /// <param name="isCurrentPage">A boolean indicating whether or not this is the current page.</param>
        /// <returns>The breadcrumb for the funding stream are you sure page.</returns>
        protected BreadCrumbViewModel SettingTypeAreYouSureBreadCrumb(bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $" Are you sure?",
                    }
                };
    }
}
