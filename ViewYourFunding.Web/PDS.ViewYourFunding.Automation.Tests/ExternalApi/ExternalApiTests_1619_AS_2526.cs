using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Tests.ExternalApi;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass, TestCategory("Regression"), TestCategory("CoreRegression")]
    public class ExternalApiTests_1619_AS_2526 : BaseExternalApiTests
    {
        private const string CurrentClassName = nameof(ExternalApiTests_1619_AS_2526);
        private const string GeneralLayoutId = "615c25bb-a221-4b7a-845c-1e6b7374c413";
        private const string GeneralSchema = "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider_AS_2526.json";
        private const string StudentNumberLayoutId = "45ffe00c-ee4b-47af-b552-5157af8a78db";
        private const string StudentNumberSchema = "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInLA_StudentNumber_2526.json";
        private const string SixthFormLayoutId = "2d551507-a4cf-4c9a-a16c-b57746f7e29a";
        private const string SixthFormSchema = "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInLA_SixthForm_AS_2526.json";
        private const string SixthFormMSSLayoutId = "5f8e8621-8ffc-46ad-b3f5-688889a1757f";
        private const string SixthFormMSSSchema = "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInLA_SixthForm_MSS_Only_AS_2526.json";
        private const string IndicativeLayoutId = "47c9dd56-a6fa-4615-a3e5-654e8431e15a";
        private const string IndicativeSchema = "1619_Indicative_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider_AS_2526.json";
        private const string FundingStreamCode = "1619";
        private const string FundingPeriod = "AS-2526";

        [ClassInitialize]
        public static void InitalizeTest(TestContext testContext)
        {
            DeleteUriCsv(CurrentClassName);
        }

        [DataRow("html_1619_WithDisadvantagedFunding_2223", "1619-AS-2223-10088096-2_0")]
        [DataRow("html_1619_IndustryPlacements_NonSpecialAcca_HasValues", "1619-AS-2324-10088092-5_0")]
        [DataRow("html_1619_2526_Increment1_ShowAll", "1619-AS-2526-10007063-1_0")]
        [DataRow("html_1619_2526_Increment1_HideAll", "1619-AS-2526-10007063-2_0")]
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

        [DataRow("html_1619_Student_Numbers_2526", "1619-AS-2122-Information-LocalAuthority-341-1_0")]
        [TestMethod]
        public async Task StudentNumber_ExternalApi_RenderHtml(string expectedHtmlFileName, string providerFundingId)
        {
            // Arrange
            var strExpectedHtml = await GetExpectedHtmlAsync(CurrentClassName, expectedHtmlFileName);

            // Act
            var actualHtml = await GetHtml(
                providerFundingId,
                StudentNumberLayoutId,
                StudentNumberSchema,
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

        [DataRow("html_1619_LA_SixthForm_2526", "1619-AS-2122-Contracting-LocalAuthoritySsf-10001464-1_0")]
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
                            GeneralLayoutId,
                            GeneralSchema,
                            FundingPeriod,
                            FundingStreamCode);
            }

            // Assert
            actualHtml.Should().Be(strExpectedHtml);
        }

        [DataRow("html_1619_IndicativeStatement", "1619-AS-2122-10028144-2_0")]
        [DataRow("html_Provider_Indicative_WithoutHighValueCourses_2223", "1619-AS-2122-10089100-2_0")]
        [DataRow("html_1619_Provider_Indicative_WithHighValueCourses", "1619-AS-2122-10089100-2_0")]
        [DataRow("html_1619_Indicative_2526_WithTLevelIndPlacements", "1619-AS-2425-10095890-1_0")]
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

        [DataRow("html_1619_SixthForm_Mss_2526", "1619-AS-2122-Contracting-LocalAuthority-10001464-1_0")]
        [TestMethod]
        public async Task SixthForm_MSS_ExternalApi_RenderHtml(string expectedHtmlFileName, string providerFundingId)
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
                            GeneralLayoutId,
                            GeneralSchema,
                            FundingPeriod,
                            FundingStreamCode);
            }

            // Assert
            actualHtml.Should().Be(strExpectedHtml);
        }
    }
}