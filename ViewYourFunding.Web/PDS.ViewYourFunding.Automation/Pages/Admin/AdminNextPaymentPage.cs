using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.Admin
{
    /// <summary>
    /// The Admin Next Payment Page class.
    /// </summary>
    public class AdminNextPaymentPage : ViewYourFundingBasePage
    {
        #region Private fields

        private const string DoNotUseFundingPeriodCode = "RT-2021";

        #endregion


        #region Actions

        /// <summary>
        /// Navigates to page.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void NavigateToPage(string username, string password)
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();
            LoginPage.Open();

            LoginPage.Login(username, password);
            Goto("view-latest-funding/admin/home");
        }

        public static void NavigateToNextPaymentPage()
        {
            AdminHomePage.ClickOnTile("Funding streams settings");
            AdminFundingStreamListPage.SelectFundingStream("PE and sport premium");
            NextPaymentLink.MoveAndClick();
        }

        public static void AddNextPayment()
        {
            AddNextPaymentLink.MoveAndClick();
            NextPaymentDateDayInput.SendKeys("12");
            NextPaymentDateMonthInput.SendKeys("12");
            NextPaymentDateYearInput.SendKeys("2023");
            FundingPeriodCodeInput.SendKeys(DoNotUseFundingPeriodCode);
            ContinueButton.MoveAndClick();
            ContinueButton.MoveAndClick();
        }

        public static void EditNextPayment()
        {
            var editRow = NextPaymentTableRows.First(row =>
               row.Text.Contains(DoNotUseFundingPeriodCode, StringComparison.InvariantCultureIgnoreCase));
            editRow.FindElements(By.LinkText("Edit")).First().MoveAndClick();

            NextPaymentDateDayInput.Clear();
            NextPaymentDateMonthInput.Clear();
            NextPaymentDateYearInput.Clear();
            NextPaymentDateDayInput.SendKeys("11");
            NextPaymentDateMonthInput.SendKeys("11");
            NextPaymentDateYearInput.SendKeys("2025");
            ContinueButton.MoveAndClick();
            ContinueButton.MoveAndClick();
        }

        public static void DeleteNextPayment()
        {
            var editRow = NextPaymentTableRows.First(row =>
               row.Text.Contains(DoNotUseFundingPeriodCode, StringComparison.InvariantCultureIgnoreCase));
            editRow.FindElements(By.LinkText("Delete")).First().MoveAndClick();

            ContinueButton.MoveAndClick();
        }

        #endregion


        #region Assertions
        public static void EnsureNextPaymentOperationConfirmation()
        {
            SuccessMessage.Displayed.Should()
                .BeTrue();
        }

        #endregion


        #region Page Elements

        protected static IWebElement NextPaymentTable => Driver.Instance.FindElement(By.ClassName("touch-table"));

        protected static IReadOnlyList<IWebElement> NextPaymentTableRows => NextPaymentTable.FindElements(By.TagName("tr"));

        protected static IWebElement NextPaymentLink => Driver.Instance.FindElement(By.LinkText("Next Payments"));

        protected static IWebElement AddNextPaymentLink => Driver.Instance.FindElement(By.LinkText("Add a Next Payment"));

        protected static IWebElement ContinueButton => Driver.Instance.FindElement(By.ClassName("button"));

        protected static IWebElement SuccessMessage => Driver.Instance.FindElement(By.ClassName("govuk-box-highlight"));

        protected static IWebElement NextPaymentDateDayInput => Driver.Instance.FindElement(By.Name("NextPayment.NextPaymentDateDay"));

        protected static IWebElement NextPaymentDateMonthInput => Driver.Instance.FindElement(By.Name("NextPayment.NextPaymentDateMonth"));

        protected static IWebElement NextPaymentDateYearInput => Driver.Instance.FindElement(By.Name("NextPayment.NextPaymentDateYear"));

        protected static IWebElement FundingPeriodCodeInput => Driver.Instance.FindElement(By.Name("NextPayment.FundingPeriodCode"));

        #endregion
    }
}