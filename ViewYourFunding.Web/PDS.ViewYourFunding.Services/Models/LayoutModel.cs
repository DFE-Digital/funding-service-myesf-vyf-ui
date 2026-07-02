using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// The Layout Model DTO class for managing page layout details.
    /// </summary>
    public class LayoutModel : CosmosDocument
    {
        /// <summary>
        /// Gets or sets the layout collection name.
        /// </summary>
        public static string LayoutCollectionName { get; set; }

        /// <summary>
        /// Gets or sets the layout json data.
        /// </summary>
        /// <value>
        /// The layout json data.
        /// </value>
        public string LayoutJsonData { get; set; }

        /// <summary>
        /// Gets or sets the layout json data.
        /// </summary>
        /// <value>
        /// The layout json data.
        /// </value>
        public Dictionary<string, object> Data { get; set; }

        /// <summary>
        /// Gets or sets the name of the layout.
        /// </summary>
        /// <value>
        /// The name of the layout.
        /// </value>
        public string LayoutName { get; set; }

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the funding view type.
        /// </summary>
        /// <value>
        /// The funding view type.
        /// </value>
        public string FundingViewType { get; set; }

        /// <summary>
        /// Gets or sets the funding view scope.
        /// </summary>
        /// <value>
        /// The funding view scope.
        /// </value>
        public string FundingViewScope { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified date time.
        /// </summary>
        /// <value>
        /// The last modified.
        /// </value>
        public DateTime LastModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the layout Deleted date time.
        /// </summary>
        /// <value>
        /// The layout deleted datetime.
        /// </value>
        public DateTime? DeletedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>
        /// The last modified by.
        /// </value>
        public string LastModifiedBy { get; set; }

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
                return LayoutCollectionName;
            }
        }
    }
}