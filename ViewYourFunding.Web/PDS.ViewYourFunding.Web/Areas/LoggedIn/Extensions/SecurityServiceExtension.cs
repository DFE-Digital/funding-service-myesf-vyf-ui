#nullable enable

using Ardalis.GuardClauses;
using Pds.Core.Common.Identity.Models;
using Pds.Core.Identity.Claims.Interfaces;
using PDS.VYF.Services.Extensions.Core;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Extensions
{
    public static class SecurityServiceExtension
    {
        private const string DefaultOrgName = "--";
        private const bool LookupOrganisationName = false;

        public static async Task<(string currentUserUkprn, User user)> GetUserFromClaims(
            this IClaimsBasedIdentityService securityService,
            ClaimsPrincipal claimsPrincipal,
            Action? actionBeforeGetProviderName = null)
        {
            var user = await securityService.GetUserFromClaims(claimsPrincipal);

            var currentUserUKPRN = user.Ukprn.ToString() ?? string.Empty;

            Guard.Against.ValidUkrpn(currentUserUKPRN);

            actionBeforeGetProviderName?.Invoke();

            return (currentUserUKPRN, user);
        }
    }
}
