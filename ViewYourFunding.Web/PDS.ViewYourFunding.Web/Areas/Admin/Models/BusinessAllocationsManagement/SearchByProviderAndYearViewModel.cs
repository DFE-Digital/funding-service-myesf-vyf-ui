using Microsoft.AspNetCore.Mvc.Rendering;
using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.BusinessAllocationsManagement
{
    public class SearchByProviderAndYearViewModel : BusinessAllocationsManagementPageViewModel
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
                SelectAFundingStreamBreadCrumb(),
                SearchByProviderAndYearBreadCrumb(FundingStreamCode, true)
            };

        #endregion

        /// <summary>
        /// Gets or sets the funding stream code.
        /// </summary>
        /// <value>
        /// The funding stream code.
        /// </value>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream codes source.
        /// </summary>
        /// <value>
        /// The funding stream codes.
        /// </value>
        public IEnumerable<SelectListItem> FundingStreamPeriodCodes { get; set; }

        /// <summary>
        /// Gets or sets the selected funding period code.
        /// </summary>
        /// /// <value>
        /// The selected funding period code.
        /// </value>
        public string SelectedFundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the ukprn.
        /// </summary>
        /// <value>
        /// The ukprn.
        /// </value>
        [Required]
        [Display(Name = "UKPRN")]
        public string Ukprn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether or not there is a validation error on the page.
        /// </summary>
        public bool ValidationError { get; set; }
    }
}
