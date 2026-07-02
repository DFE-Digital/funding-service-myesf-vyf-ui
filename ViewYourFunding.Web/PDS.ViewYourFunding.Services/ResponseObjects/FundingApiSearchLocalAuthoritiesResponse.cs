using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.ResponseObjects
{
    /// <summary>
    /// Class representing a search response for local authorities.
    /// </summary>
    public class FundingApiSearchLocalAuthoritiesResponse
    {
        /// <summary>
        /// Gets or sets a dictionary containing the local authorities matching the search, where the key is the LA code and the value is the LA name.
        /// </summary>
        public IDictionary<string, string> LocalAuthorities { get; set; }
    }
}
