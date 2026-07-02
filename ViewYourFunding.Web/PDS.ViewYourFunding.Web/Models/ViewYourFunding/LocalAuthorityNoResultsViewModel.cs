using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// View model for the local authority 'no results' page.
    /// </summary>
    public class LocalAuthorityNoResultsViewModel : SearchResultsViewModel
    {
        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems => new List<BreadCrumbViewModel>
        {
            ViewingChoicePageBreadCrumb(false),
            FindAnOrganisationPageBreadCrumb(false),
            GenericCurrentPageBreadcrumb(ViewYourFundingConstants.PageTitle_SearchResults)
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