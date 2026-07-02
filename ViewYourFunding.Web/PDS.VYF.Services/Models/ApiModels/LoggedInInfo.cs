namespace PDS.VYF.Services.Models.ApiModels
{
    /// <summary>
    /// Represents the logged-in user information.
    /// </summary>
    public class LoggedInInfo
    {
        /// <summary>
        /// Gets or sets a value indicating whether allocation statements exist for the given ukprn.
        /// </summary>
        public bool FundingsExists { get; set; }

        /// <summary>
        /// Gets or sets the number of new fundings not read.
        /// </summary>
        public int NewFundingsNotRead { get; set; }

        /// <summary>
        /// Gets or sets the number of updated fundings not read.
        /// </summary>
        public int UpdatedFundingsNotRead { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this is toggled on.
        /// </summary>
        public bool ToggledOn { get; set; }
    }
}
