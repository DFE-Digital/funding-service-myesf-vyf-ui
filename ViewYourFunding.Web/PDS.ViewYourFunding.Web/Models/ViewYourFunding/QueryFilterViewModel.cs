using PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The query filter view model class.
    /// </summary>
    public class QueryFilterViewModel
    {
        /// <summary>
        /// Gets or sets the route name to use to apply or reset the selected filters.
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// Gets or sets the query filter.
        /// </summary>
        /// <value>
        /// The query filter.
        /// </value>
        public QueryFilter QueryFilter { get; set; }

        /// <summary>
        /// Gets or sets the search term.
        /// </summary>
        /// <value>
        /// The search term.
        /// </value>
        public string SearchTerm { get; set; }
    }
}