using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Repositories.Interfaces;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Repositories.Implementations
{
    /// TODO Needs implementation on another branch.
    /// <summary>
    /// The Dynamic Settings Service class.
    /// </summary>
    /// <seealso cref="IDynamicSettingsService" />
    public class DynamicSettingsService : IDynamicSettingsService
    {
        /// <inheritdoc/>
        public string GetValueOf(ToggleSettingType toggleSettingType)
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetValuesOf(ToggleSettingType toggleSettingType, char separator = ';')
        {
            return new List<string>();
        }

        /// <inheritdoc/>
        public bool IsEnabled(ToggleSettingType toggleSettingType, bool defaultValue = false)
        {
            return true;
        }
    }
}