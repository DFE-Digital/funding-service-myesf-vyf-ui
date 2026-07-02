using Microsoft.AspNetCore.Mvc;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Common.Identity.Models;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Web.Models;
using PDS.ViewYourFunding.Web.Models.Shared;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Controllers
{
    /// <summary>
    /// A base MVC controller.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// The security service.
        /// </summary>
        protected readonly IClaimsBasedIdentityService SecurityService;

        private readonly bool _isProductionEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="securityService">The security service to use.</param>
        /// <param name="isProductionEnvironment">Whether or not it is the production environment.</param>
        public BaseController(IClaimsBasedIdentityService securityService, bool isProductionEnvironment)
        {
            SecurityService = securityService;
            _isProductionEnvironment = isProductionEnvironment;
        }

        /// <summary>
        /// Populates some base properties of the given view model.
        /// </summary>
        /// <typeparam name="T">The type of the view model to return.</typeparam>
        /// <param name="currentUser">The current user (optional).</param>
        /// <param name="currentUserPassed">Has the current user been passed in (optional).</param>
        /// <returns>A new view model of the given type.</returns>
        protected async Task<T> GetBasePageViewModel<T>(
            User currentUser = null,
            bool currentUserPassed = false)
            where T : BaseViewYourFundingPageViewModel, new()
        {
            if (!currentUserPassed)
            {
                currentUser = await SecurityService.GetUserFromClaims(User);
            }

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

            return new T
            {
                CurrentUser = currentUserViewModel,
                IsProductionEnvironment = _isProductionEnvironment,
                UserIsAdmin = (currentUser != null)
                                && (currentUser.Roles?.Contains(nameof(UserRole.SfsAdmin)) == true)
            };
        }

        /// <summary>
        /// Gets a value indicating whether whether or not preview mode is enabled in the current context.
        /// </summary>
        /// <param name="currentUser">The current user (optional).</param>
        /// <param name="currentUserPassed">Has the current user been passed in (optional).</param>
        /// <returns>True if preview is enabled.</returns>
        protected async Task<bool> PreviewModeEnabled(
            User currentUser = null,
            bool currentUserPassed = false)
        {
            if (!currentUserPassed)
            {
                currentUser = await SecurityService.GetUserFromClaims(User);
            }

            return currentUser?.Roles?.Any(r => r.Equals(nameof(UserRole.SfsAdmin), StringComparison.OrdinalIgnoreCase)) == true;
        }
    }
}