using Microsoft.AspNetCore.Mvc.Rendering;
using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.PdfGenerationActions
{
    /// <summary>
    /// The Run PDF Comparison View Model.
    /// </summary>
    /// <seealso cref="PdfGenerationActionsPageViewModel" />
    public class RunPdfComparisonViewModel : PdfGenerationActionsPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                PdfGenerationActionsListBreadCrumb(),
                RunPdComparisonBreadCrumb(true)
            };

        /// <summary>
        /// Gets or sets the funding stream code and period code.
        /// </summary>
        /// <value>
        /// The funding stream code and period code.
        /// </value>
        [Display(Name = "Funding Stream Code and Funding Period Code")]
        public string FundingStreamCodeAndPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream codes and period codes.
        /// </summary>
        /// <value>
        /// The funding stream code and period codes.
        /// </value>
        public IEnumerable<SelectListItem> FundingStreamCodesAndPeriodCodes { get; set; }

        /// <summary>
        /// Gets or sets the source folder.
        /// </summary>
        /// <value>
        /// The source folder.
        /// </value>
        [Display(Name = "Source folder in Production environment file share")]
        [Required]
        public string SourceFolder { get; set; }

        /// <summary>
        /// Gets or sets the destination folder.
        /// </summary>
        /// <value>
        /// The destination folder.
        /// </value>
        [Display(Name = "Quality assured folder in PreProduction environment file share")]
        [Required]
        public string DestinationFolder { get; set; }

        #endregion
    }
}