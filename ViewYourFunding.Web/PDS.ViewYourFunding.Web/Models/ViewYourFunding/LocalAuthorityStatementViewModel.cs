using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System.Collections.Generic;
using System.Web;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// View model for the local authority statement page.
    /// </summary>
    public class LocalAuthorityStatementViewModel : SearchResultsViewModel
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
                        LocalAuthorityStatementPageBreadCrumb(true, LocalAuthorityCode, LocalAuthorityName, SearchTerm)
                    };
                }

                return new List<BreadCrumbViewModel>
                {
                    ViewingChoicePageBreadCrumb(false),
                    FindAnOrganisationPageBreadCrumb(false),
                    LocalAuthorityStatementPageBreadCrumb(true, LocalAuthorityCode, LocalAuthorityName, SearchTerm)
                };
            }
        }

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the title in the page's main content section.
        /// </summary>
        public override bool ShowContentTitle => false;

        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle => LocalAuthorityName;

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string ContentTitle => LocalAuthorityName;

        /// <summary>
        /// Gets or sets the local authority code.
        /// </summary>
        public string LocalAuthorityCode { get; set; }

        /// <summary>
        /// Gets or sets the local authority name.
        /// </summary>
        public string LocalAuthorityName { get; set; }

        /// <summary>
        /// Gets or sets the funding stream configuration.
        /// </summary>
        public Dictionary<string, FundingStream.FundingStream> FundingStreamConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the funding view data for the local authority.
        /// </summary>
        public Dictionary<string, FundingViewData> FundingViewData { get; set; }

        /// <summary>
        /// Gets or sets the funding documents.
        /// </summary>
        public IReadOnlyCollection<FundingDocument> FundingDocuments { get; set; }

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