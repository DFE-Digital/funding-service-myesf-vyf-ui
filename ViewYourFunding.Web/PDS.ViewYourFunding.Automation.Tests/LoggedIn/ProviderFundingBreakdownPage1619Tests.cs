using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;
using System.Collections.Generic;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass, TestCategory("Regression"), TestCategory("CoreRegression")]
    [Ignore]
    public class ProviderFundingBreakdownPage1619Tests : LoggedInRegressionTestBase
    {
        #region Private fields

        private static string ProviderUserNameWithTLevelStudents => "10007063 - External User 52 Contracts";

        private static string ProviderUserNameNoTLevelStudents => "10000552 - External User 85 - 1619";

        private static string ProviderUserNameNotSchoolNotAcademyNotSpecialAcademy => "10040776 - External User 94 - 1619";

        private static string ProviderUserName_SPI => "10067343 - External User 95 - SPI";

        private static string ProviderUserNameIsSchool => "10006247 - External User 90 - 1619";

        private static string ProviderUserNameFeWithSeaFishing => "10000055 - External User 104 - FE college with sea fishing funding";

        private static string ProviderUserNameTuitionFunding => "10047244 - External User 126 1619 Tuition Funding";

        private static string ProviderUserNameAcademyWithSportingExcellence => "10015175 - External User 105 - Academy with sporting excellence funding";

        private static string ProviderUserNameAdjustedAboveUpper => "10030456 - External User 115 - 1619 Adjusted Above Upper";

        private static string ProviderUserNameAdjustedBelowLower => "10054167 - External User 116 - 1619 Adjusted Below Lower";

        private static string ProviderUserNameAdjustedBelow500 => "10033567 - External User 117 - 1619 Adjusted Below 500";

        private static string ProviderUserNameInLimit => "10061616 - External User 118 - 1619 In Limit";

        private static string DiscretionaryBursaryFundInLimitMessage => "The bursary has not been adjusted because this year's discretionary bursary fund figure is between the transition limits";

        private static string DiscretionaryBursaryFundOutsudeOfLimitMessage => "The bursary has been adjusted because this year's discretionary bursary fund figure is outside the transition limits";

        private static string DiscretionaryBursaryFundBelow500Message => "The bursary figure has been adjusted to the £500 minimum allocation";

        private string ProviderUserName_AdvancedMathsReceivesFunding => "10000552 - External User 85 - 1619";

        private string ProviderUserName_AdvancedMathsPremiumIs0 => "10053725 - External User 122 - Advanced Maths - Premium Is 0";

        private string ProviderUserName_AdvancedMathsEligableStudentsBelowBaseLine => "10037002 - External User 123 - Advanced Maths - Eligible Students Below Baseline";

        private string ProviderUserName_NoEligibleStudentsForIndustryPlacement => "10015175 - External User 105 - Academy with sporting excellence funding";

        private readonly ApplicationConfiguration _applicationConfiguration;

        private string ProviderUserName_Primary => _applicationConfiguration.TestLoginExternalUsernamePrimary;

        private string ProviderUserName_SpecialAcademy => _applicationConfiguration.TestLoginExternalUsernameSpecialAcademy;

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderFundingBreakdownPage1619Tests"/> class.
        /// </summary>
        public ProviderFundingBreakdownPage1619Tests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion


        #region Tests

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_BasicLayout()
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
            ProviderFundingBreakdownPage.Ensure1619DocumentDownload();
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_MathsAndEnglish_EnsureContentWhenAdjustmentIsZero()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();

            // Act
            ProviderFundingBreakdownPage.ClickOnMathsAndEnglishCofLink();

            // Assert Tables
            foreach (var tableData in GetMathsAndEnglishCoFTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.MathsAndEnglishCoFAccordionSection,
                    tableData);
            }
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_MathsAndEnglish_EnsureContentWhenAdjustmentIsNotZero()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619HistoryPageViaLogin(ProviderUserName_SPI, ProviderUserPassword);

            ProviderHistoryPage.ClickOnHistoricFundingBreakdownWebLink_1619();

            // Act
            ProviderFundingBreakdownPage.ClickOnMathsAndEnglishCofLink();

            // Assert Tables
            foreach (var tableData in GetMathsAndEnglishCoFTableData(true))
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.MathsAndEnglishCoFAccordionSection,
                    tableData);
            }
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_AdvancedMathsPremium_EnsureContent()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_AdvancedMathsReceivesFunding, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnAdvancedMathsPremiumLink();

            // Assert Tables
            foreach (var tableData in GetAdvancedMathsPremiumTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.AdvancedMathsPremiumAccordionSection,
                    tableData);
            }
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_AdvancedMathsPremium_EligableStudentsBelowBaseLine()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_AdvancedMathsEligableStudentsBelowBaseLine, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnAdvancedMathsPremiumLink();

            // Assert Tables
            foreach (var tableData in GetAdvancedMathsPremiumEligableStudentsBelowBaseLineTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.AdvancedMathsPremiumAccordionSection,
                    tableData);
            }
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_AdvancedMathsPremium_PremiumIsZero()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_AdvancedMathsPremiumIs0, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnAdvancedMathsPremiumLink();

            // Assert Tables
            ProviderFundingBreakdownPage.AdvancedMathsNoEligibleStudents.Text.Should().Be("There were 0 eligible students from 2021 to 2022.");
        }

        [TestMethod]
        public void ProviderFudingBreakdownPage_1619_TeachersPension_EnsureContent()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();

            // Act
            ProviderFundingBreakdownPage.ClickOnTeachersPensionLink();

            // Assert Tables
            foreach (var tableData in GetTeachersPensionTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.TeachersPensionAccordionSection,
                    tableData);
            }

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.TeachersPensionAccordionSection,
                "More about teachers' pension annual payments (opens in new tab)");
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_HighNeeds_EnsureContent()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithPreviousYear();

            // Act
            ProviderFundingBreakdownPage.ClickOnHighNeedsLink();

            // Assert Tables
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
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_IndustryPlacement_EnsureContent()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameWithTLevelStudents, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithNoComparison();

            // Act
            ProviderFundingBreakdownPage.ClickOnIndustryPlacement();

            // Assert Tables
            foreach (var tableData in GetIndustrialPlacementTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.IndustryPlacementAccordionSection,
                    tableData);
            }

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.IndustryPlacementAccordionSection,
                "More about capacity and delivery fund calculations (opens in new tab)");
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.IndustryPlacementAccordionSection,
                "More about eligibility (opens in new tab)");
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_IndustryPlacement_NoTLevelFunding_EnsureContent()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameNoTLevelStudents, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnIndustryPlacement();

            // Assert
            foreach (var tableData in GetIndustrialPlacementTableData(true, true))
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.IndustryPlacementAccordionSection,
                    tableData);
            }

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.IndustryPlacementAccordionSection,
                "More about capacity and delivery fund calculations (opens in new tab)");
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.IndustryPlacementAccordionSection,
                "More about eligibility (opens in new tab)");
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_IndustryPlacement_EligibleStudentNumbersLessThanTlevelStudents_EnsureContent()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameTuitionFunding, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnIndustryPlacement();

            // Assert
            foreach (var tableData in GetIndustrialPlacementTableDataForEligibleStudentNumbersLessThanTlevelStudents())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.IndustryPlacementAccordionSection,
                    tableData);
            }

            ProviderFundingBreakdownPage.EnsureTabElement(ProviderFundingBreakdownPage.IndustryPlacementAccordionSection, "eligible-students-less-than-tlevels", "Funded students is 0 because eligible students is lower than estimated T-Level students.");

            ProviderFundingBreakdownPage.EnsureTabLinkNotShown(
                ProviderFundingBreakdownPage.IndustryPlacementAccordionSection,
                "More about capacity and delivery fund calculations (opens in new tab)");
            ProviderFundingBreakdownPage.EnsureTabLinkNotShown(
                ProviderFundingBreakdownPage.IndustryPlacementAccordionSection,
                "More about eligibility (opens in new tab)");
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_IndustryPlacement_ZeroEligibleStudentNumbers_EnsureContent()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_NoEligibleStudentsForIndustryPlacement, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnIndustryPlacement();

            // Assert
            foreach (var tableData in GetIndustrialPlacementTableDataForNoEligibleStudentNumbers())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.IndustryPlacementAccordionSection,
                    tableData);
            }

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.IndustryPlacementAccordionSection,
                "More about capacity and delivery fund calculations (opens in new tab)");
            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.IndustryPlacementAccordionSection,
                "More about eligibility (opens in new tab)");
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_IndustryPlacement_NoAccordionSection_EnsureContent()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_SpecialAcademy, ProviderUserPassword);

            // Assert
            ProviderFundingBreakdownPage.IndustryPlacementAccordionSection.Should().BeNull();
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_CoreProgramme_EnsureContent()
        {
            // Arrange
            // Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();
            ProviderFundingBreakdownPage.OpenCoreProgrammeAccordion();

            // Assert
            foreach (var tableData in GetCoreProgrammeTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.CoreProgrammeAccordionSection,
                    tableData);
            }
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_HighValueCoursesPremium_EnsureContent()
        {
            // Arrange
            // Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithNoComparison();
            ProviderFundingBreakdownPage.OpenHighValueCoursesPremiumAccordion();

            // Assert
            foreach (var tableData in GetHighValueCoursesPremiumTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.HighValueCoursesPremiumAccordionSection,
                    tableData);
            }
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_HighValueCoursesPremium_WithCensusData_EnsureContent()
        {
            // Arrange
            // Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameAdjustedBelow500, ProviderUserPassword);
            ProviderFundingBreakdownPage.OpenHighValueCoursesPremiumAccordion();

            // Assert
            foreach (var tableData in GetHighValueCoursesPremiumTableData(true))
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.HighValueCoursesPremiumAccordionSection,
                    tableData);
            }
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_StudentFinancialSupport_EnsureContent()
        {
            // Arrange
            // Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();
            ProviderFundingBreakdownPage.OpenStudentFinancialSupportAccordion();

            // Assert
            foreach (var tableData in GetStudentFinancialSupportTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.StudentFinancialSupportAccordionSection,
                    tableData);
            }
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_StudentFinancialSupport_IsNotSchoolOrAcademy_EnsureContent()
        {
            // Arrange
            // Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameNotSchoolNotAcademyNotSpecialAcademy, ProviderUserPassword);
            ProviderFundingBreakdownPage.OpenStudentFinancialSupportAccordion();

            // Assert
            foreach (var tableData in GetStudentFinancialSupport_WhenNotSchoolOrAcademy_TableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.StudentFinancialSupportAccordionSection,
                    tableData);
            }
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_ResidentialAccommodation_EnsureContent()
        {
            // Arrange
            // Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_Primary, ProviderUserPassword);
            VarianceSelectionPage.SelectAndContinueWithCurrentYear();
            ProviderFundingBreakdownPage.OpenResidentialAccommodationAccordion();

            // Assert
            foreach (var tableData in GetResidentialAccommodationTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.ResidentialAccommodationAccordionSection,
                    tableData);
            }

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.ResidentialAccommodationAccordionSection,
                "More about residential accommodation lump sum (opens in new tab)");

            ProviderFundingBreakdownPage.EnsureGuidance(ProviderFundingBreakdownPage.ResidentialAccommodationAccordionSection, "Where eligible students 2018 to 2019 comes from", "Student numbers are from the October 2020 return (R14).");
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_SpecialAcademy_EnsureContent()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_SpecialAcademy, ProviderUserPassword);

            // Assert
            ProviderFundingBreakdownPage.Ensure1619SpecialAcademyAccordionSections();
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_CoreProgramme_SpecialAcademy_EnsureHidden()
        {
            // Arrange
            // Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserName_SpecialAcademy, ProviderUserPassword);

            // Assert
            ProviderFundingBreakdownPage.CoreProgrammeAccordionSection.Should().BeNull();
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_ResidentialAccommodation_WhenNotSchoolAndNotAcademyAndNotSpecialAcademy_EnsureHidden()
        {
            // Arrange
            // Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameNotSchoolNotAcademyNotSpecialAcademy, ProviderUserPassword);

            // Assert
            ProviderFundingBreakdownPage.ResidentialAccommodationAccordionSection.Should().BeNull();
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_HighNeeds_WhenProviderIsSchool_IsHidden()
        {
            // Arrange
            // Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameIsSchool, ProviderUserPassword);

            // Assert
            ProviderFundingBreakdownPage.HighNeedsAccordionSection.Should().BeNull();
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_TeachersPension_WhenIsSchoolOrIsAcademyOrIsSpecialAcademy_ContentHidden()
        {
            // Arrange
            // Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameNotSchoolNotAcademyNotSpecialAcademy, ProviderUserPassword);

            // Assert
            ProviderFundingBreakdownPage.TeachersPensionAccordionSection.Should().BeNull();
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_SeaFishing_EnsureContent()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameFeWithSeaFishing, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnSeaFishingDiplomaLink();

            // Assert Tables
            foreach (var tableData in GetSeaFishingTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.SeaFishingAccordionSection,
                    tableData);
            }
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_TuitionFund_EnsureContent()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameTuitionFunding, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnTuitionFundLink();

            // Assert Tables
            foreach (var tableData in GetTuitionFundTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.TuitionFundAccordionSection,
                    tableData);
            }

            ProviderFundingBreakdownPage.EnsureTabLink(
                ProviderFundingBreakdownPage.TuitionFundAccordionSection,
                "More about 16 to 19 tuition fund (opens in new tab)");
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_SportingExcellence_EnsureContent()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameAcademyWithSportingExcellence, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.ClickOnSportingDiplomaLink();

            // Assert Tables
            foreach (var tableData in GetSportingExcellenceTableData())
            {
                ProviderFundingBreakdownPage.EnsureTabTableContentData(
                    ProviderFundingBreakdownPage.SportingExcellenceAccordionSection,
                    tableData);
            }
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_AdjustedDiscretionaryBursaryFund_InLimit()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameInLimit, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.OpenStudentFinancialSupportAccordion();

            // Assert
            var actual = ProviderFundingBreakdownPage.AdjustedDiscretionaryBursaryFundTableCell.Text;
            actual.Should().Be(DiscretionaryBursaryFundInLimitMessage);
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_AdjustedDiscretionaryBursaryFund_AboveUpperLimit()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameAdjustedAboveUpper, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.OpenStudentFinancialSupportAccordion();

            // Assert
            var actual = ProviderFundingBreakdownPage.AdjustedDiscretionaryBursaryFundTableCell.Text;
            actual.Should().Be(DiscretionaryBursaryFundOutsudeOfLimitMessage);
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_AdjustedDiscretionaryBursaryFund_BelowLowerLimit()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameAdjustedBelowLower, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.OpenStudentFinancialSupportAccordion();

            // Assert
            var actual = ProviderFundingBreakdownPage.AdjustedDiscretionaryBursaryFundTableCell.Text;
            actual.Should().Be(DiscretionaryBursaryFundOutsudeOfLimitMessage);
        }

        [TestMethod]
        public void ProviderFundingBreakdownPage_1619_AdjustedDiscretionaryBursaryFund_Below500()
        {
            // Arrange Act
            ProviderFundingBreakdownPage.NavigateTo1619PageViaLogin(ProviderUserNameAdjustedBelow500, ProviderUserPassword);

            // Act
            ProviderFundingBreakdownPage.OpenStudentFinancialSupportAccordion();

            // Assert
            var actual = ProviderFundingBreakdownPage.AdjustedDiscretionaryBursaryFundTableCell.Text;
            actual.Should().Be(DiscretionaryBursaryFundBelow500Message);
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
                Id = "total-funded-students",
                TableRowData = GetTotalFundedStudentsRows()
            };

            yield return new TableData
            {
                Id = "total-mainstream-study-hours",
                TableRowData = GetTotalMainstreamStudyHoursRows()
            };

            yield return new TableData
            {
                Id = "total-t-levels-over-two",
                TableRowData = GetTotalTLevelsOverTwoYearsRows()
            };

            yield return new TableData
            {
                Id = "student-funding-breakdown",
                TableRowData = GetStudentFundingRows()
            };

            yield return new TableData
            {
                Id = "retention-factor-adjustment",
                TableRowData = GetRetentionFactorAdjustmentRows()
            };

            yield return new TableData
            {
                Id = "total-cost-weighting-breakdown",
                TableRowData = GetTotalCostWeightingBreakdownRows()
            };

            yield return new TableData
            {
                Id = "level-three-maths-english-breakdown",
                TableRowData = GetLevelThreeMathsEnglishBreakdownRows()
            };

            yield return new TableData
            {
                Id = "total-deprivation-breakdown",
                TableRowData = GetTotalDeprivationBreakdownRows()
            };

            yield return new TableData
            {
                Id = "care-leavers-breakdown",
                TableRowData = GetCareLeaversBreakdownRows()
            };

            yield return new TableData
            {
                Id = "total-low-prior-attainment-breakdown",
                TableRowData = GetTotalLowPriorAttainmentBreakdownRows()
            };

            yield return new TableData
            {
                Id = "disadvantage-funding-top-up",
                TableRowData = GetDisadvantageFundingBreakdownRows()
            };

            yield return new TableData
            {
                Id = "large-programme-uplift",
                TableRowData = GetLargeProgrammeUpliftRows()
            };
        }

        private static IEnumerable<TableData> GetMathsAndEnglishCoFTableData(bool forNonZeroAdjustment = false)
        {
            yield return new TableData
            {
                Id = "studentsFundedTable",
                TableRowData = GetStudentsFundedTableRows(forNonZeroAdjustment)
            };

            yield return new TableData
            {
                Id = "studentsNotMeetingConditionTable",
                TableRowData = GetStudentsNotMeetingConditionTableRows(forNonZeroAdjustment)
            };

            yield return new TableData
            {
                Id = "totalStudentFundingTable",
                TableRowData = GetTotalStudentFundingTableRows(forNonZeroAdjustment)
            };

            if (forNonZeroAdjustment)
            {
                yield return new TableData
                {
                    Id = "mathsAndEngDeductionTable",
                    TableRowData = GetfundingDeductionTableRows()
                };
            }
        }

        private static IEnumerable<TableData> GetAdvancedMathsPremiumTableData()
        {
            yield return new TableData
            {
                Id = "totalFundedStudentsTable",
                TableRowData = GetAdvancedMathsPremiumBreakdownRows()
            };
        }

        private static IEnumerable<TableData> GetAdvancedMathsPremiumEligableStudentsBelowBaseLineTableData()
        {
            yield return new TableData
            {
                Id = "totalFundedStudentsBelowBaselineTable",
                TableRowData = GetAdvancedMathsPremiumBreakdownRowsWithEligableStudentsBelowBaseLine()
            };
        }

        private static IEnumerable<TableData> GetHighValueCoursesPremiumTableData(bool withCensusData = false)
        {
            yield return new TableData
            {
                Id = "high-value-courses-breakdown",
                TableRowData = GetHighValuesCoursesBreakdownRows(withCensusData)
            };
        }

        private static IEnumerable<TableData> GetStudentFinancialSupportTableData()
        {
            yield return new TableData
            {
                Id = "student-financial-support",
                TableRowData = GetTotalStudentFinancialSupportBreakdownRows()
            };

            yield return new TableData
            {
                Id = "discretionary-bursary-fund-breakdown",
                TableRowData = GetDicretionaryBursaryBreakdownRows()
            };

            yield return new TableData
            {
                Id = "adjusted-discretionary-bursary-breakdown",
                TableRowData = GetAdjustedDiscretionaryBursaryFundRows()
            };

            yield return new TableData
            {
                Id = "exceptional-adjustment-breakdown",
                TableRowData = GetExceptionalAdjustmentFundRows()
            };

            yield return new TableData
            {
                Id = "adjusted-discretionary-bursary",
                TableRowData = GetAdjustedDiscretionaryFundTotalRows()
            };

            yield return new TableData
            {
                Id = "residential-funding-breakdown",
                TableRowData = GetResidentialFundingBreakdownRows()
            };

            yield return new TableData
            {
                Id = "financial-disadvantaged-breakdown",
                TableRowData = FinancialDisadvantagedStudentsRows()
            };

            yield return new TableData
            {
                Id = "proportion-of-students",
                TableRowData = GetProportionalFundingFundingBreakdownRows()
            };

            yield return new TableData
            {
                Id = "free-meals-breakdown",
                TableRowData = GetTotalFreeMealsBreakdownRows()
            };

            yield return new TableData
            {
                Id = "free-meals-exceptional-adjustments",
                TableRowData = GetFreeMealsExceptionalRows()
            };

            yield return new TableData
            {
                Id = "free-meals-total",
                TableRowData = GetFreeMealsTotalRows()
            };
        }

        private static IEnumerable<TableData> GetStudentFinancialSupport_WhenNotSchoolOrAcademy_TableData()
        {
            yield return new TableData
            {
                Id = "discretionary-bursary-fund-breakdown",
                TableRowData = GetDicretionaryBursary_WhenNotSchoolOrAcademyBreakdownRows()
            };

            yield return new TableData
            {
                Id = "adjusted-discretionary-bursary-breakdown",
                TableRowData = GetAdjustedDiscretionaryBursaryFund_WhenNotSchoolOrAcademyRows()
            };

            yield return new TableData
            {
                Id = "exceptional-adjustment-breakdown",
                TableRowData = GetExceptionalAdjustmentFundRows()
            };
        }

        private static IEnumerable<TableData> GetSeaFishingTableData()
        {
            yield return new TableData
            {
                Id = "fishing-diploma-table",
                TableRowData = GetSeaFishingTableRows()
            };
        }

        private static IEnumerable<TableData> GetTuitionFundTableData()
        {
            yield return new TableData
            {
                Id = "tuition-fund-table",
                TableRowData = GetTuitionFundTableRows()
            };
        }

        private static IEnumerable<TableData> GetSportingExcellenceTableData()
        {
            yield return new TableData
            {
                Id = "sporting-diploma-table",
                TableRowData = GetSportingExcellenceTableRows()
            };
        }

        private static IEnumerable<TableRowData> GetTotalStudentFinancialSupportBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Total student financial support 2021 to 2022",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Discretionary bursary fund",
                    string.Empty,
                    "£663,659"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Residential funding",
                    "plus",
                    "£0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Accommodation costs for financially disadvantaged students",
                    "plus",
                    "£250"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Free meals",
                    "plus",
                    "£241,552"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Total student financial support 2021 to 2022",
                    "equals",
                    "£905,211"
                }
            };
        }

        private static IEnumerable<TableRowData> GetTotalFreeMealsBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Free meals",
                    "Funded students",
                    "Proportion of\r\nstudents",
                    "Free meals\r\nstudents",
                    "Rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "450 hours and over\r\n(bands 4 and 5)",
                    "635.71",
                    "0.13",
                    "635.71",
                    "£358",
                    "£227,583"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "280 to 449 hours\r\n(bands 2 and 3)",
                    "7.22",
                    "0.13",
                    "7.22",
                    "£179",
                    "£1,293"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Full time equivalent\r\nof band 1",
                    "1.67",
                    "0.13",
                    "1.67",
                    "£358",
                    "£598"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Free meals\r\nadministration",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "£12,078"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Free meals",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "£241,552"
                }
            };
        }

        private static IEnumerable<TableRowData> GetFreeMealsExceptionalRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Free meals exceptional adjustment",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Exceptional adjustment",
                    "£0"
                }
            };
        }

        private static IEnumerable<TableRowData> GetFreeMealsTotalRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Free meals total",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Free meals",
                    string.Empty,
                    "£241,552"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Exceptional adjustment",
                    "plus",
                    "£0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Total",
                    "equals",
                    "£241,552"
                }
            };
        }

        private static IEnumerable<TableRowData> GetResidentialFundingBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Residential funding",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Residential bursary fund",
                    string.Empty,
                    "£0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Residential support scheme",
                    "plus",
                    "£0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Total residential funding",
                    "equals",
                    "£0"
                }
            };
        }

        private static IEnumerable<TableRowData> FinancialDisadvantagedStudentsRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Accommodation costs for financially disadvantaged students",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Funding to support accommodation costs",
                    "£250"
                }
            };
        }

        private static IEnumerable<TableRowData> GetProportionalFundingFundingBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Proportion of students receiving free meals",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Free school meals students 2018 to 2019",
                    string.Empty,
                    "£1,023,602\r\ndecrease\r\n£10,000,000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Number of students from October 2020 return (R14)",
                    "divide",
                    "4995"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Proportion of students receiving free meals",
                    "equals",
                    "£682,402\r\ndecrease\r\n£1,000,000"
                }
            };
        }

        private static IEnumerable<TableRowData> GetAdjustedDiscretionaryBursaryFundRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Adjusted discretionary bursary fund",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "2019 to 2020 baseline for transition",
                    "£530,927"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Transition lower limit (50%)",
                    "£398,196"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Transition upper limit (150%)",
                    "£663,659"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Discretionary bursary fund 2021 to 2022",
                    "£1,384,346"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Adjusted discretionary bursary fund",
                    "£663,659"
                }
            };
        }

        private static IEnumerable<TableRowData> GetExceptionalAdjustmentFundRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Discretionary bursary exceptional adjustment",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Exceptional adjustment",
                    "£0"
                }
            };
        }

        private static IEnumerable<TableRowData> GetAdjustedDiscretionaryFundTotalRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Adjusted discretionary bursary fund total",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Adjusted discretionary bursary fund",
                    string.Empty,
                    "£663,659"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Exceptional adjustment",
                    "plus",
                    "£0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Total",
                    "equals",
                    "£905,211"
                }
            };
        }

        private static IEnumerable<TableRowData> GetAdjustedDiscretionaryBursaryFund_WhenNotSchoolOrAcademyRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Adjusted discretionary bursary fund",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "2019 to 2020 baseline for transition",
                    "£10,299"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Transition lower limit (50%)",
                    "£7,724"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Transition upper limit (150%)",
                    "£12,874"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Discretionary bursary fund 2021 to 2022",
                    "£2,868"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Student financial support funding total",
                    "£7,724"
                }
            };
        }

        private static IEnumerable<TableRowData> GetDicretionaryBursaryBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Discretionary bursary breakdown",
                    "Total funded\r\nstudents",
                    "Instances per\r\nstudent",
                    "Number of\r\ninstances",
                    "Rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Financial disadvantage",
                    "4,946",
                    "0.174",
                    "862.43",
                    "£242",
                    "£208,707"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Travel",
                    "4,946",
                    "0.516",
                    "2,554.54",
                    "£483",
                    "£1,233,843"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Industry placement",
                    string.Empty,
                    string.Empty,
                    "770.00",
                    "£49",
                    "£37,730"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Free meals adjustments",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "-£95,935"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Discretionary bursary fund 2021 to 2022",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "£1,384,346"
                }
            };
        }

        private static IEnumerable<TableRowData> GetDicretionaryBursary_WhenNotSchoolOrAcademyBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Discretionary bursary breakdown",
                    "Total funded\r\nstudents",
                    "Instances per\r\nstudent",
                    "Number of\r\ninstances",
                    "Rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Financial disadvantage",
                    "96",
                    "0.056",
                    "5.39",
                    "£242",
                    "£1,304"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Travel",
                    "96",
                    "0.034",
                    "3.24",
                    "£483",
                    "£1,564"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Discretionary bursary fund 2021 to 2022",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "£2,868"
                }
            };
        }

        private static IEnumerable<TableData> GetResidentialAccommodationTableData()
        {
            yield return new TableData
            {
                Id = "care-standards-total-breakdown",
                TableRowData = GetCareStandardsBreakdownRows()
            };

            yield return new TableData
            {
                Id = "care-standards-total-lump-sum",
                TableRowData = GetCareStandardsLumpSumRows()
            };
        }

        private static IEnumerable<TableRowData> GetCareStandardsLumpSumRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Lump sum",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Total lump sum",
                    "£0"
                }
            };
        }

        private static IEnumerable<TableRowData> GetCareStandardsBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Student funding",
                    "Eligible students 2018 to 2019",
                    "Rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Student funding",
                    "0\r\ndecrease\r\n10",
                    "£817",
                    "£0\r\ndecrease\r\n£10"
                }
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
                    "Student numbers",
                    string.Empty,
                    "4,946"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Student funding",
                    string.Empty,
                    "£19,985,637"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Retention factor",
                    "Multiply",
                    "0.96500\r\ndecrease\r\n10.00000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Programme cost weighting",
                    "Multiply",
                    "1.07100"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Level 3 maths and English payment",
                    "Plus",
                    "£169,579"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Disadvantage funding",
                    "Plus",
                    "£1,588,716\r\ndecrease\r\n£10,000,000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Large programme uplift",
                    "Plus",
                    "£91,342"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Area cost allowance",
                    "Multiply",
                    "1.00000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total core programme",
                    "Equals",
                    "£22,508,598\r\ndecrease\r\n£100,000,000"
                }
            };
        }

        private static IEnumerable<TableRowData> GetTotalFundedStudentsRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Total funded students 2021 to 2022",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Funded students from December 2020 return (R04)",
                    string.Empty,
                    "4,926"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Funded students from October 2020 return (R14)",
                    "Multiply",
                    "4,955"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Funded students from December 2019 return (R04)",
                    "Divide",
                    "4,900"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Changes to student numbers agreed with ESFA",
                    "Plus",
                    "0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Total funded students 2021 to 2022",
                    "Equals",
                    "4,946"
                }
            };
        }

        private static IEnumerable<TableRowData> GetTotalMainstreamStudyHoursRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Mainstream annual planned study hours",
                    "Funded students",
                    "National rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "16 and 17 year olds, students aged 18 and over with high needs studying 540 hours and over (band 5)",
                    "4,117",
                    "£4,188",
                    "£17,243,651"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Aged 18 and over who are not high needs and studying over 450 hours (band 4a)",
                    "610",
                    "£3,455",
                    "£2,107,550"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "16 and 17 year olds, students aged 18 and over with high needs studying 450 to 539 hours (band 4b)",
                    "60",
                    "£3,455",
                    "£207,300"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "360 to 449 hours (band 3)",
                    "47",
                    "£2,827",
                    "£132,627"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "280 to 359 hours (band 2)",
                    "8",
                    "£2,234",
                    "£17,839"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Up to 279 hours (band 1)",
                    "60",
                    string.Empty,
                    string.Empty
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Full time equivalent (band 1)",
                    "12.69",
                    "£4,188",
                    "£53,146"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Mainstream funding",
                    "4,901",
                    string.Empty,
                    "£19,757,892"
                }
            };
        }

        private static IEnumerable<TableRowData> GetTotalTLevelsOverTwoYearsRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "T Levels planned study hours over 2 years",
                    "Funded students",
                    "National rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "At least 1650 hours for specific courses (band 9)",
                    "0",
                    "£6,108",
                    "£0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "At least 1500 hours for specific courses (band 8)",
                    "0",
                    "£5,584",
                    "£0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "At least 1300 hours for specific courses (band 7)",
                    "45",
                    "£5,061",
                    "£227,745"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "At least 1100 hours for specific courses (band 6)",
                    "0",
                    "£4,363",
                    "£0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "T Level funding",
                    "45",
                    string.Empty,
                    "£227,745"
                }
            };
        }

        private static IEnumerable<TableRowData> GetStudentFundingRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Student funding",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Mainstream funding",
                    string.Empty,
                    "£19,757,892"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "T level funding",
                    "Plus",
                    "£227,745\r\ndecrease\r\n£1,000,000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Student funding",
                    "Equals",
                    "£19,985,637"
                }
            };
        }

        private static IEnumerable<TableRowData> GetRetentionFactorAdjustmentRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Student funding after retention adjustment",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Student funding",
                    string.Empty,
                    "£19,985,637"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Adjusted proportion of continuing or completed students (retention factor)",
                    "Multiply",
                    "0.96500\r\ndecrease\r\n10.00000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Student funding after retention adjustments",
                    "Equals",
                    "£19,291,935"
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
                    "£19,291,935"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Programme cost weighting for subjects that cost more to deliver",
                    "Multiply",
                    "1.07100"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Core programme up to programme cost weighting",
                    "Equals",
                    "£20,658,962"
                }
            };
        }

        private static IEnumerable<TableRowData> GetLevelThreeMathsEnglishBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Level 3 maths and English payment\r\nSupports delivery of maths and English to students on substantial level 3 study programmes (including T Levels) who have not yet attained a grade 9 to 4 GCSE or equivalent in either or both of these subjects",
                    "Payment per subject",
                    "Instances",
                    "Total funded students",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Students on a 1 year eligible programme (less than 18 months)",
                    "£375",
                    "0.08600",
                    "424",
                    "£159,081"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Students on a 2 year eligible programme (18 months or more)",
                    "£750",
                    "0.00300",
                    "14",
                    "£10,498"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Total level 3 maths and English payment",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "£169,579"
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
                    "Core programme up to level 3 maths and English",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Deprivation",
                    "1.029",
                    "£20,828,540",
                    "£599,237"
                }
            };
        }

        private static IEnumerable<TableRowData> GetCareLeaversBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Care leavers (block 1)",
                    "Eligible students",
                    "Rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Care leavers",
                    "37\r\ndecrease\r\n100",
                    "£480",
                    "£17,760\r\ndecrease\r\n£100,000"
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
                    "Funded students",
                    "Average instances",
                    "Rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Bands 4 and 5",
                    "1,980",
                    "0.41",
                    "£480",
                    "£950,465"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Bands 2 and 3",
                    "23",
                    "0.41",
                    "£292",
                    "£6,632"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Full time equivalent of band 1",
                    "5.25",
                    "0.41",
                    "£480",
                    "£2,520"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "T Levels",
                    "19",
                    "0.41",
                    "£650",
                    "£12,101"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Total low prior attainment in maths or English",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    "£971,719"
                }
            };
        }

        private static IEnumerable<TableRowData> GetDisadvantageFundingBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Disadvantage funding top up",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Top up\r\nTo bring the disadvantage funding amount up to the £6,000 minimum",
                    "£0\r\ndecrease\r\n£10",
                }
            };
        }

        private static IEnumerable<TableRowData> GetLargeProgrammeUpliftRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Large programme uplift",
                    "Eligible students 2018 to 2019",
                    "Rate",
                    "Years funded",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Students attracting 20% of band 5 rate",
                    "39",
                    "£838",
                    "2",
                    "£65,364"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Students attracting 10% of band 5 rate",
                    "31",
                    "£419",
                    "2",
                    "£25,978"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Total for large programme uplift",
                    "70",
                    string.Empty,
                    string.Empty,
                    "£91,342"
                }
            };
        }

        private static IEnumerable<TableData> GetTeachersPensionTableData()
        {
            yield return new TableData
            {
                Id = "pension-adjusted-payments-table",
                TableRowData = GetTeachersPensionTable1Rows()
            };

            yield return new TableData
            {
                Id = "pension-12months-table",
                TableRowData = GetTeachersPensionTable2Rows()
            };

            yield return new TableData
            {
                Id = "pension-8months-table",
                TableRowData = GetTeachersPensionTable3Rows()
            };
        }

        private static IEnumerable<TableData> GetHighNeedsTableData()
        {
            yield return new TableData
            {
                Id = "high-needs-table",
                TableRowData = GetHighNeedsTableRows()
            };
        }

        private static IEnumerable<TableData> GetHighNeedsFundedPlacesTableData()
        {
            yield return new TableData
            {
                Id = "high-needs-funded-places-table",
                TableRowData = GetHighNeedsFundedPlacesTableRows()
            };
        }

        private static IEnumerable<TableData> GetIndustrialPlacementTableDataForEligibleStudentNumbersLessThanTlevelStudents()
        {
            yield return new TableData
            {
                Id = "capacity-delivery-fund-level2-level3-breakdown",
                TableRowData = GetCapacityDeliveryFundTableRowsForEligibleStudentNumbersLessThanTlevelStudents()
            };

            yield return new TableData
            {
                Id = "t-levels-breakdown",
                TableRowData = GetTLevelsTableRowsForEligibleStudentNumbersLessThanTlevelStudents()
            };
        }

        private static IEnumerable<TableData> GetIndustrialPlacementTableDataForNoEligibleStudentNumbers()
        {
            yield return new TableData
            {
                Id = "capacity-delivery-fund-level2-level3-breakdown",
                TableRowData = GetCapacityDeliveryFundTableRowsForEligibleStudentNumbersLessThanTlevelStudents(true)
            };

            yield return new TableData
            {
                Id = "t-levels-breakdown",
                TableRowData = GetTLevelsTableRows(true)
            };
        }

        private static IEnumerable<TableData> GetIndustrialPlacementTableData(
            bool takeTLevels = true,
            bool variant = false)
        {
            yield return new TableData
            {
                Id = "capacity-delivery-fund-level2-level3-breakdown",
                TableRowData = GetCapacityDeliveryFundTableRows(variant)
            };

            if (takeTLevels)
            {
                yield return new TableData
                {
                    Id = "t-levels-breakdown",
                    TableRowData = GetTLevelsTableRows(variant)
                };
            }
        }

        private static IEnumerable<TableRowData> GetStudentsFundedTableRows(bool forNonZeroAdjustment = false)
        {
            if (!forNonZeroAdjustment)
            {
                yield return new TableRowData
                {
                    HeaderRow = true,
                    RowItems = new List<string>
                {
                    "Annual planned study hours",
                    "Funded students from\r\n2019 to 2020",
                    "Rate",
                    "Sub-Total"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "540 hours and over (band 5)",
                    "4,117",
                    "£4,188\r\ndecrease\r\n£10,000",
                    "£17,243,651\r\ndecrease\r\n£100,000,000"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "450 to 539 hours (band 4)",
                    "669",
                    "£3,455",
                    "£2,310,628\r\ndecrease\r\n£10,000,000"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "360 to 449 hours (band 3)",
                    "47",
                    "£2,827\r\ndecrease\r\n£10,000",
                    "£132,627\r\ndecrease\r\n£1,000,000"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "280 to 359 hours (band 2)",
                    "8",
                    "£2,234\r\ndecrease\r\n£10,000",
                    "£17,839\r\ndecrease\r\n£100,000"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Up to 279 hours (band 1)",
                    "60",
                    string.Empty,
                    string.Empty
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Full time equivalent of band 1",
                    "13",
                    "£4,188\r\ndecrease\r\n£10,000",
                    "£53,146\r\ndecrease\r\n£100,000"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Student funding",
                    "4,901",
                    string.Empty,
                    "£19,757,892\r\ndecrease\r\n£100,000,000"
                }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    HeaderRow = true,
                    RowItems = new List<string>
                {
                    "Annual planned study hours",
                    "Funded students from\r\n2019 to 2020",
                    "Rate",
                    "Sub-Total"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "540 hours and over (band 5)",
                    "14,117",
                    "£14,188",
                    "£117,243,651"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "450 to 539 hours (band 4)",
                    "1,669",
                    "£3,455",
                    "£12,310,628"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "360 to 449 hours (band 3)",
                    "147",
                    "£12,827",
                    "£1,132,627"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "280 to 359 hours (band 2)",
                    "18",
                    "£12,234",
                    "£117,839"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Up to 279 hours (band 1)",
                    "160",
                    string.Empty,
                    string.Empty
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Full time equivalent of band 1",
                    "113",
                    "£14,188",
                    "£153,146"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Student funding",
                    "14,901",
                    string.Empty,
                    "£119,757,892"
                }
                };
            }
        }

        private static IEnumerable<TableRowData> GetStudentsNotMeetingConditionTableRows(bool forNonZeroAdjustment = false)
        {
            if (!forNonZeroAdjustment)
            {
                yield return new TableRowData
                {
                    HeaderRow = true,
                    RowItems = new List<string>
                {
                    "Annual planned study hours",
                    "Students not meeting condition",
                    "Rate",
                    "Sub-Total"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "540 hours and over (band 5)",
                    "48",
                    "£4,000\r\ndecrease\r\n£10,000",
                    "£192,000\r\ndecrease\r\n£1,000,000"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "450 to 539 hours (band 4)",
                    "18",
                    "£3,300\r\ndecrease\r\n£10,000",
                    "£59,400\r\ndecrease\r\n£100,000"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "360 to 449 hours (band 3)",
                    "6",
                    "£2,700\r\ndecrease\r\n£10,000",
                    "£16,200\r\ndecrease\r\n£100,000"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "280 to 359 hours (band 2)",
                    "2",
                    "£2,133\r\ndecrease\r\n£10,000",
                    "£4,266\r\ndecrease\r\n£10,000"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Up to 279 hours (band 1)",
                    "5",
                    string.Empty,
                    string.Empty
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Full time equivalent of band 1",
                    "2",
                    "£4,000\r\ndecrease\r\n£10,000",
                    "£7,427\r\ndecrease\r\n£10,000"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Funding for students not meeting condition",
                    "79",
                    string.Empty,
                    "£279,293\r\ndecrease\r\n£1,000,000"
                }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    HeaderRow = true,
                    RowItems = new List<string>
                {
                    "Annual planned study hours",
                    "Students not meeting condition",
                    "Rate",
                    "Sub-Total"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "540 hours and over (band 5)",
                    "148",
                    "£14,000",
                    "£1,192,000"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "450 to 539 hours (band 4)",
                    "118",
                    "£13,300",
                    "£159,400"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "360 to 449 hours (band 3)",
                    "16",
                    "£12,700",
                    "£116,200"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "280 to 359 hours (band 2)",
                    "12",
                    "£12,133",
                    "£14,266"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Up to 279 hours (band 1)",
                    "15",
                    string.Empty,
                    string.Empty
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Full time equivalent of band 1",
                    "12",
                    "£14,000",
                    "£17,427"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Funding for students not meeting condition",
                    "179",
                    string.Empty,
                    "£1,279,293"
                }
                };
            }
        }

        private static IEnumerable<TableRowData> GetTeachersPensionTable1Rows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Adjusted payments 2019 to 2020",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Payments 2019 to 2020",
                    " ",
                    "£1,938,115\r\ndecrease\r\n£10,000,000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Employer contribution rate (%) up to August 2019",
                    "divide",
                    "16.4"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Employer contribution rate (%) September 2019 onwards",
                    "Multiply",
                    "23.6"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "3.1% wage growth forecast for 2020 to 2021",
                    "Plus",
                    "£86,459"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "3% wage growth forecast for 2021 to 2022",
                    "Plus",
                    "£86,264"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total adjusted payments 2019 to 2020",
                    "equals",
                    "£2,961,718\r\ndecrease\r\n£10,000,000"
                }
            };
        }

        private static IEnumerable<TableRowData> GetTeachersPensionTable2Rows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Payments 2021 to 2022 for 12 months",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total adjusted payments 2019 to 2020",
                    " ",
                    "£2,961,718\r\ndecrease\r\n£10,000,000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Payments 2019 to 2020",
                    "Minus",
                    "£1,938,115\r\ndecrease\r\n£10,000,000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total payments 2021 to 2022 for 12 months",
                    "equals",
                    "£1,023,602\r\ndecrease\r\n£10,000,000"
                }
            };
        }

        private static IEnumerable<TableRowData> GetTeachersPensionTable3Rows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Payments 2021 to 2022 for 8 months",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Payments 2021 to 2022 for 12 months",
                    " ",
                    "£1,023,602\r\ndecrease\r\n£10,000,000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Months in the academic year",
                    "divide",
                    "12"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Months to be paid (August 2021 to March 2022)",
                    "Multiply",
                    "8"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total payments 2021 to 2022 for 8 months",
                    "equals",
                    "£682,402\r\ndecrease\r\n£1,000,000"
                }
            };
        }

        private static IEnumerable<TableRowData> GetCapacityDeliveryFundTableRows(bool variant = false)
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Capacity and delivery fund for level 2 and 3 vocational and technical programmes",
                    "Calculation",
                    "Sub-total"
                }
            };

            if (variant)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Eligible students from October 2020 return (R14)",
                        string.Empty,
                        "281"
                    }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Estimated T Level students",
                        "Minus",
                        "0"
                    }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Rate",
                        "Multiply",
                        "250"
                    }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Capacity and delivery fund for level 2 and 3 vocational and technical programmes",
                        "Equals",
                        "£70,250"
                    }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Eligible students from October 2020 return (R14)",
                        string.Empty,
                        "826"
                    }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Estimated T Level students",
                        "Minus",
                        "45"
                    }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Rate",
                        "Multiply",
                        "250"
                    }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Capacity and delivery fund for level 2 and 3 vocational and technical programmes",
                        "Equals",
                        "£206,500"
                    }
                };
            }
        }

        private static IEnumerable<TableRowData> GetTLevelsTableRows(bool noTLevels = false)
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "T Level",
                    "Estimated students",
                    "Rate",
                    "Sub-total"
                }
            };

            if (!noTLevels)
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "T Levels",
                    "45",
                    "£275",
                    "£12,375"
                }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "T Levels",
                    "0",
                    "£275",
                    "£12,375"
                }
                };
            }
        }

        private static IEnumerable<TableRowData> GetCapacityDeliveryFundTableRowsForEligibleStudentNumbersLessThanTlevelStudents(bool zeroStudentNumbers = false)
        {
            if (!zeroStudentNumbers)
            {
                yield return new TableRowData
                {
                    HeaderRow = true,
                    RowItems = new List<string>
                {
                    "Funded students for level 2 and 3 vocational and technical programmes",
                    "Calculation",
                    "Sub-total"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Eligible students from October 2020 return (R14)",
                    string.Empty,
                    "91"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Estimated T Level students",
                    "Minus",
                    "92"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Total funded students for level 2 and 3 vocational and technical programmes",
                    "Equals",
                    "0"
                }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    HeaderRow = true,
                    RowItems = new List<string>
                {
                    "Capacity and delivery fund for level 2 and 3 vocational and technical programmes",
                    "Calculation",
                    "Sub-total"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Eligible students from October 2020 return (R14)",
                    string.Empty,
                    "0"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Estimated T Level students",
                    "Minus",
                    "0"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Rate",
                        "Multiply",
                        "250"
                    }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                    {
                        "Capacity and delivery fund for level 2 and 3 vocational and technical programmes",
                        "Equals",
                        "£0"
                    }
                };
            }
        }

        private static IEnumerable<TableRowData> GetTLevelsTableRowsForEligibleStudentNumbersLessThanTlevelStudents()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "T Level",
                    "Estimated students",
                    "Rate",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "T Levels",
                    "92",
                    "£275",
                    "£19,110"
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
                    "Funded places in 2021 to 2022 allocation",
                    "Calculation",
                    "16 to 19 students",
                    "19 to 24 students"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Student numbers 2020 to 2021 (R06)",
                    string.Empty,
                    "141",
                    "72"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "All high needs students (R06)",
                    "divide",
                    "213",
                    "213"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Proportion",
                    "equals",
                    "0.66",
                    "0.34"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "All funded students 2021 to 2022",
                    "Multiply",
                    "217",
                    "217"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total funded students 2021 to 2022",
                    "equals",
                    "143",
                    "74"
                }
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
                    "16 to 19 students",
                    "143",
                    "£6,000",
                    "£858,000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "19 to 24 students",
                    "74",
                    "£6,000",
                    "£444,000"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total high needs",
                    "217",
                    " ",
                    "£1,302,000"
                }
            };
        }

        private static IEnumerable<TableRowData> GetTotalStudentFundingTableRows(bool forNonZeroAdjustment = false)
        {
            if (!forNonZeroAdjustment)
            {
                yield return new TableRowData
                {
                    HeaderRow = true,
                    RowItems = new List<string>
                {
                    "Funding for students not meeting condition in excess of 5% of total student funding",
                    "Sub-Total"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Funding for students not meeting condition",
                    "£279,293"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "5% of total student funding",
                    "£947,361"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Funding for students not meeting condition in excess of 5% of total student funding",
                    "£0"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Final condition of funding adjustment (at 50%)\r\nLess than 5% of total student funding do not meet condition of funding. No adjustment needed.",
                    "£0"
                }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    HeaderRow = true,
                    RowItems = new List<string>
                {
                    "Funding for students not meeting condition in excess of 5% of total student funding",
                    "Sub-Total"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Funding for students not meeting condition",
                    "£1,279,293"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "5% of total student funding",
                    "£1,947,361"
                }
                };

                yield return new TableRowData
                {
                    RowItems = new List<string>
                {
                    "Funding for students not meeting condition in excess of 5% of total student funding",
                    "£10"
                }
                };
            }
        }

        private static IEnumerable<TableRowData> GetfundingDeductionTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
            {
                "Maths and English condition of funding deduction",
                "Calculation",
                "Sub-Total"
            }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
            {
                "Funding for students not meeting condition in excess of 5% of total student funding...",
                string.Empty,
                "£10"
            }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
            {
                "...is reduced by 50%",
                "Multiply",
                "0.5"
            }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
            {
                "Total Maths and English condition of funding deduction",
                "equals",
                "£5"
            }
            };
        }

        private static IEnumerable<TableRowData> GetHighValuesCoursesBreakdownRows(bool withCensusData)
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "High value courses premium",
                    withCensusData ? "Eligible students Autumn census return" : "Eligible students October 2020 return (R14)",
                    "Rate",
                    "Sub-total"
                }
            };

            if (withCensusData)
            {
                yield return new TableRowData
                {
                    RowItems = new[]
                    {
                        "Total to grow students studying substantial level 3 programmes",
                        "804",
                        "£400",
                        "£321,600"
                    }
                };
            }
            else
            {
                yield return new TableRowData
                {
                    RowItems = new[]
                    {
                        "Total to grow students studying substantial level 3 programmes",
                        "382",
                        "£400",
                        "£152,800"
                    }
                };
            }
        }

        private static IEnumerable<TableRowData> GetAdvancedMathsPremiumBreakdownRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Funded students",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Eligible students 2021 to 2022",
                    string.Empty,
                    "1042"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Average of eligible students 2015 to 2016 and 2016 to 2017",
                    "Minus",
                    "849"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Rate",
                    "Multiply",
                    "£600"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Total advanced maths premium",
                    "Equals",
                    "£115,800"
                }
            };
        }

        private static IEnumerable<TableRowData> GetAdvancedMathsPremiumBreakdownRowsWithEligableStudentsBelowBaseLine()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new[]
                {
                    "Funded students",
                    "Calculation",
                    "Sub-total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Eligible students 2021 to 2022",
                    string.Empty,
                    "68"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Average of eligible students 2015 to 2016 and 2016 to 2017",
                    "Minus",
                    "79"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Funded students",
                    "Equals",
                    "0"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Rate",
                    "Multiply",
                    "£600"
                }
            };

            yield return new TableRowData
            {
                RowItems = new[]
                {
                    "Sub total",
                    "Equals",
                    "£0"
                }
            };
        }

        private static IEnumerable<TableRowData> GetSportingExcellenceTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Diploma in sporting excellence",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total for diploma in sporting excellence",
                    "£34,034"
                }
            };
        }

        private static IEnumerable<TableRowData> GetSeaFishingTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "Sea fishing course",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total for sea fishing course",
                    "£38,034"
                }
            };
        }

        private static IEnumerable<TableRowData> GetTuitionFundTableRows()
        {
            yield return new TableRowData
            {
                HeaderRow = true,
                RowItems = new List<string>
                {
                    "16 to 19 tuition fund",
                    "Sub-Total"
                }
            };

            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "16 to 19 tuition fund",
                    "£2,000"
                }
            };
        }

        #endregion
    }
}