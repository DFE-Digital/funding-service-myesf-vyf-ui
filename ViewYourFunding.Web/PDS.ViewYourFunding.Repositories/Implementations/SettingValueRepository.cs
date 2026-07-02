using Pds.Core.Logging;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Implementations
{
    /// <summary>
    /// The View your Funding Settings Repository class.
    /// </summary>
    /// <seealso cref="ISettingValueRepository" />
    public class SettingValueRepository : Repository<SettingValue>, ISettingValueRepository
    {
        private readonly ILoggerAdapter<Repository<SettingValue>> _loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingValueRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="loggerService">The logger service.</param>
        public SettingValueRepository(
            Context dbContext,
            ILoggerAdapter<Repository<SettingValue>> loggerService)
            : base(dbContext, loggerService)
        {
            _loggerService = loggerService;
        }

        /// <inheritdoc/>
        public async Task<SettingValue> GetSettingValueById(int fundingStreamId, int settingId)
        {
            return await GetFirstOrDefaultAsync(
                settingValue =>
                settingValue.SettingId == settingId && settingValue.FundingStreamId == fundingStreamId,
                $"{nameof(SettingValue.FundingStream)}, {nameof(SettingValue.Setting)}");
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateSettingValue(SettingValue settingValue)
        {
            var changes = new Dictionary<string, string>();
            var dbSettingValue = await GetDatabaseSettingValue(settingValue.Id);
            if (dbSettingValue == null)
            {
                return false;
            }

            GetChanges(settingValue, dbSettingValue, changes);

            dbSettingValue.LastUpdatedAt = DateTime.Now;
            dbSettingValue.LastUpdatedBy = settingValue.LastUpdatedBy;

            var saveAffectedRecords = await SaveChangesAsync();

            var result = saveAffectedRecords > 0;

            if (result)
            {
                _loggerService?.LogInformation(
                    $"Setting value for funding stream {dbSettingValue.FundingStreamId} ({dbSettingValue.FundingStream?.FundingStreamCode ?? "?"}) updated by {dbSettingValue.LastUpdatedBy} for the following properties {string.Join(",", changes.Select(x => $"{x.Key} => {x.Value}"))}");
            }
            else
            {
                _loggerService?.LogInformation(
                    $"Setting value for funding stream {dbSettingValue.FundingStreamId} ({dbSettingValue.FundingStream?.FundingStreamCode ?? "?"}) didn't update");
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteSettingValue(SettingValue settingValue)
        {
            var deleteResult = await RemoveAsync(settingValue.Id);
            var result = deleteResult > 0;
            _loggerService?.LogInformation(
                $"Setting value id {settingValue.Id} deleted for setting {settingValue.SettingId}. Deleted by user {settingValue.LastUpdatedBy}");

            return result;
        }

        private static void GetChanges(SettingValue settingValue, SettingValue dbSettingValue, Dictionary<string, string> changes)
        {
            if (dbSettingValue.Value != settingValue.Value)
            {
                changes.Add(nameof(dbSettingValue.Value), $"{dbSettingValue.Value} => {settingValue.Value}");
                dbSettingValue.Value = settingValue.Value;
            }
        }

        private async Task<SettingValue> GetDatabaseSettingValue(int id)
        {
            return await GetAsync(id);
        }
    }
}