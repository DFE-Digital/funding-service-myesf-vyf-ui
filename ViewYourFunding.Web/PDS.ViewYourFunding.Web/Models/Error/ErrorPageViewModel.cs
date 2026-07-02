using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using PDS.ViewYourFunding.Web.Models.Shared;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Models.Error
{
    /// <summary>
    /// The error page view Model.
    /// </summary>
    /// <seealso cref="BasePageViewModel" />
    public class ErrorPageViewModel : BaseViewYourFundingPageViewModel
    {
        public virtual string Title =>
            StatusCode switch
            {
                403 => "Access Denied",
                _ => "Error",
            };

        /// <inheritdoc />
        public override string BrowserTitle => Title;

        /// <inheritdoc />
        public override string HeaderTitle => LoggedInConstants.HeaderTitle_StandardMYESFHeader;

        /// <inheritdoc />
        public override string HeaderLink => "/";

        public override string DfeSignInLink => $"{DfeSignInUrl}/auth/cb";

        public override string DfeSignInOrganisationLink => $"{DfeSignInUrl}/organisations";

        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems => new List<BreadCrumbViewModel>
        {
            MYESFHomeBreadCrumb
        };

        /// <summary>
        /// Gets or sets the status code for the error page.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the partial view name to be used in the error page.
        /// </summary>
        public string ErrorPartialViewName { get; set; }

        /// <summary>
        /// Gets or sets the DfE Sign In base url.
        /// </summary>
        public string DfeSignInUrl { get; set; }

        /// <summary>
        /// Gets or sets the list of required user roles for accessing the denied area of service.
        /// </summary>
        public List<string> RequiredUserRolesForUrl { get; set; }
    }
}
