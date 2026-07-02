namespace PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels
{
    /// <summary>
    /// Represents the response model for a logged-in distribution period.
    /// </summary>
    public class LoggedInDistributionPeriod
    {
        /// <summary>
        /// Gets or sets the distribution period ID.
        /// </summary>
        public string? DistributionPeriodId { get; set; }

        /// <summary>
        /// Gets or sets the value of the distribution period.
        /// </summary>
        public double? Value { get; set; }
    }
}
