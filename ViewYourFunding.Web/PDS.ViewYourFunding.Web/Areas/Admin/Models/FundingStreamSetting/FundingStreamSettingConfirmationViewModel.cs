using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting
{
    /// <summary>
    /// View Model for Settings confirmation.
    /// </summary>
    /// <seealso cref="FundingStreamSettingsPageViewModel" />
    public class FundingStreamSettingConfirmationViewModel : FundingStreamSettingsPageViewModel
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string ContentTitle => "Settings Edit Confirmation";

        /// <summary>
        /// Gets the bread crumb items.
        /// </summary>
        /// <value>
        /// The bread crumb items.
        /// </value>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                SettingsListBreadCrumb(),
                SettingsFundingStreamBreadCrumb(FundingStreamId, FundingStreamName)
            };

        /// <summary>
        /// Gets a value indicating whether determines if this screen is a two thirds layout view.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the submitted at display date.
        /// </summary>
        /// <value>
        /// The submitted at display date.
        /// </value>
        public string SubmittedAtDisplayDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the funding stream.
        /// </summary>
        /// <value>
        /// The name of the funding stream.
        /// </value>
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether spreadsheet was generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if spreadsheet was generated; otherwise, <c>false</c>.
        /// </value>
        public bool SpreadsheetGenerated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether changes were saved.
        /// </summary>
        /// <value>
        ///   <c>true</c> if changes were saved; otherwise, <c>false</c>.
        /// </value>
        public bool ChangesSaved { get; set; }

        /// <summary>
        /// Gets or sets the action mode.
        /// </summary>
        /// <value>
        /// The action being performed e.g. Edit, Delete, Add.
        /// </value>
        public FundingStreamSettingAction ActionMode { get; set; }
    }
}