using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.Admin
{
    public class LayoutManagementPage : ViewYourFundingBasePage
    {
        private static string noOfResultsDisplayedText;

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

        public static void NavigateToLayoutManagementPage()
        {
            AdminHomePage.ClickOnTile("Layout management settings");
        }

        public static void DoPagination()
        {
            if (IsElementPresent(By.Id("nextPage")))
            {
                noOfResultsDisplayedText = NoOfResultsDisplayedText();

                NextButton
                .MoveAndClick();
                WaitForPageToLoad();
            }
        }

        public static string GetCurrentPageNumber()
        {
            return CurrentPageNo.GetAttribute("value");
        }

        public static void DoFundingStreamFiltering()
        {
            noOfResultsDisplayedText = NoOfResultsDisplayedText();
            if (IsElementPresent(By.ClassName("chk-funding-streams")))
            {
                FundingStreamFilters.First().Click();
                WaitForPageToLoad();
            }
        }

        public static void DoFundingViewTypeFiltering()
        {
            var key = "chk-funding-view-types";

            noOfResultsDisplayedText = NoOfResultsDisplayedText();

            if (IsElementPresent(By.ClassName(key)))
            {
                FundingViewTypeFilters.First().Click();
                WaitForPageToLoad();
            }
        }

        public static void DoFundingViewScopeFiltering()
        {
            var key = "chk-funding-view-types";

            if (IsElementPresent(By.ClassName(key)))
            {
                FundingViewScopeFilters.First().Click();
                WaitForPageToLoad();
            }
        }

        #endregion

        #region Assertions

        public static void EnsurePageTitle()
        {
            PageH1Sections.Any(h1 =>
                    h1.Text.Contains("Layout Management", StringComparison.InvariantCultureIgnoreCase))
                .Should()
                .BeTrue();
        }

        public static void EnsurePageHasFilters()
        {
            PageH2Sections.Any(h2 =>
                    h2.Text.Contains("Funding stream", StringComparison.InvariantCultureIgnoreCase))
                .Should()
                .BeTrue();

            PageH2Sections.Any(h2 =>
                    h2.Text.Contains("Funding view type", StringComparison.InvariantCultureIgnoreCase))
                .Should()
                .BeTrue();
        }


        public static void EnsurePageHasLayoutTable()
        {
            LayoutTable.Should().NotBeNull();
        }

        public static void EnsurePaginationResultsChanged()
        {
            if (IsElementPresent(By.Id("nextPage")))
            {
                NoOfResultsDisplayedText().Should().NotBeEquivalentTo(noOfResultsDisplayedText);
            }
        }

        public static void EnsureFundingStreamFilterationResultsChanged()
        {
            if (IsElementPresent(By.ClassName("chk-funding-streams")) && ElementCount(By.ClassName("chk-funding-streams")) > 1)
            {
                NoOfResultsDisplayedText().Should().NotBeEquivalentTo(noOfResultsDisplayedText);
            }
        }

        public static void EnsureFundingViewTypeFilterationResultsChanged()
        {
            var key = "chk-funding-view-types";

            if (IsElementPresent(By.ClassName(key)) && ElementCount(By.ClassName(key)) > 1)
            {
                NoOfResultsDisplayedText().Should().NotBeEquivalentTo(noOfResultsDisplayedText);
            }
        }

        public static void EnsureFundingViewScopeFilterationResultsChanged()
        {
            var key = "chk-funding-view-scopes";

            if (IsElementPresent(By.ClassName(key)) && ElementCount(By.ClassName(key)) > 1)
            {
                NoOfResultsDisplayedText().Should().NotBeEquivalentTo(noOfResultsDisplayedText);
            }
        }

        public static void WaitForPageToLoad()
        {
            Thread.Sleep(20000);
        }

        private static string NoOfResultsDisplayedText()
        {
            return PageSummary.Text;
        }

        #endregion


        #region Page Elements

        protected static IWebElement LayoutTable => Driver.Instance.FindElement(By.ClassName("touch-table"));

        protected static IReadOnlyList<IWebElement> PageH1Sections => Driver.Instance.FindElements(By.TagName("h1"));

        protected static IReadOnlyList<IWebElement> PageH2Sections => Driver.Instance.FindElements(By.TagName("h2"));

        protected static IReadOnlyList<IWebElement> FundingStreamFilters => Driver.Instance.FindElements(By.ClassName("chk-funding-streams"));

        protected static IReadOnlyList<IWebElement> FundingViewTypeFilters => Driver.Instance.FindElements(By.ClassName("chk-funding-view-types"));

        protected static IReadOnlyList<IWebElement> FundingViewScopeFilters => Driver.Instance.FindElements(By.ClassName("chk-funding-view-scopes"));

        protected static IWebElement NextButton => Driver.Instance.FindElement(By.Id("nextPage"));

        protected static IWebElement CurrentPageNo => Driver.Instance.FindElement(By.Id("currPage"));

        protected static IWebElement PageSummary => Driver.Instance.FindElement(By.Id("pgSummary"));

        protected static bool IsElementPresent(By by)
        {
            return Driver.Instance.FindElements(by).Any() && !Driver.Instance.FindElement(by).GetAttribute("class").Contains("hidden");
        }

        protected static int ElementCount(By by)
        {
            return Driver.Instance.FindElements(by).Any() ? Driver.Instance.FindElements(by).Count() : 0;
        }

        #endregion
    }
}