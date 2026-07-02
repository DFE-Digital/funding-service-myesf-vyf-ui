using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The ProviderPSGAllocationHistoryPage class.
    /// </summary>
    public class ProviderPSGAllocationHistoryPage : ViewYourFundingBasePage
    {
        #region Private Fields

        /// <summary>
        /// The provider name.
        /// </summary>
        private const string ProviderName = "St Mary's Kilburn Church of England Primary School";

        /// <summary>
        /// The page title.
        /// </summary>
        private const string PageTitle = "PE and sport premium\r\nAllocation history";

        /// <summary>
        /// The year headings.
        /// </summary>
        private static readonly ReadOnlyCollection<string> YearHeadings = new ReadOnlyCollection<string>(new List<string>
        {
            "2019 to 2020",
            "2018 to 2019",
            "2017 to 2018",
            "2016 to 2017"
        });

        /// <summary>
        /// The allocation page links.
        /// </summary>
        private static readonly ReadOnlyCollection<string> AllocationPageLinks = new ReadOnlyCollection<string>(new List<string>
        {
            "https://www.gov.uk/government/publications/pe-and-sport-premium-funding-allocations-for-2017-to-2018",
            "https://www.gov.uk/government/publications/pe-and-sport-premium-funding-conditions-for-2016-to-2017",
            "pe-and-sport-premium/provider-funding-breakdown-2018-to-2019/10079314"
        });

        /// <summary>
        /// The partial allocation page links.
        /// </summary>
        private static readonly ReadOnlyCollection<KeyValuePair<string, string>> PartialAllocationPageLinks =
            new ReadOnlyCollection<KeyValuePair<string, string>>(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("pe-and-sport-premium/provider-funding-breakdown/", "/10079314-2019-to-2020")
            });

        #endregion


        #region Actions

        /// <summary>
        /// Navigates to page.
        /// </summary>
        public static void NavigateToPage()
        {
            StartPage.Open();
            StartPage.ClickStartButton();
            ViewingChoicePage.SelectOrganisationOption();
            ViewingChoicePage.ClickContinueButton();
            FindAnOrganisationPage.ClickProviderOptionButton();
            FindAnOrganisationPage.InputProviderText(ProviderName);
            FindAnOrganisationPage.SubmitProviderSearch();
            ProviderDetailPage.OpenMainContent();
            ProviderDetailPage.NavigateToPSGAllocationHistoryPage();
        }

        /// <summary>
        /// Navigates to via did you mean page.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        public static void NavigateToViaDidYouMeanPage(string searchTerm)
        {
            StartPage.Open();
            StartPage.ClickStartButton();
            ViewingChoicePage.SelectOrganisationOption();
            ViewingChoicePage.ClickContinueButton();
            FindAnOrganisationPage.ClickProviderOptionButton();
            FindAnOrganisationPage.InputProviderText(searchTerm);
            FindAnOrganisationPage.SubmitProviderSearch();
            ProviderResultsPage.ClickProviderLink(ProviderName);
            ProviderDetailPage.OpenMainContent();
            ProviderDetailPage.NavigateToPSGAllocationHistoryPage();
        }

        /// <summary>
        /// Navigates to PSG funding breakdown page2018.
        /// </summary>
        public static void NavigateToPSGFundingBreakdownPage2018()
        {
            PSGFundingBreakDown2018Link.Click();
        }

        /// <summary>
        /// Navigates to PSG funding breakdown page.
        /// </summary>
        public static void NavigateToPsgFundingBreakdownPage()
        {
            PSGFundingBreakdownLink.Click();
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
        /// Ensures the main paragraph.
        /// </summary>
        public static void EnsureMainParagraph()
        {
            MainPageParagraph.Text.Should()
                .Contain($"Find breakdowns of PE and sport premium for {ProviderName} for each academic year.");
        }

        /// <summary>
        /// Ensures all year headings.
        /// </summary>
        public static void EnsureAllYearHeadings()
        {
            YearHeadings.ToList().ForEach(EnsureHeading);
        }

        /// <summary>
        /// Ensures all allocation links.
        /// </summary>
        public static void EnsureAllAllocationLinks()
        {
            var pageLinks = Driver.Instance.WaitToFindElements(By.TagName("a"));

            var pageLinksWithHref =
                pageLinks.Where(pageLink => !string.IsNullOrWhiteSpace(pageLink.GetAttribute("href"))).ToList();

            foreach (var allocationPageLink in AllocationPageLinks)
            {
                pageLinksWithHref.Any(pageLink => pageLink.GetAttribute("href").Contains(allocationPageLink)).Should().BeTrue();
            }

            foreach (var allocationPageLink in PartialAllocationPageLinks)
            {
                pageLinksWithHref.Any(pageLink =>
                    pageLink.GetAttribute("href").Contains(allocationPageLink.Key)
                    && pageLink.GetAttribute("href").EndsWith(allocationPageLink.Value)).Should().BeTrue();
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
        /// Gets the PSG funding break down2018 link.
        /// </summary>
        /// <value>
        /// The PSG funding break down2018 link.
        /// </value>
        protected static IWebElement PSGFundingBreakDown2018Link =>
            Driver.Instance.WaitToFindElement(By.LinkText("PE and sport premium: funding allocations for 2018 to 2019"));

        /// <summary>
        /// Gets the PSG funding breakdown link.
        /// </summary>
        /// <value>
        /// The PSG funding breakdown link.
        /// </value>
        protected static IWebElement PSGFundingBreakdownLink =>
            Driver.Instance.WaitToFindElement(By.CssSelector("table a"));

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
