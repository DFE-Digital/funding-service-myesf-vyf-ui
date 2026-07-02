namespace PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels
{
    /// <summary>
    /// Represents a request model for child summary view data.
    /// </summary>
    public class ChildSummaryViewDataRequestModel : ViewDataRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChildSummaryViewDataRequestModel"/> class.
        /// </summary>
        /// <param name="ukprn">The UKPRN.</param>
        /// <param name="userId">The user ID.</param>
        /// <param name="viaChoicePage">A flag indicating if the request is via the choice page.</param>
        public ChildSummaryViewDataRequestModel(string ukprn, string userId, bool viaChoicePage)
        {
            this.UkprnFromLoggedInUser = ukprn;
            this.UserId = userId;
            this.ViaChoicePage = viaChoicePage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildSummaryViewDataRequestModel"/> class.
        /// </summary>
        public ChildSummaryViewDataRequestModel()
        {
        }

        /// <summary>
        /// Gets the cache key for the view data.
        /// </summary>
        public override string CacheKey => $"Child-SummaryPage-ViewData-{this.UserId}-{this.UkprnFromLoggedInUser}-{this.ViaChoicePage}";

        /// <summary>
        /// Gets or sets the UKPRN from the logged-in user.
        /// </summary>
        public string? UkprnFromLoggedInUser { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public string? UserId { get; set; }
    }
}
