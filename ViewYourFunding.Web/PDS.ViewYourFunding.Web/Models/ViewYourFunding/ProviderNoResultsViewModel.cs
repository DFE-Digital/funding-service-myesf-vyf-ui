using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The Provider (School or Academy) Search Results View Model.
    /// </summary>
    public class ProviderNoResultsViewModel : SearchResultsViewModel
    {
        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems => new List<BreadCrumbViewModel>
        {
            ViewingChoicePageBreadCrumb(false),
            FindAnOrganisationPageBreadCrumb(false),
            ProviderResultsPageBreadCrumb(true, SearchTerm)
        };

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle => ViewYourFundingConstants.PageTitle_SearchResults;

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string ContentTitle => ViewYourFundingConstants.PageTitle_SearchResults;
    }
}
