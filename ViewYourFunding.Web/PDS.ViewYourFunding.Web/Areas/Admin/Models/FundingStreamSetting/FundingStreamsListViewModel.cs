using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;
using Model = PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting
{
    /// <summary>
    /// The view model for the funding streams list page.
    /// </summary>
    public class FundingStreamsListViewModel : FundingStreamSettingsPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the page title.
        /// </summary>
        public override string ContentTitle => "Funding stream settings";

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                SettingsListBreadCrumb(true)
            };

        #endregion

        #region Model Properties

        /// <summary>
        /// Gets or sets the list of all funding streams.
        /// </summary>
        public IReadOnlyList<Model.FundingStream> FundingStreams { get; set; }

        #endregion
    }
}