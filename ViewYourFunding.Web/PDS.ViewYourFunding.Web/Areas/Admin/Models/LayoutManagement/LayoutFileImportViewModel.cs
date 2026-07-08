using Microsoft.AspNetCore.Http;
using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Attributes;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement
{
    /// <summary>
    /// The view model for the import layout page.
    /// </summary>
    public class LayoutFileImportViewModel : LayoutImportViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                LayoutManagementHomeBreadCrumb(false),
                UploadLayoutBreadCrumb(true)
            };

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the file upload.
        /// </summary>
        /// <value>
        /// The file upload.
        /// </value>
        [Required]
        [AllowedFileExtensions(new[] { ".json" }, ErrorMessage = "Only .json files are allowed.")]
        [Display(Name = "Upload a layout Json file")]
        public IFormFile FileUpload { get; set; }

        /// <summary>
        /// Gets or sets the name of the funding stream.
        /// </summary>
        /// <value>
        /// The name of the funding stream.
        /// </value>
        [Display(Name = "Funding Stream Name")]
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets the layout action.
        /// </summary>
        /// <value>
        /// The layout action.
        /// </value>
        public LayoutAction LayoutAction { get; set; }

        /// <summary>
        /// Gets or sets the layout model identifier.
        /// </summary>
        /// <value>
        /// The layout model identifier.
        /// </value>
        public string LayoutModelId { get; set; }

        #endregion
    }
}