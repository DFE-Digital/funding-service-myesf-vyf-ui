using System;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// The Data Import Audit Model DTO class.
    /// </summary>
    public class DataImportAuditModel : CosmosDocument
    {
        /// <summary>
        /// Gets or sets the audit collection name.
        /// </summary>
        public static string AuditCollectionName { get; set; }

        /// <summary>
        /// Gets or sets the data import start date/time.
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the data import end date/time.
        /// </summary>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the data import status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the funding URI.
        /// </summary>
        /// <value>
        /// The funding URI.
        /// </value>
        public string FundingUri { get; set; }

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
                return AuditCollectionName;
            }
        }
    }
}
