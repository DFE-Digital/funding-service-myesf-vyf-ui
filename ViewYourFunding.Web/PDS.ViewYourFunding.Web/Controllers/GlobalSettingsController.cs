using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Attributes;
using System;
using System.Threading.Tasks;
using GlobalSetting = PDS.ViewYourFunding.Web.Models.GlobalSetting.GlobalSetting;

namespace PDS.ViewYourFunding.Web.Controllers
{
    /// <summary>
    /// Get global setting for View Your Funding.
    /// </summary>
    [ApiController]
    [TokenAuthorize]
    [Route("api/[controller]")]
    public partial class GlobalSettingsController : BaseFundingController
    {
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IMapper _mapper;
        private readonly IFundingApiService _fundingApiService;
        private readonly ILoggerAdapter<GlobalSettingsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalSettingsController"/> class.
        /// </summary>
        /// <param name="globalSettingService">The global setting service to use.</param>
        /// <param name="mapper">Auto-mapper.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="settingsService">The settings service.</param>
        /// <param name="applicationConfigurationOptions">The config.</param>
        /// <param name="fundingApiService">The funding api service.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="fundingViewService">The funding view service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="httpApiService">The HttP API service.</param>
        public GlobalSettingsController(
            IGlobalSettingService globalSettingService,
            IMapper mapper,
            IClaimsBasedIdentityService securityService,
            IUserJourneyService settingsService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            IFundingApiService fundingApiService,
            ICacheService cacheService,
            IFundingViewService fundingViewService,
            ILoggerAdapter<GlobalSettingsController> logger,
            IHttpApiService httpApiService)
            : base(securityService, applicationConfigurationOptions, settingsService, mapper, fundingApiService, cacheService, fundingViewService, globalSettingService, httpApiService)
        {
            _globalSettingService = globalSettingService;
            _fundingApiService = fundingApiService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns string of TRUE if external VYF area is available.
        /// </summary>
        /// <returns>TRUE if external VYF area is available.</returns>
        [HttpGet, Produces("text/plain")]
        [Route("IsExternalViewAvailable")]
        [TokenAuthorize]
        public async Task<string> GetExternalViewYourFundingSetting()
        {
            var result = await GetGlobalSetting(GlobalSettingTypeConstants.DisplayViewYourFundingExternalViewTypeId);
            return result ?? $"Setting type {GlobalSettingTypeConstants.DisplayViewYourFundingExternalViewTypeId} not found.";
        }

        /// <summary>
        /// Returns external view URI.
        /// </summary>
        /// <returns>External view URI.</returns>
        [HttpGet, Produces("text/plain")]
        [Route("GetExternalViewUrl")]
        [TokenAuthorize]
        public async Task<string> GetExternalViewUrlSetting()
        {
            return await BuildPath(await GetGlobalSetting(GlobalSettingTypeConstants.UrlForExternalViewTypeId));
        }

        private async Task<string> BuildPath(string result)
        {
            const string SchemeHostSeperator = "://";

            if (result?.Contains(SchemeHostSeperator) == true)
            {
                return result;
            }

            var publicFacingUrlLeftPart = await GetGlobalSetting(GlobalSettingTypeConstants.PublicFacingUrlLeftPart);

            if (string.IsNullOrEmpty(publicFacingUrlLeftPart) && Request == null)
            {
                return result;
            }

            var root = !string.IsNullOrEmpty(publicFacingUrlLeftPart) ?
                publicFacingUrlLeftPart : $"{Request?.Scheme}{SchemeHostSeperator}{Request?.Host}";

            var uriBuilder = new UriBuilder(root)
            {
                Path = result
            };

            return uriBuilder.ToString();
        }

        /// <summary>
        /// Return global setting for specified Id.
        /// </summary>
        /// <param name="settingTypeId">Setting typeId.</param>
        /// <returns>Return ActionResult and setting.</returns>
        private async Task<string> GetGlobalSetting(int settingTypeId)
        {
            var result = await _globalSettingService.GetFirstOrDefault(settingTypeId);
            var setting = _mapper.Map<GlobalSetting>(result);

            return setting?.Value;
        }
    }
}