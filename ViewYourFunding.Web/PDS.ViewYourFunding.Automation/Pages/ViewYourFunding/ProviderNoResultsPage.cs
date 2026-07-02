using FluentAssertions;
using OpenQA.Selenium;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The ProviderNoResultsPage class.
    /// </summary>
    public class ProviderNoResultsPage : ViewYourFundingBasePage
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

        #endregion


        #region Assertions

        /// <summary>
        /// Ensures the current no reults page.
        /// </summary>
        public static void EnsureCurrentNoReultsPage()
        {
            EnsureCurrentPage(PageTitle);
        }

        /// <summary>
        /// Ensures the no results text.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        public static void EnsureNoResultsText(string searchTerm)
        {
            NoResultsHeader.Text.Should().Be($"No results for '{searchTerm}'");
        }

        /// <summary>
        /// Ensures the find an organisation link.
        /// </summary>
        public static void EnsureFindAnOrganisationLink()
        {
            FindOrganisationLink.Displayed.Should().BeTrue();
        }

        #endregion


        #region Page Elements

        /// <summary>
        /// Gets the find organisation link.
        /// </summary>
        /// <value>
        /// The find organisation link.
        /// </value>
        protected static IWebElement FindOrganisationLink => Driver.Instance.WaitToFindElement(By.LinkText("Find an organisation"));

        /// <summary>
        /// Gets the no results header.
        /// </summary>
        /// <value>
        /// The no results header.
        /// </value>
        protected static IWebElement NoResultsHeader => Driver.Instance.WaitToFindElement(By.TagName("h2"));

        #endregion

    }
}
