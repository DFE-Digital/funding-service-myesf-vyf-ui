namespace PDS.ViewYourFunding.Services.DTOs
{
    /// <summary>
    /// Represents a column.
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Gets or sets the position of the column (A is 0).
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the width of the column in inches (optional).
        /// </summary>
        public double? WidthInches { get; set; }

        /// <summary>
        /// Gets or sets the number format for the column (e.g. '#,##0.000').
        /// </summary>
        public string NumberFormat { get; set; }
    }
}