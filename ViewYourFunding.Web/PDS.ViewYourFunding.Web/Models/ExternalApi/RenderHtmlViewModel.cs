using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Web.Models.Shared;

namespace PDS.ViewYourFunding.Web.Models
{
    /// <summary>
    /// The Render Html base page view model.
    /// </summary>
    /// <seealso cref="Shared.BasePageViewModel" />
    public class RenderHtmlViewModel : BaseViewYourFundingPageViewModel
    {
        /// <summary>
        /// Gets or sets the funding view data for this funding breakdown.
        /// </summary>
        public FundingViewData FundingViewData { get; set; }
    }
}