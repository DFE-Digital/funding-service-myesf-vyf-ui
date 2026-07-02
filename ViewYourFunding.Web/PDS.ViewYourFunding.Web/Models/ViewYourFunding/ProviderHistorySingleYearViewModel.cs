using Pds.Core.Web.Models;
using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Helper;
using System.Collections.Generic;
using System.Web;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The Provider Funding Breakdown 201819 view model.
    /// </summary>
    /// <seealso cref="Shared.BaseViewYourFundingPageViewModel" />
    public class ProviderHistorySingleYearViewModel : DownloadViewModel
    {
        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    SearchTerm = HttpUtility.HtmlDecode(SearchTerm);
                    return new List<BreadCrumbViewModel>
                    {
                        ViewingChoicePageBreadCrumb(false),
                        FindAnOrganisationPageBreadCrumb(false),
                        ProviderResultsPageBreadCrumb(false, SearchTerm),
                        ProviderPageBreadCrumb(false, OrganisationName, OrganisationUkprn, SearchTerm),
                        ProviderAllocationHistoryPageBreadCrumb(
                            false,
                            FundingStreamName.ToUIPathComponent(),
                            OrganisationUkprn,
                            SearchTerm),
                        ProviderFundingBreakdownPageBreadCrumb(true, ContentTitle)
                    };
                }

                return new List<BreadCrumbViewModel>
                {
                    ViewingChoicePageBreadCrumb(false),
                    FindAnOrganisationPageBreadCrumb(false),
                    ProviderPageBreadCrumb(false, OrganisationName, OrganisationUkprn),
                    ProviderAllocationHistoryPageBreadCrumb(
                        false,
                        FundingStreamName.ToUIPathComponent(),
                        OrganisationUkprn),
                    ProviderFundingBreakdownPageBreadCrumb(true, ContentTitle)
                };
            }
        }

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle => PageTitle;

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string ContentTitle => PageTitle;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the title in the page's main content section.
        /// </summary>
        public override bool ShowContentTitle => false;

        /// <summary>
        /// Gets the collection of 'related' sections to display.
        /// </summary>
        public override IList<RelatedSectionViewModel> RelatedSections => new List<RelatedSectionViewModel>();

        /// <summary>
        /// Gets or sets the organisation ukprn.
        /// </summary>
        public string OrganisationUkprn { get; set; }

        /// <summary>
        /// Gets or sets the name of the organisation.
        /// </summary>
        public string OrganisationName { get; set; }

        /// <summary>
        /// Gets or sets the code of the funding stream.
        /// </summary>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the search term.
        /// </summary>
        /// <value>
        /// The search term.
        /// </value>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the funding view data for this history.
        /// </summary>
        public FundingViewData FundingViewData { get; set; }

        /// <summary>
        /// Gets the page title.
        /// </summary>
        private string PageTitle => $"{FundingStreamName} {YearFrom} to {YearTo}";
    }
}