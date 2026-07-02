using System.Collections.Generic;

namespace PDS.ViewYourFunding.Core.Configuration
{
    /// <summary>
    /// The Cosmos Db config.
    /// </summary>
    public class CosmosDbConfiguration
    {
        /// <summary>
        /// Gets or sets the funding cosmos database connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets the collection names.
        /// </summary>
        /// <value>
        /// The collection names.
        /// </value>
        public IEnumerable<string> CollectionNames
        {
            get
            {
                return new List<string> { LayoutCollection };
            }
        }


        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        public string DatabaseName { get; set; } = "funding";

        /// <summary>
        /// Gets or sets the layout collection name.
        /// </summary>
        public string LayoutCollection { get; set; } = "layout";

        /// <summary>
        /// Gets or sets the audit collection name.
        /// </summary>
        public string AuditCollection { get; set; } = "audit";

        /// <summary>
        /// Gets or sets the provider funding collection name.
        /// </summary>
        public string ProviderFundingCollection { get; set; } = "providerfunding";

        /// <summary>
        /// Gets or sets the cosmos connection mode.
        /// </summary>
        /// <value>
        /// The cosmos connection mode.
        /// </value>
        public string CosmosConnectionMode { get; set; } = "Direct";
    }
}