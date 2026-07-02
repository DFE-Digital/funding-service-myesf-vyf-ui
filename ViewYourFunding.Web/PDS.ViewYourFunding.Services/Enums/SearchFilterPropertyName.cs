namespace PDS.ViewYourFunding.Services.Enums
{
    /// <summary>
    /// Enum for filterable field names.
    /// </summary>
    public enum SearchFilterPropertyName
    {
        /// <summary>
        /// Filter by the parents primary identifier (e.g. LA Code) - only applicable for ProviderFunding.
        /// </summary>
        ParentPrimaryIdentifier = 1,

        /// <summary>
        /// Filter by the UKPRN.
        /// </summary>
        Ukprn = 2,

        /// <summary>
        /// Filter by the primary identifier (e.g. LA Code).
        /// </summary>
        PrimaryIdentifier = 3,

        /// <summary>
        /// Filter by the grouping reason (e.g. Payment or Information).
        /// </summary>
        GroupingReason = 4,

        /// <summary>
        /// Filter by an identifier list (e.g. ukPrn).
        /// </summary>
        PrimaryIdentifierList = 5,

        /// <summary>
        /// Filter by the identifier (e.g. Id).
        /// </summary>
        Id = 6,

        /// <summary>
        /// Filter by the group name (e.g. NOTTINGHAMSHIRE COUNTY COUNCIL).
        /// </summary>
        GroupName = 7
    }
}