using FluentAssertions;
using OpenQA.Selenium;
using System;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.LoggedIn
{
    /// <summary>
    /// The provider page class.
    /// </summary>
    public class ProviderHistoryPage : ViewYourFundingBasePage
    {
        #region GAG Navigation

        /// <summary>
        /// Navigates to page with GAG funding stream.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void NavigateToPageViaLogin_GAG(string username, string password)
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();
            LoginPage.Open();

            LoginPage.Login(username, password);

            var ukPrn = username.Substring(0, username.IndexOf(" - ", StringComparison.Ordinal));
            Goto($"/view-latest-funding/pre-16-16-19-statements/{ukPrn}/general-annual-grant/allocation-history");
        }

        /// <summary>
        /// Navigates to page with GAG funding stream.
        /// </summary>
        public static void NavigateToPageWithoutLogin_GAG()
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();

            Goto("view-latest-funding/pre-16-16-19-statements/10072811/general-annual-grant/allocation-history");
        }

        /// <summary>
        /// Click on the Information Exchange link.
        /// </summary>
        public static void ClickOnInformationExchangeWebLink()
        {
            InformationExchangeLink.MoveAndClick();
        }

        /// <summary>
        /// Click on the view allocation history link.
        /// </summary>
        public static void ClickOnViewAllocationHistoryLink()
        {
            ViewAllocationHistoryLink.MoveAndClick();
        }

        #endregion GAG Navigation


        #region PSG Navigation

        /// <summary>
        /// Navigates to page with PSG funding stream.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void NavigateToPageViaLogin_PSG(string username, string password)
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();
            LoginPage.Open();

            LoginPage.Login(username, password);
            Goto("view-latest-funding/pre-16-16-19-statements/10072811/pe-and-sport-premium/allocation-history");
        }

        /// <summary>
        /// Navigates to page with PSG funding stream.
        /// </summary>
        public static void NavigateToPageWithoutLogin_PSG()
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();

            Goto("view-latest-funding/pre-16-16-19-statements/10072811/pe-and-sport-premium/allocation-history");
        }

        /// <summary>
        /// Navigates to page with PSG funding stream.
        /// </summary>
        public static void NavigateToPageDirectly_PSG()
        {
            Goto("view-latest-funding/pre-16-16-19-statements/10072811/pe-and-sport-premium/allocation-history");
        }

        /// <summary>
        /// Click on the funding breakdown link for PSG.
        /// </summary>
        public static void ClickOnFundingBreakdownWebLink_PSG()
        {
            FundingBreakdownLink_PSG.MoveAndClick();
        }

        #endregion PSG Navigation


        #region 1619 Navigation

        /// <summary>
        /// Navigates to page with 1619 funding stream.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void NavigateToPageViaLogin_1619(string username, string password)
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();
            LoginPage.Open();

            LoginPage.Login(username, password);

            var ukPrn = username.Substring(0, username.IndexOf(" - ", StringComparison.Ordinal));
            Goto($"/view-latest-funding/pre-16-16-19-statements/{ukPrn}/16-to-19-funding/allocation-history");
        }

        /// <summary>
        /// Navigates to page with 1619 funding stream.
        /// </summary>
        public static void NavigateToPageWithoutLogin_1619()
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();

            Goto("view-latest-funding/pre-16-16-19-statements/10072811/16-to-19-funding/allocation-history");
        }

        #endregion 1619 Navigation


        #region 1416 Navigation

        /// <summary>
        /// Navigates to page with 1416 funding stream.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void NavigateToPageViaLogin_1416(string username, string password)
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();
            LoginPage.Open();

            LoginPage.Login(username, password);

            var ukPrn = username.Substring(0, username.IndexOf(" - ", StringComparison.Ordinal));
            Goto($"/view-latest-funding/pre-16-16-19-statements/{ukPrn}/14-to-16-funding/allocation-history");
        }

        /// <summary>
        /// Navigates to page with 1619 funding stream.
        /// </summary>
        public static void NavigateToPageWithoutLogin_1416()
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();

            Goto("view-latest-funding/pre-16-16-19-statements/10072811/14-to-16-funding/allocation-history");
        }

        #endregion


        #region NMSS Navigation

        /// <summary>
        /// Navigates to page with NMSS funding stream.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void NavigateToPageViaLogin_NMSS(string username, string password)
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();
            LoginPage.Open();

            LoginPage.Login(username, password);

            var ukPrn = username.Substring(0, username.IndexOf(" - ", StringComparison.Ordinal));
            Goto($"/view-latest-funding/pre-16-16-19-statements/{ukPrn}/non-maintained-special-school-funding/allocation-history");
        }

        /// <summary>
        /// Navigates to page with NMSS funding stream.
        /// </summary>
        public static void NavigateToPageWithoutLogin_NMSS()
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();

            Goto("view-latest-funding/pre-16-16-19-statements/10015031/non-maintained-special-school-funding/allocation-history");
        }

        #endregion


        #region GAG Assertions

        public static void EnsureNavigatedToLogin()
        {
            EnsureCurrentPage("Login");
        }

        /// <summary>
        /// Ensures the current provider history page for GAG.
        /// </summary>
        public static void EnsureCurrentProviderHistoryPage_GAG()
        {
            EnsureCurrentPage("General annual grant\r\nAllocation history");
        }


        /// <summary>
        /// Ensures the previous years header is displayed for GAG.
        /// </summary>
        public static void EnsurePreviousYearsHeader_GAG()
        {
            EnsureCurrentPage("General annual grant\r\nAllocation history");
        }


        /// <summary>
        /// Ensures the funding breakdown link is displayed for GAG.
        /// </summary>
        public static void EnsureFundingBreakdownLinkDisplayed_GAG()
        {
            FundingBreakdownLink_GAG.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Ensures the multiple funding breakdown links are displayed for GAG.
        /// </summary>
        public static void EnsureMultipleFundingBreakdownLinksDisplayed_GAG()
        {
            FundingBreakdownMultipleLink1_GAG.Displayed.Should().BeTrue();
            FundingBreakdownMultipleLink2_GAG.Displayed.Should().BeTrue();
            FundingBreakdownMultipleLink3_GAG.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Click on the funding breakdown link for GAG.
        /// </summary>
        public static void ClickOnFundingBreakdownWebLink_GAG()
        {
            FundingBreakdownLink_GAG.MoveAndClick();
        }

        /// <summary>
        /// Click on the latest of the multiple funding breakdown link for GAG.
        /// </summary>
        public static void ClickOnLatestFundingBreakdownLink_GAG()
        {
            FundingBreakdownMultipleLink1_GAG.MoveAndClick();
        }

        /// <summary>
        /// Click on the non-latest (historic) of the multiple funding breakdown link for GAG.
        /// </summary>
        public static void ClickOnHistoricFundingBreakdownWebLink_GAG()
        {
            FundingBreakdownMultipleLink2_GAG.MoveAndClick();
        }

        /// <summary>
        /// Click on the final of the multiple funding breakdown across years link for GAG.
        /// </summary>
        public static void ClickOnFinalFundingBreakdownLink_GAG()
        {
            FundingBreakdownMultipleLink4_GAG.MoveAndClick();
        }

        /// <summary>
        /// Click on the non-latest (historic from previous year) of the multiple funding breakdown link for GAG.
        /// </summary>
        public static void ClickOnHistoricFromPreviousYearFundingBreakdownWebLink_GAG()
        {
            FundingBreakdownMultipleLink5_GAG.MoveAndClick();
        }

        /// <summary>
        /// Ensures the GAG funding breakdown page is displayed.
        /// </summary>
        public static void EnsureFundingBreakdownPage_GAG()
        {
            EnsureCurrentPage("General annual grant: 2023 to 2024\r\nThis allocation: 1 June 2023 LATEST");
        }

        /// <summary>
        /// Ensures the GAG funding breakdown page for the latest allocation is displayed.
        /// </summary>
        public static void EnsureLatestFundingBreakdownPageDisplayed_GAG()
        {
            EnsureCurrentPage("General annual grant: 2021 to 2022\r\nThis allocation: 12 June 2021 LATEST");
        }

        /// <summary>
        /// Ensures the GAG funding breakdown page for the historic allocation is displayed.
        /// </summary>
        public static void EnsureHistoricFundingBreakdownPageDisplayed_GAG()
        {
            EnsureCurrentPage("General annual grant: 2021 to 2022\r\nThis allocation: 6 June 2021 NOT LATEST");
        }

        /// <summary>
        /// Ensures the GAG funding breakdown page for the final allocation is displayed.
        /// </summary>
        public static void EnsureFinalFundingBreakdownPageDisplayed_GAG()
        {
            EnsureCurrentPage("General annual grant: 2020 to 2021\r\nThis allocation: 12 June 2020 FINAL");
        }

        /// <summary>
        /// Ensures the GAG funding breakdown page for the historic allocation from previous year is displayed.
        /// </summary>
        public static void EnsurePreviousYearsHistoricFundingBreakdownPageDisplayed_GAG()
        {
            EnsureCurrentPage("General annual grant: 2020 to 2021\r\nThis allocation: 6 June 2020 NOT LATEST");
        }

        /// <summary>
        /// Ensures the Information Exchange link is displayed.
        /// </summary>
        public static void EnsureInformationExchangeLinkDisplayed()
        {
            InformationExchangeLink.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Ensures the Document Exchange link is displayed.
        /// </summary>
        public static void EnsureDocumentExchangeLinkDisplayed()
        {
            DocumentExchangeLink.Displayed.Should().BeTrue();
        }


        #endregion GAG Assertions


        #region PSG Assertions

        /// <summary>
        /// Ensures the current provider history page for PSG.
        /// </summary>
        public static void EnsureCurrentProviderHistoryPage_PSG()
        {
            EnsureCurrentPage("PE and sport premium\r\nAllocation history");
        }

        /// <summary>
        /// Ensures the previous years header is displayed for PSG.
        /// </summary>
        public static void EnsurePreviousYearsHeader_PSG()
        {
            EnsureCurrentPage("PE and sport premium\r\nAllocation history");
        }

        /// <summary>
        /// Ensures the funding breakdown link is displayed for PSG.
        /// </summary>
        public static void EnsureFundingBreakdownLinkDisplayed_PSG()
        {
            FundingBreakdownLink_PSG.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Ensures the funding breakdown page is displayed for PSG.
        /// </summary>
        public static void EnsureFundingBreakdownPage_PSG()
        {
            EnsureCurrentPage("PE and sport premium 2019 to 2020", true);
        }

        #endregion PSG Assertions


        #region 1619 Assertions

        /// <summary>
        /// Ensures the current provider history page for 1619.
        /// </summary>
        public static void EnsureCurrentProviderHistoryPage_1619()
        {
            EnsureCurrentPage("16 to 19 funding\r\nAllocation history");
        }

        /// <summary>
        /// Ensures the funding breakdown link is displayed for 1619.
        /// </summary>
        public static void EnsureFundingBreakdownLinkDisplayed_1619()
        {
            FundingBreakdownLink_1619.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Ensures the multiple funding breakdown links are displayed for 1619.
        /// </summary>
        public static void EnsureMultipleFundingBreakdownLinksDisplayed_1619()
        {
            FundingBreakdownMultipleLink1_1619.Displayed.Should().BeTrue();
            FundingBreakdownMultipleLink2_1619.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Click on the funding breakdown link for 1619.
        /// </summary>
        public static void ClickOnFundingBreakdownWebLink_1619()
        {
            FundingBreakdownLink_1619.MoveAndClick();
        }

        /// <summary>
        /// Click on the latest of the multiple funding breakdown link for 1619.
        /// </summary>
        public static void ClickOnLatestFundingBreakdownLink_1619()
        {
            FundingBreakdownMultipleLink1_1619.MoveAndClick();
        }

        /// <summary>
        /// Click on the non-latest (historic) of the multiple funding breakdown link for 1619.
        /// </summary>
        public static void ClickOnHistoricFundingBreakdownWebLink_1619()
        {
            FundingBreakdownMultipleLink2_1619.MoveAndClick();
        }

        /// <summary>
        /// Click on the final of the multiple funding breakdown across years link for 1619.
        /// </summary>
        public static void ClickOnFinalFundingBreakdownLink_1619()
        {
            FundingBreakdownMultipleLink3_1619.MoveAndClick();
        }

        /// <summary>
        /// Click on the non-latest (historic from previous year) of the multiple funding breakdown link for 1619.
        /// </summary>
        public static void ClickOnHistoricFromPreviousYearFundingBreakdownWebLink_1619()
        {
            FundingBreakdownMultipleLink4_1619.MoveAndClick();
        }

        /// <summary>
        /// Ensures the 1619 funding breakdown page is displayed.
        /// </summary>
        public static void EnsureFundingBreakdownPage_1619()
        {
            EnsureCurrentPage("16 to 19 funding: 2021 to 2022\r\nThis allocation: 1 July 2021 LATEST");
        }

        /// <summary>
        /// Ensures the 1619 funding breakdown page for the latest allocation is displayed.
        /// </summary>
        public static void EnsureLatestFundingBreakdownPageDisplayed_1619()
        {
            EnsureCurrentPage("16 to 19 funding: 2021 to 2022\r\nThis allocation: 1 July 2021 LATEST");
        }

        /// <summary>
        /// Ensures the 1619 funding breakdown page for the historic allocation is displayed.
        /// </summary>
        public static void EnsureHistoricFundingBreakdownPageDisplayed_1619()
        {
            EnsureCurrentPage("16 to 19 funding: 2021 to 2022\r\nThis allocation: 1 January 2021 NOT LATEST");
        }

        /// <summary>
        /// Ensures the 1619 funding breakdown page for the final allocation is displayed.
        /// </summary>
        public static void EnsureFinalFundingBreakdownPageDisplayed_1619()
        {
            EnsureCurrentPage("16 to 19 funding: 2020 to 2021\r\nThis allocation: 1 June 2020 FINAL");
        }

        /// <summary>
        /// Ensures the 1619 funding breakdown page for the historic allocation from previous year is displayed.
        /// </summary>
        public static void EnsurePreviousYearsHistoricFundingBreakdownPageDisplayed_1619()
        {
            EnsureCurrentPage("16 to 19 funding: 2020 to 2021\r\nThis allocation: 1 February 2020 NOT LATEST");
        }

        #endregion 1619 Assertions


        #region 1416 Assertions

        /// <summary>
        /// Ensures the current provider history page for 1416.
        /// </summary>
        public static void EnsureCurrentProviderHistoryPage_1416()
        {
            EnsureCurrentPage("14 to 16 funding\r\nAllocation history");
        }

        /// <summary>
        /// Ensures the funding breakdown link is displayed for 1416.
        /// </summary>
        public static void EnsureFundingBreakdownLinkDisplayed_1416()
        {
            FundingBreakdownLink_1416.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Click on the funding breakdown link for 1416.
        /// </summary>
        public static void ClickOnFundingBreakdownWebLink_1416()
        {
            FundingBreakdownLink_1416.MoveAndClick();
        }

        /// <summary>
        /// Ensures the 1416 funding breakdown page is displayed.
        /// </summary>
        public static void EnsureFundingBreakdownPage_1416()
        {
            EnsureCurrentPage("14 to 16 funding: 2021 to 2022\r\nThis allocation: 1 September 2021 LATEST");
        }

        #endregion 1416 Assertions


        #region NMSS Assertions

        /// <summary>
        /// Ensures the current provider history page for NMSS.
        /// </summary>
        public static void EnsureCurrentProviderHistoryPage_NMSS()
        {
            EnsureCurrentPage("Non maintained special school funding\r\nAllocation history");
        }

        /// <summary>
        /// Ensures the funding breakdown link is displayed for NMSS.
        /// </summary>
        public static void EnsureFundingBreakdownLinkDisplayed_NMSS()
        {
            FundingBreakdownLink_NMSS.Displayed.Should().BeTrue();
        }

        #endregion NMSS Assertions


        #region Page Elements

        /// <summary>
        /// Gets the GAG funding breakdown link.
        /// </summary>
        protected static IWebElement FundingBreakdownLink_GAG => Driver.Instance.FindElement(By.LinkText("1 June 2023"));

        /// <summary>
        /// Gets the PSG funding breakdown link.
        /// </summary>
        protected static IWebElement FundingBreakdownLink_PSG => Driver.Instance.FindElement(By.LinkText("23 April 2020"));

        /// <summary>
        /// Gets the information exchange link.
        /// </summary>
        protected static IWebElement InformationExchangeLink => Driver.Instance.FindElement(By.LinkText("Information Exchange (opens in a new tab)."));

        /// <summary>
        /// Gets the view allocation history link.
        /// </summary>
        protected static IWebElement ViewAllocationHistoryLink => Driver.Instance.FindElement(By.LinkText("View history of allocated funding"));

        /// <summary>
        /// Gets the first breakdown page link.
        /// </summary>
        protected static IWebElement FundingBreakdownMultipleLink1_GAG => Driver.Instance.FindElement(By.LinkText("12 June 2021"));

        /// <summary>
        /// Gets the second breakdown page link.
        /// </summary>
        protected static IWebElement FundingBreakdownMultipleLink2_GAG => Driver.Instance.FindElement(By.LinkText("6 June 2021"));

        /// <summary>
        /// Gets the third breakdown page link.
        /// </summary>
        protected static IWebElement FundingBreakdownMultipleLink3_GAG => Driver.Instance.FindElement(By.LinkText("1 June 2021"));

        /// <summary>
        /// Gets the first breakdown page link for previous year.
        /// </summary>
        protected static IWebElement FundingBreakdownMultipleLink4_GAG => Driver.Instance.FindElement(By.LinkText("12 June 2020"));

        /// <summary>
        /// Gets the second breakdown page link for previous year.
        /// </summary>
        protected static IWebElement FundingBreakdownMultipleLink5_GAG => Driver.Instance.FindElement(By.LinkText("6 June 2020"));

        /// <summary>
        /// Gets the third breakdown page link for previous year.
        /// </summary>
        protected static IWebElement FundingBreakdownMultipleLink6_GAG => Driver.Instance.FindElement(By.LinkText("1 June 2020"));

        /// <summary>
        /// Gets the 1619 funding breakdown link.
        /// </summary>
        protected static IWebElement FundingBreakdownLink_1619 => Driver.Instance.FindElement(By.LinkText("1 July 2021"));

        /// <summary>
        /// Gets the first breakdown page link.
        /// </summary>
        protected static IWebElement FundingBreakdownMultipleLink1_1619 => Driver.Instance.FindElement(By.LinkText("1 July 2021"));

        /// <summary>
        /// Gets the second breakdown page link.
        /// </summary>
        protected static IWebElement FundingBreakdownMultipleLink2_1619 => Driver.Instance.FindElement(By.LinkText("1 January 2021"));

        /// <summary>
        /// Gets the first breakdown page link for previous year.
        /// </summary>
        protected static IWebElement FundingBreakdownMultipleLink3_1619 => Driver.Instance.FindElement(By.LinkText("1 June 2020"));

        /// <summary>
        /// Gets the second breakdown page link for previous year.
        /// </summary>
        protected static IWebElement FundingBreakdownMultipleLink4_1619 => Driver.Instance.FindElement(By.LinkText("1 February 2020"));

        /// <summary>
        /// Gets the 1416 funding breakdown link.
        /// </summary>
        protected static IWebElement FundingBreakdownLink_1416 => Driver.Instance.FindElement(By.LinkText("1 September 2021"));

        /// <summary>
        /// Gets the NMSS funding breakdown link.
        /// </summary>
        protected static IWebElement FundingBreakdownLink_NMSS => Driver.Instance.FindElement(By.LinkText("10 January 2021"));

        /// <summary>
        /// Gets the document exchange link.
        /// </summary>
        protected static IWebElement DocumentExchangeLink => Driver.Instance.FindElement(By.LinkText("Document Exchange (opens in new tab)."));

        #endregion
    }
}