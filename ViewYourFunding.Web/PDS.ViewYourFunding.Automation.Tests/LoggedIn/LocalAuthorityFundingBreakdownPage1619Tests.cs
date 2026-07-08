using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;
using System.Collections.Generic;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    [Ignore]
    public class LocalAuthorityFundingBreakdownPage1619Tests : LoggedInRegressionTestBase
    {
        #region Private fields

        private static string TestLoginExternalUserLaSsfAndMss => "10004801 - External User 103 - LocalAuthoritySsfAndMss";

        private readonly ApplicationConfiguration _applicationConfiguration;

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalAuthorityFundingBreakdownPage1619Tests"/> class.
        /// </summary>
        public LocalAuthorityFundingBreakdownPage1619Tests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion


        #region Tests

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityFundingBreakdownPage_1619_BasicLayout()
        {
            // Arrange Act
            LocalAuthorityFundingBreakdownPage.NavigateTo1619PageViaLogin(TestLoginExternalUserLaSsfAndMss, ProviderUserPassword);

            // Assert
            ViewYourFundingBasePage.EnsureBreadcrumbs();
            LocalAuthorityFundingBreakdownPage.EnsureAllocationHistory();
            LocalAuthorityFundingBreakdownPage.Ensure1619GuidanceLinkSection();
            LocalAuthorityFundingBreakdownPage.EnsurePrintOrSaveStatement();
            LocalAuthorityFundingBreakdownPage.EnsureDocumentDownload();
            LocalAuthorityFundingBreakdownPage.EnsureRoundingText();
            LocalAuthorityFundingBreakdownPage.EnsureCurrentBreakDownPage();
            LocalAuthorityFundingBreakdownPage.EnsureTotalAllocation("£1,080,095");
            LocalAuthorityFundingBreakdownPage.EnsureBreakdownTabs();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityFundingBreakdownPage_1619_SplitYearTab()
        {
            // Arrange Act
            LocalAuthorityFundingBreakdownPage.NavigateTo1619PageViaLogin(TestLoginExternalUserLaSsfAndMss, ProviderUserPassword);

            // Act
            LocalAuthorityFundingBreakdownPage.ClickOnSplitYearsTab();

            // Assert Tab Total
            LocalAuthorityFundingBreakdownPage.EnsureTabTotalAllocation(
                LocalAuthorityFundingBreakdownPage.SplitYearsTabContent,
                "Total funding split across financial years",
                "£1,080,095");

            // Assert Tables
            foreach (var tableData in GetSplitYearsTabTableData())
            {
                LocalAuthorityFundingBreakdownPage.EnsureTabTableContentData(
                    LocalAuthorityFundingBreakdownPage.SplitYearsTabContent,
                    tableData);
            }

            // Assert Bottom Allocation total
            LocalAuthorityFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                LocalAuthorityFundingBreakdownPage.SplitYearsTabContent,
                "Total funding split across financial years",
                "£1,090,880");
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityFundingBreakdownPage_1619_AcademicYearTab()
        {
            // Arrange Act
            LocalAuthorityFundingBreakdownPage.NavigateTo1619PageViaLogin(TestLoginExternalUserLaSsfAndMss, ProviderUserPassword);

            // Assert Tab Total
            LocalAuthorityFundingBreakdownPage.EnsureTabTotalAllocation(
                LocalAuthorityFundingBreakdownPage.AcademicYearTabContent,
                "local authority academic year funding",
                "£1,080,095");

            // Assert Tables
            foreach (var tableData in GetAcademicYearTabTableData())
            {
                LocalAuthorityFundingBreakdownPage.EnsureTabTableContentData(
                    LocalAuthorityFundingBreakdownPage.AcademicYearTabContent,
                    tableData);
            }

            LocalAuthorityFundingBreakdownPage.EnsureSimpleDisclosure(
                LocalAuthorityFundingBreakdownPage.AcademicYearTabContent,
                "More about maintained special schools funding",
                "This funding is aggregated and paid as a lump sum. Local Authorities should distribute it across all maintained special schools, according to need.");


            // Assert Bottom Allocation total
            LocalAuthorityFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                LocalAuthorityFundingBreakdownPage.AcademicYearTabContent,
                "Total local authority academic year funding",
                "£1,090,880");
        }

        #endregion


        #region Private Helpers

        private static IEnumerable<TableData> GetAcademicYearTabTableData()
        {
            yield return new TableData
            {
                Id = "local-authority-funding",
                TableRowData = GetLocalAuthorityFundingRows()
            };

            yield return new TableData
            {
                Id = "mainstream-schools",
                TableRowData = GetMainStreamSchoolsRows()
            };

            yield return new TableData
            {
                Id = "maintained-special-schools-breakdown",
                TableRowData = GetMaintainedSpecialSchoolsRows()
            };
        }

        private static IEnumerable<TableData> GetSplitYearsTabTableData()
        {
            yield return new TableData
            {
                Id = "local-authority-funding-two",
                TableRowData = GetLocalAuthorityFundingSplitYearsRows()
            };

            yield return new TableData
            {
                Id = "mainstream-schools-two",
                TableRowData = GetMainStreamSchoolsSplitYearsRows()
            };

            yield return new TableData
            {
                Id = "maintained-special-schools-breakdown-two",
                TableRowData = GetMaintainedSpecialSchoolsSplitYearRows()
            };
        }

        private static IEnumerable<TableRowData> GetLocalAuthorityFundingRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Local authority funding",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total mainstream schools funding",
                    "£1,080,095"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Maintained special schools funding - including student financial support",
                    "£10,785"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total local authority academic year funding",
                    "£1,090,880"
                }
            };
        }

        private static IEnumerable<TableRowData> GetLocalAuthorityFundingSplitYearsRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Local authority funding",
                    "8 months in 2021 to 2022",
                    "4 months in 2022 to 2023"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total mainstream schools funding",
                    "£1,111",
                    "£1,113"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Maintained special schools funding - including student financial support",
                    "£400",
                    "£500"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total local authority funding",
                    "£1,511",
                    "£1,613"
                }
            };
        }

        private static IEnumerable<TableRowData> GetMainStreamSchoolsRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Mainstream schools",
                    "Core programme",
                    "Student financial support",
                    "Industry placements",
                    "Advanced maths premium",
                    "High value courses",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Abbey Hill Academy",
                    "£508,152",
                    "£7,724",
                    "£0",
                    "£0",
                    "£0",
                    "£967,724"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Aylward Academy",
                    "£1,375,699",
                    "£41,327",
                    "£0",
                    "£4,200",
                    "£9,200",
                    "£1,430,427"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Barton Peveril College",
                    "£15,562,634",
                    "£221,954",
                    "£70,250",
                    "£115,800",
                    "£321,600",
                    "£16,695,943"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Brentside High School",
                    "£1,153,932",
                    "£26,242",
                    "£0",
                    "£0",
                    "£24,800",
                    "£1,204,975"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Derwen College",
                    "£508,152",
                    "£13,943",
                    "£0",
                    "£0",
                    "£0",
                    "£1,080,095"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "St Paul's Catholic College",
                    "£1,044,412",
                    "£12,268",
                    "£0",
                    "£0",
                    "£27,200",
                    "£1,104,824"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "The Stourport High School and Sixth Form College",
                    "£525,174",
                    "£8,063",
                    "£0",
                    "£0",
                    "£12,800",
                    "£601,016"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Truro and Penwith College",
                    "£22,508,598",
                    "£905,211",
                    "£218,875",
                    "£0",
                    "£152,800",
                    "£25,803,920"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Total mainstream schools funding",
                    "£508,152",
                    "£13,943",
                    "£0",
                    "£0",
                    "£0",
                    "£1,080,095"
                }
            };
        }

        private static IEnumerable<TableRowData> GetMainStreamSchoolsSplitYearsRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Mainstream schools",
                    "8 months in 2021 to 2022",
                    "4 months in 2022 to 2023"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Abbey Hill Academy",
                    "£2",
                    "£4"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Aylward Academy",
                    "£2",
                    "£4"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Barton Peveril College",
                    "£2",
                    "£4"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Brentside High School",
                    "£2",
                    "£4"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Derwen College",
                    "£2",
                    "£4"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "St Paul's Catholic College",
                    "£2",
                    "£4"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "The Stourport High School and Sixth Form College",
                    "£2",
                    "£4"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Truro and Penwith College",
                    "£2",
                    "£4"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Mainstream schools funding",
                    "£1,111",
                    "£1,113"
                }
            };
        }

        private static IEnumerable<TableRowData> GetMaintainedSpecialSchoolsRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Maintained special schools funding",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Maintained special schools funding - including student financial support",
                    "£10,785"
                }
            };
        }

        private static IEnumerable<TableRowData> GetMaintainedSpecialSchoolsSplitYearRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Maintained special schools funding",
                    "8 months in 2021 to 2022",
                    "4 months in 2022 to 2023"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Maintained special schools funding - including student financial support",
                    "£400",
                    "£500"
                }
            };
        }

        #endregion
    }
}