using FluentAssertions;
using OpenQA.Selenium;
using System.Linq;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The ProviderDetailPage class.
    /// </summary>
    public class ProviderDetailPage : ViewYourFundingBasePage
    {
        /// <summary>
        /// The provider name.
        /// </summary>
        private const string ProviderName = "St Mary's Kilburn Church of England Primary School";

        #region Actions

        /// <summary>
        /// Navigates to page.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        public static void NavigateToPage(string providerName = "")
        {
            if (string.IsNullOrEmpty(providerName))
            {
                providerName = ProviderName;
            }

            StartPage.Open();
            StartPage.ClickStartButton();
            ViewingChoicePage.SelectOrganisationOption();
            ViewingChoicePage.ClickContinueButton();
            FindAnOrganisationPage.ClickProviderOptionButton();
            FindAnOrganisationPage.InputProviderText(providerName);
            FindAnOrganisationPage.SubmitProviderSearch();
            ResetAllSectionsToCollapsed();
        }

        /// <summary>
        /// Navigates to via did you mean page.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        public static void NavigateToViaDidYouMeanPage(string searchTerm)
        {
            StartPage.Open();
            StartPage.ClickStartButton();
            ViewingChoicePage.SelectOrganisationOption();
            ViewingChoicePage.ClickContinueButton();
            FindAnOrganisationPage.ClickProviderOptionButton();
            FindAnOrganisationPage.InputProviderText(searchTerm);
            FindAnOrganisationPage.SubmitProviderSearch();
            ProviderResultsPage.ClickProviderLink(ProviderName);
            ResetAllSectionsToCollapsed();
        }

        /// <summary>
        /// Resets all sections to collapsed.
        /// </summary>
        public static void ResetAllSectionsToCollapsed()
        {
            if (MainSectionIsOpen)
            {
                ToggleMainContent();
            }

            if (AlternativeFundingSectionIsOpen)
            {
                ToggleAlternativeFunding();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the library remembers the last state the accordion was in on the page. So we need to check if it loads as open or closed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [main section is open]; otherwise, <c>false</c>.
        /// </value>
        protected static bool MainSectionIsOpen =>
            MainProviderDetailsArea.Displayed;

        /// <summary>
        /// Gets a value indicating whether [alternative funding section is open].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [alternative funding section is open]; otherwise, <c>false</c>.
        /// </value>
        protected static bool AlternativeFundingSectionIsOpen =>
            AlternativeFundingsArea.Displayed;


        /// <summary>
        /// Toggles the content of the main.
        /// </summary>
        public static void ToggleMainContent()
        {
            ToggleMainContentButton.MoveAndClick();
        }

        /// <summary>
        /// Toggles the alternative funding.
        /// </summary>
        public static void ToggleAlternativeFunding()
        {
            AlternativeFundingButton.Click();
        }

        /// <summary>
        /// Opens the content of the main.
        /// </summary>
        public static void OpenMainContent()
        {
            if (!MainProviderDetailsArea.Displayed)
            {
                ToggleMainContent();
            }
        }

        /// <summary>
        /// Navigates to PSG allocation history page.
        /// </summary>
        public static void NavigateToPSGAllocationHistoryPage()
        {
            AllocationHistoryLink.Click();
        }

        #endregion


        #region Assertions

        /// <summary>
        /// Ensures the current provider page.
        /// </summary>
        public static void EnsureCurrentProviderPage()
        {
            EnsureCurrentPage(ProviderName);
        }

        /// <summary>
        /// Ensures the next available payment date.
        /// </summary>
        /// <param name="expectedDate">The expected date.</param>
        public static void EnsureNextAvailablePaymentDate(string expectedDate)
        {
            NextAvailablePaymentDate.Text.Should().Be(expectedDate);
        }


        /// <summary>
        /// Ensures the no next available payment date text.
        /// </summary>
        /// <param name="expectedText">The expected text.</param>
        public static void EnsureNoNextAvailablePaymentDateText(string expectedText)
        {
            NoNextAvailablePaymentDateText.Text.Should().Be(expectedText);
        }

        /// <summary>
        /// Ensures the content of the main.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        public static void EnsureMainContent(bool visible = false)
        {
            MainProviderDetailsArea.Displayed.Should().Be(visible);
        }

        /// <summary>
        /// Expands the PSG.
        /// </summary>
        public static void ExpandPSG()
        {
            if (!Section1Div.Displayed)
            {
                Section1Button.Click();
            }
        }

        /// <summary>
        /// Ensures the alternative fundings.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        public static void EnsureAlternativeFundings(bool visible = false)
        {
            AlternativeFundingsArea.Displayed.Should().Be(visible);

            if (visible)
            {
                EnsureElementByTagAndTextIsDisplayed(AlternativeFundingsArea, "p", "All users can view the funding types above.");
                EnsureElementByTagAndTextIsDisplayed(AlternativeFundingsArea, "p", "There are more funding types that only the organisation that receives them can view.");
            }
        }

        /// <summary>
        /// Ensures the link is displayed.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="expectedLinkText">The expected link text.</param>
        /// <param name="expectedUrl">The expected URL.</param>
        public static void EnsureLinkIsDisplayed(IWebElement parent, string expectedLinkText, string expectedUrl)
        {
            var link = parent.FindElement(By.LinkText(expectedLinkText));
            link.Displayed.Should().BeTrue();

            var target = link.GetAttribute("href");
            target.Should().Contain(expectedUrl);
        }

        /// <summary>
        /// Ensures the element by tag and text is displayed.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="expectedText">The expected text.</param>
        public static void EnsureElementByTagAndTextIsDisplayed(IWebElement parent, string tagName, string expectedText)
        {
            parent.FindElements(By.TagName(tagName)).Any(item => item.Text.Contains(expectedText)).Should().BeTrue();
        }

        #endregion


        #region Page Elements

        /// <summary>
        /// Gets the allocation history link.
        /// </summary>
        /// <value>
        /// The allocation history link.
        /// </value>
        protected static IWebElement AllocationHistoryLink => Driver.Instance.WaitToFindElement(By.LinkText("allocation history"));

        /// <summary>
        /// Gets the alternative fundings area.
        /// </summary>
        /// <value>
        /// The alternative fundings area.
        /// </value>
        protected static IWebElement AlternativeFundingsArea => Driver.Instance.WaitToFindElement(By.Id("details-content-1"));

        /// <summary>
        /// Gets the main provider details area.
        /// </summary>
        /// <value>
        /// The main provider details area.
        /// </value>
        protected static IWebElement MainProviderDetailsArea => Driver.Instance.WaitToFindElement(By.Id("accordion-default-content-1"));

        /// <summary>
        /// Gets the toggle main content button.
        /// </summary>
        /// <value>
        /// The toggle main content button.
        /// </value>
        protected static IWebElement ToggleMainContentButton => Driver.Instance.WaitToFindElement(By.Id("accordion-default-heading-1"));

        /// <summary>
        /// Gets the alternative funding button.
        /// </summary>
        /// <value>
        /// The alternative funding button.
        /// </value>
        protected static IWebElement AlternativeFundingButton => Driver.Instance.WaitToFindElement(By.Name("alternativeFunding"));

        /// <summary>
        /// Gets the next available payment date.
        /// </summary>
        /// <value>
        /// The next available payment date.
        /// </value>
        protected static IWebElement NextAvailablePaymentDate => Driver.Instance.FindElement(By.CssSelector("h3.heading-medium.heading-guidance + p > span"));

        /// <summary>
        /// Gets the no next available payment date text.
        /// </summary>
        /// <value>
        /// The no next available payment date text.
        /// </value>
        protected static IWebElement NoNextAvailablePaymentDateText => Driver.Instance.FindElement(By.CssSelector("h3.heading-medium.heading-guidance + p"));

        /// <summary>
        /// Gets the section1 button.
        /// </summary>
        /// <value>
        /// The section1 button.
        /// </value>
        protected static IWebElement Section1Button => Driver.Instance.FindElement(By.Id("accordion-default-heading-1"));

        /// <summary>
        /// Gets the section1 div.
        /// </summary>
        /// <value>
        /// The section1 div.
        /// </value>
        protected static IWebElement Section1Div => Driver.Instance.FindElement(By.Id("accordion-default-content-1"));

        #endregion

    }
}