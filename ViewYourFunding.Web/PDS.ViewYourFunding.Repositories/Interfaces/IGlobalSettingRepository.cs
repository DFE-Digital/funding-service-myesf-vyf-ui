using PDS.ViewYourFunding.Repositories.DataModels;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Interfaces
{
    /// <summary>
    /// Global Settings repository operations.
    /// </summary>
    public interface IGlobalSettingRepository : IRepository<GlobalSetting>
    {
        /// <summary>
        /// UpdateAsync GlobalSetting value and last updated at.
        /// </summary>
        /// <param name="globalSetting">Setting to UpdateAsync.</param>
        /// <returns>true if <see cref="GlobalSetting"/> is updated.</returns>
        /// <returns>A Task if <see cref="GlobalSetting"/> type is updated successfully.</returns>
        Task<bool> UpdateAsync(GlobalSetting globalSetting);

        /// <summary>
        /// Remove global setting for specified Type.
        /// </summary>
        /// <param name="typeId">Type id to remove.</param>
        /// <returns>true if <see cref="GlobalSetting"/> type removed.</returns>
        Task<bool> RemoveSettingByTypeAsync(int typeId);
    }
}