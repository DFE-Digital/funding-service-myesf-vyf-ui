using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit
{
    /// <summary>
    /// The string type edit class.
    /// </summary>
    /// <seealso cref="DataTypeBaseEdit" />
    public class StringTypeEdit : DataTypeBaseEdit
    {
        /// <summary>
        /// Gets or sets new value.
        /// </summary>
        /// <value>
        /// The new value.
        /// </value>
        [Display(Name = "New Value")]
        [Required(ErrorMessage = EditTypeConstants.NewValueRequiredErrorMessage)]
        public string NewValue { get; set; }
    }
}