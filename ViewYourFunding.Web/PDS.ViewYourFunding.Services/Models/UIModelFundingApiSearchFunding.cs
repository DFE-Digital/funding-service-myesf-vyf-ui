using Newtonsoft.Json;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A class representing a single allocation returned from a search request on the Funding API.
    /// </summary>
    public class UiModelFundingApiSearchFunding : IFundingApiSearchFunding
    {
        /// <summary>
        /// Gets or sets the unique id for the funding.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the type of grouping (e.g. LocalAuthority).
        /// </summary>
        public string GroupingType { get; set; }

        /// <summary>
        /// Gets or sets the reason of grouping (payment or information).
        /// </summary>
        public string GroupingReason { get; set; }

        /// <summary>
        /// Gets or sets the funding period code for this funding (e.g. AY-1920).
        /// </summary>
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream code (e.g. DSG).
        /// </summary>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the date and time this allocation was published.
        /// </summary>
        public DateTime StatusChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the version of this allocation.
        /// </summary>
        public string FundingVersion { get; set; }

        /// <summary>
        /// Gets or sets the channel version.
        /// </summary>
        public IEnumerable<ChannelVersion> ChannelVersions { get; set; }

        /// <summary>
        /// Gets or sets the version from statement channel.
        /// </summary>
        public int? StatementChannelVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the funding is first version.
        /// </summary>
        public bool IsFirstStatementChannelVersion { get; set; }

        /// <summary>
        /// Gets or sets the version of the schema.
        /// </summary>
        public string SchemaVersion { get; set; }

        /// <summary>
        /// Gets or sets the version of the template.
        /// </summary>
        public string TemplateVersion { get; set; }

        /// <summary>
        /// Gets or sets the group name (e.g. East Midlands).
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the provider name.
        /// </summary>
        public string SearchableGroupName { get; set; }

        /// <summary>
        /// Gets or sets the provider establishment number.
        /// </summary>
        [JsonProperty("groupUKPRN")]
        public string GroupUkprn { get; set; }

        /// <summary>
        /// Gets or sets a code to represent the code.
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// Gets or sets the total amount of this funding.
        /// </summary>
        public double TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets detail about the makeup of this funding.
        /// </summary>
        public string FundingValue { get; set; }

        /// <summary>
        /// Gets or sets detail about the provider fundings.
        /// </summary>
        [JsonProperty("providerFundings")]
        public IEnumerable<string> ProviderFundings { get; set; }

        /// <summary>
        /// Gets or sets classes applied to this funding.
        /// </summary>
        [JsonProperty("classname")]
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the region name.
        /// </summary>
        [JsonProperty("regionName")]
        public string RegionName { get; set; }

        /// <summary>
        /// Gets or sets the description associated with the published allocation.
        /// </summary>
        [JsonProperty("variationReason")]
        public string VariationReason { get; set; }

        /// <summary>
        /// Gets the name of the school type.
        /// </summary>
        /// <value>
        /// The name of the school type.
        /// </value>
        public string SchoolType
        {
            get
            {
                return GroupingType switch
                {
                    "AlternativeProvision" => "Alternative Provision",
                    "AcademyAlternativeProvision" => "Alternative Provision Academies",
                    "NonMaintainedSpecialSchools" => "Non-maintained Special Schools",
                    "Mainstream" => "Mainstream Schools",
                    "LocalAuthorityMss" => "Maintained Special Schools",
                    "PupilReferralUnit" => "Pupil Referral Units",
                    "SpecialAcademies" => "Special Academies",
                    _ => GroupingType
                };
            }
        }

        /// <summary>
        /// Gets the name of the government office region.
        /// </summary>
        /// <value>
        /// The name of the government office region.
        /// </value>
        public string GovernmentOfficeRegion
        {
            get
            {
                return GroupName switch
                {
                    "Inner London" => "London - Inner",
                    "Outer London" => "London - Outer",
                    "London" => "London - Total",
                    _ => GroupName
                };
            }
        }
    }
}