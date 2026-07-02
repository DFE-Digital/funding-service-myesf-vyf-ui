using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.Admin
{
    public class PdfGenerationActionsPage : ViewYourFundingBasePage
    {
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

        public static void NavigateToPdfGenerationActionsPage()
        {
            AdminHomePage.ClickOnTile("Document generation actions");
        }

        public static void NavigateToRunFeedReaderProceedPage()
        {
            var editRow = GeneralSettingsTableRows.First(row =>
                        row.Text.Contains("Run Feed Reader", StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.LinkText("Proceed")).MoveAndClick();
        }

        public static void NavigateToRunPdfComparisonPage()
        {
            var editRow = GeneralSettingsTableRows.First(row =>
                row.Text.Contains("Run Pdf Comparison", StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.LinkText("Proceed")).MoveAndClick();
        }

        public static void NavigateToGenerateSinglePdfPage()
        {
            var editRow = GeneralSettingsTableRows.First(row =>
                row.Text.Contains("Generate Single Document", StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.LinkText("Proceed")).MoveAndClick();
        }


        public static void NavigateToGenerateFundingReportPage()
        {
            var editRow = GeneralSettingsTableRows.First(row =>
                row.Text.Contains("Generate Funding Reports", StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.LinkText("Proceed")).MoveAndClick();
        }

        public static void NavigateToFeedReaderLastFunPage()
        {
            var editRow = GeneralSettingsTableRows.First(row =>
                row.Text.Contains("Feed Reader Last Run Status", StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.LinkText("View status")).MoveAndClick();
        }


        public static void NavigateToRerunPdfGenerationPage()
        {
            var editRow = GeneralSettingsTableRows.First(row =>
                row.Text.Contains("ReRun Document generation", StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.LinkText("Proceed")).MoveAndClick();
        }

        #endregion


        #region Assertions

        public static void EnsurePageTitle(string title)
        {
            PageH1Sections.Any(h1 =>
                    h1.Text.Contains(title, StringComparison.InvariantCultureIgnoreCase))
                .Should()
                .BeTrue();
        }

        public static void EnsurePageHasGeneralSettingsTable()
        {
            GeneralSettingsTable.Should().NotBeNull();
        }

        public static void EnsureRunFeedReaderPageElements()
        {
            SubmitButton.Should().NotBeNull();
            ByPassBookmarkOption.Should().NotBeNull();
            FundingStreamCodesOptions.Should().NotBeEmpty();
        }

        public static void EnsureRunPdfComparisonPageElements()
        {
            SourceFolderInputBox.Should().NotBeNull();
            DestinationFolderInput.Should().NotBeNull();
            FundingStreamCodeAndFundingPeriod.Should().NotBeNull();
            SubmitButton.Should().NotBeNull();
        }

        public static void EnsureGenerateSinglePdfPageElements()
        {
            ProviderFundingIdInputBox.Should().NotBeNull();
            FundingStreamCodeAndFundingPeriod.Should().NotBeNull();
            UkprnInputBox.Should().NotBeNull();
            CutOffDateInputBox.Should().NotBeNull();
            ProviderType.Should().NotBeNull();
            ProviderSubType.Should().NotBeNull();
            SubmitButton.Should().NotBeNull();
        }

        public static void EnsureGenerateFundingReportsPageElements()
        {
            FundingPeriodList.Should().NotBeNull();
            ReportTypeList.Should().NotBeNull();
        }

        public static void EnsurePageHasLastRunTableElements()
        {
            LastRunTable.Should().NotBeNull();
            LastRunTableRows.Any(row =>
                row.Text.Contains("Run Status", StringComparison.InvariantCultureIgnoreCase)).Should().BeTrue();
            LastRunTableRows.Any(row =>
                row.Text.Contains("Start date and time", StringComparison.InvariantCultureIgnoreCase)).Should().BeTrue();
            LastRunTableRows.Any(row =>
                row.Text.Contains("Feed Reader API Url", StringComparison.InvariantCultureIgnoreCase)).Should().BeTrue();
        }

        public static void EnsureReRunPdfGenerationPageElements()
        {
            FundingStreamCode.Should().NotBeNull();
            SinceCreatedDate.Should().NotBeNull();
            EndDateTime.Should().NotBeNull();
            ResetFunding.Should().NotBeNull();
            ResetProviderFunding.Should().NotBeNull();
            SubmitButton.Should().NotBeNull();
        }

        #endregion


        #region Page Elements

        protected static IReadOnlyList<IWebElement> PageH1Sections => Driver.Instance.FindElements(By.TagName("h1"));

        protected static IWebElement GeneralSettingsTable => Driver.Instance.FindElement(By.ClassName("touch-table"));

        protected static IReadOnlyList<IWebElement> GeneralSettingsTableRows => GeneralSettingsTable.FindElements(By.TagName("tr"));

        protected static IWebElement FundingStreamCodeAndFundingPeriod => Driver.Instance.FindElement(By.Id("FundingStreamCodeAndPeriodCode"));

        protected static IWebElement DestinationFolderInput => Driver.Instance.FindElement(By.Id("DestinationFolder"));

        protected static IWebElement SourceFolderInputBox => Driver.Instance.FindElement(By.Id("SourceFolder"));

        protected static IWebElement ProviderFundingIdInputBox => Driver.Instance.FindElement(By.Id("ProviderFundingId"));

        protected static IWebElement UkprnInputBox => Driver.Instance.FindElement(By.Id("Ukprn"));

        protected static IWebElement CutOffDateInputBox => Driver.Instance.FindElement(By.Id("CutOffDate"));

        protected static IWebElement ProviderType => Driver.Instance.FindElement(By.Id("ProviderType"));

        protected static IWebElement ProviderSubType => Driver.Instance.FindElement(By.Id("ProviderSubType"));

        protected static IWebElement SubmitButton => Driver.Instance.FindElement(By.ClassName("button"));

        protected static IWebElement FundingPeriodList => Driver.Instance.FindElement(By.Id("FundingPeriodCode"));

        protected static IWebElement ReportTypeList => Driver.Instance.FindElement(By.Id("ReportType"));

        protected static IWebElement LastRunTable => Driver.Instance.FindElement(By.ClassName("touch-table"));

        protected static IReadOnlyList<IWebElement> LastRunTableRows => LastRunTable.FindElements(By.TagName("tr"));

        protected static IWebElement ByPassBookmarkOption => Driver.Instance.FindElement(By.Id("ByPassBookmark"));

        protected static IReadOnlyCollection<IWebElement> FundingStreamCodesOptions => Driver.Instance.FindElements(By.Name("fundingStreamCodes"));

        protected static IWebElement SinceCreatedDate => Driver.Instance.FindElement(By.Id("SinceCreatedDate"));

        protected static IWebElement EndDateTime => Driver.Instance.FindElement(By.Id("EndDateTime"));

        protected static IWebElement ResetFunding => Driver.Instance.FindElement(By.Id("ResetFunding"));

        protected static IWebElement ResetProviderFunding => Driver.Instance.FindElement(By.Id("ResetProviderFunding"));

        protected static IWebElement FundingStreamCode => Driver.Instance.FindElement(By.Id("FundingStreamCode"));

        #endregion
    }
}