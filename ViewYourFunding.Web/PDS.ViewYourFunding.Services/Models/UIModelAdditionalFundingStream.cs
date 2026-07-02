namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// An additional funding stream.
    /// </summary>
    public class UIModelAdditionalFundingStream
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the funding period prefix (optional).
        /// </summary>
        public string FundingPeriodPrefix { get; set; }
    }
}