using PDS.ViewYourFunding.Repositories.Model;
using System;

namespace PDS.ViewYourFunding.Repositories.DataModels
{
    /// <summary>
    /// A publication layout (specify the layout to use for a publication - for a specific type and scope).
    /// </summary>
    public class PublicationLayout : TableWithIntegerId
    {
        /// <summary>
        /// Gets or sets the distinct id for this instance of a publication layout.
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets the publication id.
        /// </summary>
        public int PublicationId { get; protected set; }

        /// <summary>
        /// Gets or sets the funding view type.
        /// </summary>
        public int FundingViewType { get; protected set; }

        /// <summary>
        /// Gets or sets the funding view scope.
        /// </summary>
        public int FundingViewScope { get; protected set; }

        /// <summary>
        /// Gets or sets the layout Id (from Cosmos).
        /// </summary>
        public string LayoutId { get; set; }

        /// <summary>
        /// Gets or sets when the publication layout information was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the publication layut information was last updated.
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the username of the last person to update the publication information.
        /// </summary>
        public string LastUpdatedBy { get; set; }
    }
}