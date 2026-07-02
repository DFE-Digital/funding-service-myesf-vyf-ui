namespace PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels
{
    /// <summary>
    /// Represents a request model for child history view data.
    /// </summary>
    public class ChildHistoryViewDataRequestModel : ViewDataRequestBase
    {
        /// <summary>
        /// Gets or sets the cache key for the view data.
        /// </summary>
        public override string CacheKey => $"Child-HistoryPage-ViewData-{this.FundingStreamCode}-{this.FundingPeriodCode}-{this.UkprnFromRoute}-{this.UkprnFromLoggedInUser}-{this.ViaChoicePage}";

        /// <summary>
        /// Gets or sets the funding stream name path part.
        /// </summary>
        public string? FundingStreamNamePathPart { get; set; }

        /// <summary>
        /// Gets or sets the UKPRN from the logged-in user.
        /// </summary>
        public string? UkprnFromLoggedInUser { get; set; }

        /// <summary>
        /// Gets or sets the UKPRN from the route.
        /// </summary>
        public string? UkprnFromRoute { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether preview mode is enabled.
        /// </summary>
        public bool IsPreviewModeEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value IYO digital live date.
        /// </summary>
        public DateTime? DigitalGoLiveDate { get; set; }
    }
}
