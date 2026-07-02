namespace PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels
{
    /// <summary>
    /// Represents the model for logged-in parent information.
    /// </summary>
    public class LoggedInParentInfoModel
    {
        /// <summary>
        /// Gets or sets the parent ID.
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the parent name.
        /// </summary>
        public string? ParentName { get; set; }

        /// <summary>
        /// Gets or sets the grouping reason.
        /// </summary>
        public string? GroupingReason { get; set; }

        /// <summary>
        /// Gets or sets the parent provider type.
        /// </summary>
        public string? ParentProviderType { get; set; }

        /// <summary>
        /// Gets or sets the grouping reason type.
        /// </summary>
        public string? GroupingReasonType { get; set; }

        /// <summary>
        /// Gets or sets the status changed date.
        /// </summary>
        public string? StatusChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the parent primary identifier.
        /// </summary>
        public string? ParentPrimaryIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the parent UKPRN.
        /// </summary>
        public string? ParentUKPRN { get; set; }
    }
}
