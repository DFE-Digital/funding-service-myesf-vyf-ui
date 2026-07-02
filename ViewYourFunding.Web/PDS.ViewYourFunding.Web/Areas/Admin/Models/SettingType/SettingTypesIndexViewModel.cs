using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType
{
    /// <summary>
    /// The next payment type index class.
    /// </summary>
    /// <seealso cref="SettingTypesIndexViewModel" />
    public class SettingTypesIndexViewModel : SettingTypePageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                SettingTypesBreadCrumb(true)
            };

        /// <summary>
        /// Gets a value indicating whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Model Properties

        /// <summary>
        /// Gets or sets the list of setting types.
        /// </summary>
        public IEnumerable<SettingType> SettingTypes { get; set; }

        #endregion
    }
}
