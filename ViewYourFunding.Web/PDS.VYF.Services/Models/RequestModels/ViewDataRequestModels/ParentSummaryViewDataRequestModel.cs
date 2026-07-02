namespace PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels
{
    /// <summary>
    /// Represents a request model for parent summary view data.
    /// </summary>
    public class ParentSummaryViewDataRequestModel : ViewDataRequestBase
    {
        /// <summary>
        /// Gets the cache key for the view data.
        /// </summary>
        public override string CacheKey => $"Parent-SummaryPage-ViewData-{this.UkprnFromLoggedInUser}-{this.ViaChoicePage}";

        /// <summary>
        /// Gets or sets the UKPRN from the logged-in user.
        /// </summary>
        public string? UkprnFromLoggedInUser { get; set; }
    }
}
