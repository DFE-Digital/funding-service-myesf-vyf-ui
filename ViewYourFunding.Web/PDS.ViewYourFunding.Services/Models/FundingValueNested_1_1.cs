using PDS.ViewYourFunding.Services.Interfaces;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A schema version 1.1 and above representation of a funding value object in the data provided by CFS.
    /// Their is some flexibility in the way the heirarchy works in this model.
    /// </summary>
    public class FundingValueNested_1_1 : IFundingValueNested
    {
        /// <inheritdoc/>
        public double SchemaVersion { get; set; }

        /// <inheritdoc/>
        public double? TotalValue { get; set; }

        /// <summary>
        /// Gets or sets the funding value property (may be null).
        /// </summary>
        public FundingValueNested_1_1 FundingValue { get; set; }

        /// <summary>
        /// Gets or sets the funding template property (may be null).
        /// </summary>
        public FundingValueNested_1_1 FundingTemplate { get; set; }

        /// <summary>
        /// Gets or sets the funding lines object.
        /// </summary>
        public Dictionary<string, FundingLineNoNesting> FundingLines { get; set; }

        /// <summary>
        /// Gets or sets the calculations dictionary.
        /// </summary>
        public Dictionary<string, CalculationNoNesting> Calculations { get; set; }
    }
}