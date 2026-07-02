using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A collection containing funding lines and calculations.
    /// </summary>
    public class CalculationAndFundingLinesCollection
    {
        /// <summary>
        /// Gets or sets the calculations.
        /// </summary>
        public Dictionary<int, CalculationNoNesting> Calculations { get; set; }

        /// <summary>
        /// Gets or sets the funding lines.
        /// </summary>
        public Dictionary<int, FundingLineNoNesting> FundingLines { get; set; }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        public double? TotalValue { get; set; }
    }
}