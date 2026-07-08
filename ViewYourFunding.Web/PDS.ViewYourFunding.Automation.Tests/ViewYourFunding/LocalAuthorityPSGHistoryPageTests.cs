using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The LocalAuthorityPSGHistoryPageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class LocalAuthorityPSGHistoryPageTests : FundingStreamRegressionTestBase
    {
        /// <summary>
        /// The multiple results search term.
        /// </summary>
        public const string MultipleResultsSearchTerm = "ca";

        /// <summary>
        /// The local authority name.
        /// </summary>
        public const string LocalAuthorityName = "Camden";

        /// <summary>
        /// The local authority code.
        /// </summary>
        public const string LocalAuthorityCode = "202";

        /// <summary>
        /// LocalAuthorityPSGHistoryPage basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityPSGHistoryPage_BasicLayout()
        {
            // Arrange Act
            LocalAuthorityPSGHistoryPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityPSGHistoryPage.EnsureBreadcrumbs();
            LocalAuthorityPSGHistoryPage.EnsureMainParagraph(LocalAuthorityName);
        }

        /// <summary>
        /// LocalAuthorityPSGHistoryPage has data for current and previous three years.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityPSGHistoryPage_HasDataForCurrentAndPreviousThreeYears()
        {
            // Arrange Act
            LocalAuthorityPSGHistoryPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityPSGHistoryPage.EnsureAllYears(PsgCurrentYearFrom, PsgCurrentYearTo, LocalAuthorityCode);
        }

        /// <summary>
        /// LocalAuthorityPSGHistoryPage when via did you mean has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGHistoryPage_WhenViaDidYouMean_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            LocalAuthorityPSGHistoryPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityPSGHistoryPage.EnsureAndClickBreadcrumb("Choose how to view funding");
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityPSGHistoryPage when via did you mean has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGHistoryPage_WhenViaDidYouMean_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            LocalAuthorityPSGHistoryPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityPSGHistoryPage.EnsureAndClickBreadcrumb("View funding at organisation level");
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityPSGHistoryPage when via did you mean has breadcrumb linking to search results page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGHistoryPage_WhenViaDidYouMean_HasBreadcrumbLinkingToSearchResultsPage()
        {
            // Arrange Act
            LocalAuthorityPSGHistoryPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityPSGHistoryPage.EnsureAndClickBreadcrumb("Search results");
            LocalAuthorityDidYouMeanPage.EnsureCurrentPage();
            LocalAuthorityDidYouMeanPage.EnsureSearchResultText(MultipleResultsSearchTerm);
        }

        /// <summary>
        /// LocalAuthorityPSGHistoryPage when via did you mean has breadcrumb linking to statement page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGHistoryPage_WhenViaDidYouMean_HasBreadcrumbLinkingToStatementPage()
        {
            // Arrange Act
            LocalAuthorityPSGHistoryPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityPSGHistoryPage.EnsureAndClickBreadcrumb(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
        }

        /// <summary>
        /// LocalAuthorityPSGHistoryPage when via exact search has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGHistoryPage_WhenViaExactSearch_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            LocalAuthorityPSGHistoryPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityPSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityPSGHistoryPage.EnsureAndClickBreadcrumb("Choose how to view funding");
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityPSGHistoryPage when via exact search has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGHistoryPage_WhenViaExactSearch_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            LocalAuthorityPSGHistoryPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityPSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityPSGHistoryPage.EnsureAndClickBreadcrumb("View funding at organisation level");
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityPSGHistoryPage when via exact search does not have breadcrumb linking to search results page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGHistoryPage_WhenViaExactSearch_DoesNotHaveBreadcrumbLinkingToSearchResultsPage()
        {
            // Arrange Act
            LocalAuthorityPSGHistoryPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityPSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityPSGHistoryPage.EnsureBreadcrumbNotShown("Search results");
        }

        /// <summary>
        /// LocalAuthorityPSGHistoryPage when via exact search has breadcrumb linking to statement page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGHistoryPage_WhenViaExactSearch_HasBreadcrumbLinkingToStatementPage()
        {
            // Arrange Act
            LocalAuthorityPSGHistoryPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityPSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityPSGHistoryPage.EnsureAndClickBreadcrumb(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
        }

        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGHistoryPage_CanNavigateToLatestPublication_StatementPage()
        {
            // Arrange Act
            LocalAuthorityPSGHistoryPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityPSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityPSGHistoryPage.ClickLatestPublicationLink();
            LocalAuthorityStatementPage.EnsureAndClickBreadcrumb("Allocation history");
            LocalAuthorityPSGHistoryPage.EnsureMainParagraph(LocalAuthorityName);
        }
    }
}