using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The ProviderNoResultsPageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class ProviderNoResultsPageTests : FundingStreamRegressionTestBase
    {
        /// <summary>
        /// The no results search term.
        /// </summary>
        private const string NoResultsSearchTerm = nameof(NoResultsSearchTerm);

        /// <summary>
        /// ProviderNoResultsPage basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderNoResultsPage_BasicLayout()
        {
            // Arrange Act
            ProviderNoResultsPage.NavigateToPageUsingSearchTerm(NoResultsSearchTerm);

            // Assert
            ProviderNoResultsPage.EnsureCurrentNoReultsPage();
            ProviderNoResultsPage.EnsureBreadcrumbs();
            ProviderNoResultsPage.EnsureFindAnOrganisationLink();
            ProviderNoResultsPage.EnsureNoResultsText(NoResultsSearchTerm);
        }

        /// <summary>
        /// ProviderNoResultsPage basic layout where special character entered.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderNoResultsPage_BasicLayout_WhereSpecialCharacterEntered()
        {
            // Arrange Act
            ProviderNoResultsPage.NavigateToPageUsingSearchTerm("$");

            // Assert
            ProviderNoResultsPage.EnsureCurrentNoReultsPage();
            ProviderNoResultsPage.EnsureBreadcrumbs();
            ProviderNoResultsPage.EnsureFindAnOrganisationLink();
            ProviderNoResultsPage.EnsureNoResultsText("$");
        }

        /// <summary>
        /// ProviderNoResultsPage has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderNoResultsPage_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            ProviderNoResultsPage.NavigateToPageUsingSearchTerm(NoResultsSearchTerm);

            // Assert
            ProviderNoResultsPage.EnsureAndClickBreadcrumb("Choose how to view funding");
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// ProviderNoResultsPage has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderNoResultsPage_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            ProviderNoResultsPage.NavigateToPageUsingSearchTerm(NoResultsSearchTerm);

            // Assert
            ProviderNoResultsPage.EnsureAndClickBreadcrumb("View funding at organisation level");
            FindAnOrganisationPage.EnsureCurrentPage();
        }
    }
}