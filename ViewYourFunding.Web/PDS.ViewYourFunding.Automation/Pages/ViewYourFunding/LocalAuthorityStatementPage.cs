using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The LocalAuthorityStatementPage class.
    /// </summary>
    public class LocalAuthorityStatementPage : ViewYourFundingBasePage
    {
        /// <summary>
        /// The expected next payment date format.
        /// </summary>
        private const string ExpectedNextPaymentDateFormat = "d MMMM yyyy";

        #region Actions

        /// <summary>
        /// Navigates to page via did you mean page.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="localAuthorityName">Name of the local authority.</param>
        public static void NavigateToPageViaDidYouMeanPage(string searchTerm, string localAuthorityName)
        {
            StartPage.Open();
            StartPage.ClickStartButton();
            ViewingChoicePage.SelectOrganisationOption();
            ViewingChoicePage.ClickContinueButton();
            FindAnOrganisationPage.ClickLaCodeOptionButton();
            FindAnOrganisationPage.InputLocalAuthoritySearchText(searchTerm);
            FindAnOrganisationPage.SubmitLocalAuthoritySearch();
            LocalAuthorityDidYouMeanPage.ClickLocalAuthorityLink(localAuthorityName);
        }

        /// <summary>
        /// Navigates to page using exact search.
        /// </summary>
        /// <param name="localAuthorityCode">The local authority code.</param>
        public static void NavigateToPageUsingExactSearch(string localAuthorityCode)
        {
            StartPage.Open();
            StartPage.ClickStartButton();
            ViewingChoicePage.SelectOrganisationOption();
            ViewingChoicePage.ClickContinueButton();
            FindAnOrganisationPage.ClickLaCodeOptionButton();
            FindAnOrganisationPage.InputLocalAuthoritySearchText(localAuthorityCode);
            FindAnOrganisationPage.SubmitLocalAuthoritySearch();
        }

        /// <summary>
        /// Clicks the funding block.
        /// </summary>
        /// <param name="blockName">Name of the block.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void ClickFundingBlock(string blockName, int yearFrom, int yearTo)
        {
            foreach (var section in AccordionSections)
            {
                var headerButton = section.FindElement(By.Id("accordion-default-heading-1"));
                if (!headerButton.Text.Contains("Dedicated schools grant"))
                {
                    continue;
                }

                var sectionExpandedAttribute = headerButton.GetAttribute("aria-expanded");

                if (sectionExpandedAttribute != true.ToString().ToLower())
                {
                    headerButton.MoveAndClick();
                }

                var sectionContent = section.FindElement(By.Id("accordion-default-content-1"));

                var fundingBlocks = sectionContent.FindElements(By.ClassName("funding-block"));

                switch (blockName)
                {
                    case "Schools":
                        fundingBlocks[0].FindElement(By.TagName("a")).MoveAndClick();
                        break;
                    case "Central school services":
                        fundingBlocks[1].FindElement(By.TagName("a")).MoveAndClick();
                        break;
                    case "High needs":
                        fundingBlocks[2].FindElement(By.TagName("a")).MoveAndClick();
                        break;
                    case "Early years":
                        fundingBlocks[3].FindElement(By.TagName("a")).MoveAndClick();
                        break;
                }

                break;
            }
        }

        #endregion


        #region Assertions

        /// <summary>
        /// Ensures the accordion sections count.
        /// </summary>
        /// <param name="expectedCount">The expected count.</param>
        public static void EnsureAccordionSectionsCount(int expectedCount)
        {
            AccordionSections.Should().NotBeNull().And.HaveCount(expectedCount);
        }

        /// <summary>
        /// Clicks the DSG schools block.
        /// </summary>
        public static void ClickDSGSchoolsBlock()
        {
            SchoolsBlockLink.Click();
        }

        /// <summary>
        /// Opens the DSG accordion.
        /// </summary>
        public static void OpenDSGAccordion()
        {
            foreach (var section in AccordionSections)
            {
                var headerButton = section.FindElement(By.Id("accordion-default-heading-1"));
                if (!headerButton.Text.Contains("Dedicated schools grant"))
                {
                    continue;
                }

                var open = headerButton.GetAttribute("aria-expanded").Equals("true");
                var times = 0;

                while (!open && times++ <= 5)
                {
                    if (times > 1)
                    {
                        Thread.Sleep(1000);
                    }

                    headerButton.Click();
                    open = headerButton.GetAttribute("aria-expanded").Equals("true");
                }

                break;
            }
        }

        /// <summary>
        /// Ensures the DSG sections for each active year.
        /// </summary>
        /// <param name="activeYears">An array of year from, year to pairs.</param>
        /// <param name="nextPaymentDates">The next payment dates by financial year.</param>
        /// <param name="noNextPaymentDateTexts">Text to show when no next Payment date available.</param>
        public static void EnsureActiveYearsDSGSections((int YearFrom, int YearTo)[] activeYears, List<(string, DateTime)> nextPaymentDates, IDictionary<(int, int), string> noNextPaymentDateTexts)
        {
            // Get only the DSG accordion sections.
            var dsgSections = AccordionSections.Where((section, index) => section.FindElement(By.Id($"accordion-default-heading-{index + 1}"))?.Text?.Contains("Dedicated schools grant") == true);

            var loopIndex = 1;
            foreach (var dsgSection in dsgSections)
            {
                // Extract the year from/to displayed in this accordion section.
                var sectionHeadingText = dsgSection.FindElement(By.Id($"accordion-default-heading-{loopIndex}")).Text;
                var yearFromToRegexMatch = Regex.Match(sectionHeadingText, @"(\d{4}) to (\d{4})");
                int.TryParse(yearFromToRegexMatch.Groups[1].Value, out int yearFrom);
                int.TryParse(yearFromToRegexMatch.Groups[2].Value, out int yearTo);

                // Get the active year that matches the section.
                var (activeYearFrom, activeYearTo) = activeYears.First(years => (years.YearFrom == yearFrom) && (years.YearTo == yearTo));

                // Make sure the section has the expected data.
                EnsureDSGSection(
                    dsgSection,
                    loopIndex++,
                    activeYearFrom,
                    activeYearTo,
                    nextPaymentDates.First(a => a.Item1 == $"FY-{yearFrom - 2000}{yearTo - 2000}").Item2,
                    noNextPaymentDateTexts[(yearFrom, yearTo)]);
            }
        }

        /// <summary>
        /// Ensures the PSG section.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="nextPaymentDate">The next payment date.</param>
        /// <param name="noNextPaymentDateText">The text to show when no next payment date available.</param>
        public static void EnsurePSGSection(int yearFrom, int yearTo, DateTime nextPaymentDate, string noNextPaymentDateText)
        {
            bool foundSection = false;

            foreach (var section in AccordionSections)
            {
                var headerButton = section.FindElement(By.ClassName("govuk-accordion__section-button"));
                if (!headerButton.Text.Contains("PE and sport premium"))
                {
                    continue;
                }

                var sectionExpandedAttribute = headerButton.GetAttribute("aria-expanded");
                if (sectionExpandedAttribute != true.ToString().ToLower())
                {
                    headerButton.MoveAndClick();
                }

                var leftHeader = headerButton.FindElement(By.ClassName("govuk-accordion__heading-text-left"));
                leftHeader.Text.Should()
                    .Be($"PE and sport premium allocation for academic year {yearFrom} to {yearTo}");

                var rightHeaders = headerButton.FindElements(By.ClassName("govuk-accordion__heading-number-right"));
                rightHeaders.Should().ContainSingle();
                rightHeaders.Single().Text.Should().Contain("£").And.NotContain(".");

                var sectionSummary = section.FindElement(By.Id("accordion-with-summary-sections-summary-3"));
                sectionSummary.Text.Should().Contain("Published:");

                var sectionContent = section.FindElement(By.Id("accordion-default-content-3"));
                sectionContent.Displayed.Should().BeTrue();

                var breakdownLink = sectionContent.FindElement(By.Id("funding-breakdown-link"));
                breakdownLink.Text.Should().Be("View all schools' and academies' individual allocations");

                var documentLinks = sectionContent.FindElements(By.ClassName("download-a-document-container"));
                documentLinks.Should().ContainSingle();

                if (nextPaymentDate != DateTime.MinValue)
                {
                    var nextPayment = sectionContent.FindElement(By.Id("next-payment-date"));
                    nextPayment.Text.Should().Contain($"Next allocation payment: {nextPaymentDate.ToString(ExpectedNextPaymentDateFormat)}.");
                }
                else
                {
                    var noNextPaymentDateTextSelector = section.FindElements(By.CssSelector("h3.heading-medium.heading-guidance + p"));
                    noNextPaymentDateTextSelector[1].Text.Should().Be(noNextPaymentDateText);
                }

                foundSection = true;
                break;
            }

            foundSection.Should().BeTrue("there should be a PSG section on the page");
        }

        /// <summary>
        /// Clicks the PSG funding breakdown link.
        /// </summary>
        /// <exception cref="Exception">PSG funding breakdown link was not found.</exception>
        public static void ClickPSGFundingBreakdownLink()
        {
            foreach (var section in AccordionSections)
            {
                var headerButton = section.FindElement(By.ClassName("govuk-accordion__section-button"));
                if (!headerButton.Text.Contains("PE and sport premium"))
                {
                    continue;
                }

                var sectionExpandedAttribute = headerButton.GetAttribute("aria-expanded");
                if (sectionExpandedAttribute != true.ToString().ToLower())
                {
                    headerButton.MoveAndClick();
                }

                var sectionContent = section.FindElement(By.Id("accordion-default-content-3"));
                var breakdownLink = sectionContent.FindElement(By.Id("funding-breakdown-link"));
                var anchor = breakdownLink.FindElement(By.TagName("a"));
                anchor.MoveAndClick();
                return;
            }

            throw new Exception("PSG funding breakdown link was not found.");
        }

        /// <summary>
        /// Clicks the PSG allocation history link.
        /// </summary>
        /// <exception cref="Exception">PSG allocation history link was not found.</exception>
        public static void ClickPSGAllocationHistoryLink()
        {
            foreach (var section in AccordionSections)
            {
                var headerButton = section.FindElement(By.ClassName("govuk-accordion__section-button"));
                if (!headerButton.Text.Contains("PE and sport premium"))
                {
                    continue;
                }

                var sectionExpandedAttribute = headerButton.GetAttribute("aria-expanded");
                if (sectionExpandedAttribute != true.ToString().ToLower())
                {
                    headerButton.MoveAndClick();
                }

                var sectionContent = section.FindElement(By.Id("accordion-default-content-3"));
                var historyLink = sectionContent.FindElement(By.Id("allocation-history-link-psg"));
                var anchor = historyLink.FindElement(By.TagName("a"));
                anchor.MoveAndClick();
                return;
            }

            throw new Exception("PSG allocation history link was not found.");
        }

        /// <summary>
        /// Clicks the DSG allocation history link.
        /// </summary>
        /// <exception cref="Exception">DSG allocation history link was not found.</exception>
        public static void ClickDSGAllocationHistoryLink()
        {
            foreach (var section in AccordionSections)
            {
                var headerButton = section.FindElement(By.Id("accordion-default-heading-1"));
                if (!headerButton.Text.Contains("Dedicated schools grant"))
                {
                    continue;
                }

                var sectionExpandedAttribute = headerButton.GetAttribute("aria-expanded");
                if (sectionExpandedAttribute != true.ToString().ToLower())
                {
                    headerButton.MoveAndClick();
                }

                var sectionContent = section.FindElement(By.Id("accordion-default-content-1"));
                var historyLink = sectionContent.FindElement(By.Id("allocation-history-link-dsg"));
                var anchor = historyLink.FindElement(By.TagName("a"));
                anchor.MoveAndClick();
                return;
            }

            throw new Exception("DSG allocation history link was not found.");
        }

        /// <summary>
        /// Ensures the alternative funding.
        /// </summary>
        public static void EnsureAlternativeFunding()
        {
            AlternativeFunding.Should().NotBeNull();
            AlternativeFunding.Text.Should().Be("I'm looking for another funding type");
        }

        #endregion


        /// <summary>
        /// Ensures the DSG section.
        /// </summary>
        /// <param name="section">The DSG accordion web element.</param>
        /// <param name="sectionIndex">The DSG accordion web element position.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="nextPaymentDate">The next payment date.</param>
        /// <param name="noNextPaymentDateText">Text to show when no next Payment date available.</param>
        private static void EnsureDSGSection(IWebElement section, int sectionIndex, int yearFrom, int yearTo, DateTime nextPaymentDate, string noNextPaymentDateText)
        {
            var headerButton = section.FindElement(By.Id($"accordion-default-heading-{sectionIndex}"));

            var sectionExpandedAttribute = headerButton.GetAttribute("aria-expanded");
            if (sectionExpandedAttribute != true.ToString().ToLower())
            {
                headerButton.MoveAndClick();
            }

            var leftHeader = headerButton.FindElement(By.ClassName("govuk-accordion__heading-text-left"));
            leftHeader.Text.Should()
                .Be($"Dedicated schools grant (DSG) allocation for financial year {yearFrom} to {yearTo}");

            var sectionSummary = section.FindElement(By.Id($"accordion-with-summary-sections-summary-{sectionIndex}"));
            sectionSummary.Text.Should().Contain("Published:");

            var rightHeaders = headerButton.FindElements(By.ClassName("govuk-accordion__heading-number-right"));
            rightHeaders.Should().HaveCount(2);
            rightHeaders.First().Text.Should().Contain("£").And.NotContain(".");
            if (!sectionSummary.Text.Contains("December"))
            {
                rightHeaders.Last().Text.Should().Be("after recoupment and deductions");
            }
            else
            {
                rightHeaders.Last().Text.Should().Be("after high needs deductions");
            }

            var sectionContent = section.FindElement(By.Id($"accordion-default-content-{sectionIndex}"));
            sectionContent.Displayed.Should().BeTrue();

            var fundingBlocks = sectionContent.FindElements(By.ClassName("funding-block"));
            fundingBlocks.Should().HaveCount(4);

            fundingBlocks[0].Text.Should().Contain("Schools block")
                .And.Contain("£").And.NotContain(".");

            fundingBlocks[1].Text.Should().Contain("Central school services block")
                .And.Contain("£").And.NotContain(".");

            fundingBlocks[2].Text.Should().Contain("High needs block")
                .And.Contain("£").And.NotContain(".");

            fundingBlocks[3].Text.Should().Contain("Early years block")
                .And.Contain("£").And.NotContain(".");

            var documentLinks = sectionContent.FindElements(By.ClassName("download-a-document-container"));
            documentLinks.Should().ContainSingle();

            if (nextPaymentDate != DateTime.MinValue)
            {
                var nextPayment = sectionContent.FindElement(By.Id("next-payment-date"));
                nextPayment.Text.Should().Contain($"Next allocation payment: {nextPaymentDate.ToString(ExpectedNextPaymentDateFormat)}.");
            }
            else
            {
                var noNextPaymentDateTextSelector = section.FindElements(By.CssSelector("h3.heading-medium.heading-guidance + p"));
                noNextPaymentDateTextSelector[0].Text.Should().Be(noNextPaymentDateText);
            }
        }


        #region Page Elements

        /// <summary>
        /// Gets the accordion sections.
        /// </summary>
        /// <value>
        /// The accordion sections.
        /// </value>
        protected static IReadOnlyCollection<IWebElement> AccordionSections
            => Driver.Instance.WaitToFindElements(By.ClassName("govuk-accordion__section"));

        /// <summary>
        /// Gets the alternative funding.
        /// </summary>
        /// <value>
        /// The alternative funding.
        /// </value>
        protected static IWebElement AlternativeFunding
            => Driver.Instance.WaitToFindElement(By.Name("alternativeFunding"));

        /// <summary>
        /// Gets the schools block link.
        /// </summary>
        /// <value>
        /// The schools block link.
        /// </value>
        protected static IWebElement SchoolsBlockLink
           => Driver.Instance.WaitToFindElement(By.LinkText("Schools block"));

        #endregion
    }
}