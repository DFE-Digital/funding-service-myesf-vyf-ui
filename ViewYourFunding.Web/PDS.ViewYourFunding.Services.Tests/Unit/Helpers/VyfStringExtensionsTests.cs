using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Helper;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    [TestClass]
    public class VyfStringExtensionsTests
    {
        [TestMethod, TestCategory("Unit")]
        [DataRow("Test@Test1", "Test[[ATSYMBOLVYF]]Test1")]
        [DataRow(null, null)]
        [DataRow("Test", "Test")]
        public void ReplaceAtSymbolInName_ReturnsExpectedResult(string input, string expectedValue)
        {
            // Arrange/Act
            var result = input.ReplaceAtSymbolInName();

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("Test[[ATSYMBOLVYF]]Test1", "Test@Test1")]
        [DataRow(null, null)]
        [DataRow("Test", "Test")]
        public void ReplaceVYFAtSymbolWithAt_ReturnsExpectedResult(string input, string expectedValue)
        {
            // Arrange/Act
            var result = input.ReplaceVYFAtSymbolWithAt();

            // Assert
            result.Should().Be(expectedValue);
        }
    }
}
