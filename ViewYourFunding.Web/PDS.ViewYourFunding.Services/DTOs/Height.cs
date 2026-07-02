namespace PDS.ViewYourFunding.Services.DTOs
{
    /// <summary>
    /// Represents row height.
    /// </summary>
    public class Height
    {
        /// <summary>
        /// Gets or sets the row number (e.g. 0).
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets the height of the row in inches.
        /// </summary>
        public double HeightInches { get; set; }
    }
}