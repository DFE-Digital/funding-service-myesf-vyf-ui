using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// The funding UI (spreadsheet and view-data) model details service.
    /// </summary>
    /// <seealso cref="IFundingUiModelDetailsService" />
    public class FundingUiModelDetailsService : IFundingUiModelDetailsService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILoggerAdapter<FundingUiModelDetailsService> _logger;

        /// <summary>
        /// The HTTP client.
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// The configuration service.
        /// </summary>
        private readonly ApplicationConfiguration _applicationConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingUiModelDetailsService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="applicationConfigurationOptions">The application configuration options.</param>
        public FundingUiModelDetailsService(
            ILoggerAdapter<FundingUiModelDetailsService> logger,
            HttpClient httpClient,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions)
        {
            _logger = logger;
            _httpClient = httpClient;
            _applicationConfiguration = applicationConfigurationOptions.Value;
        }

        /// <inheritdoc/>
        public async Task<MaximumUiSpreadsheetVersion> GetMaximumUiAndSpreadsheetVersionNumbersByUrl(string versionNumberUrl)
        {
            try
            {
                var requestUrl = $"{_applicationConfiguration.ViewYourFundingApiBaseAddress}{versionNumberUrl}";

                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var result = await _httpClient.GetAsync(requestUrl);

                if (result.IsSuccessStatusCode && result.Content != null)
                {
                    var maxUiSpreadsheetVersionNo = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MaximumUiSpreadsheetVersion>(maxUiSpreadsheetVersionNo);
                }

                return null;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetMaximumUiAndSpreadsheetVersionNumbersByUrl has encountered an error.");
                return null;
            }
        }
    }
}