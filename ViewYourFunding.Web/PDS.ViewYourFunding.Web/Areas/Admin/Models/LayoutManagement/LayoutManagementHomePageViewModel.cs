using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement
{
    /// <summary>
    /// The view model for the layout management home page.
    /// </summary>
    public class LayoutManagementHomePageViewModel : LayoutManagementPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the page title.
        /// </summary>
        public override string ContentTitle => "Layout Management";

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                LayoutManagementHomeBreadCrumb(true)
            };

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region View Model Properties

        /// <summary>
        /// Gets or sets the layout models.
        /// </summary>
        /// <value>
        /// The layout models.
        /// </value>
        public IReadOnlyList<LayoutUiModel> LayoutModels { get; set; }

        /// <summary>
        /// Gets or sets the pagination details.
        /// </summary>
        /// <value>
        /// The pagination details.
        /// </value>
        public Pagination Pagination { get; set; }

        /// <summary>
        /// Gets or sets the filter Type.
        /// </summary>
        /// <value>
        /// The filter type.
        /// </value>
        public LayoutFilter LayoutFilter { get; set; }

        #endregion
    }
}