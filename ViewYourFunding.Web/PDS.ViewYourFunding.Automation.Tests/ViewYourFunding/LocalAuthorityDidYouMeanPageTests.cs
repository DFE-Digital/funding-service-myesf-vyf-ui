using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The LocalAuthorityDidYouMeanPageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class LocalAuthorityDidYouMeanPageTests : FundingStreamRegressionTestBase
    {
        /// <summary>
        /// The multiple results search term.
        /// </summary>
        public const string MultipleResultsSearchTerm = "ca";

        /// <summary>
        /// The back to top link results search term.
        /// </summary>
        public const string BackToTopLinkResultsSearchTerm = "a";

        /// <summary>
        /// LocalAuthorityDidYouMeanPage basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDidYouMeanPage_BasicLayout()
        {
            // Arrange
            LocalAuthorityDidYouMeanPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);

            // Assert
            LocalAuthorityDidYouMeanPage.EnsureCurrentPage();
            LocalAuthorityDidYouMeanPage.EnsureBreadcrumbs();
            LocalAuthorityDidYouMeanPage.EnsureSearchResultText(MultipleResultsSearchTerm);
        }

        /// <summary>
        /// LocalAuthorityDidYouMeanPage has expected results.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDidYouMeanPage_HasExpectedResults()
        {
            // Arrange Act
            LocalAuthorityDidYouMeanPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);

            // Assert
            LocalAuthorityDidYouMeanPage.EnsureCurrentPage();
            LocalAuthorityDidYouMeanPage.EnsureLocalAuthorityStatementLinks(
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("381", "Calderdale"),
                    new KeyValuePair<string, string>("873", "Cambridgeshire"),
                    new KeyValuePair<string, string>("202", "Camden"),
                    new KeyValuePair<string, string>("371", "Doncaster"),
                    new KeyValuePair<string, string>("888", "Lancashire"),
                    new KeyValuePair<string, string>("391", "Newcastle upon Tyne"),
                    new KeyValuePair<string, string>("807", "Redcar and Cleveland"),
                });
        }

        /// <summary>
        /// LocalAuthorityDidYouMeanPage when more than n results back to top link is displayed.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDidYouMeanPage_WhenMoreThanNResults_BackToTopLink_IsDisplayed()
        {
            // Arrange Act
            LocalAuthorityDidYouMeanPage.NavigateToPageUsingSearchTerm(BackToTopLinkResultsSearchTerm);

            // Assert
            LocalAuthorityDidYouMeanPage.EnsureCurrentPage();
            LocalAuthorityDidYouMeanPage.EnsureBackToTopLinkIsDisplayed(true);
        }

        /// <summary>
        /// LocalAuthorityDidYouMeanPage when less than n results back to top link is not displayed.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDidYouMeanPage_WhenLessThanNResults_BackToTopLink_IsNotDisplayed()
        {
            // Arrange Act
            LocalAuthorityDidYouMeanPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);

            // Assert
            LocalAuthorityDidYouMeanPage.EnsureCurrentPage();
            LocalAuthorityDidYouMeanPage.EnsureBackToTopLinkIsDisplayed(false);
        }

        /// <summary>
        /// LocalAuthorityDidYouMeanPage has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDidYouMeanPage_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            LocalAuthorityDidYouMeanPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);

            // Assert
            LocalAuthorityDidYouMeanPage.EnsureCurrentPage();
            LocalAuthorityDidYouMeanPage.EnsureAndClickBreadcrumb("Choose how to view funding");
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// Locals the authority no results page has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityNoResultsPage_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            LocalAuthorityDidYouMeanPage.NavigateToPageUsingSearchTerm(MultipleResultsSearchTerm);

            // Assert
            LocalAuthorityDidYouMeanPage.EnsureCurrentPage();
            LocalAuthorityDidYouMeanPage.EnsureAndClickBreadcrumb("View funding at organisation level");
            FindAnOrganisationPage.EnsureCurrentPage();
        }
    }
}