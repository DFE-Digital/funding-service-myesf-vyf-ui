using Pds.Core.Web.Models;
using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using System.Collections.Generic;
using System.Web;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The local authoirty Funding Breakdown single year view model.
    /// </summary>
    /// <seealso cref="Shared.BaseViewYourFundingPageViewModel" />
    public class LocalAuthorityHistorySingleYearViewModel : DownloadViewModel
    {
        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
        {
            get
            {
                if (string.IsNullOrEmpty(SearchTerm))
                {
                    return new List<BreadCrumbViewModel>()
                    {
                        ViewingChoicePageBreadCrumb(false),
                        FindAnOrganisationPageBreadCrumb(false),
                        LocalAuthorityStatementPageBreadCrumb(false, LocalAuthorityCode, LocalAuthorityName),
                        LocalAuthorityAllocationHistoryPageBreadCrumb(
                            false,
                            ViewYourFundingConstants.RouteName_LocalAuthorityHistory,
                            LocalAuthorityCode,
                            FundingStreamName),
                        LocalAuthorityBreakdownPageBreadCrumb(FundingStreamName, true, YearFrom, YearTo),
                    };
                }
                else
                {
                    SearchTerm = HttpUtility.HtmlDecode(SearchTerm);
                    return new List<BreadCrumbViewModel>
                    {
                        ViewingChoicePageBreadCrumb(false),
                        FindAnOrganisationPageBreadCrumb(false),
                        LocalAuthorityDidYouMeanPageBreadCrumb(false, SearchTerm),
                        LocalAuthorityStatementPageBreadCrumb(false, LocalAuthorityCode, LocalAuthorityName, SearchTerm),
                        LocalAuthorityAllocationHistoryPageBreadCrumb(
                            false,
                            ViewYourFundingConstants.RouteName_LocalAuthorityHistory,
                            LocalAuthorityCode,
                            FundingStreamName,
                            SearchTerm),
                        LocalAuthorityBreakdownPageBreadCrumb(FundingStreamName, true, YearFrom, YearTo),
                    };
                }
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
        public new IList<RelatedSectionViewModel> RelatedSections => new List<RelatedSectionViewModel>();

        /// <summary>
        /// Gets or sets the local authority code.
        /// </summary>
        public string LocalAuthorityCode { get; set; }

        /// <summary>
        /// Gets or sets the local authority name.
        /// </summary>
        public string LocalAuthorityName { get; set; }

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
        /// Gets or sets the funding view data for this funding breakdown.
        /// </summary>
        public FundingViewData FundingViewData { get; set; }

        /// <summary>
        /// Gets the page title.
        /// </summary>
        private string PageTitle => $"{FundingStreamName} {YearFrom} to {YearTo}";
    }
}