namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A calculation (simplified).
    /// </summary>
    public class Calculation : CalculationNoNesting
    {
        /// <summary>
        /// Gets or sets the calculations.
        /// </summary>
        public Calculation[] Calculations { get; set; }
    }
}