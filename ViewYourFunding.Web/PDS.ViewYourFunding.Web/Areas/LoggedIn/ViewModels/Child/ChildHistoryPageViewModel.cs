using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.ViewModels.Child
{
    public class ChildHistoryPageViewModel : LoggedInBasePageViewModel
    {
        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
        {
            get
            {
                if (DisplayUnauthorisedAccessErrorMessage)
                {
                    return new List<BreadCrumbViewModel>() { LoggedInHomePageBreadCrumb() };
                }

                var items = SecondLevelBaseBreadCrumbItems(false);
                items.Add(ProviderAllocationHistoryBreadcrumb(true, FundingStreamNamePathPart, OrganisationUkprn, ViaChoicePage));
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
            => DisplayUnauthorisedAccessErrorMessage ? LoggedInConstants.BrowserTitle_UnauthorizedAccess : LoggedInConstants.BrowserTitle_AllocationStatements;

        /// <summary>
        /// Gets or sets the funding view data for this history.
        /// </summary>
        public FundingViewData FundingViewData { get; set; }

        /// <summary>
        /// Gets or sets the organisation ukprn.
        /// </summary>
        public string OrganisationUkprn { get; set; }

        /// <summary>
        /// Gets or sets the funding stream name path part.
        /// </summary>
        /// <value>
        /// The funding stream name path part.
        /// </value>
        public string FundingStreamNamePathPart { get; set; }
    }
}
