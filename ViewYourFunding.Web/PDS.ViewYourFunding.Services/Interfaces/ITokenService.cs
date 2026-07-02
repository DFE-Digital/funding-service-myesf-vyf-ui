using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    ///  The OAuth Token service that handles the authorization server end point methods.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <returns>The access token.</returns>
        Task<string> GetAccessToken();
    }
}