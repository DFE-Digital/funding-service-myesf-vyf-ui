namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// A funding values most basic properties.
    /// </summary>
    public interface IFundingValueNested
    {
        /// <summary>
        /// Gets or sets the schema version (e.g. 1.1).
        /// </summary>
        public double SchemaVersion { get; set; }

        /// <summary>
        /// Gets or sets the total value.
        /// </summary>
        public double? TotalValue { get; set; }
    }
}