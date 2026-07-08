using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;
using System.Collections.Generic;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass, TestCategory("Regression"), TestCategory("CoreRegression")]
    [Ignore]
    public class IndicativeProviderFundingBreakdownPageGAGTests : LoggedInRegressionTestBase
    {
        #region Private fields

        private readonly ApplicationConfiguration _applicationConfiguration;

        private string ProviderUserName_ExistingSpecialSchool => "10087061 - External User 112 - Indicative Primary Special";

        private string ProviderUserName_Secondary => "10086776 - External User 113 - Indicative Secondary Mainstream";

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        private const int YearFrom = 2023;

        private const int YearFromMinus1 = 2022;

        private const int YearTo = 2024;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IndicativeProviderFundingBreakdownPageGAGTests"/> class.
        /// </summary>
        public IndicativeProviderFundingBreakdownPageGAGTests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion


        #region Tests

        [TestMethod]
        public void IndicativeProviderFundingBreakdownPage_GAG_BasicLayout_Secondary()
        {
            // Arrange Act
            IndicativeProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_Secondary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();

            // Assert
            IndicativeProviderFundingBreakdownPage.EnsureBreakdownTabs();
            ViewYourFundingBasePage.EnsureBreadcrumbs();
            IndicativeProviderFundingBreakdownPage.EnsureCurrentProviderPage();
            IndicativeProviderFundingBreakdownPage.EnsureAllocationHistory();
            IndicativeProviderFundingBreakdownPage.EnsureGuidanceLinkSection();
            IndicativeProviderFundingBreakdownPage.EnsureTotalAllocation("£5,740,800\r\nincrease\r\n£3,000,000");
            IndicativeProviderFundingBreakdownPage.EnsurePrintOrSaveStatement();
            IndicativeProviderFundingBreakdownPage.EnsureDocumentDownload();
            IndicativeProviderFundingBreakdownPage.EnsureGuidanceLinkSection();
            IndicativeProviderFundingBreakdownPage.EnsureMandatoryH3Headers();
        }

        [TestMethod]
        public void IndicativeProviderFundingBreakdownPage_GAG_CheckAuthorise()
        {
            // Arrange Act
            IndicativeProviderFundingBreakdownPage.NavigateToPageWithoutLogin();

            // Assert
            IndicativeProviderFundingBreakdownPage.EnsureNavigatedToLogin();
        }

        [TestMethod]
        public void IndicativeProviderFundingBreakdownPage_GAG_HasBreadcrumbsLinksToPages()
        {
            // Arrange Act
            IndicativeProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_ExistingSpecialSchool, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();
            var finalBreadcrumbText = $"Indicative General annual grant {YearFrom} to {YearTo}";

            // Assert
            IndicativeProviderFundingBreakdownPage.EnsureBreadcrumbsAndFinalText(finalBreadcrumbText, new[] { "Home", "Allocation statements", "Select a previous statement to compare your figures" });
            ViewYourFundingBasePage.EnsureAndClickBreadcrumb("Allocation statements");
            ProviderPage.EnsureCurrentProviderPage();
        }

        [TestMethod]
        public void IndicativeProviderFundingBreakdownPage_GAG_MinimumFundingGuaranteeTab_Test_Secondary()
        {
            // Arrange
            IndicativeProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_Secondary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();

            // Act
            IndicativeProviderFundingBreakdownPage.ClickOnMinimumFundingGuaranteeTab();

            // Assert Tab Total
            IndicativeProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                IndicativeProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                "Total minimum funding guarantee",
                "£50,000");

            // Assert Tables
            foreach (var tableData in GetMinimumFundingGuaranteeTableData(true, false))
            {
                IndicativeProviderFundingBreakdownPage.EnsureTabTableContentData(
                    IndicativeProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                    tableData);
            }

            // Assert Paragraph content
            ProviderFundingBreakdownPage.EnsureParagraphContent(
                ProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                $"Protection funding (opens in new tab) to make sure change in per pupil school budget share between {YearFromMinus1} to {YearFrom} and {YearFrom} to {YearTo} will not fall below the local authority guaranteed change of -1.5%.");


            // Assert Links
            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                "Protection funding (opens in new tab)");

            // Assert Bottom Allocation total
            IndicativeProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                IndicativeProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                "Total minimum funding guarantee",
                "£50,000");
        }

        [TestMethod]
        public void IndicativeProviderFundingBreakdownPage_GAG_HighNeedsTab_Test()
        {
            // Arrange
            IndicativeProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_ExistingSpecialSchool, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();

            // Act
            IndicativeProviderFundingBreakdownPage.ClickOnHighNeedsTab();

            // Assert Tab Total
            IndicativeProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                "Total high needs",
                "£6,000");

            // Assert Tables
            var highNeedsTableData = GetHighNeedsTableData();

            IndicativeProviderFundingBreakdownPage.EnsureTabTableContentData(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                new TableData
                {
                    Id = "pre16highNeedsTable",
                    TableRowData = highNeedsTableData
                });

            IndicativeProviderFundingBreakdownPage.EnsureSimpleDisclosure(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                "Where pre-16 high needs places come from",
                $"Places are either rolled forward from {YearFromMinus1} to {YearFrom} or your local authority agreed it with ESFA. This provides a base level of funding.\r\nYou must agree additional funding above this level with your local authority. Local authorities pay you the top-up funding directly. It is not shown in this statement.");


            // Assert Links
            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                "More about hospital education funding (opens in new tab)");

            // Assert Bottom Allocation total
            IndicativeProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                "Total high needs",
                "£6,000");
        }

        [TestMethod]
        public void IndicativeProviderFundingBreakdownPage_GAG_SpecialSchool_HighNeedsTab_Test()
        {
            // Arrange
            IndicativeProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_ExistingSpecialSchool, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();

            // Act
            IndicativeProviderFundingBreakdownPage.ClickOnHighNeedsTab();

            // Assert Tab Total
            IndicativeProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                "Total high needs",
                "£6,000");

            // Assert Tables
            foreach (var tableData in GetSpecialSchoolHighNeedsTableData())
            {
                IndicativeProviderFundingBreakdownPage.EnsureTabTableContentData(
                    IndicativeProviderFundingBreakdownPage.HighNeedsTabContent,
                    tableData);
            }

            IndicativeProviderFundingBreakdownPage.EnsureSimpleDisclosure(
                IndicativeProviderFundingBreakdownPage.HighNeedsTabContent,
                "Where pre-16 high needs places come from",
                $"Places are either rolled forward from {YearFromMinus1} to {YearFrom} or your local authority agreed it with ESFA. This provides a base level of funding.\r\nYou must agree additional funding above this level with your local authority. Local authorities pay you the top-up funding directly. It is not shown in this statement.");

            // Assert Links
            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.HighNeedsTabContent,
                "More about pre-16 high needs (opens in new tab)");

            // Assert Bottom Allocation total
            IndicativeProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                IndicativeProviderFundingBreakdownPage.HighNeedsTabContent,
                "Total high needs",
                "£6,000");
        }

        [TestMethod]
        public void IndicativeProviderFundingBreakdownPage_GAG_StartUpGrantTab_Test_Secondary()
        {
            // Arrange
            IndicativeProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_Secondary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();

            // Act
            IndicativeProviderFundingBreakdownPage.ClickOnStartUpGrantTab();

            // Assert Tab Total
            IndicativeProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                "Total start-up grant for new sponsored academy",
                "£6,000");

            // Assert Tables
            var startUpGrantTable = GetStartUpGrantTableData(false, true, false);

            IndicativeProviderFundingBreakdownPage.EnsureTabTableContentData(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                new TableData
                {
                    Id = "startUpGrantTable",
                    TableRowData = startUpGrantTable
                });

            // Assert Links
            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                "More about startup-grant (opens in new tab)");

            // Assert Bottom Allocation total
            IndicativeProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                IndicativeProviderFundingBreakdownPage.StartUpGrantTabContent,
                "Total start-up grant for new sponsored academy",
                "£6,000");
        }

        [TestMethod]
        public void IndicativeProviderFundingBreakdownPage_GAG_StartUpGrantTab_Test_ZeroStartupFunding_Secondary()
        {
            // Arrange
            IndicativeProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_Secondary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();

            // Act
            IndicativeProviderFundingBreakdownPage.ClickOnStartUpGrantTab();

            // Assert Tab Total
            IndicativeProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                IndicativeProviderFundingBreakdownPage.StartUpGrantTabContent,
                "Total start-up grant for new sponsored academy",
                "£6,000");

            // Assert Links
            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.StartUpGrantTabContent,
                "More about startup-grant (opens in new tab)");
        }

        [TestMethod]
        public void IndicativeProviderFundingBreakdownPage_GAG_SchoolBudgetShare_Test_Existing_SpecialSchool()
        {
            // Arrange
            IndicativeProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_ExistingSpecialSchool, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();

            var expectedTotal = "£336,228.22";

            // Act
            IndicativeProviderFundingBreakdownPage.ClickOnSchoolShareBudgetTab();

            // Assert Tab Total
            IndicativeProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total school budget share",
                expectedTotal);

            // Assert Tables
            foreach (var tableData in GetSchoolShareBudgetTabTableData(true, false))
            {
                IndicativeProviderFundingBreakdownPage.EnsureTabTableContentData(
                    IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                    tableData);
            }

            // Assert Links
            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about basic entitlement (opens in new tab)");

            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "What Income Deprivation Affecting Children Index (IDACI) bands are (opens in new tab)");

            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about free school meals (opens in new tab)");

            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about low prior attainment (opens in new tab)");

            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about English as an additional language (opens in new tab)");

            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about mobility (opens in new tab)");

            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about lump sum (opens in new tab)");

            // Assert funding line Allocation totals
            IndicativeProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total pupil-led factors for full year",
                "£5,372,898.02");

            IndicativeProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total pupil-led factors for part year",
                "£318,552.95");

            IndicativeProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total other factors for full year",
                "£115,023");

            IndicativeProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total other factors for part year",
                "£115,023");

            IndicativeProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total funding adjustments for full year",
                "£0");

            IndicativeProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total funding adjustments for part year",
                "£985");

            IndicativeProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total school budget share",
                expectedTotal);
        }

        [TestMethod]
        public void IndicativeProviderFundingBreakdownPage_GAG_SchoolBudgetShare_Test_Existing_Secondary()
        {
            // Arrange
            IndicativeProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_Secondary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();
            var total = "£3,740,800";

            // Act
            IndicativeProviderFundingBreakdownPage.ClickOnSchoolShareBudgetTab();

            // Assert Tab Total
            IndicativeProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total school budget share",
                total);

            // Assert Tables
            foreach (var tableData in GetSchoolShareBudgetTabTableData(false, true))
            {
                IndicativeProviderFundingBreakdownPage.EnsureTabTableContentData(
                    IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                    tableData);
            }

            // Assert Links
            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about basic entitlement (opens in new tab)");

            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "What Income Deprivation Affecting Children Index (IDACI) bands are (opens in new tab)");

            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about free school meals (opens in new tab)");

            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about low prior attainment (opens in new tab)");

            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about English as an additional language (opens in new tab)");

            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about mobility (opens in new tab)");

            IndicativeProviderFundingBreakdownPage.EnsureTabLink(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about lump sum (opens in new tab)");

            // Disclosure check.
            IndicativeProviderFundingBreakdownPage.EligibleForSparsity.Click();
            var eligibleForSparsityTable = new TableData
            {
                Id = "eligibleForSparsityDataTable",
                TableRowData = EligibleForSparsityDataTable()
            };
            IndicativeProviderFundingBreakdownPage.EnsureTabTableContentData(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                eligibleForSparsityTable);

            // Assert funding line Allocation totals
            IndicativeProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total pupil-led factors for full year",
                "£5,372,898.02");

            IndicativeProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total pupil-led factors for part year",
                "£2,541,268.73");

            IndicativeProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total other factors for full year",
                "£115,023");

            IndicativeProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total other factors for part year",
                "£115,023");

            IndicativeProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total funding adjustments for full year",
                "£0");

            IndicativeProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total funding adjustments for part year",
                "£83,247.94");

            IndicativeProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                IndicativeProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total school budget share",
                total);
        }

        #endregion


        #region Private Helpers

        private static IEnumerable<TableRowData> GetHighNeedsTableData()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Pre-16 high needs",
                    "Rate",
                    "Places",
                    "Full year sub-total",
                    "Part year sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Alternative provision",
                    "£10,000",
                    "52\r\nincrease\r\n52",
                    "£0",
                    "£2,000"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Special",
                    "£10,000",
                    "0",
                    "£0",
                    "£0"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total pre-16 high needs",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "£0"
                }
            };
        }

        private static IEnumerable<TableRowData> GetStartUpGrantTableData(bool showPrimary, bool showSecondary, bool showAllThrough)
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Start-up grant",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Flat rate - paid first year of opening",
                    "£0"
                }
            };

            if (showPrimary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Growth towards full capacity - paid over 2 years to primary schools",
                        "£25,000"
                    }
                };
            }

            if (showSecondary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Growth towards full capacity - paid over 3 years to secondary schools",
                    "£25,000"
                }
                };
            }

            if (showAllThrough)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Growth towards full capacity - paid over 2 years to primary schools and 3 years to secondary schools",
                        "£25,000"
                    }
                };
            }

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total start-up grant",
                    "£6,000\r\nincrease\r\n£6,000"
                }
            };
        }

        private static IEnumerable<TableData> GetSchoolShareBudgetTabTableData(bool showPrimary, bool showSecondary)
        {
            yield return new TableData
            {
                Id = "schoolsBasicEntitlementTable",
                TableRowData = GetBasicEntitlementTableData_Existing(showPrimary, showSecondary)
            };

            yield return new TableData
            {
                Id = "schoolsDeprivationTable",
                TableRowData = GetDeprivationTableData_Existing(showPrimary, showSecondary)
            };

            yield return new TableData
            {
                Id = "lowPriorityTable",
                TableRowData = GetLowPriorityTableData(showPrimary, showSecondary)
            };
            yield return new TableData
            {
                Id = "englishaaalTable",
                TableRowData = GetEnglishAsAdditionalLanguageTableData(showPrimary, showSecondary)
            };
            yield return new TableData
            {
                Id = "mobilityTable",
                TableRowData = GetMobilityTableData(showPrimary, showSecondary)
            };

            yield return new TableData
            {
                Id = "lumpSumTable",
                TableRowData = GetLumpSumTableData(showPrimary, showSecondary)
            };


            yield return new TableData
            {
                Id = "sparsityTable",
                TableRowData = GetSpecialSparsityTableData(showPrimary, showSecondary)
            };


            yield return new TableData
            {
                Id = "minimumFundingTable",
                TableRowData = MinimumFundingDataTable(showPrimary, showSecondary)
            };
        }

        private static IEnumerable<TableData> GetSpecialSchoolHighNeedsTableData()
        {
            yield return new TableData
            {
                Id = "pre16highNeedsTable",
                TableRowData = GetHighNeedsSpecialTable()
            };

            yield return new TableData
            {
                Id = "hospitalEducationFundingTable",
                TableRowData = GetHospitalEducationFunding()
            };
        }

        private static IEnumerable<TableRowData> GetHighNeedsSpecialTable()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Pre-16 high needs",
                    "Rate",
                    "Places",
                    "Full year sub-total",
                    "Part year sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Alternative provision",
                    "£10,000",
                    "52\r\nincrease\r\n52",
                    "£0",
                    "£2,000"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Special",
                    "£10,000",
                    "0",
                    "£0",
                    "£0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total pre-16 high needs",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "£0"
                }
            };
        }

        private static IEnumerable<TableRowData> GetHospitalEducationFunding()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Hospital education funding",
                    "Places",
                    "Full year sub-total",
                    "Part year sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Hospital education funding",
                    "15\r\ndecrease\r\n15",
                    "£0",
                    "£1,249\r\ndecrease\r\n£1,909"
                }
            };
        }

        private static IEnumerable<TableData> GetMinimumFundingGuaranteeTableData(bool isPrimary, bool isSecondary)
        {
            yield return new TableData
            {
                Id = "mfgDaysOpenGuaranteeTable",
                TableRowData = GetAdjustedSchoolShareTable2021Rows()
            };

            yield return new TableData
            {
                Id = "mfgPupilAdjustmentTable",
                TableRowData = GetMinimumFundingGuaranteeTableRows()
            };

            yield return new TableData
            {
                Id = "mfgPupilAdjustmentOpenTable",
                TableRowData = GetTotalMinimumFundingGuaranteeTableRows()
            };
        }

        private static IEnumerable<TableRowData> GetAdjustedSchoolShareTable2021Rows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Minimum funding guarantee for days open (not adjusted for pupils)",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Minimum funding guarantee for full year",
                    string.Empty,
                    "£0",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Days in full year",
                    "Divide",
                    "365",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Days open",
                    "Multiply",
                    "365",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Minimum funding guarantee for days open (not adjusted for pupils)",
                    "Equals",
                    "£0",
                }
            };
        }

        private static IEnumerable<TableRowData> GetMinimumFundingGuaranteeTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Pupil adjustment",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Pupil numbers used in original minimum funding guarantee calculation",
                    string.Empty,
                    "£1,077",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Pupil numbers actually funded",
                    "Divide",
                    "£571",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Pupil adjustment",
                    "Equals",
                    "£23,834.42",
                }
            };
        }

        private static IEnumerable<TableRowData> GetTotalMinimumFundingGuaranteeTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Minimum funding guarantee for days open (adjusted for pupils)",
                    "Calculation",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Minimum funding guarantee for days open (not adjusted for pupils)",
                    string.Empty,
                    "£0",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Pupil adjustment",
                    "Multiply",
                    "£23,834.42",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Minimum funding guarantee for days open (adjusted for pupils)",
                    "Equals",
                    "£50,000\r\nincrease\r\n£50,000",
                }
            };
        }

        private static IEnumerable<TableRowData> GetBasicEntitlementTableData_Existing(bool showPrimary, bool showSecondary)
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Basic entitlement",
                    "Full year sub-total",
                    "Part year sub-total"
                }
            };

            if (showPrimary)
            {
                if (showSecondary)
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary",
                            "365",
                            "£1,060,024.33"
                        }
                    };
                }
                else
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary",
                            "£0",
                            "£0"
                        }
                    };
                }
            }

            if (showSecondary)
            {
                if (showPrimary)
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Key Stage 3",
                            "543",
                            "£2,203,873.25"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Key Stage 4",
                            "341",
                            "£1,867,178.23"
                        }
                    };
                }
                else
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Key Stage 3",
                            "£2,642,160",
                            "£1,340,339.55"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Key Stage 4",
                            "£1,939,878",
                            "£982,421.44"
                        }
                    };
                }
            }

            if (showPrimary && showSecondary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total Basic Entitlement",
                        string.Empty,
                        string.Empty,
                        "1,249",
                        "£5,131,075.81"
                    }
                };
            }
            else if (showPrimary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total Basic Entitlement",
                        "£5,131,075.81",
                        "£328,614.40\r\nincrease\r\n£70,000"
                    }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total Basic Entitlement",
                        "£5,131,075.81",
                        "£2,322,760.99"
                    }
                };
            }
        }

        private static IEnumerable<TableRowData> GetDeprivationTableData_Existing(bool showPrimary, bool showSecondary)
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Deprivation",
                    "Full year sub-total",
                    "Part year sub-total"
                }
            };

            if (showPrimary)
            {
                if (showSecondary)
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band A (most deprived)",
                            "£1,401.66",
                            "0",
                            "884",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band B",
                            "£1,401.64",
                            "0.006795",
                            "884",
                            "£8,419.36"
                        }
                    };
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band C",
                            "£0",
                            "0",
                            "884",
                            "£270.97"
                        }
                    };
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band D",
                            "£130.31",
                            "0.02718",
                            "884",
                            "£3,130.89"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band E",
                            "£130.29",
                            "0.074745",
                            "884",
                            "£8,608.57"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band F (least deprived)",
                            "£65.36",
                            "0.012458",
                            "884",
                            "£719.77"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary free school meals",
                            "£2,183.70",
                            "0.069005",
                            "884",
                            "£133,206.46"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary pupils eligible for free school meals in past 6 years",
                            "£0",
                            "0.14023",
                            "884",
                            "£0"
                        }
                    };
                }
                else
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band A (most deprived)",
                            "£0",
                            "£1,254.06\r\ndecrease\r\n£1,997"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band B",
                            "£0",
                            "£0"
                        }
                    };
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band C",
                            "£0",
                            "£0"
                        }
                    };
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band D",
                            "£0",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band E",
                            "£0",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band F (least deprived)",
                            "£0",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary free school meals",
                            "£0",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary pupils eligible for free school meals in past 6 years",
                            "£0",
                            "£0"
                        }
                    };
                }
            }

            if (showSecondary)
            {
                if (showPrimary)
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band A (most deprived)",
                            "£617.18",
                            "0",
                            "96\r\nincrease\r\n8",
                            "£1,254.06\r\ndecrease\r\n£1,997"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band B",
                            "£447.45",
                            "0",
                            "96\r\nincrease\r\n8",
                            "£0"
                        }
                    };
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band C",
                            "£416.60",
                            "0",
                            "96\r\nincrease\r\n8",
                            "£0"
                        }
                    };
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band D",
                            "£385.74",
                            "0.017544",
                            "96\r\nincrease\r\n8",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band E",
                            "£257.16",
                            "0.087719",
                            "96\r\nincrease\r\n8",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band F (least deprived)",
                            "£216.01",
                            "0.035088",
                            "96\r\nincrease\r\n8",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary free school meals",
                            "£462.88",
                            "0.155172",
                            "96\r\nincrease\r\n8",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary pupils eligible for free school meals in past 6 years",
                            "£576.03",
                            "0.155172",
                            "96\r\nincrease\r\n8",
                            "£0"
                        }
                    };
                }
                else
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band A (most deprived)",
                            "£22,857.45",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band B",
                            "£25,166.73",
                            "£0"
                        }
                    };
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band C",
                            "£19,858.88",
                            "£0"
                        }
                    };
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band D",
                            "£67,375.12",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band E",
                            "£59,124.80",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band F (least deprived)",
                            "£42,963.78",
                            "£289.95"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary free school meals",
                            "£48,364",
                            "£13,640.07"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary pupils eligible for free school meals in past 6 years",
                            "£166,810.81",
                            "£53,851.40"
                        }
                    };
                }
            }

            if (showPrimary && showSecondary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total deprivation",
                        "£196,663.55"
                    }
                };
            }
            else if (showPrimary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total deprivation",
                        "£196,663.55",
                        "£17,434.06"
                    }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total deprivation",
                        "£196,663.55",
                        "£67,781.42"
                    }
                };
            }
        }

        private static IEnumerable<TableRowData> GetChildrenLookedAfterTableData(bool showPrimary, bool showSecondary)
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Looked-after children",
                    "Full year sub-total",
                    "Part year sub-total"
                }
            };

            if (showPrimary && showSecondary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total looked-after children",
                        "£0",
                        "£8,646.82"
                    }
                };
            }
            else if (showPrimary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total looked-after children",
                        "£0",
                        "£1,520\r\nincrease\r\n£1,520"
                    }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total looked-after children",
                        "£0",
                        "£0"
                    }
                };
            }
        }

        private static IEnumerable<TableRowData> GetLowPriorityTableData(bool showPrimary, bool showSecondary)
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Low prior attainment",
                    "Full year sub-total",
                    "Part year sub-total"
                }
            };

            if (showPrimary)
            {
                if (showSecondary)
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary pupils not achieving the expected levels of development",
                            "£235.19",
                            "0.227273",
                            "365",
                            "£19,510.36"
                        }
                    };
                }
                else
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary pupils not achieving the expected levels of development",
                            "£0",
                            "£0"
                        }
                    };
                }
            }

            if (showSecondary)
            {
                if (showPrimary)
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary pupils not achieving the expected standards in KS3 tests",
                            "£697.46",
                            "0.192346",
                            "884",
                            "£118,591.94"
                        }
                    };
                }
                else
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary pupils not achieving the expected standards in KS3 tests",
                            "£329,650.45",
                            "£149,341.57"
                        }
                    };
                }
            }

            if (showPrimary && showSecondary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total low prior attainment",
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        "£138,102.30"
                    }
                };
            }
            else if (showPrimary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total low prior attainment",
                        "£138,102.30",
                        "£14,831.23"
                    }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total low prior attainment",
                        "£138,102.30",
                        "£149,341.57"
                    }
                };
            }
        }

        private static IEnumerable<TableRowData> GetEnglishAsAdditionalLanguageTableData(bool showPrimary, bool showSecondary)
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "English as an additional language",
                    "Full year sub-total",
                    "Part year sub-total"
                }
            };

            if (showPrimary && showSecondary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Primary pupils in first 3 years of education within England",
                        "£291.21",
                        "0.032653",
                        "365",
                        "£3,470.72"
                    }
                };
            }
            else if (showPrimary)
            {
                if (showPrimary)
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary pupils in first 3 years of education within England",
                            "£0",
                            "£27,673.26"
                        }
                    };
                }
            }

            if (showSecondary)
            {
                if (showPrimary)
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary pupils in first 3 years of education within England",
                            "£2,019.10",
                            "0.004535",
                            "884",
                            "£8,094.43"
                        }
                    };
                }
                else
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary pupils in first 3 years of education within England",
                            "£8,688",
                            "£1,384.75"
                        }
                    };
                }
            }

            if (showPrimary && showSecondary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total English as an additional language",
                        "£11,565.15"
                    }
                };
            }
            else if (showPrimary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total English as an additional language",
                        "£11,565.15",
                        "£27,673.26"
                    }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total English as an additional language",
                        "£11,565.15",
                        "£1,384.75"
                    }
                };
            }
        }

        private static IEnumerable<TableRowData> GetMobilityTableData(bool showPrimary, bool showSecondary)
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Mobility",
                    "Full year sub-total",
                    "Part year sub-total"
                }
            };

            if (showPrimary)
            {
                if (showSecondary)
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary pupils starting school on unusual entry dates (proportion above 6%)",
                            "£729.71",
                            "0",
                            "365",
                            "£0"
                        }
                    };
                }
                else
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary pupils starting school on unusual entry dates (proportion above 6%)",
                            "£0",
                            "£0"
                        }
                    };
                }
            }

            if (showSecondary)
            {
                if (showPrimary)
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary pupils starting school on unusual entry dates (proportion above 6%)",
                            "£729.71",
                            "0",
                            "884",
                            "£0"
                        }
                    };
                }
                else
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary pupils starting school on unusual entry dates (proportion above 6%)",
                            "£0",
                            "£0"
                        }
                    };
                }
            }

            if (showPrimary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total mobility",
                        "£0",
                        "£0"
                    }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total mobility",
                        "£0",
                        "£0"
                    }
                };
            }
        }

        private static IEnumerable<TableRowData> GetLumpSumTableData(bool showPrimary, bool showSecondary)
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Lump sum",
                    "Full year sub-total",
                    "Part year sub-total"
                }
            };

            if (showPrimary)
            {
                if (showSecondary)
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Total primary lump sum",
                            "£0"
                        }
                    };
                }
                else
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Total primary lump sum",
                            "£127,777.83",
                            "£0"
                        }
                    };
                }
            }

            if (showSecondary)
            {
                if (showPrimary)
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Total secondary lump sum",
                            "£110,000",
                            "£110,000"
                        }
                    };
                }
                else
                {
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Total secondary lump sum",
                            "£110,000"
                        }
                    };
                }
            }
        }

        private static IEnumerable<TableRowData> GetSpecialSparsityTableData(bool showPrimary, bool showSecondary)
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Sparsity",
                    "Sub-Total"
                }
            };

            if (showPrimary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total for small academy in sparsely populated area",
                        "£0"
                    }
                };
            }

            if (showSecondary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total for small academy in sparsely populated area",
                        "£6,283.33"
                    }
                };
            }
        }

        private static IEnumerable<TableRowData> EligibleForSparsityDataTable()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Sparsity data",
                    "Value"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Maximum sparsity amount set by local authority",
                    "£65,000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Method chosen by local authority",
                    "National funding Formula"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Minimum distance from second nearest school (miles) set by local authority",
                    "3"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Academy's distance from second nearest eligible school (miles)",
                    "4.299859"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Maximum year group size set by local authority",
                    "120"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Academy's average year group size",
                    "114.2"
                }
            };
        }

        private static IEnumerable<TableRowData> MinimumFundingDataTable(bool showPrimary, bool showSecondary)
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Minimum funding level",
                    "Full year sub-total",
                    "Part year sub-total"
                }
            };

            if (showPrimary && showSecondary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Minimum per pupil funding level set by local authority",
                        "£4,642.86"
                    }
                };
            }
            else if (showPrimary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Minimum per pupil funding level set by local authority",
                        "£0",
                        "Not Applicable"
                    }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Minimum per pupil funding level set by local authority",
                        "£4,800",
                        "Not Applicable"
                    }
                };
            }

            if (showPrimary && showSecondary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total adjustment to make sure minimum funding level is met",
                        "£52,487.76"
                    }
                };
            }
            else if (showPrimary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total adjustment to make sure minimum funding level is met",
                        "£985\r\nincrease\r\n£985"
                    }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total adjustment to make sure minimum funding level is met",
                        "£0",
                        "£83,247.94"
                    }
                };
            }
        }

        #endregion
    }
}