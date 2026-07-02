using PDS.ViewYourFunding.Services.Enums;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Represents a setting for the View Your Funding area.
    /// </summary>
    public class SettingType
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

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
        /// Gets or sets a value indicating whether or not the setting values are allowed be updated in an admin area.
        /// </summary>
        public bool ValuesAreEditable { get; set; }

        /// <summary>
        /// Gets or sets the collection of values for this setting.
        /// </summary>
        public ICollection<SettingValue> SettingValues { get; set; }

        /// <summary>
        /// Gets or sets when the setting was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the setting was last updated.
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user-name of the last person to update the setting.
        /// </summary>
        public string LastUpdatedBy { get; set; }
    }
}