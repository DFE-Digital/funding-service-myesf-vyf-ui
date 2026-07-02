using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement
{
    /// <summary>
    /// The view model for the layout preview page.
    /// </summary>
    public class PreviewLayoutViewModel : LayoutManagementPageViewModel
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
                PreviewLayoutBreadCrumb(true)
            };

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the funding view scope.
        /// </summary>
        /// <value>
        /// The funding view scope.
        /// </value>
        public FundingViewScope FundingViewScope { get; set; }

        /// <summary>
        /// Gets or sets the funding view type.
        /// </summary>
        /// <value>
        /// The funding view type.
        /// </value>
        public FundingViewType FundingViewType { get; set; }

        /// <summary>
        /// Gets or sets the layout identifier.
        /// </summary>
        /// <value>
        /// The layout identifier.
        /// </value>
        public Guid LayoutId { get; set; }

        /// <summary>
        /// Gets or sets the local authority code.
        /// </summary>
        /// <value>
        /// The local authority code.
        /// </value>
        [Required]
        [DisplayName("Local Authority Code")]
        [RegularExpression("^[0-9]{3}$", ErrorMessage = "Please enter a valid Local Authority Code")]
        public int LocalAuthorityCode { get; set; }

        /// <summary>
        /// Gets or sets the provider uk PRN.
        /// </summary>
        /// <value>
        /// The provider uk PRN.
        /// </value>
        [Required]
        [DisplayName("Organisation UKPRN")]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "Please enter a valid Provider UKPRN")]
        public int OrganisationUkprn { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is an organisation.
        /// </summary>
        /// <value>
        ///  True if this instance is an organisation.
        /// </value>
        public bool IsOrganisationViewScope => LayoutHelper.OrganisationFundingViewScopes.Contains(FundingViewScope);

        #endregion
    }
}