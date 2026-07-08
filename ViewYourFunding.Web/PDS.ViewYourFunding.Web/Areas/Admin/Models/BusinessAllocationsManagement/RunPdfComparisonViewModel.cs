using Microsoft.AspNetCore.Mvc.Rendering;
using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.BusinessAllocationsManagement
{
    public class RunPdfComparisonViewModel : BusinessAllocationsManagementPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminManageAllocationHomeBreadCrumb,
                BusinessAllocationsManagementHomeBreadCrumb(),
                RunPdfCompareBreadCrumb(true)
            };


        /// <summary>
        /// Gets or sets the funding stream and period code.
        /// </summary>
        /// <value>
        /// The funding stream and period code.
        /// </value>
        [Display(Name = "Select funding stream and funding period code")]
        [Required]
        public string FundingStreamCodeAndPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream and period codes.
        /// </summary>
        /// <value>
        /// The funding stream codes.
        /// </value>
        public IEnumerable<SelectListItem> FundingStreamCodesAndPeriodCodes { get; set; }

        /// <summary>
        /// Gets or sets the path to the production pdf file to be compared.
        /// </summary>
        /// <value>
        /// The path to the pdf file.
        /// </value>
        [Display(Name = "Enter name of source folder in production environment file share")]
        [Required]
        public string SourceFolder { get; set; }

        /// <summary>
        /// Gets or sets the path to the pre-prod Qc check file the production file will be compared against.
        /// </summary>
        /// <value>
        /// The path to the pdf file.
        /// </value>
        [Display(Name = "Enter name of quality assured folder in pre-production environment file share")]
        [Required]
        public string TargetFolder { get; set; }
        #endregion

    }
}
