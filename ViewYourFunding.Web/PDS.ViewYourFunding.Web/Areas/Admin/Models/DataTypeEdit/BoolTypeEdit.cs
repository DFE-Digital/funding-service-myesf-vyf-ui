using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit
{
    /// <summary>
    /// The Bool type edit class.
    /// </summary>
    /// <seealso cref="DataTypeBaseEdit" />
    public class BoolTypeEdit : DataTypeBaseEdit
    {
        /// <summary>
        /// Gets or sets a value indicating whether new value.
        /// </summary>
        /// <value>
        /// The new value.
        /// </value>
        [Display(Name = "New Value")]
        public bool NewValue { get; set; }

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