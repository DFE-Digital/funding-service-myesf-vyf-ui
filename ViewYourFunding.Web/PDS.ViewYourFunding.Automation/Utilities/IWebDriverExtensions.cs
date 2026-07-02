using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;

namespace ViewYourFunding.Automation.Utilities
{
    /// <summary>
    /// The web driver extensions.
    /// </summary>
    public static class IWebDriverExtensions
    {
        /// <summary>
        /// Asserts the should exist.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="by">The by.</param>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        public static void AssertShouldExist(this IWebDriver driver, By by, int timeoutInSeconds = 15)
        {
            Assert.IsNotNull(driver.WaitToFindElement(by, timeoutInSeconds), "Element could not be found when expected. " + by);
        }

        /// <summary>
        /// Asserts the should not exist.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="by">The by.</param>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        public static void AssertShouldNotExist(this IWebDriver driver, By by, int timeoutInSeconds = 2)
        {
            try
            {
                var waitToFindElement = driver.WaitToFindElement(@by, timeoutInSeconds);
                Assert.IsNull(waitToFindElement, "Element was found when not expected. " + by);
            }
            catch (WebDriverTimeoutException)
            {
                // this is expected
            }
        }

        /// <summary>
        /// Waits to find element invisibility.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="by">The by.</param>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        /// <returns>Element visibility.</returns>
        public static bool WaitToFindElementInvisibility(this IWebDriver driver, By by, int timeoutInSeconds = 15)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
#pragma warning disable CS0618 // Type or member is obsolete
                return wait.Until(ExpectedConditions.InvisibilityOfElementLocated(by));
#pragma warning restore CS0618 // Type or member is obsolete
            }

            return false;
        }

        /// <summary>
        /// Waits for the element content to change.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="webElement">The web element.</param>
        /// <param name="originalContentText">The original content text.</param>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        /// <returns>
        /// Whether the element content has changed within timeout period.
        /// </returns>
        public static bool WaitToFindElementContentHasChangedFromOriginalContent(this IWebDriver driver, IWebElement webElement, string originalContentText, int timeoutInSeconds = 15)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(ElementContentHasChanged(webElement, originalContentText));
            }

            return false;
        }

        /// <summary>
        /// Waits to find element.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="by">The by.</param>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        /// <returns>The web element.</returns>
        public static IWebElement WaitToFindElement(this IWebDriver driver, By by, int timeoutInSeconds = 15)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }

            return driver.FindElement(by);
        }

        /// <summary>
        /// Waits to find elements.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="by">The by.</param>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        /// <returns>the read only collection of web elements.</returns>
        public static ReadOnlyCollection<IWebElement> WaitToFindElements(this IWebDriver driver, By by, int timeoutInSeconds = 15)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElements(by));
            }

            return driver.FindElements(by);
        }

        /// <summary>
        /// Returns if visible.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="by">The by.</param>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        /// <returns>The web element.</returns>
        public static IWebElement ReturnIfVisible(this IWebDriver driver, By by, int timeoutInSeconds = 1)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
#pragma warning disable CS0618 // Type or member is obsolete
                return wait.Until(ExpectedConditions.ElementIsVisible(by));
#pragma warning restore CS0618 // Type or member is obsolete
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }

        /// <summary>
        /// Checks the element content has changed from the given content.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="originalContentText">The original content text.</param>
        /// <returns>
        /// Whether the element content has been changed.
        /// </returns>
        public static Func<IWebDriver, bool> ElementContentHasChanged(IWebElement element, string originalContentText)
        {
            return driver =>
            {
                try
                {
                    return !element.Text.Contains(originalContentText);
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            };
        }
    }
}