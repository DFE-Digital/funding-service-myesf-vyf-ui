using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.BusinessAllocationsManagement
{
    public class SearchByProviderDataResultViewModel : BusinessAllocationsManagementPageViewModel
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
                SearchByProviderAndYearBreadCrumb(ProviderFunding.FundingStreamCode),
                SearchByProviderAndYearDetailsBreadCrumb(ProviderFunding.FundingStreamCode, ProviderFunding.FundingPeriodId, ProviderFunding.Provider.Name, ProviderFunding.PartitionKey),
                SearchByProviderDataResultBreadCrumb(true)
            };

        /// <inheritdoc/>
        public override bool IsTwoThirdsLayout => false;

        #endregion

        /// <summary>
        /// Gets or sets the funding stream business allocation name.
        /// </summary>
        /// <value>
        /// The funding stream business allocation name.
        /// </value>
        public string FundingStreamBusinessAllocationName { get; set; }

        /// <summary>
        /// Gets or sets the provider funding.
        /// </summary>
        /// /// <value>
        /// The selected provider funding.
        /// </value>
        public ProviderFundingModel ProviderFunding { get; set; }

        /// <summary>
        /// Gets or sets the formatted funding period code.
        /// </summary>
        /// <value>
        /// The formatted funding period code.
        /// </value>
        public string FormattedFundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the data value.
        /// </summary>
        /// <value>
        /// The data value.
        /// </value>
        public string DataValue { get; set; }

        /// <summary>
        /// Gets or sets the formatted data value.
        /// </summary>
        /// <value>
        /// The json formatted data value to output.
        /// </value>
        public string FormattedDataValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether or not there is a validation error on the page.
        /// </summary>
        public bool ValidationError { get; set; }
    }
}
