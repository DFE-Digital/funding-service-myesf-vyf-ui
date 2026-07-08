using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Tests.ExternalApi;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass, TestCategory("Regression"), TestCategory("CoreRegression")]
    public class ExternalApiTests_1416_AY_2526 : BaseExternalApiTests
    {
        private const string CurrentClassName = nameof(ExternalApiTests_1416_AY_2526);
        private const string GeneralLayoutId = "82dec9cd-8d91-40c8-bcbc-c4d3cda5c33b";
        private const string GeneralSchema = "1416_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider_AY_2526.json";
        private const string FundingStreamCode = "1416";
        private const string FundingPeriod = "AY-2526";

        [ClassInitialize]
        public static void InitalizeTest(TestContext testContext)
        {
            DeleteUriCsv(CurrentClassName);
        }

        [DataRow("html_1416_Static_Summary_English_and_Maths_funding", "1416-AY-2425-10072811-1_0")]
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
    }
}