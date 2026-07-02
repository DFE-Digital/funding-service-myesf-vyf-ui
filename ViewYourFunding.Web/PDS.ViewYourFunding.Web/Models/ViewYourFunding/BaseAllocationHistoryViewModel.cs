using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The base view model for all 'allocation history' pages.
    /// </summary>
    public class BaseAllocationHistoryViewModel : SearchResultsViewModel
    {
        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the title in the page's main content section.
        /// </summary>
        public override bool ShowContentTitle => false;

        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle => ViewYourFundingConstants.PageTitle_AllocationHistory;

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string ContentTitle => ViewYourFundingConstants.PageTitle_AllocationHistory;

        /// <summary>
        /// Gets or sets the secondary title in the page's main content section.
        /// </summary>
        public string SecondaryContentTitle { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the funding stream.
        /// </summary>
        public FundingStream.FundingStream FundingStreamConfiguration { get; set; }

        /// <summary>
        /// Gets or sets a list of key-value pairs mapping the funding period years (e.g. "2020", "2021") to the list of the publications for that period.
        /// </summary>
        public List<KeyValuePair<(int yearFrom, int yearTo), List<Services.Models.Publication>>> FundingPeriodPublications { get; set; }
    }
}