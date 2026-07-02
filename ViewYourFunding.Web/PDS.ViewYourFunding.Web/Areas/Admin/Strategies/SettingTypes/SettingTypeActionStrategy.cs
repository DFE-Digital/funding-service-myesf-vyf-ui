using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.SettingTypes
{
    /// <summary>
    /// The setting type strategy for handling related setting type actions.
    /// </summary>
    public class SettingTypeActionStrategy
    {
        /// <summary>
        /// Gets or sets the setting type actions.
        /// </summary>
        /// <value>
        /// The setting type actions.
        /// </value>
        public IList<ISettingTypeAction> SettingTypeActions { get; set; }
    }
}