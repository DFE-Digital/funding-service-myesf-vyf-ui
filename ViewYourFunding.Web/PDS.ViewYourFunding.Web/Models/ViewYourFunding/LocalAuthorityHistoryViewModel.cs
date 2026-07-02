using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using System.Collections.Generic;
using System.Web;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The shared view model for all local authority 'allocation history' pages.
    /// </summary>
    public class LocalAuthorityHistoryViewModel : BaseAllocationHistoryViewModel
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
                        LocalAuthorityDidYouMeanPageBreadCrumb(false, SearchTerm),
                        LocalAuthorityStatementPageBreadCrumb(false, LocalAuthorityCode, LocalAuthorityName, SearchTerm),
                        LocalAuthorityAllocationHistoryPageBreadCrumb(true, null, LocalAuthorityCode, FundingStreamName, SearchTerm)
                    };
                }

                return new List<BreadCrumbViewModel>
                {
                    ViewingChoicePageBreadCrumb(false),
                    FindAnOrganisationPageBreadCrumb(false),
                    LocalAuthorityStatementPageBreadCrumb(false, LocalAuthorityCode, LocalAuthorityName, SearchTerm),
                    LocalAuthorityAllocationHistoryPageBreadCrumb(true, null, LocalAuthorityCode, FundingStreamName)
                };
            }
        }

        /// <summary>
        /// Gets or sets local authority name e.g. Camden.
        /// </summary>
        public string LocalAuthorityName { get; set; }

        /// <summary>
        /// Gets or sets local authority code e.g. 202.
        /// </summary>
        public string LocalAuthorityCode { get; set; }

        /// <summary>
        /// Gets or sets the funding view data.
        /// </summary>
        public FundingViewData FundingViewData { get; set; }

        /// <summary>
        /// Gets or sets the name of the funding stream.
        /// </summary>
        public string FundingStreamName { get; set; }
    }
}