using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Tests.ExternalApi;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    public class ExternalApiTests_GAG_Indicative_AC_2324 : BaseExternalApiTests
    {
        private const string CurrentClassName = nameof(ExternalApiTests_GAG_Indicative_AC_2324);
        private const string LayoutId = "0b458fac-623c-4183-9f1c-c7e8e4a1340b";
        private const string Schema = "GAG_Indicative_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider_AC_2324.json";
        private const string DefaultFundingPeriodCode = "AC-2324";
        private const string DefaultFundingStreamCode = "GAG";

        [ClassInitialize]
        public static void InitalizeTest(TestContext testContext)
        {
            DeleteUriCsv(CurrentClassName);
        }


        [DataRow("html_GAG_SpecialExisting", "GAG-FY-2021-10045843-1_0")]
        [DataRow("html_GAG_Special_IYO_PostApril", "GAG-FY-2021-10042434-2_0")]
        [DataRow("html_GAG_Special_IYO_PostSept", "GAG-FY-2021-10048870-1_0")]
        [DataRow("html_GAG_MainstreamExisting", "GAG-FY-2021-10034949-1_0")]
        [DataRow("html_GAG_MainstreamExisting_WithSparsity", "GAG-FY-2021-10043073-1_0")]
        [DataRow("html_GAG_Mainstream_IYO_PostApril", "GAG-FY-2021-10086304-2_0")]
        [DataRow("html_GAG_Mainstream_IYO_PostSept", "GAG-FY-2021-10045706-1_0")]
        [DataRow("html_GAG_Mainstream_IYO_PostSept_WithDedelgation", "GAG-FY-2021-10034574-2_0")]
        [DataRow("html_GAG_Mainstream_IYO_PostApril_WithDedelgation", "GAG-FY-2021-10040626-1_0")]

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public async Task General_ExternalApi_RenderHtml(string expectedHtmlFileName, string providerFundingId)
        {
            // Arrange
            var strExpectedHtml = await GetExpectedHtmlAsync(CurrentClassName, expectedHtmlFileName);

            // Act
            var actualHtml = await GetHtml(
                providerFundingId,
                LayoutId,
                Schema,
                DefaultFundingPeriodCode,
                DefaultFundingStreamCode);

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
                                DefaultFundingPeriodCode,
                                DefaultFundingStreamCode);
            }

            // Assert
            actualHtml.Should().Be(ReplaceCurrentDate(strExpectedHtml));
        }
    }
}