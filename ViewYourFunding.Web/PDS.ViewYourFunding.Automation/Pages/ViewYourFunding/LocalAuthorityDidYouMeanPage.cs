using FluentAssertions;
using OpenQA.Selenium;
using System.Collections.Generic;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The LocalAuthorityDidYouMeanPage class.
    /// </summary>
    public class LocalAuthorityDidYouMeanPage : ViewYourFundingBasePage
    {
        #region Private Fields

        /// <summary>
        /// The back to top link selector.
        /// </summary>
        private static readonly By BackToTopLinkSelector = By.ClassName("back-to-top-link");

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
            FindAnOrganisationPage.ClickLaCodeOptionButton();
            FindAnOrganisationPage.InputLocalAuthoritySearchText(searchTerm);
            FindAnOrganisationPage.SubmitLocalAuthoritySearch();
        }

        /// <summary>
        /// Clicks the local authority link.
        /// </summary>
        /// <param name="localAuthorityName">Name of the local authority.</param>
        public static void ClickLocalAuthorityLink(string localAuthorityName)
        {
            var link = LocalAuthorityStatementLinkBlock.FindElementOrDefault(By.LinkText(localAuthorityName));
            link.MoveAndClick();
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
        /// Ensures the search result text.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        public static void EnsureSearchResultText(string searchTerm)
        {
            SearchResultCountParagraph.Should().NotBeNull();
            SearchResultCountParagraph.Displayed.Should().BeTrue();
            SearchResultCountParagraph.Text.Should()
                .Match($"Searching on '{searchTerm}' we found * local authorities. Did you mean:");
        }

        /// <summary>
        /// Ensures the local authority statement links.
        /// </summary>
        /// <param name="expectedLocalAuthorities">The expected local authorities.</param>
        public static void EnsureLocalAuthorityStatementLinks(List<KeyValuePair<string, string>> expectedLocalAuthorities)
        {
            LocalAuthorityStatementLinkBlock.Should().NotBeNull();
            LocalAuthorityStatementLinkBlock.Displayed.Should().BeTrue();

            foreach (var expectedLocalAuthority in expectedLocalAuthorities)
            {
                var localAuthorityCode = expectedLocalAuthority.Key;
                var localAuthorityName = expectedLocalAuthority.Value;

                var link = LocalAuthorityStatementLinkBlock.FindElementOrDefault(By.LinkText(localAuthorityName));
                link.Should().NotBeNull($"there should be a link for local authority {localAuthorityName}");
                link.Displayed.Should().BeTrue($"there should be a link for local authority {localAuthorityName}");

                var target = link.GetAttribute("href");
                target.Should().Contain(
                    $"local-authority/statement/{localAuthorityCode}",
                    $"the link for {localAuthorityName} should target the statement page for local authority code {localAuthorityCode}");
            }
        }

        /// <summary>
        /// Ensures the back to top link is displayed.
        /// </summary>
        /// <param name="isDisplayed">if set to <c>true</c> [is displayed].</param>
        public static void EnsureBackToTopLinkIsDisplayed(bool isDisplayed)
        {
            if (isDisplayed)
            {
                BackToTopLink.Displayed.Should().BeTrue();
            }
            else
            {
                Driver.Instance.AssertShouldNotExist(BackToTopLinkSelector);
            }
        }

        #endregion


        #region Page Elements

        /// <summary>
        /// Gets the search result count paragraph.
        /// </summary>
        /// <value>
        /// The search result count paragraph.
        /// </value>
        protected static IWebElement SearchResultCountParagraph
            => Driver.Instance.WaitToFindElement(By.Id("search-result-count"));

        /// <summary>
        /// Gets the local authority statement link block.
        /// </summary>
        /// <value>
        /// The local authority statement link block.
        /// </value>
        protected static IWebElement LocalAuthorityStatementLinkBlock
            => Driver.Instance.WaitToFindElement(By.ClassName("local-authority-statement-links"));

        /// <summary>
        /// Gets the back to top link.
        /// </summary>
        /// <value>
        /// The back to top link.
        /// </value>
        protected static IWebElement BackToTopLink => Driver.Instance.WaitToFindElement(BackToTopLinkSelector);

        #endregion
    }
}