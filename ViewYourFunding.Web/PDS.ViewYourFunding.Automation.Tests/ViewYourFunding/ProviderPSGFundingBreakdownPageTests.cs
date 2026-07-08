using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The ProviderPSGFundingBreakdownPageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class ProviderPSGFundingBreakdownPageTests : FundingStreamRegressionTestBase
    {
        /// <summary>
        /// The multiple results search term.
        /// </summary>
        private const string MultipleResultsSearchTerm = "St Marys";

        /// <summary>
        /// ProviderPSGFundingBreakdownPage basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderPSGFundingBreakdownPage_BasicLayout()
        {
            // Arrange Act
            ProviderPSGFundingBreakdownPage.NavigateToPage();

            // Assert
            ProviderPSGFundingBreakdownPage.EnsureCurrentProviderPage();
            ProviderPSGFundingBreakdownPage.EnsureBreadcrumbs();
            ProviderPSGFundingBreakdownPage.EnsureAllocationHistoryLink();
            ProviderPSGFundingBreakdownPage.EnsureDownloadSpreadsheetLink();
            ProviderPSGFundingBreakdownPage.EnsureFundingTableRowColumns();
            ProviderPSGFundingBreakdownPage.EnsureTotalFundingSummary();
            ProviderPSGFundingBreakdownPage.EnsureGovUkLinks();
        }

        /// <summary>
        /// ProviderPSGFundingBreakdownPage has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderPSGFundingBreakdownPage_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            ProviderPSGFundingBreakdownPage.NavigateToPage();

            // Assert
            ProviderPSGFundingBreakdownPage.EnsureAndClickBreadcrumb(ChooseHowToViewFundingLinkText);
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// ProviderPSGFundingBreakdownPage has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderPSGFundingBreakdownPage_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            ProviderPSGFundingBreakdownPage.NavigateToPage();

            // Assert
            ProviderPSGFundingBreakdownPage.EnsureAndClickBreadcrumb(ViewFundingAtOrganisationLinkText);
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// ProviderPSGFundingBreakdownPage has breadcrumb linking to provider details page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderPSGFundingBreakdownPage_HasBreadcrumbLinkingToProviderDetailsPage()
        {
            // Arrange Act
            ProviderPSGFundingBreakdownPage.NavigateToPage();

            // Assert
            ProviderPSGFundingBreakdownPage.EnsureAndClickBreadcrumb(ProviderDetailsPageMainProviderName);
            ProviderDetailPage.EnsureCurrentPage(ProviderDetailsPageMainProviderName);
        }

        /// <summary>
        /// ProviderPSGFundingBreakdownPage via did you mean has breadcrumb linking provider did you mean page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderPSGFundingBreakdownPage_ViaDidYouMean_HasBreadcrumbLinkingProviderDidYouMeanPage()
        {
            // Arrange Act
            ProviderPSGFundingBreakdownPage.NavigateToViaDidYouMeanPage(MultipleResultsSearchTerm);

            // Assert
            ProviderPSGFundingBreakdownPage.EnsureAndClickBreadcrumb(SearchResultsLinkText);
            ProviderResultsPage.EnsureBreadcrumbs();
            ProviderResultsPage.EnsureResultsText(MultipleResultsSearchTerm);
        }
    }
}