using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.LoggedIn
{
    public class IndicativeProviderFundingBreakdownPage : ViewYourFundingBasePage
    {
        #region Private fields

        private static readonly IReadOnlyList<string> H3Headers = new List<string>
        {
            "Allocation history",
            "General annual grant resources"
        };

        #endregion


        #region Actions

        public static void NavigateToPageViaLogin(string username, string password)
        {
            ProviderPage.NavigateToPageViaLogin(username, password);
            ProviderPage.ExpandGAG();
            ProviderPage.ClickOnSchoolShareBudgetTabLink();
        }

        public static void NavigateTo1619PageViaLogin(string username, string password)
        {
            ProviderPage.NavigateToPageViaLogin(username, password);
            ProviderPage.Expand1619();
            ProviderPage.ClickOnViewFundingBreakdownLink();
        }

        public static void NavigateToPageWithoutLogin()
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();

            Goto("view-latest-funding/pre-16-16-19-statements/12345678/GAG/01-6-2021/2021-to-2022#minimum-funding-guarantee");
        }

        public static void ClickOnSchoolShareBudgetTab()
        {
            SchoolBudgetShareTabLink.MoveAndClick();
        }

        public static void ClickOnMinimumFundingGuaranteeTab()
        {
            MinimumFundingGuaranteeTabLink.MoveAndClick();
        }

        public static void ClickOnHighNeedsTab()
        {
            HighNeedsTabLink.MoveAndClick();
        }

        public static void ClickOnStartUpGrantTab()
        {
            StartUpGrantTabLink.MoveAndClick();
        }

        #endregion


        #region Assertions

        /// <summary>
        /// Ensures the breadcrumbs and final text.
        /// </summary>
        /// <param name="finalText">The final text.</param>
        /// <param name="breadcrumbLinks">The breadcrumb links.</param>
        public static void EnsureBreadcrumbsAndFinalText(string finalText, string[] breadcrumbLinks)
        {
            EnsureFinalBreadcrumbText(finalText);
            EnsureBreadcrumbLinksText(breadcrumbLinks);
        }

        public static void EnsureNavigatedToLogin()
        {
            EnsureCurrentPage("Login");
        }

        /// <summary>
        /// Ensures the current provider page.
        /// </summary>
        public static void EnsureCurrentProviderPage()
        {
            EnsureCurrentPage("Indicative general annual grant: 2023 to 2024\r\nThis allocation: 6 July 2021 LATEST");
        }

        public static void EnsureCurrentProviderPageFor1619()
        {
            EnsureCurrentPage("Indicative 16 to 19 funding: academic year 2021 to 2022\r\nThis allocation: 1 July 2021 LATEST");
        }

        public static void EnsureBreakdownTabs()
        {
            SchoolBudgetShareTabLink.Displayed.Should().BeTrue();
            MinimumFundingGuaranteeTabLink.Displayed.Should().BeTrue();
            HighNeedsTabLink.Displayed.Should().BeTrue();
        }

        public static void EnsurePrintOrSaveStatement()
        {
            PrintOrSaveContainer.Displayed.Should().BeTrue();
        }

        public static void EnsureDocumentDownload()
        {
            DownloadADocument.Displayed.Should().BeTrue();
            DownloadADocument.Text.Should().Contain("Download the raw data for this statement");
        }

        public static void EnsureGuidanceLinkSection()
        {
            GagGuidanceLink.Displayed.Should().BeTrue();
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
            totalText.Text.Should().Be($"Total allocation (based on being open 365 days)\r\n{allocationAmount}");
            var totalValue = TotalAllocationSection.FindElement(By.TagName("p"));
            totalValue.Text.Should().NotBeEmpty();
        }

        public static void EnsureTotalAllocation16to19(
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

        public static void EnsureMandatoryH3Headers()
        {
            foreach (var header in H3Headers)
            {
                H3Elements.Any(item =>
                    item.Text.Contains(header, StringComparison.InvariantCultureIgnoreCase)).Should().BeTrue();
            }
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

        public static void EnsureTabStatementCalculationTextAndResponse(
            IWebElement tab,
            string calculationText,
            string calculationResponse)
        {
            var tabAllocationSection =
                tab.FindElements(By.ClassName("total-allocation-wrapper")).First(section =>
                    section.Text.Contains(calculationText, StringComparison.InvariantCultureIgnoreCase));

            var totalAllocationAmountText =
                tabAllocationSection.FindElement(By.ClassName("total-allocation-label"));
            totalAllocationAmountText.Displayed.Should().BeTrue();
            totalAllocationAmountText.Text.Contains(calculationText, StringComparison.InvariantCultureIgnoreCase).Should()
                .BeTrue($"Expected tab calculation text to contain '{calculationText}' and found '{totalAllocationAmountText.Text}'");

            var valueElement =
                tabAllocationSection.FindElement(By.ClassName("total-allocation-amount"));
            valueElement.Displayed.Should().BeTrue();
            valueElement.Text.Contains(calculationResponse).Should()
                .BeTrue($"Expected tab calculation response to contain '{calculationResponse}' and found '{valueElement.Text}'");
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

        public static void EnsureMidTableLineAllocationTotal(
            IWebElement tab,
            string headerText,
            string allocationAmount)
        {
            var tabAllocationSection =
                tab.FindElements(By.CssSelector(".total-allocation-wrapper"))
                    .First(fundingLine =>
                        fundingLine.Text.Contains(headerText, StringComparison.InvariantCultureIgnoreCase));

            var totalAllocationAmountText =
                tabAllocationSection.FindElement(By.CssSelector(".total-allocation-label h2.heading-small"));
            totalAllocationAmountText.Displayed.Should().BeTrue();
            totalAllocationAmountText.Text.Contains(headerText, StringComparison.InvariantCultureIgnoreCase).Should()
                .BeTrue($"Expected tab bottom total text to contain '{headerText}' and found '{totalAllocationAmountText.Text}'");

            var valueElement =
                tabAllocationSection.FindElement(By.CssSelector(".total-allocation-amount h2.heading-small"));
            valueElement.Displayed.Should().BeTrue();
            valueElement.Text.Contains(allocationAmount).Should()
                .BeTrue($"Expected tab bottom total amount to contain '{allocationAmount}' and found '{valueElement.Text}'");
        }

        public static void EnsureTabLink(IWebElement tab, string linkText)
        {
            var link = tab.FindElement(By.LinkText(linkText));
            link.Displayed.Should()
                .BeTrue($"Expected link with text '{linkText}' to be displayed");
        }

        public static void EnsureSmallHeading(IWebElement tab, string headerText)
        {
            var header = tab.FindElements(By.TagName("h3")).First(h3 => h3.Text.Equals(headerText, StringComparison.InvariantCultureIgnoreCase));
            header.Displayed.Should()
                .BeTrue($"Expected Header 3 with text '{headerText}' to be displayed");
        }

        public static void EnsureParagraphContent(IWebElement tab, string content)
        {
            var header = tab.FindElements(By.TagName("p")).First(p => p.Text.Equals(content, StringComparison.InvariantCultureIgnoreCase));
            header.Displayed.Should()
                .BeTrue($"Expected paragraph with content '{content}' to be displayed");
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

        public static void Ensure1619AccordionSections()
        {
            AccordionContainer.Displayed.Should().BeTrue();
            AccordionSections.Should().HaveCount(10);
        }

        #endregion


        #region Page Elements

        public static IWebElement AccordionContainer => Driver.Instance.FindElement(By.Id("accordion-1619-statement"));

        public static IWebElement Indicative1619Content => Driver.Instance.FindElement(By.Id("indicative1619"));

        public static ReadOnlyCollection<IWebElement> AccordionSections => Driver.Instance.FindElements(By.ClassName("govuk-accordion__section"));

        public static IWebElement SchoolBudgetShareTabLink => TabsSection.FindElement(By.LinkText("School budget share"));

        public static IWebElement MinimumFundingGuaranteeTabLink => TabsSection.FindElement(By.LinkText("Minimum funding guarantee"));

        public static IWebElement StartUpGrantTabLink => TabsSection.FindElement(By.LinkText("Start-up grant"));

        public static IWebElement HighNeedsTabLink => TabsSection.FindElement(By.LinkText("High needs"));

        public static IWebElement SchoolBudgetShareTabContent => TabsSection.FindElement(By.Id("school-budget-share"));

        public static IWebElement MinimumFundingGuaranteeTabContent => TabsSection.FindElement(By.Id("minimum-funding-guarantee"));

        public static IWebElement StartUpGrantTabContent => TabsSection.FindElement(By.Id("start-up-grant"));

        public static IWebElement HighNeedsTabContent => TabsSection.FindElement(By.Id("high-needs"));

        public static IWebElement PostOpeningGrantTabContent => TabsSection.FindElement(By.Id("post-opening-grant"));

        public static IWebElement MFGTabLocalAuthorityApplyCappingAndScaling => TabsSection.FindElement(By.Id("LAApplyCappingAndScaling"));

        public static IWebElement MFGTabSchoolHasYearGroupsWithoutPupils => TabsSection.FindElement(By.Id("schoolHasYearGroupsWithoutPupils"));

        public static IWebElement EligibleForSparsity
            => Driver.Instance.FindElement(By.Id("eligible-for-sparsity"));

        protected static IWebElement DownloadADocument => Driver.Instance.FindElement(By.ClassName("download-a-document-container-new"));

        protected static IWebElement PrintOrSaveContainer
            => Driver.Instance.WaitToFindElement(By.ClassName("print-a-document-container"));

        protected static IWebElement GagGuidanceLink
            => Driver.Instance.WaitToFindElement(By.PartialLinkText("General annual grant allocation guide:"));

        protected static IWebElement GuidanceLink1619
            => Driver.Instance.WaitToFindElement(By.PartialLinkText("16 to 19 funding allocation guide:"));

        protected static IWebElement AllocationHistoryLink
            => Driver.Instance.WaitToFindElement(By.LinkText("View history of allocated funding"));

        protected static IEnumerable<IWebElement> H3Elements
            => Driver.Instance.FindElements(By.TagName("H3"));

        protected static IWebElement TotalAllocationSection
            => Driver.Instance.FindElement(By.ClassName("pfb-total-allocation"));

        protected static IWebElement TabsSection
            => Driver.Instance.FindElement(By.ClassName("govuk-tabs"));

        #endregion
    }
}