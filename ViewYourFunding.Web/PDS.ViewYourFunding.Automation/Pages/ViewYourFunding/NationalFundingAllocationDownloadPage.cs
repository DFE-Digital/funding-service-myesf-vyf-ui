using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The NationalFundingAllocationDownloadPage class.
    /// </summary>
    public class NationalFundingAllocationDownloadPage : ViewYourFundingBasePage
    {
        #region DSG Actions

        /// <summary>
        /// Navigates to page.
        /// </summary>
        public static void DSGNavigateToPage()
        {
            StartPage.Open();
            StartPage.ClickStartButton();

            ViewingChoicePage.SelectNationalOption();
            ViewingChoicePage.ClickContinueButton();

            WhichAllocationPage.SelectDSG();
            WhichAllocationPage.ClickContinueButton();
        }

        #endregion


        #region PSG Actions

        /// <summary>
        /// Navigates to page.
        /// </summary>
        public static void PSGNavigateToPage()
        {
            StartPage.Open();
            StartPage.ClickStartButton();

            ViewingChoicePage.SelectNationalOption();
            ViewingChoicePage.ClickContinueButton();

            WhichAllocationPage.SelectPSG();
            WhichAllocationPage.ClickContinueButton();
        }

        /// <summary>
        /// Clicks the link to latest or historic allocations.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void PSGClickLinkToLatestOrHistoricAllocations(int yearFrom, int yearTo)
        {
            var linkText = $"PE and sport premium: funding allocations for {yearFrom} to {yearTo}";
            var link = RelatedLinksSection.FindElement(By.LinkText(linkText));
            link.MoveAndClick();
        }

        #endregion


        #region DSG Assertions

        /// <summary>
        /// Ensures the current page.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void DSGEnsureCurrentPage(int yearFrom, int yearTo)
        {
            EnsureCurrentPage($"Dedicated schools grant (DSG) {yearFrom} to {yearTo}");
        }

        /// <summary>
        /// Ensures the current allocations.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void DSGEnsureCurrentAllocations(int yearFrom, int yearTo)
        {
            var expectedLinkText = $"DSG: {yearFrom} to {yearTo}";
            var expectedUrl = $"DSG/{yearFrom}-to-{yearTo}";

            EnsureRelatedLink(expectedLinkText, expectedUrl, "Historic funding allocations");
        }

        /// <summary>
        /// Ensures the historic allocations.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void DSGEnsureHistoricAllocations(int yearFrom, int yearTo)
        {
            var expectedLinkText = $"DSG: {yearFrom} to {yearTo}";
            var expectedUrl = yearFrom <= 2020
                ? $"www.gov.uk/government/publications/dedicated-schools-grant-dsg-{yearFrom}-to-{yearTo}"
                : $"dedicated-schools-grant/download-funding/{yearFrom}-to-{yearTo}";

            EnsureRelatedLink(expectedLinkText, expectedUrl, "Historic funding allocations");
        }

        /// <summary>
        /// Ensures the guidance link.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void DSGEnsureGuidanceLink(int yearFrom, int yearTo)
        {
            var listItem = DSGGuidanceLinkListItem;
            listItem.Displayed.Should().BeTrue();
            listItem.Text.Should().Be($"Get more general guidance on DSG: {yearFrom} to {yearTo}.");

            var link = listItem.FindElement(By.TagName("a"));
            link.GetAttribute("href").Should()
                .Contain($"www.gov.uk/government/publications/dedicated-schools-grant-dsg-{yearFrom}-to-{yearTo}");
        }

        /// <summary>
        /// Ensures the funding block resource links.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void DSGEnsureFundingBlockResourceLinks(int yearFrom, int yearTo)
        {
            DSGFundingBlockResourceLinks.Displayed.Should().BeTrue();

            DSGEnsureFundingBlockResourceLink(
                "Early years national funding formula: funding rates and guidance",
                $"www.gov.uk/government/publications/early-years-funding-{yearFrom}-{yearTo}");

            DSGEnsureFundingBlockResourceLink(
                $"High needs funding arrangements: {yearFrom} to {yearTo}",
                $"www.gov.uk/government/publications/high-needs-funding-arrangements-{yearFrom}-to-{yearTo}");

            DSGEnsureFundingBlockResourceLink(
                $"National funding formula tables for schools and high needs: {yearFrom} to {yearTo}",
                $"www.gov.uk/government/publications/national-funding-formula-tables-for-schools-and-high-needs-{yearFrom}-to-{yearTo}");

            DSGEnsureFundingBlockResourceLink(
                $"Pre-16 schools funding: guidance for {yearFrom} to {yearTo}",
                $"www.gov.uk/government/publications/pre-16-schools-funding-local-authority-guidance-for-{yearFrom}-to-{yearTo}");
        }

        /// <summary>
        /// Ensures the funding block resource link.
        /// </summary>
        /// <param name="expectedLinkText">The expected link text.</param>
        /// <param name="expectedUrl">The expected URL.</param>
        public static void DSGEnsureFundingBlockResourceLink(string expectedLinkText, string expectedUrl)
        {
            var link = DSGFundingBlockResourceLinks.FindElement(By.LinkText(expectedLinkText));
            link.Displayed.Should().BeTrue();

            var target = link.GetAttribute("href");
            target.Should().Contain(expectedUrl);
        }

        /// <summary>
        /// Ensures the spreadsheet links.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void DSGEnsureSpreadsheetLinks(int yearFrom, int yearTo)
        {
            DSGDocumentDownloadContainers.Should().HaveCountGreaterThan(0);

            var linkText = $"Download DSG allocations for all local authorities and regions {yearFrom} to {yearTo}";
            var isFirst = true;

            foreach (var container in DSGDocumentDownloadContainers)
            {
                var documentLink = container.FindElement(By.LinkText(linkText));
                documentLink.Displayed.Should().BeTrue();

                var tag = container.FindElementOrDefault(By.ClassName("govuk-tag"));

                if (isFirst)
                {
                    tag.Should().NotBeNull("the first document should have a tag");
                    tag.Text.Should().BeEquivalentTo("Latest");
                }
                else
                {
                    tag.Should().BeNull("there shouldn't be a tag on documents from the second onwards");
                }

                isFirst = false;
            }
        }

        /// <summary>
        /// Ensures the format request links.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void DSGEnsureFormatRequestLinks(int yearFrom, int yearTo)
        {
            FormatRequestLinks.Should().HaveCountGreaterThan(0);

            var requestSubject =
                $"Request for 'Dedicated schools grant allocations for all local authorities and regions {yearFrom} to {yearTo}' in an alternative format";

            foreach (var link in FormatRequestLinks)
            {
                var displayedTarget = link.GetAttribute("href");
                var requestFormatId = displayedTarget.Substring(displayedTarget.IndexOf("#", StringComparison.InvariantCultureIgnoreCase));

                var displayedTargetRegex = new Regex(@"^#attachment-[0-9]-accessibility-request$");
                displayedTargetRegex.IsMatch(requestFormatId).Should().BeTrue();
                link.MoveAndClick();

                var requestFormatLink = Driver.Instance.WaitToFindElement(By.CssSelector(requestFormatId));
                var targetSection = requestFormatLink.FindElement(By.TagName("a"));
                var target = targetSection.GetAttribute("href");
                target.Should().Contain($"mailto:{Uri.EscapeDataString("ESFA.GOVUK-ENQUIRIES@education.gov.uk")}?");
                target.Should().Contain("subject=" + Uri.EscapeDataString(requestSubject));
                target.Should().Contain(Uri.EscapeDataString("Original format: ods"));
            }
        }

        #endregion


        #region PSG Assertions

        /// <summary>
        /// Ensures the current page.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="isLatest">if set to <c>true</c> [is latest].</param>
        public static void PSGEnsureCurrentPage(int yearFrom, int yearTo, bool isLatest)
        {
            EnsureCurrentPage($"PE and sport premium {yearFrom} to {yearTo}", allowContains: !isLatest);

            if (!isLatest)
            {
                EnsureCurrentPage("Not latest", allowContains: true);
            }
        }

        /// <summary>
        /// Ensures the link to latest or historic allocations.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="isLatest">if set to <c>true</c> [is latest].</param>
        public static void PSGEnsureLinkToLatestOrHistoricAllocations(int yearFrom, int yearTo, bool isLatest)
        {
            var expectedLinkText = $"PE and sport premium: funding allocations for {yearFrom} to {yearTo}";

            string expectedUrl;

            if (yearFrom == 2016)
            {
                expectedUrl = "www.gov.uk/government/publications/pe-and-sport-premium-funding-conditions-for-2016-to-2017";
            }
            else if (yearFrom == 2017)
            {
                expectedUrl = "www.gov.uk/government/publications/pe-and-sport-premium-funding-allocations-for-2017-to-2018";
            }
            else
            {
                expectedUrl = $"national-funding-allocations/PSG/{yearFrom}-to-{yearTo}";
            }

            EnsureRelatedLink(expectedLinkText, expectedUrl, isLatest ? "Latest funding allocations" : "Historic funding allocations");
        }

        /// <summary>
        /// Ensures the spreadsheet links.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="isLatest">if set to <c>true</c> [is latest].</param>
        public static void PSGEnsureSpreadsheetLinks(int yearFrom, int yearTo, bool isLatest)
        {
            PSGDocumentDownloadContainers.Should().HaveCountGreaterThan(0);

            var linkText = $"Download PE and sport premium allocations {yearFrom} to {yearTo}";
            var isFirst = true;

            foreach (var container in PSGDocumentDownloadContainers)
            {
                var documentLink = container.FindElement(By.LinkText(linkText));
                documentLink.Displayed.Should().BeTrue();

                var tag = container.FindElementOrDefault(By.ClassName("govuk-tag"));

                if (isFirst)
                {
                    tag.Should().NotBeNull("the first document should have a tag");

                    if (isLatest)
                    {
                        tag.Text.Should().BeEquivalentTo("Latest");
                    }
                    else
                    {
                        tag.Text.Should().BeEquivalentTo("Final");
                    }
                }
                else
                {
                    tag.Should().BeNull("there shouldn't be a tag on documents from the second onwards");
                }

                isFirst = false;
            }
        }

        /// <summary>
        /// Ensures the format request links.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void PSGEnsureFormatRequestLinks(int yearFrom, int yearTo)
        {
            PSGFormatRequestLinks.Should().HaveCountGreaterThan(0);

            var requestSubject =
                    $"Request for 'PE and sport premium allocations {yearFrom} to {yearTo}' in an alternative format";

            foreach (var link in FormatRequestLinks)
            {
                var displayedTarget = link.GetAttribute("href");
                var requestFormatId = displayedTarget.Substring(displayedTarget.IndexOf("#", StringComparison.InvariantCultureIgnoreCase));

                var displayedTargetRegex = new Regex(@"^#attachment-[0-9]-accessibility-request$");
                displayedTargetRegex.IsMatch(requestFormatId).Should().BeTrue();
                link.MoveAndClick();

                var requestFormatLink = Driver.Instance.WaitToFindElement(By.CssSelector(requestFormatId));
                var targetSection = requestFormatLink.FindElement(By.TagName("a"));
                var target = targetSection.GetAttribute("href");
                target.Should().Contain($"mailto:{Uri.EscapeDataString("ESFA.GOVUK-ENQUIRIES@education.gov.uk")}?");
                target.Should().Contain("subject=" + Uri.EscapeDataString(requestSubject));
                target.Should().Contain(Uri.EscapeDataString("Original format: ods"));
            }
        }

        #endregion


        #region DSG Page Elements

        /// <summary>
        /// Gets the guidance link list item.
        /// </summary>
        /// <value>
        /// The guidance link list item.
        /// </value>
        protected static IWebElement DSGGuidanceLinkListItem
            => Driver.Instance.WaitToFindElement(By.Id("guidance-link"));

        /// <summary>
        /// Gets the funding block resource links.
        /// </summary>
        /// <value>
        /// The funding block resource links.
        /// </value>
        protected static IWebElement DSGFundingBlockResourceLinks
            => Driver.Instance.WaitToFindElement(By.Id("funding-block-resources"));

        /// <summary>
        /// Gets the document download containers.
        /// </summary>
        /// <value>
        /// The document download containers.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> DSGDocumentDownloadContainers
            => Driver.Instance.WaitToFindElements(By.ClassName("download-a-document-container"));

        /// <summary>
        /// Gets the format request links.
        /// </summary>
        /// <value>
        /// The format request links.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> FormatRequestLinks
            => Driver.Instance.WaitToFindElements(By.LinkText("Request an accessible format"));

        #endregion


        #region PSG Page Elements

        /// <summary>
        /// Gets the document download containers.
        /// </summary>
        /// <value>
        /// The document download containers.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> PSGDocumentDownloadContainers
            => Driver.Instance.WaitToFindElements(By.ClassName("download-a-document-container"));

        /// <summary>
        /// Gets the format request links.
        /// </summary>
        /// <value>
        /// The format request links.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> PSGFormatRequestLinks
            => Driver.Instance.WaitToFindElements(By.LinkText("Request an accessible format"));

        #endregion
    }
}
