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
    /// The LocalAuthorityPSGBreakdown201819Page class.
    /// </summary>
    public class LocalAuthorityPSGBreakdown201819Page : ViewYourFundingBasePage
    {
        #region Private Fields

        /// <summary>
        /// The guidance links.
        /// </summary>
        private static readonly ReadOnlyCollection<string> GuidanceLinks = new ReadOnlyCollection<string>(new List<string>
        {
            "https://www.gov.uk/government/publications/pe-and-sport-premium-conditions-of-grant-2018-to-2019",
            "https://www.gov.uk/guidance/pe-and-sport-premium-for-primary-schools"
        });

        #endregion


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
            LocalAuthorityPSGHistoryPage.Click201819Link();
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
            LocalAuthorityPSGHistoryPage.Click201819Link();
        }

        #endregion


        #region Assertions

        /// <summary>
        /// Ensures the current page.
        /// </summary>
        public static void EnsureCurrentPage()
        {
            EnsureCurrentPage("PE and sport premium 2018 to 2019", true);
            EnsureCurrentPage("NOT LATEST", true);
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
        /// Ensures the guidance links.
        /// </summary>
        public static void EnsureGuidanceLinks()
        {
            var pageLinks = Driver.Instance.WaitToFindElements(By.TagName("a"));
            var pageLinksWithHref =
                pageLinks.Where(pageLink => !string.IsNullOrWhiteSpace(pageLink.GetAttribute("href"))).ToList();

            foreach (var link in GuidanceLinks)
            {
                pageLinksWithHref.Any(pageLink => pageLink.GetAttribute("href").Contains(link)).Should().BeTrue();
            }
        }

        /// <summary>
        /// Ensures the download links available.
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
        protected static ReadOnlyCollection<IWebElement> DownloadDocumentLinks => Driver.Instance.WaitToFindElements(By.LinkText("Download PE and sport premium allocations 2018 to 2019"));

        #endregion

    }
}
