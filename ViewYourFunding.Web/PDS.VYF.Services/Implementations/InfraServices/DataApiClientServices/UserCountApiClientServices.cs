namespace PDS.VYF.Services.Implementations.InfraServices.DataApiClientServices
{
    using Newtonsoft.Json;
    using PDS.ViewYourFunding.Services.Interfaces;
    using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
    using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
    using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

    /// <summary>
    /// The user count API client services.
    /// </summary>
    /// <seealso cref="PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices.IUserCountApiClientServices" />
    public class UserCountApiClientServices : IUserCountApiClientServices
    {
        private readonly IHttpApiService httpApiService;
        private const string JsonMediaType = "application/json";

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCountApiClientServices" /> class.
        /// </summary>
        /// <param name="httpApiService">The HTTP API service.</param>
        public UserCountApiClientServices(IHttpApiService httpApiService)
        {
            this.httpApiService = httpApiService;
        }

        /// <summary>
        /// Gets the user view count.
        /// </summary>
        /// <param name="userViewCountRequest">The user view count request model.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task<UserViewCountResponse> GetUserViewCount(UserViewCountRequestModel userViewCountRequest)
            => await this.httpApiService.PostRequestToUserFundingView<UserViewCountResponse>($"UserView/GetUserViewCount", JsonConvert.SerializeObject(userViewCountRequest), JsonMediaType);

        /// <summary>
        /// Checks if the user has visited.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="providerFundingId">The provider funding ID.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task<bool> HasUserVisited(string userId, string providerFundingId)
            => await this.httpApiService.GetResponseFromUserFundingView<bool>($"UserView/HasUserVisited/{userId}/{providerFundingId}");

        /// <summary>
        /// Adds user visited information.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="providerFundingId">The provider funding ID.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task<bool> AddUserVisitedInfo(string userId, string providerFundingId)
            => await this.httpApiService.PostRequestToUserFundingView<bool>($"UserView/AddUserVisitedInfo/{userId}/{providerFundingId}", string.Empty, JsonMediaType);
    }
}
