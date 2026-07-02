using Microsoft.AspNetCore.Mvc.Rendering;
using PDS.ViewYourFunding.Web.Areas.Admin.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType
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
        [DistinctSettingType(nameof(Id), ErrorMessage = "Please use a distinct setting type name.")]
        [Required, Display(Name = "Setting Name")]
        public string SettingName { get; set; }

        /// <summary>
        /// Gets or sets the description of this setting.
        /// </summary>
        [Required, Display(Name = "Setting Description")]
        public string SettingDescription { get; set; }

        /// <summary>
        /// Gets or sets the data type of the values for this setting.
        /// </summary>
        [Required, Display(Name = "Data Type")]
        public SettingValueDataType ValueDataType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether or not the setting values are allowed be updated in an admin area.
        /// </summary>
        [Required, Display(Name = "Values Are Editable")]
        public bool ValuesAreEditable { get; set; }

        /// <summary>
        /// Gets or sets the collection of values for this setting.
        /// </summary>
        public ICollection<SettingValue> SettingValues { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is the setting type in use.
        /// </summary>
        public bool IsSettingTypeInUse { get; set; }

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

        /// <summary>
        /// Gets the values list.
        /// </summary>
        /// <value>
        /// The values list.
        /// </value>
        public IEnumerable<SelectListItem> ValuesList =>
            new List<SelectListItem>
            {
                new SelectListItem(bool.FalseString, bool.FalseString),
                new SelectListItem(bool.TrueString, bool.TrueString)
            };
    }
}