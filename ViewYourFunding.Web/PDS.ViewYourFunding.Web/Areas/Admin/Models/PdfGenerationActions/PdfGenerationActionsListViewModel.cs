using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.PdfGenerationActions
{
    /// <summary>
    /// The view model for the view your funding settings list page.
    /// </summary>
    public class PdfGenerationActionsListViewModel : PdfGenerationActionsPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                PdfGenerationActionsListBreadCrumb(true)
            };

        #endregion
    }
}