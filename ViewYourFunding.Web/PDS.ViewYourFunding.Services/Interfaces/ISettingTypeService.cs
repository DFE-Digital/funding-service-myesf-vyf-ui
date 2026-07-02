using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// Setting Type Service.
    /// </summary>
    public interface ISettingTypeService
    {
        /// <summary>
        /// Get all of the SettingTypes.
        /// </summary>
        /// <returns>A list of all the SettingTypes.</returns>
        Task<IList<SettingType>> GetAllSettingTypes();

        /// <summary>
        /// Gets the SettingType for the given Id.
        /// </summary>
        /// <param name="id">The id of the SettingType to get.</param>
        /// <returns>
        /// The SettingType for the given Id.
        /// </returns>
        Task<SettingType> GetSettingTypeById(int id);

        /// <summary>
        /// Creates the SettingType.
        /// </summary>
        /// <param name="settingType">The SettingType to create.</param>
        /// <returns>The created SettingType.</returns>
        Task<SettingType> CreateSettingType(SettingType settingType);

        /// <summary>
        /// Updates the setting type.
        /// </summary>
        /// <param name="settingType">The SettingType to update.</param>
        /// <returns>Returns true, if update is successful.</returns>
        Task<bool> UpdateSettingType(SettingType settingType);

        /// <summary>
        /// Deletes the Setting Type.
        /// </summary>
        /// <param name="settingType">The SettingType to delete.</param>
        /// <returns>Returns true, if deletion was successful.</returns>
        Task<bool> DeleteSettingType(SettingType settingType);

        /// <summary>
        /// Get all the editable setting types not attached to the funding stream.
        /// </summary>
        /// <param name="excludedSettingTypeIds">The setting type ids to exclude.</param>
        /// <returns>A list of all the setting types.</returns>
        Task<IList<SettingType>> GetAvailableSettingTypes(IEnumerable<int> excludedSettingTypeIds);
    }
}