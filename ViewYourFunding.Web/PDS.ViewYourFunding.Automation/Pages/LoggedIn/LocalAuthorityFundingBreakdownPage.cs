using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.LoggedIn
{
    public class LocalAuthorityFundingBreakdownPage : ViewYourFundingBasePage
    {
        #region Private fields

        private static readonly IReadOnlyList<string> H3Headers = new List<string>
        {
            "Allocation history",
            "General annual grant resources"
        };

        #endregion


        #region Actions

        public static void NavigateTo1619PageViaLogin(string username, string password)
        {
            ProviderPage.NavigateToPageViaLogin(username, password);
            ProviderPage.ExpandLaSsf();
            ProviderPage.ClickOnLAFundingBreakdownLink();
        }

        public static void ClickOnSplitYearsTab()
        {
            SplitYearsTabLink.MoveAndClick();
        }

        #endregion


        #region Assertions

        /// <summary>
        /// Ensures the current provider page.
        /// </summary>
        public static void EnsureCurrentBreakDownPage()
        {
            EnsureCurrentPage("School sixth form funding: 2021 to 2022\r\nThis allocation: 10 January 2021 LATEST");
        }

        public static void EnsureBreakdownTabs()
        {
            SplitYearsTabLink.Should().NotBeNull();
        }

        public static void EnsurePrintOrSaveStatement()
        {
            PrintOrSaveContainer.Displayed.Should().BeTrue();
        }

        public static void EnsureDocumentDownload()
        {
            DownloadADocument.Displayed.Should().BeTrue();
        }

        public static void Ensure1619GuidanceLinkSection()
        {
            GuidanceLink1619.Displayed.Should().BeTrue();
        }

        public static void EnsureTotalAllocation(
            string allocationAmount)
        {
            TotalAllocationSection.Displayed.Should().BeTrue();
            var totalText = TotalAllocationSection.FindElement(By.TagName("h2"));
            totalText.Text.Should().Be($"Total allocation\r\n{allocationAmount}");
            var totalValue = TotalAllocationSection.FindElement(By.TagName("p"));
            totalValue.Text.Should().NotBeEmpty();
        }

        public static void EnsureAllocationHistory()
        {
            AllocationHistoryLink.Displayed.Should().BeTrue();
        }

        public static void EnsureTabTotalAllocation(
            IWebElement tab,
            string headerText,
            string allocationAmount)
        {
            var tabAllocationSection = tab.FindElements(By.CssSelector(".grid-row .block-header-total")).First(tabSection => tabSection.Text.Contains(headerText));

            var headerElement = tabAllocationSection.FindElement(By.TagName("h2"));
            headerElement.Displayed.Should()
                .BeTrue();
            headerElement.Text.Contains(headerText, StringComparison.InvariantCultureIgnoreCase).Should()
                .BeTrue($"Expected tab total text to contain '{headerText}' and found '{headerElement.Text}'");

            var valueElement = tabAllocationSection.FindElement(By.TagName("p"));
            valueElement.Displayed.Should().BeTrue();
            valueElement.Text.Contains(allocationAmount).Should()
                .BeTrue($"Expected tab total text to contain '{allocationAmount}' and found '{valueElement.Text}'");
        }

        public static void EnsureTabBottomAllocationTotal(
            IWebElement tab,
            string headerText,
            string allocationAmount)
        {
            var tabAllocationSection =
                tab.FindElements(By.ClassName("bottomTab-total")).First(section =>
                    section.Text.Contains(headerText, StringComparison.InvariantCultureIgnoreCase));

            var totalAllocationAmountText =
                tabAllocationSection.FindElement(By.CssSelector(".total-allocation-label h2"));
            totalAllocationAmountText.Displayed.Should().BeTrue();
            totalAllocationAmountText.Text.Contains(headerText, StringComparison.InvariantCultureIgnoreCase).Should()
                .BeTrue($"Expected tab bottom total text to contain '{headerText}' and found '{totalAllocationAmountText.Text}'");

            var valueElement =
                tabAllocationSection.FindElement(By.CssSelector(".total-allocation-amount h2"));
            valueElement.Displayed.Should().BeTrue();
            valueElement.Text.Contains(allocationAmount).Should()
                .BeTrue($"Expected tab bottom total amount to contain '{allocationAmount}' and found '{valueElement.Text}'");
        }

        public static void EnsureSimpleDisclosure(IWebElement tab, string linkText, string content)
        {
            var disclosureLink = tab.FindElements(By.ClassName("govuk-details__summary")).First(span =>
                span.Text.Equals(linkText, StringComparison.InvariantCultureIgnoreCase));
            disclosureLink.Displayed.Should().BeTrue($"Expected disclosure link with text '{linkText}' to be displayed");
            disclosureLink.Click();

            var disclosureParent = disclosureLink.FindElement(By.XPath(".."));

            var disclosureDetail = disclosureParent.FindElement(By.ClassName("govuk-details__text"));

            disclosureDetail.Displayed.Should().BeTrue();
            disclosureDetail.Text.Contains(content, StringComparison.OrdinalIgnoreCase).Should()
                .BeTrue($"Expected a simple disclosure to have content '{content}' and found '{disclosureDetail.Text}'");
        }

        public static void EnsureTabTableContentData(IWebElement tab, TableData tableData)
        {
            var table = tab.FindElement(By.Id(tableData.Id));
            var rowIndex = 0;

            foreach (var tableRowData in tableData.TableRowData)
            {
                if (tableRowData.HeaderRow)
                {
                    var row = table.FindElement(By.TagName("thead")).FindElement(By.TagName("tr"));
                    var ths = row.FindElements(By.TagName("th")).Select(x => x.Text).ToList();

                    foreach (var item in tableRowData.RowItems)
                    {
                        row.FindElements(By.TagName("th"))
                            .Any(th => th.Text.Trim().Equals(item, StringComparison.InvariantCultureIgnoreCase)).Should()
                            .BeTrue($"Expected a table header column header to have '{item}' in '{row.Text}'");
                    }
                }
                else
                {
                    var row = table.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"))[rowIndex];
                    var columnIndex = 0;
                    foreach (var item in tableRowData.RowItems)
                    {
                        if (columnIndex == 0)
                        {
                            row.FindElements(By.TagName("th"))
                                .Any(th => th.Text.Trim().Equals(item, StringComparison.InvariantCultureIgnoreCase)).Should()
                                .BeTrue($"Expected a table row header column to have '{item}' in '{row.Text}'");
                        }
                        else
                        {
                            row.FindElements(By.TagName("td"))
                                .Any(td => td.Text.Equals(item, StringComparison.InvariantCultureIgnoreCase)).Should()
                                .BeTrue($"Expected a table row column to have '{item}' in '{row.Text}'");
                        }

                        columnIndex++;
                    }

                    rowIndex++;
                }
            }

            var rowCount = table.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr")).Count;
            var dataRowCount = tableData.TableRowData.Count(row => !row.HeaderRow);

            rowCount.Should().Be(
                dataRowCount,
                null,
                $"Expected row count to be {rowCount} for table with id '{tableData.Id}', found '{dataRowCount}' rows.");
        }

        public static void EnsureRoundingText()
        {
            var expectedText = "The values on your statement are shown rounded to various numbers of decimal places. The calculation of your funding however is done using un-rounded values. This may result in some slight differences when you work through the calculation yourselves.";
            var roundingText = Driver.Instance.FindElement(By.ClassName("rounding-text")).Text;
            roundingText.Should().Be(expectedText);
        }

        #endregion


        #region Page Elements

        public static IWebElement AcademicYearTabContent => TabsSection.FindElement(By.Id("academic-year"));

        public static IWebElement SplitYearsTabContent => TabsSection.FindElement(By.Id("split-years"));

        protected static IWebElement DownloadADocument => Driver.Instance.FindElement(By.ClassName("download-a-document-new"));

        protected static IWebElement PrintOrSaveContainer
            => Driver.Instance.WaitToFindElement(By.ClassName("print-a-document-container"));

        protected static IWebElement GuidanceLink1619
            => Driver.Instance.WaitToFindElement(By.PartialLinkText("School sixth form funding guides:"));

        protected static IWebElement AllocationHistoryLink
            => Driver.Instance.WaitToFindElement(By.LinkText("View history of allocated funding"));

        protected static IEnumerable<IWebElement> H3Elements
            => Driver.Instance.FindElements(By.TagName("H3"));

        protected static IWebElement TotalAllocationSection
            => Driver.Instance.FindElement(By.ClassName("pfb-total-allocation"));

        protected static IWebElement TabsSection
            => Driver.Instance.FindElement(By.ClassName("govuk-tabs"));

        protected static IWebElement SplitYearsTabLink => TabsSection.FindElement(By.LinkText("Split across financial years"));

        #endregion
    }
}