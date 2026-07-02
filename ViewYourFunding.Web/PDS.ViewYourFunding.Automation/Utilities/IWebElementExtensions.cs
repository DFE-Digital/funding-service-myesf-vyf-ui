using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;

namespace ViewYourFunding.Automation.Utilities
{
    /// <summary>
    /// The web element extensions.
    /// </summary>
    public static class IWebElementExtensions
    {
        /// <summary>
        /// An extension method on IWebElement to imitate a movement to an element and mouse click if radio button, otherwise imitate an enter key on it.
        /// Keeps track of the previous URL everytime this method is used.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="isRadioButton">if set to <c>true</c> [is RadioButton].</param>
        /// <returns>
        /// Returns the element that was passed on to it.
        /// </returns>
        public static IWebElement MoveAndClick(this IWebElement element, bool isRadioButton = false)
        {
            //var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            //wait.Until(ElementIsVisible(element));
            var actions = new Actions(Driver.Instance);
            actions.MoveToElement(element);
            actions.Perform();

            if (isRadioButton)
            {
                element.Click();
            }
            else
            {
                PreviousUrl = Driver.Instance.Url;
                element.SendKeys(Keys.Enter);
            }

            return element;
        }

        /// <summary>
        /// A helper method that navigates to the last url that was visited.
        /// </summary>
        public static void GotoPreviousUrl()
        {
            Driver.Instance.Navigate().GoToUrl(PreviousUrl);
        }

        /// <summary>
        /// Finds the element or default.
        /// </summary>
        /// <param name="webElement">The web element.</param>
        /// <param name="findBy">The find by.</param>
        /// <returns>The found web element.</returns>
        public static IWebElement FindElementOrDefault(this IWebElement webElement, By findBy)
        {
            try
            {
                return webElement.FindElement(findBy);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the element or default.
        /// </summary>
        /// <param name="findBy">The find by.</param>
        /// <returns>The found web element.</returns>
        public static IWebElement FindElementOrDefault(By findBy)
        {
            try
            {
                return Driver.Instance.FindElement(findBy);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        /// <summary>
        /// Downloads the link.
        /// </summary>
        /// <param name="webElement">The web element.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The Download file.</returns>
        /// <exception cref="Exception">The element doesn't have the a href attribute.</exception>
        public static string DownloadLink(this IWebElement webElement, string fileName)
        {
            var href = webElement.GetAttribute("href");

            if (href == null)
            {
                throw new Exception("The element doesn't have the a href attribute");
            }

            return Driver.DownloadFile(href, fileName);
        }

        /// <summary>
        /// Gets the particular column content for a table row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnNumber">The column number.</param>
        /// <returns>
        /// The web element.
        /// </returns>
        public static IWebElement GetCell(this IWebElement row, int columnNumber)
        {
            return row.FindElements(By.TagName("td"))[columnNumber];
        }

        /// <summary>
        /// Gets the particular column content for a table row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnNumber">The column number.</param>
        /// <param name="tagName">Tag name to find by e.g. "td".</param>
        /// <returns>
        /// The element for specified row, column and tag name.
        /// </returns>
        public static IWebElement GetCellByElementType(this IWebElement row, int columnNumber, string tagName)
        {
            return row.FindElements(By.TagName(tagName))[columnNumber];
        }

        /// <summary>
        /// Get the particular Header element of a table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnNumber">The column number.</param>
        /// <returns>The web element.</returns>
        public static IWebElement GetHeaderLinkElement(this IWebElement table, int columnNumber)
        {
            return table.FindElement(By.TagName("thead")).FindElements(By.TagName("a"))[columnNumber];
        }

        /// <summary>
        /// Gets the particular row from a table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="rowNumber">The row number.</param>
        /// <returns>The web element.</returns>
        public static IWebElement GetRow(this IWebElement table, int rowNumber)
        {
            return table.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"))[rowNumber];
        }

        /// <summary>
        /// Returns the number of rows in the table.
        /// </summary>
        /// <param name="table">The table to act on.</param>
        /// <returns>Number of rows.</returns>
        public static int GetNumberOfRows(this IWebElement table)
        {
            return table.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr")).Count;
        }

        /// <summary>
        /// Check whether the element is rendered/present on the page.
        /// </summary>
        /// <param name="by">The parameter to find the control by. e.g. the id.</param>
        /// <returns>
        /// Returns a bool value indicating if the element exists on the page or not.
        /// </returns>
        public static bool IsElementPresent(By by)
        {
            try
            {
                Driver.Instance.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        #region Assertions

        /// <summary>
        /// Shoulds the have text.
        /// </summary>
        /// <param name="webElement">The web element.</param>
        /// <param name="text">The text.</param>
        /// <returns>The returned web element.</returns>
        public static IWebElement ShouldHaveText(this IWebElement webElement, string text)
        {
            Assert.AreEqual(text, webElement.Text, "web elements text not as expected for " + webElement);
            return webElement;
        }

        /// <summary>
        /// Elements if visible.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The web element.</returns>
        private static IWebElement ElementIfVisible(IWebElement element)
        {
            return !element.Displayed ? null : element;
        }

        /// <summary>
        /// Elements the is visible.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The web element.</returns>
        private static Func<IWebDriver, IWebElement> ElementIsVisible(IWebElement element)
        {
            return driver =>
            {
                try
                {
                    return ElementIfVisible(element);
                }
                catch (StaleElementReferenceException)
                {
                    return null as IWebElement;
                }
            };
        }

        #endregion


        #region Previous Url Helper

        //Variable to keep track of the last url

        /// <summary>
        /// Gets or sets the previous URL.
        /// </summary>
        /// <value>
        /// The previous URL.
        /// </value>
        private static string PreviousUrl { get; set; }

        #endregion
    }
}