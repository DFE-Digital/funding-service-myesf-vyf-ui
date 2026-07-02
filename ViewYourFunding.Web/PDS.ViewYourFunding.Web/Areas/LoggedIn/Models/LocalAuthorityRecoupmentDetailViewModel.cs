using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Models
{
    /// <summary>
    /// Logged in local authority recoupment detail page view model.
    /// </summary>
    public class LocalAuthorityRecoupmentDetailViewModel : LoggedInProviderBasePage
    {
        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
        {
            get
            {
                var items = new List<BreadCrumbViewModel>
                {
                    LoggedInHomePageBreadCrumb()
                };

                items.Add(LocalAuthorityRecoupmentSummaryPageBreadCrumb(false));
                items.Add(LocalAuthorityRecoupmentDetailPageBreadCrumb("Recoupment from the dedicated schools grant (DSG)", OrganisationUkPrn, PublishedDate, YearFrom, YearTo, true));
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
        public override string BrowserTitle => LoggedInConstants.BrowserTitle_RecoupmentReports;

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
        /// Gets or sets a value indicating whether include history allocation page.
        /// </summary>
        public bool IncludeHistory { get; set; }

        /// <summary>
        /// Gets or sets published date.
        /// </summary>
        public string PublishedDate { get; set; }

        /// <summary>
        /// Gets or sets tab that was selected to come through to funding breakdown page.
        /// </summary>
        public string Tab { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the funding stream.
        /// </summary>
        public FundingStream FundingStream { get; set; }

        /// <summary>
        /// Gets or sets the downloadable document for this funding breakdown.
        /// </summary>
        public FundingDocument Document { get; set; }

        /// <summary>
        /// Gets get the downloadable document's title.
        /// </summary>
        public string DocumentTitle =>
            string.Format(ViewYourFundingConstants.DocumentTitle_LARECFundingRecoupment, OrganisationName, YearFrom, YearTo, FundingStream.FundingStreamCode);
    }
}
