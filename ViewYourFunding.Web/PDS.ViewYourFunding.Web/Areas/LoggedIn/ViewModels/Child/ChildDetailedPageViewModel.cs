using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.ViewModels.Child
{
    public class ChildDetailedPageViewModel : LoggedInBasePageViewModel
    {
        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle
        {
            get
            {
                if (DisplayUnauthorisedAccessErrorMessage)
                {
                    return LoggedInConstants.BrowserTitle_UnauthorizedAccess;
                }

                return string.Format(
                ViewYourFundingConstants.BrowserTitleShortFormat_Common,
                this.YearFrom,
                this.YearTo,
                this.FundingStreamName);
            }
        }

        /// <summary>
        /// Gets a value indicating whether whether or not to show the title in the page's main content section.
        /// </summary>
        public override bool ShowContentTitle => false;

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string ContentTitle =>
            string.Format(
                ViewYourFundingConstants.ContentTitleFormat_Common(
                    this.FundingStreamCode,
                    this.FundingStreamName,
                    false),
                YearFrom,
                YearTo);

        /// <summary>
        /// Gets a value indicating whether use full column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

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
                    items.Add(ProviderAllocationHistoryBreadcrumb(false, this.FundingStreamNamePathPart, OrganisationUkPrn, ViaChoicePage));
                }

                var indicativeText = IsIndicative ? "Indicative " : string.Empty;
                items.Add(ProviderFundingBreakDownBreadcrumb(true, $"{indicativeText}{ContentTitle}"));

                return items;
            }
        }

        /// <summary>
        /// Gets a value indicating whether whether or not to show the beta tag banner.
        /// </summary>
        public override bool ShowBetaTag => true;

        // Domain specific properties

        /// <summary>
        /// Gets or sets the organisation uk PRN.
        /// </summary>
        /// <value>
        /// The organisation uk PRN.
        /// </value>
        public string OrganisationUkPrn { get; set; }

        /// <summary>
        /// Gets or sets the start year of the funding period being displayed.
        /// </summary>
        public int YearFrom { get; set; }

        /// <summary>
        /// Gets or sets the end year of the funding period being displayed.
        /// </summary>
        public int YearTo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is this the latest (or final) funding for the year being shown?.
        /// </summary>
        public bool IsLatestOrFinalFundingForYear { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is the current year's funding being shown?.
        /// </summary>
        public bool IsCurrentYear { get; set; }

        /// <summary>
        /// Gets or sets the downloadable document for this funding breakdown.
        /// </summary>
        public FundingDocument Document { get; set; }

        /// <summary>
        /// Gets get the downloadable document's title.
        /// </summary>
        public string DocumentTitle =>
            string.Format(ViewYourFundingConstants.DocumentTitle_LAFundingBreakdown, OrganisationName, YearFrom, YearTo, this.FundingStreamCode);

        /// <summary>
        /// Gets or sets the configuration for the funding stream.
        /// </summary>
        public FundingStream FundingStream { get; set; }

        public string FundingStreamCode { get; set; }

        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets the funding view data for this funding breakdown.
        /// </summary>
        public FundingViewData FundingViewData { get; set; }

        /// <summary>
        /// Gets or sets tab that was selected to come through to funding breakdown page, if it has tabs.
        /// </summary>
        public string Tab { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether include history allocation page.
        /// </summary>
        public bool IncludeHistory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user entered via the variance page.
        /// </summary>
        public bool ViaVariancePage { get; set; }

        /// <summary>
        /// Gets or sets the funding stream name.
        /// </summary>
        public string FundingStreamNamePathPart { get; set; }

        /// <summary>
        /// Gets or sets published date .e.g. 2020-01-01.
        /// </summary>
        public string PublishedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether funding status is indicative.
        /// </summary>
        public bool IsIndicative { get; set; }
    }
}
