using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication
{
    /// <summary>
    /// The view model for the publication action.
    /// </summary>
    public class PublicationActionViewModel : PublicationPageViewModel
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
                    SettingsFundingStreamBreadCrumb(FundingStreamId, FundingStreamName),
                    SettingsActionModeBreadCrumb(FundingPublication?.Id, FundingStreamId, ActionMode, false),
                    SettingsAreYouSureBreadCrumb(true)
                }
                : ChangesSaved
                 ? new List<BreadCrumbViewModel>
                {
                    AdminHomeBreadCrumb,
                    SettingsListBreadCrumb(),
                    SettingsFundingStreamBreadCrumb(FundingStreamId, FundingStreamName)
                }
                : new List<BreadCrumbViewModel>
                {
                    AdminHomeBreadCrumb,
                    SettingsListBreadCrumb(),
                    SettingsFundingStreamBreadCrumb(FundingStreamId, FundingStreamName),
                    SettingsActionModeBreadCrumb(FundingPublication?.Id, FundingStreamId, ActionMode, true)
                };


        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Model Properties

        /// <summary>
        /// Gets or sets the current funding stream publication value being edited.
        /// </summary>
        public PublicationViewModel FundingPublication { get; set; }

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the name of the funding stream.
        /// </summary>
        /// <value>
        /// The name of the funding stream.
        /// </value>
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the changes were saved.
        /// </summary>
        /// <value>
        /// Whether the changes were saved.
        /// </value>
        public bool ChangesSaved { get; set; }

        /// <summary>
        /// Gets or sets the action mode.
        /// </summary>
        /// <value>
        /// The action mode.
        /// </value>
        public ActionMode ActionMode { get; set; }

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

        /// <summary>
        /// Gets or sets the funding stream code.
        /// </summary>
        /// <value>
        /// The funding stream code.
        /// </value>
        public string FundingStreamCode { get; set; }

        #endregion
    }
}