namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The view model containing latest year for funding streams.
    /// </summary>
    public class FundingStreamCurrentYearViewModel
    {
        /// <summary>
        /// Gets or sets the name of the funding stream.
        /// </summary>
        /// <value>
        /// The name of the funding stream.
        /// </value>
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets the code of the funding stream.
        /// </summary>
        /// <value>
        /// The code of the funding stream.
        /// </value>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the year start for which to show allocations for funding stream.
        /// </summary>
        public int YearStart { get; set; }

        /// <summary>
        /// Gets or sets the year end for which to show allocations for funding stream.
        /// </summary>
        public int YearEnd { get; set; }
    }
}
