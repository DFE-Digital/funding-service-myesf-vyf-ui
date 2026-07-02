namespace PDS.ViewYourFunding.Web.Models.Request
{
    /// <summary>
    /// Request object for a local authority funding breakdown request.
    /// </summary>
    public class LocalAuthorityFundingBreakdownRequest
    {
        /// <summary>
        /// Gets or sets the funding stream code.
        /// </summary>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream name.
        /// </summary>
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets year from (e.g. 2020).
        /// </summary>
        public int YearFrom { get; set; }

        /// <summary>
        /// Gets or sets year to (e.g. 2020).
        /// </summary>
        public int YearTo { get; set; }

        /// <summary>
        /// Gets or sets local authority code (e.g. 203).
        /// </summary>
        public string LocalAuthorityCode { get; set; }

        /// <summary>
        /// Gets or sets published date .e.g. 2020-01-01.
        /// </summary>
        public string PublishedDate { get; set; }

        /// <summary>
        /// Gets or sets search term that was used to get to Breakdown page.
        /// Null if exact search was found.
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets tab that was selected to come through to funding breakdown page.
        /// </summary>
        public string Tab { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether include history allocation page.
        /// </summary>
        public bool IncludeHistory { get; set; }
    }
}