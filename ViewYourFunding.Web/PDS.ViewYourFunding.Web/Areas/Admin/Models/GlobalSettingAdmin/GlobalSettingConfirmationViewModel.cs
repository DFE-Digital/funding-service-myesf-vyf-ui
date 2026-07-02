using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.GlobalSettingAdmin
{
    /// <summary>
    /// The view model for the global setting confirmation page.
    /// </summary>
    public class GlobalSettingConfirmationViewModel : GlobalSettingPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                GeneralSettingsListBreadCrumb()
            };

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Model Properties

        /// <summary>
        /// Gets or sets the global setting being edited.
        /// </summary>
        public GlobalSetting.GlobalSetting GlobalSetting { get; set; }

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

        #endregion
    }
}