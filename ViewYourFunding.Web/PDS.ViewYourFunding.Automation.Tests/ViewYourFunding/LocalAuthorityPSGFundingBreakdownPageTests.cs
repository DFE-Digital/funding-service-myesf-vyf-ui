using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The LocalAuthorityPSGFundingBreakdownPageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class LocalAuthorityPSGFundingBreakdownPageTests : FundingStreamRegressionTestBase
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
        /// The establishment filter search term.
        /// </summary>
        public const string EstablishmentFilterSearchTerm = "ham";

        /// <summary>
        /// The establishment filter local authority name.
        /// </summary>
        public const string EstablishmentFilterLocalAuthorityName = "Hammersmith and Fulham";

        /// <summary>
        /// LocalAuthorityPSGFundingBreakdownPage basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityPSGFundingBreakdownPage_BasicLayout()
        {
            // Arrange Act
            LocalAuthorityPSGFundingBreakdownPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGFundingBreakdownPage.EnsureCurrentPage($"PE and sport premium {PsgCurrentYearFrom} to {PsgCurrentYearTo}", true);
            LocalAuthorityPSGFundingBreakdownPage.EnsureBreadcrumbs();
            LocalAuthorityPSGFundingBreakdownPage.EnsureFilters();
            LocalAuthorityPSGFundingBreakdownPage.EnsureTotalAllocation(PsgCurrentYearFrom, PsgCurrentYearTo);
            LocalAuthorityPSGFundingBreakdownPage.EnsureDocumentLink();
            LocalAuthorityPSGFundingBreakdownPage.EnsureBreakdownTable();
            LocalAuthorityPSGFundingBreakdownPage.EnsureBackToTopLink();
            LocalAuthorityPSGFundingBreakdownPage.EnsureAllocationHistoryLink();
            LocalAuthorityPSGFundingBreakdownPage.EnsureGeneralResourcesSection();
        }

        /// <summary>
        /// LocalAuthorityPSGFundingBreakdownPage apply filter top link is not displayed.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGFundingBreakdownPage_ApplyFilter_TopLink_IsNotDisplayed()
        {
            // Arrange Act
            LocalAuthorityPSGFundingBreakdownPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGFundingBreakdownPage.EnsureCurrentPage($"PE and sport premium {PsgCurrentYearFrom} to {PsgCurrentYearTo}", true);
            LocalAuthorityPSGFundingBreakdownPage.EnsureBreadcrumbs();
            LocalAuthorityPSGFundingBreakdownPage.EnsureFilters();
            LocalAuthorityPSGFundingBreakdownPage.EnsureBackToTopLink();
            LocalAuthorityPSGFundingBreakdownPage.ApplyFirstEstablishmentFilter();
            LocalAuthorityPSGFundingBreakdownPage.EnsureBackToTopLinkIsNotDisplayed();
        }

        /// <summary>
        /// LocalAuthorityPSGFundingBreakdownPage apply filter provider total allocation amount is updated.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGFundingBreakdownPage_ApplyFilter_ProviderTotalAllocationAmount_IsUpdated()
        {
            // Arrange
            LocalAuthorityPSGFundingBreakdownPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Act
            var providerTotalAllocationAmount =
                LocalAuthorityPSGFundingBreakdownPage.GetProviderTotalAllocationAmount();
            LocalAuthorityPSGFundingBreakdownPage.ApplyLastEstablishmentFilter();

            // Assert
            LocalAuthorityPSGFundingBreakdownPage.EnsureProviderFundingTotalAllocationAmountIsUpdated(providerTotalAllocationAmount);
        }

        /// <summary>
        /// LocalAuthorityPSGFundingBreakdownPage apply filter total count text is updated.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGFundingBreakdownPage_ApplyFilter_TotalCountText_IsUpdated()
        {
            // Arrange Act
            LocalAuthorityPSGFundingBreakdownPage.NavigateToPageViaDidYouMeanPage(
                EstablishmentFilterSearchTerm, EstablishmentFilterLocalAuthorityName);

            // Assert
            LocalAuthorityPSGFundingBreakdownPage.EnsureFilterMultipleMatchTextIsDisplayed(true);
            LocalAuthorityPSGFundingBreakdownPage.EnsureFilterSingleMatchTextIsDisplayed(false);
            LocalAuthorityPSGFundingBreakdownPage.ApplyLastEstablishmentFilter();
            LocalAuthorityPSGFundingBreakdownPage.EnsureFilterMultipleMatchTextIsDisplayed(false);
            LocalAuthorityPSGFundingBreakdownPage.EnsureFilterSingleMatchTextIsDisplayed(true);
        }

        /// <summary>
        /// LocalAuthorityPSGFundingBreakdownPage when via did you mean has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGFundingBreakdownPage_WhenViaDidYouMean_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            LocalAuthorityPSGFundingBreakdownPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGFundingBreakdownPage.EnsureAndClickBreadcrumb("Choose how to view funding");
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityPSGFundingBreakdownPage when via did you mean has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGFundingBreakdownPage_WhenViaDidYouMean_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            LocalAuthorityPSGFundingBreakdownPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGFundingBreakdownPage.EnsureAndClickBreadcrumb("View funding at organisation level");
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityPSGFundingBreakdownPage when via did you mean has breadcrumb linking to search results page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGFundingBreakdownPage_WhenViaDidYouMean_HasBreadcrumbLinkingToSearchResultsPage()
        {
            // Arrange Act
            LocalAuthorityPSGFundingBreakdownPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGFundingBreakdownPage.EnsureAndClickBreadcrumb("Search results");
            LocalAuthorityDidYouMeanPage.EnsureCurrentPage();
            LocalAuthorityDidYouMeanPage.EnsureSearchResultText(MultipleResultsSearchTerm);
        }

        /// <summary>
        /// LocalAuthorityPSGFundingBreakdownPage when via did you mean has breadcrumb linking statement page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGFundingBreakdownPage_WhenViaDidYouMean_HasBreadcrumbLinkingStatementPage()
        {
            // Arrange Act
            LocalAuthorityPSGFundingBreakdownPage.NavigateToPageViaDidYouMeanPage(
                MultipleResultsSearchTerm, LocalAuthorityName);

            // Assert
            LocalAuthorityPSGFundingBreakdownPage.EnsureAndClickBreadcrumb(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
        }

        /// <summary>
        /// LocalAuthorityPSGFundingBreakdownPage when via exact search has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGFundingBreakdownPage_WhenViaExactSearch_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            LocalAuthorityPSGFundingBreakdownPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityPSGFundingBreakdownPage.EnsureAndClickBreadcrumb("Choose how to view funding");
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityPSGFundingBreakdownPage when via exact search has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGFundingBreakdownPage_WhenViaExactSearch_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            LocalAuthorityPSGFundingBreakdownPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityPSGFundingBreakdownPage.EnsureAndClickBreadcrumb("View funding at organisation level");
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityPSGFundingBreakdownPage when via exact search does not have breadcrumb linking to search results page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGFundingBreakdownPage_WhenViaExactSearch_DoesNotHaveBreadcrumbLinkingToSearchResultsPage()
        {
            // Arrange Act
            LocalAuthorityPSGFundingBreakdownPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityPSGFundingBreakdownPage.EnsureBreadcrumbNotShown("Search results");
        }

        /// <summary>
        /// LocalAuthorityPSGFundingBreakdownPage when via exact search has breadcrumb linking statement page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityPSGFundingBreakdownPage_WhenViaExactSearch_HasBreadcrumbLinkingStatementPage()
        {
            // Arrange Act
            LocalAuthorityPSGFundingBreakdownPage.NavigateToPageUsingExactSearch(LocalAuthorityCode);

            // Assert
            LocalAuthorityPSGFundingBreakdownPage.EnsureAndClickBreadcrumb(LocalAuthorityName);
            LocalAuthorityStatementPage.EnsureCurrentPage(LocalAuthorityName);
        }
    }
}