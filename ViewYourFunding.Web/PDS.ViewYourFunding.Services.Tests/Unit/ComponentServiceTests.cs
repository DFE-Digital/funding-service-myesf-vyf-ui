using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    /// <summary>
    /// The ComponentServiceTests class.
    /// </summary>
    [TestClass]
    public class ComponentServiceTests
    {
        /// <summary>
        /// as the get new a instance letter should be a.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void GetComponent_CheckNumberFormatFlowsThrough_KeepsFormat()
        {
            // Arrange
            var service = new ComponentService(null, null);
            var data = new FundingApiSearchFunding { TotalAmount = 12345.67 };

            // Act
            var actual = service.GetComponent(
                new UiModelGroup
                {
                    Datastyle = new UiModelStyle
                    {
                        NumberFormat = "ThousandsSeperatedNoTrailingZeroes"
                    },
                    Selector = "$..totalValue"
                },
                new Services.Interfaces.Models.ComponentConfiguration { Data = data },
                null,
                null,
                DateTime.MinValue,
                null,
                null);

            // Assert
            actual.Values.First().Should().Be("12,345.67");
        }

        [DataRow(10, false, FieldViewDataNumberFormat.GBCurrencyWithoutTrailingZeroes, null, "£10")]
        [DataRow(10131, false, FieldViewDataNumberFormat.ThousandsSeperatedNoTrailingZeroes, null, "10,131")]
        [DataRow(10131, false, FieldViewDataNumberFormat.ThousandsSeperated2DP, null, "10,131.00")]
        [DataRow(10131, false, FieldViewDataNumberFormat.GBCurrency, null, "£10,131.00")]
        [DataRow(10131, false, FieldViewDataNumberFormat.TwoDPWithoutTrailingZeroes, null, "10,131")]
        [DataRow(10131.01, false, FieldViewDataNumberFormat.TwoDPWithoutTrailingZeroes, null, "10,131.01")]
        [DataRow(31.01, false, FieldViewDataNumberFormat.PercentageWith1DecimalPlace, null, "31.0%")]
        [DataRow(31.01, false, FieldViewDataNumberFormat.PercentageWith2DecimalPlaces, null, "31.01%")]
        [DataRow(31.01, false, FieldViewDataNumberFormat.GBCurrencyWithoutDecimalPlace, null, "£31")]
        [DataRow(31.01, false, FieldViewDataNumberFormat.PercentageWithSignificantDecimalPlaces, null, "31.01%")]
        [DataRow(31.10, false, FieldViewDataNumberFormat.PercentageWithSignificantDecimalPlaces, null, "31.1%")]
        [DataRow(31.00, false, FieldViewDataNumberFormat.PercentageWithSignificantDecimalPlaces, null, "31%")]
        [TestMethod, TestCategory("Unit")]
        public void StyleNumber_ReturnsCorrectFormat(object input, bool makeAbsolute, string numberFormat, double? scaleFactor, string expectedResult)
        {
            // Arrange
            // Act
            var result = ComponentService.StyleNumber(input, makeAbsolute, numberFormat, scaleFactor);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("@IsAcademyProviderType=True&(@FreeMealsFundingValue>0|@HasEitherResidentialBursaryFundOrResidentialSupportScheme=True)", false)]
        [DataRow("@FreeMealsFundingValue>0|@HasEitherResidentialBursaryFundOrResidentialSupportScheme=True", true)]
        [DataRow("@IsAcademyProviderType=True&@HasFMForRBForRSSGreaterThanZero=True", true)]
        public void EvaluateVariable_TestFails(string expression, object expectedResult)
        {
            // Arrange
            var service = new ComponentService(null, null);
            UIModelVariable uIModelVariable = new UIModelVariable()
            {
                Expression = expression
            };
            ComponentConfiguration componentConfiguration = new ComponentConfiguration()
            {
                IsAcademyProviderType = true,
            };
            DateTime publicationDate = DateTime.Today;

            Dictionary<string, object> variables = new Dictionary<string, object>()
            {
                { "ResidentialBursaryFund", "4383" },
                { "ResidentialSupportScheme", "4383" },
                { "FreeMealsFundingValue", "10" },
                { "HasEitherResidentialBursaryFundOrResidentialSupportScheme", "True" },
                { "HasFMForRBForRSSGreaterThanZero", "True" }
            };


            // Act
            var result = service.EvaluateVariable(uIModelVariable, componentConfiguration, publicationDate, string.Empty, variables);


            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}