using PDS.ViewYourFunding.Services.Enums;
using System;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// The Preview layout model used from the layout management screens.
    /// </summary>
    public class PreviewLayoutModel
    {
        /// <summary>
        /// Gets or sets the layout identifier.
        /// </summary>
        /// <value>
        /// The layout identifier.
        /// </value>
        public string LayoutId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is preview.
        /// </summary>
        /// <value>
        ///  True if this instance is in preview mode.
        /// </value>
        public bool IsPreview { get; set; }

        /// <summary>
        /// Gets or sets the funding view scope.
        /// </summary>
        /// <value>
        /// The funding view scope.
        /// </value>
        public FundingViewScope FundingViewScope { get; set; }

        /// <summary>
        /// Gets or sets the type of the funding view.
        /// </summary>
        /// <value>
        /// The type of the funding view.
        /// </value>
        public FundingViewType FundingViewType { get; set; }

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is valid preview mode.
        /// </summary>
        /// <value>
        ///  True if this instance is in valid preview mode.
        /// </value>
        public bool IsValidPreviewMode => Guid.TryParse(LayoutId, out _) && IsPreview;
    }
}