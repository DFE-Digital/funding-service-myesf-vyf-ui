using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// GlobalSetting Service.
    /// </summary>
    public interface IGlobalSettingService
    {
        /// <summary>
        /// Get setting for specified type.
        /// </summary>
        /// <param name="settingTypeId">The Type to retrieve.</param>
        /// <returns>Global setting task.</returns>
        Task<GlobalSetting> GetFirstOrDefault(int settingTypeId);

        /// <summary>
        /// Gets the specified global setting by identifier.
        /// </summary>
        /// <param name="globalSettingId">The global setting identifier.</param>
        /// <returns>Global setting task.</returns>
        Task<GlobalSetting> Get(int globalSettingId);

        /// <summary>
        /// Remove GlobalSetting by specified type.
        /// </summary>
        /// <param name="settingTypeId">setting type to remove.</param>
        /// <returns>True if setting removed.</returns>
        Task<bool> RemoveSettingByTypeAsync(int settingTypeId);

        /// <summary>
        /// Remove GlobalSetting by specified id.
        /// </summary>
        /// <param name="id">setting id remove.</param>
        /// <returns>True if setting removed.</returns>
        Task<bool> RemoveAsync(int id);

        /// <summary>
        /// Add a GlobalSetting.
        /// </summary>
        /// <param name="globalSetting">Global setting to add.</param>
        /// <returns>Return true if successfully added.</returns>
        Task<GlobalSetting> AddAsync(GlobalSetting globalSetting);

        /// <summary>
        /// Update a GlobalSetting.
        /// </summary>
        /// <param name="globalSetting">Setting to update.</param>
        /// <returns>True if the global setting is updated successfully.</returns>
        Task<bool> UpdateAsync(GlobalSetting globalSetting);

        /// <summary>
        /// Get all the global settings.
        /// </summary>
        /// <returns>A list of all the global settings.</returns>
        Task<IList<GlobalSetting>> GetAllGlobalSettings();
    }
}