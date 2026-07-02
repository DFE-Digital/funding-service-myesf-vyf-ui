using PDS.ViewYourFunding.Services.Interfaces;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A schema version 1.0 representation of a funding value object in the data provided by CFS.
    /// Their is some flexibility in the way the heirarchy works in this model.
    /// </summary>
    public class FundingValueNested_1_0 : IFundingValueNested
    {
        /// <inheritdoc/>
        public double SchemaVersion { get; set; }

        /// <inheritdoc/>
        public double? TotalValue { get; set; }

        /// <summary>
        /// Gets or sets the funding value property (may be null).
        /// </summary>
        public FundingValueNested_1_0 FundingValue { get; set; }

        /// <summary>
        /// Gets or sets the funding template property (may be null).
        /// </summary>
        public FundingValueNested_1_0 FundingTemplate { get; set; }

        /// <summary>
        /// Gets or sets the funding lines array.
        /// </summary>
        public FundingLine[] FundingLines { get; set; }

        /// <summary>
        /// Gets or sets the calculations array.
        /// </summary>
        public Calculation[] Calculations { get; set; }
    }
}