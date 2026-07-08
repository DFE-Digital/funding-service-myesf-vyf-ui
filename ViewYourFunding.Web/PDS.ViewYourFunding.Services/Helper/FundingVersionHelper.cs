using PDS.ViewYourFunding.Services.Interfaces.Models;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Helper methods for working with funding versions.
    /// </summary>
    public static class FundingVersionHelper
    {
        /// <summary>
        /// Parse a funding version string (possibly containing underscores) into a dobule.
        /// </summary>
        /// <param name="fundingVersionStr">The funding version as a string (possibly containing underscores).</param>
        /// <returns>A double.</returns>
        public static double Parse(string fundingVersionStr)
        {
            const double defaultVersionNumber = -1.0;

            if (string.IsNullOrEmpty(fundingVersionStr))
            {
                return defaultVersionNumber;
            }

            return double.TryParse(fundingVersionStr?.Replace("_", "."), out var fundingVersion) ? fundingVersion : defaultVersionNumber;
        }

        /// <summary>
        /// Parse a channel version string array into json array and return the value from statement channel type.
        /// </summary>
        /// <param name="channelVersions">The channel version string array.</param>
        /// <returns>The statement channel value.</returns>
        public static int? GetStatementVersionNumber(ChannelVersion[] channelVersions)
        {
            if (channelVersions != null && channelVersions.Any())
            {
                return channelVersions.Where(x => x.Type == "Statement").First().Value;
            }

            return null;
        }

        /// <summary>
        /// Build channel versions.
        /// </summary>
        /// <param name="statementVersionNumber">Statement Version Number.</param>
        /// <returns>List of channel version.</returns>
        public static ChannelVersion[] BuildChannelVersions(int statementVersionNumber)
        {
            return new ChannelVersion[]
            {
                new ChannelVersion { Type = "Contract", Value = 0 },
                new ChannelVersion { Type = "Payment", Value = 0 },
                new ChannelVersion { Type = "Statement", Value = statementVersionNumber },
            };
        }
    }
}