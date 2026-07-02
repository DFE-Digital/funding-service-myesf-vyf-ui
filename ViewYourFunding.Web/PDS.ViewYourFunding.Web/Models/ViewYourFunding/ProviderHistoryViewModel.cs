using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Helper;
using System.Collections.Generic;
using System.Web;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The Provider PSG Allocation History view model.
    /// </summary>
    public class ProviderHistoryViewModel : BaseAllocationHistoryViewModel
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
                        ProviderAllocationHistoryPageBreadCrumb(true, FundingStreamName.ToUIPathComponent(), OrganisationUkprn, SearchTerm)
                    };
                }

                return new List<BreadCrumbViewModel>
                {
                    ViewingChoicePageBreadCrumb(false),
                    FindAnOrganisationPageBreadCrumb(false),
                    ProviderPageBreadCrumb(false, OrganisationName, OrganisationUkprn),
                    ProviderAllocationHistoryPageBreadCrumb(true, FundingStreamName.ToUIPathComponent(), OrganisationUkprn)
                };
            }
        }

        /// <summary>
        /// Gets or sets the name of the organisation.
        /// </summary>
        public string OrganisationName { get; set; }

        /// <summary>
        /// Gets or sets the organisation ukprn.
        /// </summary>
        public string OrganisationUkprn { get; set; }

        /// <summary>
        /// Gets or sets the name of the funding stream.
        /// </summary>
        /// <value>
        /// The name of the funding stream.
        /// </value>
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets the funding view data for this history.
        /// </summary>
        public FundingViewData FundingViewData { get; set; }
    }
}