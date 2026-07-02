using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.GlobalSettingAdmin
{
    /// <summary>
    /// The view model for the global settings list page.
    /// </summary>
    public class GlobalSettingListViewModel : GlobalSettingPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                GeneralSettingsListBreadCrumb(true)
            };

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Model Properties

        /// <summary>
        /// Gets or sets the global settings.
        /// </summary>
        /// <value>
        /// The global settings.
        /// </value>
        public IReadOnlyList<GlobalSetting.GlobalSetting> GlobalSettings { get; set; }

        #endregion
    }
}