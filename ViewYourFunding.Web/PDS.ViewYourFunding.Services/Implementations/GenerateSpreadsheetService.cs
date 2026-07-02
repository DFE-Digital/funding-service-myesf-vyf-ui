using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// Service for generating spreadsheets.
    /// </summary>
    /// <seealso cref="IGenerateSpreadsheetService" />
    public class GenerateSpreadsheetService : IGenerateSpreadsheetService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILoggerAdapter<GenerateSpreadsheetService> _logger;

        /// <summary>
        /// The HTTP client.
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// The configuration service.
        /// </summary>
        private readonly ApplicationConfiguration _applicationConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateSpreadsheetService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="httpClient">The HTTP client service.</param>
        /// <param name="applicationConfiguration">The configuration service.</param>
        public GenerateSpreadsheetService(ILoggerAdapter<GenerateSpreadsheetService> logger, HttpClient httpClient, ApplicationConfiguration applicationConfiguration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _applicationConfiguration = applicationConfiguration;
        }

        /// <inheritdoc/>
        public async Task<GenerateSpreadsheetResult> GenerateFundingStreamSpreadSheetByUrlAsync(
            string spreadsheetUrl,
            string settingName,
            string fundingStreamName)
        {
            _logger.LogDebug($"About to call the generate spreadsheet for the {settingName} setting for the {fundingStreamName} funding stream url  at {spreadsheetUrl}");

            try
            {
                var baseUrl = _applicationConfiguration.ViewYourFundingApiBaseAddress;

                if (baseUrl.EndsWith("/") && spreadsheetUrl.StartsWith("/"))
                {
                    baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
                }

                var requestUrl = $"{baseUrl}{spreadsheetUrl}";
                var result = await _httpClient.GetAsync(requestUrl);

                if (!result.IsSuccessStatusCode)
                {
                    if (result.Content == null)
                    {
                        throw new Exception($"Error was returned - status code {result.StatusCode}");
                    }

                    var content = await result.Content.ReadAsStringAsync();
                    throw new Exception($"Error was returned - status code {result.StatusCode} - message {content}");
                }

                _logger.LogDebug($"Completed call to the generate spreadsheet for the {settingName} setting for the {fundingStreamName} funding stream url at {requestUrl} with success status code =  {result.IsSuccessStatusCode}");

                return new GenerateSpreadsheetResult { Success = result.IsSuccessStatusCode };
            }
            catch (Exception exception)
            {
                _logger.LogError(null, exception);
                return new GenerateSpreadsheetResult { Success = false, ErrorMessage = exception.Message };
            }
        }
    }
}