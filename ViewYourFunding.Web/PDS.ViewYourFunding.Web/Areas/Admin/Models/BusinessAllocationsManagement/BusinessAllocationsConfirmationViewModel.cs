using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.BusinessAllocationsManagement
{
    /// <summary>
    /// The PdfGenerationOperationConfirmationViewModel class.
    /// </summary>
    /// <seealso cref="PdfGenerationActionsPageViewModel" />
    public class BusinessAllocationsConfirmationViewModel : BusinessAllocationsManagementPageViewModel
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
            => FeedReaderBreadCrumb();

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        /// <returns>Ilist of BreadCrumbViewModel.</returns>
        public IList<BreadCrumbViewModel> FeedReaderBreadCrumb()
        {
            if (BusinessAllocationsAction == BusinessAllocationsAction.RunFeedReader)
            {
                return new List<BreadCrumbViewModel>
                {
                    AdminManageAllocationHomeBreadCrumb,
                    BusinessAllocationsManagementHomeBreadCrumb(),
                    RunFeedReaderBreadCrumb(),
                    FeedReaderLastRunBreadCrumb(SelectedFundingStreamCodes),
                    ConfirmationBreadCrumb(true)
                };
            }
            else
            {
                return new List<BreadCrumbViewModel>
                {
                    AdminManageAllocationHomeBreadCrumb,
                    BusinessAllocationsManagementHomeBreadCrumb(),
                    RunPdfCompareBreadCrumb(),
                    RunPdfConfirmComparisonBreadCrumb(SelectedFundingStreamCodes, SourceFolder, TargetFolder),
                    ConfirmationBreadCrumb(true)
                };
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether action has been successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the business allocation action.
        /// </summary>
        /// <value>
        /// The business allocation action.
        /// </value>
        public BusinessAllocationsAction BusinessAllocationsAction { get; set; }

        /// <summary>
        /// Gets or Sets Funding Stream Code And Period Code.
        /// </summary>
        public string FundingStreamCodeAndPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets Selected funding stream codes.
        /// </summary>
        public string SelectedFundingStreamCodes { get; set; }

        /// <summary>
        /// Gets or sets source folder.
        /// </summary>
        public string SourceFolder { get; set; }

        /// <summary>
        /// Gets or sets target folder.
        /// </summary>
        public string TargetFolder { get; set; }
    }
}