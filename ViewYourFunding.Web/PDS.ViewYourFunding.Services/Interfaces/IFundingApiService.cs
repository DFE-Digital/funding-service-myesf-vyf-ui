using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.RequestObjects;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// Provide mechanism to fetch funding data from an api.
    /// </summary>
    public interface IFundingApiService
    {
        /// <summary>
        /// An api service to fetch funding.
        /// </summary>
        /// <param name="requestObj">The options to filter by - including funding streams and periods.</param>
        /// <returns>A funding collection.</returns>
        Task<IFundingApiSearchResponseFunding> SearchFunding(FundingApiSearchRequestObject requestObj);

        /// <summary>
        /// Search for local authorities.
        /// </summary>
        /// <param name="request">A request object containing the search parameters.</param>
        /// <returns>A response object containing the local authorities matching the search parameters.</returns>
        Task<FundingApiSearchLocalAuthoritiesResponse> SearchLocalAuthorities(FundingApiSearchLocalAuthoritiesRequest request);

        /// <summary>
        /// An HTTP api service to fetch provider funding.
        /// </summary>
        /// <param name="request">A request object containing the search parameters.</param>
        /// <param name="getLatest">Whether to get the latest funding only.</param>
        /// <returns>A provider funding collection.</returns>
        Task<IFundingApiSearchResponseProviderFunding> SearchProviderFunding(FundingApiSearchRequestObject request, bool getLatest = false);

        /// <summary>
        /// An HTTP api service to fetch funding.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>A matched funding.</returns>
        Task<IFundingApiSearchFunding> GetFunding(string id);

        /// <summary>
        /// An HTTP api service to fetch provider funding.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>A matched provider funding.</returns>
        Task<IFundingApiSearchProviderFunding> GetProviderFunding(string id);

        /// <summary>
        /// An HTTP api service to check if user has visited an funding or not.
        /// </summary>
        /// <param name="userId">The user id to lookup.</param>
        /// <param name="fundingId">The funding id to lookup.</param>
        /// <returns>True if user has viewed the funding else false.</returns>
        Task<bool> HasUserVisitedFunding(string userId, string fundingId);

        /// <summary>
        /// An HTTP api service to add user funding view detail.
        /// </summary>
        /// <param name="request">The reuest object containing user and funding detail.</param>
        /// <returns>The asynchronous task.</returns>
        Task AddUserFundingViewDetail(AddUserFundingViewRequest request);

        /// <summary>
        /// An HTTP api service to get a response representing the number of unread new or updated fundings for user.
        /// </summary>
        /// <param name="userId">The user id to lookup.</param>
        /// <param name="fundingVersionDetails">The funding ids and statement versions to lookup.</param>
        /// <returns>Returns response with required counts.</returns>
        Task<UserFundingViewCountResponse> GetUserFundingViewCount(string userId, List<FundingVersionDetail> fundingVersionDetails);
    }
}