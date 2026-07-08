using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Tests.ExternalApi;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass, TestCategory("Regression"), TestCategory("CoreRegression")]
    public class ExternalApiTests_1619_AS_2425 : BaseExternalApiTests
    {
        private const string CurrentClassName = nameof(ExternalApiTests_1619_AS_2425);
        private const string GeneralLayoutId = "bc484d1b-837c-45c8-909e-c72412b24f81";
        private const string GeneralSchema = "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider_AS_2425.json";
        private const string IndicativeLayoutId = "676fdd18-51fb-4e73-80ef-a5e258f4c7b0";
        private const string IndicativeSchema = "1619_Indicative_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider_AS_2425.json";
        private const string FundingStreamCode = "1619";
        private const string FundingPeriod = "AS-2425";

        private const string SixthFormLayoutId = "4bb8e1ba-0a57-4fbb-97ab-4faef2340f4f";
        private const string SixthFormSchema = "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInLA_SixthForm_AS_2425.json";
        private const string SixthFormMSSLayoutId = "f500c762-5c38-4c08-a04d-a12f1eb8fb79";
        private const string SixthFormMSSSchema = "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInLA_SixthForm_MSS_Only_AS_2425.json";

        [ClassInitialize]
        public static void InitalizeTest(TestContext testContext)
        {
            DeleteUriCsv(CurrentClassName);
        }

        [DataRow("html_1619_FE_with", "1619-AS-2122-10007063-2_0")]
        [DataRow("html_1619_FE_without", "1619-AS-2122-10000552-2_0")]
        [DataRow("html_1619_Academies_without", "1619-AS-2122-10030654-2_0")]
        [DataRow("html_1619_Academies_with", "1619-AS-2122-10034690-2_0")]
        [DataRow("html_1619_Academies_with_PhasedClosure", "1619-AS-2122-10034690-3_0")]
        [DataRow("html_1619_TuitionFunding_With_MathsTop", "1619-AS-2122-10047244-2_0")]
        [DataRow("html_1619_TuitionFunding_Without_MathsTop", "1619-AS-2122-10064744-2_0")]
        [DataRow("html_1619_AcademyConverterInYearOpener", "1619-AS-2324-10088096-2_0")]
        [DataRow("html_1619_SpecialPost16", "1619-AS-2122-10001929-2_0")]
        [DataRow("html_1619_SpecialAcademies", "1619-AS-2122-10004756-2_0")]
        [DataRow("html_1619_SixthForm_without", "1619-AS-2122-10000866-2_0")]
        [DataRow("html_1619_SixthForm_withFreeMealsLine", "1619-AS-2122-10040630-2_0")]
        [DataRow("html_1619_SixthForm_withHighValueCoursesForSchoolLeavers_And_SUP_AND_POG", "1619-FY-2021-10040631-1_0")]
        [DataRow("html_1619_SixthForm_with", "1619-AS-2122-10006247-2_0")]
        [DataRow("html_1619_SixthForm_No_HighValueCoursesForSchoolAndCollegeLeavers_Post2122", "1619-AS-2223-10040631-1_0")]
        [DataRow("html_1619_WithDisadvantagedFunding_2223", "1619-AS-2223-10088096-2_0")]
        [DataRow("html_1619_Adjustments_Show_None_0", "1619-AS-2324-10088092-2_0")]
        [DataRow("html_1619_Adjustments_Show_All", "1619-AS-2324-10088092-3_0")]
        [DataRow("html_1619_Adjustments_Show_None_null", "1619-AS-2324-10088092-4_0")]
        [DataRow("html_1619_IndustryPlacements_NonSpecialAcca_HasValues", "1619-AS-2324-10088092-5_0")]
        [DataRow("html_1619_IndustryPlacements_NonSpecialAcca_HasZero", "1619-AS-2324-10088092-6_0")]
        [DataRow("html_1619_IndustryPlacements_SpecialAcca_HasValues", "1619-AS-2324-10088092-7_0")]
        [DataRow("html_1619_IndustryPlacements_SpecialAcca_HasZero", "1619-AS-2324-10088092-8_0")]
        [DataRow("html_1619_IndustryPlacements_BringTableUp_MathsTopUpAndTuitionFunding_Pos", "1619-AS-2324-10088092-9_0")]
        [DataRow("html_1619_IndustryPlacements_BringTableUp_MathsTopUpAndTuitionFunding_Neg1", "1619-AS-2324-10088092-10_0")]
        [DataRow("html_1619_IndustryPlacements_BringTableUp_MathsTopUpAndTuitionFunding_Neg2", "1619-AS-2324-10088092-11_0")]
        [DataRow("html_1619_Adjustments_2425_TBandVCShow", "1619-AS-2425-10088092-11_0")]
        [DataRow("html_1619_Adjustments_2425_TBandVCHide", "1619-AS-2425-10088092-12_0")]
        [DataRow("html_1619_Adjustments_2425_IndPlacementFundingTblHide", "1619-AS-2425-10088092-1_0")]
        [DataRow("html_1619_Adjustments_2425_IndPlacementFundingTblShow", "1619-AS-2425-10088092-2_0")]
        [DataRow("html_1619_Main_Adjustments_2425_NS_SET2AllShow", "1619-AS-2425-99999002-1_0")]
        [DataRow("html_1619_IYO_Adjustments_2425_NS_SET2AllShow", "1619-AS-2425-99999003-1_0")]
        [DataRow("html_1619_Main_Adjustments_2425_S_SET2AllShow", "1619-AS-2425-99999004-1_0")]
        [DataRow("html_1619_IYO_Adjustments_2425_S_SET2AllShow", "1619-AS-2425-99999005-1_0")]
        [DataRow("html_1619_Main_Adjustments_2324_SummaryTableBugFixTesting", "1619-AS-2425-99999006-1_0")]
        [DataRow("html_1619_IYO_Adjustments_2324_SummaryTableBugFixTesting", "1619-AS-2425-99999007-1_0")]
        [DataRow("html_1619_Static_2425_Residential_Support_Scheme", "1619-AS-2425-99999008-1_0")]
        [TestMethod]
        public async Task General_ExternalApi_RenderHtml(string expectedHtmlFileName, string providerFundingId)
        {
            // Arrange
            var strExpectedHtml = await GetExpectedHtmlAsync(CurrentClassName, expectedHtmlFileName);

            // Act
            var actualHtml = await GetHtml(
                providerFundingId,
                GeneralLayoutId,
                GeneralSchema,
                FundingPeriod,
                FundingStreamCode);

            //Please set CanWriteExpectedHtml appsettings.json as true if you want to rewrite the Regression testing results in output folder.
            if (CanWriteExpectedHtml)
            {
                await WriteExpectedHtmlAsync(
                            CurrentClassName,
                            expectedHtmlFileName,
                            actualHtml,
                            providerFundingId,
                            GeneralLayoutId,
                            GeneralSchema,
                            FundingPeriod,
                            FundingStreamCode);
            }

            // Assert
            actualHtml.Should().Be(strExpectedHtml);
        }

        [DataRow("html_1619_IndicativeStatement", "1619-AS-2122-10007063-2_0")]
        [DataRow("html_Provider_Indicative_WithoutHighValueCourses_2223", "1619-AS-2122-10088092-2_0")]
        [DataRow("html_1619_Provider_Indicative_WithHighValueCourses", "1619-AS-2122-10088092-2_0")]
        [DataRow("html_1619_Indicative_2425_WithTLevelIndPlacements", "1619-AS-2425-99999001-1_0")]
        [TestMethod]
        public async Task Indicative_ExternalApi_RenderHtml(string expectedHtmlFileName, string providerFundingId)
        {
            // Arrange
            var strExpectedHtml = await GetExpectedHtmlAsync(CurrentClassName, expectedHtmlFileName);

            // Act
            var actualHtml = await GetHtml(
                providerFundingId,
                IndicativeLayoutId,
                IndicativeSchema,
                FundingPeriod,
                FundingStreamCode);

            //Please set CanWriteExpectedHtml appsettings.json as true if you want to rewrite the Regression testing results in output folder.
            if (CanWriteExpectedHtml)
            {
                await WriteExpectedHtmlAsync(
                                CurrentClassName,
                                expectedHtmlFileName,
                                actualHtml,
                                providerFundingId,
                                IndicativeLayoutId,
                                IndicativeSchema,
                                FundingPeriod,
                                FundingStreamCode);
            }

            // Assert
            actualHtml.Should().Be(strExpectedHtml);
        }

        [DataRow("html_1619_LA_SixthForm", "1619-AS-2122-Contracting-LocalAuthoritySsf-10001464-1_0")]
        [TestMethod]
        public async Task SixthForm_ExternalApi_RenderHtml(string expectedHtmlFileName, string providerFundingId)
        {
            // Arrange
            var strExpectedHtml = await GetExpectedHtmlAsync(CurrentClassName, expectedHtmlFileName);

            // Act
            var actualHtml = await GetHtml(
                providerFundingId,
                SixthFormLayoutId,
                SixthFormSchema,
                FundingPeriod,
                FundingStreamCode);

            //Please set CanWriteExpectedHtml appsettings.json as true if you want to rewrite the Regression testing results in output folder.
            if (CanWriteExpectedHtml)
            {
                await WriteExpectedHtmlAsync(
                            CurrentClassName,
                            expectedHtmlFileName,
                            actualHtml,
                            providerFundingId,
                            SixthFormLayoutId,
                            SixthFormSchema,
                            FundingPeriod,
                            FundingStreamCode);
            }

            // Assert
            actualHtml.Should().Be(strExpectedHtml);
        }

        [DataRow("html_1619_SixthForm_Mss", "1619-AS-2122-Contracting-LocalAuthority-10001464-1_0")]
        [TestMethod]
        public async Task SixthFormMSS_ExternalApi_RenderHtml(string expectedHtmlFileName, string providerFundingId)
        {
            // Arrange
            var strExpectedHtml = await GetExpectedHtmlAsync(CurrentClassName, expectedHtmlFileName);

            // Act
            var actualHtml = await GetHtml(
                providerFundingId,
                SixthFormMSSLayoutId,
                SixthFormMSSSchema,
                FundingPeriod,
                FundingStreamCode);

            //Please set CanWriteExpectedHtml appsettings.json as true if you want to rewrite the Regression testing results in output folder.
            if (CanWriteExpectedHtml)
            {
                await WriteExpectedHtmlAsync(
                            CurrentClassName,
                            expectedHtmlFileName,
                            actualHtml,
                            providerFundingId,
                            SixthFormMSSLayoutId,
                            SixthFormMSSSchema,
                            FundingPeriod,
                            FundingStreamCode);
            }

            // Assert
            actualHtml.Should().Be(strExpectedHtml);
        }
    }
}