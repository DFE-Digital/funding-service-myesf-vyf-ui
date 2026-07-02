using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// The OAuth token service that handles the authorization server end point methods.
    /// </summary>
    /// <seealso cref="ITokenService" />
    public class TokenService : ITokenService
    {
        /// <summary>
        /// The authentication.
        /// </summary>
        private readonly Authentication _authentication;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="authenticationOptions">The authentication options.</param>
        public TokenService(IOptions<Authentication> authenticationOptions)
        {
            _authentication = authenticationOptions.Value;
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <returns>The access token.</returns>
        /// <exception cref="Exception">Access token cannot be acquired.</exception>
        public async Task<string> GetAccessToken()
        {
            var authContext = new AuthenticationContext($"{_authentication.Instance}{_authentication.TenantId}");
            var clientCredential = new ClientCredential(_authentication.ClientId, _authentication.ClientSecret);

            var token = await authContext.AcquireTokenAsync(_authentication.AppIdUrl, clientCredential);

            if (token == null)
            {
                throw new Exception("Access token cannot be acquired");
            }

            return token.AccessToken;
        }
    }
}