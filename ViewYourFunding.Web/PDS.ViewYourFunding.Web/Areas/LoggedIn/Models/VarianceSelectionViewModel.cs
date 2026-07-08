using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Models
{
    /// <summary>
    /// Logged in provider variance selection page view model.
    /// </summary>
    public class VarianceSelectionViewModel : LoggedInProviderBasePage
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
                    items.Add(ProviderAllocationHistoryBreadcrumb(false, FundingStream.FundingStreamName.ToUIPathComponent(), OrganisationUkPrn, ViaChoicePage));
                }

                return items;
            }
        }



        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        /// <summary>
        /// Gets the title of the page.
        /// </summary>
        public override string ContentTitle => LoggedInConstants.PageTitle_VarianceSelection;

        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle => LoggedInConstants.BrowserTitle_VarianceSelection;

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string HeaderTitle => LoggedInConstants.HeaderTitle_StandardMYESFHeader;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the beta tag banner.
        /// </summary>
        public override bool ShowBetaTag => true;

        /// <inheritdoc/>
        public override string HeaderLink => "/";

        /// <summary>
        /// Gets or sets the configuration for the funding stream.
        /// </summary>
        public FundingStream FundingStream { get; set; }

        /// <summary>
        /// Gets or sets the list of options on variance page.
        /// </summary>
        [Required]
        public List<VarianceSelectionOption> Options { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether or not there is a validation error on the page.
        /// </summary>
        public bool ValidationError { get; set; }

        /// <summary>
        /// Gets or sets the start year of the funding period being displayed.
        /// </summary>
        public int YearPrevious { get; set; }

        /// <summary>
        /// Gets or sets previous year final statement published date .e.g. 2020-01-01.
        /// </summary>
        public DateTime FinalStatementPublishedDate { get; set; }

        /// <summary>
        /// Gets or sets previous statement for current year published date .e.g. 2020-01-01.
        /// </summary>
        public DateTime PreviousStatementPublishedDate { get; set; }

        /// <summary>
        /// Gets or sets the funding stream name.
        /// </summary>
        public string FundingStreamNamePathPart { get; set; }

        /// <summary>
        /// Gets or sets ukprn.
        /// </summary>
        public string Ukprn { get; set; }

        /// <summary>
        /// Gets or sets year from (e.g. 2020).
        /// </summary>
        public int YearFrom { get; set; }

        /// <summary>
        /// Gets or sets year to (e.g. 2020).
        /// </summary>
        public int YearTo { get; set; }

        /// <summary>
        /// Gets or sets published date .e.g. 2020-01-01.
        /// </summary>
        public string PublishedDate { get; set; }

        /// <summary>
        /// Gets or sets tab that was selected to come through to funding breakdown page.
        /// </summary>
        public string Tab { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether include history allocation page.
        /// </summary>
        public bool IncludeHistory { get; set; }

        /// <summary>
        /// Gets or sets the organisation uk PRN.
        /// </summary>
        /// <value>
        /// The organisation uk PRN.
        /// </value>
        public string OrganisationUkPrn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user entered via the variance page.
        /// </summary>
        public bool ViaVariancePage { get; set; }
    }
}