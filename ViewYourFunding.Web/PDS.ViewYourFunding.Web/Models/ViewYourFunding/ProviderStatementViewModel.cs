using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    ///  The view model to use for the 'Provider statement' page.
    /// </summary>
    /// <seealso cref="SearchResultsViewModel" />
    public class ProviderStatementViewModel : SearchResultsViewModel
    {
        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle => FundingViewData?.First().Value.EntityName;

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string ContentTitle => FundingViewData?.First().Value.EntityName;

        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    return new List<BreadCrumbViewModel>
                    {
                        ViewingChoicePageBreadCrumb(false),
                        FindAnOrganisationPageBreadCrumb(false),
                        ProviderResultsPageBreadCrumb(false, SearchTerm),
                        ProviderPageBreadCrumb(
                            true,
                            FundingViewData?.First().Value.EntityName,
                            FundingViewData?.First().Value.EntityPrimaryIdentifier,
                            SearchTerm)
                    };
                }

                return new List<BreadCrumbViewModel>
                {
                    ViewingChoicePageBreadCrumb(false),
                    FindAnOrganisationPageBreadCrumb(false),
                    ProviderPageBreadCrumb(
                        true,
                        FundingViewData?.First().Value.EntityName,
                        FundingViewData?.First().Value.EntityPrimaryIdentifier)
                };
            }
        }

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        /// <summary>
        /// Gets or sets the funding view data for the provider.
        /// </summary>
        public Dictionary<string, FundingViewData> FundingViewData { get; set; }

        /// <summary>
        /// Gets or sets the organisation UKPRN.
        /// </summary>
        public string OrganisationUkprn { get; set; }
    }
}