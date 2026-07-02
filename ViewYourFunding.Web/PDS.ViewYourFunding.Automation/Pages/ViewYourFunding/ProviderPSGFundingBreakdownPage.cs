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
    /// The ProviderPSGFundingBreakdownPage class.
    /// </summary>
    public class ProviderPSGFundingBreakdownPage : ViewYourFundingBasePage
    {
        #region Private fields

        /// <summary>
        /// The provider name.
        /// </summary>
        private const string ProviderName = "St Mary's Kilburn Church of England Primary School";

        /// <summary>
        /// The page title starts with.
        /// </summary>
        private const string PageTitleStartsWith = "PE and sport premium 2019 to 2020\r\nThis allocation: ";

        /// <summary>
        /// The page title ends with.
        /// </summary>
        private const string PageTitleEndsWith = " LATEST";

        /// <summary>
        /// The gov uk links.
        /// </summary>
        private static readonly ReadOnlyCollection<string> GovUKLinks = new ReadOnlyCollection<string>(new List<string>
        {
            "https://www.gov.uk/government/publications/pe-and-sport-premium-conditions-of-grant-2019-to-2020",
            "https://www.gov.uk/guidance/pe-and-sport-premium-for-primary-schools"
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
            ProviderPSGAllocationHistoryPage.NavigateToPsgFundingBreakdownPage();
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
            ProviderPSGAllocationHistoryPage.NavigateToPsgFundingBreakdownPage();
        }

        #endregion


        #region Assertions

        /// <summary>
        /// Ensures the current provider page.
        /// </summary>
        public static void EnsureCurrentProviderPage()
        {
            EnsureCurrentPage(PageTitleStartsWith, true);
            EnsureCurrentPage(PageTitleEndsWith, true);
        }

        /// <summary>
        /// Ensures the allocation history link.
        /// </summary>
        public static void EnsureAllocationHistoryLink()
        {
            AllocationHistoryLink.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Ensures the total funding summary.
        /// </summary>
        public static void EnsureTotalFundingSummary()
        {
            TotalFundingSummary.Text.Should().NotBeNullOrWhiteSpace();
        }

        /// <summary>
        /// Ensures the download spreadsheet link.
        /// </summary>
        public static void EnsureDownloadSpreadsheetLink()
        {
            DownloadSpreadsheetLink.Displayed.Should().BeTrue();
            DownloadSpreadsheetLink.Text.Should()
                .Contain($"Download PE and sport premium allocation for {ProviderName} 2019 to 2020");
        }

        /// <summary>
        /// Ensures the funding table row columns.
        /// </summary>
        public static void EnsureFundingTableRowColumns()
        {
            ProviderDetailsRowColumns.Any(column => string.IsNullOrWhiteSpace(column.Text)).Should().BeFalse();
        }

        /// <summary>
        /// Ensures the gov uk links.
        /// </summary>
        public static void EnsureGovUkLinks()
        {
            var pageLinks = Driver.Instance.WaitToFindElements(By.TagName("a"));

            var pageLinksWithHref =
                pageLinks.Where(pageLink => !string.IsNullOrWhiteSpace(pageLink.GetAttribute("href"))).ToList();

            foreach (var govUkLink in GovUKLinks)
            {
                pageLinksWithHref.Any(pageLink => pageLink.GetAttribute("href").Contains(govUkLink)).Should().BeTrue();
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
        /// Gets the allocation history link.
        /// </summary>
        /// <value>
        /// The allocation history link.
        /// </value>
        protected static IWebElement AllocationHistoryLink => Driver.Instance.WaitToFindElement(By.LinkText("allocation history"));

        /// <summary>
        /// Gets the total funding summary.
        /// </summary>
        /// <value>
        /// The total funding summary.
        /// </value>
        protected static IWebElement TotalFundingSummary => Driver.Instance.WaitToFindElement(By.ClassName("heading-funding-summary"));

        /// <summary>
        /// Gets the provider details row columns.
        /// </summary>
        /// <value>
        /// The provider details row columns.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> ProviderDetailsRowColumns => Driver.Instance.WaitToFindElements(By.CssSelector(".document-list table tbody tr td"));

        /// <summary>
        /// Gets the download spreadsheet link.
        /// </summary>
        /// <value>
        /// The download spreadsheet link.
        /// </value>
        protected static IWebElement DownloadSpreadsheetLink => Driver.Instance.WaitToFindElement(By.ClassName("download-a-document"));

        #endregion
    }
}
