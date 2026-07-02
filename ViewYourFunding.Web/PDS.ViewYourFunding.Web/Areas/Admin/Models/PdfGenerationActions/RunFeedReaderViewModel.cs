using Microsoft.AspNetCore.Mvc.Rendering;
using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.PdfGenerationActions
{
    public class RunFeedReaderViewModel : PdfGenerationActionsPageViewModel
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
                RunFeedReaderBreadCrumb(true)
            };


        /// <summary>
        /// Gets or sets a value indicating whether [by pass bookmark].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [by pass bookmark]; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Bypass bookmark functionality")]
        public bool ByPassBookmark { get; set; }

        /// <summary>
        /// Gets or sets the funding stream codes.
        /// </summary>
        /// <value>
        /// The funding stream codes.
        /// </value>
        public IEnumerable<string> FundingStreamCodes { get; set; }

        /// <summary>
        /// Gets or sets the funding stream codes source.
        /// </summary>
        /// <value>
        /// The funding stream codes.
        /// </value>
        public IEnumerable<SelectListItem> FundingStreamCodesSource { get; set; }

        /// <summary>
        /// Gets or sets the selected funding stream codes.
        /// </summary>
        /// <value>
        /// The selected funding stream codes.
        /// </value>
        [Display(Name = "Selected funding stream code(s)")]
        public string SelectedFundingStreamCodes { get; set; }

        #endregion
    }
}