using Newtonsoft.Json;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.RequestObjects;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations.Hacks
{
    /// <summary>
    /// A short term use implementation of funding service to get mock 1416 data.
    /// </summary>
    public class LocalPPG_FakeApiService : BaseFakeApiService, IFundingApiService
    {
        /// <summary>
        /// Gets or sets the funding api service to use to fetch data from.
        /// </summary>
        private readonly IFundingApiService _fundingApiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalPPG_FakeApiService"/> class.
        /// </summary>
        /// <param name="httpApiService">The http api service.</param>
        /// <param name="fundingApiService">The funding api service to use to fetch data from.</param>
        public LocalPPG_FakeApiService(IHttpApiService httpApiService, IFundingApiService fundingApiService) : base(httpApiService)
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
            requestObj.FundingStreams[0].FundingStreamCode = "DSG";
            requestObj.FundingStreams[0].PeriodCodes = new[] { "FY-2021" };
            requestObj.FundingStreams[0].BeforeDateTime = new DateTime(2020, 04, 23);
            var result = await _fundingApiService.SearchFunding(requestObj);

            var sample1 = result.Funding.Where(funding => funding.GroupingType == "LocalGovernmentGroup" && funding.GroupName == "Upper Tier Authorities");
            foreach (var funding in sample1)
            {
                funding.GroupingType = "AlternativeProvision";
                funding.GroupName = "Alternative Provision";
                var fundingCopy = GetCopy(funding);
                fundingCopy.GroupingType = "Special";
                fundingCopy.GroupName = "Maintained Special Schools";
                result.Funding.Append(fundingCopy);
            }

            var sample2 = result.Funding.Where(funding => funding.GroupingType == "LocalGovernmentGroup" && funding.GroupName == "Unitary Authorities");
            foreach (var funding in sample2)
            {
                funding.GroupingType = "AcademyAlternativeProvision";
                funding.GroupName = "Alternative Provision Academies";
                var fundingCopy = GetCopy(funding);
                fundingCopy.GroupingType = "PupilReferralUnit";
                fundingCopy.GroupName = "Pupil Referral Units";
                result.Funding.Append(fundingCopy);
            }

            var sample3 = result.Funding.Where(funding => funding.GroupingType == "LocalGovernmentGroup" && funding.GroupName == "Metropolitan_Authorities");
            foreach (var funding in sample3)
            {
                funding.GroupingType = "Mainstream";
                funding.GroupName = "Mainstream Schools";
                var fundingCopy = GetCopy(funding);
                fundingCopy.GroupingType = "SpecialAcademies";
                fundingCopy.GroupName = "Special Academies";
                result.Funding.Append(fundingCopy);
            }


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
            requestObj.FundingStreams[0].FundingStreamCode = "DSG";
            requestObj.FundingStreams[0].PeriodCodes = new[] { "FY-2021" };
            requestObj.FundingStreams[0].BeforeDateTime = new DateTime(2020, 04, 23);
            return await _fundingApiService.SearchProviderFunding(requestObj);
        }

        private IFundingApiSearchFunding GetCopy(IFundingApiSearchFunding source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<FundingApiSearchFunding>(serialized);
        }
    }
}
