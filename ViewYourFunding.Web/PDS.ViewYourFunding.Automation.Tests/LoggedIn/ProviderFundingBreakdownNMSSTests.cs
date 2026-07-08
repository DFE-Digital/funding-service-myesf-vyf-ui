using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;
using System.Collections.Generic;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass, TestCategory("Regression"), TestCategory("CoreRegression")]
    [Ignore]
    public class ProviderFundingBreakdownNMSSTests : LoggedInRegressionTestBase
    {
        #region Private fields

        private static string ProviderUserNameForNMSS => "10015031 - External User 114 - NMSS";

        private static string ProviderUserNameForNMSSDecreaseInStudentNumber => "10016972 - External User 119 - NMSS2";

        private static string ProviderUserNameForNMSSIdenticalStudentNumbers => "10001232 - External User 121 - NMSS3 - Same student numbers";

        private readonly ApplicationConfiguration _applicationConfiguration;

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        private static string yearMinusOne = "2019";

        private static string yearZero = "2020";

        private static string currentYear = "2021";

        private static string nextYear = "2022";

        private static string ProviderUserNameAdjustedAboveUpper => "10030456 - External User 115 - 1619 Adjusted Above Upper";

        private static string ProviderUserNameAdjustedBelowLower => "10054167 - External User 116 - 1619 Adjusted Below Lower";

        private static string ProviderUserNameAdjustedBelow500 => "10033567 - External User 117 - 1619 Adjusted Below 500";

        private static string ProviderUserNameInLimit => "10061616 - External User 118 - 1619 In Limit";

        private static string DiscretionaryBursaryFundInLimitMessage => "The bursary has not been adjusted because this year's discretionary bursary fund figure is between the transition limits.";

        private static string DiscretionaryBursaryFundOutsudeOfLimitMessage => "The bursary has been adjusted because this year's discretionary bursary fund figure is outside the transition limits.";

        private static string DiscretionaryBursaryFundBelow500Message => "The bursary figure has been adjusted to the £500 minimum allocation.";

        private static string DiscretionaryBursaryFundReducedPupilNumberGuidance => $"Any reduction in pupil numbers is set to zero.\r\nThis ensures no school is funded below its autumn {yearMinusOne} census pupil numbers.";


        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderFundingBreakdownNMSSTests"/> class.
        /// </summary>
        public ProviderFundingBreakdownNMSSTests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion


        #region Tests

        [TestMethod]
        public void ProviderFundingBreakdownPage_NMSS_BasicLayout()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateToNMSSPageViaLogin(ProviderUserNameForNMSS, ProviderUserPassword);

            // Assert
            ViewYourFundingBasePage.EnsureBreadcrumbs();
            ProviderFundingBreakdownPage.EnsureAllocationHistory();
            ProviderFundingBreakdownPage.EnsureNMSSGuidanceLinkSection();
            ProviderFundingBreakdownPage.EnsurePrintOrSaveStatement();
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_NMSS_HighNeeds_EnsureContent()
        {
            List<(string Id, string Text, bool Click)> textElements = GetHighNeedsTextElements();
            ProviderFundingBreakdownPage.NavigateToNMSSPageViaLogin(ProviderUserNameForNMSS, ProviderUserPassword);

            // Act
            // Assert Tables
            foreach (var tableData in GetHighNeedsCalculationOfIncreaseTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.HighNeedsAccordionSection,
                    tableData);
            }

            foreach (var tableData in GetHighNeedsFundedPupilsTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.HighNeedsAccordionSection,
                    tableData);
            }

            foreach (var tableData in GetHighNeedsFundedPlacesTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.HighNeedsAccordionSection,
                    tableData);
            }

            foreach (var tableData in GetHighNeedsTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.HighNeedsAccordionSection,
                    tableData);
            }

            foreach (var textElement in textElements)
            {
                ProviderFundingBreakdownPage.EnsureTabElement(ProviderFundingBreakdownPage.HighNeedsAccordionSection, textElement.Id, textElement.Text, textElement.Click);
            }

            ProviderFundingBreakdownPage.EnsureTabLink(
                    ProviderFundingBreakdownPage.HighNeedsAccordionSection,
                    "More about pupil numbers (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                    ProviderFundingBreakdownPage.HighNeedsAccordionSection,
                    "Where places comes from (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.HighNeedsAccordionSection,
                "More about amendments to high needs places (opens in new tab)");
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_NMSS_DiscretionaryBursaryFund_EnsureContent()
        {
            // Arrange Act
            List<(string Id, string Text, bool Click)> textElements = GetDiscretionaryBursaryFundTextElements();
            ProviderFundingBreakdownPage.NavigateToNMSSPageViaLogin(ProviderUserNameForNMSS, ProviderUserPassword, "discretionary-bursary");

            // Assert Tables
            foreach (var tableData in GetDiscretionaryBursaryFundBreakdownTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.SixteenTo19DiscretionaryBursaryFundAccordionSection,
                    tableData);
            }

            foreach (var tableData in GetAdjustedDiscretionaryBursaryFundBreakdownTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.SixteenTo19DiscretionaryBursaryFundAccordionSection,
                    tableData);
            }

            foreach (var tableData in GetDiscretionaryBursaryFundExceptionalAdjustmentTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.SixteenTo19DiscretionaryBursaryFundAccordionSection,
                    tableData);
            }

            foreach (var textElement in textElements)
            {
                ProviderFundingBreakdownPage.EnsureTabElement(ProviderFundingBreakdownPage.SixteenTo19DiscretionaryBursaryFundAccordionSection, textElement.Id, textElement.Text, textElement.Click);
            }

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SixteenTo19DiscretionaryBursaryFundAccordionSection,
                "More about exceptional adjustments (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SixteenTo19DiscretionaryBursaryFundAccordionSection,
                "More about the 16 to 19 bursary fund (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SixteenTo19DiscretionaryBursaryFundAccordionSection,
                "How instances are calculated (opens in new tab)");
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_NMSS_AdjustedDiscretionaryBursaryFund_InLimit()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateToNMSSPageViaLogin(ProviderUserNameInLimit, ProviderUserPassword, "discretionary - bursary");

            // Act
            ProviderFundingBreakdownPage.ClickOn1619DiscretionaryBursaryFund();

            // Assert
            var actual = ProviderFundingBreakdownPage.AdjustedDiscretionaryBursaryFundTableCell.Text;
            actual.Should().Be(DiscretionaryBursaryFundInLimitMessage);
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_NMSS_AdjustedDiscretionaryBursaryFund_AboveUpperLimit()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateToNMSSPageViaLogin(ProviderUserNameAdjustedAboveUpper, ProviderUserPassword, "discretionary - bursary");

            // Act
            ProviderFundingBreakdownPage.ClickOn1619DiscretionaryBursaryFund();

            // Assert
            var actual = ProviderFundingBreakdownPage.AdjustedDiscretionaryBursaryFundTableCell.Text;
            actual.Should().Be(DiscretionaryBursaryFundOutsudeOfLimitMessage);
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_NMSS_AdjustedDiscretionaryBursaryFund_BelowLowerLimit()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateToNMSSPageViaLogin(ProviderUserNameAdjustedBelowLower, ProviderUserPassword, "discretionary - bursary");

            // Act
            ProviderFundingBreakdownPage.ClickOn1619DiscretionaryBursaryFund();

            // Assert
            var actual = ProviderFundingBreakdownPage.AdjustedDiscretionaryBursaryFundTableCell.Text;
            actual.Should().Be(DiscretionaryBursaryFundOutsudeOfLimitMessage);
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_NMSS_AdjustedDiscretionaryBursaryFund_Below500()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateToNMSSPageViaLogin(ProviderUserNameAdjustedBelow500, ProviderUserPassword, "discretionary - bursary");

            // Act
            ProviderFundingBreakdownPage.ClickOn1619DiscretionaryBursaryFund();

            // Assert
            var actual = ProviderFundingBreakdownPage.AdjustedDiscretionaryBursaryFundTableCell.Text;
            actual.Should().Be(DiscretionaryBursaryFundBelow500Message);
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_NMSS_DecreaseInPupilNumbersGuidance_IsShown_WhenPupilNumbersDecrease()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateToNMSSPageViaLogin(ProviderUserNameForNMSSDecreaseInStudentNumber, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnHighNeedsTab();

            // Assert
            ProviderFundingBreakdownPage.ReducedPupilNumberGuidance.Should().NotBeNull();
            var actual = ProviderFundingBreakdownPage.ReducedPupilNumberGuidance.Text;
            actual.Should().Be(DiscretionaryBursaryFundReducedPupilNumberGuidance);
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_NMSS_DecreaseInPupilNumbersGuidance_IsNotShown_WhenPupilNumbersIncrease()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateToNMSSPageViaLogin(ProviderUserNameForNMSS, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnHighNeedsTab();

            // Assert
            Assert.ThrowsException<NoSuchElementException>(() => ProviderFundingBreakdownPage.ReducedPupilNumberGuidance);
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_NMSS_DecreaseInPupilNumbersGuidance_IsNotShown_WhenPupilNumbersAreTheSame()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateToNMSSPageViaLogin(ProviderUserNameForNMSSIdenticalStudentNumbers, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnHighNeedsTab();

            // Assert
            Assert.ThrowsException<NoSuchElementException>(() => ProviderFundingBreakdownPage.ReducedPupilNumberGuidance);
        }

        #endregion


        #region Private Helpers

        private static List<(string Id, string Text, bool Click)> GetHighNeedsTextElements()
        {
            // Arrange Act
            return new List<(string Id, string Text, bool Click)>()
            {
                ("high-needs-rounding", "Rounding differences in your calculations", true),
                ("hnr-desc", "The values in this statement may not add up precisely to the totals provided due to rounding. The calculation of your funding is done using exact figures.", false)
            };
        }

        private static IEnumerable<TableData> GetHighNeedsTableData()
        {
            yield return new TableData
            {
                Id = "pfHighNeedsTable",
                TableRowData = GetHighNeedsTableRows()
            };
        }

        private static IEnumerable<TableData> GetHighNeedsCalculationOfIncreaseTableData()
        {
            yield return new TableData
            {
                Id = "pfCalculationOfIncreaseTable",
                TableRowData = GetHighNeedsCalculationOfIncreaseTableRows()
            };
        }

        private static IEnumerable<TableData> GetHighNeedsFundedPupilsTableData()
        {
            yield return new TableData
            {
                Id = "pfFundedPupilsTable",
                TableRowData = GetHighNeedsFundedPupilsTableRows()
            };
        }

        private static IEnumerable<TableData> GetHighNeedsFundedPlacesTableData()
        {
            yield return new TableData
            {
                Id = "pfFundedPlacesTable",
                TableRowData = GetHighNeedsFundedPlacesTableRows()
            };
        }

        private static IEnumerable<TableRowData> GetHighNeedsTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "High needs",
                    "Places",
                    "Rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Pre-16",
                    "42",
                    "£10,000",
                    "£420,000"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Post-16",
                    "18",
                    "£10,000",
                    "£180,000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total high needs",
                    "60",
                    string.Empty,
                    "£600,000"
                }
            };
        }

        private static IEnumerable<TableRowData> GetHighNeedsCalculationOfIncreaseTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Calculation of increase",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Funded pupils from spring {yearZero} census",
                    string.Empty,
                    "52"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Funded pupils from autumn {yearMinusOne} census",
                    "Minus",
                    "48"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Increase in funded pupils",
                    "Equals",
                    "4"
                }
            };
        }

        private static IEnumerable<TableRowData> GetHighNeedsFundedPupilsTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    $"Funded pupils {currentYear} to {nextYear}",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Funded pupils from autumn {yearZero} census",
                    string.Empty,
                    "56"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Increase (calculated above)",
                    "Plus",
                    "4"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Total funded pupils {currentYear} to {nextYear}",
                    "Equals",
                    "60"
                }
            };
        }

        private static IEnumerable<TableRowData> GetHighNeedsFundedPlacesTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    $"Funded places in {currentYear} to {nextYear} allocation",
                    "Calculation",
                    "Pre-16 sub-total",
                    "Post-16 sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Total pupils from autumn {yearZero} census",
                    string.Empty,
                    "39",
                    "17"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"All high needs pupils from autumn {yearZero} census",
                    "Divide",
                    "56",
                    "56"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Proportion of pupils",
                    "Equals",
                    "0.70",
                    "0.30"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"All funded pupils {currentYear} to {nextYear}",
                    "Multiply",
                    "60",
                    "60"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Total funded pupils {currentYear} to {nextYear}",
                    "Equals",
                    "42",
                    "18"
                }
            };
        }

        private static List<(string Id, string Text, bool Click)> GetDiscretionaryBursaryFundTextElements()
        {
            // Arrange Act
            return new List<(string Id, string Text, bool Click)>()
            {
                ("dbf-rounding", "Rounding differences in your calculations", true),
                ("dbf-rounding-desc", "The values in this statement may not add up precisely to the totals provided due to rounding. The calculation of your funding is done using exact figures.", false),
                ("dbf-transitionlimits", "More about the transition limits", true),
                ("dbf-more-about", "More about this table", true),
                ("dbf-more-about-subtotal", "The sub-total for each discretionary bursary element is calculated by multiplying the number of students by the instances per student to get the number of instances:", false),
                ("dbf-more-about-totalfunded", "Total funded students × Instances per student = Number of instances", false),
                ("dbf-more-about-noof", "The number of instances is then multiplied by the rate:", false),
                ("dbf-more-about-noofFormula", "Number of instances × Rate = Sub-total", false),
                ("dbf-more-about-subTotalsHelp", $"The sub-totals are then added together to make your discretionary bursary fund total. This figure is then used in the next calculation. It is compared with your total discretionary bursary fund from {yearMinusOne} to {yearZero}. It will be adjusted if it falls outside the limits set for this year.", false),
                ("dbf-transitionlimits-methodology", $"From {yearZero} to {currentYear}, we moved to a new methodology to calculate your discretionary bursary fund.", false),
                ("dbf-transitionlimits-goingforward", $"In {yearMinusOne} to {yearZero}, you received the amount shown in the first row of the adjusted discretionary bursary fund table. Going forward:", false),
                ("dbf-transitionlimits-capped", $"In {yearZero} to {currentYear}, you received an amount capped between +/- 25% of the {yearMinusOne} to {yearZero} total", false),
                ("dbf-transitionlimits-thisYear", $"This year, you will receive an amount capped between +/- 50% of the {yearMinusOne} to {yearZero} total", false),
                ("dbf-transitionlimits-nextyear", $"In {nextYear} to 2023, you will receive an amount capped between +/- 75% of the {yearMinusOne} to {yearZero} total", false),
                ("dbf-transitionlimits-future", "2023 to 2024 onward - you receive the amount with no cap applied", false)
            };
        }

        private static IEnumerable<TableData> GetDiscretionaryBursaryFundBreakdownTableData()
        {
            yield return new TableData
            {
                Id = "discretionaryBursaryBreakdownTable",
                TableRowData = GetDiscretionaryBursaryBreakdownTableRows()
            };
        }

        private static IEnumerable<TableData> GetAdjustedDiscretionaryBursaryFundBreakdownTableData()
        {
            yield return new TableData
            {
                Id = "adjDiscretionaryBursaryFundTable",
                TableRowData = GetAdjustedDiscretionaryBursaryTableRows()
            };
        }

        private static IEnumerable<TableData> GetDiscretionaryBursaryFundExceptionalAdjustmentTableData()
        {
            yield return new TableData
            {
                Id = "exceptionalAdjustmentTable",
                TableRowData = GetDiscretionaryBursaryFundExceptionalAdjustmentTableRows()
            };
        }

        private static IEnumerable<TableRowData> GetDiscretionaryBursaryBreakdownTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Discretionary bursary breakdown",
                    "Total funded\r\nstudents",
                    "Instances\r\nper student",
                    "Number of\r\ninstances",
                    "Rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Financial disadvantage",
                    "18",
                    "0.28",
                    "5.04",
                    "£242",
                    "£1,220"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Travel",
                    "18",
                    "0.04",
                    "0.72",
                    "£483",
                    "£350"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Industry placement",
                    string.Empty,
                    string.Empty,
                    "0",
                    "£49",
                    "£0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Discretionary bursary fund {currentYear} to {nextYear}",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "£1,570"
                }
            };
        }

        private static IEnumerable<TableRowData> GetAdjustedDiscretionaryBursaryTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Adjusted discretionary bursary fund",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"{yearMinusOne} to {yearZero} Discretionary bursary fund (baseline for transition)",
                    "£1,140"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Transition lower limit (50%)",
                    "£855"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Transition upper limit (150%)",
                    "£1,425"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Discretionary bursary fund {currentYear} to {nextYear}",
                    "£1,570"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Adjusted discretionary bursary fund",
                    "£1,425"
                }
            };
        }

        private static IEnumerable<TableRowData> GetDiscretionaryBursaryFundExceptionalAdjustmentTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Exceptional adjustment",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Exceptional adjustment",
                    "£0"
                }
            };
        }

        #endregion
    }
}