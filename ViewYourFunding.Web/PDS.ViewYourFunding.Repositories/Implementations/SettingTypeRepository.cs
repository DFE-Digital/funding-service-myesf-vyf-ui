using Newtonsoft.Json;
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
    /// SettingType repository operations.
    /// </summary>
    public class SettingTypeRepository : Repository<Setting>, ISettingTypeRepository
    {
        private readonly ILoggerAdapter<Repository<Setting>> _loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingTypeRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DbContext.</param>
        /// <param name="loggerService">The logger service.</param>
        public SettingTypeRepository(Context dbContext, ILoggerAdapter<Repository<Setting>> loggerService)
            : base(dbContext, loggerService)
        {
            _loggerService = loggerService;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Setting>> GetAllSettingTypes()
        {
            return await GetAllAsync(includeProperties: $"{nameof(Setting.SettingValues)}");
        }

        /// <inheritdoc/>
        public async Task<Setting> GetSettingTypeById(int id)
        {
            return await GetAsync(id);
        }

        /// <inheritdoc/>
        public async Task<Setting> CreateSettingType(Setting settingType)
        {
            var currentDateTime = DateTime.Now;

            settingType.LastUpdatedAt = currentDateTime;
            settingType.CreatedAt = currentDateTime;

            var createResult = await AddAsync(settingType);
            _loggerService?.LogInformation(Message(settingType, createResult, nameof(CreateSettingType)));

            return createResult;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateSettingType(Setting settingType)
        {
            var changes = new Dictionary<string, string>();
            var dbSettingType = await GetDatabaseSettingType(settingType);
            if (dbSettingType == null)
            {
                var exception = new InvalidOperationException($"SettingType for {settingType.Id} not found.");
                _loggerService?.LogError(exception, "SettingType not found", settingType.Id, settingType.SettingName, settingType.SettingDescription);
                throw exception;
            }

            GetChanges(settingType, dbSettingType, changes);

            dbSettingType.LastUpdatedAt = DateTime.Now;
            dbSettingType.LastUpdatedBy = settingType.LastUpdatedBy;

            var saveAffectedRecords = await SaveChangesAsync();

            var result = saveAffectedRecords > 0;
            if (result)
            {
                _loggerService?.LogInformation(
                    $"SettingType {dbSettingType.Id} updated by {dbSettingType.LastUpdatedBy} for the following properties {string.Join(",", changes.Select(x => $"{x.Key} => {x.Value}"))}");
            }
            else
            {
                _loggerService?.LogInformation($"SettingType with ID {dbSettingType.Id} did not update");
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteSettingType(Setting settingType)
        {
            var deleteResult = await RemoveAsync(settingType.Id);
            var result = deleteResult > 0;
            _loggerService?.LogInformation(
                $"SettingType {nameof(DeleteSettingType)} action for {GetSettingTypeData(settingType)} had result {result}");

            return result;
        }

        #region Helpers

        private static void GetChanges(Setting settingType, Setting dbSettingType, Dictionary<string, string> changes)
        {
            if (dbSettingType.SettingDescription != settingType.SettingDescription)
            {
                changes.Add(nameof(dbSettingType.SettingDescription), $"{dbSettingType.SettingDescription} => {settingType.SettingDescription}");
                dbSettingType.SettingDescription = settingType.SettingDescription;
            }

            if (dbSettingType.SettingName != settingType.SettingName)
            {
                changes.Add(nameof(dbSettingType.SettingName), $"{dbSettingType.SettingName} => {settingType.SettingName}");
                dbSettingType.SettingName = settingType.SettingName;
            }

            if (dbSettingType.ValueDataType != settingType.ValueDataType)
            {
                changes.Add(nameof(dbSettingType.ValueDataType), $"{dbSettingType.ValueDataType} => {settingType.ValueDataType}");
                dbSettingType.ValueDataType = settingType.ValueDataType;
            }

            if (dbSettingType.ValuesAreEditable != settingType.ValuesAreEditable)
            {
                changes.Add(nameof(dbSettingType.ValuesAreEditable), $"{dbSettingType.ValuesAreEditable} => {settingType.ValuesAreEditable}");
                dbSettingType.ValuesAreEditable = settingType.ValuesAreEditable;
            }
        }

        private static string Message(Setting settingType, Setting actionResult, string actionName)
        {
            return $"SettingType update {actionName} for {GetSettingTypeData(settingType)} had result {GetSettingTypeData(actionResult)}";
        }

        private static string GetSettingTypeData(Setting settingType)
        {
            if (settingType == null)
            {
                return "null";
            }

            return JsonConvert.SerializeObject(
                settingType,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        private async Task<Setting> GetDatabaseSettingType(Setting settingId)
        {
            var setting = await GetAsync(settingId.Id);
            return setting;
        }

        #endregion Helpers
    }
}