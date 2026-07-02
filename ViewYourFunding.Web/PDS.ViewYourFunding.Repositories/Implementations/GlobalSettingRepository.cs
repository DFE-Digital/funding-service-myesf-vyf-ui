using Pds.Core.Logging;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Implementations
{
    /// <summary>
    /// Global Settings repository operations.
    /// </summary>
    public class GlobalSettingRepository : Repository<GlobalSetting>, IGlobalSettingRepository
    {
        private readonly ILoggerAdapter<Repository<GlobalSetting>> _loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalSettingRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DbContext.</param>
        /// <param name="loggerService">The logger service.</param>
        public GlobalSettingRepository(Context dbContext, ILoggerAdapter<Repository<GlobalSetting>> loggerService)
            : base(dbContext, loggerService)
        {
            _loggerService = loggerService;
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">Exception thrown if global setting not found.</exception>
        public async Task<bool> UpdateAsync(GlobalSetting globalSetting)
        {
            var dbGlobalSetting = await GetAsync(globalSetting.Id);

            if (dbGlobalSetting != null)
            {
                var oldValue = dbGlobalSetting.Value;
                dbGlobalSetting.Value = globalSetting.Value;
                dbGlobalSetting.UpdatedAt = DateTime.Now;

                var result = await SaveChangesAsync();
                _loggerService?.LogInformation($"GlobalSetting updated for Id: {dbGlobalSetting.Id} old value: {oldValue} new value: {globalSetting.Value}");
                return result > 0;
            }

            var exception = new InvalidOperationException($"GlobalSetting for type {globalSetting.Type} not found.");
            _loggerService?.LogError(exception, "Global Setting not found", globalSetting.Id, globalSetting.Type, globalSetting.Description);
            throw exception;
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveSettingByTypeAsync(int typeId)
        {
            var dbGlobalSetting = await GetFirstOrDefaultAsync(s => s.Type == typeId);

            if (dbGlobalSetting != null)
            {
                var result = await RemoveAsync(dbGlobalSetting.Id);
                return result > 0;
            }

            return false;
        }
    }
}