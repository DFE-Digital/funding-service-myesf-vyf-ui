using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Cache;
using PDS.ViewYourFunding.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// Service to talk to the Funding API endpoint.
    /// </summary>
    public class FundingApiHttpClient : IHttpApiService
    {
        /// <summary>
        /// The bearer header name.
        /// </summary>
        private const string BearerHeaderName = "Bearer";

        /// <summary>
        /// The application json media type.
        /// </summary>
        private const string ApplicationJsonMediaType = "application/json";

        /// <summary>
        /// The accept header name.
        /// </summary>
        private const string AcceptHeaderName = "Accept";

        /// <summary>
        /// The default request type.
        /// </summary>
        private const string DefaultRequestType = "application/x-www-form-urlencoded";

        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly ApplicationConfiguration _applicationConfiguration;

        /// <summary>
        /// The caching policy.
        /// </summary>
        private readonly ICacheService _cachingService;

        /// <summary>
        /// The OAuthentication token service.
        /// </summary>
        private readonly ITokenService _tokenService;

        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// The access token to be used to call the secure Funding Api.
        /// </summary>
        private string _accessToken;

        /// <summary>
        /// The API client.
        /// </summary>
        private HttpClient _apiClient;

        /// <summary>
        /// The token refresh attempt count.
        /// </summary>
        private int _tokenRefreshAttemptCount = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingApiHttpClient"/> class.
        /// Construct an instance of a Funding API client.
        /// </summary>
        /// <param name="applicationConfigOptions">The config service to use to lookup config info.</param>
        /// <param name="cachingService">The caching service.</param>
        /// <param name="tokenService">The Oauth token service.</param>
        /// <param name="httpClientFactory">Http client factory.</param>
        public FundingApiHttpClient(
            IOptions<ApplicationConfiguration> applicationConfigOptions,
            ICacheService cachingService,
            ITokenService tokenService,
            IHttpClientFactory httpClientFactory)
        {
            _applicationConfiguration = applicationConfigOptions.Value;
            _cachingService = cachingService;
            _tokenService = tokenService;
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient ApiClient
        {
            get
            {
                if (_apiClient == null)
                {
                    _apiClient = _httpClientFactory.CreateClient();
                    _apiClient.Timeout = new TimeSpan(0, 0, _applicationConfiguration.FundingSearchTimeoutSeconds);
                    _apiClient.BaseAddress = new Uri(_applicationConfiguration.FundingDataApiEndPoint);
                    _apiClient.DefaultRequestHeaders.Add(AcceptHeaderName, ApplicationJsonMediaType);

                    // Access tokens will be reused whilst still valid.
                    _accessToken = Task.Run(async () => await _tokenService.GetAccessToken()).GetAwaiter().GetResult();
                    _apiClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(BearerHeaderName, _accessToken);
                }

                return _apiClient;
            }
        }

        /// <summary>Creates a GET request to a single result.</summary>
        /// <typeparam name="T">The retrn type.</typeparam>
        /// <param name="queryStringParams">URL and querystring parameters to request.</param>
        /// <returns>A single result.</returns>
        public async Task<T> GetRequestSingleResult<T>(string queryStringParams)
        {
            var responseString = await GetResponse(queryStringParams);
            return JsonConvert.DeserializeObject<T>(responseString);
        }

        /// <summary>
        /// Get the string result of making a request to the specified url.
        /// </summary>
        /// <param name="url">The url to request.</param>
        /// <returns>A string containing the response content.</returns>
        public async Task<string> GetResponse(string url)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            SetupBaseUrl();

            var cacheKey = $"{nameof(GetPostResponse)}-{url}";
            var response =
                await _cachingService.AddOrGetExistingResultAsync(
                    cacheKey,
                    () => ApiClient.GetAsync(url),
                    CacheExpirationPolicy.Sliding);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await TokenRefreshRetries(() => GetResponse(url), response);
                throw new Exception(content);
            }

            return content;
        }

        /// <summary>
        /// Get the string result of making a request to the specified url.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="url">The url to request.</param>
        /// <returns>A string containing the response content.</returns>
        public async Task<T> GetResponseFromUserFundingView<T>(string url)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            SetupBaseUrl();

            var response = await ApiClient.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await TokenRefreshRetries(() => GetResponseFromUserFundingView<T>(url), response);
                throw new Exception(content);
            }

            return JsonConvert.DeserializeObject<T>(content);
        }

        /// <summary>Posts a request to a service.</summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="url">The url.</param>
        /// <param name="formData">Parameters to post to the endpoint.</param>
        /// <param name="requestType">The request type.</param>
        /// <returns>The result type.</returns>
        public async Task<T> PostRequest<T>(
            string url,
            string formData,
            string requestType = DefaultRequestType)
        {
            var parameters = formData?.Any() == true ? $"{ConvertJsonToSafeQueryString(JsonConvert.SerializeObject(formData))}" : string.Empty;
            var cacheKey = $"{url}{parameters}";
            return await _cachingService.AddOrGetExistingResultAsync(
                cacheKey,
                () => GetPostResponse<T>(url, requestType, formData),
                CacheExpirationPolicy.Sliding);
        }

        /// <summary>Posts a request to a service.</summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="url">The url.</param>
        /// <param name="formData">Parameters to post to the endpoint.</param>
        /// <param name="requestType">The request type.</param>
        /// <returns>The result type.</returns>
        public async Task<T> PostRequestToUserFundingView<T>(
            string url,
            string formData,
            string requestType = DefaultRequestType)
        {
            var parameters = formData?.Any() == true ? $"{ConvertJsonToSafeQueryString(JsonConvert.SerializeObject(formData))}" : string.Empty;
            var response = await GetPostResponse<T>(url, requestType, formData);

            return response;
        }

        /// <summary>
        /// Convert the Json to a safe query string.
        /// </summary>
        /// <param name="jsonQuery">The json query.</param>
        /// <returns>The query string.</returns>
        private static string ConvertJsonToSafeQueryString(string jsonQuery)
        {
            var str = "?";
            str += jsonQuery.Replace(":", "=")
                .Replace("{", string.Empty)
                .Replace("}", string.Empty)
                .Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Replace(",", "&")
                .Replace("\"", string.Empty)
                .Replace("\\", string.Empty);
            return str;
        }

        /// <summary>
        /// Creates a POST request to the supplied URL, and returns the response.
        /// </summary>
        /// <param name="url">The url to send the request.</param>
        /// <param name="requestType">The request type.</param>
        /// <param name="parameters">The parameters to post to the endpoint.</param>
        private async Task<T> GetPostResponse<T>(string url, string requestType, string parameters = null)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            var stringContent = new StringContent(parameters ?? string.Empty, Encoding.UTF8, requestType);

            SetupBaseUrl();

            var response = await ApiClient.PostAsync(url, stringContent);
            if (!response.IsSuccessStatusCode)
            {
                await TokenRefreshRetries(() => GetPostResponse<T>(url, requestType, parameters), response);
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception(errorContent);
            }

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                using (var streamReader = new StreamReader(stream))
                {
                    using (var reader = new JsonTextReader(streamReader))
                    {
                        var serializer = new JsonSerializer();

                        var result = serializer.Deserialize<T>(reader);
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// Set up the base url and flag on API to use.
        /// </summary>
        private void SetupBaseUrl()
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                _apiClient = null;
            }
        }

        /// <summary>
        /// We will attempt to refresh the access token three times before throwing an unauthorized exception.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="response">The response.</param>
        /// <exception cref="UnauthorizedAccessException">An unauthorized exception.</exception>
        private async Task TokenRefreshRetries<T>(Func<Task<T>> action, HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                while (_tokenRefreshAttemptCount <= 3)
                {
                    _apiClient = null;
                    _tokenRefreshAttemptCount++;
                    await action.Invoke();
                }

                throw new UnauthorizedAccessException();
            }
        }
    }
}