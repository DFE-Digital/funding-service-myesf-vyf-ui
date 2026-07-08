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
    /// The Admin Next Payment Type Page class.
    /// </summary>
    public class AdminNextPaymentTypePage : ViewYourFundingBasePage
    {
        #region Private fields

        private const string DoNotUseRegressionNextPaymentTypeDescription = "donotuseregressionextpaymenttype";
        private const string DoNotUseRegressionNextPaymentTypeCode = "RTXXX";
        private const string DoNotUseRegressionUpdatedNextPaymentTypeDescription = "donotuseregressionupdatednextpaymenttype";

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

        public static void NavigateToNextPaymentTypePage()
        {
            AdminHomePage.ClickOnTile("Funding streams settings");
            AdminFundingStreamListPage.SelectFundingStream("PE and sport premium");
            NextPaymentTypeLink.MoveAndClick();
        }

        public static void AddNextPaymentType()
        {
            AddNextPaymentTypeLink.MoveAndClick();
            NextPaymentTypeCode.SendKeys(DoNotUseRegressionNextPaymentTypeCode);
            NextPaymentTypeDescription.SendKeys(DoNotUseRegressionNextPaymentTypeDescription);
            ContinueButton.MoveAndClick();
            ContinueButton.MoveAndClick();
        }

        public static void EditNextPaymentType()
        {
            var editRow = NextPaymentTypeTableRows.First(row =>
               row.Text.Contains(DoNotUseRegressionNextPaymentTypeCode, StringComparison.InvariantCultureIgnoreCase));
            editRow.FindElements(By.TagName("a")).First().MoveAndClick();

            NextPaymentTypeDescription.SendKeys(DoNotUseRegressionUpdatedNextPaymentTypeDescription);
            ContinueButton.MoveAndClick();
            ContinueButton.MoveAndClick();
        }

        public static void DeleteNextPaymentType()
        {
            var editRow = NextPaymentTypeTableRows.First(row =>
               row.Text.Contains(DoNotUseRegressionNextPaymentTypeCode, StringComparison.InvariantCultureIgnoreCase));
            editRow.FindElements(By.TagName("a"))[1].MoveAndClick();

            ContinueButton.MoveAndClick();
        }


        #endregion


        #region Assertions
        public static void EnsureNextPaymentTypeOperationConfirmation()
        {
            SuccessMessage.Displayed.Should()
                .BeTrue();
        }

        #endregion


        #region Page Elements

        protected static IWebElement NextPaymentTypeTable => Driver.Instance.FindElement(By.ClassName("touch-table"));

        protected static IReadOnlyList<IWebElement> NextPaymentTypeTableRows => NextPaymentTypeTable.FindElements(By.TagName("tr"));

        protected static IWebElement NextPaymentTypeLink => Driver.Instance.FindElement(By.LinkText("Next Payment Types"));

        protected static IWebElement AddNextPaymentTypeLink => Driver.Instance.FindElement(By.LinkText("Add a Next Payment Type"));

        protected static IWebElement NextPaymentTypeCode => Driver.Instance.FindElement(By.Name("NextPaymentType.TypeCode"));

        protected static IWebElement NextPaymentTypeDescription => Driver.Instance.FindElement(By.Name("NextPaymentType.Description"));

        protected static IWebElement ContinueButton => Driver.Instance.FindElement(By.ClassName("button"));

        protected static IWebElement SuccessMessage => Driver.Instance.FindElement(By.ClassName("govuk-box-highlight"));

        #endregion
    }
}