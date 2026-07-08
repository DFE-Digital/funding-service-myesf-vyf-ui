using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Tests.Constants;
using System.Collections.Generic;
using ViewYourFunding.Automation.Pages;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The LocalAuthorityDSGFundingBreakdownPageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class LocalAuthorityDSGFundingBreakdownPageTests : FundingStreamRegressionTestBase
    {
        /// <summary>
        /// The early years block name.
        /// </summary>
        private const string EarlyYearsBlockName = "Early years";

        /// <summary>
        /// The high needs block name.
        /// </summary>
        private const string HighNeedsBlockName = "High needs";

        /// <summary>
        /// The schools block name.
        /// </summary>
        private const string SchoolsBlockName = "Schools";

        /// <summary>
        /// The CSS block name.
        /// </summary>
        private const string CSSBlockName = "Central school services";

        /// <summary>
        /// The previous year from.
        /// </summary>
        private const int PreviousYearFrom = DsgCurrentYearFrom - 1, PreviousYearTo = DsgCurrentYearTo - 1;

        /// <summary>
        /// The funding type name.
        /// </summary>
        private const string FundingTypeName = "Dedicated schools grant";

        /// <summary>
        /// The search term.
        /// </summary>
        private const string SearchTerm = "ca";

        /// <summary>
        /// The local authority name.
        /// </summary>
        private const string LocalAuthorityName = "Camden";

        /// <summary>
        /// The local authority name with special free schools deductions.
        /// </summary>
        private const string LocalAuthorityNameWithSpecialFreeSchoolsDeductions = "Cambridgeshire";

        /// <summary>
        /// The local authority code.
        /// </summary>
        private const string LocalAuthorityCode = "202";

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure current page.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureCurrentPage()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            BasePage.EnsureCurrentPage($"{FundingTypeName} ({FundingStreamCode.DSG}) {DsgCurrentYearFrom} to {DsgCurrentYearTo}", true);
            BasePage.EnsureCurrentPage($"This allocation: ", true);
            BasePage.EnsureCurrentPage($"2020", true);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure guidance link.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureGuidanceLink()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureGuidanceLink(DsgCurrentYearFrom, DsgCurrentYearTo);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure spreadsheet link.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureSpreadsheetLink()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureSpreadsheetLink(LocalAuthorityName, DsgCurrentYearFrom, DsgCurrentYearTo);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure allocation history link.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureAllocationHistoryLink()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureHistoryLink(DsgCurrentYearFrom, DsgCurrentYearTo);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click history link and ensure on correct page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickHistoryLinkAndEnsureOnCorrectPage()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Act
            LocalAuthorityDSGFundingBreakdownPage.ClickHistoryLink();

            // Assert
            LocalAuthorityDSGHistoryPage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure early years total is correct.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureEarlyYearsTotalIsCorrect()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureTotalAmount(EarlyYearsBlockName);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure early years universal entitlement.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureEarlyYearsUniversalEntitlement()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureEarlyYearsUniversalEntitlement("£13,102,064.09");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure early years additional15.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureEarlyYearsAdditional15()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureEarlyYearsAdditional15("£2,648,967.28");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure early years2 year entitlement.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureEarlyYears2YearEntitlement()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureEarlyYears2YearEntitlement("£1,822,119");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure early years pupil premium.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureEarlyYearsPupilPremium()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureEarlyYearsPupilPremium("£109,813.35");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure early years disability access.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureEarlyYearsDisabilityAccess()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureEarlyYearsDisabilityAccess("£51,045");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure early years maintained nursery.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureEarlyYearsMaintainedNursery()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureEarlyYearsMaintainedNursery("£0");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure high needs disability access fund shows.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureHighNeedsDisabilityAccessFundShows()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.HighNeedsDisabilityAccessFundShows();
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure high needs hospital does not show.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureHighNeedsHospitalDoesNotShow()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.HighNeedsHospitalDoesNotShow();
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure high needs NFF amount.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureHighNeedsNFFAmount()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityNameWithSpecialFreeSchoolsDeductions, HighNeedsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureHighNeedsNFFAmount("£62,287,125.37");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure high needs basic entitlement amount.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureHighNeedsBasicEntitlementAmount()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, HighNeedsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureHighNeedsBasicEntitlementAmount("£1,649,305.78");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure high needs import export amount.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureHighNeedsImportExportAmount()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, HighNeedsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureHighNeedsImportExportAmount("£1,419,000");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure high needs additional funding special amount.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureHighNeedsAdditionalFundingSpecialAmount()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, HighNeedsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureHighNeedsAdditionalFundingSpecialAmount("£123,456,789");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure high needs hospital education and teachers pay/pension amount.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureHighNeedsHospitalEducationTeachersPayPensionAmount()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, HighNeedsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureHighNeedsHospitalEducationTeachersPayPensionAmount("£3,626,832.69");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure high need total before deductions.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureHighNeedTotalBeforeDeductions()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, HighNeedsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureHighNeedTotalBeforeDeductions("£36,294,153");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage high needs tab ensure post16 special educational needs place values match.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_HighNeedsTab_EnsurePost16SpecialEducationalNeedsPlaceValuesMatch()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityNameWithSpecialFreeSchoolsDeductions, HighNeedsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureAllPost16SpecialEducationalNeedsPlacesMatch("£456,000", "£710,000", "£710,000", "£700,000", "-£999");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure high need ap amount.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureHighNeedAPAmount()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, HighNeedsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureHighNeedAPAmount("£20,000");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure high need maintained special amount.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureHighNeedMaintainedSpecialAmount()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, HighNeedsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureHighNeedMaintainedSpecialAmount("£0");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure high need feilp amount.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureHighNeedFEILPAmount()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, HighNeedsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureHighNeedFEILPAmount("£0");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure high need deductions direct funding amount.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureHighNeedDeductionsDirectFundingAmount()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, HighNeedsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureHighNeedDeductionsDirectFundingAmount("£298,000,000");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click schools tab and ensure tab is selected.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickSchoolsTabAndEnsureTabIsSelected()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Act
            LocalAuthorityDSGFundingBreakdownPage.ClickSchoolsTab();

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(SchoolsBlockName);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click central school services tab and ensure tab is selected.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickCentralSchoolServicesTabAndEnsureTabIsSelected()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Act
            LocalAuthorityDSGFundingBreakdownPage.ClickCentralSchoolServicesTab();

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(CSSBlockName);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click high needs tab and ensure tab is selected.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickHighNeedsTabAndEnsureTabIsSelected()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Act
            LocalAuthorityDSGFundingBreakdownPage.ClickHighNeedsTab();

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(HighNeedsBlockName);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure high needs total is correct.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureHighNeedsTotalIsCorrect()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, HighNeedsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureTotalAmount(HighNeedsBlockName);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure allocation amount.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureAllocationAmount()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureAllocationAmount("£170,472,000");
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure allocation financial year.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureAllocationFinancialYear()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            BasePage.EnsureCurrentPage($"{FundingTypeName} ({FundingStreamCode.DSG}) {DsgCurrentYearFrom} to {DsgCurrentYearTo}", true);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure format request links.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureFormatRequestLinks()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Act
            LocalAuthorityDSGFundingBreakdownPage.ClickFormatRequestLink();

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureFormatRequestEmailLinkShown(LocalAuthorityName, DsgCurrentYearFrom, DsgCurrentYearTo);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure format request email link not shown.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureFormatRequestEmailLinkNotShown()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureFormatRequestEmailLinkNotShownWhenSectionHasBeenCollapsed(0);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage ensure ensure open document link.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_EnsureEnsureOpenDocumentLink()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureOpenDocumentLink();
        }

        #region Early Years block

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage when early years link clicked on summary page ensure correct tab is selected.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_WhenEarlyYearsLinkClickedOnSummaryPage_EnsureCorrectTabIsSelected()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(EarlyYearsBlockName);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click early years tab and ensure tab is selected.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickEarlyYearsTabAndEnsureTabIsSelected()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, SchoolsBlockName);

            // Act
            LocalAuthorityDSGFundingBreakdownPage.ClickEarlyYearsTab();

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(EarlyYearsBlockName);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click early years block link on statement page and ensure correct tab is selected on summary page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickEarlyYearsBlockLinkOnStatementPageAndEnsureCorrectTabIsSelectedOnSummaryPage()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(EarlyYearsBlockName);
        }

        #endregion


        #region Schools block

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage when schools link clicked on summary page ensure correct tab is selected.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_WhenSchoolsLinkClickedOnSummaryPage_EnsureCorrectTabIsSelected()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, SchoolsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(SchoolsBlockName);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click school block link on statement page and ensure correct tab is selected on summary page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickSchoolBlockLinkOnStatementPageAndEnsureCorrectTabIsSelectedOnSummaryPage()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, SchoolsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(SchoolsBlockName);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click schools tab and whole pupil number ensure tab is selected and all links on tab are as expected.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickSchoolsTabAndWholePupilNumber_EnsureTabIsSelectedAndAllLinksOnTabAreAsExpected()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, SchoolsBlockName);

            // Act
            LocalAuthorityDSGFundingBreakdownPage.ClickSchoolsTab();

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(SchoolsBlockName);

            LocalAuthorityDSGFundingBreakdownPage.EnsureFindOutWhyPartialPupilNumbersAreUsedLink(DsgCurrentYearFrom, DsgCurrentYearTo, false);

            LocalAuthorityDSGFundingBreakdownPage.EnsureHowWeCalculateThisLinkToggles(0, DsgCurrentYearFrom, DsgCurrentYearTo); // collapsed

            LocalAuthorityDSGFundingBreakdownPage.ClickHowWeCalculateThisLink("school-tab-how-we-calculate-this");

            LocalAuthorityDSGFundingBreakdownPage.EnsureHowWeCalculateThisLinkToggles(1, DsgCurrentYearFrom, DsgCurrentYearTo); // expanded

            LocalAuthorityDSGFundingBreakdownPage.EnsureDSGTechnicalNotesLinkIsCorrect(DsgCurrentYearFrom, DsgCurrentYearTo);

            LocalAuthorityDSGFundingBreakdownPage.EnsureGuidanceListItems(0, DsgCurrentYearFrom, DsgCurrentYearTo);

            LocalAuthorityDSGFundingBreakdownPage.EnsurePrintThisPageItems(0);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click schools tab and pupil number has decimal value ensure tab is selected and all links on tab are as expected.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickSchoolsTabAndPupilNumberHasDecimalValue_EnsureTabIsSelectedAndAllLinksOnTabAreAsExpected()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, "Cambridgeshire", SchoolsBlockName);

            // Act
            LocalAuthorityDSGFundingBreakdownPage.ClickSchoolsTab();

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(SchoolsBlockName);

            LocalAuthorityDSGFundingBreakdownPage.EnsureFindOutWhyPartialPupilNumbersAreUsedLink(DsgCurrentYearFrom, DsgCurrentYearTo, true);

            LocalAuthorityDSGFundingBreakdownPage.EnsureHowWeCalculateThisLinkToggles(0, DsgCurrentYearFrom, DsgCurrentYearTo); // collapsed

            LocalAuthorityDSGFundingBreakdownPage.ClickHowWeCalculateThisLink("school-tab-how-we-calculate-this");

            LocalAuthorityDSGFundingBreakdownPage.EnsureHowWeCalculateThisLinkToggles(1, DsgCurrentYearFrom, DsgCurrentYearTo); // expanded

            LocalAuthorityDSGFundingBreakdownPage.EnsureDSGTechnicalNotesLinkIsCorrect(DsgCurrentYearFrom, DsgCurrentYearTo);

            LocalAuthorityDSGFundingBreakdownPage.EnsureGuidanceListItems(0, DsgCurrentYearFrom, DsgCurrentYearTo);

            LocalAuthorityDSGFundingBreakdownPage.EnsurePrintThisPageItems(0);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click schools tab ensure all data is as expected on tab.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickSchoolsTab_EnsureAllDataIsAsExpectedOnTab()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, SchoolsBlockName);

            // Act
            LocalAuthorityDSGFundingBreakdownPage.ClickSchoolsTab();

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureSchoolTabTotal();
            var expectedDataTable = new[]
            {
                new KeyValuePair<string, string>("Unit of funding", "£5,389.25"),
                new KeyValuePair<string, string>("Number of pupils", "10,751"),
                new KeyValuePair<string, string>("Total primary schools", "£57,939,826.75")
            };

            LocalAuthorityDSGFundingBreakdownPage.EnsureTableData(
                LocalAuthorityDSGFundingBreakdownPage.PrimarySchoolTable,
                expectedDataTable);

            var expectedDtTable = new[]
            {
                new KeyValuePair<string, string>("Unit of funding", "£6,928.20"),
                new KeyValuePair<string, string>("Number of pupils", "7,957"),
                new KeyValuePair<string, string>("Total secondary schools", "£55,127,687")
            };

            LocalAuthorityDSGFundingBreakdownPage.EnsureTableData(
                LocalAuthorityDSGFundingBreakdownPage.SecondarySchoolTable,
                expectedDtTable);

            var dataTable = new[]
            {
                new KeyValuePair<string, string>("Total funding through the premises factor", "£3,410,075.04")
            };

            LocalAuthorityDSGFundingBreakdownPage.EnsureTableData(
                LocalAuthorityDSGFundingBreakdownPage.PremisesMobilityTable,
                dataTable);

            var dTable = new[]
            {
                new KeyValuePair<string, string>("Total growth funding", "£457,719.88"),
            };

            LocalAuthorityDSGFundingBreakdownPage.EnsureTableData(
                LocalAuthorityDSGFundingBreakdownPage.GrowthTable,
                dTable);

            LocalAuthorityDSGFundingBreakdownPage.EnsureSchoolTabTotal();

            LocalAuthorityDSGFundingBreakdownPage.EnsureFundingTeacherPayProtected();
            LocalAuthorityDSGFundingBreakdownPage.EnsureTotalFundingTransferable();
        }

        #endregion


        #region Central School Services block

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage when central school services link clicked on summary page ensure correct tab is selected.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_WhenCentralSchoolServicesLinkClickedOnSummaryPage_EnsureCorrectTabIsSelected()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, CSSBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(CSSBlockName);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click central school services tab ensure tab is selected and all links on tab are as expected.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickCentralSchoolServicesTab_EnsureTabIsSelectedAndAllLinksOnTabAreAsExpected()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, SchoolsBlockName);

            // Act
            LocalAuthorityDSGFundingBreakdownPage.ClickCentralSchoolServicesTab();

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(CSSBlockName);

            LocalAuthorityDSGFundingBreakdownPage.EnsureGuidanceListItems(1, DsgCurrentYearFrom, DsgCurrentYearTo);

            LocalAuthorityDSGFundingBreakdownPage.EnsurePrintThisPageItems(1);

            LocalAuthorityDSGFundingBreakdownPage.EnsureHowWeCalculateThisLinkToggles(0, PreviousYearFrom, PreviousYearTo); // collapsed

            LocalAuthorityDSGFundingBreakdownPage.ClickHowWeCalculateThisLink("css-tab-how-we-calculate-this");

            LocalAuthorityDSGFundingBreakdownPage.EnsureHowWeCalculateThisLinkToggles(1, DsgCurrentYearFrom, DsgCurrentYearTo); // expanded
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click central school services tab ensure all data is as expected on tab.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickCentralSchoolServicesTab_EnsureAllDataIsAsExpectedOnTab()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, SchoolsBlockName);

            // Act
            LocalAuthorityDSGFundingBreakdownPage.ClickCentralSchoolServicesTab();

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCentralSchoolServicesTabTotal();
            var expDataTable = new[]
            {
                new KeyValuePair<string, string>("Unit of funding", "£4.76"),
                new KeyValuePair<string, string>("Number of pupils", "18,708"),
                new KeyValuePair<string, string>("Total central school services", "£721,567.56"),
            };

            LocalAuthorityDSGFundingBreakdownPage.EnsureTableData(
                LocalAuthorityDSGFundingBreakdownPage.CentralSchoolServicesTable,
                expDataTable);

            var table = new[]
            {
                new KeyValuePair<string, string>("Total funding for historic commitments", "£708,000"),
            };

            LocalAuthorityDSGFundingBreakdownPage.EnsureTableData(
                LocalAuthorityDSGFundingBreakdownPage.HistoricCommitmentsTable,
                table);

            LocalAuthorityDSGFundingBreakdownPage.EnsureCentralSchoolServicesTabTotal();
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click CSS block link on statement page and ensure correct tab is selected on summary page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickCSSBlockLinkOnStatementPageAndEnsureCorrectTabIsSelectedOnSummaryPage()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, CSSBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(CSSBlockName);
        }

        #endregion


        #region High needs block

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage when high needs block link clicked on summary page ensure correct tab is selected.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_WhenHighNeedsBlockLinkClickedOnSummaryPage_EnsureCorrectTabIsSelected()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, HighNeedsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(HighNeedsBlockName);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click hig needs tab and ensure tab is selected.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickHigNeedsTabAndEnsureTabIsSelected()
        {
            // Arrange
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, SchoolsBlockName);

            // Act
            LocalAuthorityDSGFundingBreakdownPage.ClickHighNeedsTab();

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(HighNeedsBlockName);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage click high needs block link on statement page and ensure correct tab is selected on summary page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_ClickHighNeedsBlockLinkOnStatementPageAndEnsureCorrectTabIsSelectedOnSummaryPage()
        {
            // Arrange Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, HighNeedsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureCorrectTabHasBeenSelected(HighNeedsBlockName);
        }

        #endregion


        #region Ensure breadcrumb links

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage when exact search found ensure there is no breadcrumb linking to search results and go back to summary page.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityDSGFundingBreakdownPage_WhenExactSearchFound_EnsureThereIsNoBreadcrumbLinkingToSearchResultsAndGoBackToSummaryPage()
        {
            // Arrange
            var finalBreadcrumbText = $"DSG {DsgCurrentYearFrom} to {DsgCurrentYearTo}";

            // Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, LocalAuthorityCode, LocalAuthorityName, EarlyYearsBlockName, false);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureBreadcrumbsAndFinalText(finalBreadcrumbText, new[] { "Choose how to view funding", "View funding at organisation level", LocalAuthorityName });
            ViewYourFundingBasePage.EnsureAndClickBreadcrumb(LocalAuthorityName);
            BasePage.EnsureCurrentPage(LocalAuthorityName);
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage when more than one search result found ensure there is a breadcrumb linking to search results page and go to search page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_WhenMoreThanOneSearchResultFound_EnsureThereIsABreadcrumbLinkingToSearchResultsPageAndGoToSearchPage()
        {
            // Arrange
            var finalBreadcrumbText = $"DSG {DsgCurrentYearFrom} to {DsgCurrentYearTo}";

            // Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureBreadcrumbsAndFinalText(finalBreadcrumbText, new[] { "Choose how to view funding", "View funding at organisation level", "Search results", LocalAuthorityName });
            ViewYourFundingBasePage.EnsureAndClickBreadcrumb("Search results");
            LocalAuthorityDidYouMeanPage.EnsureCurrentPage();
        }

        /// <summary>
        /// LocalAuthorityDSGFundingBreakdownPage when more than one search result found ensure there is a breadcrumb linking to search results page and go back to summary page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void LocalAuthorityDSGFundingBreakdownPage_WhenMoreThanOneSearchResultFound_EnsureThereIsABreadcrumbLinkingToSearchResultsPageAndGoBackToSummaryPage()
        {
            // Arrange
            var finalBreadcrumbText = $"DSG {DsgCurrentYearFrom} to {DsgCurrentYearTo}";

            // Act
            LocalAuthorityDSGFundingBreakdownPage.Open(DsgCurrentYearFrom, DsgCurrentYearTo, SearchTerm, LocalAuthorityName, EarlyYearsBlockName);

            // Assert
            LocalAuthorityDSGFundingBreakdownPage.EnsureBreadcrumbsAndFinalText(finalBreadcrumbText, new[] { "Choose how to view funding", "View funding at organisation level", "Search results", LocalAuthorityName });
            ViewYourFundingBasePage.EnsureAndClickBreadcrumb(LocalAuthorityName);
            BasePage.EnsureCurrentPage(LocalAuthorityName);
        }

        #endregion

    }
}