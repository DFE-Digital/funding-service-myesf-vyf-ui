namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Represents the maximum UI and spreadsheet version numbers.
    /// </summary>
    public class MaximumUiSpreadsheetVersion
    {
        /// <summary>
        /// Gets or sets the maximum UI version number.
        /// </summary>
        public int? MaximumUiVersion { get; set; }

        /// <summary>
        /// Gets or sets the maximum spreadsheet version number.
        /// </summary>
        public int? MaximumSpreadsheetVersion { get; set; }
    }
}