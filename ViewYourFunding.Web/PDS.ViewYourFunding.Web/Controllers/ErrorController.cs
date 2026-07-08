using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Identity.Claims.Interfaces;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Interfaces;
using PDS.ViewYourFunding.Web.Models.Error;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Controllers
{
    /// <summary>
    /// The Error Page controller.
    /// </summary>
    /// <seealso cref="BaseController" />
    public class ErrorController : BaseFundingController
    {
        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly ApplicationConfiguration applicationConfiguration;

        /// <summary>
        /// The user role authorization service.
        /// </summary>
        private readonly IUserRoleAuthorizationService _userRoleAuthorizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorController"/> class.
        /// </summary>
        /// <param name="securityService">The security service to use.</param>
        /// <param name="applicationOptions">The Application options.</param>
        /// <param name="userRoleAuthorizationService">The user role authorization service.</param>
        /// <param name="settingsService">The settings service to use.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="fundingApiService">The API service to use for searching for funding.</param>
        /// <param name="fundingViewService">The funding view service.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="globalSettingService">The global setting service to use.</param>
        public ErrorController(
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationOptions,
            IUserRoleAuthorizationService userRoleAuthorizationService,
            IUserJourneyService settingsService,
            IMapper mapper,
            IFundingApiService fundingApiService,
            IFundingViewService fundingViewService,
            ICacheService cacheService,
            IGlobalSettingService globalSettingService)
            : base(
                securityService,
                applicationOptions,
                settingsService,
                mapper,
                fundingApiService,
                cacheService,
                fundingViewService,
                globalSettingService)
        {
            applicationConfiguration = applicationOptions.Value;
            _userRoleAuthorizationService = userRoleAuthorizationService;
        }

        /// <summary>
        /// The General Error Page action.
        /// </summary>
        /// <returns>The MVC view result.</returns>
        [Route(
            ViewYourFundingConstants.Route_ErrorPage,
            Name = ViewYourFundingConstants.RouteName_ErrorPage)]
        public async Task<IActionResult> Index()
        {
            var userDetails = await GetUserAsync();

            var viewModel = await GetBasePageViewModel<ErrorPageViewModel>(userDetails, true);
            viewModel.StatusCode = 0;
            viewModel.ErrorPartialViewName = "_ErrorHelpDetails";
            viewModel.ContactUsLink = viewModel.CurrentUser.IsExternalUser
                ? applicationConfiguration.ContactUsLink
                : applicationConfiguration.ServiceNowLink;

            return View(viewModel);
        }

        /// <summary>
        /// The Access Denied Page action.
        /// </summary>
        /// <param name="code">The encrypted code of required user roles for denied portion of service.</param>
        /// <returns>The MVC view result.</returns>
        [Route(ViewYourFundingConstants.Route_StatusCode403DSIRequiredUserRoles, Name = ViewYourFundingConstants.RouteName_StatusCode403DSIRequiredUserRoles)]
        public async Task<IActionResult> StatusCode403_DSIRequiredUserRoles(string code = null)
        {
            var userDetails = await GetUserAsync();

            var viewModel = await GetBasePageViewModel<ErrorPageViewModel>(userDetails, true);
            viewModel.StatusCode = 403;
            viewModel.ErrorPartialViewName = "StatusCodes/_403DSIRequiredUserRoles";
            viewModel.DfeSignInUrl = applicationConfiguration.DfeSignInUrl;
            viewModel.RequiredUserRolesForUrl = code != null ? _userRoleAuthorizationService.DecryptUrlParameterToRequiredUserRoleNames(code, viewModel.CurrentUser.IsExternalUser) : null;

            return View("Index", viewModel);
        }
    }
}