using PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.SettingTypes
{
    /// <summary>
    /// The setting type action class.
    /// </summary>
    public interface ISettingTypeAction
    {
        /// <summary>
        /// Actions the specified payment type id.
        /// </summary>
        /// <param name="settingTypeId">The setting type identifier.</param>
        /// <returns>The SettingTypeViewModel. </returns>
        Task<SettingTypeViewModel> Action(int settingTypeId);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="settingTypeViewModel">The view your funding setting type view model.</param>
        /// <returns>True if the changes are saved successfully.</returns>
        Task<bool> SaveChanges(SettingTypeViewModel settingTypeViewModel);

        /// <summary>
        /// Checks if the action mode applies to the class.
        /// </summary>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>True if the action mode applies to the action.</returns>
        bool AppliesTo(ActionMode actionMode);

        /// <summary>
        /// Gets the action mode.
        /// </summary>
        /// <value>
        /// The action mode.
        /// </value>
        ActionMode ActionMode { get; }
    }
}