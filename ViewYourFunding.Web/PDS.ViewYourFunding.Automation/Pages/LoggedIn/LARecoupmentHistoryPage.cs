using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Linq;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;


namespace PDS.ViewYourFunding.Automation.Pages.LoggedIn
{
    /// <summary>
    /// The la recoupment history page class.
    /// </summary>
    public class LARecoupmentHistoryPage : ViewYourFundingBasePage
    {
        #region Actions

        /// <summary>
        /// Logs in and navigates to recoupment history.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void NavigateToLaRecoupmentHistoryPageViaLogin(string username, string password)
        {
            ProviderPage.NavigateToRecoupmentHistoryPageViaLogin(username, password);
        }

        #endregion

        #region Assertions

        /// <summary>
        /// Ensures the tab elements are present.
        /// </summary>
        public static void EnsureTabElements()
        {
            SubHeadingTextCss.Displayed.Should().BeTrue();
        }

        public static void EnsureRecoupmentReportHeadingAccordionSections()
        {
            AccordionContainer.Displayed.Should().BeTrue();

            var reportHeadingCount = Driver.Instance.FindElements(By.CssSelector(".heading-large")).Count;

            reportHeadingCount.Should().Be(3);
        }

        public static void EnsureRecoupmentReportTableSections()
        {
            ReportTableElement.Displayed.Should().BeTrue();

            var reportTableCount = Driver.Instance.FindElements(By.TagName("table")).Count;

            reportTableCount.Should().Be(3);
        }

        /// <summary>
        /// Ensures the Report data links is displayed.
        /// </summary>
        public static void EnsureReportDataLinksDisplayed()
        {
            ReportDataLink_11November2022.Displayed.Should().BeTrue();
            ReportDataLink_25October2022.Displayed.Should().BeTrue();

            var links_25October2020 = Driver.Instance.FindElements(By.LinkText("25 October 2020"));
            var links_11November2020 = Driver.Instance.FindElements(By.LinkText("11 November 2020"));
            var links_25October2021 = Driver.Instance.FindElements(By.LinkText("25 October 2021"));
            var links_11November2021 = Driver.Instance.FindElements(By.LinkText("11 November 2021"));
            var links_25October2022 = Driver.Instance.FindElements(By.LinkText("25 October 2022"));
            var links_11November2022 = Driver.Instance.FindElements(By.LinkText("11 November 2022"));

            links_25October2020.Count().Should().Be(1);
            links_11November2020.Count().Should().Be(1);
            links_25October2021.Count().Should().Be(1);
            links_11November2021.Count().Should().Be(1);
            links_25October2022.Count().Should().Be(1);
            links_11November2022.Count().Should().Be(1);
        }

        /// <summary>
        /// Ensures the Report table data links count correct.
        /// </summary>
        public static void EnsureRecoupmentReportDataPublicationLinks()
        {
            ReportTableDataElement_2022To2023.Displayed.Should().BeTrue();
            ReportTableDataElement_2021To2022.Displayed.Should().BeTrue();
            ReportTableDataElement_2020To2021.Displayed.Should().BeTrue();

            var publicationLink_2022To2023 = Driver.Instance.FindElements(By.CssSelector(".publication-link-2022-to-2023"));
            var publicationLink_2021To2022 = Driver.Instance.FindElements(By.CssSelector(".publication-link-2021-to-2022"));
            var publicationLink_2020To2021 = Driver.Instance.FindElements(By.CssSelector(".publication-link-2020-to-2021"));

            publicationLink_2022To2023.Count().Should().Be(2);
            publicationLink_2021To2022.Count().Should().Be(2);
            publicationLink_2020To2021.Count().Should().Be(2);
        }

        /// <summary>
        /// Ensures the Report table a data texts displayed.
        /// </summary>
        public static void EnsureRecoupmentReportDataPublicationLinkTexts()
        {
            var publicationLink_2022To2023Text =
            Driver.Instance.FindElements(By.CssSelector(".publication-link-2022-to-2023 a")).First();
            publicationLink_2022To2023Text.Displayed.Should().BeTrue();
            publicationLink_2022To2023Text.Text.Should().BeEquivalentTo("11 November 2022");

            var publicationLink_2022To2023Text_Last =
            Driver.Instance.FindElements(By.CssSelector(".publication-link-2022-to-2023 a")).Last();
            publicationLink_2022To2023Text_Last.Displayed.Should().BeTrue();
            publicationLink_2022To2023Text_Last.Text.Should().BeEquivalentTo("25 October 2022");

            var publicationLink_2021To2022Text =
            Driver.Instance.FindElements(By.CssSelector(".publication-link-2021-to-2022 a")).First();
            publicationLink_2021To2022Text.Displayed.Should().BeTrue();
            publicationLink_2021To2022Text.Text.Should().BeEquivalentTo("11 November 2021");

            var publicationLink_2021To2022Text_Last =
            Driver.Instance.FindElements(By.CssSelector(".publication-link-2021-to-2022 a")).Last();
            publicationLink_2021To2022Text_Last.Displayed.Should().BeTrue();
            publicationLink_2021To2022Text_Last.Text.Should().BeEquivalentTo("25 October 2021");

            var publicationLink_2020To2021Text =
            Driver.Instance.FindElements(By.CssSelector(".publication-link-2020-to-2021 a")).First();
            publicationLink_2020To2021Text.Displayed.Should().BeTrue();
            publicationLink_2020To2021Text.Text.Should().BeEquivalentTo("11 November 2020");

            var publicationLink_2020To2021Text_Last =
            Driver.Instance.FindElements(By.CssSelector(".publication-link-2020-to-2021 a")).Last();
            publicationLink_2020To2021Text_Last.Displayed.Should().BeTrue();
            publicationLink_2020To2021Text_Last.Text.Should().BeEquivalentTo("25 October 2020");
        }

