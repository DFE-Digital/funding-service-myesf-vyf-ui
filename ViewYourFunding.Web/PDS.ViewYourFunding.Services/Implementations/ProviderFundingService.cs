using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// The providerfunding service class.
    /// </summary>
    /// <seealso cref="IProviderFundingService" />
    public class ProviderFundingService : IProviderFundingService
    {
        private readonly ICosmosDbService<ProviderFundingModel> _cosmosDbService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderFundingService"/> class.
        /// </summary>
        /// <param name="cosmosDbService">The cosmos database service.</param>
        public ProviderFundingService(ICosmosDbService<ProviderFundingModel> cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        /// <inheritdoc />
        public async Task<ProviderFundingModel> GetProviderFundingById(string id)
        {
            return await _cosmosDbService.GetAsync(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProviderFundingModel>> GetProviderFundingsById(string id)
        {
            return await RunQueryAsync($"SELECT TOP 1 c.id, c.partitionKey, c.provider, c.fundingPeriodId, c.fundingStreamCode, c.fundingVersion, c.createdDate, (IS_DEFINED(c.channelVersion) ? c.channelVersion : []) as ChannelVersions, (select value max(SV['value']) from SV in c.channelVersion where SV.type = 'Statement') as StatementChannelVersion FROM c WHERE c.id = '{id}'");
        }

        /// <inheritdoc/>
        public async Task<string> GetProviderFundingFromAllById(string id)
        {
            return await RunQueryAllAsync($"SELECT TOP 1 * FROM c WHERE c.id = '{id}'");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProviderFundingModel>> GetProviderFundings(string fundingStreamCode, string ukprn, string fundingPeriodId)
        {
            return await RunQueryAsync($"SELECT c.id, c.partitionKey, c.provider, c.fundingPeriodId, c.fundingStreamCode, c.fundingVersion, c.createdDate, (IS_DEFINED(c.channelVersion) ? c.channelVersion : []) as ChannelVersions, (select value max(SV['value']) from SV in c.channelVersion where SV.type = 'Statement') as StatementChannelVersion FROM c where c.fundingStreamCode='{fundingStreamCode}' and c.partitionKey ='{ukprn}' and c.fundingPeriodId='{fundingPeriodId}'");
        }

        private async Task<IEnumerable<ProviderFundingModel>> RunQueryAsync(string query)
        {
            return await _cosmosDbService.RunQueryAsync<ProviderFundingModel>(query);
        }

        private async Task<string> RunQueryAllAsync(string query)
        {
            return await _cosmosDbService.RunQueryAllAsync<dynamic>(query);
        }
    }
}
