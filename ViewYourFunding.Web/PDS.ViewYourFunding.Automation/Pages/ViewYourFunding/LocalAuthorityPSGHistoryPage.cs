using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Linq;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The LocalAuthorityPSGHistoryPage class.
    /// </summary>
    public class LocalAuthorityPSGHistoryPage : ViewYourFundingBasePage
    {
        #region Actions

        /// <summary>
        /// Navigates to page via did you mean page.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="localAuthorityName">Name of the local authority.</param>
        public static void NavigateToPageViaDidYouMeanPage(string searchTerm, string localAuthorityName)
        {
            StartPage.Open();
            StartPage.ClickStartButton();
            ViewingChoicePage.SelectOrganisationOption();
            ViewingChoicePage.ClickContinueButton();
            FindAnOrganisationPage.ClickLaCodeOptionButton();
            FindAnOrganisationPage.InputLocalAuthoritySearchText(searchTerm);
            FindAnOrganisationPage.SubmitLocalAuthoritySearch();
            LocalAuthorityDidYouMeanPage.ClickLocalAuthorityLink(localAuthorityName);
            LocalAuthorityStatementPage.ClickPSGAllocationHistoryLink();
        }

        /// <summary>
        /// Navigates to page using exact search.
        /// </summary>
        /// <param name="localAuthorityCode">The local authority code.</param>
        public static void NavigateToPageUsingExactSearch(string localAuthorityCode)
        {
            StartPage.Open();
            StartPage.ClickStartButton();
            ViewingChoicePage.SelectOrganisationOption();
            ViewingChoicePage.ClickContinueButton();
            FindAnOrganisationPage.ClickLaCodeOptionButton();
            FindAnOrganisationPage.InputLocalAuthoritySearchText(localAuthorityCode);
            FindAnOrganisationPage.SubmitLocalAuthoritySearch();
            LocalAuthorityStatementPage.ClickPSGAllocationHistoryLink();
        }

        /// <summary>
        /// Click201819s the link.
        /// </summary>
        public static void Click201819Link()
        {
            LegacyFundingBreakdownLinkForYear(2018, 2019).MoveAndClick();
        }

        public static void ClickLatestPublicationLink()
        {
            var publicationLinkTableCell = LocalAuthorityPSGFundingBreakDownPublicationLinkCells(2019, 2020).First();
            var link = publicationLinkTableCell.FindElement(By.TagName("a"));
            link.MoveAndClick();
        }

        #endregion


        #region Assertions

        /// <summary>
        /// Ensures the current page.
        /// </summary>
        public static void EnsureCurrentPage()
        {
            EnsureCurrentPage("PE and sport premium", true);
            EnsureCurrentPage("Allocation history", true);
        }

        /// <summary>
        /// Ensures the main paragraph.
        /// </summary>
        /// <param name="localAuthorityName">Name of the local authority.</param>
        public static void EnsureMainParagraph(string localAuthorityName)
        {
            MainPageParagraph.Text.Should()
                .Contain($"Find breakdowns of PE and sport premium for {localAuthorityName} for each academic year.");
        }

        /// <summary>
        /// Ensures all years.
        /// </summary>
        /// <param name="latestYearFrom">The latest year from.</param>
        /// <param name="latestYearTo">The latest year to.</param>
        /// <param name="localAuthorityCode">The local authority code.</param>
        public static void EnsureAllYears(int latestYearFrom, int latestYearTo, string localAuthorityCode)
        {
            for (int yearsAgo = 0; yearsAgo <= 3; yearsAgo++)
            {
                var currentYearFrom = latestYearFrom - yearsAgo;
                var currentYearTo = latestYearTo - yearsAgo;

                EnsureHeading($"{currentYearFrom} to {currentYearTo}");

                var isFirst = true;
                if (currentYearFrom > 2018)
                {
                    var publicationLinkTableCells = LocalAuthorityPSGFundingBreakDownPublicationLinkCells(currentYearFrom, currentYearTo);
                    publicationLinkTableCells.Should().HaveCountGreaterThan(0);

                    foreach (var publicationLinkTableCell in publicationLinkTableCells)
                    {
                        var link = publicationLinkTableCell.FindElement(By.TagName("a"));
                        link
                            .GetAttribute("href")
                            .Should().Contain($"pe-and-sport-premium/funding-breakdown/{currentYearFrom}-to-{currentYearTo}/{localAuthorityCode}/");

                        var tag = publicationLinkTableCell.FindElementOrDefault(By.ClassName("govuk-tag"));

                        if (isFirst)
                        {
                            tag.Should().NotBeNull();
                            tag.Text.Should().BeEquivalentTo("Latest");
                            isFirst = false;
                        }
                        else
                        {
                            tag.Should().BeNull();
                        }
                    }
                }
                else if (currentYearFrom == 2018)
                {
                    LegacyFundingBreakdownLinkForYear(currentYearFrom, currentYearTo)
                        .GetAttribute("href")
                        .Should().Contain($"local-authority/pe-and-sport-premium/funding-breakdown-2018-to-2019/{localAuthorityCode}");
                }
                else if (currentYearFrom == 2017)
                {
                    LegacyFundingBreakdownLinkForYear(currentYearFrom, currentYearTo)
                        .GetAttribute("href")
                        .Should().Contain("www.gov.uk/government/publications/pe-and-sport-premium-funding-allocations-for-2017-to-2018");
                }
                else if (currentYearFrom == 2016)
                {
                    LegacyFundingBreakdownLinkForYear(currentYearFrom, currentYearTo)
                        .GetAttribute("href")
                        .Should().Contain("www.gov.uk/government/publications/pe-and-sport-premium-funding-conditions-for-2016-to-2017");
                }
            }
        }

        #endregion


        #region Page Elements

        /// <summary>
        /// Gets the main page paragraph.
        /// </summary>
        /// <value>
        /// The main page paragraph.
        /// </value>
        protected static IWebElement MainPageParagraph =>
            Driver.Instance.WaitToFindElement(new ByAll(By.TagName("p"), By.ClassName("lede")));

        /// <summary>
        /// Locals the authority PSG funding break down publication link cells.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <returns>The list of web elements.</returns>
        protected static IReadOnlyCollection<IWebElement> LocalAuthorityPSGFundingBreakDownPublicationLinkCells(int yearFrom, int yearTo) =>
            Driver.Instance.WaitToFindElements(By.ClassName($"publication-link-{yearFrom}-to-{yearTo}"));

        /// <summary>
        /// Legacies the funding breakdown link for year.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <returns>The web element.</returns>
        protected static IWebElement LegacyFundingBreakdownLinkForYear(int yearFrom, int yearTo) =>
            Driver.Instance.WaitToFindElement(By.LinkText($"PE and sport premium: funding allocations for {yearFrom} to {yearTo}"));

        #endregion


        #region Private helpers

        /// <summary>
        /// Ensures the heading.
        /// </summary>
        /// <param name="headingText">The heading text.</param>
        private static void EnsureHeading(string headingText)
        {
            var headings = Driver.Instance.WaitToFindElements(new ByAll(By.TagName("h2"), By.ClassName("heading-guidance")));
            var item = headings.First(heading => heading.Text.Contains(headingText));
            item.Displayed.Should().BeTrue();
        }

        #endregion
    }
}
