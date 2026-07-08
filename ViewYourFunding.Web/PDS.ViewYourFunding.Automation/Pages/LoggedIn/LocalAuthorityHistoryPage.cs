using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.LoggedIn
{
    /// <summary>
    /// The Local Authority history page class.
    /// </summary>
    public class LocalAuthorityHistoryPage : ViewYourFundingBasePage
    {
        #region 1619 Navigation

        /// <summary>
        /// Navigates to page with School sixth form funding stream.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void NavigateToPageViaLogin_SchoolSixthForm(string username, string password)
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();
            LoginPage.Open();

            LoginPage.Login(username, password);

            var ukPrn = username.Substring(0, username.IndexOf(" - ", StringComparison.Ordinal));
            Goto($"/view-latest-funding/pre-16-16-19-statements/{ukPrn}/16-to-19-funding/allocation-history/la");
        }

        /// <summary>
        /// Navigates to page with 1619 funding stream.
        /// </summary>
        public static void NavigateToPageWithoutLogin_SchoolSixthForm()
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();

            Goto("view-latest-funding/pre-16-16-19-statements/10072811/16-to-19-funding/allocation-history/la");
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

        #endregion 1619 Navigation


        #region 1619 Assertions

        public static void EnsureNavigatedToLogin()
        {
            EnsureCurrentPage("Login");
        }

        /// <summary>
        /// Ensures the Information Exchange link is displayed.
        /// </summary>
        public static void EnsureInformationExchangeLinkDisplayed()
        {
            InformationExchangeLink.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Ensures the current Local Authority history page for school sixth form.
        /// </summary>
        public static void EnsureCurrentLocalAuthorityHistoryPage_SchoolSixthForm()
        {
            EnsureCurrentPage("School sixth form funding\r\nAllocation history");
        }


        /// <summary>
        /// Ensures the multiple funding breakdown links are displayed for school sixth form.
        /// </summary>
        /// <param name="dateLinkText"> The funding breaskdown dates that we wantto find.</param>
        public static void EnsureMultipleFundingBreakdownLinksDisplayed_SchoolSixthForm(IEnumerable<string> dateLinkText)
        {
            foreach (var dateToFind in dateLinkText)
            {
                Driver.Instance.FindElement(By.LinkText(dateToFind)).Displayed.Should().BeTrue();
            }
        }

        /// <summary>
        /// Click on the funding breakdown link based on the date passed in like for example "1 July 2021".
        /// </summary>
        /// <param name="dateLinkText">The date which we are interested in like  "1 July 2021".</param>
        public static void ClickOnFundingBreakdownLink_ForChosenDate(string dateLinkText)
        {
            Driver.Instance.FindElement(By.LinkText(dateLinkText)).MoveAndClick();
        }

        /// <summary>
        /// Ensures the School sixth form funding breakdown page for the latest allocation is displayed.
        /// </summary>
        public static void EnsureLatestFundingBreakdownPageDisplayed_SchoolSixthForm()
        {
            EnsureCurrentPage("School sixth form funding: 2021 to 2022\r\nThis allocation: 10 January 2021 LATEST");
        }

        /// <summary>
        /// Ensures the School sixth form funding breakdown page for the historic allocation from previous year is displayed.
        /// </summary>
        public static void EnsurePreviousYearsHistoricFundingBreakdownPageDisplayed_SchoolSixthForm()
        {
            EnsureCurrentPage("School sixth form funding: 2021 to 2022\r\nThis allocation: 10 January 2020 FINAL");
        }


        #endregion 1619 Assertions


        #region Page Elements

        /// <summary>
        /// Gets the information exchange link.
        /// </summary>
        protected static IWebElement InformationExchangeLink => Driver.Instance.FindElement(By.LinkText("Information Exchange (opens in a new tab)."));

        /// <summary>
        /// Gets the view allocation history link.
        /// </summary>
        protected static IWebElement ViewAllocationHistoryLink => Driver.Instance.FindElement(By.LinkText("View history of allocated funding"));

        /// <summary>
        /// Gets the 1619 funding breakdown link.
        /// </summary>
        protected static IWebElement FundingBreakdownLink_1619 => Driver.Instance.FindElement(By.LinkText("1 July 2021"));

        #endregion
    }
}