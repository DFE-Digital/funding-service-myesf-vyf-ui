using FluentAssertions;
using OpenQA.Selenium;
using System.Linq;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The WhichAllocationPage class.
    /// </summary>
    public class WhichAllocationPage : ViewYourFundingBasePage
    {
        #region Actions

        /// <summary>
        /// Navigates to page.
        /// </summary>
        public static void NavigateToPage()
        {
            StartPage.Open();
            StartPage.ClickStartButton();

            ViewingChoicePage.SelectNationalOption();
            ViewingChoicePage.ClickContinueButton();
        }

        /// <summary>
        /// Clicks the continue button.
        /// </summary>
        public static void ClickContinueButton()
        {
            ContinueButton.MoveAndClick();
        }

        /// <summary>
        /// Clicks the progressive disclosure button.
        /// </summary>
        public static void ClickProgressiveDisclosureButton()
        {
            ProgressiveDisclosureButton.Click();
        }

        /// <summary>
        /// Selects the DSG.
        /// </summary>
        public static void SelectDSG()
        {
            Option_DSG.MoveAndClick(true);
        }

        /// <summary>
        /// Selects the PSG.
        /// </summary>
        public static void SelectPSG()
        {
            Option_PSG.MoveAndClick(true);
        }

        #endregion


        #region Assertions

        /// <summary>
        /// Ensures the current page.
        /// </summary>
        public static void EnsureCurrentPage()
        {
            EnsureCurrentPage("Select a funding type");
        }

        /// <summary>
        /// Ensures the content of the progressive disclosure.
        /// </summary>
        /// <param name="expectedVisible">if set to <c>true</c> [expected visible].</param>
        public static void EnsureProgressiveDisclosureContent(bool expectedVisible)
        {
            ProgressiveDisclosureContent.Displayed.Should().Be(expectedVisible);
        }

        #endregion


        #region Page Elements

        /// <summary>
        /// Gets the continue button.
        /// </summary>
        /// <value>
        /// The continue button.
        /// </value>
        protected static IWebElement ContinueButton => Driver.Instance.WaitToFindElement(By.ClassName("button"));

        /// <summary>
        /// Gets the progressive disclosure button.
        /// </summary>
        /// <value>
        /// The progressive disclosure button.
        /// </value>
        protected static IWebElement ProgressiveDisclosureButton => Driver.Instance.WaitToFindElement(By.Name("alternativeFunding"));

        /// <summary>
        /// Gets the content of the progressive disclosure.
        /// </summary>
        /// <value>
        /// The content of the progressive disclosure.
        /// </value>
        protected static IWebElement ProgressiveDisclosureContent => Driver.Instance.WaitToFindElement(By.Id("details-content-1"));

        /// <summary>
        /// Gets the option DSG.
        /// </summary>
        /// <value>
        /// The option DSG.
        /// </value>
        protected static IWebElement Option_DSG => Driver.Instance.WaitToFindElement(By.Id("radio-1"));

        /// <summary>
        /// Gets the option PSG.
        /// </summary>
        /// <value>
        /// The option PSG.
        /// </value>
        protected static IWebElement Option_PSG => Driver.Instance.WaitToFindElements(By.CssSelector(".multiple-choice.choice-required input")).LastOrDefault();

        #endregion
    }
}