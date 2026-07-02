using PDS.ViewYourFunding.Services.Enums;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Represents a setting value for a given setting type and funding stream in the View Your Funding area.
    /// </summary>
    public class PublicationLayout
    {
        /// <summary>
        /// Gets or sets the publication id.
        /// </summary>
        public int PublicationId { get; set; }

        /// <summary>
        /// Gets or sets the funding view type.
        /// </summary>
        public FundingViewType FundingViewType { get; set; }

        /// <summary>
        /// Gets or sets the funding view scope.
        /// </summary>
        public FundingViewScope FundingViewScope { get; set; }

        /// <summary>
        /// Gets or sets the layout id.
        /// </summary>
        public string LayoutId { get; set; }
    }
}