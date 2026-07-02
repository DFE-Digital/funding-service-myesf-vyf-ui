namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// The GetAutoPullFundingStreamResult to be used by feed reader.
    /// </summary>
    public class GetAutoPullFundingStreamResult
    {
        /// <summary>
        /// Gets or sets the funding stream code found.
        /// </summary>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream name found.
        /// </summary>
        public string FundingStreamName { get; set; }
    }
}
