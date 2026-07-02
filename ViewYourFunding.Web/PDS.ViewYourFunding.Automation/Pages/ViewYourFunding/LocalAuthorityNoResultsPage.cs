using FluentAssertions;
using OpenQA.Selenium;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The LocalAuthorityNoResultsPage class.
    /// </summary>
    public class LocalAuthorityNoResultsPage : ViewYourFundingBasePage
    {
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
            FindAnOrganisationPage.ClickLaCodeOptionButton();
            FindAnOrganisationPage.InputLocalAuthoritySearchText(searchTerm);
            FindAnOrganisationPage.SubmitLocalAuthoritySearch();
        }

        #endregion


        #region Assertions

        /// <summary>
        /// Ensures the current page.
        /// </summary>
        public static void EnsureCurrentPage()
        {
            EnsureCurrentPage("Search results");
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
        /// Ensures the and click find an organisation link.
        /// </summary>
        public static void EnsureAndClickFindAnOrganisationLink()
        {
            FindOrganisationLink.Displayed.Should().BeTrue();
            FindOrganisationLink.MoveAndClick();
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
        protected static IWebElement NoResultsHeader => Driver.Instance.WaitToFindElement(By.ClassName("heading-medium"));

        #endregion
    }
}