namespace PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels
{
    /// <summary>
    /// The logged in template line.
    /// </summary>
    public class LoggedInTemplateLine
    {
        /// <summary>
        /// Gets or sets the template line identifier.
        /// </summary>
        /// <value>
        /// The template line identifier.
        /// </value>
        public int TemplateLineId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; } = default!;

        /// <summary>
        /// Gets or sets the distribution periods.
        /// </summary>
        /// <value>
        /// The distribution periods.
        /// </value>
        public List<LoggedInDistributionPeriod>? DistributionPeriods { get; set; }
    }
}
