using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.RequestObjects
{
    /// <summary>
    /// Class representing a search request for local authorities.
    /// </summary>
    public class FundingApiSearchLocalAuthoritiesRequest
    {
        /// <summary>
        /// Gets or sets the search term.
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets a dictionary containing the configuration for each funding stream, with the funding stream code as the key.
        /// </summary>
        public IDictionary<string, FundingStream> FundingStreamConfiguration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether preview mode is enabled or not.
        /// </summary>
        public bool PreviewModeEnabled { get; set; }
    }
}