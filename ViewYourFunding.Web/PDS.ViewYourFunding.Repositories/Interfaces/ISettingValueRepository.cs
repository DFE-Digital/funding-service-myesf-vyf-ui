using PDS.ViewYourFunding.Repositories.DataModels;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Interfaces
{
    /// <summary>
    /// The Setting value Repository Interface.
    /// </summary>
    public interface ISettingValueRepository : IRepository<SettingValue>
    {
        /// <summary>
        /// Gets the setting value for a given funding stream and setting.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream ID.</param>
        /// <param name="settingId">The setting ID.</param>
        /// <returns>The setting value for the given funding stream id and setting id .</returns>
        Task<SettingValue> GetSettingValueById(int fundingStreamId, int settingId);

        /// <summary>
        /// Updates the setting value.
        /// </summary>
        /// <param name="settingValue">The setting value.</param>
        /// <returns>Returns true, is setting updated successfully.</returns>
        Task<bool> UpdateSettingValue(SettingValue settingValue);

        /// <summary>
        /// Delete setting value.
        /// </summary>
        /// <param name="settingValue">Setting value to delete.</param>
        /// <returns>Returns true, is setting was deleted successfully.</returns>
        Task<bool> DeleteSettingValue(SettingValue settingValue);
    }
}