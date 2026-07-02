using FluentAssertions;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Linq;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.LoggedIn
{
    public class MultipleAcademyTrustStatementPage : ViewYourFundingBasePage
    {
        public static void NavigateToPageViaLogin(string username, string password)
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();
            LoginPage.Open();

            LoginPage.Login(username, password);

            Goto("view-latest-funding/pre-16-16-19-statements/parent");
        }

        public static void NavigateToPageWithoutLogin()
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();

            Goto("view-latest-funding/pre-16-16-19-statements/parent");
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

        public static void EnsureHasSections()
        {
            Sections.Count.Should().BeGreaterThan(13);
        }

        public static void EnsureHasFilters()
        {
            Filters.Count.Should().BeGreaterThan(0);
        }

        public static void ExpandSixteen19()
        {
            if (!Sixteen19AccordionSection.Displayed)
            {
                Sixteen19AccordionButton.Click();
            }
        }

        public static void ExpandGAG()
        {
            if (!GAGAccordionSection.Displayed)
            {
                GAGAccordionButton.Click();
            }
        }

        public static void ExpandPSG()
        {
            if (!PSGAccordionSection.Displayed)
            {
                PSGAccordionButton.Click();
            }
        }

        public static void EnsureResources()
        {
            var found = false;

            foreach (var h3 in H3Titles)
            {
                if (h3.Text.Contains("resources"))
                {
                    found = true;
                    break;
                }
            }

            found.Should().BeTrue();
        }

        public static void EnsureExploreTopic()
        {
            var found = false;

            foreach (var h3 in H3Titles)
            {
                if (h3.Text.Contains("Explore the topic"))
                {
                    found = true;
                    break;
                }
            }

            found.Should().BeTrue();
        }

        public static void EnsureAllocationHistory()
        {
            var found = false;

            foreach (var h3 in H3Titles)
            {
                if (h3.Text.Equals("Allocation history"))
                {
                    found = true;
                    break;
                }
            }

            found.Should().BeTrue();
        }

        public static void EnsureGAGMinimumFundingGuaranteeDisplays()
        {
            GAGMinimumFundingGuarantee.Displayed.Should().BeTrue();
        }

        public static void EnsureSixteen19BreakdownTextDisplays()
        {
            Sixteen19BreakdownText.Displayed.Should().BeTrue();
        }

        public static void EnsurePSGFundingBreakdownLinkDisplays()
        {
            PSGViewFundingBreakdown.Displayed.Should().BeTrue();
        }

        public static void EnsureGAGVariance()
        {
            GAGHeadingVariance.Displayed.Should().BeTrue();
            GAGContentVariance.Displayed.Should().BeTrue();
        }

        #endregion

        #region Page Elements
        protected static ReadOnlyCollection<IWebElement> Filters => Driver.Instance.FindElements(By.ClassName("filter"));

        protected static ReadOnlyCollection<IWebElement> Sections => Driver.Instance.FindElements(By.ClassName("govuk-accordion__section-header"));

        protected static IWebElement GAGAccordion => Driver.Instance.FindElement(By.CssSelector("[data-fundingtype='GAG']"));

        protected static IWebElement GAGAccordionSection => GAGAccordion.FindElement(By.ClassName("govuk-accordion__section-content"));

        protected static IWebElement GAGAccordionButton => GAGAccordion.FindElement(By.ClassName("govuk-accordion__section-button"));

        protected static IWebElement Sixteen19Accordion => Driver.Instance.FindElement(By.CssSelector("[data-fundingtype='1619']"));

        protected static IWebElement Sixteen19AccordionSection => Sixteen19Accordion.FindElement(By.ClassName("govuk-accordion__section-content"));

        protected static IWebElement Sixteen19AccordionButton => Sixteen19Accordion.FindElement(By.ClassName("govuk-accordion__section-button"));

        public static void EnsureHasAcademySection(string academyName)
        {
            academyName = academyName.Replace("'", "\\'");
            var sectionToFind = Driver.Instance.FindElement(By.CssSelector($"[data-academy='{academyName}']"));
            sectionToFind.Should().NotBeNull();
            sectionToFind.Displayed.Should().BeTrue();
        }

        public static void EnsureHasLocalAuthoritySection(string localAuthority)
        {
            localAuthority = localAuthority.Replace("'", "\\'");
            var sectionToFind = Driver.Instance.FindElement(By.CssSelector($"[data-localauthority='{localAuthority}']"));
            sectionToFind.Should().NotBeNull();
            sectionToFind.Displayed.Should().BeTrue();
        }

        public static void EnsureHasFundingTypeSection(string fundingType)
        {
            fundingType = fundingType.Replace("'", "\\'");
            var sectionToFind = Driver.Instance.FindElement(By.CssSelector($"[data-fundingtype='{fundingType}']"));
            sectionToFind.Should().NotBeNull();
            sectionToFind.Displayed.Should().BeTrue();
        }

        protected static IWebElement PSGAccordion => Driver.Instance.FindElement(By.CssSelector("[data-fundingtype='PSG']"));

        protected static IWebElement PSGAccordionSection => PSGAccordion.FindElement(By.ClassName("govuk-accordion__section-content"));

        protected static IWebElement PSGAccordionButton => PSGAccordion.FindElement(By.ClassName("govuk-accordion__section-button"));

        protected static ReadOnlyCollection<IWebElement> H3Titles => Driver.Instance.FindElements(By.TagName("h3"));

        public static void FilterStatementsByFundingType(string fundingType)
        {
            var filterCheckbox = Driver.Instance.FindElement(By.Id($"queryFilter-{fundingType}"));

            filterCheckbox.Click();
        }

        public static void FilterStatementsByAcademyName(string academyName)
        {
            var filterCheckbox = Driver.Instance.FindElement(By.Id($"queryFilter-{academyName}"));

            filterCheckbox.Click();
        }

        public static void FilterStatementsByLocalAuthority(string localAuthority)
        {
            var filterCheckbox = Driver.Instance.FindElement(By.Id($"queryFilter-{localAuthority}"));

            filterCheckbox.Click();
        }

        protected static IWebElement GAGMinimumFundingGuarantee => Driver.Instance.FindElement(By.LinkText("Minimum funding guarantee"));

        protected static IWebElement Sixteen19BreakdownText => Sixteen19AccordionSection.FindElement(By.LinkText("View funding breakdown for Truro and Penwith College"));

        protected static IWebElement PSGViewFundingBreakdown => Driver.Instance.FindElement(By.LinkText("View funding breakdown"));

        protected static IWebElement SchoolShareTabLink => Driver.Instance.FindElement(By.LinkText("Minimum funding guarantee"));

        protected static IWebElement ViewHistoryOfAllocatedFundingLink => Driver.Instance.FindElement(By.LinkText("View history of allocated funding"));

        protected static IWebElement GAGHeadingVariance => GAGAccordionButton.FindElements(By.ClassName("variance-summary-text")).First();

        protected static IWebElement GAGContentVariance => GAGAccordionSection.FindElements(By.ClassName("variance-summary-text")).First();

        #endregion
    }
}