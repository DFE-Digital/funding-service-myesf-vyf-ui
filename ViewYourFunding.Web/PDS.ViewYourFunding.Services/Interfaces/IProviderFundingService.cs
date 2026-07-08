using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// The ProviderFunding service interface.
    /// </summary>
    public interface IProviderFundingService
    {
        /// <summary>
        /// Gets the provider funding by Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The layout model.</returns>
        Task<ProviderFundingModel> GetProviderFundingById(string id);

        /// <summary>
        /// Gets ProviderFunding from a query.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Provider Funding Models.</returns>
        Task<IEnumerable<ProviderFundingModel>> GetProviderFundingsById(string id);

        /// <summary>
        /// Gets ProviderFunding from a query.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Provider Funding Models.</returns>
        Task<string> GetProviderFundingFromAllById(string id);

        /// <summary>
        /// Gets ProviderFunding from a query.
        /// </summary>
        /// <param name="fundingStreamCode">The fundingStreamCode.</param>
        /// <param name="ukprn">The ukprn.</param>
        /// <param name="fundingPeriodId">The fundingPeriodId.</param>
        /// <returns>Provider Funding Models.</returns>
        Task<IEnumerable<ProviderFundingModel>> GetProviderFundings(string fundingStreamCode, string ukprn, string fundingPeriodId);
    }
}
