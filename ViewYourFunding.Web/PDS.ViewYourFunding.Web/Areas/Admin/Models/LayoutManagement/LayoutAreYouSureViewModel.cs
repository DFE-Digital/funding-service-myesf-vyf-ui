using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement
{
    /// <summary>
    /// The layout for are you sure viewModel.
    /// </summary>
    public class LayoutAreYouSureViewModel : LayoutManagementPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                LayoutManagementHomeBreadCrumb(false),
                DeleteLayoutBreadCrumb(true)
            };

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Model Properties

        /// <summary>
        /// Gets or sets the layout UI model.
        /// </summary>
        /// <value>
        /// The layout UI model.
        /// </value>
        public LayoutUiModel LayoutUiModel { get; set; }

        /// <summary>
        /// Gets or sets the layout action.
        /// </summary>
        /// <value>
        /// The layout action.
        /// </value>
        public LayoutAction LayoutAction { get; set; }

        #endregion
    }
}
