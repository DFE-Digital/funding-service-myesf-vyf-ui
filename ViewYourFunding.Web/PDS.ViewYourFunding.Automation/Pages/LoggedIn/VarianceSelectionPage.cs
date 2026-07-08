using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.LoggedIn
{
    public class VarianceSelectionPage : ViewYourFundingBasePage
    {
        public static void NavigateToPageViaLogin(string username, string password, string ukprn, string fundingStream, string publicationDate, int yearTo, int yearFrom, string tab)
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();
            LoginPage.Open();

            LoginPage.Login(username, password);

            Goto($"view-latest-funding/pre-16-16-19-statements/variance-selection/{ukprn}/{fundingStream}/{publicationDate}/{yearFrom}-to-{yearTo}?tab={tab}");
        }

        #region Assertions

        public static void EnsureNavigatedToLogin()
        {
            EnsureCurrentPage("Login");
        }

        public static void EnsureCurrentProviderPage()
        {
            EnsureCurrentPage("Select a previous statement to compare your figures");
        }

        public static void EnsureStatement(string fundingStream, int yearTo, int yearFrom, string publicationDate)
        {
            Paragraphs.Count().Should().Be(1);
            Paragraphs.First().Text.Should().Be($"Statement: {fundingStream}: academic year {yearFrom} to {yearTo} published on {publicationDate}");
        }


        public static void EnsureFinalVarianceOptionDisplays(int yearTo, int yearFrom, string publicationDate)
        {
            var element = Driver.Instance.FindElement(By.LinkText($"{yearFrom} to {yearTo} final statement(published {publicationDate})"));
            element.Should().NotBeNull();
        }

        public static void EnsurePreviousVarianceOptionDisplays(int yearTo, int yearFrom, string publicationDate)
        {
            var element = Driver.Instance.FindElement(By.LinkText($"{yearFrom} to {yearTo} previous statement(published {publicationDate})"));
            element.Should().NotBeNull();
        }

        public static void EnsureNoComparisonVarianceOptionDisplays(int yearTo, int yearFrom, string publicationDate)
        {
            NoComparison.Should().NotBeNull();
        }

        public static void SelectAndContinueWithNoComparison()
        {
            NoComparison.Click();
            ContinueButton.MoveAndClick();
        }

        public static void SelectAndContinueWithCurrentYear()
        {
            PreviousStatementCurrentYear.Click();
            ContinueButton.MoveAndClick();
        }

        public static void SelectAndContinueWithPreviousYear()
        {
            FinalStatementPreviousYear.Click();
            ContinueButton.MoveAndClick();
        }

        #endregion

        #region Page Elements

        protected static ReadOnlyCollection<IWebElement> Paragraphs => Driver.Instance.FindElements(By.TagName("p"));

        protected static IWebElement NoComparison => Driver.Instance.FindElement(By.Id("radio-NoComparison"));

        protected static IWebElement PreviousStatementCurrentYear => Driver.Instance.FindElement(By.Id("radio-PreviousStatementCurrentYear"));

        protected static IWebElement FinalStatementPreviousYear => Driver.Instance.FindElement(By.Id("radio-FinalStatementPreviousYear"));

        protected static IWebElement ContinueButton => Driver.Instance.FindElement(By.ClassName("button"));

        #endregion
    }
}