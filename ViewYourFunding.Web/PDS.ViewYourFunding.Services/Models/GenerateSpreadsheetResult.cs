namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// The Generate spreadsheet result.
    /// </summary>
    public class GenerateSpreadsheetResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether whether generation was successful.
        /// </summary>
        /// <value>
        ///   true if success; otherwise, false.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets error message (often will be null).
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
    }
}