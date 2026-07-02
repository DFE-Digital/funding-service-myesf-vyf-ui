using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// This class represents a search result filter.
    /// </summary>
    public class SearchResultsFilter
    {
        /// <summary>
        /// Gets or sets the displayable title of the filter.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a string that is used to identify the property that the filter represents.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if. <code>true</code>, the filter values should be visible, otherwise they should be hidden.
        /// </summary>
        public bool Open { get; set; }

        /// <summary>
        /// Gets or sets the collection of values relevant to this filter.
        /// </summary>
        public List<SearchFilterValue> Values { get; set; } = new List<SearchFilterValue>();

        /// <summary>
        /// Gets or sets a value indicating whether if. <code>true</code>, the search filter values can be filtered by a search box, otherwise no search box should be displayed.
        /// </summary>
        public bool SearchEnabled { get; set; }
    }
}
