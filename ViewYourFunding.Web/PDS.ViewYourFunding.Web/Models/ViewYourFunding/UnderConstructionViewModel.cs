using PDS.ViewYourFunding.Web.Models.Shared;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The view model for the 'under construction' page.
    /// </summary>
    public class UnderConstructionViewModel : BaseViewYourFundingPageViewModel
    {
        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle => ViewYourFundingConstants.PageTitle_UnderConstruction;

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string ContentTitle => ViewYourFundingConstants.PageTitle_UnderConstruction;
    }
}
