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
    /// The ProviderPSGFundingBreakdown201819Page class.
    /// </summary>
    public class ProviderPSGFundingBreakdown201819Page : ViewYourFundingBasePage
    {
        #region Private Fields

        /// <summary>
        /// The provider name.
        /// </summary>
        private const string ProviderName = "St Mary's Kilburn Church of England Primary School";

        /// <summary>
        /// The page title.
        /// </summary>
        private const string PageTitle = "PE and sport premium 2018 to 2019\r\nNOT LATEST";

        /// <summary>
        /// The gov uk links.
        /// </summary>
        private static readonly ReadOnlyCollection<string> GovUKLinks = new ReadOnlyCollection<string>(new List<string>
        {
            "https://www.gov.uk/government/publications/pe-and-sport-premium-conditions-of-grant-2018-to-2019",
            "https://www.gov.uk/guidance/pe-and-sport-premium-for-primary-schools"
        });

        /// <summary>
        /// The spreadsheet links.
        /// </summary>
        private static readonly ReadOnlyCollection<string> SpreadsheetLinks = new ReadOnlyCollection<string>(new List<string>
        {
            "/DownloadSpreadsheet/PSG_AY-1819",
            "/DownloadSpreadsheet/PSG_AY-1920"
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
            ProviderPSGAllocationHistoryPage.NavigateToPSGFundingBreakdownPage2018();
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
            ProviderPSGAllocationHistoryPage.NavigateToPSGFundingBreakdownPage2018();
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
        /// Ensures the allocation history link.
        /// </summary>
        public static void EnsureAllocationHistoryLink()
        {
            AllocationHistoryLink.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Ensures the main paragraph.
        /// </summary>
        public static void EnsureMainParagraph()
        {
            MainPageParagraph.Text.Should()
                .Contain("Find out about PE and sport premium funding allocations for academic year 2018 to 2019.");
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

        /// <summary>
        /// Ensures the download links are available.
        /// </summary>
        public static void EnsureDownloadLinksAvailable()
        {
            if (DownloadDocumentLinks.Any())
            {
                DownloadDocumentLinks.Count.Should().Be(3);
                foreach (var downloadDocumentLink in DownloadDocumentLinks)
                {
                    var hrefValue = downloadDocumentLink.GetAttribute("href");
                    hrefValue.Should().NotBeNullOrEmpty();
                    SpreadsheetLinks.Any(link => hrefValue.Contains(link)).Should().BeTrue();
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
        /// Gets the allocation history link.
        /// </summary>
        /// <value>
        /// The allocation history link.
        /// </value>
        protected static IWebElement AllocationHistoryLink => Driver.Instance.WaitToFindElement(By.LinkText("allocation history"));

        /// <summary>
        /// Gets the download document links.
        /// </summary>
        /// <value>
        /// The download document links.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> DownloadDocumentLinks => Driver.Instance.WaitToFindElements(By.PartialLinkText("Download PE and sport premium allocations"));

        #endregion

    }
}