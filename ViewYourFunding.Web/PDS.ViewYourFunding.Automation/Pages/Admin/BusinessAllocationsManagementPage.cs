using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.Admin
{
    public class BusinessAllocationsManagementPage : ViewYourFundingBasePage
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
            Goto("view-latest-funding/admin/allocations-management/home");
        }

        public static void NavigateToRequestDataPage()
        {
            var editRow = AllocationsManagementActionsTableRows.First(row =>
                        row.Text.Contains("Request data", StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.LinkText("Request data")).MoveAndClick();
        }

        public static void NavigateToRunStatusPageFromRequestData()
        {
            CheckBox_1619.Click();
            SubmitButton.MoveAndClick();
        }

        public static void NavigateToRunPdfComparisonPage()
        {
            var editRow = AllocationsManagementActionsTableRows.First(row =>
                row.Text.Contains("Compare statements", StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.LinkText("Compare statements")).MoveAndClick();
        }

        public static void NavigateToRunStatusPageFromHome()
        {
            var editRow = AllocationsManagementActionsTableRows.First(row =>
                        row.Text.Contains("Check status", StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.LinkText("Check status")).MoveAndClick();
        }

        public static void NavigateToSearchProviderData()
        {
            var editRow = AllocationsManagementActionsTableRows.First(row =>
                        row.Text.Contains("Search provider data", StringComparison.InvariantCultureIgnoreCase));

            editRow.FindElement(By.LinkText("Search provider data")).MoveAndClick();
        }

        public static void NavigateToSearchByProviderAndYear()
        {
            SearchProviderDataFundingStreams.First().Click();
            SubmitButton.MoveAndClick();
        }

        public static void NavigateToSearchByProviderAndYearDetails()
        {
            SearchByProviderAndYearUkprnTextbox.SendKeys("12345678");
            SearchByProviderAndYearFundingPeriods.First().Click();
            SubmitButton.MoveAndClick();
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

        public static void EnsurePageHasAllocationsManagementActions()
        {
            AllocationsManagementActionsTable.Should().NotBeNull();
            AllocationsManagementActionsTableRows.Count.Should().Be(4);
            AllocationsManagementActionsTableRows.Any(row =>
               row.Text.Contains("Request Data", StringComparison.InvariantCultureIgnoreCase)).Should().BeTrue();
            AllocationsManagementActionsTableRows.Any(row =>
                row.Text.Contains("Check status", StringComparison.InvariantCultureIgnoreCase)).Should().BeTrue();
            AllocationsManagementActionsTableRows.Any(row =>
                row.Text.Contains("Compare statements", StringComparison.InvariantCultureIgnoreCase)).Should().BeTrue();
            AllocationsManagementActionsTableRows.Any(row =>
                row.Text.Contains("Search provider data", StringComparison.InvariantCultureIgnoreCase)).Should().BeTrue();
        }

        public static void EnsureRunFeedReaderPageElements()
        {
            SubmitButton.Should().NotBeNull();
            FundingStreamCodesOptions.Should().NotBeEmpty();
        }


        public static void EnsureRunPdfComparisonPageElements()
        {
            SourceFolderInputBox.Should().NotBeNull();
            TargetFolderInput.Should().NotBeNull();
            FundingStreamCodeAndFundingPeriod.Should().NotBeNull();
            SubmitButton.Should().NotBeNull();
        }

        public static void EnsureSearchProviderDataPageElements()
        {
            EnsurePageTitle("Select a funding stream");
            SearchProviderDataFundingStreams.Should().NotBeEmpty();
            SubmitButton.Should().NotBeNull();
        }

        public static void EnsureSearchByProviderAndYearPageElements()
        {
            EnsurePageTitle("Search by provider and year");
            SearchByProviderAndYearUkprnTextbox.Should().NotBeNull();
            SearchByProviderAndYearFundingPeriods.Should().NotBeEmpty();
            SubmitButton.Should().NotBeNull();
        }

        public static void EnsureSearchByProviderAndYearDetailsPageElements()
        {
            EnsurePageTitle("(UKPRN: 12345678)");
            SearchByProviderAndYearDetailsProviderFundingsHeading.Should().NotBeNull();
            SearchByProviderAndYearDetailsProviderFundings.Should().NotBeNull();
            SearchByProviderAndYearDetailsGoBackLink.Should().NotBeNull();
        }

        #endregion


        #region Page Elements

        protected static IReadOnlyList<IWebElement> PageH1Sections => Driver.Instance.FindElements(By.TagName("h1"));

        protected static IWebElement AllocationsManagementActionsTable => Driver.Instance.FindElement(By.ClassName("touch-table"));

        protected static IReadOnlyList<IWebElement> AllocationsManagementActionsTableRows => AllocationsManagementActionsTable.FindElements(By.TagName("tr"));

        protected static IReadOnlyCollection<IWebElement> FundingStreamCodesOptions => Driver.Instance.FindElements(By.Name("fundingStreamCodes"));

        protected static IWebElement SubmitButton => Driver.Instance.FindElement(By.ClassName("button"));

        protected static IWebElement CheckBox_1619 => Driver.Instance.FindElement(By.Id("check-1619"));

        protected static IWebElement FundingStreamCodeAndFundingPeriod => Driver.Instance.FindElement(By.Id("FundingStreamCodeAndPeriodCode"));

        protected static IWebElement TargetFolderInput => Driver.Instance.FindElement(By.Id("TargetFolder"));

        protected static IWebElement SourceFolderInputBox => Driver.Instance.FindElement(By.Id("SourceFolder"));

        protected static IReadOnlyCollection<IWebElement> SearchProviderDataFundingStreams => Driver.Instance.FindElements(By.Name("SelectedfundingStreamCode"));

        protected static IWebElement SearchByProviderAndYearUkprnTextbox => Driver.Instance.FindElement(By.Id("Ukprn"));

        protected static IReadOnlyCollection<IWebElement> SearchByProviderAndYearFundingPeriods => Driver.Instance.FindElements(By.Name("SelectedFundingPeriodCode"));

        protected static IWebElement SearchByProviderAndYearDetailsProviderFundingsHeading => Driver.Instance.FindElement(By.Name("ProviderFundingsHeading"));

        protected static IWebElement SearchByProviderAndYearDetailsProviderFundings => Driver.Instance.FindElement(By.Name("ProviderFundings"));

        protected static IWebElement SearchByProviderAndYearDetailsGoBackLink => Driver.Instance.FindElement(By.Name("Back"));

        #endregion
    }
}