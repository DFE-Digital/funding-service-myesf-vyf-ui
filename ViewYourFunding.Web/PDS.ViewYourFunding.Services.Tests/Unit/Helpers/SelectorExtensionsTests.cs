using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Helper;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    [TestClass]
    public class SelectorExtensionsTests
    {
        [TestMethod, TestCategory("Unit")]
        [DataRow(true, "Yes")]
        [DataRow(false, "No")]
        [DataRow("dummy", "No")]
        [DataRow("true", "Yes")]
        [DataRow("false", "No")]
        public void ToBooleanYesOrNo_ReturnsExpectedResult(object input, string expectedValue)
        {
            // Arrange/Act
            var result = input.ToBooleanYesOrNo();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(0, "Lump sum")]
        [DataRow(1, "Taper")]
        [DataRow(2, "National funding formula")]
        [DataRow(8, "National funding formula")]
        public void ToSparsityMethodologyDisplayValue_ReturnsExpectedResult(int input, string expectedValue)
        {
            // Arrange/Act
            var result = input.ToSparsityMethodologyDisplayValue();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("Lump sum", "Fixed")]
        [DataRow("Taper", "Tapered")]
        [DataRow("NFF", "NFF")]
        [DataRow("ABC", "NFF")]
        public void ToSparsityMethodologyDisplayValuePdf_ReturnsExpectedResult(string input, string expectedValue)
        {
            // Arrange/Act
            var result = input.ToSparsityMethodologyDisplayValuePdfs();

            // Assert
            result.Should().Be(expectedValue);
        }
    }
}