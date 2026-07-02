namespace PDS.ViewYourFunding.Services.DTOs
{
    /// <summary>
    /// Represents cell/row merge data.
    /// </summary>
    public class MergeData
    {
        /// <summary>
        /// Gets or sets the row to start merging from.
        /// </summary>
        public int StartRow { get; set; }

        /// <summary>
        /// Gets or sets the cell to start merging from (A is 0).
        /// </summary>
        public int StartCell { get; set; }

        /// <summary>
        /// Gets or sets the number of rows to merge.
        /// </summary>
        public int NumberRows { get; set; }

        /// <summary>
        /// Gets or sets the number of cells to merge.
        /// </summary>
        public int NumberCells { get; set; }

        /// <summary>
        /// Gets or sets a name for the column (useful for debugging only).
        /// </summary>
        public string Name { get; set; }
    }
}