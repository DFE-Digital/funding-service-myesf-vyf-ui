using Microsoft.AspNetCore.Mvc.Rendering;
using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting
{
    /// <summary>
    /// The view model for the set type page.
    /// </summary>
    public class SetSettingTypeViewModel : FundingStreamSettingsPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the page title.
        /// </summary>
        public override string ContentTitle => FundingStreamName;

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                SettingsListBreadCrumb(),
                SettingsFundingStreamBreadCrumb(FundingStreamId, FundingStreamName)
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
        /// Gets or sets the setting type Id.
        /// </summary>
        /// <value>
        /// The setting identifier.
        /// </value>
        [Display(Name = "Setting Type")]
        public int SettingTypeId { get; set; }

        /// <summary>
        /// Gets or sets the setting types.
        /// </summary>
        /// <value>
        /// The setting types.
        /// </value>
        public IEnumerable<SelectListItem> SettingTypes { get; set; }

        #endregion
    }
}