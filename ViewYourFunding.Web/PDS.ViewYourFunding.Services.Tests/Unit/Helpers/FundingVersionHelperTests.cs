using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces.Models;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    /// <summary>
    /// The FundingPeriodHelperTests class.
    /// </summary>
    [TestClass]
    public class FundingVersionHelperTests
    {
        /// <summary>
        /// Gets the years from code valid funding period code should evaluate correct.
        /// </summary>
        /// <param name="statementVersionNumber">The statement channel version number.</param>
        /// <param name="expectedStatementVersionNumber">The expected version number value from statment channel.</param>
        [TestMethod, TestCategory("Unit")]
        [DataRow(0, 0)]
        [DataRow(1, 1)]
        [DataRow(2, 2)]
        [DataRow(null, null)]
        public void GetStatementChannelValue_ExpectedResult(int? statementVersionNumber, int? expectedStatementVersionNumber)
        {
            //Arrange
            ChannelVersion[] channelVersions = null;
            if (statementVersionNumber != null)
            {
                channelVersions = new ChannelVersion[]
                {
                    new ChannelVersion { Type = "Statement", Value = (int)statementVersionNumber },
                    new ChannelVersion { Type = "Payment", Value = 0 },
                    new ChannelVersion { Type = "Contract", Value = 0 }
                };
            }

            // Act
            var actual = FundingVersionHelper.GetStatementVersionNumber(channelVersions);

            // Assert
            actual.Should().Be(expectedStatementVersionNumber);
        }
    }
}