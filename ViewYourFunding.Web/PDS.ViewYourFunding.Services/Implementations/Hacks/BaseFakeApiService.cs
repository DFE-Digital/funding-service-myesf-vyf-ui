using Newtonsoft.Json;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations.Hacks
{
    /// <summary>
    /// The base fake API service.
    /// </summary>
    public class BaseFakeApiService
    {
        private IHttpApiService _httpApiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFakeApiService"/> class.
        /// </summary>
        /// <param name="httpApiService">The Http API service.</param>
        public BaseFakeApiService(IHttpApiService httpApiService)
        {
            this._httpApiService = httpApiService;
        }

        /// <summary>
        /// An HTTP api service to check if user has visited an funding or not.
        /// </summary>
        /// <param name="userId">The user id to lookup.</param>
        /// <param name="fundingId">The funding id to lookup.</param>
        /// <returns>True if user has viewed the funding else false.</returns>
        public Task<bool> HasUserVisitedFunding(string userId, string fundingId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// An HTTP api service to add user funding view detail.
        /// </summary>
        /// <param name="request">The reuest object containing user and funding detail.</param>
        /// <returns>The asynchronous task.</returns>
        public Task AddUserFundingViewDetail(AddUserFundingViewRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// An HTTP api service to get a response representing the number of unread new or updated fundings for user.
        /// </summary>
        /// <param name="userId">The user id to lookup.</param>
        /// <param name="fundingVersionDetails">The funding ids and statement versions to lookup.</param>
        /// <returns>Returns response with required counts.</returns>
        public async Task<UserFundingViewCountResponse> GetUserFundingViewCount(string userId, List<FundingVersionDetail> fundingVersionDetails)
        {
            var request = new UserFundingViewCountRequest { UserId = userId, FundingVersionDetails = fundingVersionDetails };

            var response = await _httpApiService.PostRequestToUserFundingView<UserFundingViewCountResponse>(
                $"user/GetUserFundingViewCount",
                JsonConvert.SerializeObject(request),
                "application/json");

            if (response == null)
            {
                return new UserFundingViewCountResponse();
            }

            return response;
        }
    }
}