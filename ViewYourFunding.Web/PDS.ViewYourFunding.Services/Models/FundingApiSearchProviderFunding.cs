using Newtonsoft.Json;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A class representing a single funding returned from a search request on the Funding API.
    /// </summary>
    public class FundingApiSearchProviderFunding : IFundingApiSearchProviderFunding
    {
        /// <summary>
        /// Gets or sets the unique id for the funding.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the funding this came from (if there are multiple, there will be multiple instances of this object).
        /// </summary>
        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        /// <summary>
        /// Gets or sets the funding period code for this funding (e.g. AY-1920).
        /// </summary>
        [JsonProperty("fundingPeriodCode")]
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream code (e.g. DSG).
        /// </summary>
        [JsonProperty("fundingStreamCode")]
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the date and time this allocation was published.
        /// </summary>
        [JsonProperty("statusChangedDate")]
        public DateTime StatusChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the description associated with the published allocation.
        /// </summary>
        [JsonProperty("variationReason")]
        public string VariationReason { get; set; }

        /// <summary>
        /// Gets or sets the version of this allocation.
        /// </summary>
        [JsonProperty("fundingVersion")]
        public string FundingVersion { get; set; }

        /// <summary>
        /// Gets or sets the channel version.
        /// </summary>
        [JsonProperty("channelVersions")]
        public IEnumerable<ChannelVersion> ChannelVersions { get; set; }

        /// <summary>
        /// Gets or sets the version from statement channel.
        /// </summary>
        [JsonProperty("statementChannelVersion")]
        public int? StatementChannelVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the funding is first version.
        /// </summary>
        [JsonProperty("isFirstStatementChannelVersion")]
        public bool IsFirstStatementChannelVersion { get; set; }

        /// <summary>
        /// Gets or sets the version of the schema.
        /// </summary>
        [JsonProperty("schemaVersion")]
        public string SchemaVersion { get; set; }

        /// <summary>
        /// Gets or sets the version of the template.
        /// </summary>
        [JsonProperty("templateVersion")]
        public string TemplateVersion { get; set; }

        /// <summary>
        /// Gets or sets the searchable provider name.
        /// </summary>
        [JsonProperty("organisationName")]
        public string OrganisationName { get; set; }

        /// <summary>
        /// Gets or sets the searchable provider name (strips some characters).
        /// </summary>
        [JsonProperty("searchableOrganisationName")]
        public string SearchableOrganisationName { get; set; }

        /// <summary>
        /// Gets or sets the provider UKPRN number.
        /// </summary>
        [JsonProperty("organisationUkprn")]
        public string OrganisationUkprn { get; set; }

        /// <summary>
        /// Gets or sets the provider establishment number.
        /// </summary>
        [JsonProperty("organisationDfeNumber")]
        public string OrganisationDfeNumber { get; set; }

        /// <summary>
        /// Gets or sets the provider town.
        /// </summary>
        [JsonProperty("organisationTown")]
        public string OrganisationTown { get; set; }

        /// <summary>
        /// Gets or sets the provider postcode.
        /// </summary>
        [JsonProperty("organisationPostcode")]
        public string OrganisationPostcode { get; set; }

        /// <summary>
        /// Gets or sets name of the parent organisation group.
        /// </summary>
        [JsonProperty("parentName")]
        public string ParentName { get; set; }

        /// <summary>
        /// Gets or sets the parent organisation group's primary identifier (e.g. UKPRN).
        /// </summary>
        [JsonProperty("parentPrimaryIdentifier")]
        public string ParentPrimaryIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the parent organisation group's provider type (e.g. LocalAuthority).
        /// </summary>
        [JsonProperty("parentProviderType")]
        public string ParentProviderType { get; set; }

        /// <summary>
        /// Gets or sets the type of provider.
        /// </summary>
        [JsonProperty("providerType")]
        public string ProviderType { get; set; }

        /// <summary>
        /// Gets or sets the sub-type of provider.
        /// </summary>
        [JsonProperty("providerSubType")]
        public string ProviderSubType { get; set; }

        /// <summary>
        /// Gets or sets the status of the provider (e.g. 'Open').
        /// </summary>
        [JsonProperty("providerStatus")]
        public string ProviderStatus { get; set; }

        /// <summary>
        /// Gets or sets the closure reason (e.g. 'Closure' or 'Not applicable').
        /// </summary>
        [JsonProperty("closeReason")]
        public string CloseReason { get; set; }

        /// <summary>
        /// Gets or sets the open reason (e.g. 'Fresh Start' or 'Not applicable').
        /// </summary>
        [JsonProperty("openReason")]
        public string OpenReason { get; set; }

        /// <summary>
        /// Gets or sets the total amount of this funding.
        /// </summary>
        [JsonProperty("totalAmount")]
        public double TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets detail about the make up of this provider funding.
        /// </summary>
        [JsonProperty("fundingValue")]
        public string FundingValue { get; set; }

        /// <summary>
        /// Gets or sets the search result display.
        /// </summary>
        /// <value>
        /// The search result display.
        /// </value>
        public string SearchResultDisplay { get; set; }

        /// <summary>
        /// Gets or sets the grouping reason (e.g. Payment or Information).
        /// </summary>
        [JsonProperty("groupingReason")]
        public string GroupingReason { get; set; }

        /// <summary>
        /// Gets or sets the phase of education.
        /// </summary>
        [JsonProperty("phaseOfEducation")]
        public string PhaseOfEducation { get; set; }

        /// <summary>
        /// Gets or sets the date opened.
        /// </summary>
        [JsonProperty("dateOpened")]
        public DateTime? DateOpened { get; set; }

        /// <summary>
        /// Gets or sets the date closed.
        /// </summary>
        [JsonProperty("dateClosed")]
        public DateTime? DateClosed { get; set; }

        /// <summary>
        /// Gets or sets the region name.
        /// </summary>
        [JsonProperty("regionName")]
        public string RegionName { get; set; }

        /// <summary>
        /// Gets or sets the provider upin.
        /// </summary>
        [JsonProperty("providerUpin")]
        public string ProviderUpin { get; set; }

        /// <summary>
        /// Gets or sets the provider urn.
        /// </summary>
        [JsonProperty("providerUrn")]
        public string ProviderUrn { get; set; }

        /// <summary>
        /// Gets or sets the local authority name.
        /// </summary>
        [JsonProperty("localAuthorityName")]
        public string LocalAuthorityName { get; set; }

        /// <summary>
        /// Gets the name of the provider sub type.
        /// </summary>
        /// <value>
        /// The name of the provider sub type.
        /// </value>
        public string ProviderSubTypeName
        {
            get
            {
                return ProviderSubType switch
                {
                    "11ACA" => "Academy",
                    "20MSS" => "Maintained special school",
                    "17NMF" => "Academy Special",
                    "08SSF" => "School sixth form",
                    "01GFE" => "General FE and tertiary",
                    "22OTH" => "Other",
                    "02IPP" => "Independent Learning Provider",
                    "18ISP" => "Special Post-16 Institution",
                    "15UTC" => "University Technical College",
                    "03SFC" => "Sixth Form college",
                    "12FSC" => "Free school",
                    "04AHC" => "Agricultural and horticultural college",
                    "22AAP" => "Academy AP",
                    "13SSA" => "Studio school",
                    "10LAU" => "Local authority",
                    "07HEP" => "Higher education provider",
                    "19FSS" => "Free School Special",
                    "05ADC" => "Art and Design college",
                    "14CTC" => "City technology college",
                    "06SDC" => "Specialist designated college",
                    "21NMS" => "Non maintained special school",
                    "FS1619" => "16-19 free school",
                    "16NPF" => "Non programme funded provider",
                    _ => ProviderSubType
                };
            }
        }

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

        private bool? _isNewOpener;

        /// <summary>
        /// Gets a value indicating whether the given provider is a New Opener.
        /// </summary>
        [JsonIgnore]
        public bool IsNewOpener => _isNewOpener ??= this.CheckProviderHasNewOpenerProvisionalAllocation();

        private bool? _hasFinalAllocationForUIFSM;

        /// <summary>
        /// Gets a value indicating whether the given provider has final Allocation (for UIFSM report).
        /// </summary>
        [JsonIgnore]
        public bool HasFinalAllocationForUIFSM => _hasFinalAllocationForUIFSM ??= this.CheckProviderHasFinalAllocation();

        private string _finalAllocationSchoolType;

        /// <summary>
        /// Gets a value of Final Allocation School Type (for UIFSM Report).
        /// </summary>
        [JsonIgnore]
        public string FinalAllocationSchoolType => _finalAllocationSchoolType ??= this.GetFinalAllocationSchoolType();
    }
}