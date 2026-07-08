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
    /// The Admin Funding stream settings page.
    /// </summary>
    public class AdminPublicationPage : ViewYourFundingBasePage
    {
        #region Private fields

        private const string DoNotUsePublicationDescription = "donotuseregressionpublicationdescription";
        private const string DoNotUsePublicationFundingPeriodCode = "RT-2021";

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

        public static void NavigateToFundingStreamSettingsPage()
        {
            AdminHomePage.ClickOnTile("Funding streams settings");
            AdminFundingStreamListPage.SelectFundingStream("PE and sport premium");
        }

        public static void AddPublication()
        {
            AddPublicationLink
                .MoveAndClick();

            FundingPeriodCodeInput.SendKeys(DoNotUsePublicationFundingPeriodCode);
            DescriptionInput.SendKeys(DoNotUsePublicationDescription);
            PublishDateDayInput.SendKeys("12");
            PublishDateMonthInput.SendKeys("12");
            PublishDateYearInput.SendKeys("2023");

            ContinueButton.MoveAndClick();

            ContinueButton.MoveAndClick();
        }

        public static void EditPublication()
        {
            var editRow = PublicationTableRows.First(row =>
                row.Text.Contains(DoNotUsePublicationFundingPeriodCode, StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.LinkText("Edit"))
                .MoveAndClick();

            DescriptionInput.SendKeys("updated setting value");

            ContinueButton.MoveAndClick();

            ContinueButton.MoveAndClick();
        }

        public static void DeletePublication()
        {
            var deleteRow = PublicationTableRows.First(row =>
                row.Text.Contains(DoNotUsePublicationDescription, StringComparison.InvariantCultureIgnoreCase));

            deleteRow.FindElement(By.LinkText("Delete"))
                .MoveAndClick();

            ContinueButton.MoveAndClick();
        }

        #endregion


        #region Assertions


        public static void EnsureSettingOperationConfirmation()
        {
            SuccessMessage.Displayed.Should()
                .BeTrue();
        }

        #endregion


        #region Page Elements

        protected static IReadOnlyList<IWebElement> PublicationTableRows => PublicationSettingsTable.FindElements(By.TagName("tr"));

        protected static IWebElement AddPublicationLink =>
            Driver.Instance.FindElement(By.LinkText("Add a Publication"));

        protected static IWebElement PublicationSettingsTable =>
            Driver.Instance.FindElement(By.ClassName("fundingstream-publications-table"));

        protected static IWebElement ContinueButton => Driver.Instance.FindElement(By.ClassName("button"));

        protected static IWebElement FundingPeriodCodeInput => Driver.Instance.FindElement(By.Name("FundingPublication.FundingPeriodCode"));

        protected static IWebElement DescriptionInput => Driver.Instance.FindElement(By.Name("FundingPublication.Description"));

        protected static IWebElement PublishDateDayInput => Driver.Instance.FindElement(By.Name("FundingPublication.PublishedDateDay"));

        protected static IWebElement PublishDateMonthInput => Driver.Instance.FindElement(By.Name("FundingPublication.PublishedDateMonth"));

        protected static IWebElement PublishDateYearInput => Driver.Instance.FindElement(By.Name("FundingPublication.PublishedDateYear"));

        protected static IWebElement SuccessMessage => Driver.Instance.FindElement(By.ClassName("govuk-box-highlight"));

        #endregion
    }
}