using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.BusinessAllocationsManagement
{
    public class FeedReaderLastRunViewModel : BusinessAllocationsManagementPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => FeedReaderLastRunBreadCrumb();

        /// <summary>
        /// Gets the breadcrumbs based on condition.
        /// </summary>
        /// <returns>IList of BreadCrumbViewModel.</returns>
        public IList<BreadCrumbViewModel> FeedReaderLastRunBreadCrumb()
        {
            if (ShowAllFundingStreams)
            {
                return new List<BreadCrumbViewModel>
                {
                    AdminManageAllocationHomeBreadCrumb,
                    BusinessAllocationsManagementHomeBreadCrumb(),
                    CheckStatusBreadCrumb(true)
                };
            }
            else
            {
                return new List<BreadCrumbViewModel>
                {
                    AdminManageAllocationHomeBreadCrumb,
                    BusinessAllocationsManagementHomeBreadCrumb(),
                    RunFeedReaderBreadCrumb(),
                    FeedReaderLastRunBreadCrumb(SelectedFundingStreamCodes, true)
                };
            }
        }

        #endregion

        public FeedReaderLastRunViewModel()
        {
            Audit = new Dictionary<string, DataImportAuditModel>();
        }

        /// <summary>
        /// Gets or sets the latest data import audit item.
        /// </summary>
        public Dictionary<string, DataImportAuditModel> Audit { get; set; }

        /// <summary>
        /// Gets or sets the funding stream codes.
        /// </summary>
        /// <value>
        /// The funding stream codes.
        /// </value>
        public IEnumerable<string> FundingStreamCodes { get; set; }

        /// <summary>
        /// Gets or sets the selected funding stream codes.
        /// </summary>
        /// <value>
        /// The selected funding stream codes.
        /// </value>
        [Display(Name = "Selected funding stream code(s)")]
        public string SelectedFundingStreamCodes { get; set; }

        /// <summary>
        /// Gets a value indicating whether to disable feed reader run.
        /// </summary>
        public bool DisableFeedReaderRun => Audit.Any(audit => audit.Value.Status == "Started");

        /// <summary>
        /// Gets the CFS Environment where the feed reader has read data from.
        /// </summary>
        /// <param name="fundingUri">The funding uri.</param>
        /// <returns>The mapped environment.</returns>
        public string Environment(string fundingUri)
        {
            if (fundingUri.StartsWith("https://api-calculate-funding.education.gov.uk"))
            {
                return "Production";
            }

            if (fundingUri.StartsWith("https://pp-api-calculate-funding.education.gov.uk"))
            {
                return "Pre-production";
            }

            return "Test";
        }

        /// <summary>
        /// Gets or sets a value indicating whether all funding streams are shown.
        /// </summary>
        public bool ShowAllFundingStreams { get; set; }
    }
}
