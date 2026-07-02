namespace PDS.ViewYourFunding.Services.Interfaces.Models
{
    /// <summary>
    /// Model to store funding version details.
    /// </summary>
    public class FundingVersionDetail
    {
        /// <summary>
        /// Gets or sets the funding id of the statement.
        /// </summary>
        public string FundingId { get; set; }

        /// <summary>
        /// Gets or sets the statement version number of the statement.
        /// </summary>
        public int? StatementChannelVersion { get; set; }
    }
}
