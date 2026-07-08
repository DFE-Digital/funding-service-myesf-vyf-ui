using PDS.ViewYourFunding.Services.Interfaces.Models;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// The Provider Funding Model class.
    /// </summary>
    public class ProviderFundingModel : CosmosDocument
    {
        /// <summary>
        /// Gets or sets thepProvider funding collection name.
        /// </summary>
        public static string ProviderFundingCollectionName { get; set; }

        /// <summary>
        /// Gets or sets the partitionKey.
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// Gets or sets the Provider.
        /// </summary>
        public ProviderModel Provider { get; set; }

        /// <summary>
        /// Gets or sets the fundingPeriodId.
        /// </summary>
        public string FundingPeriodId { get; set; }

        /// <summary>
        /// Gets or sets the fundingStreamCode.
        /// </summary>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the fundingVersion.
        /// </summary>
        public string FundingVersion { get; set; }

        /// <summary>
        /// Gets or sets the channel version.
        /// </summary>
        public IEnumerable<ChannelVersion> ChannelVersions { get; set; }

        /// <summary>
        /// Gets or sets the version from statement channel.
        /// </summary>
        public int? StatementChannelVersion { get; set; }

        /// <summary>
        /// Gets the version from statement channel.
        /// </summary>
        public string DisplayStatementVersion
        {
            get
            {
                return StatementChannelVersion.HasValue ? StatementChannelVersion.Value.ToString() : FundingVersion.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>
        /// The name of the collection.
        /// </value>
        public override string CollectionName
        {
            get
            {
                return ProviderFundingCollectionName;
            }
        }
    }
}
