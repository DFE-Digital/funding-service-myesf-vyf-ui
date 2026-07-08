using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The LocalAuthorityDSGHistoryPageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class LocalAuthorityDSGHistoryPageTests : FundingStreamRegressionTestBase
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
        /// LocalAuthorityDSGHistoryPage basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGHistoryPage_BasicLayout()
        {
            // Arrange Act
            LocalAuthorityDSGHistoryPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityDSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityDSGHistoryPage.EnsureBreadcrumbs();
            LocalAuthorityDSGHistoryPage.EnsureMainParagraph(LocalAuthorityName);
        }

        /// <summary>
        /// LocalAuthorityDSGHistoryPage has data for current and previous three years.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGHistoryPage_HasDataForCurrentAndPreviousThreeYears()
        {
            // Arrange Act
            LocalAuthorityDSGHistoryPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityDSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityDSGHistoryPage.EnsureAllYears(DsgCurrentYearFrom, DsgCurrentYearTo, LocalAuthorityCode);
        }

        /// <summary>
        /// LocalAuthorityDSGHistoryPage when via did you mean has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGHistoryPage_WhenViaDidYouMean_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            LocalAuthorityDSGHistoryPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityDSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityDSGHistoryPage.EnsureAndClickBreadcrumb("Choose how to view funding");
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityDSGHistoryPage when via did you mean has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGHistoryPage_WhenViaDidYouMean_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            LocalAuthorityDSGHistoryPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityDSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityDSGHistoryPage.EnsureAndClickBreadcrumb("View funding at organisation level");
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityDSGHistoryPage when via did you mean has breadcrumb linking to search results page.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGHistoryPage_WhenViaDidYouMean_HasBreadcrumbLinkingToSearchResultsPage()
        {
            // Arrange Act
            LocalAuthorityDSGHistoryPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityDSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityDSGHistoryPage.EnsureAndClickBreadcrumb("Search results");
            LocalAuthorityDidYouMeanPage.EnsureCurrentPage();
            LocalAuthorityDidYouMeanPage.EnsureSearchResultText(MultipleResultsSearchTerm);
        }

        /// <summary>
        /// LocalAuthorityDSGHistoryPage when via did you mean has breadcrumb linking to statement page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGHistoryPage_WhenViaDidYouMean_HasBreadcrumbLinkingToStatementPage()
        {
            // Arrange Act
            LocalAuthorityDSGHistoryPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityDSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityDSGHistoryPage.EnsureAndClickBreadcrumb(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
        }

        /// <summary>
        /// LocalAuthorityDSGHistoryPage when via exact search has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGHistoryPage_WhenViaExactSearch_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            LocalAuthorityDSGHistoryPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityDSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityDSGHistoryPage.EnsureAndClickBreadcrumb("Choose how to view funding");
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityDSGHistoryPage when via exact search has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGHistoryPage_WhenViaExactSearch_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            LocalAuthorityDSGHistoryPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityDSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityDSGHistoryPage.EnsureAndClickBreadcrumb("View funding at organisation level");
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityDSGHistoryPage when via exact search does not have breadcrumb linking to search results page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGHistoryPage_WhenViaExactSearch_DoesNotHaveBreadcrumbLinkingToSearchResultsPage()
        {
            // Arrange Act
            LocalAuthorityDSGHistoryPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityDSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityDSGHistoryPage.EnsureBreadcrumbNotShown("Search results");
        }

        /// <summary>
        /// LocalAuthorityDSGHistoryPage when via exact search has breadcrumb linking to statement page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGHistoryPage_WhenViaExactSearch_HasBreadcrumbLinkingToStatementPage()
        {
            // Arrange Act
            LocalAuthorityDSGHistoryPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityDSGHistoryPage.EnsureCurrentPage();
            LocalAuthorityDSGHistoryPage.EnsureAndClickBreadcrumb(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
        }
    }
}