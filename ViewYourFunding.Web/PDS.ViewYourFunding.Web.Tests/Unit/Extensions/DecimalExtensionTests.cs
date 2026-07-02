using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Web.Helpers;
using System;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Extensions
{
    /// <summary>
    /// The DecimalExtensionTests class.
    /// </summary>
    [TestClass]
    public class DecimalExtensionTests
    {
        /// <summary>
        /// Converts to gbcurrencywithoutdecimalplace_forspecifieddecimal_returnswholenumberwithpoundsign.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="expectedResult">The expected result.</param>
        [DataRow(12.54567, "£13")]
        [DataRow(12.44567, "£12")]
        [DataRow(-12.54567, "-£13")]
        [DataRow(-12.44567, "-£12")]
        [TestMethod, TestCategory("Unit")]
        public void ToGBCurrencyWithoutDecimalPlace_ForSpecifiedDecimal_ReturnsWholeNumberWithPoundSign(double amount, string expectedResult)
        {
            // Arrange
            var decimalAmount = Convert.ToDecimal(amount);  // have to convert as you cannot pass decimal(not primitive type) into DataRow

            // Act
            var actual = decimalAmount.ToGBCurrencyWithoutDecimalPlace();

            // Assert
            actual.Should().Be(expectedResult);
        }

        /// <summary>
        /// Converts to gbcurrency_forspecifieddecimal_returnscurrencywithpoundsign.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="expectedResult">The expected result.</param>
        [DataRow(12.44567, "£12.45")]
        [DataRow(12.44467, "£12.44")]
        [DataRow(-12.44567, "-£12.45")]
        [DataRow(-12.44467, "-£12.44")]
        [TestMethod, TestCategory("Unit")]
        public void ToGBCurrency_ForSpecifiedDecimal_ReturnsCurrencyWithPoundSign(double amount, string expectedResult)
        {
            // Arrange
            var decimalAmount = Convert.ToDecimal(amount);

            // Act
            var actual = decimalAmount.ToGBCurrency();

            // Assert
            actual.Should().Be(expectedResult);
        }

        /// <summary>
        /// Converts to numberformat_forspecifieddecimal_returnsnumberincorrectformat.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="expectedResult">The expected result.</param>
        [DataRow(0.00, "£0")]
        [DataRow(0.01, "£0.01")]
        [DataRow(0.015, "£0.02")]
        [DataRow(0.1, "£0.10")]
        [DataRow(1.00, "£1")]
        [DataRow(1.009, "£1.01")]
        [DataRow(5.00, "£5")]
        [DataRow(10.00, "£10")]
        [DataRow(10.99, "£10.99")]
        [DataRow(10.999, "£11")]
        [DataRow(100.00, "£100")]
        [DataRow(100.99, "£100.99")]
        [DataRow(1000.00, "£1,000")]
        [DataRow(1000.09, "£1,000.09")]
        [DataRow(1000000.00, "£1,000,000")]
        [DataRow(1000000.56, "£1,000,000.56")]
        [DataRow(1000000.567, "£1,000,000.57")]
        [DataRow(20000000.00, "£20,000,000")]
        [DataRow(20000000.01, "£20,000,000.01")]
        [DataRow(1000.00, "£1,000")]
        [TestMethod, TestCategory("Unit")]
        public void ToNumberFormat_ForSpecifiedDecimal_ReturnsNumberInCorrectFormat(double? amount, string expectedResult)
        {
            // Arrange
            var decimalAmount = Convert.ToDecimal(amount);

            // Act
            var actual = decimalAmount.ToGBCurrencyWithoutTrailingZeroes();

            // Assert
            actual.Should().Be(expectedResult);
        }

        /// <summary>
        /// Converts to 2dpwithouttrailingzeroes_forspecifieddecimal_returnsnumberincorrectformat.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="expectedResult">The expected result.</param>
        [DataRow(0.00, "0")]
        [DataRow(0.01, "0.01")]
        [DataRow(0.015, "0.02")]
        [DataRow(0.1, "0.10")]
        [DataRow(1.00, "1")]
        [DataRow(1.009, "1.01")]
        [DataRow(5.00, "5")]
        [DataRow(10.00, "10")]
        [DataRow(10.99, "10.99")]
        [DataRow(10.999, "11")]
        [DataRow(100.00, "100")]
        [DataRow(100.99, "100.99")]
        [DataRow(1000.00, "1,000")]
        [DataRow(1000.09, "1,000.09")]
        [DataRow(1000000.00, "1,000,000")]
        [DataRow(1000000.56, "1,000,000.56")]
        [DataRow(1000000.567, "1,000,000.57")]
        [DataRow(20000000.00, "20,000,000")]
        [DataRow(20000000.01, "20,000,000.01")]
        [DataRow(1000.00, "1,000")]
        [TestMethod, TestCategory("Unit")]
        public void To2DPWithoutTrailingZeroes_ForSpecifiedDecimal_ReturnsNumberInCorrectFormat(double? amount, string expectedResult)
        {
            // Arrange
            var decimalAmount = Convert.ToDecimal(amount);

            // Act
            var actual = decimalAmount.To2DPWithoutTrailingZeroes();

            // Assert
            actual.Should().Be(expectedResult);
        }
    }
}
