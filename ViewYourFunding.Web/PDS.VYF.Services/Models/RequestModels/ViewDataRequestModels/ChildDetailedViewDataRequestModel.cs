namespace PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels
{
    using PDS.ViewYourFunding.Services.Enums;

    /// <summary>
    /// Represents a request model for child detailed view data.
    /// </summary>
    public class ChildDetailedViewDataRequestModel : ViewDataRequestBase
    {
        /// <summary>
        /// Gets the cache key for the request.
        /// </summary>
        public override string CacheKey => $"ChildDetailedViewData-{this.FundingStreamCode}-{this.FundingPeriodCode}-{this.UkprnFromRoute}-{this.PublicationDate?.ToString("yyyy-MM-dd")}-{this.ViaChoicePage}-{this.UserId}-{this.SelectedVarianceOption ?? ViewYourFunding.Services.Enums.VarianceSelectionOption.NoComparison}";

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
        /// Gets or sets the starting year.
        /// </summary>
        public int YearFrom { get; set; }

        /// <summary>
        /// Gets or sets the ending year.
        /// </summary>
        public int YearTo { get; set; }

        /// <summary>
        /// Gets or sets the published date.
        /// </summary>
        public string? PublishedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the request is via the variance page.
        /// </summary>
        public bool ViaVariancePage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include history.
        /// </summary>
        public bool IncludeHistory { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the comparison options.
        /// </summary>
        /// <value>
        /// The comparison options.
        /// </value>
        public Dictionary<VarianceSelectionOption, DateTime>? ComparisonOptions { get; set; }
    }
}
