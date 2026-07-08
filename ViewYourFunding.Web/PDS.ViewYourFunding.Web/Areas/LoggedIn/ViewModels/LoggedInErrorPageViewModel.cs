using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.ViewModels
{
    public class LoggedInErrorPageViewModel : LoggedInBasePageViewModel
    {
        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems => new List<BreadCrumbViewModel>() { LoggedInHomePageBreadCrumb() };

        /// <summary>
        /// Gets a value indicating whether or not to use the two-thirds column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        public override bool ShowBetaTag => true;

        public override string HeaderLink => "/";

        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle => ErrorTitle ?? string.Empty;

        public string ErrorTitle { get; set; }
    }
}
