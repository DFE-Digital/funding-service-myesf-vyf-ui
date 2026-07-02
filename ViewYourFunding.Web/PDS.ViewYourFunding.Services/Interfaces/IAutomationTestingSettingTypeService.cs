using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// An interface exposing set operations for settings in the View Your Funding area.
    /// </summary>
    public interface IAutomationTestingSettingTypeService
    {
        /// <summary>
        /// Adds the regression test setting type.
        /// </summary>
        /// <returns>True if successful.</returns>
        Task<bool> AddRegressionSettingValueTestSettingTypeAsync();

        /// <summary>
        /// Deletes the regression test setting type.
        /// </summary>
        /// <returns>True if successful.</returns>
        Task<bool> DeleteRegressionSettingValueTestSettingTypeAsync();

        /// <summary>
        /// Deletes the regression test setting type.
        /// </summary>
        /// <returns>True if successful.</returns>
        Task<bool> DeleteRegressionTestSettingTypeAsync();
    }
}