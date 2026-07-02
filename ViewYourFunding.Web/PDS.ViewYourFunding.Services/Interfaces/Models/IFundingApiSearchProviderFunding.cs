using Newtonsoft.Json;
using PDS.ViewYourFunding.Services.Helper;
using System;
using System.ComponentModel;

namespace PDS.ViewYourFunding.Services.Interfaces.Models
{
    /// <summary>
    /// An interface representing a single provider funding returned from a search request on the Funding API.
    /// </summary>
    public interface IFundingApiSearchProviderFunding : IFundingApiSearch
    {
        /// <summary>
        /// Gets or sets the unique id for the funding.
        /// </summary>
        [JsonProperty("id")]
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the funding this came from (if there are multiple, there will be multiple instances of this object).
        /// </summary>
        [JsonProperty("parentId")]
        string ParentId { get; set; }

        /// <summary>
        /// Gets or sets the funding period code for this funding (e.g. AY-1920).
        /// </summary>
        [JsonProperty("fundingPeriodCode")]
        string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream code (e.g. DSG).
        /// </summary>
        [JsonProperty("fundingStreamCode")]
        string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the date and time this allocation was published.
        /// </summary>
        [JsonProperty("statusChangedDate")]
        DateTime StatusChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the description associated with the published allocation.
        /// </summary>
        [JsonProperty("variationReason")]
        string VariationReason { get; set; }

        /// <summary>
        /// Gets or sets the version of the template.
        /// </summary>
        [JsonProperty("templateVersion")]
        string TemplateVersion { get; set; }

        /// <summary>
        /// Gets or sets the provider name (e.g. Meden School and Technology College).
        /// </summary>
        [JsonProperty("organisationName")]
        string OrganisationName { get; set; }

        /// <summary>
        /// Gets or sets the provider name.
        /// </summary>
        [JsonProperty("searchableOrganisationName")]
        string SearchableOrganisationName { get; set; }

        /// <summary>
        /// Gets or sets the provider UKPRN number.
        /// </summary>
        [JsonProperty("organisationUkprn")]
        string OrganisationUkprn { get; set; }

        /// <summary>
        /// Gets or sets the provider establishment number.
        /// </summary>
        [JsonProperty("organisationDfeNumber")]
        string OrganisationDfeNumber { get; set; }

        /// <summary>
        /// Gets or sets the provider town.
        /// </summary>
        [JsonProperty("organisationTown")]
        string OrganisationTown { get; set; }

        /// <summary>
        /// Gets or sets the provider postcode.
        /// </summary>
        [JsonProperty("organisationPostcode")]
        string OrganisationPostcode { get; set; }

        /// <summary>
        /// Gets or sets name of the parent organisation group.
        /// </summary>
        [JsonProperty("parentName")]
        string ParentName { get; set; }

        /// <summary>
        /// Gets or sets the parent organisation group's primary identifier (e.g. UKPRN).
        /// </summary>
        [JsonProperty("parentPrimaryIdentifier")]
        string ParentPrimaryIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the parent organisation groups' provider type (e.g. LocalAuthority).
        /// </summary>
        [JsonProperty("parentProviderType")]
        string ParentProviderType { get; set; }

        /// <summary>
        /// Gets or sets the type of provider.
        /// </summary>
        [JsonProperty("providerType")]
        string ProviderType { get; set; }

        /// <summary>
        /// Gets or sets the sub-type of provider.
        /// </summary>
        [JsonProperty("providerSubType")]
        string ProviderSubType { get; set; }

        /// <summary>
        /// Gets or sets the status of provider (e.g. 'Open').
        /// </summary>
        [JsonProperty("providerStatus")]
        string ProviderStatus { get; set; }

        /// <summary>
        /// Gets or sets the closure reason (e.g. 'Closure' or 'Not applicable').
        /// </summary>
        [JsonProperty("closeReason")]
        string CloseReason { get; set; }

        /// <summary>
        /// Gets or sets the opening reason (e.g. 'Fresh Start' or 'Not applicable').
        /// </summary>
        [JsonProperty("openReason")]
        string OpenReason { get; set; }

        /// <summary>
        /// Gets or sets the search result display.
        /// </summary>
        /// <value>
        /// The search result display.
        /// </value>
        string SearchResultDisplay { get; set; }

        /// <summary>
        /// Gets or sets the grouping reason (e.g. Payment or Information).
        /// </summary>
        [JsonProperty("groupingReason")]
        string GroupingReason { get; set; }

        /// <summary>
        /// Gets or sets the phase of education.
        /// </summary>
        [JsonProperty("phaseOfEducation")]
        string PhaseOfEducation { get; set; }

        /// <summary>
        /// Gets or sets the date opened.
        /// </summary>
        /// <value>
        /// The date opened.
        /// </value>
        [JsonProperty("dateOpened")]
        [TypeConverter(typeof(CustomDateTimeTypeConverter))]
        DateTime? DateOpened { get; set; }

        /// <summary>
        /// Gets or sets the date closed.
        /// </summary>
        /// <value>
        /// The date closed.
        /// </value>
        [JsonProperty("dateClosed")]
        [TypeConverter(typeof(CustomDateTimeTypeConverter))]
        DateTime? DateClosed { get; set; }

        /// <summary>
        /// Gets or sets the region name.
        /// </summary>
        [JsonProperty("regionName")]
        string RegionName { get; set; }

        /// <summary>
        /// Gets or sets the provider upin.
        /// </summary>
        [JsonProperty("providerUpin")]
        string ProviderUpin { get; set; }

        /// <summary>
        /// Gets or sets the provider urn.
        /// </summary>
        [JsonProperty("providerUrn")]
        string ProviderUrn { get; set; }

        /// <summary>
        /// Gets or sets the local authority name.
        /// </summary>
        [JsonProperty("localAuthorityName")]
        string LocalAuthorityName { get; set; }

        /// <summary>
        /// Gets the name of the provider sub type.
        /// </summary>
        /// <value>
        /// The name of the provider sub type.
        /// </value>
        string ProviderSubTypeName { get; }

        /// <summary>
        /// Gets or sets the parliamentary constituency name.
        /// </summary>
        [JsonProperty("parliamentaryConstituencyName")]
        public string ParliamentaryConstituencyName { get; set; }

        /// <summary>
        /// Gets or sets the parliamentary constituency code.
        /// </summary>
        [JsonProperty("parliamentaryConstituencyCode")]
        public string ParliamentaryConstituencyCode { get; set; }

        /// <summary>
        /// Gets a value indicating whether the given providers is a New Opener.
        /// </summary>
        [JsonIgnore]
        public bool IsNewOpener { get; }

        /// <summary>
        /// Gets a value indicating whether the given provider has final Allocation (for UIFSM report).
        /// </summary>
        [JsonIgnore]
        public bool HasFinalAllocationForUIFSM { get; }

        /// <summary>
        /// Gets a value of Final Allocation School Type (for UIFSM Report).
        /// </summary>
        [JsonIgnore]
        public string FinalAllocationSchoolType { get; }
    }
}