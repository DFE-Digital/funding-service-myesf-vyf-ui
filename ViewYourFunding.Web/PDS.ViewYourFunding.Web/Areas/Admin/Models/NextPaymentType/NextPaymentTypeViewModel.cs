using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPaymentType
{
    /// <summary>
    /// The next payment type index class.
    /// </summary>
    /// <seealso cref="NextPaymentTypePageViewModel" />
    public class NextPaymentTypeViewModel : NextPaymentTypePageViewModel
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
                    NextPaymentTypesBreadCrumb(FundingStreamId, FundingStreamName),
                    NextPaymentTypeActionModeBreadCrumb(NextPaymentType?.Id, FundingStreamId, ActionMode),
                    NextPaymentTypeAreYouSureBreadCrumb(true)
                }
                : ChangesSaved
                 ? new List<BreadCrumbViewModel>
                {
                    AdminHomeBreadCrumb,
                    SettingsListBreadCrumb(),
                    SettingsFundingStreamBreadCrumb(FundingStreamId, FundingStreamName),
                    NextPaymentTypesBreadCrumb(FundingStreamId, FundingStreamName),
                }
                : new List<BreadCrumbViewModel>
                {
                    AdminHomeBreadCrumb,
                    SettingsListBreadCrumb(),
                    SettingsFundingStreamBreadCrumb(FundingStreamId, FundingStreamName),
                    NextPaymentTypesBreadCrumb(FundingStreamId, FundingStreamName),
                    NextPaymentTypeActionModeBreadCrumb(NextPaymentType?.Id, FundingStreamId, ActionMode, true)
                };


        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Model Properties

        /// <summary>
        /// Gets or sets the current funding stream next payment type value being actioned.
        /// </summary>
        public NextPaymentType NextPaymentType { get; set; }

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
        /// Gets or sets a value indicating whether current page is Are you sure page.
        /// </summary>
        /// <value>
        /// The bool value to state if Are you sure page .
        /// </value>
        public bool IsAreYouSurePage { get; set; }

        #endregion
    }
}