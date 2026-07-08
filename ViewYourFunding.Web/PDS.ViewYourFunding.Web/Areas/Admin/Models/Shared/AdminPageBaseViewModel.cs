using PDS.ViewYourFunding.Web.Models.Shared;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Shared
{
    /// <summary>
    /// The base class for all admin page view models in the View Your Funding area.
    /// </summary>
    public class AdminPageBaseViewModel : BaseViewYourFundingPageViewModel
    {
        /// <summary>
        /// Gets the View your Sub Services Link.
        /// </summary>
        public override string ViewYourSubServicesLink => string.Empty;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the beta tag banner.
        /// </summary>
        public override bool ShowBetaTag => false;

        /// <summary>
        /// Gets a value indicating whether whether or not to show the feedback link in the beta tag banner.
        /// </summary>
        public override bool ShowFeedbackLink => false;
    }
}
