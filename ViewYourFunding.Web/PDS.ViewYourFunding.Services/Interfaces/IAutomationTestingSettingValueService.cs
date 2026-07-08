using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// An interface exposing set operations for settings in the View Your Funding area.
    /// </summary>
    public interface IAutomationTestingSettingValueService
    {
        /// <summary>
        /// Updates the setting value for a given funding stream and setting.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream ID.</param>
        /// <param name="settingValueId">The setting ID.</param>
        /// <param name="newValue">The new setting value.</param>
        /// <returns>True if successful.</returns>
        Task<bool> UpdateSettingValueAsync(int fundingStreamId, int settingValueId, string newValue);

        /// <summary>
        /// Deletes the PSG regression setting value.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <returns>True if successful.</returns>
        Task<bool> DeletePsgRegressionSettingValue(int fundingStreamId);
    }
}