using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;
using System.Collections.Generic;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass, TestCategory("Regression"), TestCategory("CoreRegression")]
    [Ignore]
    public class IndicativeProviderFundingBreakdownPage1619Tests : LoggedInRegressionTestBase
    {
        #region Private fields

        private readonly ApplicationConfiguration _applicationConfiguration;

        private const string ProviderUserNameIndicative = "10060613 - External User 109 - Academy Trust 1619";

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        private const int YearFrom = 2021;

        private const int YearFromMinus1 = 2020;

        private const int YearTo = 2022;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IndicativeProviderFundingBreakdownPage1619Tests"/> class.
        /// </summary>
        public IndicativeProviderFundingBreakdownPage1619Tests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion


        #region Tests

        [TestMethod]
        public void IndicativeProviderFundingBreakdownPage_1619_BasicLayout()
        {
            // Arrange Act
            IndicativeProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameIndicative, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();

            // Assert
            ViewYourFundingBasePage.EnsureBreadcrumbs();
            IndicativeProviderFundingBreakdownPage.EnsureCurrentProviderPageFor1619();
            IndicativeProviderFundingBreakdownPage.EnsureAllocationHistory();
            IndicativeProviderFundingBreakdownPage.EnsureTotalAllocation16to19("£25,803,920\r\ndecrease\r\n£100,000,000");
            IndicativeProviderFundingBreakdownPage.EnsurePrintOrSaveStatement();
            IndicativeProviderFundingBreakdownPage.EnsureDocumentDownload();
        }

        [TestMethod]
        public void IndicativeProviderFundingBreakdownPage_1619_CheckAuthorise()
        {
            // Arrange Act
            IndicativeProviderFundingBreakdownPage.NavigateToPageWithoutLogin();

            // Assert
            IndicativeProviderFundingBreakdownPage.EnsureNavigatedToLogin();
        }

        [TestMethod]
        public void IndicativeProviderFundingBreakdownPage_1619_HasBreadcrumbsLinksToPages()
        {
            // Arrange Act
            IndicativeProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameIndicative, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();
            var finalBreadcrumbText = $"Indicative 16 to 19 funding {YearFrom} to {YearTo}";

            // Assert
            IndicativeProviderFundingBreakdownPage.EnsureBreadcrumbsAndFinalText(finalBreadcrumbText, new[] { "Home", "Allocation statements", "Select a previous statement to compare your figures" });
            ViewYourFundingBasePage.EnsureAndClickBreadcrumb("Allocation statements");
            ProviderPage.EnsureCurrentProviderPage();
        }

        [TestMethod]
        public void IndicativeProviderFundingBreakdownPage_1619_CheckContent()
        {
            // Arrange
            IndicativeProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameIndicative, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();

            // Assert Tables
            foreach (var tableData in GetTableData())
            {
                IndicativeProviderFundingBreakdownPage.EnsureTabTableContentData(
                    IndicativeProviderFundingBreakdownPage.Indicative1619Content,
                    tableData);
            }
        }

        #endregion


        #region Private Helpers

        private static IEnumerable<TableData> GetTableData()
        {
            yield return new TableData
            {
                Id = "summaryTable",
                TableRowData = GetSummaryTableRows()
            };

            yield return new TableData
            {
                Id = "sugTable",
                TableRowData = GetSugTableRows()
            };

            yield return new TableData
            {
                Id = "pogTable",
                TableRowData = GetPogTableRows()
            };
        }

        private static IEnumerable<TableRowData> GetSummaryTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Funding allocation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Core programme",
                    "£22,508,598"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Condition of funding deduction\r\nMore about condition of funding reduction (opens in new tab)",
                    "£169,579"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Advanced maths premium\r\nMore about advanced maths premium (opens in new tab)",
                    "£0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "High value courses premium\r\nMore about high value courses premium (opens in new tab)",
                    "£152,800"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Industry placements\r\nMore about industry placements (opens in new tab)",
                    "£218,875"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "High needs",
                    "£1,302,000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Student financial support\r\nMore about student financial support (opens in new tab)",
                    "£905,211"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Alternative completion",
                    "£34,034"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "High value courses for school and college leavers",
                    "£152,800"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Offset high value courses for school and college leavers in-year programme funding for 2021/22",
                    "£0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Start-up and post-opening grant",
                    "-£997"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Maths top up",
                    "-£997"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total funding allocation",
                    "£25,803,920"
                }
            };
        }

        private static IEnumerable<TableRowData> GetSugTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Start-up",
                    "Funding"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Part A",
                    "£0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Part B",
                    "£0"
                }
            };
        }

        private static IEnumerable<TableRowData> GetPogTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Post-opening",
                    "Funding"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Per pupil resourses",
                    "-£997"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Leadership diseconomies",
                    "-£997"
                }
            };
        }

        #endregion
    }
}