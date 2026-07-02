using FluentAssertions;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.LoggedIn
{
    public class IndicativeProviderPage : ViewYourFundingBasePage
    {
        public static void NavigateToPageViaLogin(string username, string password)
        {
            LoginPage.LogoutIfLoggedIn();
            LoginPage.Open();
            LoginPage.Login(username, password);

            Goto("view-latest-funding/pre-16-16-19-statements");
        }

        public static void NavigateToPageWithoutLogin()
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();

            Goto("view-latest-funding/pre-16-16-19-statements");
        }

        public static void NavigateToPageDirectly()
        {
            Goto("view-latest-funding/pre-16-16-19-statements");
        }

        public static void ExpandGAG()
        {
            if (!GAGSectionDiv.Displayed)
            {
                GAGSectionButton.Click();
            }
        }

        #region Assertions

        public static void EnsureNavigatedToLogin()
        {
            EnsureCurrentPage("Login");
        }

        public static void EnsureCurrentProviderPage()
        {
            EnsureCurrentPage("Allocation statements");
        }

        public static void EnsureHasExpectedNumberOfSections(int expectedCount)
        {
            Sections.Count.Should().Be(expectedCount);
        }

        public static void EnsureGAGSchoolBudgetShareDisplays()
        {
            SchoolShareTabLink.Displayed.Should().BeTrue();
        }

        public static void EnsureGAGPostOpeningGrantDisplays()
        {
            PostOpeningGrantLink.Displayed.Should().BeTrue();
        }

        public static void EnsureGAGMinimumFundingGuaranteeDisplays()
        {
            GAGMinimumFundingGuaranteeLink.Displayed.Should().BeTrue();
        }

        public static void EnsureGAGStartUpGrantDisplays()
        {
            StartUpGrantLink.Displayed.Should().BeTrue();
        }

        public static void EnsureHighNeedsDisplays()
        {
            HighNeedsLink.Displayed.Should().BeTrue();
        }

        #endregion


        #region Page Elements

        protected static ReadOnlyCollection<IWebElement> Sections => Driver.Instance.FindElements(By.ClassName("govuk-accordion__section-header"));

        protected static IWebElement GAGSectionButton => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-GAG .govuk-accordion__section-button"));

        protected static IWebElement GAGSectionDiv => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-GAG .govuk-accordion__section-content"));

        protected static IWebElement GAGDownloadADocument => GAGSectionDiv.FindElement(By.ClassName("download-a-document")).FindElement(By.TagName("a"));

        protected static IWebElement GAGMinimumFundingGuaranteeLink => GAGSectionDiv.FindElement(By.LinkText("Minimum funding guarantee"));

        protected static IWebElement SchoolShareTabLink => Driver.Instance.FindElement(By.LinkText("School budget share"));

        protected static IWebElement PostOpeningGrantLink => GAGSectionDiv.FindElement(By.LinkText("Post-opening grant"));

        protected static IWebElement HighNeedsLink => Driver.Instance.FindElement(By.LinkText("School budget share"));

        protected static IWebElement StartUpGrantLink => Driver.Instance.FindElement(By.LinkText("School budget share"));

        #endregion
    }
}