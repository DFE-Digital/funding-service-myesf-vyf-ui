using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// The audit service class.
    /// </summary>
    /// <seealso cref="IAuditService" />
    public class AuditService : IAuditService
    {
        private readonly ICosmosDbService<DataImportAuditModel> _cosmosDbService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditService"/> class.
        /// </summary>
        /// <param name="cosmosDbService">The cosmos database service.</param>
        public AuditService(ICosmosDbService<DataImportAuditModel> cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }


        /// <inheritdoc/>
        public async Task<IEnumerable<DataImportAuditModel>> GetDataImportAudit()
        {
            return await RunQueryAsync(@"SELECT TOP 1 * FROM c WHERE c.action = 'Import' ORDER BY c.partitionKey DESC");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DataImportAuditModel>> GetDataImportAudit(string fundingStreamCode)
        {
            return await RunQueryAsync($"SELECT TOP 1 * FROM c WHERE c.action = 'Import' and Contains(c.fundingStreams,'{fundingStreamCode}') ORDER BY c.partitionKey DESC");
        }

        private async Task<IEnumerable<DataImportAuditModel>> RunQueryAsync(string query)
        {
            return await _cosmosDbService.RunQueryAsync<DataImportAuditModel>(query);
        }
    }
}
