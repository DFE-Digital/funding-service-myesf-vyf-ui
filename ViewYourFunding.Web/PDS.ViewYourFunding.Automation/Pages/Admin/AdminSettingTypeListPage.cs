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
    /// The Admin settings type list class.
    /// </summary>
    public class AdminSettingTypeListPage : ViewYourFundingBasePage
    {
        private const string DoNotUseSettingNameAndDescription = "donotuseregressionsettingtype";

        #region Actions

        public static void NavigateToPage(string username, string password)
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();
            LoginPage.Open();

            LoginPage.Login(username, password);
            Goto("view-latest-funding/admin/home");
        }

        public static void AddSetting()
        {
            AddSettingLink
                .MoveAndClick();

            SettingDescriptionInput.SendKeys(DoNotUseSettingNameAndDescription);
            SettingNameInput.SendKeys(DoNotUseSettingNameAndDescription);

            ContinueButton.MoveAndClick();

            ContinueButton.MoveAndClick();
        }

        public static void EditSetting()
        {
            var editRow = SettingsTableRows.First(row =>
                row.Text.Contains(DoNotUseSettingNameAndDescription, StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.ClassName("edit-setting"))
                .MoveAndClick();

            SettingDescriptionInput.SendKeys("updated description");

            ContinueButton.MoveAndClick();

            ContinueButton.MoveAndClick();
        }

        public static void DeleteSetting()
        {
            var deleteRow = SettingsTableRows.First(row =>
                row.Text.Contains(DoNotUseSettingNameAndDescription, StringComparison.InvariantCultureIgnoreCase));

            deleteRow.FindElement(By.ClassName("delete-setting"))
                .MoveAndClick();

            ContinueButton.MoveAndClick();
        }

        #endregion


        #region Assertions

        public static void EnsureSettingsTable()
        {
            SettingsTable.Should().NotBeNull();
            SettingsTable.FindElements(By.TagName("tr")).Count.Should().BeGreaterThan(0);
        }

        public static void EnsureSettingOperationConfirmation()
        {
            SuccessMessage.Displayed.Should()
                .BeTrue();
        }

        public static void EnsureSettingTypesHeader()
        {
            PageH2Sections.Any(h2 =>
                    h2.Text.Contains("Setting Types", StringComparison.InvariantCultureIgnoreCase))
                .Should()
                .BeTrue();
        }

        #endregion


        #region Page Elements

        protected static IReadOnlyList<IWebElement> SettingsTableRows => SettingsTable.FindElements(By.TagName("tr"));

        protected static IReadOnlyList<IWebElement> PageH2Sections => Driver.Instance.FindElements(By.TagName("h2"));

        protected static IWebElement SettingsTable =>
            Driver.Instance.FindElement(By.ClassName("touch-table"));

        protected static IWebElement AddSettingLink =>
            Driver.Instance.FindElement(By.Id("add-setting-type-link"));

        protected static IWebElement ContinueButton => Driver.Instance.FindElement(By.ClassName("button"));

        protected static IWebElement SettingNameInput => Driver.Instance.FindElement(By.Name("SettingType.SettingName"));

        protected static IWebElement SettingDescriptionInput => Driver.Instance.FindElement(By.Name("SettingType.SettingDescription"));

        protected static IWebElement SuccessMessage => Driver.Instance.FindElement(By.ClassName("govuk-box-highlight"));

        #endregion
    }
}