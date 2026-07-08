using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The ProviderResultsPageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class ProviderResultsPageTests : FundingStreamRegressionTestBase
    {
        /// <summary>
        /// The multiple results search term.
        /// </summary>
        private const string MultipleResultsSearchTerm = "church of england primary school";

        /// <summary>
        /// ProviderResultsPage basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderResultsPage_BasicLayout()
        {
            // Arrange Act
            ProviderResultsPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);

            // Assert
            ProviderResultsPage.EnsureCurrentProviderPage();
            ProviderResultsPage.EnsureBreadcrumbs();
            ProviderResultsPage.EnsureResultsText(MultipleResultsSearchTerm);
        }

        /// <summary>
        /// ProviderResultsPage has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderResultsPage_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            ProviderResultsPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);

            // Assert
            ProviderResultsPage.EnsureAndClickBreadcrumb(ChooseHowToViewFundingLinkText);
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// ProviderResultsPage has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderResultsPage_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            ProviderResultsPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);

            // Assert
            ProviderResultsPage.EnsureAndClickBreadcrumb(ViewFundingAtOrganisationLinkText);
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// ProviderResultsPage ensure search results.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderResultsPage_EnsureSearchResults()
        {
            // Arrange Act
            ProviderResultsPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);

            // Assert
            ProviderResultsPage.EnsureCurrentProviderPage();
            ProviderResultsPage.EnsureSearchResultsReturned();
        }
    }
}