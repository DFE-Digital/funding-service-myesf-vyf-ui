using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Models
{
    /// <summary>
    /// Logged in local authority funding breakdown page view model.
    /// </summary>
    public class LocalAuthorityBreakdownViewModel : LoggedInProviderBasePage
    {
        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
        {
            get
            {
                var items = SecondLevelBaseBreadCrumbItems(false);
                if (IncludeHistory)
                {
                    items.Add(LocalAuthorityAllocationHistoryBreadcrumb(false, FundingStream.FundingStreamName.ToUIPathComponent(), OrganisationUkPrn));
                }

                items.Add(LocalAuthorityBreakdownPageBreadCrumb("School sixth form funding", true, YearFrom, YearTo));
                return items;
            }
        }

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle => LoggedInConstants.BrowserTitle_AllocationStatements;

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string HeaderTitle => LoggedInConstants.HeaderTitle_StandardMYESFHeader;

        /// <summary>
        /// Gets or sets the funding view data for the local authority.
        /// </summary>
        public FundingViewData FundingViewData { get; set; }

        /// <summary>
        /// Gets or sets the funding.
        /// </summary>
        /// <value>
        /// The funding.
        /// </value>
        public IFundingApiSearchFunding Funding { get; set; }

        /// <summary>
        /// Gets a value indicating whether whether or not to show the beta tag banner.
        /// </summary>
        public override bool ShowBetaTag => true;

        /// <inheritdoc/>
        public override string HeaderLink => "/";

        /// <summary>
        /// Gets or sets the start year of the funding period being displayed.
        /// </summary>
        public int YearFrom { get; set; }

        /// <summary>
        /// Gets or sets the end year of the funding period being displayed.
        /// </summary>
        public int YearTo { get; set; }

        /// <summary>
        /// Gets or sets the organisation uk PRN.
        /// </summary>
        /// <value>
        /// The organisation uk PRN.
        /// </value>
        public string OrganisationUkPrn { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the funding stream.
        /// </summary>
        public FundingStream FundingStream { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether include history allocation page.
        /// </summary>
        public bool IncludeHistory { get; set; }
    }
}