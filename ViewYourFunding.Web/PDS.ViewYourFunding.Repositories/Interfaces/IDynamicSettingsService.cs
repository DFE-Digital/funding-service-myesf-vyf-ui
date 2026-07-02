using PDS.ViewYourFunding.Repositories.Enums;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Repositories.Interfaces
{
    /// <summary>
    /// Interface representing a settings service for settings that could change at runtime.
    /// </summary>
    public interface IDynamicSettingsService
    {
        /// <summary>
        /// Gets the current value of the given setting.
        /// </summary>
        /// <param name="toggleSettingType">The setting type to get.</param>
        /// <returns>The value of the given setting.</returns>
        string GetValueOf(ToggleSettingType toggleSettingType);

        /// <summary>
        /// Gets the collection of current values of the given setting.
        /// </summary>
        /// <param name="toggleSettingType">The setting type to get.</param>
        /// <param name="separator">The character to use to separate the delimited list of values.</param>
        /// <returns>The collection of values of the given setting.</returns>
        IEnumerable<string> GetValuesOf(ToggleSettingType toggleSettingType, char separator = ';');

        /// <summary>
        /// Checks whether a toggle setting is enabled.
        /// </summary>
        /// <param name="toggleSettingType">The setting type to get.</param>
        /// <param name="defaultValue">The default value to return if the setting is not configured.</param>
        /// <returns>The configured setting value if present, or the default value otherwise.</returns>
        bool IsEnabled(ToggleSettingType toggleSettingType, bool defaultValue = false);
    }
}