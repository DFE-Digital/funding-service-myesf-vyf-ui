using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The ProviderPSGAllocationHistoryPageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class ProviderPSGAllocationHistoryPageTests : FundingStreamRegressionTestBase
    {
        /// <summary>
        /// The multiple results search term.
        /// </summary>
        private const string MultipleResultsSearchTerm = "St Marys";

        /// <summary>
        /// ProviderPSGAllocationHistoryPage basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderPSGAllocationHistoryPage_BasicLayout()
        {
            // Arrange Act
            ProviderPSGAllocationHistoryPage.NavigateToPage();

            // Assert
            ProviderPSGAllocationHistoryPage.EnsureCurrentProviderPage();
            ProviderPSGAllocationHistoryPage.EnsureBreadcrumbs();
            ProviderPSGAllocationHistoryPage.EnsureAllAllocationLinks();
            ProviderPSGAllocationHistoryPage.EnsureMainParagraph();
            ProviderPSGAllocationHistoryPage.EnsureAllYearHeadings();
        }

        /// <summary>
        /// ProviderPSGAllocationHistoryPage has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderPSGAllocationHistoryPage_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            ProviderPSGAllocationHistoryPage.NavigateToPage();

            // Assert
            ProviderPSGAllocationHistoryPage.EnsureAndClickBreadcrumb(ChooseHowToViewFundingLinkText);
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// ProviderPSGAllocationHistoryPage has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderPSGAllocationHistoryPage_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            ProviderPSGAllocationHistoryPage.NavigateToPage();

            // Assert
            ProviderPSGAllocationHistoryPage.EnsureAndClickBreadcrumb(ViewFundingAtOrganisationLinkText);
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// ProviderPSGAllocationHistoryPage has breadcrumb linking to provider details page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderPSGAllocationHistoryPage_HasBreadcrumbLinkingToProviderDetailsPage()
        {
            // Arrange Act
            ProviderPSGAllocationHistoryPage.NavigateToPage();

            // Assert
            ProviderPSGAllocationHistoryPage.EnsureAndClickBreadcrumb(ProviderDetailsPageMainProviderName);
            ProviderDetailPage.EnsureCurrentPage(ProviderDetailsPageMainProviderName);
        }

        /// <summary>
        /// ProviderPSGAllocationHistoryPage via did you mean has breadcrumb linking provider did you mean page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderPSGAllocationHistoryPage_ViaDidYouMean_HasBreadcrumbLinkingProviderDidYouMeanPage()
        {
            // Arrange Act
            ProviderPSGAllocationHistoryPage.NavigateToViaDidYouMeanPage(MultipleResultsSearchTerm);

            // Assert
            ProviderPSGAllocationHistoryPage.EnsureAndClickBreadcrumb(SearchResultsLinkText);
            ProviderResultsPage.EnsureBreadcrumbs();
            ProviderResultsPage.EnsureResultsText(MultipleResultsSearchTerm);
        }
    }
}