using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPaymentType
{
    /// <summary>
    /// The next payment type index class.
    /// </summary>
    /// <seealso cref="NextPaymentTypePageViewModel" />
    public class NextPaymentTypesIndexViewModel : NextPaymentTypePageViewModel
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
                NextPaymentTypesBreadCrumb(FundingStreamId, FundingStreamName, true)
            };

        /// <summary>
        /// Gets a value indicating whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Model Properties

        /// <summary>
        /// Gets or sets the list of next payments.
        /// </summary>
        public IEnumerable<NextPaymentType> NextPaymentTypes { get; set; }

        /// <summary>
        /// Gets or sets the funding stream identifier.
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

        #endregion
    }
}
