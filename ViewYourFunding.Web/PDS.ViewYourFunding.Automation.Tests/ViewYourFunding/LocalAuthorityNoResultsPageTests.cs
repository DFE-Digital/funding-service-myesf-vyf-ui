using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The LocalAuthorityNoResultsPageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class LocalAuthorityNoResultsPageTests : FundingStreamRegressionTestBase
    {
        /// <summary>
        /// The no results search term.
        /// </summary>
        private const string NoResultsSearchTerm = "test search term no match";

        /// <summary>
        /// LocalAuthorityNoResultsPage basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityNoResultsPage_BasicLayout()
        {
            // Arrange
            LocalAuthorityNoResultsPage.NavigateToPageUsingSearchTerm(NoResultsSearchTerm);

            // Assert
            LocalAuthorityNoResultsPage.EnsureCurrentPage();
            LocalAuthorityNoResultsPage.EnsureBreadcrumbs();
            LocalAuthorityNoResultsPage.EnsureNoResultsText(NoResultsSearchTerm);
        }

        /// <summary>
        /// LocalAuthorityNoResultsPage basic layout where special charcter entered.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityNoResultsPage_BasicLayout_WhereSpecialCharcterEntered()
        {
            // Arrange
            LocalAuthorityNoResultsPage.NavigateToPageUsingSearchTerm("$");

            // Assert
            LocalAuthorityNoResultsPage.EnsureCurrentPage();
            LocalAuthorityNoResultsPage.EnsureBreadcrumbs();
            LocalAuthorityNoResultsPage.EnsureNoResultsText("$");
        }

        /// <summary>
        /// LocalAuthorityNoResultsPage has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityNoResultsPage_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            LocalAuthorityNoResultsPage.NavigateToPageUsingSearchTerm(NoResultsSearchTerm);

            // Assert
            LocalAuthorityNoResultsPage.EnsureCurrentPage();
            LocalAuthorityNoResultsPage.EnsureAndClickBreadcrumb("Choose how to view funding");
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityNoResultsPage has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityNoResultsPage_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            LocalAuthorityNoResultsPage.NavigateToPageUsingSearchTerm(NoResultsSearchTerm);

            // Assert
            LocalAuthorityNoResultsPage.EnsureCurrentPage();
            LocalAuthorityNoResultsPage.EnsureAndClickBreadcrumb("View funding at organisation level");
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityNoResultsPage has working link to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityNoResultsPage_HasWorkingLinkToFindAnOrganisationPage()
        {
            // Arrange
            LocalAuthorityNoResultsPage.NavigateToPageUsingSearchTerm(NoResultsSearchTerm);

            // Assert
            LocalAuthorityNoResultsPage.EnsureCurrentPage();
            LocalAuthorityNoResultsPage.EnsureAndClickFindAnOrganisationLink();
            FindAnOrganisationPage.EnsureCurrentPage();
        }
    }
}