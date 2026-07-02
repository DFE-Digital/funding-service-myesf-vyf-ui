using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.ResponseObjects;
using PDS.ViewYourFunding.Web.Models.Shared;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The view model to use for the National Funding Stream Allocations download page.
    /// </summary>
    public class NationalFundingAllocationViewModel : BaseViewYourFundingPageViewModel
    {
        #region Browser Title and Breadcumbs

        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle =>
            string.Format(ViewYourFundingConstants.BrowserTitleFormat_Common, YearFrom, YearTo, FundingStreamName, FundingStreamCode, CanUseShortCode);

        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems =>
            new List<BreadCrumbViewModel>
            {
                ViewingChoicePageBreadCrumb(false),
                WhichAllocationPageBreadCrumb(false),
                NationalFundingAllocationPageBreadCrumb(true, FundingStreamCode, FundingStreamName, YearFrom, YearTo, CanUseShortCode)
            };

        #endregion Browser Title and Breadcumbs

        #region Funding stream data

        /// <summary>
        /// Gets or sets the Funding Stream Code for the funding stream allocations data.
        /// </summary>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the funding stream short code can be used.
        /// </summary>
        public bool CanUseShortCode { get; set; }

        /// <summary>
        /// Gets or sets the Funding Stream Name for the funding stream allocations data.
        /// </summary>
        public string FundingStreamName { get; set; }

        /// <summary>
        ///  Gets or sets the start year of the funding period being displayed.
        /// </summary>
        public int YearFrom { get; set; }

        /// <summary>
        /// Gets or sets the end year of the funding period being displayed.
        /// </summary>
        public int YearTo { get; set; }

        /// <summary>
        /// Gets or sets the collection of spreadsheet documents, each of which should have a link on the page.
        /// </summary>
        public IReadOnlyCollection<FundingDocument> Spreadsheets { get; set; }

        #endregion Funding stream data


        #region Historic allocation links

        /// <summary>
        /// Gets a value indicating whether or not to show the 'related' sections.
        /// </summary>
        public override bool ShowRelatedSections => false;

        /// <summary>
        /// Gets a value indicating whether or not to use the two thirds layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion Historic allocation links


        #region Page Layout

        /// <summary>
        /// Gets or sets the downloadable document for this funding breakdown.
        /// </summary>
        public FundingDocument Document { get; set; }

        /// <summary>
        /// Gets or sets the funding view data for this funding allocation.
        /// </summary>
        public FundingViewData FundingViewData { get; set; }

        #endregion Page Layout


        #region Validation Errors

        /// <summary>
        /// Gets or sets a value indicating whether or not there is a validation error on the page.
        /// </summary>
        public bool ValidationError { get; set; }

        /// <summary>
        /// Gets or sets the ValidationError message.
        /// </summary>
        public string ValidationErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the historic years.
        /// </summary>
        /// <value>
        /// The historic years.
        /// </value>
        public IList<(int yearFrom, int yearTo)> HistoricYears { get; set; } = new List<(int yearFrom, int yearTo)>();

        /// <summary>
        /// Gets or sets the latest historic years.
        /// </summary>
        /// <value>
        /// The latest historic years.
        /// </value>
        public IList<(int yearFrom, int yearTo)> LatestHistoricYears { get; set; } = new List<(int yearFrom, int yearTo)>();

        #endregion


        #region Banners

        /// <summary>
        /// Gets a value indicating whether or not to show the notification banner.
        /// </summary>
        public override bool ShowNotificationBanner => false;

        /// <summary>
        /// Gets the text in the banner heading section.
        /// </summary>
        public override string NotificationBannerHeadingText => ViewYourFundingConstants.DsgNotificationBannerHeadingText;

        /// <summary>
        /// Gets the text in the banner heading section.
        /// </summary>
        public override string NotificationBannerBodyText => ViewYourFundingConstants.DsgNotificationBannerBodyText;

        #endregion Banners
    }
}
