using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;
using System.Collections.Generic;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    [Ignore]
    public class ProviderFundingBreakdownPage1416Tests : LoggedInRegressionTestBase
    {
        #region Private fields

        private readonly ApplicationConfiguration _applicationConfiguration;

        private string ProviderUserName_Primary => _applicationConfiguration.TestLoginExternalUsernamePrimary;

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        private const int YearFrom = 2021;

        private const int YearFromMinus1 = 2020;

        private const int YearTo = 2022;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderFundingBreakdownPage1416Tests"/> class.
        /// </summary>
        public ProviderFundingBreakdownPage1416Tests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion


        #region Tests

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_1416_BasicLayout()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1416PageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithNoComparison();

            // Assert
            ViewYourFundingBasePage.EnsureBreadcrumbs();
            ProviderFundingBreakdownPage.EnsureAllocationHistory();
            ProviderFundingBreakdownPage.Ensure1416GuidanceLinkSection();
            ProviderFundingBreakdownPage.EnsurePrintOrSaveStatement();
            ProviderFundingBreakdownPage.Ensure1416BreakdownTabs();
            ProviderFundingBreakdownPage.EnsureTotalAllocation("£360,710");
            ProviderFundingBreakdownPage.Ensure1416DocumentDownload();
            ProviderFundingBreakdownPage.EnsureMandatoryH3Headers();
            ProviderFundingBreakdownPage.EnsureProviderNameForPrint1416IsNotDisplayed();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_1416_CheckAuthorise()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1416PageWithoutLogin();

            // Assert
            ProviderFundingBreakdownPage.EnsureNavigatedToLogin();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_1416_HasBreadcrumbsLinksToPages()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1416PageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithNoComparison();
            var finalBreadcrumbText = $"14 to 16 funding {YearFrom} to {YearTo}";

            // Assert
            ProviderFundingBreakdownPage.EnsureBreadcrumbsAndFinalText(finalBreadcrumbText, new[] { "Home", "Allocation statements", "Select a previous statement to compare your figures" });
            ViewYourFundingBasePage.EnsureAndClickBreadcrumb("Allocation statements");
            ProviderPage.EnsureCurrentProviderPage();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_1416_CoreProgramme_EnsureContent()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateTo1416PageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithNoComparison();

            // Act
            ProviderFundingBreakdownPage.ClickOnCoreProgrammeTab();

            // Assert Tab Total
            ProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.CoreProgrammeTabContent,
                "Total core programme",
                "£342,720");

            // Assert table data
            foreach (var tableData in GetCoreProgrammeTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.CoreProgrammeTabContent,
                    tableData);
            }

            // Assert funding line sub-totals
            ProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                ProviderFundingBreakdownPage.CoreProgrammeTabContent,
                "Total core programme",
                "£342,720");

            ProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                ProviderFundingBreakdownPage.CoreProgrammeTabContent,
                "Total retention adjustment",
                "£0");

            ProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                ProviderFundingBreakdownPage.CoreProgrammeTabContent,
                "Total disadvantage funding",
                "£55,680");

            // Assert Links
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.CoreProgrammeTabContent,
                "More about proportion of students (opens in new tab)");

            // Assert Bottom Allocation total
            ProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                ProviderFundingBreakdownPage.CoreProgrammeTabContent,
                "Total additional funding",
                "£342,720");
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_1416_PupilPremium_EnsureContent()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateTo1416PageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithNoComparison();

            // Act
            ProviderFundingBreakdownPage.ClickOnPupilPremiumTab();

            // Assert Tab Total
            ProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.PupilPremiumContent,
                "Total additional funding",
                "£17,990");

            // Assert table data
            foreach (var tableData in GetPupilPremiumTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.PupilPremiumContent,
                    tableData);
            }

            // Assert Links
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.PupilPremiumContent,
                "More about free meals (opens in new tab)");

            // Assert Links
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.PupilPremiumContent,
                "More about service pupil premium (opens in new tab)");
        }

        #endregion


        #region Private Helpers

        private static IEnumerable<TableData> GetCoreProgrammeTableData()
        {
            yield return new TableData
            {
                Id = "total-core-programme-breakdown",
                TableRowData = GetTotalCoreProgrammeBreakdownRows()
            };

            yield return new TableData
            {
                Id = "total-student-funding-breakdown",
                TableRowData = GetTotalStudentFundingRows()
            };

            yield return new TableData
            {
                Id = "programme-cost-weighting-breakdown",
                TableRowData = GetTotalCostWeightingBreakdownRows()
            };

            yield return new TableData
            {
                Id = "total-deprivation-breakdown",
                TableRowData = GetTotalDeprivationBreakdownRows()
            };

            yield return new TableData
            {
                Id = "total-low-prior-attainment-breakdown",
                TableRowData = GetTotalLowPriorAttainmentBreakdownRows()
            };
        }

        private static IEnumerable<TableRowData> GetTotalCoreProgrammeBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Core programme",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Student funding",
                    string.Empty,
                    "£276,000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Retention factor",
                    "Plus",
                    "£0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Programme cost weighting",
                    "Plus",
                    "£11,040"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Disadvantage funding",
                    "Plus",
                    "£55,680"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Area cost allowance",
                    "Multiply",
                    "1"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total core programme",
                    "Equals",
                    "£342,720"
                }
            };
        }

        private static IEnumerable<TableRowData> GetTotalStudentFundingRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Student funding",
                    "14 to 16 students",
                    "National rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Student funding",
                    "69",
                    "£4,000",
                    "£276,000"
                }
            };
        }

        private static IEnumerable<TableRowData> GetTotalCostWeightingBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Programme cost weighting uplift",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Student funding after retention adjustment",
                    string.Empty,
                    "£276,000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Programme cost weighting for subjects that cost more to deliver",
                    "Multiply",
                    "1.04"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Core programme up to programme cost weighting",
                    "Equals",
                    "£287,040"
                }
            };
        }

        private static IEnumerable<TableRowData> GetTotalDeprivationBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Deprivation (block 1)",
                    "Proportion of students",
                    "Student funding and programme cost uplift",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Deprivation",
                    "0.07481",
                    "£287,040",
                    "£21,461"
                }
            };
        }

        private static IEnumerable<TableRowData> GetTotalLowPriorAttainmentBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Low prior attainment (block 2)",
                    "14 to 16 students",
                    "Proportion of students",
                    "Rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Low prior attainment in maths or English",
                    "69",
                    "0.51661",
                    "£960",
                    "£34,219"
                }
            };
        }

        private static IEnumerable<TableData> GetPupilPremiumTableData()
        {
            yield return new TableData
            {
                Id = "pupil-premium-breakdown",
                TableRowData = GetPupilPremiumBreakdownRows()
            };

            yield return new TableData
            {
                Id = "service-pupil-premium-breakdown",
                TableRowData = GetServicePupilPremiumBreakdownRows()
            };

            yield return new TableData
            {
                Id = "total-pupil-premium-breakdown",
                TableRowData = GetTotalPupilPremiumBreakdownRows()
            };
        }

        private static IEnumerable<TableRowData> GetPupilPremiumBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Pupil premium",
                    "Eligible pupils",
                    "Rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Free meals",
                    "14",
                    "£935",
                    "£13,090"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Pupils in or recently left care",
                    "2",
                    "£2,300",
                    "£4,600"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total pupil premium",
                    "16",
                    string.Empty,
                    "£17,690"
                }
            };
        }

        private static IEnumerable<TableRowData> GetServicePupilPremiumBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Service pupil premium",
                    "Eligible pupils",
                    "Rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Children of service personnel",
                    "1",
                    "£300",
                    "£300"
                }
            };
        }

        private static IEnumerable<TableRowData> GetTotalPupilPremiumBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Total pupil premium",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Pupil premium",
                    string.Empty,
                    "£17,690"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Service pupil premium",
                    "Plus",
                    "£300"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total",
                    "Equals",
                    "£17,990"
                }
            };
        }

        #endregion
    }
}