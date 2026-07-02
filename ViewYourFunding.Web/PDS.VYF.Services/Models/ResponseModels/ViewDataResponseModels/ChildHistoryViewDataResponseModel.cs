namespace PDS.VYF.Services.Models.ResponseModels.ViewDataResponseModels
{
    using PDS.ViewYourFunding.Services.DTOs;

    /// <summary>
    /// The child history view data response model.
    /// </summary>
    /// <seealso cref="PDS.VYF.Services.Models.ResponseModels.ViewDataResponseModels.ViewDataResponseModelBase" />
    public class ChildHistoryViewDataResponseModel : ViewDataResponseModelBase
    {
        /// <summary>
        /// Gets or sets the funding view data.
        /// </summary>
        /// <value>
        /// The funding view data.
        /// </value>
        public FundingViewData? FundingViewData { get; set; }
    }
}
