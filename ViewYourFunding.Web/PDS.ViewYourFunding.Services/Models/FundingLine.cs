namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A funding line (simplified).
    /// </summary>
    public class FundingLine : FundingLineNoNesting
    {
        /// <summary>
        /// Gets or sets the calculations.
        /// </summary>
        public Calculation[] Calculations { get; set; }

        /// <summary>
        /// Gets or sets the funding lines.
        /// </summary>
        public FundingLine[] FundingLines { get; set; }
    }
}