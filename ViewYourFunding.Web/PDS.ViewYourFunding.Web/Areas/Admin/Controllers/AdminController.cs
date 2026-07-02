using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Builder;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The Admin home controller for showing available tiles when a user logs in.
    /// </summary>
    /// <seealso cref="BaseController" />
    [Authorize(Policy = nameof(UserRole.SfsAdmin))]
    [Area("Admin")]
    public class AdminController : BaseController
    {
        #region Private Members

        /// <summary>
        /// The tile builder.
        /// </summary>
        private readonly ITileBuilder _tileBuilder;

        /// <summary>
        /// The DfESignIn identity service.
        /// </summary>
        private readonly IClaimsBasedIdentityService _identityService;

        /// <summary>
        /// The feedback link.
        /// </summary>
        private readonly string _feedbackLink;

        /// <summary>
        /// The contact us link.
        /// </summary>
        private readonly string _contactUsLink;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="securityService">The security service.</param>
        /// <param name="tileBuilder">The tile builder.</param>
        /// <param name="configurationService">The configuration service.</param>
        public AdminController(
            IClaimsBasedIdentityService securityService,
            ITileBuilder tileBuilder,
            IOptions<ApplicationConfiguration> configurationService)
            : base(securityService, configurationService.Value.IsProductionEnvironment)
        {
            _tileBuilder = tileBuilder;
            _identityService = securityService;
            _feedbackLink = configurationService.Value.FeedbackLink;
            _contactUsLink = configurationService.Value.ContactUsLink;
        }

        /// <summary>
        /// Homes this instance.
        /// </summary>
        /// <returns>The MVC view result.</returns>
        [Route(ViewYourFundingConstants.Route_AdminHome, Name = ViewYourFundingConstants.RouteName_AdminHome)]
        public async Task<IActionResult> Home()
        {
            var homeViewModel = await GetBasePageViewModel<HomeViewModel>();
            var user = await _identityService.GetUserFromClaims(User);
            homeViewModel.Tiles = _tileBuilder
                .ForUser(user)
                .BuildTiles() as List<ITile>;

            homeViewModel.FeedbackLink = _feedbackLink;
            homeViewModel.ContactUsLink = _contactUsLink;

            return View(homeViewModel);
        }
    }
}