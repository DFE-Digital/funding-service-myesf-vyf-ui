using Microsoft.AspNetCore.Mvc.Rendering;
using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.PdfGenerationActions
{
    /// <summary>
    /// The GenerateFundingReportsViewModel class.
    /// </summary>
    /// <seealso cref="PdfGenerationActionsPageViewModel" />
    public class GenerateFundingReportsViewModel : PdfGenerationActionsPageViewModel
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
                GenerateFundingReportsBreadCrumb(true)
            };

        /// <summary>
        /// Gets or sets the funding period code.
        /// </summary>
        /// <value>
        /// The funding period code.
        /// </value>
        [Display(Name = "Funding Period Code")]
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the funding period codes.
        /// </summary>
        /// <value>
        /// The funding period codes.
        /// </value>
        public IEnumerable<SelectListItem> FundingPeriodCodes { get; set; }

        /// <summary>
        /// Gets or sets the type of the report.
        /// </summary>
        /// <value>
        /// The type of the report.
        /// </value>
        [Display(Name = "Report Type")]
        public string ReportType { get; set; }

        /// <summary>
        /// Gets or sets the report types.
        /// </summary>
        public IEnumerable<SelectListItem> ReportTypes { get; set; }

        #endregion
    }
}