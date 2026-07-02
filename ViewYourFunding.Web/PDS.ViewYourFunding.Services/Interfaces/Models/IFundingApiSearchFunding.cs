using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Interfaces.Models
{
    /// <summary>
    /// An interface representing a funding returned from a search request on the Funding API.
    /// </summary>
    public interface IFundingApiSearchFunding : IFundingApiSearch
    {
        /// <summary>
        /// Gets or sets the unique id for the funding.
        /// </summary>
        [JsonProperty("id")]
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the type of grouping (e.g. LocalAuthority).
        /// </summary>
        [JsonProperty("groupingType")]
        string GroupingType { get; set; }

        /// <summary>
        /// Gets or sets the reason of grouping (payment or information).
        /// </summary>
        [JsonProperty("groupingReason")]
        string GroupingReason { get; set; }

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
        /// Gets or sets the version of the template.
        /// </summary>
        [JsonProperty("templateVersion")]
        string TemplateVersion { get; set; }

        /// <summary>
        /// Gets or sets the group name (e.g. East Midlands).
        /// </summary>
        [JsonProperty("groupName")]
        string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the group name without certain characters (e.g. 'EastMidlands').
        /// </summary>
        [JsonProperty("searchableGroupName")]
        string SearchableGroupName { get; set; }

        /// <summary>
        /// Gets or sets the groups UKPRN (if applicable).
        /// </summary>
        [JsonProperty("groupUKPRN")]
        string GroupUkprn { get; set; }

        /// <summary>
        /// Gets or sets a code to represent the code.
        /// </summary>
        [JsonProperty("groupCode")]
        string GroupCode { get; set; }


        /// <summary>
        /// Gets or sets detail about the provider fundings.
        /// </summary>
        [JsonProperty("providerFundings")]
        IEnumerable<string> ProviderFundings { get; set; }

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
        /// Gets the school type.
        /// </summary>
        [JsonProperty("schoolType")]
        public string SchoolType { get; }

        /// <summary>
        /// Gets the Government office region.
        /// </summary>
        [JsonProperty("governmentOfficeRegion")]
        public string GovernmentOfficeRegion { get; }
    }
}