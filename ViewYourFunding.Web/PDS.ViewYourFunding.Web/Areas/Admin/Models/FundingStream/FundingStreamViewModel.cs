using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStream
{
    /// <summary>
    /// The funding stream viewModel.
    /// </summary>
    public class FundingStreamViewModel : FundingStreamPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => IsAreYouSurePage && !ChangesSaved
                ? new List<BreadCrumbViewModel>
                {
                    AdminHomeBreadCrumb,
                    SettingsListBreadCrumb(),
                    FundingStreamActionModeBreadCrumb(FundingStream?.Id, FundingStream?.FundingStreamName, ActionMode, false),
                    FundingStreamAreYouSureBreadCrumb(true)
                }
                : ChangesSaved && ActionMode != ActionMode.Delete
                ? new List<BreadCrumbViewModel>
                {
                    AdminHomeBreadCrumb,
                    SettingsListBreadCrumb(),
                    SettingsFundingStreamBreadCrumb(FundingStream?.Id, FundingStream?.FundingStreamName, false)
                }
                : ChangesSaved && ActionMode == ActionMode.Delete
                ? new List<BreadCrumbViewModel>
                {
                    AdminHomeBreadCrumb,
                    SettingsListBreadCrumb()
                }
                : ActionMode == ActionMode.Add
                ? new List<BreadCrumbViewModel>
                {
                    AdminHomeBreadCrumb,
                    SettingsListBreadCrumb(),
                    FundingStreamActionModeBreadCrumb(FundingStream?.Id, FundingStream?.FundingStreamName, ActionMode, true)
                }
                : new List<BreadCrumbViewModel>
                 {
                    AdminHomeBreadCrumb,
                    SettingsListBreadCrumb(),
                    SettingsFundingStreamBreadCrumb(FundingStream?.Id, FundingStream?.FundingStreamName, false),
                    FundingStreamActionModeBreadCrumb(FundingStream?.Id, FundingStream?.FundingStreamName, ActionMode, true)
                 };

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Model Properties

        /// <summary>
        /// Gets or sets the current funding stream value being edited.
        /// </summary>
        public FundingStream FundingStream { get; set; }

        /// <summary>
        /// Gets or sets the action mode.
        /// </summary>
        /// <value>
        /// The action mode.
        /// </value>
        public ActionMode ActionMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the changes were saved.
        /// </summary>
        /// <value>
        /// Whether the changes were saved.
        /// </value>
        public bool ChangesSaved { get; set; }

        /// <summary>
        /// Gets or sets the submitted at display date.
        /// </summary>
        /// <value>
        /// The submitted at display date.
        /// </value>
        public string SubmittedAtDisplayDate { get; set; }

        /// <summary>
        /// Gets or sets an optional error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current page is Are you sure page.
        /// </summary>
        public bool IsAreYouSurePage { get; set; }

        #endregion
    }
}