using PDS.ViewYourFunding.Services.Enums;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Represents a setting for the View Your Funding area.
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; protected set; }

        /// <summary>
        /// Gets or sets the name of this setting.
        /// </summary>
        public string SettingName { get; set; }

        /// <summary>
        /// Gets or sets the description of this setting.
        /// </summary>
        public string SettingDescription { get; set; }

        /// <summary>
        /// Gets or sets the data type of the values for this setting.
        /// </summary>
        public SettingValueDataType ValueDataType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether or not the setting values are allowed be updated in an admin area.
        /// </summary>
        public bool ValuesAreEditable { get; set; }

        /// <summary>
        /// Gets or sets the collection of values for this setting.
        /// </summary>
        public ICollection<SettingValue> SettingValues { get; set; }
    }
}