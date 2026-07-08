using Microsoft.AspNetCore.Mvc.Rendering;
using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.BusinessAllocationsManagement
{
    public class SelectFundingStreamViewModel : BusinessAllocationsManagementPageViewModel
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
                SearchProviderDataBreadCrumb(),
                SelectAFundingStreamBreadCrumb(true)
            };

        #endregion

        /// <summary>
        /// Gets or sets the funding streams.
        /// </summary>
        /// <value>
        /// The next payment types.
        /// </value>
        public IEnumerable<SelectListItem> FundingStreams { get; set; }

        /// <summary>
        /// Gets or sets the funding stream codes source.
        /// </summary>
        /// <value>
        /// The funding stream codes.
        /// </value>
        public IEnumerable<SelectListItem> FundingStreamCodesSource { get; set; }

        /// <summary>
        /// Gets or sets the selected funding Stream Code.
        /// </summary>
        /// /// <value>
        /// The selected funding stream code.
        /// </value>
        public string SelectedFundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether or not there is a validation error on the page.
        /// </summary>
        public bool ValidationError { get; set; }
    }
}