        /// <summary>
        /// Ensures the Report table total recoupment displayed.
        /// </summary>
        public static void EnsureRecoupmentReportDataPublicationTotalRecoupmentValue()
        {
            var publication_2022To2023 =
                Driver.Instance.FindElements(By.TagName("table")).First().FindElements(By.CssSelector(".right")).First();
            publication_2022To2023.Displayed.Should().BeTrue();
            publication_2022To2023.Text.Should().BeEquivalentTo("£10,000");

            var publication_2022To2023_Last =
            Driver.Instance.FindElements(By.TagName("table")).First().FindElements(By.CssSelector(".right")).Last();
            publication_2022To2023_Last.Displayed.Should().BeTrue();
            publication_2022To2023_Last.Text.Should().BeEquivalentTo("£10,000");

            var publication_2020To2021 =
            Driver.Instance.FindElements(By.TagName("table")).Last().FindElements(By.CssSelector(".right")).First();
            publication_2020To2021.Displayed.Should().BeTrue();
            publication_2020To2021.Text.Should().BeEquivalentTo("£10,000");

            var publication_2020To2021_Last =
            Driver.Instance.FindElements(By.TagName("table")).Last().FindElements(By.CssSelector(".right")).Last();
            publication_2020To2021.Displayed.Should().BeTrue();
            publication_2020To2021.Text.Should().BeEquivalentTo("£10,000");
        }

        #endregion

        #region Page Elements

        /// <summary>
        /// Gets the recoupment history accordion.
        /// </summary>
        protected static IWebElement Accordion
            => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-funding-LAREC-1 .govuk-accordion__section-button"));

        /// <summary>
        /// Gets the recoupment report accordion content.
        /// </summary>
        protected static IWebElement AccordionId
            => Driver.Instance.FindElement(By.Id("content"));

        /// <summary>
        /// Gets the recoupment subheading css.
        /// </summary>
        protected static IWebElement SubHeadingTextCss
            => AccordionId.FindElement(By.CssSelector(".lede"));

        /// <summary>
        /// Gets the recoupment number of reports heading cssSelectors.
        /// </summary>
        protected static IWebElement ReportTableHeadingCss
            => AccordionId.FindElement(By.CssSelector(".heading-large .heading-guidance"));

        protected static IWebElement AccordionContainer => Driver.Instance.FindElement(By.CssSelector(".heading-large"));

        protected static IWebElement ReportTableElement => Driver.Instance.FindElement(By.TagName("table"));

        protected static IWebElement ReportTableDataElement_2022To2023 => Driver.Instance.FindElement(By.CssSelector(".publication-link-2022-to-2023"));

        protected static IWebElement ReportTableDataElement_2021To2022 => Driver.Instance.FindElement(By.CssSelector(".publication-link-2021-to-2022"));

        protected static IWebElement ReportTableDataElement_2020To2021 => Driver.Instance.FindElement(By.CssSelector(".publication-link-2020-to-2021"));

        /// <summary>
        /// Gets the report data 25 October 2022 link.
        /// </summary>
        protected static IWebElement ReportDataLink_25October2020 => Driver.Instance.FindElement(By.LinkText("25 October 2020"));

        /// <summary>
        /// Gets the report data 11 November 2022 link.
        /// </summary>
        protected static IWebElement ReportDataLink_11November2020 => Driver.Instance.FindElement(By.LinkText("11 November 2020"));

        /// <summary>
        /// Gets the report data 25 October 2022 link.
        /// </summary>
        protected static IWebElement ReportDataLink_25October2021 => Driver.Instance.FindElement(By.LinkText("25 October 2021"));

        /// <summary>
        /// Gets the report data 11 November 2022 link.
        /// </summary>
        protected static IWebElement ReportDataLink_11November2021 => Driver.Instance.FindElement(By.LinkText("11 November 2021"));

        /// <summary>
        /// Gets the report data 25 October 2022 link.
        /// </summary>
        protected static IWebElement ReportDataLink_25October2022 => Driver.Instance.FindElement(By.LinkText("25 October 2022"));

        /// <summary>
        /// Gets the report data 11 November 2022 link.
        /// </summary>
        protected static IWebElement ReportDataLink_11November2022 => Driver.Instance.FindElement(By.LinkText("11 November 2022"));




        #endregion
    }
}
