using Newtonsoft.Json;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects
{
    /// <summary>
    /// A class representing the response from a search request on the Funding API.
    /// </summary>
    public class ProviderFundingApiSearchResponse : IFundingApiSearchResponseProviderFunding
    {
        /// <summary>
        /// Gets or sets the collection of matching provider funding.
        /// </summary>
        [JsonConverter(typeof(ConcreteTypeConverter<IEnumerable<FundingApiSearchProviderFunding>>))]
        public IEnumerable<IFundingApiSearchProviderFunding> ProviderFunding { get; set; }
    }
}