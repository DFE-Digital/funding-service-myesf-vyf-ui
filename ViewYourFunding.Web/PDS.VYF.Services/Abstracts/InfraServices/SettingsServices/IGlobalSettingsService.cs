namespace PDS.VYF.Services.Abstracts.InfraServices.SettingsServices
{
    using PDS.ViewYourFunding.Services.Models;

    /// <summary>
    /// Represents a service for managing global settings.
    /// </summary>
    public interface IGlobalSettingsService
    {
        /// <summary>
        /// Gets the first global setting of the specified type.
        /// </summary>
        /// <param name="settingTypeId">The ID of the setting type.</param>
        /// <returns>The first global setting of the specified type, or null if not found.</returns>
        Task<GlobalSetting?> GetFirstOrDefault(int settingTypeId);

        /// <summary>
        /// Gets the value of the global setting as a boolean.
        /// </summary>
        /// <param name="settingTypeId">The ID of the setting type.</param>
        /// <returns>The value of the global setting as a boolean.</returns>
        Task<bool> GetValueAsBool(int settingTypeId);
    }
}
