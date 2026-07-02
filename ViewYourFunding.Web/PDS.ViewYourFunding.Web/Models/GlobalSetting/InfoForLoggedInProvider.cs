namespace PDS.ViewYourFunding.Web.Models.GlobalSetting
{
    /// <summary>
    /// Allocation statements exist result model.
    /// </summary>
    public class InfoForLoggedInProvider
    {
        /// <summary>
        /// Gets or sets a value indicating whether allocation statements exist for the given ukprn.
        /// </summary>
        public bool FundingsExists { get; set; }

        /// <summary>
        /// Gets or sets a the number of fundings not read.
        /// </summary>
        public int NewFundingsNotRead { get; set; }

        /// <summary>
        /// Gets or sets a the number of fundings not read.
        /// </summary>
        public int UpdatedFundingsNotRead { get; set; }

        /// <summary>
        /// Gets or sets a the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is toggled on.
        /// </summary>
        public bool ToggledOn { get; set; }
    }
}