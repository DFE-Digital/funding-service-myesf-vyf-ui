using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting
{
    /// <summary>
    /// The view model for the 'Are You Sure?' page.
    /// </summary>
    public class FundingStreamSettingAreYouSureViewModel : FundingStreamSettingsPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the page title.
        /// </summary>
        public override string ContentTitle => "Are you sure?";

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems =>
            !string.IsNullOrWhiteSpace(NewValue)
                ? new List<BreadCrumbViewModel>
                {
                    AdminHomeBreadCrumb,
                    SettingsListBreadCrumb(),
                    SettingsFundingStreamBreadCrumb(FundingStreamId, FundingStreamName),
                    SettingsFundingStreamEditBreadCrumb(FundingStreamId, FundingStreamName, ActionMode.ToString(), true)
                }
                : new List<BreadCrumbViewModel> { AdminHomeBreadCrumb };

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Model Properties

        /// <summary>
        /// Gets or sets the setting description.
        /// </summary>
        /// <value>
        /// The setting description.
        /// </value>
        public string SettingDescription { get; set; }

        /// <summary>
        /// Gets or sets the setting identifier.
        /// </summary>
        /// <value>
        /// The setting identifier.
        /// </value>
        public int SettingId { get; set; }

        /// <summary>
        /// Gets or sets creates new value.
        /// </summary>
        /// <value>
        /// The new value.
        /// </value>
        public string NewValue { get; set; }

        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        /// <value>
        /// The current value.
        /// </value>
        public string CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets the name of the setting.
        /// </summary>
        /// <value>
        /// The name of the setting.
        /// </value>
        public string SettingName { get; set; }

        /// <summary>
        /// Gets or sets the name of the funding stream.
        /// </summary>
        /// <value>
        /// The name of the funding stream.
        /// </value>
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the funding stream setting action.
        /// </summary>
        /// <value>
        /// The funding stream setting action.
        /// </value>
        public FundingStreamSettingAction ActionMode { get; set; }

        /// <summary>
        /// Gets or sets the setting value identifier.
        /// </summary>
        /// <value>
        /// The setting value identifier.
        /// </value>
        public int SettingValueId { get; set; }

        /// <summary>
        /// Gets or sets the name of the national layout friendly.
        /// </summary>
        /// <value>
        /// The name of the national layout friendly.
        /// </value>
        public string NationalLayoutFriendlyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a national layout setting.
        /// </summary>
        /// <value>
        ///   True if it is a national layout setting.
        /// </value>
        public bool IsNationalLayoutSetting { get; set; }

        #endregion
    }
}