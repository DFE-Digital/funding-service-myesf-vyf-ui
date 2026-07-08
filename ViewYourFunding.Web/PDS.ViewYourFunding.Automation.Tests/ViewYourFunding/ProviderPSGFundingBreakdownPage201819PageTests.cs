using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The ProviderPSGFundingBreakdownPage201819PageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class ProviderPSGFundingBreakdownPage201819PageTests : FundingStreamRegressionTestBase
    {
        /// <summary>
        /// The multiple results search term.
        /// </summary>
        private const string MultipleResultsSearchTerm = "St Marys";

        /// <summary>
        /// ProviderPSGFundingBreakdownPage201819Page basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderPSGFundingBreakdownPage201819Page_BasicLayout()
        {
            // Arrange Act
            ProviderPSGFundingBreakdown201819Page.NavigateToPage();

            // Assert
            ProviderPSGFundingBreakdown201819Page.EnsureCurrentProviderPage();
            ProviderPSGFundingBreakdown201819Page.EnsureBreadcrumbs();
            ProviderPSGFundingBreakdown201819Page.EnsureAllocationHistoryLink();
            ProviderPSGFundingBreakdown201819Page.EnsureMainParagraph();
            ProviderPSGFundingBreakdown201819Page.EnsureGovUkLinks();
            ProviderPSGFundingBreakdown201819Page.EnsureDownloadLinksAvailable();
        }

        /// <summary>
        /// ProviderPSGFundingBreakdownPage201819Page has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderPSGFundingBreakdownPage201819Page_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            ProviderPSGFundingBreakdown201819Page.NavigateToPage();

            // Assert
            ProviderPSGFundingBreakdown201819Page.EnsureAndClickBreadcrumb(ChooseHowToViewFundingLinkText);
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// ProviderPSGFundingBreakdownPage201819Page has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderPSGFundingBreakdownPage201819Page_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            ProviderPSGFundingBreakdown201819Page.NavigateToPage();

            // Assert
            ProviderPSGFundingBreakdown201819Page.EnsureAndClickBreadcrumb(ViewFundingAtOrganisationLinkText);
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// ProviderPSGFundingBreakdownPage201819Page has breadcrumb linking to provider details page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderPSGFundingBreakdownPage201819Page_HasBreadcrumbLinkingToProviderDetailsPage()
        {
            // Arrange Act
            ProviderPSGFundingBreakdown201819Page.NavigateToPage();

            // Assert
            ProviderPSGFundingBreakdown201819Page.EnsureAndClickBreadcrumb(ProviderDetailsPageMainProviderName);
            ProviderDetailPage.EnsureCurrentPage(ProviderDetailsPageMainProviderName);
        }

        /// <summary>
        /// ProviderPSGFundingBreakdownPage201819Page via did you mean has breadcrumb linking provider did you mean page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderPSGFundingBreakdownPage201819Page__ViaDidYouMean_HasBreadcrumbLinkingProviderDidYouMeanPage()
        {
            // Arrange Act
            ProviderPSGFundingBreakdown201819Page.NavigateToViaDidYouMeanPage(MultipleResultsSearchTerm);

            // Assert
            ProviderPSGFundingBreakdown201819Page.EnsureAndClickBreadcrumb(SearchResultsLinkText);
            ProviderResultsPage.EnsureBreadcrumbs();
            ProviderResultsPage.EnsureResultsText(MultipleResultsSearchTerm);
        }
    }
}