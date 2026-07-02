using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit
{
    /// <summary>
    /// The int type edit class.
    /// </summary>
    /// <seealso cref="DataTypeBaseEdit" />
    public class IntTypeEdit : DataTypeBaseEdit
    {
        /// <summary>
        /// Gets or sets new value.
        /// </summary>
        /// <value>
        /// The new value.
        /// </value>
        [Display(Name = "New Value")]
        [Required(ErrorMessage = EditTypeConstants.NewValueRequiredErrorMessage)]
        [Range(1, 2000000000, ErrorMessage = EditTypeConstants.RangeRequiredErrorMessage)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#}")]
        public int NewValue { get; set; }
    }
}