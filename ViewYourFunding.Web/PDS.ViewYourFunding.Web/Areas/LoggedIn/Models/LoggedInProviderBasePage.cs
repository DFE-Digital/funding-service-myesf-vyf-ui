using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using PDS.ViewYourFunding.Web.Models.Shared;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Models
{
    /// <summary>
    /// The Logged In Provider base page view model.
    /// </summary>
    /// <seealso cref="Web.Models.Shared.BasePageViewModel" />
    public class LoggedInProviderBasePage : BaseViewYourFundingPageViewModel
    {
        /// <inheritdoc/>
        public override string HeaderLink => HeaderTitle == LoggedInConstants.HeaderTitle_StandardMYESFHeader ? "/" : $"{Startup.PathPrefix}{Startup.RouteStart}";

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string HeaderTitle => LoggedInConstants.HeaderTitle_StandardMYESFHeader;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the DfE banner.
        /// </summary>
        public override bool ShowDfeBanner => false;

        /// <summary>
        /// Get the breadcrumb view model for the 'Allocation statement' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="viaChoicePage">Whether we entered via the choose allocation page (e.g. pre or post 16).</param>
        /// <param name="orgName">The organisation name.</param>
        /// <param name="orgUkprn">The organisation UKPRN.</param>
        /// <param name="linkRouteName">The route name for the breadcrumb link.</param>
        /// <returns>The breadcrumb view model for the 'Choose how to view funding' page.</returns>
        protected static BreadCrumbViewModel AllocationStatementsBreadcrumb(
            bool isCurrentPage,
            bool viaChoicePage,
            string orgName = null,
            string orgUkprn = null,
            string linkRouteName = null)
        {
            var linkText = string.IsNullOrEmpty(orgName) ? LoggedInConstants.PageTitle_AllocationStatements
                                                         : $"{LoggedInConstants.PageTitle_AllocationStatements}: {orgName} (URN: {orgUkprn})";

            var linkRoute = linkRouteName ?? LoggedInConstants.RouteName_ProviderStatement;

            return new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = linkText,
                    RouteName = linkRoute,
                    Parameters = viaChoicePage ? new { viaChoicePage } : null
                }
            };
        }

        /// <summary>
        /// Get the breadcrumb view model for the 'Recoupment reports' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="viaChoicePage">Whether we entered via the choose allocation page (e.g. pre or post 16).</param>
        /// <param name="orgName">The organisation name.</param>
        /// <param name="orgUkprn">The organisation UKPRN.</param>
        /// <param name="linkRouteName">The route name for the breadcrumb link.</param>
        /// <returns>The breadcrumb view model for the 'Choose how to view funding' page.</returns>
        protected static BreadCrumbViewModel RecoupmentReportsBreadcrumb(
            bool isCurrentPage,
            bool viaChoicePage,
            string orgName = null,
            string orgUkprn = null,
            string linkRouteName = null)
        {
            var linkText = string.IsNullOrEmpty(orgName) ? LoggedInConstants.PageTitle_RecoupmentReports
                                                         : $"{LoggedInConstants.PageTitle_RecoupmentReports}: {orgName} (URN: {orgUkprn})";

            var linkRoute = linkRouteName ?? LoggedInConstants.RouteName_LocalAuthorityRecoupmentSummary;

            return new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = linkText,
                    RouteName = linkRoute,
                    Parameters = viaChoicePage ? new { viaChoicePage } : null
                }
            };
        }

        /// <summary>
        /// Get the breadcrumb view model for the 'Variance Selection' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="ukprn">The ukprn.</param>
        /// <param name="fundingStreamNamePathPart">the funding stream name path part.</param>
        /// <param name="publishedDate">The published date.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <returns>The breadcrumb view model for the 'Variance Selection' page.</returns>
        [Obsolete("Variance Selection Breadcrumb is hidden for GAG digital MVS Go Live.")]
        protected static BreadCrumbViewModel VarianceSelectionBreadcrumb(
            bool isCurrentPage, string ukprn, string fundingStreamNamePathPart, string publishedDate, int yearFrom, int yearTo) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = LoggedInConstants.PageTitle_VarianceSelection,
                    RouteName = LoggedInConstants.RouteName_VarianceSelection,
                    Parameters = new { ukprn, fundingStreamNamePathPart, publishedDate, yearFrom, yearTo }
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the 'Provider funding breakdown' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="title">The title.</param>
        /// <returns>The breadcrumb view model for the 'Choose how to view funding' page.</returns>
        protected static BreadCrumbViewModel ProviderFundingBreakDownBreadcrumb(
            bool isCurrentPage,
            string title) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = title,
                    RouteName = LoggedInConstants.RouteName_ProviderFundingBreakdown
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the Provider History page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="fundingStreamNamePathPart">Funding stream name.</param>
        /// <param name="ukprn">The provider UKPRN.</param>
        /// <param name="viaChoicePage">Whether we entered via the choose allocation page (e.g. pre or post 16).</param>
        /// <returns>The breadcrumb view model for the 'Choose how to view funding' page.</returns>
        protected static BreadCrumbViewModel ProviderAllocationHistoryBreadcrumb(bool isCurrentPage, string fundingStreamNamePathPart, string ukprn, bool viaChoicePage) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = LoggedInConstants.PageTitle_ProviderHistory,
                    RouteName = LoggedInConstants.RouteName_ProviderHistory,
                    Parameters = new { fundingStreamNamePathPart, ukprn, viaChoicePage }
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the Local authority History page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="fundingStreamNamePathPart">Funding stream name.</param>
        /// <param name="ukprn">The provider UKPRN.</param>
        /// <returns>The breadcrumb view model for the 'Choose how to view funding' page.</returns>
        protected static BreadCrumbViewModel LocalAuthorityAllocationHistoryBreadcrumb(bool isCurrentPage, string fundingStreamNamePathPart, string ukprn) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = LoggedInConstants.PageTitle_ProviderHistory,
                    RouteName = LoggedInConstants.RouteName_LocalAuthorityHistory,
                    Parameters = new { fundingStreamNamePathPart, ukprn }
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the la recoupment History page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="fundingStreamNamePathPart">Funding stream name.</param>
        /// <param name="ukprn">The provider UKPRN.</param>
        /// <returns>The breadcrumb view model for the 'Choose how to view funding' page.</returns>
        protected static BreadCrumbViewModel LARecoupmentHistoryBreadcrumb(bool isCurrentPage, string fundingStreamNamePathPart, string ukprn) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = LoggedInConstants.PageTitle_RecoupmentHistory,
                    RouteName = LoggedInConstants.RouteName_LARecoupmentHistory,
                    Parameters = new { fundingStreamNamePathPart, ukprn }
                }
            };


        /// <summary>
        /// Get the breadcrumb view model for the 'Provider funding breakdown' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="title">The title.</param>
        /// <returns>The breadcrumb view model for the 'Choose how to view funding' page.</returns>
        protected static BreadCrumbViewModel LocalAuthorityFundingBreakDownBreadcrumb(
            bool isCurrentPage,
            string title) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = title,
                    RouteName = LoggedInConstants.RouteName_LocalAuthorityFundingBreakdown
                }
            };

        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        /// <param name="summaryIsCurrentPage">If the summary page is the current page.</param>
        /// <returns>A list of breadcrumbs.</returns>
        protected IList<BreadCrumbViewModel> SecondLevelBaseBreadCrumbItems(bool summaryIsCurrentPage)
        {
            var items = new List<BreadCrumbViewModel>
            {
                LoggedInHomePageBreadCrumb()
            };

            if (ViaChoicePage)
            {
                items.Add(new BreadCrumbViewModel
                {
                    IsCurrentPage = false,
                    ExplicitUrl = ChoicePageLink,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Choose a statement type: Pre-16"
                    }
                });
            }

            if (FromMatStatementsPage)
            {
                items.Add(AllocationStatementsBreadcrumb(summaryIsCurrentPage, ViaChoicePage, OrganisationName, ProviderUrn, LoggedInConstants.RouteName_MultipleAcademyTrustStatement));
            }
            else
            {
                items.Add(AllocationStatementsBreadcrumb(summaryIsCurrentPage, ViaChoicePage));
            }

            return items;
        }

        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        /// <param name="summaryIsCurrentPage">If the summary page is the current page.</param>
        /// <returns>A list of breadcrumbs.</returns>
        protected IList<BreadCrumbViewModel> RecopementHistorySecondLevelBaseBreadCrumbItems(bool summaryIsCurrentPage)
        {
            var items = new List<BreadCrumbViewModel>
            {
                LoggedInHomePageBreadCrumb()
            };

            if (ViaChoicePage)
            {
                items.Add(new BreadCrumbViewModel
                {
                    IsCurrentPage = false,
                    ExplicitUrl = ChoicePageLink,
                    Link = new MvcRouteLinkViewModel
                    {
                        LinkText = "Choose a statement type: Pre-16"
                    }
                });
            }

            if (FromMatStatementsPage)
            {
                items.Add(RecoupmentReportsBreadcrumb(summaryIsCurrentPage, ViaChoicePage, OrganisationName, ProviderUrn, LoggedInConstants.RouteName_MultipleAcademyTrustStatement));
            }
            else
            {
                items.Add(RecoupmentReportsBreadcrumb(summaryIsCurrentPage, ViaChoicePage));
            }

            return items;
        }

        /// <summary>
        /// Get the breadcrumb view model for the 'Home' page.
        /// </summary>
        /// <returns>The breadcrumb view model for the 'Home' page.</returns>
        protected BreadCrumbViewModel LoggedInHomePageBreadCrumb() =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = false,
                ExplicitUrl = HomeLink,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = "Home"
                }
            };

        /// <summary>
        /// Gets or sets the 'Home' link.
        /// </summary>
        public string HomeLink { get; set; }

        /// <summary>
        /// Gets or sets the 'Choice Page' (the breadcrumb that lets you go back to pick pre or post 16 allocations) link.
        /// </summary>
        public string ChoicePageLink { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user came via the allocation choice page (pre or post 16).
        /// </summary>
        public bool ViaChoicePage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we have arrived here from the MAT statement page.
        /// </summary>
        public bool FromMatStatementsPage { get; set; }

        /// <summary>
        /// Gets or sets the provider's URN.
        /// </summary>
        public string ProviderUrn { get; set; }

        /// <summary>
        /// Gets or sets the organisation's name.
        /// </summary>
        public string OrganisationName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display the unauthorised access error message.
        /// </summary>
        public bool DisplayUnauthorisedAccessErrorMessage { get; set; }
    }
}