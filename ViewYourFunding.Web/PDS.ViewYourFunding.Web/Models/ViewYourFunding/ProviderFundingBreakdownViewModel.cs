using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Web.Models.Shared;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The Provider Funding breakdown view model.
    /// </summary>
    public class ProviderFundingBreakdownViewModel : BaseViewYourFundingPageViewModel
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
                    return new List<BreadCrumbViewModel>
                    {
                        ViewingChoicePageBreadCrumb(false),
                        FindAnOrganisationPageBreadCrumb(false),
                        ProviderResultsPageBreadCrumb(false, SearchTerm),
                        ProviderPageBreadCrumb(
                            false,
                            ProviderStatementSection.ProviderResult.OrganisationName,
                            ProviderStatementSection.ProviderResult.OrganisationUkprn,
                            SearchTerm),
                        ProviderAllocationHistoryPageBreadCrumb(
                            false,
                            FundingStreamName.ToUIPathComponent(),
                            ProviderStatementSection.ProviderResult.OrganisationUkprn,
                            SearchTerm),
                        ProviderFundingBreakdownPageBreadCrumb(true, ContentTitle)
                    };
                }

                return new List<BreadCrumbViewModel>
                {
                    ViewingChoicePageBreadCrumb(false),
                    FindAnOrganisationPageBreadCrumb(false),
                    ProviderPageBreadCrumb(
                        false,
                        ProviderStatementSection.ProviderResult.OrganisationName,
                        ProviderStatementSection.ProviderResult.OrganisationUkprn),
                    ProviderAllocationHistoryPageBreadCrumb(
                        false,
                        FundingStreamName.ToUIPathComponent(),
                        ProviderStatementSection.ProviderResult.OrganisationUkprn),
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
        /// Gets or sets the provider statement section.
        /// </summary>
        /// <value>
        /// The provider statement section.
        /// </value>
        public ProviderStatementSectionViewModel ProviderStatementSection { get; set; }

        /// <summary>
        /// Gets or sets the start year of the funding period being displayed.
        /// </summary>
        public int YearFrom { get; set; }

        /// <summary>
        /// Gets or sets the end year of the funding period being displayed.
        /// </summary>
        public int YearTo { get; set; }

        /// <summary>
        /// Gets or sets the name of the funding stream.
        /// </summary>
        /// <value>
        /// The name of the funding stream.
        /// </value>
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets the funding status.
        /// </summary>
        /// <value>
        /// The funding status.
        /// </value>
        public string FundingStatus { get; set; }

        /// <summary>
        /// Gets or sets the not latest CSS class.
        /// </summary>
        /// <value>
        /// The not latest CSS class.
        /// </value>
        public string NotLatestCssClass { get; set; } = "govuk-bggrey";

        /// <summary>
        /// Gets or sets the search term.
        /// </summary>
        /// <value>
        /// The search term.
        /// </value>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        public DateTime PublicationDate { get; set; }

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