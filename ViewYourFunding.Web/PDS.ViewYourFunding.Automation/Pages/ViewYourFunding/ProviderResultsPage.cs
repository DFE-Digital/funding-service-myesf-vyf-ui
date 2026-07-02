using FluentAssertions;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The ProviderResultsPage class.
    /// </summary>
    public class ProviderResultsPage : ViewYourFundingBasePage
    {
        #region Private Fields

        /// <summary>
        /// The page title.
        /// </summary>
        private const string PageTitle = "Search results";

        #endregion


        #region Actions

        /// <summary>
        /// Navigates to page using search term.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        public static void NavigateToPageUsingSearchTerm(string searchTerm)
        {
            StartPage.Open();
            StartPage.ClickStartButton();
            ViewingChoicePage.SelectOrganisationOption();
            ViewingChoicePage.ClickContinueButton();
            FindAnOrganisationPage.ClickProviderOptionButton();
            FindAnOrganisationPage.InputProviderText(searchTerm);
            FindAnOrganisationPage.SubmitProviderSearch();
        }

        /// <summary>
        /// Clicks the provider link.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        public static void ClickProviderLink(string providerName)
        {
            var link = ProviderStatementLinkBlock.FindElementOrDefault(By.PartialLinkText(providerName));
            link.MoveAndClick();
        }

        #endregion


        #region Assertions

        /// <summary>
        /// Ensures the current provider page.
        /// </summary>
        public static void EnsureCurrentProviderPage()
        {
            EnsureCurrentPage(PageTitle);
        }

        /// <summary>
        /// Ensures the results text.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        public static void EnsureResultsText(string searchTerm)
        {
            ResultsParagraph.Text.Should().Contain($"Searching on '{searchTerm}' we found");
        }

        /// <summary>
        /// Ensures the search results returned.
        /// </summary>
        public static void EnsureSearchResultsReturned()
        {
            ResultRows.Count.Should().BeGreaterOrEqualTo(1);
        }

        #endregion


        #region Page Elements

        /// <summary>
        /// Gets the results list section.
        /// </summary>
        /// <value>
        /// The results list section.
        /// </value>
        protected static IWebElement ResultsListSection => Driver.Instance.WaitToFindElement(By.CssSelector(".column-document-list-two-thirds .form-block"));

        /// <summary>
        /// Gets the result rows.
        /// </summary>
        /// <value>
        /// The result rows.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> ResultRows => ResultsListSection.FindElements(By.ClassName("sectionContent"));

        /// <summary>
        /// Gets the results paragraph.
        /// </summary>
        /// <value>
        /// The results paragraph.
        /// </value>
        protected static IWebElement ResultsParagraph => Driver.Instance.WaitToFindElement(By.CssSelector(".column-document-list-two-thirds .form-block p"));

        /// <summary>
        /// Gets the provider statement link block.
        /// </summary>
        /// <value>
        /// The provider statement link block.
        /// </value>
        protected static IWebElement ProviderStatementLinkBlock
            => Driver.Instance.WaitToFindElement(By.ClassName("document-list"));
        #endregion

    }
}
