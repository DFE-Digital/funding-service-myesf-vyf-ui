using PDS.ViewYourFunding.Services.Models;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// The Admin screen funding stream settings service class.
    /// </summary>
    public interface IAdminFundingStreamSettingService
    {
        /// <summary>
        /// Updates the setting value.
        /// </summary>
        /// <param name="setting">The setting value.</param>
        /// <returns>Returns true, if update was successful.</returns>
        Task<bool> UpdateAsync(SettingValue setting);

        /// <summary>
        /// Delete the setting value.
        /// </summary>
        /// <param name="settingValue">The setting value to delete.</param>
        /// <returns>Returns true, if delete was successful.</returns>
        Task<bool> DeleteAsync(SettingValue settingValue);

        /// <summary>
        /// Add the setting value.
        /// </summary>
        /// <param name="settingValue">The setting value to add.</param>
        /// <returns>Returns setting value, if addition was successful.</returns>
        Task<SettingValue> AddAsync(SettingValue settingValue);

        /// <summary>
        /// Gets the first or default SettingValue.
        /// </summary>
        /// <param name="settingValueId">The setting value id.</param>
        /// <param name="fundingStreamId">The funding stream id.</param>
        /// <returns>The Setting Value.</returns>
        Task<SettingValue> GetFirstOrDefaultAsync(int settingValueId, int fundingStreamId);
    }
}
