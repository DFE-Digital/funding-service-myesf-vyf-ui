using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Interfaces.Models
{
    /// <summary>
    /// An interface representing the response from a search request on the Fundings API.
    /// </summary>
    public interface IFundingApiSearchResponseFunding
    {
        /// <summary>
        /// Gets or sets the collection of matching funding.
        /// </summary>
        IEnumerable<IFundingApiSearchFunding> Funding { get; set; }

        /// <summary>
        /// Gets or sets the collection of matching provider funding.
        /// </summary>
        IEnumerable<IFundingApiSearchProviderFunding> ProviderFunding { get; set; }
    }
}