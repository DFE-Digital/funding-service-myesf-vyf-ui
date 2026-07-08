using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Tests.ExternalApi;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass, TestCategory("Regression"), TestCategory("CoreRegression")]
    public class ExternalApiTests_1619_AS_2627 : BaseExternalApiTests
    {
        private const string CurrentClassName = nameof(ExternalApiTests_1619_AS_2627);
        private const string GeneralLayoutId = "d2616a1c-eb3b-4db9-b897-150f2edd685d";
        private const string GeneralSchema = "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider_AS_2627.json";
        private const string IndicativeLayoutId = "4db1f1e5-1219-4bb3-bcf5-40df22a30299";
        private const string IndicativeSchema = "1619_Indicative_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider_AS_2627.json";
        private const string FundingStreamCode = "1619";
        private const string FundingPeriod = "AS-2627";

        [ClassInitialize]
        public static void InitalizeTest(TestContext testContext)
        {
            DeleteUriCsv(CurrentClassName);
        }

        [DataRow("html_1619_WithDisadvantagedFunding_2223", "1619-AS-2223-10088096-2_0")]
        [DataRow("html_1619_IndustryPlacements_NonSpecialAcca_HasValues", "1619-AS-2324-10088092-5_0")]
        [DataRow("html_1619_2526_Increment1_ShowAll", "1619-AS-2526-10007063-1_0")]
        [DataRow("html_1619_2526_Increment1_HideAll", "1619-AS-2526-10007063-2_0")]
        [DataRow("html_1619_Static_PolicyChanges_2627_ShowAll", "1619-AS-2627-10007063-1_0")]
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
    }
}