using Newtonsoft.Json;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.RequestObjects;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GroupingType = PDS.ViewYourFunding.Services.Constants.GroupingType;
using SearchFilter = PDS.ViewYourFunding.Services.DTOs.SearchFilter;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// Provide implementation to fetch funding data via an HTTP api.
    /// </summary>
    public class FundingApiService : IFundingApiService
    {
        /// <summary>
        /// The regular expression to match a Local Authority code (3 digits).
        /// </summary>
        private static readonly Regex LocalAuthorityCodeRegex = new Regex("^[0-9]{3}$", RegexOptions.Compiled);

        /// <summary>
        /// The regular expression to match a UKPRN code (8 digits).
        /// </summary>
        private static readonly Regex UkprnCodeRegex = new Regex("^[0-9]{8}$", RegexOptions.Compiled);

        /// <summary>
        /// The regular expression to check for at least one alphanumeric character.
        /// </summary>
        private static readonly Regex SearchTermRegex = new Regex("[a-zA-Z0-9]", RegexOptions.Compiled);

        /// <summary>
        /// Regular Expressions for special characters.
        /// </summary>
        private static readonly Regex _spacingCharacters = new Regex(@"[\s\u2212\u2013\u2014\u2010-]+", RegexOptions.Compiled);

        /// <summary>
        /// Regular Expressions for disallowed characters.
        /// </summary>
        private static readonly Regex _disallowedCharacters = new Regex(@"[^\w]+", RegexOptions.Compiled);

        /// <summary>
        /// Regular Expressions for multiple underscores.
        /// </summary>
        private static readonly Regex _multipleUnderScores = new Regex(@"_{2,}", RegexOptions.Compiled);

        /// <summary>
        /// Default Search pattern.
        /// </summary>
        private const string DefaultSearchPattern = "/.*/";

        /// <summary>
        /// The http api service to use.
        /// </summary>
        private readonly IHttpApiService _httpApiService;

        /// <summary>
        /// Logger.
        /// </summary>
        private readonly ILoggerAdapter<FundingApiService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingApiService"/> class.
        /// Create a new instance of a FundingApiService.
        /// </summary>
        /// <param name="httpApiService">The HTTP service to use.</param>
        /// <param name="logger">Logger.</param>
        public FundingApiService(IHttpApiService httpApiService, ILoggerAdapter<FundingApiService> logger)
        {
            _httpApiService = httpApiService;
            _logger = logger;
        }

        /// <summary>
        /// Use HTTP api service to fetch funding.
        /// </summary>
        /// <param name="requestObj">The options to filter by - including funding streams and periods.</param>
        /// <returns>A funding collection.</returns>
        public async Task<IFundingApiSearchResponseFunding> SearchFunding(FundingApiSearchRequestObject requestObj)
        {
            _logger?.LogInformation("DEBUG1 SearchFunding() Start");

            if (requestObj.SearchTerm != null && !SearchTermRegex.IsMatch(requestObj.SearchTerm))
            {
                return null;
            }

            var response = await _httpApiService.PostRequest<FundingApiSearchResponse>($"funding/SearchFunding", JsonConvert.SerializeObject(requestObj), "application/json");

            _logger?.LogInformation("DEBUG1 SearchFunding() End");

            return response;
        }

        /// <summary>
        /// Search for local authorities.
        /// </summary>
        /// <param name="request">A request object containing the search parameters.</param>
        /// <returns>A response object containing the local authorities matching the search parameters.</returns>
        public async Task<FundingApiSearchLocalAuthoritiesResponse> SearchLocalAuthorities(
            FundingApiSearchLocalAuthoritiesRequest request)
        {
            var localAuthorities = new Dictionary<string, string>();
            var searchTerm = request.SearchTerm;
            int? laCode = null;

            if (request.SearchTerm != null && !SearchTermRegex.IsMatch(searchTerm))
            {
                return null;
            }

            if (searchTerm != null && LocalAuthorityCodeRegex.IsMatch(searchTerm))
            {
                laCode = int.Parse(searchTerm);
            }

            searchTerm = GetCleanSearchTerm(searchTerm);

            var fundingStreams = new List<FundingApiSearchFundingStream>();

            foreach (var fundingStreamCode in request.FundingStreamConfiguration.Keys)
            {
                var configuration = request.FundingStreamConfiguration[fundingStreamCode];
                var applicableFundingPeriods = FundingPeriodHelper.GetLatestFundingPeriodCodes_FundingPeriodFormat(configuration.SettingValues, FundingPeriodHelper.GetActiveFundingPeriodCodes(configuration, request.PreviewModeEnabled));

                foreach (var fundingPeriodCode in applicableFundingPeriods)
                {
                    var publication = configuration.Publications
                        .Where(publication => publication.FundingPeriodCode == fundingPeriodCode &&
                        (publication.Status == PublicationStatus.Published
                            || (request.PreviewModeEnabled && publication.Status == PublicationStatus.Preview)))
                        .OrderByDescending(p => p.PublishedDate)
                        .FirstOrDefault();

                    if (publication == null)
                    {
                        continue;
                    }

                    var filters = new List<SearchFilter>();

                    if (laCode != null)
                    {
                        filters.Add(new SearchFilter
                        {
                            PropertyName = SearchFilterPropertyName.PrimaryIdentifier,
                            PropertyValue = laCode.ToString()
                        });
                    }

                    var groupingReason = configuration.SettingValues?
                        .FirstOrDefault(settingValue => settingValue.Setting.SettingName == "LAGroupingReason")?.Value ?? "Information";

                    filters.Add(new SearchFilter
                    {
                        PropertyName = SearchFilterPropertyName.GroupingReason,
                        PropertyValue = groupingReason
                    });

                    var cutOffDate = FundingPeriodHelper.GetCutOffDateForPublication(publication);

                    fundingStreams.Add(new FundingApiSearchFundingStream
                    {
                        GroupingType = GroupingType.LocalAuthority,
                        Filters = filters.ToArray(),
                        BeforeDateTime = cutOffDate,
                        FundingStreamCode = fundingStreamCode,
                        PeriodCodes = new[] { publication.FundingPeriodCode }
                    });
                }
            }

            var requestObj = new FundingApiSearchRequestObject
            {
                WaitForIndexBuild = false,
                SearchTerm = searchTerm,
                FundingStreams = fundingStreams.ToArray()
            };

            var result = await SearchFunding(requestObj);

            foreach (var funding in result.Funding)
            {
                var localAuthorityCode = funding.GroupCode;

                if (string.IsNullOrEmpty(localAuthorityCode) || localAuthorities.ContainsKey(localAuthorityCode))
                {
                    continue;
                }

                localAuthorities.Add(localAuthorityCode, funding.GroupName);
            }

            return new FundingApiSearchLocalAuthoritiesResponse
            {
                LocalAuthorities = localAuthorities
            };
        }

        /// <summary>
        /// Use HTTP api service to fetch provider funding.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="getLatest">Whether to get the latest funding only.</param>
        /// <returns>A provider funding collection.</returns>
        public async Task<IFundingApiSearchResponseProviderFunding> SearchProviderFunding(
            FundingApiSearchRequestObject request, bool getLatest = false)
        {
            _logger?.LogInformation("DEBUG1 SearchProviderFunding() Start");

            if (request.SearchTerm != null && !SearchTermRegex.IsMatch(request.SearchTerm))
            {
                return null;
            }

            if (request.FundingStreams.First()?.Filters?.Any(filter => filter.PropertyName.Equals(SearchFilterPropertyName.PrimaryIdentifierList)) == true)
            {
                request.SearchTerm = string.Empty;
            }

            if (request.SearchTerm != null && UkprnCodeRegex.IsMatch(request.SearchTerm))
            {
                if (request.FundingStreams?.FirstOrDefault() == null)
                {
                    throw new System.Exception("FundingStreams cannot be null");
                }

                var firstFundingStream = request.FundingStreams.First();

                if (firstFundingStream.Filters == null)
                {
                    firstFundingStream.Filters = new SearchFilter[0];
                }

                var filtersList = firstFundingStream.Filters.ToList();

                filtersList.Add(
                    new SearchFilter
                    {
                        PropertyName = SearchFilterPropertyName.Ukprn,
                        PropertyValue = request.SearchTerm
                    });
                firstFundingStream.Filters = filtersList.ToArray();
                request.SearchTerm = string.Empty;
            }

            request.SearchTerm = GetCleanSearchTerm(request.SearchTerm);

            var endpoint = getLatest ? "funding/SearchLatestProviderFunding" : "funding/SearchProviderFunding";

            var response = await _httpApiService.PostRequest<ProviderFundingApiSearchResponse>(
                endpoint,
                JsonConvert.SerializeObject(request),
                "application/json");

            foreach (var providerFunding in response.ProviderFunding)
            {
                providerFunding.SearchResultDisplay =
                    ProviderDisplayHelper.GetSearchResultDisplayName(
                    providerFunding.OrganisationName,
                    providerFunding.OrganisationTown,
                    providerFunding.OrganisationPostcode);
            }

            _logger?.LogInformation("DEBUG1 SearchProviderFunding() End");

            return response;
        }

        /// <summary>
        /// An HTTP api service to fetch funding.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>A matched funding.</returns>
        public async Task<IFundingApiSearchFunding> GetFunding(string id)
        {
            _logger?.LogInformation("DEBUG1 GetFunding() Start");

            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var response = await _httpApiService.GetRequestSingleResult<FundingApiSearchFunding>(
                $"funding/GetFunding?id={id}");

            _logger?.LogInformation("DEBUG1 GetFunding() End");

            return response;
        }

        /// <summary>
        /// An HTTP api service to fetch provider funding.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>A matched provider funding.</returns>
        public async Task<IFundingApiSearchProviderFunding> GetProviderFunding(string id)
        {
            _logger?.LogInformation("DEBUG1 GetProviderFunding() Start");

            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var response = await _httpApiService.GetRequestSingleResult<FundingApiSearchProviderFunding>(
                $"funding/GetProviderFunding?id={id}");

            _logger?.LogInformation("DEBUG1 GetProviderFunding() End");

            return response;
        }

        /// <summary>
        /// An HTTP api service to check if user has visited an funding or not.
        /// </summary>
        /// <param name="userId">The user id to lookup.</param>
        /// <param name="fundingId">The funding id to lookup.</param>
        /// <returns>True if user has viewed the funding else false.</returns>
        public async Task<bool> HasUserVisitedFunding(string userId, string fundingId)
        {
            try
            {
                var response = await _httpApiService.GetResponseFromUserFundingView<bool>(
                    $"user/HasUserVisitedFunding?userId={userId}&fundingId={fundingId}");

                return response;
            }
            catch (Exception)
            {
                _logger?.LogError($"Error occured while getting user visit details for UserId: {userId}, FundingId: {fundingId}");
                return true;
            }
        }

        /// <summary>
        /// An HTTP api service to add user funding view detail.
        /// </summary>
        /// <param name="request">The reuest object containing user and funding detail.</param>
        /// <returns>The asynchronous task.</returns>
        public async Task AddUserFundingViewDetail(AddUserFundingViewRequest request)
        {
            try
            {
                var response = await _httpApiService.PostRequestToUserFundingView<bool?>(
                    $"user/AddUserFundingViewDetail",
                    JsonConvert.SerializeObject(request),
                    "application/json");

                if (response == null)
                {
                    _logger.LogError($"Error occured whilst adding user allocation view details for: User Id: {request.UserId}, Funding Id: {request.FundingId}");
                }
            }
            catch (Exception)
            {
                _logger?.LogError($"Error occured while saving user visit details for UserId: {request.UserId}, Funding Id: {request.FundingId}");
            }
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
                _logger.LogError($"Error occured whilst getting user funding view count view details for: User Id: {userId}");
                return new UserFundingViewCountResponse();
            }

            return response;
        }

        /// <summary>
        /// Clean up the search term to match how its kept in the search index.
        /// </summary>
        /// <param name="originalSearchTerm">The original search term to clean up.</param>
        /// <returns>A cleaned up search term.</returns>
        private string GetCleanSearchTerm(string originalSearchTerm)
        {
            if (string.IsNullOrEmpty(originalSearchTerm))
            {
                return DefaultSearchPattern;
            }

            var cleanSearchTerm = _spacingCharacters.Replace(originalSearchTerm, "_");
            cleanSearchTerm = cleanSearchTerm.Replace(".", "_");
            cleanSearchTerm = _disallowedCharacters.Replace(cleanSearchTerm, string.Empty);
            cleanSearchTerm = _multipleUnderScores.Replace(cleanSearchTerm, "_");
            cleanSearchTerm = cleanSearchTerm.Trim(new[] { '_' });

            if (cleanSearchTerm == string.Empty)
            {
                return DefaultSearchPattern;
            }

            // Lucene regex pattern with wildcards at start and end of the search string:
            return $"/.*{cleanSearchTerm}.*/";
        }
    }
}