using OpenQA.Selenium;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The ViewingChoicePage class.
    /// </summary>
    public class ViewingChoicePage : ViewYourFundingBasePage
    {
        /// <summary>
        /// Clicks the continue button.
        /// </summary>
        public static void ClickContinueButton()
        {
            ContinueButton.MoveAndClick();
        }

        /// <summary>
        /// Selects the organisation option.
        /// </summary>
        public static void SelectOrganisationOption()
        {
            OrganisationOption.MoveAndClick(true);
        }

        /// <summary>
        /// Selects the national option.
        /// </summary>
        public static void SelectNationalOption()
        {
            NationalOption.MoveAndClick(true);
        }

        /// <summary>
        /// Ensures the current page.
        /// </summary>
        public static void EnsureCurrentPage()
        {
            EnsureCurrentPage("Choose how to view funding");
        }

        /// <summary>
        /// Gets the continue button.
        /// </summary>
        /// <value>
        /// The continue button.
        /// </value>
        protected static IWebElement ContinueButton => Driver.Instance.WaitToFindElement(By.ClassName("button"));

        /// <summary>
        /// Gets the organisation option.
        /// </summary>
        /// <value>
        /// The organisation option.
        /// </value>
        protected static IWebElement OrganisationOption => Driver.Instance.WaitToFindElement(By.Id("radio-1"));

        /// <summary>
        /// Gets the national option.
        /// </summary>
        /// <value>
        /// The national option.
        /// </value>
        protected static IWebElement NationalOption => Driver.Instance.WaitToFindElement(By.Id("radio-2"));
    }
}
