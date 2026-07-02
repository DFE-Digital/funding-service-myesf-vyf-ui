using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting
{
    /// <summary>
    /// The view model for the action page.
    /// </summary>
    public class FundingStreamSettingActionViewModel : FundingStreamSettingsPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                SettingsListBreadCrumb(),
                SettingsFundingStreamBreadCrumb(FundingStreamId, FundingStreamName),
                SettingsFundingStreamEditBreadCrumb(SettingName, true)
            };

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Model Properties

        /// <summary>
        /// Gets or sets the funding stream ID.
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
        /// Gets or sets the setting ID.
        /// </summary>
        /// <value>
        /// The setting identifier.
        /// </value>
        public int SettingId { get; set; }

        /// <summary>
        /// Gets the new setting value.
        /// </summary>
        /// <value>
        /// The setting value.
        /// </value>
        public string NewSettingValue
        {
            get
            {
                string newValue;
                switch (SettingValueDataType)
                {
                    case SettingValueDataType.String:
                        newValue = NewStringValue;
                        break;
                    case SettingValueDataType.Int:
                        newValue = NewIntValue.ToString();
                        break;
                    case SettingValueDataType.DateTime:
                        newValue = NewDateTimeValue.ToString("dd/MM/yyyy HH:mm");
                        break;
                    case SettingValueDataType.Bool:
                        newValue = NewBoolValue.ToString();
                        break;
                    case SettingValueDataType.Time:
                        newValue = NewTimeValue.ToString("HH:mm");
                        break;
                    case SettingValueDataType.Date:
                        newValue = NewDateValue.ToString("dd/MM/yyyy");
                        break;
                    case SettingValueDataType.NationalSpreadsheetLayout:
                        newValue = NewNationalSpreadsheetLayoutIdValue;
                        break;
                    default:
                        newValue = NewNationalLayoutIdValue;
                        break;
                }

                return newValue;
            }
        }

        /// <summary>
        /// Gets the new setting value if the current setting is a date and time.
        /// </summary>
        /// <value>
        /// The new date time value.
        /// </value>
        public DateTime NewDateTimeValue
        {
            get
            {
                var date = NewDateValue.Date;
                var time = NewTimeValue.TimeOfDay;
                return date.Add(time);
            }
        }

        /// <summary>
        /// Gets or sets the proposed new date value if this is a date (or date and time) type setting.
        /// </summary>
        [Display(Name = "New value")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime NewDateValue { get; set; }

        /// <summary>
        /// Gets or sets the proposed new time value if this is a time (or date and time) type setting.
        /// </summary>
        [Display(Name = "New value")]
        [DataType(DataType.Time)]
        [Required]
        public DateTime NewTimeValue { get; set; }

        /// <summary>
        /// Gets or sets the proposed new value if this is a string type setting.
        /// </summary>
        [Display(Name = "New value")]
        [DataType(DataType.Text)]
        [Required]
        public string NewStringValue { get; set; }

        /// <summary>
        /// Gets or sets the proposed new value if this is an integer type setting.
        /// </summary>
        [Display(Name = "New value")]
        [Required]
        public int NewIntValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the proposed new value if this is a boolean type setting.
        /// </summary>
        [Display(Name = "New value")]
        [Required]
        public bool NewBoolValue { get; set; }

        /// <summary>
        /// Gets or sets creates new national layout value.
        /// </summary>
        /// <value>
        /// The new national layout value.
        /// </value>
        [Display(Name = "New value")]
        public string NewNationalLayoutIdValue { get; set; }

        /// <summary>
        /// Gets or sets creates new national spreadsheet layout value.
        /// </summary>
        /// <value>
        /// The new national layout value.
        /// </value>
        [Display(Name = "New value")]
        public string NewNationalSpreadsheetLayoutIdValue { get; set; }

        /// <summary>
        /// Gets or sets the layout UI models.
        /// </summary>
        /// <value>
        /// The layout UI models.
        /// </value>
        public IReadOnlyList<LayoutUiModel> LayoutUiModels { get; set; }

        /// <summary>
        /// Gets or sets the setting description.
        /// </summary>
        /// <value>
        /// The setting description.
        /// </value>
        public string SettingDescription { get; set; }

        /// <summary>
        /// Gets or sets the name of the setting.
        /// </summary>
        /// <value>
        /// The name of the setting.
        /// </value>
        public string SettingName { get; set; }

        /// <summary>
        /// Gets or sets the type of the setting value data.
        /// </summary>
        /// <value>
        /// The type of the setting value data.
        /// </value>
        public SettingValueDataType SettingValueDataType { get; set; }

        /// <summary>
        /// Gets or sets the action mode.
        /// </summary>
        /// <value>
        /// The action mode.
        /// </value>
        public FundingStreamSettingAction ActionMode { get; set; }

        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        /// <value>
        /// The current value.
        /// </value>
        public string CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets the setting value identifier.
        /// </summary>
        /// <value>
        /// The setting value identifier.
        /// </value>
        public int SettingValueId { get; set; }

        #endregion
    }
}