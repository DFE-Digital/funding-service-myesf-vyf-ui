using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.BusinessAllocationsManagement
{
    /// <summary>
    /// The view model for the allocations management home page.
    /// </summary>
    public class BusinessAllocationsManagementHomeViewModel : BusinessAllocationsManagementPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminManageAllocationHomeBreadCrumb,
                BusinessAllocationsManagementHomeBreadCrumb(true)
            };

        #endregion
    }
}