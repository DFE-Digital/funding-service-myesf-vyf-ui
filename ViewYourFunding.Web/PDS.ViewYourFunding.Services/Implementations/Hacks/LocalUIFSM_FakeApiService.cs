using Newtonsoft.Json;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.RequestObjects;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations.Hacks
{
    /// <summary>
    /// A short term use implementation of funding service to get mock 1416 data.
    /// </summary>
    public class LocalUIFSM_FakeApiService : BaseFakeApiService, IFundingApiService
    {
        /// <summary>
        /// Gets or sets the funding api service to use to fetch data from.
        /// </summary>
        private readonly IFundingApiService _fundingApiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalUIFSM_FakeApiService"/> class.
        /// </summary>
        /// <param name="httpApiService">The http api service.</param>
        /// <param name="fundingApiService">The funding api service to use to fetch data from.</param>
        public LocalUIFSM_FakeApiService(IHttpApiService httpApiService, IFundingApiService fundingApiService) : base(httpApiService)
        {
            _fundingApiService = fundingApiService;
        }

        /// <inheritdoc/>
        public async Task<IFundingApiSearchFunding> GetFunding(string id)
        {
            return await _fundingApiService.GetFunding(id);
        }

        /// <inheritdoc/>
        public async Task<IFundingApiSearchProviderFunding> GetProviderFunding(string id)
        {
            return await _fundingApiService.GetProviderFunding(id);
        }

        /// <inheritdoc/>
        public async Task<IFundingApiSearchResponseFunding> SearchFunding(FundingApiSearchRequestObject requestObj)
        {
            requestObj.FundingStreams[0].FundingStreamCode = "PP";
            requestObj.FundingStreams[0].PeriodCodes = new[] { "FY-2223" };
            requestObj.FundingStreams[0].BeforeDateTime = new DateTime(2022, 10, 25);
            var result = await _fundingApiService.SearchFunding(requestObj);

            return result;
        }

        /// <inheritdoc/>
        public Task<FundingApiSearchLocalAuthoritiesResponse> SearchLocalAuthorities(FundingApiSearchLocalAuthoritiesRequest request)
        {
            throw new NotImplementedException(nameof(SearchLocalAuthorities));
        }

        /// <inheritdoc/>
        public async Task<IFundingApiSearchResponseProviderFunding> SearchProviderFunding(FundingApiSearchRequestObject requestObj, bool getLatest = false)
        {
            requestObj.FundingStreams[0].FundingStreamCode = "PP";
            requestObj.FundingStreams[0].PeriodCodes = new[] { "FY-2223" };
            requestObj.FundingStreams[0].BeforeDateTime = new DateTime(2022, 10, 25);
            var result = await _fundingApiService.SearchProviderFunding(requestObj, getLatest: true);
            return result;
        }

        private IFundingApiSearchFunding GetCopy(IFundingApiSearchFunding source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<FundingApiSearchFunding>(serialized);
        }
    }
}
