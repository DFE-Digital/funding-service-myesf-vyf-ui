using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Represents the filter options for layout management page.
    /// </summary>
    public class FilterOptions
    {
        /// <summary>
        /// Gets or sets the list of funding streams ids.
        /// </summary>
        public List<int> FundingStreamsIds { get; set; }

        /// <summary>
        /// Gets or sets the list of funding view types.
        /// </summary>
        public List<string> FundingViewTypes { get; set; }

        /// <summary>
        /// Gets or sets the list of funding view scopes.
        /// </summary>
        public List<string> FundingViewScopes { get; set; }
    }
}