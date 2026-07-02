namespace PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels
{
    /// <summary>
    /// Represents a logged-in child model.
    /// </summary>
    public class LoggedInChildModel
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the ID without version.
        /// </summary>
        public string? IdWithoutVersion { get; set; }

        /// <summary>
        /// Gets or sets the parent information.
        /// </summary>
        public List<LoggedInParentInfoModel>? ParentInfo { get; set; }

        /// <summary>
        /// Gets or sets the funding period code.
        /// </summary>
        public string? FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream code.
        /// </summary>
        public string? FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream period.
        /// </summary>
        public string? FundingStreamPeriod { get; set; }

        /// <summary>
        /// Gets or sets the year from.
        /// </summary>
        public int? YearFrom { get; set; }

        /// <summary>
        /// Gets or sets the year to.
        /// </summary>
        public int? YearTo { get; set; }

        /// <summary>
        /// Gets or sets the status changed date.
        /// </summary>
        public DateTime? StatusChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the status changed date only.
        /// </summary>
        public DateTime? StatusChangedDateOnly { get; set; }

        /// <summary>
        /// Gets or sets the funding version.
        /// </summary>
        public string? FundingVersion { get; set; }

        /// <summary>
        /// Gets or sets the funding version integer.
        /// </summary>
        public string? FundingVersionInt { get; set; }

        /// <summary>
        /// Gets or sets the template version.
        /// </summary>
        public string? TemplateVersion { get; set; }

        /// <summary>
        /// Gets or sets the schema version.
        /// </summary>
        public string? SchemaVersion { get; set; }

        /// <summary>
        /// Gets or sets the organisation name.
        /// </summary>
        public string? OrganisationName { get; set; }

        /// <summary>
        /// Gets or sets the searchable organisation name.
        /// </summary>
        public string? SearchableOrganisationName { get; set; }

        /// <summary>
        /// Gets or sets the organisation UKPRN.
        /// </summary>
        public string? OrganisationUkprn { get; set; }

        /// <summary>
        /// Gets or sets the provider URN.
        /// </summary>
        public string? ProviderUrn { get; set; }

        /// <summary>
        /// Gets or sets the organisation DFE number.
        /// </summary>
        public string? OrganisationDfeNumber { get; set; }

        /// <summary>
        /// Gets or sets the organisation town.
        /// </summary>
        public string? OrganisationTown { get; set; }

        /// <summary>
        /// Gets or sets the organisation postcode.
        /// </summary>
        public string? OrganisationPostcode { get; set; }

        /// <summary>
        /// Gets or sets the provider type.
        /// </summary>
        public string? ProviderType { get; set; }

        /// <summary>
        /// Gets or sets the open reason.
        /// </summary>
        public string? OpenReason { get; set; }

        /// <summary>
        /// Gets or sets the provider status.
        /// </summary>
        public string ProviderStatus { get; set; } = default!;

        /// <summary>
        /// Gets or sets the close reason.
        /// </summary>
        public string? CloseReason { get; set; }

        /// <summary>
        /// Gets or sets the provider subtype.
        /// </summary>
        public string? ProviderSubType { get; set; }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        public double? TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the funding lines.
        /// </summary>
        public IEnumerable<LoggedInTemplateLine>? FundingLines { get; set; }

        /// <summary>
        /// Gets or sets the funding lines for summary.
        /// </summary>
        public IEnumerable<LoggedInTemplateLine>? FundingLinesForSummary { get; set; }

        /// <summary>
        /// Gets or sets the calculations.
        /// </summary>
        public IEnumerable<LoggedInCalculation>? Calculations { get; set; }

        /// <summary>
        /// Gets or sets the calculations for summary.
        /// </summary>
        public IEnumerable<LoggedInCalculation>? CalculationsForSummary { get; set; }

        /// <summary>
        /// Gets or sets the phase of education.
        /// </summary>
        public string? PhaseOfEducation { get; set; }

        /// <summary>
        /// Gets or sets the date opened.
        /// </summary>
        public DateTime? DateOpened { get; set; }

        /// <summary>
        /// Gets or sets the date closed.
        /// </summary>
        public DateTime? DateClosed { get; set; }

        /// <summary>
        /// Gets or sets the variation reasons.
        /// </summary>
        public IEnumerable<string>? VariationReasons { get; set; }

        /// <summary>
        /// Gets or sets the local authority name.
        /// </summary>
        public string? LocalAuthorityName { get; set; }

        /// <summary>
        /// Gets or sets the parliamentary constituency name.
        /// </summary>
        public string? ParliamentaryConstituencyName { get; set; }

        /// <summary>
        /// Gets or sets the parliamentary constituency code.
        /// </summary>
        public string? ParliamentaryConstituencyCode { get; set; }

        /// <summary>
        /// Gets or sets the statement channel version.
        /// </summary>
        public int? StatementChannelVersion { get; set; }

        /// <summary>
        /// Gets or sets the statement type.
        /// </summary>
        public string? StatementType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is indicative.
        /// </summary>
        public bool? IsIndicative { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is an in-year opener.
        /// </summary>
        public bool? InYearOpener { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is the latest.
        /// </summary>
        public bool? IsLatest { get; set; }
    }
}
