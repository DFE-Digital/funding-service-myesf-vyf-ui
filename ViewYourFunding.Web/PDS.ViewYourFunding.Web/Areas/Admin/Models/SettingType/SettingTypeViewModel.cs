using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Attributes;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType
{
    /// <summary>
    /// The next payment type index class.
    /// </summary>
    /// <seealso cref="SettingTypePageViewModel" />
    public class SettingTypeViewModel : SettingTypePageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            =>
                IsAreYouSurePage && !ChangesSaved
                ? new List<BreadCrumbViewModel>
                {
                    AdminHomeBreadCrumb,
                    SettingsListBreadCrumb(),
                    SettingTypeActionModeBreadCrumb(SettingType?.Id, SettingType?.IsSettingTypeInUse, ActionMode),
                    SettingTypeAreYouSureBreadCrumb(true)
                }
                : ChangesSaved
                 ? new List<BreadCrumbViewModel>
                {
                    AdminHomeBreadCrumb,
                    SettingsListBreadCrumb(),
                }
                : new List<BreadCrumbViewModel>
                {
                    AdminHomeBreadCrumb,
                    SettingsListBreadCrumb(false),
                    SettingTypeActionModeBreadCrumb(SettingType?.Id, SettingType?.IsSettingTypeInUse, ActionMode, true)
                };

        /// <summary>
        /// Gets a value indicating whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Model Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the current funding stream setting type value being actioned.
        /// </summary>
        public SettingType SettingType { get; set; }

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the name of the setting name.
        /// </summary>
        /// <value>
        /// The name of the setting type.
        /// </value>
        [DistinctSettingType(nameof(Id), ErrorMessage = "Please use a distinct setting type name.")]
        public string SettingTypeName { get; set; }

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
        /// The boolean value to state if Are you sure page .
        /// </value>
        public bool IsAreYouSurePage { get; set; }

        #endregion
    }
}