using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Services.Enums
{
    /// <summary>
    /// The funding view type (e.g. a spreadsheet or something else).
    /// </summary>
    public enum FundingViewType
    {
        /// <summary>
        /// A spreadsheet.
        /// </summary>
        Spreadsheet = 1,

        /// <summary>
        /// An object representation of some view data.
        /// </summary>
        [Display(Name = "UI view")]
        ViewData = 2,

        /// <summary>
        /// An object representation of another view data type.
        /// </summary>
        [Display(Name = "Other")]
        Other = 3,

        /// <summary>
        /// An object representation of pdf type.
        /// </summary>
        [Display(Name = "PDF")]
        Pdf = 4,
    }
}