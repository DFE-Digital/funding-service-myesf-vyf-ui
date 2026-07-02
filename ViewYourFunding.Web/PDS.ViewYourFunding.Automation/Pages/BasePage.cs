using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages
{
    /// <summary>
    /// The Base Page class.
    /// </summary>
    public class BasePage
    {
        #region Assertions

        /// <summary>
        /// Ensures the current page.
        /// </summary>
        /// <param name="expectedText">The expected text.</param>
        /// <param name="allowContains">if set to <c>true</c> [allow contains].</param>
        public static void EnsureCurrentPage(string expectedText, bool allowContains = false)
        {
            MainTitleHeading?.Text.Should().NotBeNull();

            if (!allowContains)
            {
                MainTitleHeading.Text.Should().Be(expectedText);
            }
            else
            {
                MainTitleHeading.Text.Should().ContainEquivalentOf(expectedText);
            }
        }

        public static void EnsureLoggedIn()
        {
            AccountDetails.Should().HaveCount(1);
        }

        public static void EnsureLoggedOut()
        {
            AccountDetails.Should().HaveCount(0);
        }

        public static void EnsureSkillsFundingServiceHeaderLink()
        {
            SkillsFundingServiceHeaderLink.Text.Should().Be("Manage your education and skills funding");
        }

        public static void EnsureNffHeaderLink()
        {
            SkillsFundingServiceHeaderLink.Text.Should().Be("View national funding formula for schools");
        }

        /// <summary>
        /// Ensures the service name heading doesnt exist.
        /// </summary>
        public static void EnsureServiceNameHeadingDoesntExist()
        {
            Driver.Instance.AssertShouldNotExist(ServiceNameHeading);
        }

        /// <summary>
        /// Ensures the final breadcrumb text.
        /// </summary>
        /// <param name="text">The text.</param>
        public static void EnsureFinalBreadcrumbText(string text)
        {
            BreadcrumbFinalText.Should().Be(text);
        }

        /// <summary>
        /// Ensures the breadcrumb links text.
        /// </summary>
        /// <param name="linkTexts">The link texts.</param>
        public static void EnsureBreadcrumbLinksText(params string[] linkTexts)
        {
            var breadcrumbLinks = BreadcrumbLinks;

            linkTexts.Length.Should().Be(breadcrumbLinks.Count);

            for (var i = 0; i < linkTexts.Length; i++)
            {
                var linkText = linkTexts[i];
                var breadcrumbLinkText = breadcrumbLinks[i].Text;

                linkText.Should().Be(breadcrumbLinkText);
            }
        }

        #endregion


        #region Main Api

        /// <summary>
        /// Clicks the skills funding service header link.
        /// </summary>
        public static void ClickSkillsFundingServiceHeaderLink()
        {
            SkillsFundingServiceHeaderLink.MoveAndClick();
        }

        /// <summary>
        /// Goes to the specified relative URL.
        /// </summary>
        /// <param name="relativeUrl">The relative URL.</param>
        /// <param name="checkHeading">Whether to check the header is correct (optional - default is true).</param>
        protected static void Goto(string relativeUrl, bool checkHeading = true)
        {
            var config = ConfigHelper.GetIConfigurationRoot();
            var baseUrl = config["baseSiteUrl"];
            var uri = new Uri(new Uri(baseUrl), relativeUrl);

            Driver.Instance.Navigate().GoToUrl(uri);

            if (checkHeading)
            {
                MainTitleHeading.Text.Should().NotBe("This site can’t be reached", "{0} should be reachable", uri.OriginalString);
            }
        }

        protected static void GotoWithoutMainTitle(string relativeUrl)
        {
            var config = ConfigHelper.GetIConfigurationRoot();
            var baseUrl = config["baseSiteUrl"];
            var uri = new Uri(new Uri(baseUrl), relativeUrl);

            Driver.Instance.Navigate().GoToUrl(uri);
        }

        #endregion


        #region Controls

        /// <summary>
        /// Gets the main title heading.
        /// </summary>
        /// <value>
        /// The main title heading.
        /// </value>
        protected static IWebElement MainTitleHeading => Driver.Instance.WaitToFindElement(By.TagName("h1"));

        /// <summary>
        /// Gets the breadcrumb container.
        /// </summary>
        /// <value>
        /// The breadcrumb container.
        /// </value>
        protected static IWebElement BreadcrumbContainer => Driver.Instance.WaitToFindElement(By.Id("global-breadcrumb"));

        /// <summary>
        /// Gets the breadcrumb links.
        /// </summary>
        /// <value>
        /// The breadcrumb links.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> BreadcrumbLinks => BreadcrumbContainer.FindElements(By.TagName("a"));

        /// <summary>
        /// Gets the breadcrumb final text.
        /// </summary>
        /// <value>
        /// The breadcrumb final text.
        /// </value>
        protected static string BreadcrumbFinalText => BreadcrumbContainer.FindElement(By.TagName("ol")).FindElements(By.TagName("li")).Last().Text.Trim();

        /// <summary>
        /// Gets the support information.
        /// </summary>
        /// <value>
        /// The support information.
        /// </value>
        protected static IWebElement SupportInformation => Driver.Instance.WaitToFindElement(SupportInformationByLocator);

        /// <summary>
        /// Gets the support information by locator.
        /// </summary>
        /// <value>
        /// The support information by locator.
        /// </value>
        protected static By SupportInformationByLocator => By.Id("supportInformation");

        /// <summary>
        /// Gets the skills funding service header link.
        /// </summary>
        /// <value>
        /// The skills funding service header link.
        /// </value>
        protected static IWebElement SkillsFundingServiceHeaderLink => Driver.Instance.WaitToFindElement(By.Id("proposition-name"));

        /// <summary>
        /// Gets the accounts details div.
        /// </summary>
        /// <value>
        /// The account details div.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> AccountDetails => Driver.Instance.FindElements(By.ClassName("account-name"));

        /// <summary>
        /// Gets the current URL.
        /// </summary>
        /// <value>
        /// The current URL.
        /// </value>
        protected static string CurrentUrl => Driver.Instance.Url;

        /// <summary>
        /// Gets the service name heading.
        /// </summary>
        /// <value>
        /// The service name heading.
        /// </value>
        protected static By ServiceNameHeading => By.Id("ServiceName");

        #endregion


        #region Helper Methods

        /// <summary>
        /// For a given By object, we retrieve the interactable Web element on the page for it.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns>The web element.</returns>
        protected static IWebElement GetWebElementForBy(By b)
        {
            return Driver.Instance.WaitToFindElement(b);
        }

        #endregion
    }
}