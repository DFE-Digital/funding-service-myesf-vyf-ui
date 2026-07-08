using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The LocalAuthorityPSGBreakdown201819PageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class LocalAuthorityPSGBreakdown201819PageTests : FundingStreamRegressionTestBase
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
        /// LocalAuthorityPSGBreakdown201819Page page basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityPSGBreakdown201819Page_BasicLayout()
        {
            // Arrange Act
            LocalAuthorityPSGBreakdown201819Page.NavigateToPageViaDidYouMeanPage(MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGBreakdown201819Page.EnsureCurrentPage();
            LocalAuthorityPSGBreakdown201819Page.EnsureBreadcrumbs();
            LocalAuthorityPSGBreakdown201819Page.EnsureAllocationHistoryLink();
            LocalAuthorityPSGBreakdown201819Page.EnsureMainParagraph();
            LocalAuthorityPSGBreakdown201819Page.EnsureGuidanceLinks();
            LocalAuthorityPSGBreakdown201819Page.EnsureDownloadLinksAvailable();
        }

        /// <summary>
        /// LocalAuthorityPSGBreakdown201819Page page has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGBreakdown201819Page_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            LocalAuthorityPSGBreakdown201819Page.NavigateToPageViaDidYouMeanPage(MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGBreakdown201819Page.EnsureAndClickBreadcrumb(ChooseHowToViewFundingLinkText);
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityPSGBreakdown201819Page page has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGBreakdown201819Page_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            LocalAuthorityPSGBreakdown201819Page.NavigateToPageViaDidYouMeanPage(MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGBreakdown201819Page.EnsureAndClickBreadcrumb(ViewFundingAtOrganisationLinkText);
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityPSGBreakdown201819Page page has breadcrumb linking to statement page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGBreakdown201819Page_HasBreadcrumbLinkingToStatementPage()
        {
            // Arrange Act
            LocalAuthorityPSGBreakdown201819Page.NavigateToPageViaDidYouMeanPage(MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGBreakdown201819Page.EnsureAndClickBreadcrumb(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
        }

        /// <summary>
        /// LocalAuthorityPSGBreakdown201819Page page via did you mean has breadcrumb linking to did you mean page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGBreakdown201819Page_ViaDidYouMean_HasBreadcrumbLinkingToDidYouMeanPage()
        {
            // Arrange Act
            LocalAuthorityPSGBreakdown201819Page.NavigateToPageViaDidYouMeanPage(MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGBreakdown201819Page.EnsureAndClickBreadcrumb(SearchResultsLinkText);
            LocalAuthorityDidYouMeanPage.EnsureCurrentPage();
            LocalAuthorityDidYouMeanPage.EnsureSearchResultText(MultipleResultsSearchTerm);
        }

        /// <summary>
        /// LocalAuthorityPSGBreakdown201819Page page via exact match does not have breadcrumb linking to did you mean page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGBreakdown201819Page_ViaExactMatch_DoesNotHaveBreadcrumbLinkingToDidYouMeanPage()
        {
            // Arrange Act
            LocalAuthorityPSGBreakdown201819Page.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityPSGBreakdown201819Page.EnsureBreadcrumbNotShown(SearchResultsLinkText);
        }
    }
}