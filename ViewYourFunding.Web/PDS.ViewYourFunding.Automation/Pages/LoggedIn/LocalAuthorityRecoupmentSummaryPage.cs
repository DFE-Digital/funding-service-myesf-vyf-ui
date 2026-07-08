using FluentAssertions;
using OpenQA.Selenium;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.LoggedIn
{
    public class LocalAuthorityRecoupmentSummaryPage : ViewYourFundingBasePage
    {
        #region Actions

        /// <summary>
        /// Logs in and navigates to recoupment reports.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void NavigateToLaRecoupmentSummaryPageViaLogin(string username, string password)
        {
            ProviderPage.NavigateToRecoupmentPageViaLogin(username, password);
        }

        /// <summary>
        /// Expands the report accordion.
        /// </summary>
        public static void ExpandAccordion()
        {
            Accordion.MoveAndClick();
        }

        /// <summary>
        /// Clicks the recoupment calculations link.
        /// </summary>
        public static void ClickRecoupmentCalculationsLink()
        {
            RecoupmentCalculationsLink.MoveAndClick();
        }

        #endregion

        #region Assertions

        /// <summary>
        /// Ensures the recoupment history link is present.
        /// </summary>
        public static void EnsureRecoupmentHistory()
        {
            RecoupmentHistoryLink.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Ensures the tab links are present.
        /// </summary>
        public static void EnsureTabLinks()
        {
            RecoupmentCalculationsLink.Displayed.Should().BeTrue();
            AnomaliesLink.Displayed.Should().BeTrue();
            PhaseTotalsLink.Displayed.Should().BeTrue();
        }


        #endregion

        #region Page Elements


        /// <summary>
        /// Gets the recoupment history link.
        /// </summary>
        protected static IWebElement RecoupmentHistoryLink
            => Driver.Instance.WaitToFindElement(By.LinkText("View history of recoupment"));

        /// <summary>
        /// Gets the recoupment report accordion.
        /// </summary>
        protected static IWebElement Accordion
            => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-funding-LAREC-1 .govuk-accordion__section-button"));

        /// <summary>
        /// Gets the recoupment report accordion content.
        /// </summary>
        protected static IWebElement AccordionDiv
            => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-funding-LAREC-1 .govuk-accordion__section-content"));

        /// <summary>
        /// Gets the recoupment calculations tab link.
        /// </summary>
        protected static IWebElement RecoupmentCalculationsLink
            => AccordionDiv.FindElement(By.LinkText("Recoupment calculations"));

        /// <summary>
        /// Gets the anolmalies tab link.
        /// </summary>
        protected static IWebElement AnomaliesLink
            => AccordionDiv.FindElement(By.LinkText("Recoupment calculations"));

        /// <summary>
        /// Gets the phase totals tab link.
        /// </summary>
        protected static IWebElement PhaseTotalsLink
            => AccordionDiv.FindElement(By.LinkText("Recoupment calculations"));


        #endregion
    }
}