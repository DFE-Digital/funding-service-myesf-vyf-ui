using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    [TestClass, TestCategory("Unit")]
    public class UIFSMNewOpenerAndFinalAllocationFiltersHelperTests
    {
        private const string SHEMA1Point0 = "1.0";
        private const string SHEMA1Point1 = "1.1";
        private const string SHEMA1Point2 = "1.2";

        [TestMethod]
        [DataRow(SHEMA1Point0, NewOpenerConstants.ValidSchema1Point0.DataPostiveAmount, true)]
        [DataRow(SHEMA1Point0, NewOpenerConstants.ValidSchema1Point0.DataNegativeAmount, false)]
        [DataRow(SHEMA1Point0, NewOpenerConstants.ValidSchema1Point0.DataZeroAmount, false)]
        [DataRow(SHEMA1Point0, NewOpenerConstants.ValidSchema1Point0.DataNoCalcID39, false)]
        [DataRow(SHEMA1Point1, NewOpenerConstants.ValidSchema1Point1.DataNegativeAmount, false)]
        [DataRow(SHEMA1Point1, NewOpenerConstants.ValidSchema1Point1.DataZeroAmount, false)]
        [DataRow(SHEMA1Point1, NewOpenerConstants.ValidSchema1Point1.DataNoCalcID39, false)]
        [DataRow(SHEMA1Point2, NewOpenerConstants.ValidSchema1Point2.DataPostiveAmount, true)]
        [DataRow(SHEMA1Point2, NewOpenerConstants.ValidSchema1Point2.DataNegativeAmount, false)]
        [DataRow(SHEMA1Point2, NewOpenerConstants.ValidSchema1Point2.DataZeroAmount, false)]
        [DataRow(SHEMA1Point2, NewOpenerConstants.ValidSchema1Point2.DataNoCalcID39, false)]
        [DataRow(SHEMA1Point2, NewOpenerConstants.ValidSchema1Point2.DataNoCalculations, false)]
        public void CheckProviderHasNewOpenerProvisionalAllocation_ReturnsExpectedResult(string dataSchemaVersion, string fundingValue, bool expectedValue)
        {
            // Arrange
            FundingApiSearchProviderFunding providerFunding = new FundingApiSearchProviderFunding()
            {
                FundingValue = fundingValue,
                SchemaVersion = dataSchemaVersion
            };

            //Act
            var result = providerFunding.CheckProviderHasNewOpenerProvisionalAllocation();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        [DataRow(SHEMA1Point0, FinalAllocationConstants.ValidSchema1Point0.C18P_C45P, true)]
        [DataRow(SHEMA1Point0, FinalAllocationConstants.ValidSchema1Point0.C18P_C45N_TotalNonZero, true)]
        [DataRow(SHEMA1Point0, FinalAllocationConstants.ValidSchema1Point0.C18P_C45N_TotalZero, false)]
        [DataRow(SHEMA1Point0, FinalAllocationConstants.ValidSchema1Point0.C18Z_C45Z, false)]
        [DataRow(SHEMA1Point0, FinalAllocationConstants.ValidSchema1Point0.C18Null_C45Null, false)]
        [DataRow(SHEMA1Point0, FinalAllocationConstants.ValidSchema1Point0.C18Z_C45P, true)]
        [DataRow(SHEMA1Point0, FinalAllocationConstants.ValidSchema1Point0.C18N_C45Null, true)]
        [DataRow(SHEMA1Point1, FinalAllocationConstants.ValidSchema1Point1.C18P_C45P, true)]
        [DataRow(SHEMA1Point1, FinalAllocationConstants.ValidSchema1Point1.C18N_C45N, true)]
        [DataRow(SHEMA1Point1, FinalAllocationConstants.ValidSchema1Point1.C18N_C45P_TotalPos, true)]
        [DataRow(SHEMA1Point1, FinalAllocationConstants.ValidSchema1Point1.C18N_C45P_TotalNeg, true)]
        [DataRow(SHEMA1Point1, FinalAllocationConstants.ValidSchema1Point1.C18N_C45P_TotalZero, false)]
        [DataRow(SHEMA1Point1, FinalAllocationConstants.ValidSchema1Point1.C18N_C45Z, true)]
        [DataRow(SHEMA1Point1, FinalAllocationConstants.ValidSchema1Point1.C18NULL_C45P, true)]
        [DataRow(SHEMA1Point2, FinalAllocationConstants.ValidSchema1Point2.C18P_C45N_TotalNonZero, true)]
        [DataRow(SHEMA1Point2, FinalAllocationConstants.ValidSchema1Point2.C18N_C45N, true)]
        [DataRow(SHEMA1Point2, FinalAllocationConstants.ValidSchema1Point2.C18N_C45P_TotalZero, false)]
        [DataRow(SHEMA1Point2, FinalAllocationConstants.ValidSchema1Point2.C18NULL_C45P, true)]
        [DataRow(SHEMA1Point2, FinalAllocationConstants.ValidSchema1Point2.C18N_C45Zero, true)]
        [DataRow(SHEMA1Point2, FinalAllocationConstants.ValidSchema1Point2.C18NA_C45P, true)]
        [DataRow(SHEMA1Point2, FinalAllocationConstants.ValidSchema1Point2.C18NA_C45NA, false)]
        [DataRow(SHEMA1Point2, FinalAllocationConstants.ValidSchema1Point2.C18Zero_C45NA, false)]
        public void CheckProviderHasFinalAllocation_ReturnsExpectedResult(string dataSchemaVersion, string fundingValue, bool expectedValue)
        {
            // Arrange
            FundingApiSearchProviderFunding providerFunding = new FundingApiSearchProviderFunding()
            {
                FundingValue = fundingValue,
                SchemaVersion = dataSchemaVersion
            };

            //Act
            var result = providerFunding.CheckProviderHasFinalAllocation();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        [DataRow(SHEMA1Point0, FinalAllocationSchoolTypeConstants.ValidSchema1Point0.C74_HasValue, "Non-Maintained Special School")]
        [DataRow(SHEMA1Point0, FinalAllocationSchoolTypeConstants.ValidSchema1Point0.C74_NULL, "")]
        [DataRow(SHEMA1Point0, FinalAllocationSchoolTypeConstants.ValidSchema1Point0.C74_NA, "")]
        [DataRow(SHEMA1Point1, FinalAllocationSchoolTypeConstants.ValidSchema1Point1.C74_HasValue, "Non-Maintained Special School")]
        [DataRow(SHEMA1Point1, FinalAllocationSchoolTypeConstants.ValidSchema1Point1.C74_NULL, "")]
        [DataRow(SHEMA1Point1, FinalAllocationSchoolTypeConstants.ValidSchema1Point1.C74_NA, "")]
        [DataRow(SHEMA1Point2, FinalAllocationSchoolTypeConstants.ValidSchema1Point2.C74_HasValue, "Non-Maintained Special School")]
        [DataRow(SHEMA1Point2, FinalAllocationSchoolTypeConstants.ValidSchema1Point2.C74_NULL, "")]
        [DataRow(SHEMA1Point2, FinalAllocationSchoolTypeConstants.ValidSchema1Point2.C74_NA, "")]
        public void GetFinalAllocationSchoolType_ReturnsExpectedResult(string dataSchemaVersion, string fundingValue, string expectedValue)
        {
            // Arrange
            FundingApiSearchProviderFunding providerFunding = new FundingApiSearchProviderFunding()
            {
                FundingValue = fundingValue,
                SchemaVersion = dataSchemaVersion
            };

            //Act
            var result = providerFunding.GetFinalAllocationSchoolType();

            // Assert
            result.Should().Be(expectedValue);
        }

        private static class NewOpenerConstants
        {
            internal static class ValidSchema1Point0
            {
                public const string DataPostiveAmount =
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
									""templateCalculationId"": 39,
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

                public const string DataNegativeAmount =
                    @"{
					""fundingValue"": {
						""totalValue"": 16850,
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
											""templateCalculationId"": 39,
											""value"": ""-585"",
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
						}
					}}";

                public const string DataZeroAmount =
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
									""templateCalculationId"": 39,
									""value"": ""0"",
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

                public const string DataNoCalcID39 =
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

            internal static class ValidSchema1Point1
            {
                public const string DataNegativeAmount =
                    @"{
					""totalValue"": 21280,
					""calculations"": {
						""2"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 39,
							""value"": -6000,
							""valueFormat"": ""Currency""
						}}}";

                public const string DataZeroAmount =
                    @"{
					""totalValue"": 21280,
					""calculations"": {
						""2"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 39,
							""value"": 0,
							""valueFormat"": ""Currency""
						}}}";

                public const string DataNoCalcID39 =
                    @"{
					""totalValue"": 21280,
					""calculations"": {
						""2"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 60,
							""value"": 4000,
							""valueFormat"": ""Currency""
						}}}";
            }

            internal static class ValidSchema1Point2
            {
                public const string DataPostiveAmount =
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
					}],
					""calculations"": [
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 39,
							""value"": ""16850"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						}
					]}";

                public const string DataNegativeAmount =
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
					}],
					""calculations"": [
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 39,
							""value"": ""-4254"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						}
					]}";

                public const string DataZeroAmount =
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
					}],
					""calculations"": [
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 39,
							""value"": ""0"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						}
					]}";

                public const string DataNoCalcID39 =
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
					}],
					""calculations"": [
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 75,
							""value"": ""16850"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						}
					]}";

                public const string DataNoCalculations =
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
            }
        }

        private static class FinalAllocationConstants
        {
            internal static class ValidSchema1Point0
            {
                public const string C18P_C45P =
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
									""templateCalculationId"": 18,
									""value"": ""16850"",
									""valueFormat"": ""Currency"",
									""type"": ""Cash"",
									""formulaText"": ""Something * something"",
									""aggregationType"": ""None"",
									""calculations"": null,
									""referenceData"": null
								},
								{
									""name"": ""Total Allocation"",
									""templateCalculationId"": 45,
									""value"": ""10000"",
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

                public const string C18P_C45N_TotalNonZero =
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
									""templateCalculationId"": 18,
									""value"": ""16850"",
									""valueFormat"": ""Currency"",
									""type"": ""Cash"",
									""formulaText"": ""Something * something"",
									""aggregationType"": ""None"",
									""calculations"": null,
									""referenceData"": null
								},
								{
									""name"": ""Total Allocation"",
									""templateCalculationId"": 45,
									""value"": ""-10000"",
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

                public const string C18P_C45N_TotalZero =
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
									""templateCalculationId"": 18,
									""value"": ""10000"",
									""valueFormat"": ""Currency"",
									""type"": ""Cash"",
									""formulaText"": ""Something * something"",
									""aggregationType"": ""None"",
									""calculations"": null,
									""referenceData"": null
								},
								{
									""name"": ""Total Allocation"",
									""templateCalculationId"": 45,
									""value"": ""-10000"",
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

                public const string C18Z_C45Z =
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
									""templateCalculationId"": 18,
									""value"": ""0"",
									""valueFormat"": ""Currency"",
									""type"": ""Cash"",
									""formulaText"": ""Something * something"",
									""aggregationType"": ""None"",
									""calculations"": null,
									""referenceData"": null
								},
								{
									""name"": ""Total Allocation"",
									""templateCalculationId"": 45,
									""value"": ""0"",
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

                public const string C18Null_C45Null =
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
									""templateCalculationId"": 18,
									""value"": null,
									""valueFormat"": ""Currency"",
									""type"": ""Cash"",
									""formulaText"": ""Something * something"",
									""aggregationType"": ""None"",
									""calculations"": null,
									""referenceData"": null
								},
								{
									""name"": ""Total Allocation"",
									""templateCalculationId"": 45,
									""value"": null,
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

                public const string C18Z_C45P =
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
									""templateCalculationId"": 18,
									""value"": ""0"",
									""valueFormat"": ""Currency"",
									""type"": ""Cash"",
									""formulaText"": ""Something * something"",
									""aggregationType"": ""None"",
									""calculations"": null,
									""referenceData"": null
								},
								{
									""name"": ""Total Allocation"",
									""templateCalculationId"": 45,
									""value"": ""100"",
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

                public const string C18N_C45Null =
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
									""templateCalculationId"": 18,
									""value"": ""-1000"",
									""valueFormat"": ""Currency"",
									""type"": ""Cash"",
									""formulaText"": ""Something * something"",
									""aggregationType"": ""None"",
									""calculations"": null,
									""referenceData"": null
								},
								{
									""name"": ""Total Allocation"",
									""templateCalculationId"": 45,
									""value"": null,
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

            internal static class ValidSchema1Point1
            {
                public const string C18P_C45P =
                    @"{
					""totalValue"": 21280,
					""calculations"": {
						""18"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 18,
							""value"": 6000,
							""valueFormat"": ""Currency""
						},
						""45"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 45,
							""value"": 12000,
							""valueFormat"": ""Currency""
						}}}";

                public const string C18N_C45N =
                    @"{
					""totalValue"": 21280,
					""calculations"": {
						""18"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 18,
							""value"": -6000,
							""valueFormat"": ""Currency""
						},
						""45"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 45,
							""value"": -12000,
							""valueFormat"": ""Currency""
						}}}";

                public const string C18N_C45P_TotalPos =
                    @"{
					""totalValue"": 21280,
					""calculations"": {
						""18"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 18,
							""value"": -6000,
							""valueFormat"": ""Currency""
						},
						""45"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 45,
							""value"": 12000,
							""valueFormat"": ""Currency""
						}}}";

                public const string C18N_C45P_TotalNeg =
                    @"{
					""totalValue"": 21280,
					""calculations"": {
						""18"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 18,
							""value"": -6000,
							""valueFormat"": ""Currency""
						},
						""45"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 45,
							""value"": 3000,
							""valueFormat"": ""Currency""
						}}}";

                public const string C18N_C45P_TotalZero =
                    @"{
					""totalValue"": 21280,
					""calculations"": {
						""18"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 18,
							""value"": -6000,
							""valueFormat"": ""Currency""
						},
						""45"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 45,
							""value"": 6000,
							""valueFormat"": ""Currency""
						}}}";

                public const string C18N_C45Z =
                    @"{
					""totalValue"": 21280,
					""calculations"": {
						""18"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 18,
							""value"": -6000,
							""valueFormat"": ""Currency""
						},
						""45"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 45,
							""value"": 0,
							""valueFormat"": ""Currency""
						}}}";

                public const string C18NULL_C45P =
                    @"{
					""totalValue"": 21280,
					""calculations"": {
						""18"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 18,
							""value"": null,
							""valueFormat"": ""Currency""
						},
						""45"": {
							""name"": ""Total Allocation"",
							""type"": ""Cash"",
							""aggregationType"": ""None"",
							""formulaText"": ""Something * something"",
							""templateCalculationId"": 45,
							""value"": 12000,
							""valueFormat"": ""Currency""
						}}}";
            }

            internal static class ValidSchema1Point2
            {
                public const string C18P_C45P =
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
					}],
					""calculations"": [
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 18,
							""value"": ""16850"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						},
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 45,
							""value"": ""16850"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						}
					]}";

                public const string C18P_C45N_TotalNonZero =
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
					}],
					""calculations"": [
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 18,
							""value"": ""16850"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						},
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 45,
							""value"": ""-1000"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						}
					]}";

                public const string C18N_C45N =
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
					}],
					""calculations"": [
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 18,
							""value"": ""-16850"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						},
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 45,
							""value"": ""-16850"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						}
					]}";

                public const string C18N_C45P_TotalZero =
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
					}],
					""calculations"": [
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 18,
							""value"": ""-16850"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						},
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 45,
							""value"": ""16850"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						}
					]}";

                public const string C18NULL_C45P =
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
					}],
					""calculations"": [
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 18,
							""value"": null,
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						},
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 45,
							""value"": ""16850"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						}
					]}";

                public const string C18N_C45Zero =
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
					}],
					""calculations"": [
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 18,
							""value"": ""-16850"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						},
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 45,
							""value"": ""0"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						}
					]}";

                public const string C18NA_C45P =
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
					}],
					""calculations"": [
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 19,
							""value"": ""-16850"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						},
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 45,
							""value"": ""100"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						}
					]}";

                public const string C18NA_C45NA =
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
					}],
					""calculations"": [
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 19,
							""value"": ""-16850"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						},
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 50,
							""value"": ""100"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						}
					]}";

                public const string C18Zero_C45NA =
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
					}],
					""calculations"": [
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 18,
							""value"": ""-0"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						},
						{
							""name"": ""Total Allocation"",
							""templateCalculationId"": 50,
							""value"": ""100"",
							""valueFormat"": ""Currency"",
							""type"": ""Cash"",
							""formulaText"": ""Something * something"",
							""aggregationType"": ""None"",
							""calculations"": null,
							""referenceData"": null
						}
					]}";
            }
        }

        private static class FinalAllocationSchoolTypeConstants
        {
            internal static class ValidSchema1Point0
            {
                public const string C74_HasValue =
                @"{
					""fundingValue"": {
					""totalValue"": 16850,
					""fundingLines"": [
						{
							""name"": ""Final Allocation School Type"",
							""fundingLineCode"": ""TotalAllocation"",
							""value"": 16850,
							""templateLineId"": 1,
							""type"": ""Payment"",
							""calculations"": [
								{
									""name"": ""Final Allocation School Type"",
									""type"": ""Enum"",
									""aggregationType"": ""None"",
									""formulaText"": """",
									""templateCalculationId"": 74,
									""value"": ""Non-Maintained Special School"",
									""valueFormat"": ""String"",
									""allowedEnumTypeValues"": [
										""Maintained School"",
										""Academy"",
										""Non-Maintained Special School"",
										""LA Alternative Provision""
									]
								}
							]
						}
					]
					}}";

                public const string C74_NULL =
                @"{
					""fundingValue"": {
					""totalValue"": 16850,
					""fundingLines"": [
						{
							""name"": ""Final Allocation School Type"",
							""fundingLineCode"": ""TotalAllocation"",
							""value"": 16850,
							""templateLineId"": 1,
							""type"": ""Payment"",
							""calculations"": [
								{
									""name"": ""Final Allocation School Type"",
									""type"": ""Enum"",
									""aggregationType"": ""None"",
									""formulaText"": """",
									""templateCalculationId"": 74,
									""value"": null,
									""valueFormat"": ""String"",
									""allowedEnumTypeValues"": [
										""Maintained School"",
										""Academy"",
										""Non-Maintained Special School"",
										""LA Alternative Provision""
									]
								}
							]
						}
					]
					}}";

                public const string C74_NA =
                @"{
					""fundingValue"": {
					""totalValue"": 16850,
					""fundingLines"": [
						{
							""name"": ""Final Allocation School Type"",
							""fundingLineCode"": ""TotalAllocation"",
							""value"": 16850,
							""templateLineId"": 1,
							""type"": ""Payment"",
							""calculations"": [
								{
									""name"": ""Final Allocation School Type"",
									""type"": ""Enum"",
									""aggregationType"": ""None"",
									""formulaText"": """",
									""templateCalculationId"": 75,
									""value"": null,
									""valueFormat"": ""String"",
									""allowedEnumTypeValues"": [
										""Maintained School"",
										""Academy"",
										""Non-Maintained Special School"",
										""LA Alternative Provision""
									]
								}
							]
						}
					]
					}}";
            }

            internal static class ValidSchema1Point1
            {
                public const string C74_HasValue =
                    @"{
					""totalValue"": 21280,
					""calculations"": {
						""2"": {
								""name"": ""Final Allocation School Type"",
								""type"": ""Enum"",
								""aggregationType"": ""None"",
								""formulaText"": """",
								""templateCalculationId"": 74,
								""value"": ""Non-Maintained Special School"",
								""valueFormat"": ""String"",
								""allowedEnumTypeValues"": [
									""Maintained School"",
									""Academy"",
									""Non-Maintained Special School"",
									""LA Alternative Provision""
								]
							}}}";

                public const string C74_NULL =
                    @"{
					""totalValue"": 21280,
					""calculations"": {
						""2"": {
								""name"": ""Final Allocation School Type"",
								""type"": ""Enum"",
								""aggregationType"": ""None"",
								""formulaText"": """",
								""templateCalculationId"": 74,
								""value"": null,
								""valueFormat"": ""String"",
								""allowedEnumTypeValues"": [
									""Maintained School"",
									""Academy"",
									""Non-Maintained Special School"",
									""LA Alternative Provision""
								]
							}}}";

                public const string C74_NA =
                    @"{
					""totalValue"": 21280,
					""calculations"": {
						""2"": {
								""name"": ""Final Allocation School Type"",
								""type"": ""Enum"",
								""aggregationType"": ""None"",
								""formulaText"": """",
								""templateCalculationId"": 75,
								""value"": ""Academy"",
								""valueFormat"": ""String"",
								""allowedEnumTypeValues"": [
									""Maintained School"",
									""Academy"",
									""Non-Maintained Special School"",
									""LA Alternative Provision""
								]
							}}}";
            }

            internal static class ValidSchema1Point2
            {
                public const string C74_HasValue =
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
					}],
					""calculations"": [
						{
							""name"": ""Final Allocation School Type"",
							""type"": ""Enum"",
							""aggregationType"": ""None"",
							""formulaText"": """",
							""templateCalculationId"": 74,
							""value"": ""Non-Maintained Special School"",
							""valueFormat"": ""String"",
							""allowedEnumTypeValues"": [
								""Maintained School"",
								""Academy"",
								""Non-Maintained Special School"",
								""LA Alternative Provision""
							]
						}
					]}";

                public const string C74_NULL =
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
					}],
					""calculations"": [
						{
							""name"": ""Final Allocation School Type"",
							""type"": ""Enum"",
							""aggregationType"": ""None"",
							""formulaText"": """",
							""templateCalculationId"": 74,
							""value"": null,
							""valueFormat"": ""String"",
							""allowedEnumTypeValues"": [
								""Maintained School"",
								""Academy"",
								""Non-Maintained Special School"",
								""LA Alternative Provision""
							]
						}
					]}";

                public const string C74_NA =
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
					}],
					""calculations"": [
						{
							""name"": ""Final Allocation School Type"",
							""type"": ""Enum"",
							""aggregationType"": ""None"",
							""formulaText"": """",
							""templateCalculationId"": 75,
							""value"": ""Non-Maintained Special School"",
							""valueFormat"": ""String"",
							""allowedEnumTypeValues"": [
								""Maintained School"",
								""Academy"",
								""Non-Maintained Special School"",
								""LA Alternative Provision""
							]
						}
					]}";
            }
        }
    }
}
