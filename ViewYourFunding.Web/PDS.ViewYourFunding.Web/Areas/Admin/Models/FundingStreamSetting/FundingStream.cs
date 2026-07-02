namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting
{
    /// <summary>
    /// Represents a funding stream in the View Your Funding area.
    /// </summary>
    public class FundingStream
    {
        /// <summary>
        /// Gets or sets the name of the funding stream.
        /// </summary>
        /// <value>
        /// The name of the funding stream.
        /// </value>
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the funding stream code.
        /// </summary>
        /// <value>
        /// The funding stream code.
        /// </value>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether funding stream is Active.
        /// </summary>
        /// <value>
        /// The funding is Active or not.
        /// </value>
        public bool Active { get; set; }
    }
}