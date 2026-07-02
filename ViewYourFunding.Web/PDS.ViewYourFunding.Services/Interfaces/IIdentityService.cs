using PDS.ViewYourFunding.Services.DTOs;
using System.Security.Claims;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// An interface providing access to security functionality.
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Gets the current user of the service.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal to get the user from.</param>
        /// <param name="defaultProviderName">The default provider name to set against the returned user if we cannot find the provider information.</param>
        /// <returns>The current user of the service.</returns>
        User GetCurrentUser(ClaimsPrincipal claimsPrincipal, string defaultProviderName = "--");
    }
}