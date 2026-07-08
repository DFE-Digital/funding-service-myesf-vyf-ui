using PDS.ViewYourFunding.Web.Enums;
using System;

namespace PDS.ViewYourFunding.Web.Models.GlobalSetting
{
    /// <summary>
    /// Represents an application setting.
    /// </summary>
    public class GlobalSetting
    {
        #region Properties

        /// <summary>
        /// Gets or sets the data store id of this instance.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of setting that this instance represents.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the value of the setting.
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// Gets or sets when the setting was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the setting's value was last updated.
        /// </summary>
        /// <remarks>Will be set to the same value as the CreatedAt upon creation.</remarks>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the setting description.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the setting edit type.
        /// </summary>
        public SettingEditType EditType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the setting is read only.
        /// </summary>
        public virtual bool ReadOnly { get; set; }

        /// <summary>
        /// Gets the last updated by.
        /// </summary>
        /// <value>
        /// The last updated by.
        /// </value>
        public string LastUpdatedBy { get; internal set; }

        #endregion
    }
}
