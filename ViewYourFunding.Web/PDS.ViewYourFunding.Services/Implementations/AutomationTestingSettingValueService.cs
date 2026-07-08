using Pds.Core.Logging;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Implementations;
using PDS.ViewYourFunding.Services.Interfaces;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// A class exposing set operations for settings in the View Your Funding area that are stored in the SQL database.
    /// </summary>
    public class AutomationTestingSettingValueService : Repository<SettingValue>, IAutomationTestingSettingValueService
    {
        private const string DoNotUseSettingNameAndDescription = "donotuseregressionsetting";
        private readonly ILoggerAdapter<Repository<SettingValue>> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomationTestingSettingValueService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="loggerService">The logger service.</param>
        public AutomationTestingSettingValueService(
            Context dbContext,
            ILoggerAdapter<Repository<SettingValue>> loggerService)
        : base(dbContext, loggerService)
        {
            _logger = loggerService;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateSettingValueAsync(int fundingStreamId, int settingValueId, string newValue)
        {
            var success = false;
            var settingToUpdate = await GetAsync(settingValueId);
            if (settingToUpdate != null)
            {
                if (settingToUpdate.Value != newValue)
                {
                    settingToUpdate.Value = newValue;

                    var result = await SaveChangesAsync();
                    success = result > 0;
                    _logger?.LogInformation($"Setting for {fundingStreamId}, {settingValueId} update result = {result}");
                }
                else
                {
                    _logger?.LogInformation($"Setting for {fundingStreamId}, {settingValueId} no update required, values are the same.");
                }
            }

            return success;
        }

        /// <inheritdoc />
        public async Task<bool> DeletePsgRegressionSettingValue(int fundingStreamId)
        {
            var settingToDelete = await GetFirstOrDefaultAsync(settingValue =>
                settingValue.FundingStreamId == fundingStreamId &&
                settingValue.Setting.SettingName == DoNotUseSettingNameAndDescription);

            if (settingToDelete != null)
            {
                var result = await RemoveAsync(settingToDelete.Id);

                return result > 0;
            }

            return false;
        }
    }
}