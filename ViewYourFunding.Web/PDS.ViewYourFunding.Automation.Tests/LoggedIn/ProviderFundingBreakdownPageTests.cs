using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;
using System;
using System.Collections.Generic;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    [Ignore]
    public class ProviderFundingBreakdownPageTests : LoggedInRegressionTestBase
    {
        #region Private fields

        private readonly ApplicationConfiguration _applicationConfiguration;

        private string ProviderUserName_ExistingSpecialSchool => _applicationConfiguration.TestLoginExternalUserExistingSpecialSchool;

        private string ProviderUserName_Primary => _applicationConfiguration.TestLoginExternalUsernamePrimary;

        private string ProviderUserName_PrimaryMainStream => _applicationConfiguration.TestLoginExternalUsernamePrimaryMainStream;

        private string ProviderUserName_PrimaryInYear => _applicationConfiguration.TestLoginExternalUsernamePrimaryInYear;

        private string ProviderUserName_PrimaryInYear_ZeroStartUpFunding => _applicationConfiguration.TestLoginExternalUsernamePrimaryInYearZeroStartUpFunding;

        private string ProviderUserName_Secondary => _applicationConfiguration.TestLoginExternalUsernameSecondary;

        private string ProviderUserName_SecondaryInYear => _applicationConfiguration.TestLoginExternalUsernameSecondaryInYear;

        private string ProviderUserName_SecondaryInYear_ZeroStartUpFunding => _applicationConfiguration.TestLoginExternalUsernameSecondaryInYearZeroStartUpFunding;

        private string ProviderUserName_AllThrough => _applicationConfiguration.TestLoginExternalUsernameAllThrough;

        private string ProviderUserName_AllThroughInYear => _applicationConfiguration.TestLoginExternalUsernameAllThroughInYear;

        private string ProviderUserName_AllThroughInYear_ZeroStartUpFunding => _applicationConfiguration.TestLoginExternalUsernameAllThroughInYearZeroStartUpFunding;

        private string ProviderUserName_InyearOpenerFreeSchool => _applicationConfiguration.TestLoginExternalMainstreamInyearOpenerFreeSchool;

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        private const int YearFrom = 2023;

        private const int YearFromMinus1 = 2022;

        private const int YearTo = 2024;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderFundingBreakdownPageTests"/> class.
        /// </summary>
        public ProviderFundingBreakdownPageTests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion


        #region Tests

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_BasicLayout_Primary()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithPreviousYear();

            // Assert
            ProviderFundingBreakdownPage.EnsureBreakdownTabs();
            ViewYourFundingBasePage.EnsureBreadcrumbs();
            ProviderFundingBreakdownPage.EnsureCurrentProviderPage();
            ProviderFundingBreakdownPage.EnsureAllocationHistory();
            ProviderFundingBreakdownPage.EnsureGuidanceLinkSection();
            ProviderFundingBreakdownPage.EnsureTotalAllocation("£436,228.22");
            ProviderFundingBreakdownPage.EnsurePrintOrSaveStatement();
            ProviderFundingBreakdownPage.EnsureDocumentDownload();
            ProviderFundingBreakdownPage.EnsureGuidanceLinkSection();
            ProviderFundingBreakdownPage.EnsureMandatoryH3Headers();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_CheckAuthorise()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateToPageWithoutLogin();

            // Assert
            ProviderFundingBreakdownPage.EnsureNavigatedToLogin();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_HasBreadcrumbsLinksToPages()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithPreviousYear();
            var finalBreadcrumbText = $"General annual grant {YearFrom} to {YearTo}";

            // Assert
            ProviderFundingBreakdownPage.EnsureBreadcrumbsAndFinalText(finalBreadcrumbText, new[] { "Home", "Allocation statements", "Select a previous statement to compare your figures" });
            ViewYourFundingBasePage.EnsureAndClickBreadcrumb("Allocation statements");
            ProviderPage.EnsureCurrentProviderPage();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_MinimumFundingGuaranteeTab_Test_Primary()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithPreviousYear();

            // Act
            ProviderFundingBreakdownPage.ClickOnMinimumFundingGuaranteeTab();

            // Assert Tab Total
            ProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                "Total minimum funding guarantee",
                "£0");

            // Assert Tables
            foreach (var tableData in GetMinimumFundingGuaranteeTableData(true, false))
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                    tableData);
            }

            // Assert Links
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                "More about adjusted school budget (opens in new tab)");
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                "capping and scaling back adjustment (opens in new tab)");
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                "Protection funding (opens in new tab)");

            // Assert Small Headers
            ProviderFundingBreakdownPage.EnsureSmallHeading(
                ProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                $"{YearFrom} to {YearTo}");
            ProviderFundingBreakdownPage.EnsureSmallHeading(
                ProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                $"{YearFromMinus1} to {YearFrom}");

            // Assert Paragraph content
            ProviderFundingBreakdownPage.EnsureParagraphContent(
                ProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                $"Protection funding (opens in new tab) to make sure change in per pupil school budget share between {YearFromMinus1} to {YearFrom} and {YearFrom} to {YearTo} will not fall below the local authority guaranteed change of 150%.");

            // Assert Calculation Text and response content
            ProviderFundingBreakdownPage.EnsureTabStatementCalculationTextAndResponse(
                ProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                "Does the local authority apply capping and scaling back to your funding gains to make the funding formula affordable?",
                "No");

            var mfgTabLocalAuthorityApplyCappingAndScalingResponse = ProviderFundingBreakdownPage.MFGTabLocalAuthorityApplyCappingAndScaling.Text;

            if (mfgTabLocalAuthorityApplyCappingAndScalingResponse.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
            {
                var schoolHasYearGroupsWithoutPupils = ProviderFundingBreakdownPage.MFGTabSchoolHasYearGroupsWithoutPupils.Text;

                if (schoolHasYearGroupsWithoutPupils.Equals("No", StringComparison.InvariantCultureIgnoreCase))
                {
                    //Assert Tables
                    foreach (var tableData in GetMFGCappingAndScalingBackByLocalAuthorityTables())
                    {
                        ProviderFundingBreakdownPage.EnsureTabTableContentData(
                            ProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                            tableData);
                    }
                }
            }

            // Assert Bottom Allocation total
            ProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                ProviderFundingBreakdownPage.MinimumFundingGuaranteeTabContent,
                "Total minimum funding guarantee",
                "£0");
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_HighNeedsTab_Test()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithPreviousYear();

            // Act
            ProviderFundingBreakdownPage.ClickOnHighNeedsTab();

            // Assert Tab Total
            ProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                "Total high needs",
                "£11,306");

            // Assert Tables
            var highNeedsTableData = GetHighNeedsTableData();

            ProviderFundingBreakdownPage.EnsureTabTableContentData(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                new TableData
                {
                    Id = "pre16highNeedsTable",
                    TableRowData = highNeedsTableData
                });

            ProviderFundingBreakdownPage.EnsureSimpleDisclosure(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                "Where places come from",
                $"Places are either rolled forward from {YearFromMinus1} to {YearFrom} or your local authority agreed it with ESFA. This provides a base level of funding.\r\nYou must agree additional funding above this level with your local authority. Local authorities pay you the top-up funding directly. It is not shown in this statement.");


            // Assert Links
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                "Difference between occupied and unoccupied places (opens in new tab)");

            // Assert Bottom Allocation total
            ProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                "Total high needs",
                "£11,306");
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_SpecialSchool_HighNeedsTab_Test()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLoginAndHighNeeds(ProviderUserName_ExistingSpecialSchool, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();

            // Act
            ProviderFundingBreakdownPage.ClickOnHighNeedsTab();

            // Assert Tab Total
            ProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                "Total high needs",
                "£6,000");

            // Assert Tables
            foreach (var tableData in GetSpecialSchoolHighNeedsTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.HighNeedsTabContent,
                    tableData);
            }

            ProviderFundingBreakdownPage.EnsureSimpleDisclosure(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                "Where pre-16 high needs places come from",
                $"Places are either rolled forward from {YearFromMinus1} to {YearFrom} or your local authority agreed it with ESFA. This provides a base level of funding.\r\nYou must agree additional funding above this level with your local authority. Local authorities pay you the top-up funding directly. It is not shown in this statement.");

            // Assert Links
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                "More about pre-16 high needs (opens in new tab)");

            // Assert Bottom Allocation total
            ProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                ProviderFundingBreakdownPage.HighNeedsTabContent,
                "Total high needs",
                "£6,000");
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_StartUpGrantTab_Test_InYear_Primary()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_PrimaryInYear, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnStartUpGrantTab();

            // Assert Tab Total
            ProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                "Total start-up grant for new sponsored academy",
                "£25,000");

            // Assert Tables
            var startUpGrantTable = GetStartUpGrantTableData(true, false, false);

            ProviderFundingBreakdownPage.EnsureTabTableContentData(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                new TableData
                {
                    Id = "startUpGrantTable",
                    TableRowData = startUpGrantTable
                });

            // Assert Links
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                "More about startup-grant (opens in new tab)");

            // Assert Bottom Allocation total
            ProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                "Total start-up grant for new sponsored academy",
                "£25,000");
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_StartUpGrantTab_Test_InYear_Secondary()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_SecondaryInYear, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnStartUpGrantTab();

            // Assert Tab Total
            ProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                "Total start-up grant for new sponsored academy",
                "£25,000");

            // Assert Tables
            var startUpGrantTable = GetStartUpGrantTableData(false, true, false);

            ProviderFundingBreakdownPage.EnsureTabTableContentData(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                new TableData
                {
                    Id = "startUpGrantTable",
                    TableRowData = startUpGrantTable
                });

            // Assert Links
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                "More about startup-grant (opens in new tab)");

            // Assert Bottom Allocation total
            ProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                "Total start-up grant for new sponsored academy",
                "£25,000");
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_StartUpGrantTab_Test_InYear_AllThrough()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_AllThroughInYear, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnStartUpGrantTab();

            // Assert Tab Total
            ProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                "Total start-up grant for new sponsored academy",
                "£25,000");

            // Assert Tables
            var startUpGrantTable = GetStartUpGrantTableData(false, false, true);

            ProviderFundingBreakdownPage.EnsureTabTableContentData(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                new TableData
                {
                    Id = "startUpGrantTable",
                    TableRowData = startUpGrantTable
                });

            // Assert Links
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                "More about startup-grant (opens in new tab)");

            // Assert Bottom Allocation total
            ProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                ProviderFundingBreakdownPage.StartUpGrantTabContent,
                "Total start-up grant for new sponsored academy",
                "£25,000");
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_StartUpGrantTab_Test_InYearZeroStartUpFunding_Primary()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_PrimaryInYear_ZeroStartUpFunding, ProviderUserPassword);

            // Assert
            ProviderFundingBreakdownPage.EnsureStartUpGrantTabNotDisplayed();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_StartUpGrantTab_Test_InYearZeroStartupFunding_Secondary()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_SecondaryInYear_ZeroStartUpFunding, ProviderUserPassword);

            // Assert
            ProviderFundingBreakdownPage.EnsureStartUpGrantTabNotDisplayed();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_StartUpGrantTab_Test_InYearZeroStartUpFunding_AllThrough()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_AllThroughInYear_ZeroStartUpFunding, ProviderUserPassword);

            // Assert
            ProviderFundingBreakdownPage.EnsureStartUpGrantTabNotDisplayed();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_IYO_SchoolBudgetShare_SplitSites()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_AllThroughInYear_ZeroStartUpFunding, ProviderUserPassword);

            // Assert
            ProviderFundingBreakdownPage.ClickOnSchoolShareBudgetTab();

            // Assert Tables
            foreach (var tableData in GetSplitSitesTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                    tableData);
            }
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_ProtectionFundingTab_Test()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_InyearOpenerFreeSchool, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnProtectionFundingTab();

            // Assert Tab Total
            ProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.ProtectionFundingTabContent,
                "Total protection funding",
                "£25,001");

            // Assert Tables
            var laAvgPerPupilTableData = GetLocalAuthorityAvgPerPupilTableData();

            ProviderFundingBreakdownPage.EnsureTabTableContentData(
                ProviderFundingBreakdownPage.ProtectionFundingTabContent,
                new TableData
                {
                    Id = "pfProtectionFundingTable",
                    TableRowData = laAvgPerPupilTableData
                });

            int.TryParse(ProviderFundingBreakdownPage.ProtectionFundingTabAveragePupilRate.Text, out int avgPerPupilRate);

            var diffPerpupilTableData = GetDifferencePerPupilTableData();

            ProviderFundingBreakdownPage.EnsureTabTableContentData(
                ProviderFundingBreakdownPage.ProtectionFundingTabContent,
                new TableData
                {
                    Id = "pfDifferencePerPupilTable",
                    TableRowData = diffPerpupilTableData
                });

            var totalPfForFullYearTableData = TotalProtectionFundingForFullYearTableData();

            ProviderFundingBreakdownPage.EnsureTabTableContentData(
                ProviderFundingBreakdownPage.ProtectionFundingTabContent,
                new TableData
                {
                    Id = "pfTotalYearFundingTable",
                    TableRowData = totalPfForFullYearTableData
                });

            var totalPfForDaysOpenTableData = TotalProtectionFundingForDaysOpenTableData();

            ProviderFundingBreakdownPage.EnsureTabTableContentData(
                ProviderFundingBreakdownPage.ProtectionFundingTabContent,
                new TableData
                {
                    Id = "pfTotalDaysOpenFundingTable",
                    TableRowData = totalPfForDaysOpenTableData
                });

            // Assert Bottom Allocation total
            ProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                ProviderFundingBreakdownPage.ProtectionFundingTabContent,
                "Total protection funding",
                "£25,001");
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_PostOpeningGrantTab_Test()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_InyearOpenerFreeSchool, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnPostOpeningGrantTab();

            // Assert
            if (ProviderFundingBreakdownPage.PostOpeningGrantTabTotal.Text == "£0")
            {
                ProviderFundingBreakdownPage.EnsureTabLink(
              ProviderFundingBreakdownPage.PostOpeningGrantTabContent,
              "post-opening grant (opens in new tab).");

                ProviderFundingBreakdownPage.EnsureTabTotalAllocation(
             ProviderFundingBreakdownPage.PostOpeningGrantTabContent,
             "Total post-opening grant",
             "£0");
            }
            else
            {
                ProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.PostOpeningGrantTabContent,
                "Total post-opening grant",
                "£25,000");

                var getPostOpeningGrantTableData = GetPostOpeningGrantTableData();

                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.PostOpeningGrantTabContent,
                    new TableData
                    {
                        Id = "postOpeningGrantTable",
                        TableRowData = getPostOpeningGrantTableData
                    });

                ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.PostOpeningGrantTabContent,
                "More about post-opening grant (opens in new tab)");

                // Assert Bottom Allocation total
                ProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                    ProviderFundingBreakdownPage.PostOpeningGrantTabContent,
                    "Total post-opening grant",
                    "£25,000");
            }
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_SchoolBudgetShare_Test_Existing_Primary()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithPreviousYear();
            var expectedTotal = "£436,228.22";

            // Act
            ProviderFundingBreakdownPage.ClickOnSchoolShareBudgetTab();

            // Assert Tab Total
            ProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total school budget share",
                expectedTotal);

            // Assert Tables
            foreach (var tableData in GetSchoolShareBudgetTabTableData(true, false))
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                    tableData);
            }

            // Assert Links
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Where total pupil numbers come from (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "What Income Deprivation Affecting Children Index (IDACI) bands are (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Where free school meals weightings come from (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about mobility (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about lump sum (opens in new tab)");

            // Assert funding line Allocation totals
            ProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total pupil-led factors",
                "£318,552.95");

            ProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total other factors",
                "£115,023");

            ProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total school budget share",
                expectedTotal);
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_SchoolBudgetShare_Test_Existing_Secondary()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_Secondary, ProviderUserPassword);
            var total = "£2,740,800";

            // Act
            ProviderFundingBreakdownPage.ClickOnSchoolShareBudgetTab();

            // Assert Tab Total
            ProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total school budget share",
                total);

            // Assert Tables
            foreach (var tableData in GetSchoolShareBudgetTabTableData(false, true))
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                    tableData);
            }

            // Assert Links
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Where total pupil numbers come from (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "What Income Deprivation Affecting Children Index (IDACI) bands are (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Where free school meals weightings come from (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about low prior attainment (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about lump sum (opens in new tab)");

            // Disclosure check.
            ProviderFundingBreakdownPage.EligibleForSparsity.Click();
            var eligibleForSparsityTable = new TableData
            {
                Id = "eligibleForSparsityDataTable",
                TableRowData = EligibleForSparsityDataTable()
            };
            ProviderFundingBreakdownPage.EnsureTabTableContentData(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                eligibleForSparsityTable);

            // Assert funding line Allocation totals
            ProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total pupil-led factors",
                "£2,541,268.73");

            ProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total other factors",
                "£115,023");

            ProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total school budget share",
                total);
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFundingBreakdownPage_GAG_SchoolBudgetShare_Test_Existing_AllThrough()
        {
            // Arrange
            ProviderFundingBreakdownPage.NavigateToPageViaLogin(ProviderUserName_AllThrough, ProviderUserPassword);
            var total = "£5,798,928";

            // Act
            ProviderFundingBreakdownPage.ClickOnSchoolShareBudgetTab();

            // Assert Tab Total
            ProviderFundingBreakdownPage.EnsureTabTotalAllocation(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total school budget share",
                total);

            // Assert Tables
            foreach (var tableData in GetSchoolShareBudgetTabTableData(true, true))
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                    tableData);
            }

            // Assert Links
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Where total pupil numbers come from (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "What Income Deprivation Affecting Children Index (IDACI) bands are (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Where free school meals weightings come from (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about low prior attainment (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about mobility (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "More about lump sum (opens in new tab)");

            // Assert funding line Allocation totals
            ProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total pupil-led factors",
                "£5,485,782.66");

            ProviderFundingBreakdownPage.EnsureMidTableLineAllocationTotal(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total other factors",
                "£260,658.15");

            ProviderFundingBreakdownPage.EnsureTabBottomAllocationTotal(
                ProviderFundingBreakdownPage.SchoolBudgetShareTabContent,
                "Total school budget share",
                total);
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderFudingBreakdownPage_1619_BasicLayout()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithNoComparison();

            // Assert
            ViewYourFundingBasePage.EnsureBreadcrumbs();
            ProviderFundingBreakdownPage.EnsureAllocationHistory();
            ProviderFundingBreakdownPage.Ensure1619GuidanceLinkSection();
            ProviderFundingBreakdownPage.EnsurePrintOrSaveStatement();
            ProviderFundingBreakdownPage.Ensure1619AccordionSections();
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
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Alternative provision",
                    "£10,000",
                    "0",
                    "£0"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Special occupied",
                    "£6,000",
                    "0",
                    "£0"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Special unoccupied",
                    "£10,000",
                    "0",
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
                    "£0"
                }
            };
        }

        private static IEnumerable<TableRowData> GetLocalAuthorityAvgPerPupilTableData()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Change in local authority average per pupil rate",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"{YearFrom} to {YearTo}",
                    string.Empty,
                    "£2,635,991.17"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"{YearFromMinus1} to {YearFrom}",
                    "Minus",
                    "£0"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"{YearFromMinus1} to {YearFrom}",
                    "Divide",
                    "£0"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Change in local authority average per pupil rate",
                    "Equals",
                    "-200%"
                }
            };
        }

        private static IEnumerable<TableRowData> GetPostOpeningGrantTableData()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Post-opening grant",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Per pupil resources",
                    "£25,000"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Leadership or other costs to build up pupil numbers",
                    "£0"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total post-opening grant",
                    "£25,000"
                }
            };
        }

        private static IEnumerable<TableRowData> GetDifferencePerPupilTableData()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Difference Per Pupil",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Local authority average per pupil rate {YearFrom} to {YearTo}",
                    string.Empty,
                    "£2,635,991.17"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Change in average per pupil rate is lower than local authority guaranteed change by",
                    "Multiply",
                    "-200%"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Difference per pupil",
                    "Equals",
                    "£0"
                }
            };
        }

        private static IEnumerable<TableRowData> TotalProtectionFundingForFullYearTableData()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Total protection funding for full year",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Pupils {YearFrom} to {YearTo}",
                    string.Empty,
                    "571"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Difference per pupil",
                    "Multiply",
                    "£0"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total protection funding for full year",
                    "Equals",
                    "£0"
                }
            };
        }

        private static IEnumerable<TableRowData> TotalProtectionFundingForDaysOpenTableData()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Total protection funding for days open",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total protection funding for full year",
                    string.Empty,
                    "£0"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Days open",
                    "Multiply",
                    "153"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Days in the year",
                    "Divide",
                    "365"
                }
            };
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total protection funding for days open",
                    "Equals",
                    "£25,001"
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
                    "£25,000"
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

            if (!showPrimary)
            {
                yield return new TableData
                {
                    Id = "sparsityTable",
                    TableRowData = GetSparsityTableData()
                };
            }

            yield return new TableData
            {
                Id = "lumpSumTable",
                TableRowData = GetLumpSumTableData(showPrimary, showSecondary)
            };

            yield return new TableData
            {
                Id = "mobilityTable",
                TableRowData = GetMobilityTableData(showPrimary, showSecondary)
            };

            yield return new TableData
            {
                Id = "englishaaalTable",
                TableRowData = GetEnglishAsAdditionalLanguageTableData(showPrimary, showSecondary)
            };

            yield return new TableData
            {
                Id = "lowPriorityTable",
                TableRowData = GetLowPriorityTableData(showPrimary, showSecondary)
            };

            yield return new TableData
            {
                Id = "minimumFundingTable",
                TableRowData = MinimumFundingDataTable(showPrimary, showSecondary)
            };
        }

        private static IEnumerable<TableData> GetSplitSitesTableData()
        {
            yield return new TableData
            {
                Id = "splitSitesTable",
                TableRowData = GetSplitSitesTable()
            };
        }

        private static IEnumerable<TableRowData> GetSplitSitesTable()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Split sites",
                    "Full year sub-total",
                    "Part year sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Split sites",
                    "£0",
                    "£100"
                }
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
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Alternative provision",
                    "£10,000",
                    "52\r\nincrease\r\n52",
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
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Hospital education funding",
                    "15\r\ndecrease\r\n15",
                    "£1,249\r\ndecrease\r\n£1,909"
                }
            };
        }

        private static IEnumerable<TableData> GetMinimumFundingGuaranteeTableData(bool isPrimary, bool isSecondary)
        {
            yield return new TableData
            {
                Id = "mfgBudgetShareTable",
                TableRowData = GetAdjustedSchoolShareTableRows()
            };

            yield return new TableData
            {
                Id = "mfgPupilBudgetShareTable",
                TableRowData = GetPerPupilAdjustedSchoolShareTableRows()
            };

            yield return new TableData
            {
                Id = "mfgGuaranteedPupilBudgetShareTable",
                TableRowData = GetGuaranteedPerPupilTableRows()
            };

            yield return new TableData
            {
                Id = "mfgGuaranteedBudgetShareTable",
                TableRowData = GetGuaranteedSchoolShareTableRows(isPrimary, isSecondary)
            };

            yield return new TableData
            {
                Id = "mfgAdjustedBudgetShareTable",
                TableRowData = GetAdjustedSchoolShareTableYearFromYearToRows()
            };

            yield return new TableData
            {
                Id = "mfgMinimumBudgetShareTable",
                TableRowData = GetMinimumFundingGuaranteeTableRows()
            };

            yield return new TableData
            {
                Id = "mfgTotalMinimumGuaranteedBudgetShareTable",
                TableRowData = GetTotalMinimumFundingGuaranteeTableRows()
            };
        }

        private static IEnumerable<TableData> GetMFGCappingAndScalingBackByLocalAuthorityTables()
        {
            yield return new TableData
            {
                Id = "mfgPerPupilAdjustedSchoolBudgetShareTable",
                TableRowData = GetPerPupilAdjustedSBSRows()
            };

            yield return new TableData
            {
                Id = "mfgChangeInAdjustedSchoolBudgetShareTable",
                TableRowData = GetChangeInAdjustedSBSRows()
            };

            yield return new TableData
            {
                Id = "mfgExcessFundingGainsAboveLocalAuthorityCapTable",
                TableRowData = GetExcessFundingGainsOverLACapRows()
            };

            yield return new TableData
            {
                Id = "mfgPercentageScaledBackTable",
                TableRowData = GetPercentageScaledBackRows()
            };

            yield return new TableData
            {
                Id = "mfgCappingAndScalingBackAdjustmentTable",
                TableRowData = GetCappingAndScalingBackAdjustment()
            };
        }

        private static IEnumerable<TableRowData> GetCappingAndScalingBackAdjustment()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Capping and scaling back adjustment",
                    "Calculation",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Percentage scaled back",
                    string.Empty,
                    "0.0%",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Pupils {YearFrom} to {YearTo}",
                    string.Empty,
                    "£0",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Per pupil adjusted school budget share {YearFromMinus1} to {YearFrom}",
                    string.Empty,
                    "£0",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Capping and scaling back adjustment",
                    string.Empty,
                    "£0",
                }
            };
        }

        private static IEnumerable<TableRowData> GetPercentageScaledBackRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Percentage scaled back",
                    "Calculation",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Excess funding gains above local authority cap...",
                    string.Empty,
                    "0.0%",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "...is scaled back by",
                    string.Empty,
                    "0.0%",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Percentage scaled back",
                    string.Empty,
                    "0.0%",
                }
            };
        }

        private static IEnumerable<TableRowData> GetExcessFundingGainsOverLACapRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Excess funding gains above local authority cap",
                    "Calculation",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Change in per pupil adjusted school budget share",
                    string.Empty,
                    "0.0%",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Local authority cap",
                    string.Empty,
                    "0.0%",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Excess funding gains above local authority cap",
                    string.Empty,
                    "0.0%",
                }
            };
        }

        private static IEnumerable<TableRowData> GetChangeInAdjustedSBSRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Change in adjusted school budget share",
                    "Calculation",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Per pupil {YearFrom} to {YearTo}",
                    string.Empty,
                    "£4,443.93",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Per pupil {YearFromMinus1} to {YearFrom}",
                    string.Empty,
                    "£4,443.93",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Change in per pupil adjusted school budget share",
                    string.Empty,
                    "0.0%",
                }
            };
        }

        private static IEnumerable<TableRowData> GetPerPupilAdjustedSBSRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    $"Per pupil adjusted school budget share {YearFromMinus1} to {YearFrom}",
                    "Calculation",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Adjusted school budget share {YearFrom} to {YearTo}",
                    string.Empty,
                    "£2,771,249.75",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Pupils {YearFrom} to {YearTo}",
                    string.Empty,
                    "88",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Per pupil adjusted school budget share {YearFromMinus1} to {YearTo}",
                    string.Empty,
                    "£4,443.93",
                }
            };
        }

        private static IEnumerable<TableRowData> GetAdjustedSchoolShareTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Adjusted school budget share",
                    "Calculation",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "School budget share",
                    string.Empty,
                    "£2,635,991.17",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Schools Supplementary Grant",
                    "Plus",
                    "£0",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Approved exclusions",
                    "Minus",
                    "£0",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Lump sum (including London fringe) {YearFrom} to {YearTo}",
                    "Minus",
                    "£110,000",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Sparsity {YearFrom} to {YearTo}",
                    "Minus",
                    "£0",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Technical adjustments",
                    "Minus",
                    "£0",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Adjusted school budget share",
                    "Equals",
                    "£2,519,707.84",
                }
            };
        }

        private static IEnumerable<TableRowData> GetPerPupilAdjustedSchoolShareTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Per pupil adjusted school budget share",
                    "Calculation",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Adjusted school budget share",
                    string.Empty,
                    "£2,519,707.84",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Pupils",
                    "Divide",
                    "567",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Per pupil adjusted school budget share {YearFromMinus1} to {YearFrom}",
                    "Equals",
                    "£4,443.93",
                }
            };
        }

        private static IEnumerable<TableRowData> GetGuaranteedPerPupilTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Guaranteed per pupil school budget share",
                    "Calculation",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Per pupil adjusted school budget share {YearFromMinus1} to {YearFrom}",
                    string.Empty,
                    "£4,443.93"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Change guaranteed by local authority",
                    "Multiply",
                    "150%"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Per pupil adjusted school budget share {YearFromMinus1} to {YearFrom}",
                    "Plus",
                    "£4,443.93"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Guaranteed per pupil school budget share",
                    "Equals",
                    "£4,510.59"
                }
            };
        }

        private static IEnumerable<TableRowData> GetGuaranteedSchoolShareTableRows(bool isPrimary, bool isSeconday)
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Guaranteed school budget share",
                    "Calculation",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Guaranteed per pupil school budget share",
                    string.Empty,
                    "£4,510.59",
                }
            };

            int pupils;
            if (isPrimary && isSeconday)
            {
                pupils = -1;
            }
            else if (isPrimary)
            {
                pupils = 88;
            }
            else
            {
                pupils = -2;
            }

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Pupils",
                    "Multiply",
                    pupils.ToString()
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Guaranteed school budget share {YearFrom} to {YearTo}",
                    "Equals",
                    "£2,575,546.89"
                }
            };
        }

        private static IEnumerable<TableRowData> GetAdjustedSchoolShareTableYearFromYearToRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Adjusted school budget share",
                    "Calculation",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "School budget share",
                    string.Empty,
                    "£2,771,249.75",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Approved exclusions",
                    "Minus",
                    "£0",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Lump sum (including London fringe)",
                    "Minus",
                    "£110,000",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Sparsity",
                    "Minus",
                    "£0",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Technical adjustments",
                    "Minus",
                    "£0",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Adjusted school budget share {YearFrom} to {YearTo}",
                    "Equals",
                    "£2,771,249.75",
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
                    "Minimum funding guarantee",
                    "Calculation",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Guaranteed school budget share {YearFrom} to {YearTo}",
                    string.Empty,
                    "£2,575,546.89",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    $"Adjusted school budget share {YearFrom} to {YearTo}",
                    "Minus",
                    "£2,771,249.75",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Minimum funding guarantee",
                    "Equals",
                    "£0",
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
                    "Total minimum funding guarantee",
                    "Calculation",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Minimum funding guarantee",
                    string.Empty,
                    "£0",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Capping and scaling back adjustment",
                    "Minus",
                    "£0",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Adjustment to make sure capping and scaling back will not take the school budget share lower than the minimum funding level",
                    "Plus",
                    "£0",
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total minimum funding guarantee",
                    "Equals",
                    "£0",
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
                    "Rate",
                    "Weighting",
                    "Pupils",
                    "Sub-Total"
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
                            "£2,904.18",
                            "1",
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
                            "£2,938.80",
                            "1",
                            "88",
                            "£258,614.40"
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
                            "£4,058.70",
                            "1",
                            "543",
                            "£2,203,873.25"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Key Stage 4",
                            "£5,475.60",
                            "1",
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
                            "£3,862.65",
                            "1",
                            "347",
                            "£1,340,339.55"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Key Stage 4",
                            "£4,385.81",
                            "1",
                            "224",
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
                        string.Empty,
                        string.Empty,
                        "88",
                        "£258,614.40"
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
                        string.Empty,
                        string.Empty,
                        "571",
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
                    "Rate",
                    "Weighting",
                    "Pupils",
                    "Sub-Total"
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
                            "Primary IDACI band A (most deprived)",
                            "£345.88",
                            "0",
                            "365",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band B",
                            "£112.20",
                            "0.003279",
                            "365",
                            "£134.29"
                        }
                    };
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band C",
                            "£112.20",
                            "0",
                            "365",
                            "£0"
                        }
                    };
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band D",
                            "£112.20",
                            "0",
                            "365",
                            "£671.34"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band E",
                            "£112.20",
                            "0.045902",
                            "365",
                            "£1,879.83"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band F (least deprived)",
                            "£0",
                            "0.009836",
                            "365",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary free school meals",
                            "£2,546.83",
                            "0.042623",
                            "365",
                            "£39,622.07"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary pupils eligible for free school meals in past 6 years",
                            "£0",
                            "365",
                            "0.070833",
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
                            "£617.18",
                            "0",
                            "88",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band B",
                            "£447.45",
                            "0",
                            "88",
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
                            "88",
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
                            "88",
                            "£595.53"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band E",
                            "£257.16",
                            "0.087719",
                            "88",
                            "£1,985.09"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary IDACI band F (least deprived)",
                            "£216.01",
                            "0.035088",
                            "88",
                            "£666.98"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary free school meals",
                            "£462.88",
                            "0.155172",
                            "88",
                            "£6,320.69"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Primary pupils eligible for free school meals in past 6 years",
                            "£576.03",
                            "0.155172",
                            "88",
                            "£7,865.77"
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
                            "0.027180",
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
                            "0.140230",
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
                            "Secondary IDACI band A (most deprived)",
                            "£950",
                            "0",
                            "571",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band B",
                            "£900",
                            "0",
                            "571",
                            "£0"
                        }
                    };
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band C",
                            "£850",
                            "0",
                            "571",
                            "£0"
                        }
                    };
                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band D",
                            "£800",
                            "0",
                            "571",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band E",
                            "£750",
                            "0",
                            "571",
                            "£0"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary IDACI band F (least deprived)",
                            "£290",
                            "0.001751",
                            "571",
                            "£289.95"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary free school meals",
                            "£440",
                            "0.054291",
                            "571",
                            "£13,640.07"
                        }
                    };

                    yield return new TableRowData
                    {
                        RowItems = new List<string>
                        {
                            "Secondary pupils eligible for free school meals in past 6 years",
                            "£785",
                            "0.120141",
                            "571",
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
                    "Rate",
                    "Weighting",
                    "Pupils",
                    "Sub-Total"
                }
            };

            if (showPrimary && showSecondary)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Total looked-after children",
                        "£1,920.92",
                        "0.003604",
                        "1,249",
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
                        "0",
                        "88",
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
                        "Total looked-after children",
                        "£0",
                        "0.014134",
                        "571",
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
                    "Rate",
                    "Weighting",
                    "Pupils",
                    "Sub-Total"
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
                            "0.153846",
                            "88",
                            "£14,831.23"
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
                            "Secondary pupils not achieving the expected standards in KS2 tests",
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
                            "Secondary pupils not achieving the expected standards in KS2 tests",
                            "£1,550",
                            "0.168738",
                            "571",
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
                        string.Empty,
                        string.Empty,
                        string.Empty,
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
                        string.Empty,
                        string.Empty,
                        string.Empty,
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
                    "Rate",
                    "Weighting",
                    "Pupils",
                    "Sub-Total"
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
                            "£550.32",
                            "0.571429",
                            "88",
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
                            "£1,385",
                            "0.001751",
                            "571",
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
                    "Rate",
                    "Weighting",
                    "Pupils",
                    "Sub-Total"
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
                            "£900.05",
                            "0",
                            "88",
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
                            "0",
                            "571",
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
                    "Sub-Total"
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
                            "£117,675.27"
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
                            "£172,190"
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

        private static IEnumerable<TableRowData> GetSparsityTableData()
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

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total for small academy in sparsely populated area",
                    "£6,283.33"
                }
            };
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
                    "Does the local authority use the distance taper?",
                    "No"
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
                    "Sub-Total"
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
                        "Minimum per pupil funding level set by local authority",
                        "£4,800"
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
                        "Total adjustment to make sure minimum funding level is met",
                        "£83,247.94"
                    }
                };
            }
        }

        #endregion
    }
}