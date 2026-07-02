using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Common.Identity.Models;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PublicationStatus = PDS.ViewYourFunding.Services.Enums.PublicationStatus;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The Admin actions controller.
    /// </summary>
    /// <seealso cref="BaseController" />
    public class AdminActionsBaseController : BaseController
    {
        /// <summary>
        /// The admin settings service.
        /// </summary>
        private readonly IAdminSettingsService _adminSettingsService;

        /// <summary>
        /// The HTTP client.
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// The back ground task queue service.
        /// </summary>
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILoggerAdapter<AdminActionsBaseController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminActionsBaseController"/> class.
        /// </summary>
        /// <param name="securityService">The security service.</param>
        /// <param name="applicationConfigurationOptions">The application configuration options.</param>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="adminSettingsService">The admin settings service.</param>
        /// <param name="backgroundTaskQueue">The background task queue.</param>
        /// <param name="logger">The logger.</param>
        public AdminActionsBaseController(
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            IHttpClientFactory httpClientFactory,
            IAdminSettingsService adminSettingsService,
            IBackgroundTaskQueue backgroundTaskQueue,
            ILoggerAdapter<AdminActionsBaseController> logger)
            : base(securityService, applicationConfigurationOptions.Value.IsProductionEnvironment)
        {
            _adminSettingsService = adminSettingsService;
            _httpClient = httpClientFactory.CreateClient();
            _backgroundTaskQueue = backgroundTaskQueue;
            _logger = logger;
        }

        protected (DateTime startDateTime, DateTime? enfDateTime) FormatDateTimes(DateTime startDateTime, DateTime? endDateTime)
        {
            DateTime startLocalTime;
            DateTime? endLocalTime = null;

            try
            {
                var britishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                startLocalTime = TimeZoneInfo.ConvertTimeFromUtc(startDateTime, britishTimeZone);
                if (endDateTime != null)
                {
                    endLocalTime = TimeZoneInfo.ConvertTimeFromUtc((DateTime)endDateTime, britishTimeZone);
                }
            }
            catch
            {
                var localTime = DateTime.Now;
                var localTimeUtc = DateTime.Now.ToUniversalTime();

                var offSet = localTime - localTimeUtc;
                startLocalTime = startDateTime + offSet;
                if (endDateTime != null)
                {
                    endLocalTime = endDateTime + offSet;
                }
            }

            return (startLocalTime, endLocalTime);
        }

        protected async Task<IEnumerable<SelectListItem>> GetFundingStreamCodes(bool allFundingStreams = false)
        {
            var fundingStreams = allFundingStreams ? await GetAllFundingStreams() : await GetRelevantFundingStreams(SettingName.PDFDocumentGenerationEnabled);

            var result = fundingStreams
                .Where(fs => fs.Publications.Any(pub => pub.Status == PublicationStatus.Published))
                .Select(fs => new SelectListItem(fs.FundingStreamCode, fs.FundingStreamCode))
                .OrderBy(fs => fs.Text)
                .Distinct();

            return result;
        }

        protected async Task<IEnumerable<SelectListItem>> GetFundingStreamNames(bool allFundingStreams = false)
        {
            var fundingStreams = allFundingStreams ? await GetAllFundingStreams() : await GetRelevantFundingStreams(SettingName.PDFDocumentGenerationEnabled);

            var result = fundingStreams
                .Where(fs => fs.Publications.Any(pub => pub.Status == PublicationStatus.Published))
                .Select(fs => new SelectListItem(fs.FundingStreamBusinessAllocationName, fs.FundingStreamCode))
                .OrderBy(fs => fs.Text)
                .Distinct();

            return result;
        }

        protected async Task<IEnumerable<Services.Models.FundingStream>> GetRelevantFundingStreams(string settingName)
        {
            var fundingStreams = await _adminSettingsService.GetAllFundingStreams(FetchData.Publications, FetchData.SettingValues_Setting);

            return fundingStreams.Where(fs => fs.SettingValues.FirstOrDefault(sv => sv.Setting.SettingName == settingName)?.Value?.ToLower() == "true");
        }

        protected async Task<IEnumerable<Services.Models.FundingStream>> GetAllFundingStreams()
        {
            return await _adminSettingsService.GetAllFundingStreams(FetchData.Publications);
        }

        protected async Task<User> GetUserAsync()
        {
            var userDetails = await SecurityService.GetUserFromClaims(User);
            return userDetails;
        }

        protected bool QueueBackGroundTask(string requestUrl)
        {
            _backgroundTaskQueue.QueueTask(async token =>
            {
                await CallUrl(requestUrl);
            });

            return true;
        }

        protected async Task CallUrl(string requestUrl)
        {
            _logger.LogDebug($"About to call the http URL for {requestUrl}.");

            try
            {
                var result = await _httpClient.GetAsync(requestUrl);

                if (!result.IsSuccessStatusCode)
                {
                    if (result.Content == null)
                    {
                        throw new RequestException($"Error was returned - status code {result.StatusCode}");
                    }

                    var content = await result.Content.ReadAsStringAsync();
                    throw new RequestException($"Error was returned - status code {result.StatusCode} - message {content}");
                }

                _logger.LogDebug($"Completed call to the http URL {requestUrl}.with success status code = {result.IsSuccessStatusCode}");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Error on calling {requestUrl}", exception);
            }
        }

        protected async Task<IEnumerable<SelectListItem>> GetFundingStreamSelectItemList()
        {
            var allfundingStreams = await GetFundingStreamNames(true);
            var roles = (await GetUserAsync()).Roles;

            if (roles.Contains(nameof(UserRole.SfsAdmin)))
            {
                return allfundingStreams;
            }

            return allfundingStreams.Where(fundingStream => roles.Any(role => role.Contains(fundingStream.Value)));
        }
    }
}