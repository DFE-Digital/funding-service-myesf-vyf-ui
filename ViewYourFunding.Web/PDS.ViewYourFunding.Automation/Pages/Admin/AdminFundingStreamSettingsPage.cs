using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.Admin
{
    /// <summary>
    /// The Admin Funding stream settings page.
    /// </summary>
    public class AdminFundingStreamSettingsPage : ViewYourFundingBasePage
    {
        private const string DoNotUseSettingName = "donotuseregressionsetting";

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

        public static void NavigateToFundingStreamSettingsPage()
        {
            AdminHomePage.ClickOnTile("Funding streams settings");
            AdminFundingStreamListPage.SelectFundingStream("PE and sport premium");
        }

        public static void AddFundingStreamSetting()
        {
            AddFundingStreamLink
                .MoveAndClick();

            SettingTypeDropDown.SelectByText(DoNotUseSettingName);

            ContinueButton.MoveAndClick();

            NewValueInput.SendKeys("new setting value");

            ContinueButton.MoveAndClick();

            ContinueButton.MoveAndClick();
        }

        public static void EditFundingStreamSetting()
        {
            var editRow = FundingStreamSettingsTableRows.First(row =>
                row.Text.Contains(DoNotUseSettingName, StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.ClassName("edit-fundingstream-setting"))
                .MoveAndClick();

            NewValueInput.SendKeys("updated setting value");

            ContinueButton.MoveAndClick();

            ContinueButton.MoveAndClick();
        }

        public static void DeleteFundingStreamSetting()
        {
            var deleteRow = FundingStreamSettingsTableRows.First(row =>
                row.Text.Contains(DoNotUseSettingName, StringComparison.InvariantCultureIgnoreCase));

            deleteRow.FindElement(By.ClassName("delete-fundingstream-setting"))
                .MoveAndClick();

            ContinueButton.MoveAndClick();
        }

        #endregion


        #region Assertions

        public static void EnsureFundingStreamSettingsTable()
        {
            FundingStreamSettingsTable.Should().NotBeNull();
        }

        public static void EnsureNextPaymentTypesOption()
        {
            AnchorElements.Any(anchor =>
                    anchor.Text.Contains("Next Payment Types", StringComparison.InvariantCultureIgnoreCase))
                .Should()
                .BeTrue();
        }

        public static void EnsureSettingOperationConfirmation()
        {
            SuccessMessage.Displayed.Should()
                .BeTrue();
        }

        public static void EnsureNextPaymentsOption()
        {
            AnchorElements.Any(anchor =>
                    anchor.Text.Contains("Next Payments", StringComparison.InvariantCultureIgnoreCase))
                .Should()
                .BeTrue();
        }

        public static void EnsurePublicationsOption()
        {
            PageH2Sections.Any(h2 =>
                    h2.Text.Contains("Publications", StringComparison.InvariantCultureIgnoreCase))
                .Should()
                .BeTrue();
            PublicationSettingsTable.FindElements(By.TagName("tr")).Count.Should().BeGreaterThan(0);
        }

        #endregion


        #region Page Elements

        protected static IReadOnlyList<IWebElement> FundingStreamSettingsTableRows => FundingStreamSettingsTable.FindElements(By.TagName("tr"));

        protected static IReadOnlyList<IWebElement> AdminTiles => Driver.Instance.FindElements(By.ClassName("transactions"));

        protected static IReadOnlyList<IWebElement> PageH2Sections => Driver.Instance.FindElements(By.TagName("h2"));

        protected static IReadOnlyList<IWebElement> AnchorElements => Driver.Instance.FindElements(By.TagName("a"));

        protected static IWebElement FundingStreamSettingsTable =>
            Driver.Instance.FindElement(By.ClassName("fundingstream-settings-table"));

        protected static IWebElement AddFundingStreamLink =>
            Driver.Instance.FindElement(By.ClassName("add-fundingstream-setting"));

        protected static IWebElement PublicationSettingsTable =>
            Driver.Instance.FindElement(By.ClassName("fundingstream-publications-table"));

        protected static IWebElement SettingTypeDropDownElement => Driver.Instance.FindElement(By.Name("SettingTypeId"));

        protected static SelectElement SettingTypeDropDown => new SelectElement(SettingTypeDropDownElement);

        protected static IWebElement ContinueButton => Driver.Instance.FindElement(By.ClassName("button"));

        protected static IWebElement NewValueInput => Driver.Instance.FindElement(By.Name("NewStringValue"));

        protected static IWebElement SuccessMessage => Driver.Instance.FindElement(By.ClassName("govuk-box-highlight"));

        #endregion
    }
}