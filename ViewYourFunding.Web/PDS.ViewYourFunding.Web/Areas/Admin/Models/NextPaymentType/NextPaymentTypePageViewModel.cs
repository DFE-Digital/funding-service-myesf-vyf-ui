using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Shared;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPaymentType
{
    /// <summary>
    /// Class for the Next Payment types pages view models.
    /// </summary>
    public class NextPaymentTypePageViewModel : AdminPageBaseViewModel
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
                        RouteName = ViewYourFundingConstants.RouteName_AdminSettingsHome,
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
        /// Gets the breadcrumb for the next payment types list page.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream ID.</param>
        /// <param name="fundingStreamName">The funding stream name.</param>
        /// <param name="isCurrentPage">A bool indicating whether or not this is the current page.</param>
        /// <returns>The breadcrumb for the next payment types list page.</returns>
        protected BreadCrumbViewModel NextPaymentTypesBreadCrumb(
            int fundingStreamId,
            string fundingStreamName,
            bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $" {fundingStreamName} Next Payment Types",
                        RouteName = ViewYourFundingConstants.RouteName_AdminNextPaymentTypeIndex,
                        Parameters = new { FundingStreamId = fundingStreamId }
                    }
                };

        /// <summary>
        /// Gets the breadcrumb for the funding stream action page.
        /// </summary>
        /// <param name="nextPaymentTypeId">The next payment type identifier.</param>
        /// <param name="fundingStreamId">The funding stream ID.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <param name="isCurrentPage">A bool indicating whether or not this is the current page.</param>
        /// <returns>The breadcrumb for the funding stream settings action page.</returns>
        protected BreadCrumbViewModel NextPaymentTypeActionModeBreadCrumb(
            int? nextPaymentTypeId,
            int fundingStreamId,
            ActionMode actionMode,
            bool isCurrentPage = false) =>
                new BreadCrumbViewModel
                {
                    IsCurrentPage = isCurrentPage,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = $"{actionMode} Next Payment Type",
                        RouteName = ViewYourFundingConstants.RouteName_AdminNextPaymentTypeAction,
                        Parameters = new { nextPaymentTypeId = nextPaymentTypeId, FundingStreamId = fundingStreamId, actionMode = actionMode }
                    }
                };

        /// <summary>
        /// Gets the breadcrumb for the funding stream are you sure page.
        /// </summary>
        /// <param name="isCurrentPage">A bool indicating whether or not this is the current page.</param>
        /// <returns>The breadcrumb for the funding stream are you sure page.</returns>
        protected BreadCrumbViewModel NextPaymentTypeAreYouSureBreadCrumb(bool isCurrentPage = false) =>
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
