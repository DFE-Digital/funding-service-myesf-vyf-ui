using PDS.ViewYourFunding.Web.Models.Shared;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The base Search Results View Model.
    /// </summary>
    public abstract class SearchResultsViewModel : BaseViewYourFundingPageViewModel
    {
        /// <summary>
        /// Gets or sets the search term.
        /// </summary>
        /// <value>
        /// The search term.
        /// </value>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets the result count.
        /// </summary>
        /// <value>
        /// The result count.
        /// </value>
        public virtual int ResultCount { get; }

        /// <summary>
        /// Gets or sets the back to top link minimum count.
        /// </summary>
        /// <value>
        /// The back to top link minimum count.
        /// </value>
        public int BackToTopLinkMinimumCount { get; set; }

        /// <summary>
        /// Gets a value indicating whether to the show back to top link.
        /// </summary>
        public bool ShowBackToTopLink =>
            ResultCount >= BackToTopLinkMinimumCount;
    }
}
