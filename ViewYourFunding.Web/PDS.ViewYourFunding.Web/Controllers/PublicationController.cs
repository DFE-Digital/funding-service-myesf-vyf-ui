using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Identity.Claims.Interfaces;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Attributes;
using PDS.ViewYourFunding.Web.Constants;
using PDS.ViewYourFunding.Web.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Controllers
{
    /// <summary>
    /// Get Publication related information for View Your Funding.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [TokenAuthorize]
    public class PublicationController : BaseFundingController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationController"/> class.
        /// </summary>
        /// <param name="globalSettingService">The global settings service to use.</param>
        /// <param name="mapper">Auto-mapper.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="settingsService">The settings service.</param>
        /// <param name="applicationConfigurationOptions">The config.</param>
        /// <param name="fundingApiService">The funding api service.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="fundingViewService">The funding view service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="httpApiService">The HttP API service.</param>
        public PublicationController(
            IGlobalSettingService globalSettingService,
            IMapper mapper,
            IClaimsBasedIdentityService securityService,
            IUserJourneyService settingsService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            IFundingApiService fundingApiService,
            ICacheService cacheService,
            IFundingViewService fundingViewService,
            IHttpApiService httpApiService)
            : base(securityService, applicationConfigurationOptions, settingsService, mapper, fundingApiService, cacheService, fundingViewService, globalSettingService, httpApiService)
        {
        }

        /// <summary>
        /// Returns the latest published date for a funding stream.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <returns>The latest publication date if available. If not available default date as 1-Jan-1900.</returns>
        [HttpGet]
        [Route(FundingConstants.Route_GetLatestFundingStreamPublishedDate, Name = FundingConstants.RouteName_GetLatestFundingStreamPublishedDate)]
        public async Task<DateTime> GetLatestFundingStreamPublishedDate(string fundingStreamCode, string fundingPeriodCode)
        {
            var activeFundingStreams = await this.GetActiveFundingStreams();
            var fundingStream = activeFundingStreams?.FirstOrDefault(a => string.Equals(a.FundingStreamCode, fundingStreamCode, StringComparison.OrdinalIgnoreCase));
            var latestPublication = fundingStream?.GetLatestPublication(false, null, fundingPeriodCode, null);
            return latestPublication?.PublishedDate ?? new DateTime(1900, 1, 1);
        }
    }
}