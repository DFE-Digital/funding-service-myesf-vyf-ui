using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting
{
    /// <summary>
    /// The view model for the funding stream settings list page.
    /// </summary>
    public class FundingStreamSettingsViewModel : FundingStreamSettingsPageViewModel
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
                SettingsFundingStreamBreadCrumb(FundingStream.FundingStreamId, FundingStream.FundingStreamName, true)
            };

        /// <summary>
        /// Gets a value indicating whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Model Properties

        /// <summary>
        /// Gets or sets the list of all settings for the currently selected funding stream.
        /// </summary>
        public IReadOnlyList<FundingStreamSettingViewModel> FundingStreamSettings { get; set; }

        /// <summary>
        /// Gets or sets the funding publications.
        /// </summary>
        /// <value>The funding publications.</value>
        public IReadOnlyList<PublicationViewModel> FundingPublications { get; set; } = new List<PublicationViewModel>();

        /// <summary>
        /// Gets or sets the funding stream.
        /// </summary>
        /// <value>The funding stream.</value>
        public FundingStream FundingStream { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can add new settings.
        /// </summary>
        /// <value>
        ///   True if this instance can add new settings.
        /// </value>
        public bool CanAddNewSettings { get; set; }

        #endregion
    }
}