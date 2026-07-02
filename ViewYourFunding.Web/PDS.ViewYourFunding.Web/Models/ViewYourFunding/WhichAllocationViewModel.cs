using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Models.Shared;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The view model to use for the 'Select a funding type' page.
    /// </summary>
    public class WhichAllocationViewModel : BaseViewYourFundingPageViewModel
    {
        /// <summary>
        /// Gets the title to use in the html 'title' tag.
        /// </summary>
        public override string BrowserTitle => ViewYourFundingConstants.PageTitle_WhichAllocation;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the title in the page's main content section.
        /// </summary>
        public override bool ShowContentTitle => false;

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string ContentTitle => ViewYourFundingConstants.PageTitle_WhichAllocation;

        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems => new List<BreadCrumbViewModel>
        {
            ViewingChoicePageBreadCrumb(false),
            WhichAllocationPageBreadCrumb(true)
        };

        /// <summary>
        /// Gets or sets a value indicating whether whether or not there is a validation error on the page.
        /// </summary>
        public bool ValidationError { get; set; }

        /// <summary>
        /// Gets or sets the latest year for which to show allocations for funding stream.
        /// </summary>
        public List<FundingStreamCurrentYearViewModel> LatestYear { get; set; }

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