namespace PDS.ViewYourFunding.Services.DTOs
{
    /// <summary>
    /// Represents cell/row frozen data.
    /// </summary>
    public class FrozenData
    {
        /// <summary>
        /// Gets or sets the row to start merging from.
        /// </summary>
        public int StartRow { get; set; }

        /// <summary>
        /// Gets or sets the cell to start merging from (A is 0).
        /// </summary>
        public int StartCell { get; set; }
    }
}