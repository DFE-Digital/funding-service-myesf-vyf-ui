namespace PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices
{
    using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
    using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

    /// <summary>
    /// Represents the interface for user count API client services.
    /// </summary>
    public interface IUserCountApiClientServices
    {
        /// <summary>
        /// Adds user visited information.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="providerFundingId">The provider funding ID.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<bool> AddUserVisitedInfo(string userId, string providerFundingId);

        /// <summary>
        /// Gets the user view count.
        /// </summary>
        /// <param name="userViewCountRequest">The user view count request model.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<UserViewCountResponse> GetUserViewCount(UserViewCountRequestModel userViewCountRequest);

        /// <summary>
        /// Checks if the user has visited.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="providerFundingId">The provider funding ID.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<bool> HasUserVisited(string userId, string providerFundingId);
    }
}
