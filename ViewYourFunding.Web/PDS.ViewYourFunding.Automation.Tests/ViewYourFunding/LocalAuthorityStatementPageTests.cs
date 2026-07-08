using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The LocalAuthorityStatementPageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class LocalAuthorityStatementPageTests : FundingStreamRegressionTestBase
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
        /// LocalAuthorityStatementPage basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityStatementPage_BasicLayout()
        {
            // Arrange Act
            LocalAuthorityStatementPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureBreadcrumbs();
            LocalAuthorityStatementPage.EnsureAccordionSectionsCount(3);
            LocalAuthorityStatementPage.EnsureAlternativeFunding();
        }

        /// <summary>
        /// LocalAuthorityStatementPage ensure DSG section.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityStatementPage_EnsureDSGSection()
        {
            // Arrange Act
            LocalAuthorityStatementPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureActiveYearsDSGSections(DsgActiveYears, DsgNextPaymentDateFsParsed, DsgNoNextPaymentDateTexts);
        }

        /// <summary>
        /// LocalAuthorityStatementPage ensure PSG section.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityStatementPage_EnsurePSGSection()
        {
            // Arrange Act
            LocalAuthorityStatementPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsurePSGSection(PsgCurrentYearFrom, PsgCurrentYearTo, PsgNextPaymentDateMaintainedParsed, NoPsgNextPaymentDateText);
        }

        /// <summary>
        /// LocalAuthorityStatementPage when via did you mean has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityStatementPage_WhenViaDidYouMean_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            LocalAuthorityStatementPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureAndClickBreadcrumb("Choose how to view funding");
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityStatementPage when via did you mean has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityStatementPage_WhenViaDidYouMean_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            LocalAuthorityStatementPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureAndClickBreadcrumb("View funding at organisation level");
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityStatementPage when via did you mean has breadcrumb linking to search results page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityStatementPage_WhenViaDidYouMean_HasBreadcrumbLinkingToSearchResultsPage()
        {
            // Arrange Act
            LocalAuthorityStatementPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureAndClickBreadcrumb("Search results");
            LocalAuthorityDidYouMeanPage.EnsureCurrentPage();
            LocalAuthorityDidYouMeanPage.EnsureSearchResultText(MultipleResultsSearchTerm);
        }

        /// <summary>
        /// LocalAuthorityStatementPage when via exact search has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityStatementPage_WhenViaExactSearch_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            LocalAuthorityStatementPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureAndClickBreadcrumb("Choose how to view funding");
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityStatementPage when via exact search has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityStatementPage_WhenViaExactSearch_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            LocalAuthorityStatementPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureAndClickBreadcrumb("View funding at organisation level");
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityStatementPage when via exact search does not have breadcrumb linking to search results page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityStatementPage_WhenViaExactSearch_DoesNotHaveBreadcrumbLinkingToSearchResultsPage()
        {
            // Arrange Act
            LocalAuthorityStatementPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureBreadcrumbNotShown("Search results");
        }
    }
}