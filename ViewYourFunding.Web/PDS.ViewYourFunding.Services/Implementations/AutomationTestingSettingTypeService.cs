using Pds.Core.Logging;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Repositories.Implementations;
using PDS.ViewYourFunding.Services.Interfaces;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// A class exposing set operations for settings in the View Your Funding area that are stored in the SQL database.
    /// </summary>
    public class AutomationTestingSettingTypeService : Repository<Setting>, IAutomationTestingSettingTypeService
    {
        private const string DoNotUseSettingNameAndDescription = "donotuseregressionsetting";

        private const string DoNotUseSettingTypeSettingNameAndDescription = "donotuseregressionsettingtype";

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomationTestingSettingTypeService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="loggerService">The logger service.</param>
        public AutomationTestingSettingTypeService(
            Context dbContext,
            ILoggerAdapter<Repository<Setting>> loggerService)
        : base(dbContext, loggerService)
        {
        }

        /// <inheritdoc />
        public async Task<bool> AddRegressionSettingValueTestSettingTypeAsync()
        {
            var settingToAdd = await GetFirstOrDefaultAsync(settingType =>
                settingType.SettingName == DoNotUseSettingNameAndDescription);
            if (settingToAdd == null)
            {
                var newSetting = new Setting()
                {
                    SettingName = DoNotUseSettingNameAndDescription,
                    SettingDescription = DoNotUseSettingNameAndDescription,
                    ValueDataType = SettingValueDataType.String,
                    ValuesAreEditable = true
                };

                var result = await AddAsync(newSetting);
                return !result.Id.Equals(default);
            }

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteRegressionSettingValueTestSettingTypeAsync()
        {
            var settingToDelete = await GetFirstOrDefaultAsync(settingType =>
                settingType.SettingName == DoNotUseSettingNameAndDescription);
            if (settingToDelete != null)
            {
                var result = await RemoveAsync(settingToDelete.Id);

                return result > 0;
            }

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteRegressionTestSettingTypeAsync()
        {
            var settingToDelete = await GetFirstOrDefaultAsync(settingType =>
                settingType.SettingName == DoNotUseSettingTypeSettingNameAndDescription);
            if (settingToDelete != null)
            {
                var result = await RemoveAsync(settingToDelete.Id);

                return result > 0;
            }

            return true;
        }
    }
}