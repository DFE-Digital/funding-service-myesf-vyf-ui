using PDS.ViewYourFunding.Repositories.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Interfaces
{
    /// <summary>
    /// SettingType repository operations.
    /// </summary>
    public interface ISettingTypeRepository : IRepository<Setting>
    {
        /// <summary>
        /// Gets all of the SettingTypes.
        /// </summary>
        /// <returns>
        /// A list containing all of the SettingTypes.
        /// </returns>
        Task<IEnumerable<Setting>> GetAllSettingTypes();

        /// <summary>
        /// Gets the SettingType for the given Id.
        /// </summary>
        /// <param name="id">The id of the SettingType to get.</param>
        /// <returns>
        /// The SettingType for the given Id.
        /// </returns>
        Task<Setting> GetSettingTypeById(int id);

        /// <summary>
        /// Create a new SettingType.
        /// </summary>
        /// <param name="settingType">The SettingType to create.</param>
        /// <returns>A Task if <see cref="Setting"/> type is created successfully.</returns>
        Task<Setting> CreateSettingType(Setting settingType);

        /// <summary>
        /// Updates the setting type.
        /// </summary>
        /// <param name="settingType">The setting type to update.</param>
        /// <returns>Returns true, if update is successful.</returns>
        Task<bool> UpdateSettingType(Setting settingType);

        /// <summary>
        /// Deletes the SettingType.
        /// </summary>
        /// <param name="settingType">The SettingType to delete.</param>
        /// <returns>Returns true if setting type deleted.</returns>
        Task<bool> DeleteSettingType(Setting settingType);
    }
}