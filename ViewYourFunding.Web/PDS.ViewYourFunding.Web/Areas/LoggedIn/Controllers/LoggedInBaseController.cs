#nullable enable

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Models;
using Pds.Core.Identity.Claims.Interfaces;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Helpers;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.ViewModels;
using PDS.ViewYourFunding.Web.Controllers;
using System;
using System.Net;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Controllers
{
    public abstract class LoggedInBaseController : BaseController
    {
        public User? UserFromClaims { get; set; }

        public string? CurrentUserUKPRN { get; set; }

        public bool HasUserLoggedInAsParent { get; set; }

        protected readonly ApplicationConfiguration applicationConfiguration;

        protected LoggedInBaseController(
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions)
            : base(securityService, applicationConfigurationOptions?.Value?.IsProductionEnvironment ?? false)
        {
            this.applicationConfiguration = applicationConfigurationOptions?.Value ?? throw new ArgumentNullException(nameof(applicationConfigurationOptions));
        }

        protected T GetViewModel<T>(bool viaChoicePage)
            where T : LoggedInBasePageViewModel, new()
        {
            return UserFromClaims.GetBasePageViewModel<T>(applicationConfiguration, viaChoicePage);
        }

        protected ViewResult BaseView(LoggedInBasePageViewModel viewModel)
        {
            Response.StatusCode = viewModel switch
            {
                _ when !viewModel.IsValidUrl => (int)HttpStatusCode.NotFound,
                _ when !viewModel.HasUserHaveRightAccess => (int)HttpStatusCode.Unauthorized,
                _ when !viewModel.HasFundingDataExists => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.OK,
            };

            if (!viewModel.IsValidUrl || !viewModel.HasUserHaveRightAccess)
            {
                var errorModel = UserFromClaims.GetBasePageViewModel<LoggedInErrorPageViewModel>(applicationConfiguration, false);
                errorModel.ErrorTitle = viewModel switch
                {
                    _ when !viewModel.IsValidUrl => "Page not found",
                    _ => "You do not have permission"
                };

                var errorViewName = viewModel switch
                {
                    _ when !viewModel.IsValidUrl => "../Error/PageNotFound",
                    _ => "../Error/UnauthorisedAccess"
                };

                return View(errorViewName, errorModel);
            }
            else if (!viewModel.HasFundingDataExists)
            {
                return View("../Error/FundingNotExists", viewModel);
            }

            return View(viewModel);
        }
    }
}
