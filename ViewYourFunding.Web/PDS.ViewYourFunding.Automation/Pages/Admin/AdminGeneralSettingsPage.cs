using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.Admin
{
    public class AdminGeneralSettingsPage : ViewYourFundingBasePage
    {
        private static string currentValue;

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

        public static void NavigateToGeneralSettingsPage()
        {
            AdminHomePage.ClickOnTile("General settings");
        }

        public static void EditGeneralSetting()
        {
            var editRow = GeneralSettingsTableRows.First(row =>
              row.Text.Contains("URL for internal admin view", StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.LinkText("Edit"))
               .MoveAndClick();

            currentValue = CurrentValue.Text;

            NewValueInput.SendKeys("abc/def");

            ContinueButton.MoveAndClick();
            ContinueButton.MoveAndClick();
        }

        public static void RevertGeneralSetting()
        {
            var editRow = GeneralSettingsTableRows.First(row =>
              row.Text.Contains("URL for internal admin view", StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.LinkText("Edit"))
               .MoveAndClick();

            NewValueInput.Clear();
            NewValueInput.SendKeys(currentValue);

            ContinueButton.MoveAndClick();
            ContinueButton.MoveAndClick();
        }

        #endregion

        #region Assertions

        public static void EnsurePageTitle()
        {
            PageH1Sections.Any(h1 =>
                    h1.Text.Contains("General Settings List", StringComparison.InvariantCultureIgnoreCase))
                .Should()
                .BeTrue();
        }

        public static void EnsurePageHasGeneralSettingsTable()
        {
            GeneralSettingsTable.Should().NotBeNull();
        }

        public static void EnsureGeneralSettingsOperationConfirmation()
        {
            SuccessMessage.Displayed.Should()
                .BeTrue();
        }

        #endregion


        #region Page Elements

        protected static IWebElement ContinueButton => Driver.Instance.FindElement(By.ClassName("button"));

        protected static IWebElement NewValueInput => Driver.Instance.FindElement(By.Name("NewValue"));

        protected static IWebElement CurrentValue => EditSettingsTable.FindElement(By.TagName("dd"));

        protected static IWebElement EditSettingsTable => Driver.Instance.FindElement(By.ClassName("dl-horizontal"));

        protected static IWebElement GeneralSettingsTable => Driver.Instance.FindElement(By.ClassName("touch-table"));

        protected static IReadOnlyList<IWebElement> PageH1Sections => Driver.Instance.FindElements(By.TagName("h1"));

        protected static IReadOnlyList<IWebElement> GeneralSettingsTableRows => GeneralSettingsTable.FindElements(By.TagName("tr"));

        protected static IWebElement SuccessMessage => Driver.Instance.FindElement(By.ClassName("govuk-box-highlight"));

        #endregion
    }
}
