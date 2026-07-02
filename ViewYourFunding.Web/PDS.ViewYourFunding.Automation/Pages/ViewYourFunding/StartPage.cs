using OpenQA.Selenium;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The StartPage class.
    /// </summary>
    public class StartPage : ViewYourFundingBasePage
    {
        /// <summary>
        /// Opens this instance.
        /// </summary>
        public static void Open()
        {
            Goto(string.Empty);
        }

        /// <summary>
        /// Clicks the start button.
        /// </summary>
        public static void ClickStartButton()
        {
            StartButton.MoveAndClick();
        }

        public static void ClickAcceptCookiesButton()
        {
            AcceptCookiesButton.Click();
        }

        public static void ClickHideCookieMessageButton()
        {
            HideCookieMessageButton.Click();
        }

        /// <summary>
        /// Gets the start button.
        /// </summary>
        /// <value>
        /// The start button.
        /// </value>
        protected static IWebElement StartButton => Driver.Instance.WaitToFindElement(By.ClassName("button-start"));

        protected static IWebElement AcceptCookiesButton => Driver.Instance.WaitToFindElement(By.Id("accept-button"));

        protected static IWebElement HideCookieMessageButton => Driver.Instance.WaitToFindElement(By.Id("hide-message-button-accept"));
    }
}
