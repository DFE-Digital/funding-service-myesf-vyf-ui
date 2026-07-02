using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Models
{
    /// <summary>
    /// Logged in provider statement page view model.
    /// </summary>
    public class ProviderStatementViewModel : LoggedInProviderBasePage
    {
        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
        {
            get
            {
                return SecondLevelBaseBreadCrumbItems(true);
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
        /// Gets or sets the provider funding view data for the provider.
        /// </summary>
        public Dictionary<string, FundingViewData> ProviderFundingViewData { get; set; } = new Dictionary<string, FundingViewData>();

        /// <summary>
        /// Gets or sets the funding view data for the provider.
        /// </summary>
        public Dictionary<string, FundingViewData> FundingViewData { get; set; } = new Dictionary<string, FundingViewData>();

        /// <summary>
        /// Gets a value indicating whether whether or not to show the beta tag banner.
        /// </summary>
        public override bool ShowBetaTag => true;

        /// <inheritdoc/>
        public override string HeaderLink => "/";

        /// <summary>
        /// Gets or sets a value indicating whether to display the no allocation message.
        /// </summary>
        public bool DisplayNoAllocationMessage { get; set; }
    }
}