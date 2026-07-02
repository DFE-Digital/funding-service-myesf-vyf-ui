using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    [TestClass, TestCategory("Unit")]
    public class SchemaVersionHelperTests
    {
        [TestMethod]
        [DataRow("1.0", ValidSchema1Point0Data, 1.0)]
        [DataRow("1.0", ValidSchema1Point1Data, 1.1)]
        [DataRow("1.1", ValidSchema1Point2Data, 1.2)]
        [DataRow("1.1", ValidSchema1Point1Data, 1.1)]
        [DataRow("1.2", ValidSchema1Point2Data, 1.2)]
        [DataRow("1.2", ValidSchema1Point1Data, 1.1)]
        public void GetFundingValueDataValidatedSchemaVersion_ReturnsExpectedResult(
            string dataSchemaVersion,
            string fundingValue,
            double expectedValue)
        {
            // Arrange/Act
            var searchResult = new FundingApiSearchFunding
            {
                SchemaVersion = dataSchemaVersion,
                FundingValue = fundingValue
            };
            var result = searchResult.GetFundingValueDataValidatedSchemaVersion();

            // Assert
            result.Should().Be(expectedValue);
        }

        private const string ValidSchema1Point2Data =
            @"{
	    ""totalValue"": 971832.37,
	    ""fundingLines"": [
		    {
			""name"": ""School Allocation Block with Notional SEN and DeDelegation"",
			""fundingLineCode"": null,
			""value"": 1052661.96,
			""templateLineId"": 0,
			""type"": ""Information"",
			""distributionPeriods"": null
		}]}";

        private const string ValidSchema1Point1Data =
            @"{
        ""totalValue"": 21280,
        ""calculations"": {
            ""2"": {
                ""name"": ""Total Allocation"",
                ""type"": ""Cash"",
                ""aggregationType"": ""None"",
                ""formulaText"": ""Something * something"",
                ""templateCalculationId"": 2,
                ""value"": 21280,
                ""valueFormat"": ""Currency""
            }}}";

        private const string ValidSchema1Point0Data =
            @"{
	    ""fundingValue"": {
		""totalValue"": 16850,
		""fundingLines"": [
			{
				""name"": ""Total Allocation"",
				""fundingLineCode"": ""TotalAllocation"",
				""value"": 16850,
				""templateLineId"": 1,
				""type"": ""Payment"",
				""calculations"": [
					{
						""name"": ""Total Allocation"",
						""templateCalculationId"": 2,
						""value"": ""16850"",
						""valueFormat"": ""Currency"",
						""type"": ""Cash"",
						""formulaText"": ""Something * something"",
						""aggregationType"": ""None"",
						""calculations"": null,
						""referenceData"": null
					}
				]
			}
		]
	    }}";
    }
}