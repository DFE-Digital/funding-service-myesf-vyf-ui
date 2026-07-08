using Microsoft.AspNetCore.Mvc.Rendering;
using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.PdfGenerationActions
{
    public class RerunPdfGenerationViewModel : PdfGenerationActionsPageViewModel
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
                RerunPdfGenerationBreadCrumb(true)
            };

        /// <summary>
        /// The cosmos date regex.
        /// </summary>
        private const string CosmosDateFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";
        private const string CosmosDateRegex = @"\d{4}-(?:0[1-9]|1[0-2])-(?:0[1-9]|[1-2]\d|3[0-1])T(?:[0-1]\d|2[0-3]):[0-5]\d:[0-5]\d(\.\d{3,8})(Z|([+|-][0-1][0-9]:[0-5][0-9]))";

        /// <summary>
        /// Gets or sets the start date  for the date range.
        /// </summary>
        /// <value>
        /// The start date for the date range.
        /// </value>
        [Display(Name = "Created between")]
        [Required]
        [RegularExpression(CosmosDateRegex, ErrorMessage = "Please enter a valid date in format :" + CosmosDateFormat)]
        public string SinceCreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the end date for the date range.
        /// </summary>
        /// <value>
        /// The end date for the date range date.
        /// </value>
        [Display(Name = "And")]
        [Required]
        [RegularExpression(CosmosDateRegex, ErrorMessage = "Please enter a valid date in format :" + CosmosDateFormat)]
        public string EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the funding stream code.
        /// </summary>
        /// <value>
        /// The funding stream code.
        /// </value>
        [Display(Name = "Funding Stream Code")]
        [Required]
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream codes.
        /// </summary>
        /// <value>
        /// The funding stream codes.
        /// </value>
        public IEnumerable<SelectListItem> FundingStreamCodes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remove the funding pdf generated tag.
        /// </summary>
        [Display(Name = "Remove funding document generated tag")]
        public bool ResetFunding { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remove the provider funding pdf generated tag.
        /// </summary>
        [Display(Name = "Remove provider funding document generated tag")]
        public bool ResetProviderFunding { get; set; }

        #endregion
    }
}
