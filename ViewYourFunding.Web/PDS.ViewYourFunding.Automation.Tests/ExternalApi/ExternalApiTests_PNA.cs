using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Tests.ExternalApi;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass, TestCategory("Regression"), TestCategory("CoreRegression")]
    public class ExternalApiTests_PNA : BaseExternalApiTests
    {
        private const string CurrentClassName = nameof(ExternalApiTests_PNA);
        private const string LayoutId = "5ed12204-857a-4ead-8b28-adff4999011b";
        private const string Schema = "PNA_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json";
        private const string FundingStreamCode = "PNA";
        private const string FundingPeriod = "AC-2122";

        [ClassInitialize]
        public static void InitalizeTest(TestContext testContext)
        {
            DeleteUriCsv(CurrentClassName);
        }

        [DataRow("html_pna_standard", "PNA-AC-2122-10072811-1_0")]
        [DataRow("html_pna_dual_census_no_threshold", "PNA-AC-2122-10072812-1_0")]
        [DataRow("html_pna_dual_census_using_threshold", "PNA-AC-2122-10072814-1_0")]
        [DataRow("html_pna_threshold_used", "PNA-AC-2122-10072815-1_0")]
        [DataRow("html_pna_guaranteed_pupil_numbers", "PNA-AC-2122-10072816-1_0")]
        [TestMethod]
        public async Task General_ExternalApi_RenderHtml(string expectedHtmlFileName, string providerFundingId)
        {
            // Arrange
            var strExpectedHtml = await GetExpectedHtmlAsync(CurrentClassName, expectedHtmlFileName);

            // Act
            var actualHtml = await GetHtml(
                providerFundingId,
                LayoutId,
                Schema,
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
                            LayoutId,
                            Schema,
                            FundingPeriod,
                            FundingStreamCode);
            }

            // Assert
            actualHtml.Should().Be(strExpectedHtml);
        }
    }
}