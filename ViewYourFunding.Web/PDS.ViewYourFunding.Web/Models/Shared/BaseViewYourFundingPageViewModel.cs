using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Models.Shared
{
    /// <summary>
    /// The base class for all page view models in the View Your Funding area.
    /// </summary>
    public abstract class BaseViewYourFundingPageViewModel : Pds.Core.Web.Models.BasePageViewModel
    {
        #region Overriden Base Properties

        /// <summary>
        /// Gets the base path.
        /// </summary>
        public string BasePath => Startup.PathPrefix;

        /// <summary>
        /// Gets the logout link.
        /// </summary>
        public override string LogoutLink => Startup.PathPrefix + ViewYourFundingConstants.AdminSignOutLink;

        /// <summary>
        /// Gets the title to show in the header bar.
        /// </summary>
        public override string HeaderTitle => "View latest funding";

        /// <summary>
        /// Gets the link target for the header bar.
        /// </summary>
        public override string HeaderLink => UserIsAdmin ? $"{Startup.PathPrefix}{ViewYourFundingConstants.Route_AdminHome}" : $"{Startup.PathPrefix}{Startup.RouteStart}";

        /// <summary>
        /// Gets an additional css class to inject into the html `body` tag.
        /// </summary>
        public override string BodyClass => "viewyourfunding";

        /// <summary>
        /// Gets a value indicating whether whether or not to show the title in the header bar.
        /// </summary>
        public override bool ShowHeaderTitle => true;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the beta tag banner.
        /// </summary>
        public override bool ShowBetaTag => true;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the feedback link in the beta tag banner.
        /// </summary>
        public override bool ShowFeedbackLink => true;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the DfE banner.
        /// </summary>
        public override bool ShowDfeBanner => true;

        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems => new List<BreadCrumbViewModel>();

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => true;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the title in the page's main content section.
        /// </summary>
        public override bool ShowContentTitle => true;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the UKPRN and school in the page's main content section.
        /// </summary>
        public override bool ShowUkprnAndSchool => false;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the 'related' sections.
        /// </summary>
        public override bool ShowRelatedSections => false;

        /// <inheritdoc/>
        public override string MSClarityId => ComponentHelper.GetMSClarityId();

        #endregion


        /// <summary>
        /// Gets or sets a value indicating whether the current user has the sfs admin role.
        /// </summary>
        public bool UserIsAdmin { get; set; }


        #region BreadCrumbs

        /// <summary>
        /// Gets the standard home breadcrumb.
        /// </summary>
        public BreadCrumbViewModel MYESFHomeBreadCrumb =>
            new BreadCrumbViewModel
            {
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = "Home",
                    RouteName = ViewYourFundingConstants.RouteName_MYESFHome
                },
                ExplicitUrl = "/"
            };

        /// <summary>
        /// Gets the admin home breadcrumb.
        /// </summary>
        public BreadCrumbViewModel AdminHomeBreadCrumb => new BreadCrumbViewModel
        {
            Link = new MvcRouteLinkViewModel
            {
                LinkText = "Home",
                RouteName = ViewYourFundingConstants.RouteName_AdminHome
            }
        };

        /// <summary>
        /// Gets the admin manage allocation data home breadcrumb.
        /// </summary>
        public BreadCrumbViewModel AdminManageAllocationHomeBreadCrumb => new BreadCrumbViewModel
        {
            Link = new MvcRouteLinkViewModel
            {
                LinkText = "Home",
                RouteName = ViewYourFundingConstants.RouteName_AdminManageAllocationHome
            },
            ExplicitUrl = "/"
        };

        /// <summary>
        /// Gets the standard home breadcrumb.
        /// </summary>
        public BreadCrumbViewModel HomeBreadCrumb =>
            new BreadCrumbViewModel
            {
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = "Home",
                    RouteName = ViewYourFundingConstants.RouteName_AdminHome
                }
            };


        /// <summary>
        /// A generic method to get a current page breadcrumb with the given link text.
        /// </summary>
        /// <param name="linkText">The breadcrumb text.</param>
        /// <returns>A current page breadcrumb with the given link text.</returns>
        protected static BreadCrumbViewModel GenericCurrentPageBreadcrumb(string linkText) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = true,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = linkText
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the 'Choose how to view funding' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <returns>The breadcrumb view model for the 'Choose how to view funding' page.</returns>
        protected static BreadCrumbViewModel ViewingChoicePageBreadCrumb(bool isCurrentPage) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = ViewYourFundingConstants.PageTitle_ViewingChoice,
                    RouteName = ViewYourFundingConstants.RouteName_ViewingChoice
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the 'Select a funding type' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <returns>The breadcrumb view model for the 'Select a funding type' page.</returns>
        protected static BreadCrumbViewModel WhichAllocationPageBreadCrumb(bool isCurrentPage) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = ViewYourFundingConstants.PageTitle_WhichAllocation,
                    RouteName = ViewYourFundingConstants.RouteName_WhichAllocation
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the 'National Funding Allocations Download {FundingStreamCode}' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="fundingStreamCode">The code of the allocation being displayed.</param>
        /// <param name="fundingStreamName">The name of the allocation being displayed.</param>
        /// <param name="yearFrom">The start year of the allocation being displayed.</param>
        /// <param name="yearTo">The end year of the allocation being displayed.</param>
        /// <param name="canUseShortCode">Can use the funding stream short code.</param>
        /// <returns>The breadcrumb view model for the 'National Funding Allocations Download {FundingStreamCode}' page.</returns>
        protected static BreadCrumbViewModel NationalFundingAllocationPageBreadCrumb(
                bool isCurrentPage,
                string fundingStreamCode,
                string fundingStreamName,
                int yearFrom,
                int yearTo,
                bool canUseShortCode) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = string.Format(
                        fundingStreamCode.Equals(ViewYourFundingConstants.RouteName_NationalFundingAllocation)
                        ? ViewYourFundingConstants.BreadCrumbTextFormat_FundingStreamCode
                        : ViewYourFundingConstants.BreadCrumbTextFormat_FundingStreamName,
                        fundingStreamName,
                        yearFrom,
                        yearTo,
                        fundingStreamCode),
                    RouteName = ViewYourFundingConstants.RouteName_WhichAllocation,
                    Parameters = new { yearFrom, yearTo }
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the 'Find an organisation' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <returns>The breadcrumb view model for the 'Find an organisation' page.</returns>
        protected static BreadCrumbViewModel FindAnOrganisationPageBreadCrumb(bool isCurrentPage) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = ViewYourFundingConstants.PageTitle_FindAnOrganisation,
                    RouteName = ViewYourFundingConstants.RouteName_FindAnOrganisation
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the 'Provider details' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="providerLinkText">The provider link text.</param>
        /// <param name="organisationUkprn">The organisation Ukprn.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>The breadcrumb view model for the 'Provider Details' page.</returns>
        protected static BreadCrumbViewModel ProviderPageBreadCrumb(
            bool isCurrentPage,
            string providerLinkText,
            string organisationUkprn,
            string searchTerm = null) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = providerLinkText,
                    RouteName = ViewYourFundingConstants.RouteName_ProviderStatement,
                    Parameters = new { organisationUkprn, searchTerm }
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the 'allocation history' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="fundingStreamName">The funding stream name.</param>
        /// <param name="organisationUkprn">The organisation ukprn.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>The breadcrumb view model for the 'allocation history' page.</returns>
        protected static BreadCrumbViewModel ProviderAllocationHistoryPageBreadCrumb(
            bool isCurrentPage,
            string fundingStreamName,
            string organisationUkprn,
            string searchTerm = null) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = ViewYourFundingConstants.PageTitle_AllocationHistory,
                    RouteName = ViewYourFundingConstants.RouteName_ProviderHistory,
                    Parameters = new { fundingStreamName, organisationUkprn, searchTerm }
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the 'provider funding breakdown' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="linkText">The link text.</param>
        /// <returns>The breadcrumb view model for the 'provider funding breakdown' page.</returns>
        protected static BreadCrumbViewModel ProviderFundingBreakdownPageBreadCrumb(bool isCurrentPage, string linkText) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = linkText,
                    RouteName = ViewYourFundingConstants.RouteName_ProviderFundingBreakdown
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the 'Provider Results' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>The breadcrumb view model for the 'Provider results' page.</returns>
        protected static BreadCrumbViewModel ProviderResultsPageBreadCrumb(bool isCurrentPage, string searchTerm) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = ViewYourFundingConstants.PageTitle_SearchResults,
                    RouteName = ViewYourFundingConstants.RouteName_ProviderDidYouMean,
                    Parameters = new { searchTerm }
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the local authority 'did you mean' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="searchTerm">The search term that was used.</param>
        /// <returns>The breadcrumb view model for the local authority 'did you mean' page.</returns>
        protected static BreadCrumbViewModel LocalAuthorityDidYouMeanPageBreadCrumb(
            bool isCurrentPage,
            string searchTerm) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = ViewYourFundingConstants.PageTitle_SearchResults,
                    RouteName = ViewYourFundingConstants.RouteName_LocalAuthorityDidYouMean,
                    Parameters = new { searchTerm }
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the local authority statement page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="localAuthorityCode">The local authority code.</param>
        /// <param name="localAuthorityName">The local authority name.</param>
        /// <param name="searchTerm">The local authority search term.</param>
        /// <returns>The breadcrumb view model for the local authority statement page.</returns>
        protected static BreadCrumbViewModel LocalAuthorityStatementPageBreadCrumb(
            bool isCurrentPage,
            string localAuthorityCode,
            string localAuthorityName,
            string searchTerm = null) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = localAuthorityName,
                    RouteName = ViewYourFundingConstants.RouteName_LocalAuthorityStatement,
                    Parameters = new { localAuthorityCode, searchTerm }
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the local authority 'allocation history' pages.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="routeName">The route name to link to.</param>
        /// <param name="localAuthorityCode">The local authority code.</param>
        /// <param name="fundingStreamName">The funding stream name (path formatted e.g. dedicated-school-grant)..</param>
        /// <param name="searchTerm">The local authority search term.</param>
        /// <returns>The breadcrumb view model for the local authority 'allocation history' pages.</returns>
        protected static BreadCrumbViewModel LocalAuthorityAllocationHistoryPageBreadCrumb(
            bool isCurrentPage,
            string routeName,
            string localAuthorityCode,
            string fundingStreamName,
            string searchTerm = null) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = ViewYourFundingConstants.PageTitle_AllocationHistory,
                    RouteName = routeName,
                    Parameters = new
                    {
                        localAuthorityCode,
                        fundingStreamName = fundingStreamName?.ToUIPathComponent(),
                        searchTerm
                    }
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the 'Under construction' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <returns>The breadcrumb view model for the 'Under construction' page.</returns>
        protected static BreadCrumbViewModel UnderConstructionBreadCrumb(bool isCurrentPage) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = ViewYourFundingConstants.PageTitle_UnderConstruction,
                    RouteName = ViewYourFundingConstants.RouteName_UnderConstruction
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the 'Funding breakdown' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="yearFrom">The start year of the funding period being displayed.</param>
        /// <param name="yearTo">The end year of the funding period being displayed.</param>
        /// <param name="canShowAbbreviation">Can the funding stream code be shown?.</param>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingStreamName">The funding stream name (e.g. PE and sport).</param>
        /// <returns>The breadcrumb view model for the 'Funding breakdown' page.</returns>
        protected static BreadCrumbViewModel FundingBreakdownPageBreadCrumb(
            bool isCurrentPage,
            int yearFrom,
            int yearTo,
            bool canShowAbbreviation,
            string fundingStreamCode,
            string fundingStreamName) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = string.Format(
                    ViewYourFundingConstants.BreadcrumbFormat_FundingBreakdown(canShowAbbreviation, fundingStreamCode, fundingStreamName),
                    yearFrom,
                    yearTo),
                    RouteName = ViewYourFundingConstants.RouteName_LocalAuthorityFundingBreakdown,
                    Parameters = new { yearFrom, yearTo }
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the 'Funding breakdown' page.
        /// </summary>
        /// <param name="fundingStreamName">The funding stream name.</param>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <param name="yearFrom">The start year of the funding period being displayed.</param>
        /// <param name="yearTo">The end year of the funding period being displayed.</param>
        /// <returns>The breadcrumb view model for the 'Funding breakdown' page.</returns>
        protected static BreadCrumbViewModel LocalAuthorityBreakdownPageBreadCrumb(string fundingStreamName, bool isCurrentPage, int yearFrom, int yearTo) =>
            new BreadCrumbViewModel
            {
                IsCurrentPage = isCurrentPage,
                Link = new MvcRouteLinkViewModel
                {
                    LinkText = string.Format(ViewYourFundingConstants.PageTitleFormat_Common(fundingStreamName), yearFrom, yearTo),
                    RouteName = ViewYourFundingConstants.RouteName_LocalAuthorityFundingBreakdown,
                    Parameters = new { yearFrom, yearTo }
                }
            };

        /// <summary>
        /// Get the breadcrumb view model for the 'Recoupment Summary' page.
        /// </summary>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <returns>The breadcrumb view model for the 'Funding breakdown' page.</returns>
        protected static BreadCrumbViewModel LocalAuthorityRecoupmentSummaryPageBreadCrumb(bool isCurrentPage) => new BreadCrumbViewModel
        {
            IsCurrentPage = isCurrentPage,
            Link = new MvcRouteLinkViewModel
            {
                LinkText = LoggedInConstants.PageTitle_RecoupmentReports,
                RouteName = LoggedInConstants.RouteName_LocalAuthorityRecoupmentSummary
            }
        };

        /// <summary>
        /// Get the breadcrumb view model for the 'Recoupment Detail' page.
        /// </summary>
        /// <param name="fundingStreamName">The funding stream name.</param>
        /// <param name="ukprn">The ukprn.</param>
        /// <param name="publishedDate">The published date.</param>
        /// <param name="yearFrom">The start year of the funding period being displayed.</param>
        /// <param name="yearTo">The end year of the funding period being displayed.</param>
        /// <param name="isCurrentPage">Whether or not it is the current page.</param>
        /// <returns>The breadcrumb view model for the 'Recoupment Detail' page.</returns>
        protected static BreadCrumbViewModel LocalAuthorityRecoupmentDetailPageBreadCrumb(string fundingStreamName, string ukprn, string publishedDate, int yearFrom, int yearTo, bool isCurrentPage) => new BreadCrumbViewModel
        {
            IsCurrentPage = isCurrentPage,
            Link = new MvcRouteLinkViewModel
            {
                LinkText = string.Format(ViewYourFundingConstants.PageTitleFormat_Common(fundingStreamName), yearFrom, yearTo),
                RouteName = LoggedInConstants.RouteName_LocalAuthorityRecoupmentDetail,
                Parameters = new { ukprn, publishedDate, yearTo, yearFrom }
            }
        };

        #endregion
    }
}