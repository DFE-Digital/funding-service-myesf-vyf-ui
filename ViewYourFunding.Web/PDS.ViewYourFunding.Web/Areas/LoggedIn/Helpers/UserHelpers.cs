using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Common.Identity.Models;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Web.Models;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Extensions;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.ViewModels;
using PDS.VYF.Services.Abstracts.AppServices;
using System.Globalization;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Helpers
{
    public static class UserHelpers
    {
        public static async Task<User> GetUserFromClaims(HttpContext httpContext)
        {
            var securityService = httpContext.RequestServices.GetService<IClaimsBasedIdentityService>();
            var childOrParentNameServices = httpContext.RequestServices.GetService<IChildOrParentNameServices>();

            var userFromClaims = await securityService.GetUserFromClaims(httpContext.User);

            var currentUserUKPRN = userFromClaims.Ukprn?.ToString() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(currentUserUKPRN))
            {
                userFromClaims.ProviderName = await childOrParentNameServices.GetParentOrChildName(currentUserUKPRN);
            }

            return userFromClaims;
        }

        public static T GetBasePageViewModel<T>(this User currentUser, HttpContext httpContext, bool viaChoicePage)
            where T : LoggedInBasePageViewModel, new()
        {
            var applicationConfigurationOptions = httpContext.RequestServices.GetService<IOptions<ApplicationConfiguration>>();
            return GetBasePageViewModel<T>(currentUser, applicationConfigurationOptions?.Value, viaChoicePage);
        }

        public static T GetBasePageViewModel<T>(this User currentUser, ApplicationConfiguration applicationConfiguration, bool viaChoicePage)
            where T : LoggedInBasePageViewModel, new()
        {
            CurrentUserViewModel currentUserViewModel = null;

            if (currentUser != null)
            {
                var providerName =
                    string.IsNullOrWhiteSpace(currentUser.ProviderName) ? null : CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(currentUser.ProviderName.ToLower());

                currentUserViewModel = new CurrentUserViewModel
                {
                    IsLoggedIn = currentUser.IsAuthenticated,
                    IsExternalUser = currentUser.IsExternalUser,
                    Ukprn = currentUser.Ukprn,
                    ProviderName = providerName,
                    CanViewAsOrganisation = currentUser.Roles?.Contains(
                        UserRole.ViewAsProvider.ToString()) == true ||
                        currentUser.Roles?.Contains(nameof(UserRole.SfsAdmin)) == true,
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    FullName = currentUser.FullName
                };
            }

            var isProductionEnvironment = applicationConfiguration?.IsProductionEnvironment ?? true;

            var viewModel = new T
            {
                CurrentUser = currentUserViewModel,
                IsProductionEnvironment = isProductionEnvironment,
                UserIsAdmin = (currentUser != null)
                                && (currentUser.Roles?.Contains(nameof(UserRole.SfsAdmin)) == true)
            };

            viewModel.AddLinks(applicationConfiguration, viaChoicePage);

            return viewModel;
        }
    }
}
