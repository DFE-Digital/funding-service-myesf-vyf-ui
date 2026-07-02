namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The base filterable search results view model.
    /// </summary>
    public abstract class FilterableSearchResultsViewModel : SearchResultsViewModel
    {
        /// <summary>
        /// Gets or sets the query filter view model.
        /// </summary>
        public QueryFilterViewModel QueryFilterViewModel { get; set; }
    }
}