namespace PDS.VYF.Services.Models.ResponseModels.ViewDataResponseModels
{
    using PDS.ViewYourFunding.Services.DTOs;

    /// <summary>
    /// Represents a detailed view data response model for a child.
    /// </summary>
    public class ChildDetailedViewDataResponseModel : ViewDataResponseModelBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the funding is indicative.
        /// </summary>
        public bool IsIndicative { get; set; }

        /// <summary>
        /// Gets or sets the funding view data.
        /// </summary>
        public FundingViewData? FundingViewData { get; set; }

        /// <summary>
        /// Gets or sets the funding stream code.
        /// </summary>
        public string? FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream name.
        /// </summary>
        public string? FundingStreamName { get; set; }
    }
}
