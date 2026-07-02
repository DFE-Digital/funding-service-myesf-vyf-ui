using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ViewYourFunding.Automation.Utilities;
using Driver = ViewYourFunding.Automation.Utilities.Driver;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The ProviderFilterResultsPage class.
    /// </summary>
    public class ProviderFilterResultsPage : ViewYourFundingBasePage
    {
        #region Private Fields

        /// <summary>
        /// The page title.
        /// </summary>
        private const string PageTitle = "Search results";

        /// <summary>
        /// The local authority search box selector.
        /// </summary>
        private static readonly ByAll LocalAuthoritySearchBoxSelector = new ByAll(By.TagName("input"), By.ClassName("filter-search"));

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
        /// Applies the one local authority filter.
        /// </summary>
        public static void ApplyOneLocalAuthorityFilter()
        {
            LocalAuthorityFilterOptions.First().Click();
        }

        /// <summary>
        /// Applies the one establishment filter.
        /// </summary>
        public static void ApplyOneEstablishmentFilter()
        {
            EstablishmentOptions.First().Click();
        }

        /// <summary>
        /// Applies all local authority filters.
        /// </summary>
        public static void ApplyAllLocalAuthorityFilters()
        {
            foreach (var laFilterOption in LocalAuthorityFilterOptions)
            {
                laFilterOption.Click();
            }
        }

        /// <summary>
        /// Applies all establishment filters.
        /// </summary>
        public static void ApplyAllEstablishmentFilters()
        {
            foreach (var establishmentOption in EstablishmentOptions)
            {
                establishmentOption.Click();
            }
        }

        /// <summary>
        /// Resets the filters.
        /// </summary>
        public static void ResetFilters()
        {
            ResetFilterLink.MoveAndClick();
        }

        /// <summary>
        /// Applies the last local authority filter.
        /// </summary>
        public static void ApplyLastLocalAuthorityFilter()
        {
            LocalAuthorityFilterOptions.Last().Click();
        }

        /// <summary>
        /// Applies the last establishment type filter.
        /// </summary>
        public static void ApplyLastEstablishmentTypeFilter()
        {
            EstablishmentOptions.Last().Click();
        }

        /// <summary>
        /// Applies the local authority filter term.
        /// </summary>
        /// <param name="filterTerm">The filter term.</param>
        public static void ApplyLocalAuthorityFilterTerm(string filterTerm)
        {
            LocalAuthorityFilterSearchBox.SendKeys(filterTerm);
        }

        /// <summary>
        /// Gets the local authority display options count.
        /// </summary>
        /// <returns>The display options count.</returns>
        public static int GetLocalAuthorityDisplayOptionsCount()
        {
            return LocalAuthorityOptionDisplayItems.Count(x => x.Displayed);
        }

        /// <summary>
        /// Resets the local authority filter term.
        /// </summary>
        public static void ResetLocalAuthorityFilterTerm()
        {
            LocalAuthorityFilterSearchBox.Clear();
            LocalAuthorityFilterSearchBox.MoveAndClick();
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
        /// Ensures the no filter results text.
        /// </summary>
        public static void EnsureNoFilterResultsText()
        {
            ResultsParagraph.Text.Should()
                .Contain("no schools were found matching your search. Please refine your criteria.");
        }

        /// <summary>
        /// Ensures the search results returned.
        /// </summary>
        public static void EnsureSearchResultsReturned()
        {
            ResultRows.Count.Should().BeGreaterOrEqualTo(1);
            if (ResultRows.Count >= 25)
            {
                BackToTopLink.Displayed.Should().BeTrue();
            }
            else
            {
                BackToTopLink.Displayed.Should().BeFalse();
            }
        }

        /// <summary>
        /// Ensures the reset filter link.
        /// </summary>
        public static void EnsureResetFilterLink()
        {
            ResetFilterLink.Displayed.Should().BeTrue();
            ResetFilterLink.Text.Should().Contain("Reset filters");
        }

        /// <summary>
        /// Ensures the filter options.
        /// </summary>
        public static void EnsureFilterOptions()
        {
            LocalAuthorityFilterOptions.Count().Should().BeGreaterOrEqualTo(1);
            EstablishmentOptions.Count().Should().BeGreaterOrEqualTo(1);
        }

        /// <summary>
        /// Ensures the less results.
        /// </summary>
        /// <param name="originalSearchCount">The original search count.</param>
        public static void EnsureLessResults(int originalSearchCount)
        {
            originalSearchCount.Should().BeGreaterThan(GetSearchResultsCount());
        }

        /// <summary>
        /// Ensures the same results.
        /// </summary>
        /// <param name="originalSearchCount">The original search count.</param>
        public static void EnsureSameResults(int originalSearchCount)
        {
            originalSearchCount.Should().Be(GetSearchResultsCount());
        }

        /// <summary>
        /// Ensures the no option is selected.
        /// </summary>
        public static void EnsureNoOptionIsSelected()
        {
            LocalAuthorityFilterOptions.Any(option => option.Selected).Should().BeFalse();
            EstablishmentOptions.Any(option => option.Selected).Should().BeFalse();
        }

        /// <summary>
        /// Ensures the local authority filter search box.
        /// </summary>
        /// <param name="isVisible">if set to <c>true</c> [is visible].</param>
        public static void EnsureLocalAuthorityFilterSearchBox(bool isVisible)
        {
            LocalAuthorityFilterSearchBox.Displayed.Should().Be(isVisible);
        }

        /// <summary>
        /// Ensures the local authority count is less than.
        /// </summary>
        /// <param name="originalLocalAuthoritiesCount">The original local authorities count.</param>
        public static void EnsureLocalAuthorityCountIsLessThan(int originalLocalAuthoritiesCount)
        {
            originalLocalAuthoritiesCount.Should().BeGreaterThan(GetLocalAuthorityDisplayOptionsCount());
        }

        /// <summary>
        /// Ensures the local authority count is the same as.
        /// </summary>
        /// <param name="originalLocalAuthoritiesCount">The original local authorities count.</param>
        public static void EnsureLocalAuthorityCountIsTheSameAs(int originalLocalAuthoritiesCount)
        {
            originalLocalAuthoritiesCount.Should().Be(GetLocalAuthorityDisplayOptionsCount());
        }

        #endregion


        #region Page Properties

        /// <summary>
        /// Gets the search results count.
        /// </summary>
        /// <returns>The search results count.</returns>
        public static int GetSearchResultsCount()
        {
            return ResultRows.Count(result => result.Displayed);
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
        protected static ReadOnlyCollection<IWebElement> ResultRows => ResultsListSection.FindElements(By.ClassName("searchResultFilter"));

        /// <summary>
        /// Gets the results paragraph.
        /// </summary>
        /// <value>
        /// The results paragraph.
        /// </value>
        protected static IWebElement ResultsParagraph => Driver.Instance.WaitToFindElement(By.CssSelector(".column-document-list-two-thirds .form-block p"));

        /// <summary>
        /// Gets the reset filter link.
        /// </summary>
        /// <value>
        /// The reset filter link.
        /// </value>
        protected static IWebElement ResetFilterLink => Driver.Instance.WaitToFindElement(By.Id("ResetSearch"));

        /// <summary>
        /// Gets the local authority filter options.
        /// </summary>
        /// <value>
        /// The local authority filter options.
        /// </value>
        protected static IEnumerable<IWebElement> LocalAuthorityFilterOptions => Driver.Instance.WaitToFindElements(By.Name("QueryFilter.Filters.LocalAuthority"));

        /// <summary>
        /// Gets the local authority option display items.
        /// </summary>
        /// <value>
        /// The local authority option display items.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> LocalAuthorityOptionDisplayItems => Driver.Instance.FindElements(By.CssSelector(".LocalAuthorityFilter li.gds-multiple-choice.multiple-choice.myesf-filter"));

        /// <summary>
        /// Gets the establishment options.
        /// </summary>
        /// <value>
        /// The establishment options.
        /// </value>
        protected static IEnumerable<IWebElement> EstablishmentOptions => Driver.Instance.WaitToFindElements(By.Name("QueryFilter.Filters.EstablishmentType"));

        /// <summary>
        /// Gets the back to top link.
        /// </summary>
        /// <value>
        /// The back to top link.
        /// </value>
        protected static IWebElement BackToTopLink => Driver.Instance.WaitToFindElement(By.CssSelector(".form-group.direction-rtl"));

        /// <summary>
        /// Gets the local authority filter search box.
        /// </summary>
        /// <value>
        /// The local authority filter search box.
        /// </value>
        protected static IWebElement LocalAuthorityFilterSearchBox => Driver.Instance.WaitToFindElement(LocalAuthoritySearchBoxSelector);

        #endregion
    }
}
