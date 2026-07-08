using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The ProviderFilterResultsPageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class ProviderFilterResultsPageTests : FundingStreamRegressionTestBase
    {
        #region Private fields

        /// <summary>
        /// The multiple results search term.
        /// </summary>
        private const string MultipleResultsSearchTerm = "st johns";

        /// <summary>
        /// The less than25 results search term.
        /// </summary>
        private const string LessThan25ResultsSearchTerm = "st johns catholic";

        /// <summary>
        /// The local authority filter term.
        /// </summary>
        private const string LocalAuthorityFilterTerm = "yorkshire";

        #endregion


        #region Tests

        /// <summary>
        /// ProviderFilterResultsPage basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFilterResultsPage_BasicLayout()
        {
            // Arrange Act
            ProviderFilterResultsPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);

            // Assert
            ProviderFilterResultsPage.EnsureCurrentProviderPage();
            ProviderFilterResultsPage.EnsureBreadcrumbs();
            ProviderFilterResultsPage.EnsureResultsText(MultipleResultsSearchTerm);
            ProviderFilterResultsPage.EnsureFilterOptions();
            ProviderFilterResultsPage.EnsureResetFilterLink();
            ProviderFilterResultsPage.EnsureLocalAuthorityFilterSearchBox(true);
        }

        /// <summary>
        /// ProviderFilterResultsPage has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderFilterResultsPage_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            ProviderFilterResultsPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);

            // Assert
            ProviderFilterResultsPage.EnsureAndClickBreadcrumb(ChooseHowToViewFundingLinkText);
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// ProviderFilterResultsPage has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderFilterResultsPage_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            ProviderFilterResultsPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);

            // Assert
            ProviderFilterResultsPage.EnsureAndClickBreadcrumb(ViewFundingAtOrganisationLinkText);
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// ProviderFilterResultsPage ensure search results.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFilterResultsPage_EnsureSearchResults()
        {
            // Arrange Act
            ProviderFilterResultsPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);

            // Assert
            ProviderFilterResultsPage.EnsureCurrentProviderPage();
            ProviderFilterResultsPage.EnsureSearchResultsReturned();
        }

        /// <summary>
        /// ProviderFilterResultsPage ensure local authority filter search box is not displayed.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderFilterResultsPage_EnsureLocalAuthorityFilterSearchBox_IsNotDisplayed()
        {
            // Arrange Act
            ProviderFilterResultsPage.NavigateToPageUsingSearchTerm(LessThan25ResultsSearchTerm);

            // Assert
            ProviderFilterResultsPage.EnsureCurrentProviderPage();
            ProviderFilterResultsPage.EnsureLocalAuthorityFilterSearchBox(false);
        }

        /// <summary>
        /// ProviderFilterResultsPage filter local authorities.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderFilterResultsPage_Filter_LocalAuthorities()
        {
            // Arrange Act
            ProviderFilterResultsPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);

            // Assert
            ProviderFilterResultsPage.EnsureCurrentProviderPage();
            ProviderFilterResultsPage.EnsureLocalAuthorityFilterSearchBox(true);
            var originalLocalAuthoritiesCount = ProviderFilterResultsPage.GetLocalAuthorityDisplayOptionsCount();
            ProviderFilterResultsPage.ApplyLocalAuthorityFilterTerm(LocalAuthorityFilterTerm);
            ProviderFilterResultsPage.EnsureLocalAuthorityCountIsLessThan(originalLocalAuthoritiesCount);
            ProviderFilterResultsPage.ResetLocalAuthorityFilterTerm();
            ProviderFilterResultsPage.EnsureLocalAuthorityCountIsTheSameAs(originalLocalAuthoritiesCount);
        }

        /// <summary>
        /// ProviderFilterResultsPage apply la ensure filter results.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderFilterResultsPage_ApplyLa_EnsureFilterResults()
        {
            // Arrange Act
            ProviderFilterResultsPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);
            var originalSearchCount = ProviderFilterResultsPage.GetSearchResultsCount();
            ProviderFilterResultsPage.ApplyOneLocalAuthorityFilter();

            // Assert
            ProviderFilterResultsPage.EnsureCurrentProviderPage();
            ProviderFilterResultsPage.EnsureLessResults(originalSearchCount);
        }

        /// <summary>
        /// ProviderFilterResultsPage apply establishment ensure filter results.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderFilterResultsPage_ApplyEstablishment_EnsureFilterResults()
        {
            // Arrange Act
            ProviderFilterResultsPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);
            var originalSearchCount = ProviderFilterResultsPage.GetSearchResultsCount();
            ProviderFilterResultsPage.ApplyOneEstablishmentFilter();

            // Assert
            ProviderFilterResultsPage.EnsureCurrentProviderPage();
            ProviderFilterResultsPage.EnsureLessResults(originalSearchCount);
        }

        /// <summary>
        /// ProviderFilterResultsPage apply all filters ensure all results.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderFilterResultsPage_ApplyAllFilters_EnsureAllResults()
        {
            // Arrange Act
            ProviderFilterResultsPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);
            var originalSearchCount = ProviderFilterResultsPage.GetSearchResultsCount();
            ProviderFilterResultsPage.ApplyAllEstablishmentFilters();
            ProviderFilterResultsPage.ApplyAllLocalAuthorityFilters();

            // Assert
            ProviderFilterResultsPage.EnsureCurrentProviderPage();
            ProviderFilterResultsPage.EnsureSameResults(originalSearchCount);
            ProviderFilterResultsPage.EnsureResultsText(MultipleResultsSearchTerm);
        }

        /// <summary>
        /// ProviderFilterResultsPage apply some filters ensure no results.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderFilterResultsPage_ApplySomeFilters_EnsureNoResults()
        {
            // Arrange Act
            ProviderFilterResultsPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);
            var originalSearchCount = ProviderFilterResultsPage.GetSearchResultsCount();
            ProviderFilterResultsPage.ApplyLastEstablishmentTypeFilter();
            ProviderFilterResultsPage.ApplyLastLocalAuthorityFilter();

            // Assert
            ProviderFilterResultsPage.EnsureCurrentProviderPage();
            ProviderFilterResultsPage.EnsureLessResults(originalSearchCount);
            ProviderFilterResultsPage.EnsureSameResults(0);
            ProviderFilterResultsPage.EnsureNoFilterResultsText();
        }

        /// <summary>
        /// ProviderFilterResultsPage ensure reset filters.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderFilterResultsPage_EnsureResetFilters()
        {
            // Arrange Act
            ProviderFilterResultsPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);
            var originalSearchCount = ProviderFilterResultsPage.GetSearchResultsCount();
            ProviderFilterResultsPage.ApplyOneEstablishmentFilter();
            ProviderFilterResultsPage.ApplyLastLocalAuthorityFilter();

            // Assert
            ProviderFilterResultsPage.EnsureCurrentProviderPage();
            ProviderFilterResultsPage.EnsureLessResults(originalSearchCount);
            ProviderFilterResultsPage.ResetFilters();
            ProviderFilterResultsPage.EnsureNoOptionIsSelected();
        }

        #endregion
    }
}