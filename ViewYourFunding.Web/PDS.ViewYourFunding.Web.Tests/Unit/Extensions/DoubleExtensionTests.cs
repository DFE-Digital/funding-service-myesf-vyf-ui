using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Helper;
using System;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Extensions
{
    [TestClass, TestCategory("Unit")]
    public class DoubleExtensionTests
    {
        [DataRow(12.54567, "£13")]
        [DataRow(12.44567, "£12")]
        [DataRow(-12.54567, "-£13")]
        [DataRow(-12.44567, "-£12")]
        [TestMethod]
        public void ToGBCurrencyWithoutDoublePlace_ForSpecifiedDouble_ReturnsWholeNumberWithPoundSign(double amount, string expectedResult)
        {
            // Arrange
            var doubleAmount = Convert.ToDouble(amount);  // have to convert as you cannot pass Double(not primitive type) into DataRow

            // Act
            var actual = doubleAmount.ToGBCurrencyWithoutDecimalPlace();

            // Assert
            actual.Should().Be(expectedResult);
        }

        [DataRow(12.54567, "12.545670")]
        [DataRow(1.0, "1")]
        [DataRow(0, "0")]
        [DataRow(1.87, "1.870000")]
        [TestMethod]
        public void ToWeightingWithoutTrailingZeroes_ReturnsExpected(double amount, string expectedResult)
        {
            // Arrange
            var doubleAmount = Convert.ToDouble(amount);  // have to convert as you cannot pass Double(not primitive type) into DataRow

            // Act
            var actual = doubleAmount.ToWeightingWithoutTrailingZeroes();

            // Assert
            actual.Should().Be(expectedResult);
        }

        [DataRow(12.44567, "£12.45")]
        [DataRow(12.44467, "£12.44")]
        [DataRow(-12.44567, "-£12.45")]
        [DataRow(-12.44467, "-£12.44")]
        [TestMethod]
        public void ToGBCurrency_ForSpecifiedDouble_ReturnsCurrencyWithPoundSign(double amount, string expectedResult)
        {
            // Arrange
            var doubleAmount = Convert.ToDouble(amount);

            // Act
            var actual = doubleAmount.ToGBCurrency();

            // Assert
            actual.Should().Be(expectedResult);
        }

        [DataRow(12111.00, "12,111")]
        [DataRow(12111.44, "12,111.44")]
        [DataRow(12.44, "12.44")]
        [DataRow(12.1, "12.10")]
        [DataRow(-1.50, "-1.50")]
        [DataRow(-10.00, "-10")]
        [DataRow(3, "3")]
        [DataRow(3.56, "3.56")]
        [DataRow(207, "207")]
        [DataRow(0, "0")]
        [TestMethod]
        public void ToThousandsSeperatedNoTrailingZeroes_ForSpecifiedDouble_ReturnsCorrectResult(double amount, string expectedResult)
        {
            // Arrange
            var doubleAmount = Convert.ToDouble(amount);

            // Act
            var actual = doubleAmount.ToThousandsSeperatedNoTrailingZeroes();

            // Assert
            actual.Should().Be(expectedResult);
        }

        [DataRow(12111.00, "12,111.00")]
        [DataRow(12111.44, "12,111.44")]
        [DataRow(12.44, "12.44")]
        [DataRow(12.1, "12.10")]
        [DataRow(-1.50, "-1.50")]
        [DataRow(-10.00, "-10.00")]
        [DataRow(3, "3.00")]
        [DataRow(3.56, "3.56")]
        [DataRow(207, "207.00")]
        [DataRow(0, "0.00")]
        [TestMethod]
        public void ToThousandsSeperated2DP_ForSpecifiedDouble_ReturnsCorrectResult(double amount, string expectedResult)
        {
            // Arrange
            var doubleAmount = Convert.ToDouble(amount);

            // Act
            var actual = doubleAmount.ToThousandsSeperated2DP();

            // Assert
            actual.Should().Be(expectedResult);
        }

        [DataRow(123453.56, "123,453.56%")]
        [DataRow(0, "0.00%")]
        [DataRow(-1.50, "-1.50%")]
        [DataRow(1.5, "1.50%")]
        [DataRow(3, "3.00%")]
        [DataRow(3.56, "3.56%")]
        [TestMethod]
        public void ToPercentageWith2DecimalPlaces_ForSpecifiedDouble_ReturnsCorrectResult(double amount, string expectedResult)
        {
            // Arrange
            var doubleAmount = Convert.ToDouble(amount);

            // Act
            var actual = doubleAmount.ToPercentageWith2DecimalPlaces();

            // Assert
            actual.Should().Be(expectedResult);
        }

        [DataRow(0, "0%")]
        [DataRow(-1.50, "-1.5%")]
        [DataRow(1.5, "1.5%")]
        [DataRow(3, "3%")]
        [DataRow(3.56, "3.56%")]
        [DataRow(13.56, "13.56%")]
        [DataRow(313.5, "313.5%")]
        [TestMethod]
        public void ToPercentageWithSignificantDigits_ReturnsCorrectString(double amount, string expectedResult)
        {
            // Arrange
            var doubleAmount = Convert.ToDouble(amount);

            // Act
            var actual = doubleAmount.ToPercentageWithSignificantDigits();

            // Assert
            actual.Should().Be(expectedResult);
        }

        [DataRow(22.5, "22")]
        [DataRow(23.5, "24")]
        [DataRow(12.44, "12")]
        [DataRow(3, "3")]
        [DataRow(3.56, "4")]
        [DataRow(123455.56, "123,456")]
        [TestMethod]
        public void ToThousandsSeparatedNoDP_ForSpecifiedDouble_ReturnsWholeNumberWithoutPoundSign(double amount, string expectedResult)
        {
            // Arrange
            var doubleAmount = Convert.ToDouble(amount);

            // Act
            var actual = doubleAmount.ToThousandsSeparatedNoDP();

            // Assert
            actual.Should().Be(expectedResult);
        }

        [DataRow(0, "0.0")]
        [DataRow(1.01, "1.0")]
        [DataRow(1.05, "1.1")]
        [DataRow(75.75, "75.8")]
        [DataRow(75.85, "75.8")]
        [TestMethod]
        public void To1DPWithoutTrailingZeroes_ReturnsCorrectString(double amount, string expectedResult)
        {
            // Arrange
            var doubleAmount = Convert.ToDouble(amount);

            // Act
            var actual = doubleAmount.To1DPWithoutTrailingZeroes();

            // Assert
            actual.Should().Be(expectedResult);
        }

        [DataRow(0, 2, "0.00")]
        [DataRow(1.01, 1, "1.0")]
        [DataRow(1.053, 2, "1.05")]
        [DataRow(75.7536734, 2, "75.75")]
        [DataRow(0.85736355, 4, "0.8574")]
        [TestMethod]
        public void ToSpecifiedDecimalPlaces_ReturnsCorrectString(double amount, int decimalPlaces, string expectedResult)
        {
            // Arrange
            var doubleAmount = Convert.ToDouble(amount);

            // Act
            var actual = doubleAmount.ToSpecifiedDecimalPlaces(decimalPlaces);

            // Assert
            actual.Should().Be(expectedResult);
        }

        [DataRow(0, "0")]
        [DataRow(1.01, "1")]
        [DataRow(1.05, "1")]
        [DataRow(75.75, "76")]
        [DataRow(1234567.89, "1234568")]
        [TestMethod]
        public void ZeroDecimalPLaces_ForSpecifiedDouble_ReturnsWholeNumberWithoutDecimalPlaces(double amount, string expectedResult)
        {
            // Arrange
            var doubleAmount = Convert.ToDouble(amount);

            // Act
            var actual = doubleAmount.ZeroDecimalPlaces();

            // Assert
            actual.Should().Be(expectedResult);
        }

        [DataRow(0, "0")]
        [DataRow(1.23456789, "1.234568")]
        [DataRow(1.2345671, "1.234567")]
        [DataRow(1.234, "1.234")]
        [TestMethod]
        public void To6DPWithoutTrailingZeroes_ReturnsCorrectString(double amount, string expectedResult)
        {
            // Arrange
            var doubleAmount = Convert.ToDouble(amount);

            // Act
            var actual = doubleAmount.To6DPWithoutTrailingZeroes();

            // Assert
            actual.Should().Be(expectedResult);
        }
    }
}