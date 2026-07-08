using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Tests.ExternalApi;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    public class ExternalApiTests_NMSS_AY_2526 : BaseExternalApiTests
    {
        private const string CurrentClassName = nameof(ExternalApiTests_NMSS_AY_2526);
        private const string LayoutId = "db8a83e2-a08c-4872-b25b-d551ef7b2956";
        private const string DefaultFundingPeriodCode = "AY-2526";
        private const string DefaultFundingStreamCode = "NMSS";
        private const string Schema = "NMSS_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider_AY_2526.json";

        [ClassInitialize]
        public static void InitalizeTest(TestContext testContext)
        {
            DeleteUriCsv(CurrentClassName);
        }

        [DataRow("html_NMSS_No_AgreedChangeToHighNeedsPlaces", "NMSS-AC-2122-10000552-2_0")]
        [DataRow("html_NMSS_With_AgreedChangeToHighNeedsPlaces", "NMSS-AY-2223-10034690-2_0")]
        [DataRow("html_NMSS_With_Post_Pre16PupilProportions_AS_NA", "NMSS-AY-2223-10040631-1_0")]
        [DataRow("html_NMSS_With_BaselineForTransition", "NMSS-AY-2324-10034690-2_0")]
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
            actualHtml.Should().Be(strExpectedHtml);
        }
    }
}