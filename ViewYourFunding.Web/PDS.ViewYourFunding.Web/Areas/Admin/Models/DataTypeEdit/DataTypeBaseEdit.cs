using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.GlobalSettingAdmin;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit
{
    /// <summary>
    /// The base class of data edit types.
    /// </summary>
    public class DataTypeBaseEdit : GlobalSettingEditViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => IsCurrentPageEditPage ? new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                GeneralSettingsListBreadCrumb(),
                GeneralSettingEditBreadCrumb(DataTypeId, Description, IsCurrentPageEditPage)
            }
            :
            new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                GeneralSettingsListBreadCrumb(),
                GeneralSettingEditBreadCrumb(DataTypeId, Description, IsCurrentPageEditPage),
                GeneralSettingsAreYouSureBreadCrumb(true)
            };

        #endregion


        /// <summary>
        /// Gets or sets the data type identifier.
        /// </summary>
        /// <value>
        /// The data type identifier.
        /// </value>
        public int DataTypeId { get; set; }

        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        /// <value>
        /// The current value.
        /// </value>
        [Display(Name = "Current Value")]
        public string CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets new value.
        /// </summary>
        /// <value>
        /// The new value.
        /// </value>
        [Display(Name = "New Value")]
        public string NewValueString { get; set; }

        /// <summary>
        /// Gets or sets the name of the edit template.
        /// </summary>
        /// <value>
        /// The name of the edit template.
        /// </value>
        public string EditTemplateName { get; set; }

        /// <summary>
        /// Gets or sets the type of the setting edit.
        /// </summary>
        /// <value>
        /// The type of the setting edit.
        /// </value>
        public SettingEditType SettingEditType { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current page is edit page.
        /// </summary>
        /// <value>
        /// Is current page edit page.
        /// </value>
        public bool IsCurrentPageEditPage { get; set; }
    }
}