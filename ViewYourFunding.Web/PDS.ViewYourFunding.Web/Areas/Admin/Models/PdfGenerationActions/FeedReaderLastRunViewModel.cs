using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.PdfGenerationActions
{
    public class FeedReaderLastRunViewModel : PdfGenerationActionsPageViewModel
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
                FeedReaderLastRunBreadCrumb()
            };

        #endregion

        /// <summary>
        /// Gets or sets the latest data import audit item.
        /// </summary>
        public DataImportAuditModel Audit { get; set; }
    }
}
