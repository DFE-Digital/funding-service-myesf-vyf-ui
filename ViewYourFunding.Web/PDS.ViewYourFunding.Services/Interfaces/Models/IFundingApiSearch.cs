using Newtonsoft.Json;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Interfaces.Models
{
    /// <summary>
    /// A generic interface for funding and provider funding.
    /// </summary>
    public interface IFundingApiSearch
    {
        /// <summary>
        /// Gets or sets the version of this allocation.
        /// </summary>
        [JsonProperty("fundingVersion")]
        string FundingVersion { get; set; }

        /// <summary>
        /// Gets or sets the version from statement channel.
        /// </summary>
        [JsonProperty("channelVersions")]
        IEnumerable<ChannelVersion> ChannelVersions { get; set; }

        /// <summary>
        /// Gets or sets the version from statement channel.
        /// </summary>
        [JsonProperty("statementChannelVersion")]
        public int? StatementChannelVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the funding is first version.
        /// </summary>
        [JsonProperty("isFirstStatementChannelVersion")]
        bool IsFirstStatementChannelVersion { get; set; }

        /// <summary>
        /// Gets or sets the funding value.
        /// </summary>
        [JsonProperty("fundingValue")]
        string FundingValue { get; set; }

        /// <summary>
        /// Gets or sets the schema version.
        /// </summary>
        [JsonProperty("schemaVersion")]
        string SchemaVersion { get; set; }

        /// <summary>
        /// Gets or sets the total amount of this funding.
        /// </summary>
        [JsonProperty("totalAmount")]
        double TotalAmount { get; set; }
    }
}