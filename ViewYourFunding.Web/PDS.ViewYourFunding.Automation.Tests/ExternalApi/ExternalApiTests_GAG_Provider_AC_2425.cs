using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Tests.ExternalApi;
using System.Threading.Tasks;


namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    public class ExternalApiTests_GAG_Provider_AC_2425 : BaseExternalApiTests
    {
        private const string CurrentClassName = nameof(ExternalApiTests_GAG_Provider_AC_2425);
        private const string LayoutId = "5b2b22c4-1a97-4b0d-a0ba-ee1ff56e15a6";
        private const string Schema = "GAG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider_AC_2425.json";
        private const string DefaultFundingPeriodCode = "AC-2425";
        private const string DefaultFundingStreamCode = "GAG";

        [ClassInitialize]
        public static void InitalizeTest(TestContext testContext)
        {
            DeleteUriCsv(CurrentClassName);
        }


        [DataRow("html_GAG_MainstreamExisting_WithSparsity_Standard", "GAG-AC-2425-10039385-1_0")]
        [DataRow("html_GAG_MainstreamExisting_WithSparsity_Adjusted", "GAG-AC-2425-10040622-1_0")]
        [DataRow("html_GAG_Special_IYO_PostApril_Year2", "GAG-AC-2425-10007135-1_0")]
        [DataRow("html_GAG_Static_IYO_Special_PostSept_SecondYear", "GAG-AC-2223-10086304-1_0")]
        [DataRow("html_GAG_Mainstream_IYO_PostApril", "GAG-AC-2324-10086304-2_0")]
        [DataRow("html_GAG_Mainstream_IYO_PostSept", "GAG-AC-2425-10045706-1_0")]
        [DataRow("html_GAG_Mainstream_IYO_PostSept_WithDedelgation", "GAG-AC-2425-10034574-1_0")]

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