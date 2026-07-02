using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Repositories.Interfaces;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Rules
{
    /// <summary>
    /// Abstract class to check if a rule is enabled.
    /// </summary>
    public abstract class SettingMustBeEnabledBase
    {
        private readonly IDynamicSettingsService _dynamicSettingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingMustBeEnabledBase"/> class.
        /// </summary>
        /// <param name="dynamicSettingsService">The dynamic settings service.</param>
        protected SettingMustBeEnabledBase(IDynamicSettingsService dynamicSettingsService)
        {
            _dynamicSettingsService = dynamicSettingsService;
        }

        /// <summary>
        /// Determines whether the specified setting type is enabled.
        /// </summary>
        /// <param name="toggleSettingType">Type of the setting.</param>
        /// <returns>
        ///   <c>true</c> if the specified setting type is met; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEnabled(ToggleSettingType toggleSettingType)
        {
            return _dynamicSettingsService.IsEnabled(toggleSettingType);
        }
    }
}