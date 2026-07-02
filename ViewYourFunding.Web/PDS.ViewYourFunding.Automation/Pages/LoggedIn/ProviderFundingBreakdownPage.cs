using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.LoggedIn
{
    public class ProviderFundingBreakdownPage : ViewYourFundingBasePage
    {
        #region Private fields

        private static readonly IReadOnlyList<string> H3Headers = new List<string>
        {
            "Allocation history",
            "Explore the topic"
        };

        #endregion


        #region Actions
        public static void NavigateToNMSSPageViaLogin(string username, string password, string selectedBreakDownLink = "high-needs")
        {
            ProviderPage.NavigateToPageViaLogin(username, password);
            ProviderPage.ExpandNMSS();
            ProviderPage.EnsureNMSSFundingBreakdownLinks();
            if (selectedBreakDownLink == "high-needs")
            {
                ProviderPage.ClickOnNMSSHighNeedsLink();
            }
            else
            {
                ProviderPage.ClickOnNMSSDiscretionaryBursaryFundLink();
            }
        }

        public static void NavigateToPageViaLogin(string username, string password)
        {
            ProviderPage.NavigateToPageViaLogin(username, password);
            ProviderPage.ExpandGAG();
            ProviderPage.ClickOnSchoolShareBudgetTabLink();
        }

        public static void NavigateToPageViaLoginAndHighNeeds(string username, string password)
        {
            ProviderPage.NavigateToPageViaLogin(username, password);
            ProviderPage.ExpandGAG();
            ProviderPage.ClickOnHighNeedsTabLink();
        }

        public static void NavigateTo1619PageViaLogin(string username, string password)
        {
            ProviderPage.NavigateToPageViaLogin(username, password);
            ProviderPage.Expand1619();
            ProviderPage.ClickOnViewFundingBreakdownLink();
        }

        public static void NavigateTo1619HistoryPageViaLogin(string username, string password)
        {
            ProviderPage.NavigateToPageViaLogin(username, password);
            ProviderPage.Expand1619();
            ProviderPage.ClickOnViewHistoryLink();
        }

        public static void NavigateTo1416PageViaLogin(string username, string password)
        {
            ProviderPage.NavigateToPageViaLogin(username, password);
            ProviderPage.Expand1416();
            ProviderPage.ClickOnViewFourteen16FundingBreakdownLink();
        }

        public static void NavigateTo1416PageWithoutLogin()
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();

            Goto("view-latest-funding/pre-16-16-19-statements/10072811/14-to-16-funding/12-9-2021/2021-to-2022?tab=core-programme");
        }

        public static void NavigateToPageDirectly()
        {
            ProviderPage.NavigateToPageDirectly();
            ProviderPage.ExpandGAG();
            ProviderPage.ClickOnSchoolShareBudgetTabLink();
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

        public static void ClickOnProtectionFundingTab()
        {
            ProtectionFundingTabLink.MoveAndClick();
        }

        public static void ClickOnPostOpeningGrantTab()
        {
            PostOpeningGrantTabLink.MoveAndClick();
        }

        public static void ClickOnMathsAndEnglishCofLink()
        {
            MathsAndEnglishCoFAccordionSection.Click();
        }

        public static void ClickOnSportingDiplomaLink()
        {
            SportingExcellenceAccordionSection.Click();
        }

        public static void ClickOnSeaFishingDiplomaLink()
        {
            SeaFishingAccordionSection.Click();
        }

        public static void ClickOnTuitionFundLink()
        {
            TuitionFundAccordionSection.Click();
        }

        public static void ClickOnAdvancedMathsPremiumLink()
        {
            AdvancedMathsPremiumAccordionSection.Click();
        }

        public static void ClickOnHighNeedsLink()
        {
            HighNeedsAccordionSection.Click();
        }

        public static void ClickOnTeachersPensionLink()
        {
            TeachersPensionAccordionSection.Click();
        }

        public static void ClickOnDiplomaLink()
        {
            DiplomaAccordionSection.Click();
        }

        public static void ClickOnIndustryPlacement()
        {
            OpenAccordion(IndustryPlacementAccordionSection);
        }

        public static void ClickOn1619DiscretionaryBursaryFund()
        {
            SixteenTo19DiscretionaryBursaryFundAccordionSection.Click();
        }

        public static void OpenCoreProgrammeAccordion()
        {
            if (CoreProgrammeAccordionIsOpen())
            {
                return;
            }

            CoreProgrammeAccordionSection.Click();
            var runs = 0;

            while (!CoreProgrammeAccordionIsOpen() && runs++ <= 10)
            {
                Thread.Sleep(100);
            }
        }

        public static bool CoreProgrammeAccordionIsOpen()
        {
            return CoreProgrammeAccordionSection.GetAttribute("class").Contains("govuk-accordion__section--expanded");
        }

        public static void OpenHighValueCoursesPremiumAccordion()
        {
            HighValueCoursesPremiumAccordionSection.Click();
        }

        public static void OpenAccordion(IWebElement accordionSection)
        {
            if (!accordionSection.GetAttribute("class").Contains("govuk-accordion__section--expanded"))
            {
                accordionSection.Click();
            }
        }

        public static void OpenStudentFinancialSupportAccordion()
        {
            StudentFinancialSupportAccordionSection.Click();
        }

        public static void OpenResidentialAccommodationAccordion()
        {
            ResidentialAccommodationAccordionSection.Click();
        }

        public static void ClickOnCoreProgrammeTab()
        {
            CoreProgrammeTabLink.MoveAndClick();
        }

        public static void ClickOnPupilPremiumTab()
        {
            PupilPremium1416TabLink.MoveAndClick();
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
            EnsureCurrentPage("General annual grant: 2023 to 2024\r\nThis allocation: 1 June 2023 LATEST");
        }

        public static void EnsureBreakdownTabs()
        {
            SchoolBudgetShareTabLink.Displayed.Should().BeTrue();
            MinimumFundingGuaranteeTabLink.Displayed.Should().BeTrue();
            HighNeedsTabLink.Displayed.Should().BeTrue();
        }

        public static void EnsureStartUpGrantTabNotDisplayed()
        {
            var link = TabsSection.FindElementOrDefault(By.LinkText("Start-up grant"));
            link.Should().BeNull();
        }

        public static void EnsurePrintOrSaveStatement()
        {
            PrintOrSaveContainer.Displayed.Should().BeTrue();
        }

        public static void EnsureDocumentDownload()
        {
            DownloadADocument.Displayed.Should().BeTrue();
            DownloadADocument.Text.Should().Contain("Download the raw data for this statement (CSV, 201KB)");
        }

        public static void Ensure1619DocumentDownload()
        {
            DownloadADocument.Displayed.Should().BeTrue();
            DownloadADocument.Text.Should().Contain("Download the raw data for this statement (csv, 206KB)");
        }

        public static void EnsureGuidanceLinkSection()
        {
            GagGuidanceLink.Displayed.Should().BeTrue();
        }

        public static void EnsureNMSSGuidanceLinkSection()
        {
            NMSSGuidanceLink.Displayed.Should().BeTrue();
        }

        public static void Ensure1619GuidanceLinkSection()
        {
            GuidanceLink1619.Displayed.Should().BeTrue();
        }

        public static void Ensure1416GuidanceLinkSection()
        {
            GuidanceLink1416.Displayed.Should().BeTrue();
        }

        public static void Ensure1416BreakdownTabs()
        {
            CoreProgramme1416TabLink.Displayed.Should().BeTrue();
            PupilPremium1416TabLink.Displayed.Should().BeTrue();
        }

        public static void Ensure1416DocumentDownload()
        {
            DownloadADocument1416.Displayed.Should().BeTrue();
        }

        public static void EnsureProviderNameForPrint1416IsNotDisplayed()
        {
            ProviderNameForPrint1416.Displayed.Should().BeFalse();
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

        public static void EnsureTabLinkNotShown(IWebElement tab, string linkText)
        {
            var links = tab.FindElements(By.LinkText(linkText));
            links.Count.Should()
                .Be(0, $"Unexpected link with text '{linkText}' displayed");
        }

        public static void EnsureTabElement(IWebElement tab, string elementId, string textContent, bool click = false)
        {
            var element = tab.FindElement(By.Id(elementId));
            var text = element.Text;
            text.Contains(textContent).Should()
                .BeTrue($"Expected element with id '{elementId}' to be present with text '{textContent}' but found '{text}' instead.");
            if (click)
            {
                element.Click();
            }
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
                            if (row.FindElements(By.TagName("span")).Count > 0)
                            {
                                row.FindElements(By.TagName("th"))
                                .Any(th => th.Text.Trim().Contains(item, StringComparison.InvariantCultureIgnoreCase)).Should()
                                .BeTrue($"Expected a table row header column to have '{item}' in '{row.Text}'");
                            }
                            else
                            {
                                row.FindElements(By.TagName("th"))
                                .Any(th => th.Text.Trim().Equals(item, StringComparison.InvariantCultureIgnoreCase)).Should()
                                .BeTrue($"Expected a table row header column to have '{item}' in '{row.Text}'");
                            }
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

        public static void EnsureTabTableContentNotDisplayed(
            IWebElement tabContent,
            string tableId)
        {
            var table = tabContent.FindElementOrDefault(By.Id(tableId));

            table.Should().BeNull();
        }

        public static void Ensure1619AccordionSections()
        {
            AccordionContainer.Displayed.Should().BeTrue();
            AccordionSections.Should().HaveCount(10);
        }

        public static void Ensure1619SpecialAcademyAccordionSections()
        {
            AccordionContainer.Displayed.Should().BeTrue();

            var mathsAndEnglishCofInstance = Driver.Instance.FindElements(By.Id("maths-English-CoF")).Count;

            mathsAndEnglishCofInstance.Should().Be(0);
            CoreProgrammeAccordionSection.Should().BeNull();
            ResidentialAccommodationAccordionSection.Should().BeNull();
            HighNeedsAccordionSection.Displayed.Should().BeTrue();
            StudentFinancialSupportAccordionSection.Displayed.Should().BeTrue();
            TeachersPensionAccordionSection.Should().BeNull();
        }

        public static void EnsureCoreProgrammeAccordionDoesNotExist()
        {
            Func<IWebElement> getCoreProgrammeAccordion = () => CoreProgrammeAccordionSection;

            getCoreProgrammeAccordion.Should().Throw<NoSuchElementException>();
        }

        public static void EnsureGuidance(IWebElement section, string linkText, string content)
        {
            var guidanceLink = section.FindElements(By.ClassName("govuk-details__summary")).First(span =>
                span.Text.Equals(linkText, StringComparison.InvariantCultureIgnoreCase));
            guidanceLink.Displayed.Should().BeTrue($"Expected guidance link with text '{linkText}' to be displayed");
            guidanceLink.Click();

            var disclosureParent = guidanceLink.FindElement(By.XPath(".."));

            var guidanceDetail = disclosureParent.FindElement(By.ClassName("govuk-details__text"));

            guidanceDetail.Displayed.Should().BeTrue();
            guidanceDetail.Text.Contains(content, StringComparison.OrdinalIgnoreCase).Should()
                .BeTrue($"Expected a simple disclosure to have content '{content}' and found '{guidanceDetail.Text}'");
        }

        #endregion


        #region Page Elements

        public static IWebElement AccordionContainer => Driver.Instance.FindElement(By.Id("accordion-1619-statement"));

        public static ReadOnlyCollection<IWebElement> AccordionSections => Driver.Instance.FindElements(By.ClassName("govuk-accordion__section"));

        public static IWebElement CoreProgramme1416TabLink => TabsSection.FindElement(By.LinkText("Core programme"));

        public static IWebElement PupilPremium1416TabLink => TabsSection.FindElement(By.LinkText("Pupil premium"));

        public static IWebElement SchoolBudgetShareTabLink => TabsSection.FindElement(By.LinkText("School budget share"));

        public static IWebElement MinimumFundingGuaranteeTabLink => TabsSection.FindElement(By.LinkText("Minimum funding guarantee"));

        public static IWebElement StartUpGrantTabLink => TabsSection.FindElement(By.LinkText("Start-up grant"));

        public static IWebElement HighNeedsTabLink => TabsSection.FindElement(By.LinkText("High needs"));

        public static IWebElement ProtectionFundingTabLink => TabsSection.FindElement(By.LinkText("Protection Funding"));

        public static IWebElement PostOpeningGrantTabLink => TabsSection.FindElement(By.LinkText("Post-opening grant"));

        public static IWebElement CoreProgrammeTabLink => TabsSection.FindElement(By.LinkText("Core programme"));

        public static IWebElement SchoolBudgetShareTabContent => TabsSection.FindElement(By.Id("school-budget-share"));

        public static IWebElement MinimumFundingGuaranteeTabContent => TabsSection.FindElement(By.Id("minimum-funding-guarantee"));

        public static IWebElement StartUpGrantTabContent => TabsSection.FindElement(By.Id("start-up-grant"));

        public static IWebElement HighNeedsTabContent => TabsSection.FindElement(By.Id("high-needs"));

        public static IWebElement ProtectionFundingTabContent => TabsSection.FindElement(By.Id("protection-funding"));

        public static IWebElement PostOpeningGrantTabContent => TabsSection.FindElement(By.Id("post-opening-grant"));

        public static IWebElement MFGTabLocalAuthorityApplyCappingAndScaling => TabsSection.FindElement(By.Id("LAApplyCappingAndScaling"));

        public static IWebElement MFGTabSchoolHasYearGroupsWithoutPupils => TabsSection.FindElement(By.Id("schoolHasYearGroupsWithoutPupils"));

        public static IWebElement CoreProgrammeTabContent => TabsSection.FindElement(By.Id("core-programme"));

        public static IWebElement PupilPremiumContent => TabsSection.FindElement(By.Id("pupil-premium"));

        public static IWebElement EligibleForSparsity
            => Driver.Instance.FindElement(By.Id("eligible-for-sparsity"));

        public static IWebElement ProtectionFundingTabAveragePupilRate
         => Driver.Instance.FindElement(By.XPath("//table[@id='pfProtectionFundingTable']/tbody/tr[4]/td[2]"));

        public static IWebElement PostOpeningGrantTabTotal
       => PostOpeningGrantTabContent.FindElements(By.CssSelector(".grid-row .block-header-total")).First(tabSection => tabSection.Text.Contains("Total post-opening grant")).FindElement(By.TagName("p"));

        public static IWebElement MathsAndEnglishCoFAccordionSection => Driver.Instance.FindElement(By.Id("maths-English-CoF"));

        public static IWebElement SportingExcellenceAccordionSection => Driver.Instance.FindElement(By.Id("sporting-diploma"));

        public static IWebElement SeaFishingAccordionSection => Driver.Instance.FindElement(By.Id("fishing-diploma"));

        public static IWebElement TuitionFundAccordionSection => Driver.Instance.FindElement(By.Id("tuition-fund"));

        public static IWebElement AdvancedMathsPremiumAccordionSection => Driver.Instance.FindElement(By.Id("advanced-maths-premium"));

        public static IWebElement AdvancedMathsNoEligibleStudents => Driver.Instance.FindElement(By.Id("no-advanced-maths-students"));

        public static IWebElement HighNeedsAccordionSection => IWebElementExtensions.FindElementOrDefault(By.Id("high-needs"));

        public static IWebElement SixteenTo19DiscretionaryBursaryFundAccordionSection => IWebElementExtensions.FindElementOrDefault(By.Id("tab-16-19-discretionary-bursary-fund"));

        public static IWebElement TeachersPensionAccordionSection => IWebElementExtensions.FindElementOrDefault(By.Id("pension-scheme"));

        public static IWebElement DiplomaAccordionSection => Driver.Instance.FindElement(By.Id("diploma"));

        public static IWebElement IndustryPlacementAccordionSection => IWebElementExtensions.FindElementOrDefault(By.Id("industry-placement-funding"));

        public static IWebElement CoreProgrammeAccordionSection => IWebElementExtensions.FindElementOrDefault(By.Id("core-programme"));

        public static IWebElement HighValueCoursesPremiumAccordionSection => Driver.Instance.FindElement(By.Id("high-value-courses-premium"));

        public static IWebElement StudentFinancialSupportAccordionSection => Driver.Instance.FindElement(By.Id("student-financial-support"));

        public static IWebElement ResidentialAccommodationAccordionSection => IWebElementExtensions.FindElementOrDefault(By.Id("residential-accommodation"));

        public static IWebElement AdjustedDiscretionaryBursaryFundTableCell
      => Driver.Instance.FindElement(By.Id("adjusted-bursary-fund-message"));

        public static IWebElement ReducedPupilNumberGuidance
      => Driver.Instance.FindElement(By.Id("reduced-pupil-numbers-guidance"));

        protected static IWebElement DownloadADocument => Driver.Instance.FindElement(By.ClassName("download-a-document-container-new"));

        protected static IWebElement DownloadADocument1416 => Driver.Instance.FindElement(By.ClassName("download-a-document-new"));

        protected static IWebElement DocumentDownloadContainer
            => Driver.Instance.WaitToFindElement(By.ClassName("download-a-document-container"));

        protected static IWebElement PrintOrSaveContainer
            => Driver.Instance.WaitToFindElement(By.ClassName("print-a-document-container"));

        protected static IWebElement GagGuidanceLink
            => Driver.Instance.WaitToFindElement(By.PartialLinkText("General annual grant allocation guide:"));

        protected static IWebElement GuidanceLink1619
            => Driver.Instance.WaitToFindElement(By.PartialLinkText("16 to 19 funding allocation guide:"));

        protected static IWebElement GuidanceLink1416
            => Driver.Instance.WaitToFindElement(By.PartialLinkText("14 to 16 funding guide:"));

        protected static IWebElement AllocationHistoryLink
            => Driver.Instance.WaitToFindElement(By.LinkText("View history of allocated funding"));

        protected static IEnumerable<IWebElement> H3Elements
            => Driver.Instance.FindElements(By.TagName("H3"));

        protected static IWebElement TotalAllocationSection
            => Driver.Instance.FindElement(By.ClassName("pfb-total-allocation"));

        protected static IWebElement TabsSection
            => Driver.Instance.FindElement(By.ClassName("govuk-tabs"));

        protected static IWebElement NMSSGuidanceLink
            => Driver.Instance.WaitToFindElement(By.PartialLinkText("Non maintained special school funding allocation guide"));

        protected static IWebElement ProviderNameForPrint1416
            => Driver.Instance.WaitToFindElement(By.Id("providername-for-print"));

        #endregion
    }
}