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
    /// The Admin Funding stream list page class.
    /// </summary>
    public class AdminFundingStreamListPage : ViewYourFundingBasePage
    {
        #region Private fields

        private const string DoNotUseRegressionFundingStreamDescription = "donotuseregressiofundingstream";
        private const string DoNotUseRegressionFundingStreamCode = "RTXXX";
        private const string DoNotUseRegressionUpdatedFundingStreamDescription = "donotuseregressionupdatedfundingstream";

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

        /// <summary>
        /// Selects Funding stream.
        /// </summary>
        /// <param name="fundingStream">The fundingstream.</param>
        public static void SelectFundingStream(string fundingStream)
        {
            var editRow = FundingStreamTableRows.First(row =>
                row.Text.Contains(fundingStream, StringComparison.InvariantCultureIgnoreCase));
            editRow.FindElements(By.TagName("a")).First().MoveAndClick();
        }

        public static void AddFundingStream()
        {
            AddFundingStreamLink.MoveAndClick();
            FundingStreamCode.SendKeys(DoNotUseRegressionFundingStreamCode);
            FundingStreamDescription.SendKeys(DoNotUseRegressionFundingStreamDescription);
            ContinueButton.MoveAndClick();
            ContinueButton.MoveAndClick();
        }

        public static void EditFundingStream()
        {
            var editRow = FundingStreamTableRows.First(row =>
               row.Text.Contains(DoNotUseRegressionFundingStreamDescription, StringComparison.InvariantCultureIgnoreCase));
            editRow.FindElements(By.TagName("a")).First().MoveAndClick();

            var settingRow = SettingsTableRows.First(row =>
               row.Text.Contains(DoNotUseRegressionFundingStreamCode, StringComparison.InvariantCultureIgnoreCase));
            settingRow.FindElement(By.TagName("a")).MoveAndClick();

            FundingStreamDescription.SendKeys(DoNotUseRegressionUpdatedFundingStreamDescription);

            ContinueButton.MoveAndClick();
            ContinueButton.MoveAndClick();
        }

        public static void DeleteFundingStream()
        {
            var editRow = FundingStreamTableRows.First(row =>
               row.Text.Contains(DoNotUseRegressionFundingStreamCode, StringComparison.InvariantCultureIgnoreCase));
            editRow.FindElements(By.TagName("a"))[1].MoveAndClick();

            ContinueButton.MoveAndClick();
        }

        #endregion


        #region Assertions

        public static void EnsureFundingStreamTableExists()
        {
            FundingStreamTableTd.Any(x => x.Text.Contains("PE and sport premium", StringComparison.InvariantCultureIgnoreCase)).Should().BeTrue();
            FundingStreamTableTd.Any(x => x.Text.Contains("Dedicated schools grant", StringComparison.InvariantCultureIgnoreCase)).Should().BeTrue();
        }

        public static void EnsureFundingStreamOperationConfirmation()
        {
            SuccessMessage.Displayed.Should()
                .BeTrue();
        }

        #endregion


        #region Page Elements

        protected static IWebElement ContinueButton => Driver.Instance.FindElement(By.ClassName("button"));

        protected static IWebElement FundingStreamTable => Driver.Instance.FindElement(By.ClassName("touch-table"));

        protected static IReadOnlyList<IWebElement> SettingsTableRows => SettingsTable.FindElements(By.TagName("tr"));

        protected static IReadOnlyList<IWebElement> FundingStreamTableRows => FundingStreamTable.FindElements(By.TagName("tr"));

        protected static IWebElement AddFundingStreamLink => Driver.Instance.FindElement(By.LinkText("Add a new funding stream"));

        protected static IWebElement FundingStreamCode => Driver.Instance.FindElement(By.Name("FundingStream.FundingStreamCode"));

        protected static IWebElement FundingStreamDescription => Driver.Instance.FindElement(By.Name("FundingStream.FundingStreamName"));

        protected static IWebElement SuccessMessage => Driver.Instance.FindElement(By.ClassName("govuk-box-highlight"));

        protected static IWebElement SettingsTable => Driver.Instance.FindElement(By.ClassName("touch-table"));

        protected static IReadOnlyList<IWebElement> FundingStreamTableTd => FundingStreamTable.FindElements(By.XPath("//tr/td"));

        #endregion
    }
}