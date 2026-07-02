using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Shared;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement
{
    /// <summary>
    /// Base class for Layout Management admin pages' view models.
    /// </summary>
    public abstract class LayoutManagementPageViewModel : AdminPageBaseViewModel
    {
        /// <summary>
        /// The layout management home page bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">Set to true if it is the current page.</param>
        /// <returns>The breadcrumb for the layout management home page.</returns>
        protected BreadCrumbViewModel LayoutManagementHomeBreadCrumb(bool isCurrentPage = false) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = "Layout Management",
                    RouteName = ViewYourFundingConstants.RouteName_AdminLayoutManagementHome,
                }
            };

        /// <summary>
        /// The import layout page bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">Set to true if it is the current page.</param>
        /// <returns>The breadcrumb for the import layout page.</returns>
        protected BreadCrumbViewModel ImportLayoutBreadCrumb(bool isCurrentPage = false) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = "Import layout",
                    RouteName = ViewYourFundingConstants.RouteName_AdminLayoutImport,
                }
            };

        /// <summary>
        /// The upload layout page bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">Set to true if it is the current page.</param>
        /// <returns>The breadcrumb for the upload layout page.</returns>
        protected BreadCrumbViewModel UploadLayoutBreadCrumb(bool isCurrentPage = false) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = "Upload layout",
                    RouteName = ViewYourFundingConstants.RouteName_AdminLayoutFileImport,
                }
            };

        /// <summary>
        /// The delete layout bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">Set to true if it is the current page.</param>
        /// <returns>The breadcrumb for the delete layout page.</returns>
        protected BreadCrumbViewModel DeleteLayoutBreadCrumb(bool isCurrentPage = false) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = "Delete Layout",
                }
            };

        /// <summary>
        /// The preview layout bread crumb.
        /// </summary>
        /// <param name="isCurrentPage">Set to true if it is the current page.</param>
        /// <returns>The breadcrumb for the preview layout page.</returns>
        protected BreadCrumbViewModel PreviewLayoutBreadCrumb(bool isCurrentPage = false) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = "Preview Layout",
                }
            };
    }
}