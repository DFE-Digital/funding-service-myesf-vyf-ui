namespace PDS.ViewYourFunding.Services.Interfaces.Models
{
    /// <summary>
    /// A colection of all the things to limit to.
    /// </summary>
    public class FundingApiSearchRequestObject
    {
        /// <summary>
        /// Gets or sets the funding streams to limit to.
        /// </summary>
        public FundingApiSearchFundingStream[] FundingStreams { get; set; }

        /// <summary>
        /// Gets or sets the search term to limit to.
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether should we wait for an index (re)build or fail if its re-building.
        /// </summary>
        public bool WaitForIndexBuild { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to bypass the grouping and return all versions.
        /// </summary>
        public bool BypassGrouping { get; set; }

        /// <summary>
        /// Gets or sets the type (e.g. Funding or ProviderFunding).
        /// </summary>
        public string Type { get; set; }
    }
}