using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.PdfGenerationActions
{
    /// <summary>
    /// The PdfGenerationOperationConfirmationViewModel class.
    /// </summary>
    /// <seealso cref="PdfGenerationActionsPageViewModel" />
    public class PdfGenerationOperationConfirmationViewModel : PdfGenerationActionsPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                PdfGenerationActionsListBreadCrumb(),
                GetConfirmationBreadCrumb(PdfGenerationAction),
                RunFeedReaderConfirmationBreadCrumb(true)
            };

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether action has been successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the PDF generation action.
        /// </summary>
        /// <value>
        /// The PDF generation action.
        /// </value>
        public PdfGenerationAction PdfGenerationAction { get; set; }
    }
}