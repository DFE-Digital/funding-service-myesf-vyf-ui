namespace PDS.VYF.Services.Abstracts.AppServices
{
    using PDS.VYF.Services.Models.ApiModels;

    /// <summary>
    /// Represents the interface for logged-in API services.
    /// </summary>
    public interface ILoggedInApiServices
    {
        /// <summary>
        /// Retrieves information for a logged-in user.
        /// </summary>
        /// <param name="ukprn">The UKPRN.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="scheme">The scheme.</param>
        /// <param name="host">The host.</param>
        /// <returns>The logged-in information.</returns>
        Task<LoggedInInfo> GetInfoForLoggedIn(string ukprn, string principal, string scheme, string host);
    }
}
