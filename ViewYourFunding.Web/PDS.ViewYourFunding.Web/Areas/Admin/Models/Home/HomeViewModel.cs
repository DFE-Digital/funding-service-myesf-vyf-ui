using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home
{
    /// <summary>
    /// The Home view model.
    /// </summary>
    /// <seealso cref="HomePageViewModel" />
    public class HomeViewModel : HomePageViewModel
    {
        /// <summary>
        /// Gets override text for the final breadcrumb.
        /// </summary>
        public override string FinalBreadCrumbText => "Home";

        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems => new List<BreadCrumbViewModel>();

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        /// <value>
        /// The tiles.
        /// </value>
        public List<ITile> Tiles { get; set; }
    }
}