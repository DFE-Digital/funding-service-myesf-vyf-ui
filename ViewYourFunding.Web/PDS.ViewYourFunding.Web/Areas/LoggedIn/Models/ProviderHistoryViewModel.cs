using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStream;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Models
{
    /// <summary>
    /// Logged in provider statement page view model.
    /// </summary>
    public class ProviderHistoryViewModel : LoggedInProviderBasePage
    {
        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
        {
            get
            {
                var items = SecondLevelBaseBreadCrumbItems(false);
                items.Add(ProviderAllocationHistoryBreadcrumb(true, FundingStream.FundingStreamName.ToUIPathComponent(), OrganisationUkprn, ViaChoicePage));
                return items;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not to use the two-thirds column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

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

                return LoggedInConstants.BrowserTitle_AllocationStatements;
            }
        }

        /// <summary>
        /// Gets or sets the funding view data for this history.
        /// </summary>
        public FundingViewData FundingViewData { get; set; }

        /// <summary>
        /// Gets or sets the organisation ukprn.
        /// </summary>
        public string OrganisationUkprn { get; set; }

        /// <summary>
        /// Gets or sets the secondary title in the page's main content section.
        /// </summary>
        public string SecondaryContentTitle { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the funding stream.
        /// </summary>
        public FundingStream FundingStream { get; set; }

        /// <summary>
        /// Gets or sets a list of key-value pairs mapping the funding period years (e.g. "2020", "2021") to the list of the publications for that period.
        /// </summary>
        public List<KeyValuePair<(int yearFrom, int yearTo), List<Services.Models.Publication>>> FundingPeriodPublications { get; set; }


        /// <summary>
        /// Gets or sets a list of key-value pairs mapping the funding period years (e.g. "2020", "2021") to the list of the provider funding for that period.
        /// </summary>
        public List<KeyValuePair<(int yearFrom, int yearTo), List<ProviderFundingViewModel>>> FundingPeriodProviderFundings { get; set; }
    }
}