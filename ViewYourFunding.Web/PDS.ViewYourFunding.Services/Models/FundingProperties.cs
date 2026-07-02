using System;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Funding properties (taken from the JSON).
    /// </summary>
    public class FundingProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FundingProperties"/> class.
        /// </summary>
        /// <param name="fundingValue">The funding value json.</param>
        /// <param name="entityName">The entity name (e.g. Johnstone School).</param>
        /// <param name="entityParentName">The parent entity name (e.g. Camden).</param>
        /// <param name="entityIdentifier">The identifer (e.g. 12345678).</param>
        /// <param name="entityParentIdentifier">The parent identifier (e.g. 202).</param>
        /// <param name="entityAlternativeIdentifier">An alternative identifier (e.g. DfE number for a provider).</param>
        /// <param name="entityType">An entity type (e.g. provider).</param>
        /// <param name="entitySubType">An entity sub type (e.g. Non-maintained special school.).</param>
        /// <param name="totalAmount">The total amount (e.g. 1.23).</param>
        /// <param name="providerStatus">The provider status (e.g. Open or Closed).</param>
        /// <param name="phaseOfEducation">The phase of education.</param>
        /// <param name="dateOpened">Provider date opened.</param>
        /// <param name="entityRegionName">The region name.</param>
        /// <param name="entityProviderUpin">The provider UPIN.</param>
        /// <param name="entityProviderUrn">The provider URN.</param>
        /// <param name="statusChangedDate">The status changed date.</param>
        /// <param name="localAuthorityName">The local authority name.</param>
        /// <param name="entityGroupUkprn">The group ukprn.</param>
        /// <param name="entityGroupingReason">The entity grouping reason.</param>
        /// <param name="openReason">The entity opening reason.</param>
        /// <param name="closeReason">The entity close reason.</param>
        /// <param name="dateClosed">Provider date closed.</param>
        public FundingProperties(
            string fundingValue,
            string entityName,
            string entityParentName,
            string entityIdentifier,
            string entityParentIdentifier,
            string entityAlternativeIdentifier,
            string entityType,
            string entitySubType,
            string totalAmount,
            string providerStatus,
            string phaseOfEducation,
            DateTime dateOpened,
            string entityRegionName,
            string entityProviderUpin,
            string entityProviderUrn,
            DateTime statusChangedDate,
            string localAuthorityName,
            string entityGroupUkprn,
            string entityGroupingReason,
            string openReason,
            string closeReason,
            DateTime dateClosed)
        {
            FundingValue = fundingValue;
            EntityName = entityName;
            EntityParentName = entityParentName;
            EntityIdentifier = entityIdentifier;
            EntityParentIdentifier = entityParentIdentifier;
            EntityAlternativeIdentifier = entityAlternativeIdentifier;
            EntityType = entityType;
            EntitySubType = entitySubType;
            TotalAmount = totalAmount;
            ProviderStatus = providerStatus;
            PhaseOfEducation = phaseOfEducation;
            DateOpened = dateOpened;
            EntityRegionName = entityRegionName;
            EntityProviderUpin = entityProviderUpin;
            EntityProviderUrn = entityProviderUrn;
            StatusChangedDate = statusChangedDate;
            LocalAuthorityName = localAuthorityName;
            EntityGroupUkprn = entityGroupUkprn;
            EntityGroupingReason = entityGroupingReason;
            OpenReason = openReason;
            CloseReason = closeReason;
            DateClosed = dateClosed;
        }

        /// <summary>
        /// Gets or sets the funding value json.
        /// </summary>
        public string FundingValue { get; set; }

        /// <summary>
        /// Gets or sets the entity name (e.g. Johnstone School).
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the parent entity name (e.g. Camden).
        /// </summary>
        public string EntityParentName { get; set; }

        /// <summary>
        /// Gets or sets the identifer (e.g. 12345678).
        /// </summary>
        public string EntityIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier (e.g. 202).
        /// </summary>
        public string EntityParentIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the alternative identifier (e.g. DfE number for a provider).
        /// </summary>
        public string EntityAlternativeIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the entity type (e.g. provider).
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the entity sub type (e.g. Non-maintained special school.).
        /// </summary>
        public string EntitySubType { get; set; }

        /// <summary>
        /// Gets or sets the entity grouping reason (e.g. Information, Indicative, Payment).
        /// </summary>
        public string EntityGroupingReason { get; set; }

        /// <summary>
        /// Gets or sets the total amount (e.g. 1.23).
        /// </summary>
        public string TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the provider status (e.g. Open or Closed).
        /// </summary>
        public string ProviderStatus { get; set; }

        /// <summary>
        /// Gets or sets the phase of education.
        /// </summary>
        public string PhaseOfEducation { get; set; }

        /// <summary>
        /// Gets or sets the reason of opening.
        /// </summary>
        public string OpenReason { get; set; }

        /// <summary>
        /// Gets or sets the date opened.
        /// </summary>
        public DateTime? DateOpened { get; set; }

        /// <summary>
        /// Gets or sets the UPIN.
        /// </summary>
        public string EntityProviderUpin { get; set; }

        /// <summary>
        /// Gets or sets the URN.
        /// </summary>
        public string EntityProviderUrn { get; set; }

        /// <summary>
        /// Gets or sets the region name.
        /// </summary>
        public string EntityRegionName { get; set; }

        /// <summary>
        /// Gets or sets the date and time this allocation was published.
        /// </summary>
        public DateTime? StatusChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the local authority name.
        /// </summary>
        public string LocalAuthorityName { get; set; }

        /// <summary>
        /// Gets or sets the Group UKPRN for the entity.
        /// </summary>
        public string EntityGroupUkprn { get; set; }

        /// <summary>
        /// Gets or sets the reason of closing.
        /// </summary>
        public string CloseReason { get; set; }

        /// <summary>
        /// Gets or sets the date closed.
        /// </summary>
        public DateTime? DateClosed { get; set; }
    }
}