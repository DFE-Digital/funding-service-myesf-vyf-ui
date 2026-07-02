namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A calculation (simplified).
    /// </summary>
    public class CalculationNoNesting
    {
        /// <summary>
        /// Gets or sets a template calculation id.
        /// </summary>
        public int TemplateCalculationId { get; set; }

        /// <summary>
        /// Gets or sets an object that contains a value.
        /// </summary>
        public object Value { get; set; }
    }
}