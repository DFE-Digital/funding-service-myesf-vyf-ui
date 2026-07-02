namespace PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels
{
    /// <summary>
    /// Represents the response model for a logged-in parent.
    /// </summary>
    public class LoggedInParentModel
    {
        /// <summary>
        /// Gets or sets the ID of the parent.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the grouping type of the parent.
        /// </summary>
        public string? GroupingType { get; set; }

        /// <summary>
        /// Gets or sets the grouping reason of the parent.
        /// </summary>
        public string? GroupingReason { get; set; }

        /// <summary>
        /// Gets or sets the funding stream grouping type reason of the parent.
        /// </summary>
        public string? FundingStreamGroupingTypeReason { get; set; }

        /// <summary>
        /// Gets or sets the funding period code of the parent.
        /// </summary>
        public string? FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream code of the parent.
        /// </summary>
        public string? FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream period of the parent.
        /// </summary>
        public string? FundingStreamPeriod { get; set; }

        /// <summary>
        /// Gets or sets the status changed date of the parent.
        /// </summary>
        public DateTime? StatusChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the funding version of the parent.
        /// </summary>
        public string? FundingVersion { get; set; }

        /// <summary>
        /// Gets or sets the funding version as an integer of the parent.
        /// </summary>
        public int? FundingVersionInt { get; set; }

        /// <summary>
        /// Gets or sets the schema version of the parent.
        /// </summary>
        public string? SchemaVersion { get; set; }

        /// <summary>
        /// Gets or sets the template version of the parent.
        /// </summary>
        public string? TemplateVersion { get; set; }

        /// <summary>
        /// Gets or sets the group name of the parent.
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// Gets or sets the searchable group name of the parent.
        /// </summary>
        public string? SearchableGroupName { get; set; }

        /// <summary>
        /// Gets or sets the group UKPRN of the parent.
        /// </summary>
        public string? GroupUkprn { get; set; }

        /// <summary>
        /// Gets or sets the group code of the parent.
        /// </summary>
        public string? GroupCode { get; set; }

        /// <summary>
        /// Gets or sets the total amount of the parent.
        /// </summary>
        public double? TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the funding lines of the parent.
        /// </summary>
        public IEnumerable<LoggedInTemplateLine>? FundingLines { get; set; }

        /// <summary>
        /// Gets or sets the calculations of the parent.
        /// </summary>
        public IEnumerable<LoggedInCalculation>? Calculations { get; set; }

        /// <summary>
        /// Gets or sets the provider fundings of the parent.
        /// </summary>
        public IEnumerable<string>? ProviderFundings { get; set; }

        /// <summary>
        /// Gets or sets the child UKPRNs of the parent.
        /// </summary>
        public IEnumerable<string>? ChildUKPRNs { get; set; }

        /// <summary>
        /// Gets or sets the variation reasons of the parent.
        /// </summary>
        public IEnumerable<string>? VariationReasons { get; set; }

        /// <summary>
        /// Gets or sets the statement channel version of the parent.
        /// </summary>
        public int? StatementChannelVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parent is a parent.
        /// </summary>
        public bool? IsParent { get; set; }
    }
}
