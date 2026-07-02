namespace PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels
{
    /// <summary>
    /// Represents a logged-in calculation.
    /// </summary>
    public class LoggedInCalculation
    {
        /// <summary>
        /// Gets or sets the template calculation ID.
        /// </summary>
        public int TemplateCalculationId { get; set; }

        /// <summary>
        /// Gets or sets the value of the calculation.
        /// </summary>
        public string? Value { get; set; }
    }
}
