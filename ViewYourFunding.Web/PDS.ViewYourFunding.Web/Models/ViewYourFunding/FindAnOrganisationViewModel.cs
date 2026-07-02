using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Models.Shared;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The find an organisation view model.
    /// </summary>
    /// <seealso cref="BaseViewYourFundingPageViewModel" />
    public class FindAnOrganisationViewModel : BaseViewYourFundingPageViewModel
    {
        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle => ViewYourFundingConstants.PageTitle_FindAnOrganisation;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the title in the page's main content section.
        /// </summary>
        public override bool ShowContentTitle => false;

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string ContentTitle => ViewYourFundingConstants.PageTitle_FindAnOrganisation;

        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems => new List<BreadCrumbViewModel>
        {
            ViewingChoicePageBreadCrumb(false),
            FindAnOrganisationPageBreadCrumb(true)
        };

        /// <summary>
        /// Gets or sets which search box has been submitted.
        /// </summary>
        public string ValidationErrorInputId { get; set; }

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
