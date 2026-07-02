using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Identity.Claims.Interfaces;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Attributes;
using PDS.ViewYourFunding.Services.Cache;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Implementations.Hacks;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using PDS.ViewYourFunding.Web.Enums;
using PDS.ViewYourFunding.Web.Helpers;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FundingStreamHelper = PDS.ViewYourFunding.Web.Helpers.FundingStreamHelper;
using User = Pds.Core.Common.Identity.Models.User;

namespace PDS.ViewYourFunding.Web.Controllers
{
    /// <summary>
    /// A base MVC controller.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class BaseFundingController : BaseController
    {
        protected const string PARENT_PROVIDER_TYPE_DEFAULT = "DEFAULT";

        protected const string PARENT_PROVIDER_TYPE_MIXED = "IS_MIXED";

        /// <summary>
        /// The settings service.
        /// </summary>
        private readonly IUserJourneyService _userJourneyService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The funding Api service.
        /// </summary>
        private readonly IFundingApiService _fundingApiService;

        /// <summary>
        /// The cache service.
        /// </summary>
        private readonly ICacheService _cacheService;

        private readonly IGlobalSettingService _globalSettingService;

        /// <summary>
        /// The http api service to use.
        /// </summary>
        private readonly IHttpApiService _httpApiService;

        /// <summary>
        /// The funding view service.
        /// </summary>
        private readonly IFundingViewService _fundingViewService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFundingController"/> class.
        /// </summary>
        /// <param name="securityService">The security service to use.</param>
        /// <param name="applicationConfigurationOptions">The configuration service.</param>
        /// <param name="settingsService">The settings service to use.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="fundingApiService">The API service to use for searching for funding.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="fundingViewService">The funding view service.</param>
        /// <param name="globalSettingService">The global setting service.</param>
        /// <param name="httpApiService">The HTTP service to use.</param>
        public BaseFundingController(
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            IUserJourneyService settingsService,
            IMapper mapper,
            IFundingApiService fundingApiService,
            ICacheService cacheService,
            IFundingViewService fundingViewService,
            IGlobalSettingService globalSettingService,
            IHttpApiService httpApiService = null)
            : base(securityService, applicationConfigurationOptions?.Value?.IsProductionEnvironment ?? false)
        {
            _userJourneyService = settingsService;
            _mapper = mapper;
            _fundingApiService = fundingApiService;
            _fundingViewService = fundingViewService;
            _cacheService = cacheService;
            _globalSettingService = globalSettingService;
            _httpApiService = httpApiService;
        }

        /// <summary>
        /// Get the active funding streams.
        /// </summary>
        /// <param name="userJourneyService">A user journey service to use.</param>
        /// <returns>A list of funding streams.</returns>
        public static async Task<List<FundingStream>> GetActiveFundingStreams(IUserJourneyService userJourneyService)
        {
            var allFundingStreams = await userJourneyService.GetFundingStreams();

            return allFundingStreams?
                .Where(fundingStream => fundingStream.SettingValues?.Any() == true)
                .OrderBy(fundingStream => fundingStream.FundingStreamName).ToList();
        }

        /// <summary>
        /// Can we show the statement specification or not.
        /// </summary>
        /// <returns>True if to show as a statement specification, false if not.</returns>
        public async Task<bool> GetStatementSpecificationState()
        {
            var cacheKey = GlobalSettingTypeConstants.GetCacheKey(GlobalSettingTypeConstants.DisplayStatementSpecificationTypeId);

            var showStatementSpecificationSetting = await _cacheService.AddOrGetExistingResult(
                cacheKey,
                async () => await _globalSettingService.GetFirstOrDefault(GlobalSettingTypeConstants.DisplayStatementSpecificationTypeId),
                CacheExpirationPolicy.Absolute);

            bool.TryParse(showStatementSpecificationSetting?.Value, out var showStatementSpecificationBool);
            return showStatementSpecificationBool;
        }

        /// <summary>
        /// Can we show selectors or not.
        /// </summary>
        /// <returns>True if selectors should be shown, false if not.</returns>
        public async Task<bool> GetShowSelectorsState()
        {
            var cacheKey = GlobalSettingTypeConstants.GetCacheKey(GlobalSettingTypeConstants.DisplaySelectorsTypeId);

            var showSelectorsSetting = await _cacheService.AddOrGetExistingResult(
                cacheKey,
                async () => await _globalSettingService.GetFirstOrDefault(GlobalSettingTypeConstants.DisplaySelectorsTypeId),
                CacheExpirationPolicy.Absolute);

            bool.TryParse(showSelectorsSetting?.Value, out var showSelectors);
            return showSelectors;
        }

        /// <summary>
        /// Show we show selectors or not.
        /// </summary>
        /// <returns>True if selectors should be shown, false if not.</returns>
        public async Task<bool> GetShowData()
        {
            var cacheKey = GlobalSettingTypeConstants.GetCacheKey(GlobalSettingTypeConstants.ShowDataTypeId);

            var showDataSetting = await _cacheService.AddOrGetExistingResult(
                cacheKey,
                async () => await _globalSettingService.GetFirstOrDefault(GlobalSettingTypeConstants.ShowDataTypeId),
                CacheExpirationPolicy.Absolute);

            bool.TryParse(showDataSetting?.Value, out var showData);
            return showData;
        }

        /// <summary>
        /// Get the component defaults.
        /// </summary>
        /// <returns>The component defaults..</returns>
        protected static Dictionary<ComponentType, Defaults> GetComponentDefaults()
        {
            if (_componentDefaults == null)
            {
                _componentDefaults = GetDefaultsUsingReflection();
            }

            return _componentDefaults;
        }

        /// <summary>
        /// Get the provider parent grouping types.
        /// </summary>
        /// <param name="fundingStreams">The funding streams to get parent grouping types for.</param>
        /// <param name="defaultGroupingType">The default grouping type.</param>
        /// <returns>A list of strings.</returns>
        protected static Dictionary<string, string> GetProviderParentGroupingType(Dictionary<string, FundingStream> fundingStreams, string defaultGroupingType = GroupingType.LocalAuthority)
        {
            var returnList = new Dictionary<string, string>
            {
                { PARENT_PROVIDER_TYPE_DEFAULT, defaultGroupingType }
            };

            BuildFundingStreamDictionary(fundingStreams, returnList);

            return returnList;
        }

        /// <summary>
        /// Get the provider parent grouping types.
        /// </summary>
        /// <param name="fundingStreams">The funding streams to get parent grouping types for.</param>
        /// <param name="defaultGroupingType">The default grouping type.</param>
        /// <returns>A list of strings.</returns>
        protected static Dictionary<string, string> GetProviderParentGroupingTypeWithIndicative(Dictionary<string, FundingStream> fundingStreams, string defaultGroupingType = GroupingType.LocalAuthority)
        {
            var returnList = new Dictionary<string, string>
            {
                { PARENT_PROVIDER_TYPE_DEFAULT, defaultGroupingType },
                { PARENT_PROVIDER_TYPE_MIXED, "true" }
            };

            BuildFundingStreamDictionary(fundingStreams, returnList);

            return returnList;
        }

        /// <summary>
        /// Gets the historic years.
        /// </summary>
        /// <param name="currentYearFrom">The current 'from' year.</param>
        /// <returns>The historic years.</returns>
        protected static List<(int yearFrom, int yearTo)> GetHistoricYears(int currentYearFrom)
        {
            var currentAndHistoricYears = new List<(int, int)>();

            for (var numberOfYearsAgo = 1;
                numberOfYearsAgo <= ViewYourFundingConstants.NumberOfYearsOfHistoricAllocationsToShow;
                numberOfYearsAgo++)
            {
                var historicYearFrom = currentYearFrom - numberOfYearsAgo;

                currentAndHistoricYears.Add((historicYearFrom, historicYearFrom + 1));
            }

            return currentAndHistoricYears;
        }

        /// <summary>
        /// Returns true in the historic years are external.
        /// </summary>
        /// <param name="fundingStream">The funging stream.</param>
        /// <param name="yearFrom">The yearFrom.</param>
        /// <param name="currentYear">The currentYear.</param>
        /// <returns>bool.</returns>
        protected static bool HistoricAllocationsAreExternal(FundingStream fundingStream, int yearFrom, int currentYear)
        {
            var externalYearString = fundingStream.SettingValues?
                .FirstOrDefault(settingValue =>
                    settingValue.Setting.SettingName.Equals("HistoricAllocationsAreExternalYear", StringComparison.InvariantCultureIgnoreCase))
                ?.Value;

            if (!int.TryParse(externalYearString, out var externalYear))
            {
                externalYear = currentYear - 1;
            }

            return yearFrom <= externalYear;
        }

        /// <summary>
        /// Squashes the data requirements.
        /// </summary>
        /// <param name="dataRequirements">The data requirements.</param>
        /// <param name="type">The type.</param>
        /// <returns>The Funding Api Search Results.</returns>
        protected static FundingApiSearchRequestObject SquashDataRequirements(IEnumerable<FundingApiSearchRequestObject> dataRequirements, string type)
        {
            var matches = dataRequirements?.Where(dataRequirement => dataRequirement.Type == type);

            if (matches?.Any() != true)
            {
                return null;
            }

            var returnObj = dataRequirements.First();
            returnObj.FundingStreams = matches.SelectMany(match => match.FundingStreams).Distinct().ToArray();

            return returnObj;
        }

        /// <summary>
        /// Checks and removed funding publications.
        /// </summary>
        /// <param name="fundingPeriodPublications">The funding publications.</param>
        /// <param name="providerFundings">The provider fundings.</param>
        protected static void CheckAndRemoveProviderFundingPublications(
            List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>> fundingPeriodPublications,
            List<IFundingApiSearchProviderFunding> providerFundings)
        {
            fundingPeriodPublications.RemoveAll(fundingPeriodPublication => fundingPeriodPublication.Value.Count == 0);

            foreach (var fundingPeriodPublication in fundingPeriodPublications)
            {
                var publicationsToRemove = new List<Publication>();
                foreach (var publicationToCheck in fundingPeriodPublication.Value)
                {
                    var cutoffDate = FundingPeriodHelper.GetCutOffDateForPublication(publicationToCheck);
                    var cutoffDateAfterPublicationDate = providerFundings.Where(funding => funding.FundingPeriodCode == publicationToCheck.FundingPeriodCode).Any(x => x.StatusChangedDate <= cutoffDate);

                    // There is no match with a publication date before cut off date
                    if (!cutoffDateAfterPublicationDate)
                    {
                        publicationsToRemove.Add(publicationToCheck);
                    }
                }

                // Remove any we need to.
                foreach (var publicationToRemove in publicationsToRemove)
                {
                    fundingPeriodPublication.Value.Remove(publicationToRemove);
                }
            }
        }

        /// <summary>
        /// Get distinct provider fundings (for example: If provider funding is grouped in Information and Payment, this method will ensure provider funding from one of the groups is selected).
        /// </summary>
        /// <param name="providerFundings">The provider fundings.</param>
        /// <returns>List on distinct provider fundings.</returns>
        protected static List<IFundingApiSearchProviderFunding> GetDistinctProviderFundings(IEnumerable<IFundingApiSearchProviderFunding> providerFundings)
        {
            if (providerFundings?.Any() == false)
            {
                return new List<IFundingApiSearchProviderFunding>();
            }

            return providerFundings
                        .OrderByDescending(fv => FundingVersionHelper.Parse(fv.FundingVersion))
                        .ThenBy(a => a.StatusChangedDate)
                        .DistinctBy(a => a.Id)
                        .ToList();
        }

        /// <summary>
        /// Gets the latest provider fundings under each funding streams.
        /// </summary>
        /// <param name="providerFundings">The provider fundings.</param>
        /// <returns>The Latest Provider Fundings Under Each Funding Streams.</returns>
        protected static List<IFundingApiSearchProviderFunding> GetLatestProviderFundingsByFundingStreams(IEnumerable<IFundingApiSearchProviderFunding> providerFundings)
        {
            if (providerFundings?.Any() == false)
            {
                return new List<IFundingApiSearchProviderFunding>();
            }

            providerFundings = GetDistinctProviderFundings(providerFundings);

            return providerFundings
                        .GroupBy(
                            pf => new { pf.FundingStreamCode, pf.FundingPeriodCode, pf.OrganisationUkprn },
                            (key, groupedValues) => groupedValues
                                                        .OrderByDescending(gpf => FundingVersionHelper.Parse(gpf.FundingVersion))
                                                        .ThenByDescending(gpf => gpf.StatusChangedDate)
                                                        .First()).ToList();
        }

        /// <summary>
        /// Checks and removed funding publications.
        /// </summary>
        /// <param name="fundingPeriodPublications">The funding publications.</param>
        /// <param name="fundings">The provider fundings.</param>
        protected static void CheckAndRemoveFundingPublications(
          List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>> fundingPeriodPublications,
          List<IFundingApiSearchFunding> fundings)
        {
            fundingPeriodPublications.RemoveAll(fundingPeriodPublication => fundingPeriodPublication.Value.Count == 0);

            foreach (var fundingPeriodPublication in fundingPeriodPublications)
            {
                var publicationsToRemove = new List<Publication>();
                foreach (var publicationToCheck in fundingPeriodPublication.Value)
                {
                    var cutoffDate = FundingPeriodHelper.GetCutOffDateForPublication(publicationToCheck);
                    var cutoffDateAfterPublicationDate = fundings.Where(funding => funding.FundingPeriodCode == publicationToCheck.FundingPeriodCode).Any(x => x.StatusChangedDate <= cutoffDate);

                    // There is no match with a publication date before cut off date
                    if (!cutoffDateAfterPublicationDate)
                    {
                        publicationsToRemove.Add(publicationToCheck);
                    }
                }

                // Remove any we need to.
                foreach (var publicationToRemove in publicationsToRemove)
                {
                    fundingPeriodPublication.Value.Remove(publicationToRemove);
                }
            }
        }

        protected IEnumerable<string> GetProviderFundingIds(IEnumerable<IEnumerable<string>> providerFundings)
        {
            foreach (var providerFunding in providerFundings)
            {
                if (providerFunding != null)
                {
                    var ukprnList = providerFunding.Select(GetUkprnFromProviderFunding);

                    foreach (var ukPrn in ukprnList)
                    {
                        if (!string.IsNullOrWhiteSpace(ukPrn))
                        {
                            yield return ukPrn;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the current users ukprn via the claim but sets the users providername via funding depending on the type of provider - MAT, LA, provider etc.
        /// </summary>
        /// <returns>A user.</returns>
        protected async Task<User> GetUserAsync()
        {
            var userDetails = await SecurityService.GetUserFromClaims(User);
            userDetails.ProviderName = await GetProviderName(userDetails);
            return userDetails;
        }

        /// <summary>
        /// Get the funding stream configuration.
        /// </summary>
        /// <param name="fundingUIViewType">The funding UI screen it is for.</param>
        /// <param name="currentUser">The current user (optional).</param>
        /// <param name="currentUserPassed">Has the current user been passed in (optional).</param>
        /// <returns>The relevant funding streams.</returns>
        protected async Task<Dictionary<string, FundingStream>> GetRelevantFundingStreams(
            FundingUIViewType fundingUIViewType,
            User currentUser = null,
            bool currentUserPassed = false)
        {
            var fundingStreamDictionary = new Dictionary<string, FundingStream>();
            var fundingStreams = await GetActiveFundingStreams();

            if (fundingStreams == null)
            {
                return fundingStreamDictionary;
            }

            var previewModeEnabled = await PreviewModeEnabled(currentUser, currentUserPassed);

            foreach (var fundingStream in fundingStreams)
            {
                var fundingStreamCode = fundingStream.FundingStreamCode;
                var isRelevant = false;

                switch (fundingUIViewType)
                {
                    case FundingUIViewType.National:
                        isRelevant = fundingStream.RelevantForNational;
                        break;
                    case FundingUIViewType.Providers_LoggedIn:
                        isRelevant = fundingStream.RelevantForProviders_LoggedIn;
                        break;
                    case FundingUIViewType.Providers_Public:
                        isRelevant = fundingStream.RelevantForProviders_Public;
                        break;
                    case FundingUIViewType.Organisations_LoggedIn:
                        isRelevant = fundingStream.RelevantForOrganisations_LoggedIn;
                        break;
                    case FundingUIViewType.Organisations_Public:
                        isRelevant = fundingStream.RelevantForOrganisations_Public;
                        break;
                }

                if (!isRelevant)
                {
                    continue;
                }

                fundingStream.Publications = fundingStream.Publications == null
                    ? new List<Publication>()
                    : fundingStream.Publications
                        .OrderByDescending(publication => publication.PublishedDate)
                        .Where(publication => publication.Status == PublicationStatus.Published
                            || (previewModeEnabled && publication.Status == PublicationStatus.Preview))
                        .ToList();

                fundingStreamDictionary.Add(fundingStreamCode, fundingStream);
            }

            return fundingStreamDictionary;
        }

        /// <summary>
        /// Get active funding streams.
        /// </summary>
        /// <returns>Active funding steams.</returns>
        protected async Task<List<FundingStream>> GetActiveFundingStreams()
        {
            return await GetActiveFundingStreams(_userJourneyService);
        }

        /// <summary>
        /// Get the funding streams configured to use auto pull.
        /// </summary>
        /// <returns>A list of funding streams configured to use auto pull.</returns>
        protected async Task<List<FundingStream>> GetAutoPullFundingStreams()
        {
            var allFundingStreams = await _userJourneyService.GetFundingStreams();

            return allFundingStreams?
                .Where(fundingStream => fundingStream.UseAutoPull())
                .OrderBy(fundingStream => fundingStream.FundingStreamName).ToList();
        }

        /// <summary>
        /// Gets the provider funding document.
        /// </summary>
        /// <param name="providerDetails">The provider details.</param>
        /// <param name="publishedDate">The published date.</param>
        /// <param name="fundingStream">Configuration for the funding stream.</param>
        /// <param name="isLoggedIn">Is the call from a logged in action.</param>
        /// <param name="fileFormat">The file format.</param>
        /// <returns>The funding document.</returns>
        protected FundingDocument GetProviderFundingDocument(
            IFundingApiSearchProviderFunding providerDetails,
            DateTime publishedDate,
            FundingStream fundingStream,
            bool isLoggedIn,
            string fileFormat = FundingDocumentFileType.Spreadsheet_OpenFormat)
        {
            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(providerDetails.FundingPeriodCode);
            var yearTypeCode = providerDetails.FundingPeriodCode.Substring(0, 2);

            var routeName = isLoggedIn ? LoggedInConstants.RouteName_ProviderSpreadsheetDownload : ViewYourFundingConstants.RouteName_ProviderSpreadsheetDownload;

            var routeToFundingDocument = Url?.RouteUrl(
                routeName,
                new
                {
                    providerDetails.Id,
                    providerDetails.FundingStreamCode,
                    Ukprn = providerDetails.OrganisationUkprn,
                    YearTypeCode = yearTypeCode,
                    YearFrom = yearFrom,
                    YearTo = yearTo,
                    Format = fileFormat,
                    PublishedDate = publishedDate.ToRouteParameterString()
                });

            return new FundingDocument
            {
                FileExtension = fileFormat,
                FileSizeBytes = GetFileDownloadSizeInBytes(fundingStream, "ProviderDownloadSizeInBytes"),
                FilePath = routeToFundingDocument,
                FundingStreamCode = providerDetails.FundingStreamCode,
                YearFrom = yearFrom,
                YearTo = yearTo
            };
        }

        /// <summary>
        /// Gets the provider funding document.
        /// </summary>
        /// <param name="providerDetails">The provider details.</param>
        /// <param name="publishedDate">The published date.</param>
        /// <param name="fundingStream">Configuration for the funding stream.</param>
        /// <param name="isLoggedIn">Is the call from a logged in action.</param>
        /// <param name="fileFormat">The file format.</param>
        /// <returns>The funding document.</returns>
        protected FundingDocument GetFundingDocument(
            IFundingApiSearchFunding providerDetails,
            DateTime publishedDate,
            FundingStream fundingStream,
            bool isLoggedIn,
            string fileFormat = FundingDocumentFileType.Spreadsheet_OpenFormat)
        {
            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(providerDetails.FundingPeriodCode);
            var yearTypeCode = providerDetails.FundingPeriodCode.Substring(0, 2);

            var routeName = isLoggedIn ? LoggedInConstants.RouteName_ProviderSpreadsheetDownload : ViewYourFundingConstants.RouteName_ProviderSpreadsheetDownload;

            var routeToFundingDocument = Url?.RouteUrl(
                routeName,
                new
                {
                    providerDetails.Id,
                    providerDetails.FundingStreamCode,
                    Ukprn = providerDetails.GroupUkprn,
                    YearTypeCode = yearTypeCode,
                    YearFrom = yearFrom,
                    YearTo = yearTo,
                    Format = fileFormat,
                    PublishedDate = publishedDate.ToRouteParameterString()
                });

            return new FundingDocument
            {
                FileExtension = fileFormat,
                FileSizeBytes = GetFileDownloadSizeInBytes(fundingStream, "ProviderDownloadSizeInBytes"),
                FilePath = routeToFundingDocument,
                FundingStreamCode = providerDetails.FundingStreamCode,
                YearFrom = yearFrom,
                YearTo = yearTo
            };
        }

        protected FundingDocument GetFundingDocumentForOrganisation(
            FundingStream fundingStream,
            string fundingPeriodCode,
            DateTime publishedDate,
            string localAuthorityCode,
            string id = null,
            string ukprn = "",
            bool isLoggedIn = false,
            string fileFormat = FundingDocumentFileType.Spreadsheet_OpenFormat)
        {
            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(fundingPeriodCode);

            var routeName = isLoggedIn ? LoggedInConstants.RouteName_OrganisationSpreadsheetDownload : ViewYourFundingConstants.RouteName_LocalAuthoritySpreadsheetDownload;

            var routeToFundingDocument =
                Url?.RouteUrl(routeName, new
                {
                    fundingStream.FundingStreamCode,
                    LocalAuthorityCode = localAuthorityCode,
                    YearTypeCode = FundingPeriodHelper.GetYearSettingCode(fundingStream.SettingValues),
                    YearFrom = yearFrom,
                    YearTo = yearTo,
                    Format = fileFormat,
                    PublishedDate = publishedDate.ToRouteParameterString(),
                    Ukprn = ukprn,
                    Id = id
                });

            return new FundingDocument
            {
                DocumentPublishedDate = publishedDate,
                FileExtension = isLoggedIn ? FundingDocumentFileType.Spreadsheet_CSVFormat : FundingDocumentFileType.Spreadsheet_OpenFormat,
                FileSizeBytes = GetFileDownloadSizeInBytes(fundingStream, "OrganisationDownloadSizeInBytes"),
                FilePath = routeToFundingDocument,
                FundingStreamCode = fundingStream.FundingStreamCode,
                YearFrom = yearFrom,
                YearTo = yearTo
            };
        }

        /// <summary>
        /// Does the provider funding search.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="fundingStreams">The funding stream configuration.</param>
        /// <param name="parentProviderTypes">The parent provider type to find.</param>
        /// <param name="groupingReason">The grouping reason to find.</param>
        /// <param name="fundingPeriodCode">The year to retrieve.</param>
        /// <param name="publishedDate">The published date.</param>
        /// <param name="multipleUkPrnSearchList">The multiple ukprn search list.</param>
        /// <param name="currentUser">The current user (optional).</param>
        /// <param name="currentUserPassed">Has the current user been passed in (optional).</param>
        /// <param name="byPassGrouping">By Pass grouping in search results.</param>
        /// <returns>The search provider result list.</returns>
        /// <exception cref="ArgumentOutOfRangeException">There are no publications for the date {publishedDate}.</exception>
        protected async Task<List<IFundingApiSearchProviderFunding>> DoProviderFundingSearch(
            string searchTerm,
            Dictionary<string, FundingStream> fundingStreams,
            Dictionary<string, string> parentProviderTypes,
            string groupingReason,
            string fundingPeriodCode = null,
            DateTime? publishedDate = null,
            string multipleUkPrnSearchList = null,
            User currentUser = null,
            bool currentUserPassed = false,
            bool byPassGrouping = false)
        {
            var returnList = new List<IFundingApiSearchProviderFunding>();
            var previewModeEnabled = await PreviewModeEnabled(currentUser, currentUserPassed);

            foreach (var fundingStreamCode in fundingStreams.Keys)
            {
                var fundingStream = fundingStreams[fundingStreamCode];
                var publication = fundingStream.GetLatestPublication(previewModeEnabled, publishedDate);

                if (publication == null)
                {
                    continue;
                }

                var cutOffDate = FundingPeriodHelper.GetCutOffDateForPublication(publication);
                var fundingPeriodCodes = new List<string>();

                if (!string.IsNullOrEmpty(fundingPeriodCode))
                {
                    fundingPeriodCodes.Add(fundingPeriodCode);
                }
                else
                {
                    fundingPeriodCodes.Add(publication.FundingPeriodCode);
                }

                var fundingStreamsRequest = new List<FundingApiSearchFundingStream>
                {
                    new FundingApiSearchFundingStream
                    {
                        BeforeDateTime = cutOffDate,
                        FundingStreamCode = fundingStreamCode,
                        GroupingType = null,
                        PeriodCodes = fundingPeriodCodes.ToArray()
                    }
                };

                var searchFilterList = new List<SearchFilter>();

                if (!string.IsNullOrEmpty(groupingReason))
                {
                    searchFilterList.Add(new SearchFilter
                    {
                        PropertyName = SearchFilterPropertyName.GroupingReason,
                        PropertyValue = groupingReason
                    });
                }

                if (!string.IsNullOrEmpty(multipleUkPrnSearchList))
                {
                    searchFilterList.Add(new SearchFilter
                    {
                        PropertyName = SearchFilterPropertyName.PrimaryIdentifierList,
                        PropertyValue = multipleUkPrnSearchList
                    });
                }

                if (searchFilterList.Any())
                {
                    fundingStreamsRequest.First().Filters = searchFilterList.ToArray();
                }

                var requestObj = new FundingApiSearchRequestObject
                {
                    FundingStreams = fundingStreamsRequest.ToArray(),
                    SearchTerm = searchTerm,
                    WaitForIndexBuild = false,
                    BypassGrouping = byPassGrouping
                };

                var searchResponse = await GetApiService(fundingStreamCode, fundingStream.UseFakeApiService()).SearchProviderFunding(requestObj);

                // ParentProviderType isn't filterable in the search index, so we do it after
                var viewModelProviderResults = searchResponse?.ProviderFunding?
                    .Where(provider =>
                    {
                        var providerFundingStreamCode = provider.FundingStreamCode;
                        var parentProviderType = parentProviderTypes?.ContainsKey(providerFundingStreamCode) == true
                            ? parentProviderTypes[providerFundingStreamCode] : null;

                        if (parentProviderType == null)
                        {
                            parentProviderType = parentProviderTypes?.ContainsKey(PARENT_PROVIDER_TYPE_DEFAULT) == true
                                ? parentProviderTypes[PARENT_PROVIDER_TYPE_DEFAULT] : null;
                        }

                        return string.IsNullOrEmpty(parentProviderType) ||
                            parentProviderType.Equals(provider.ParentProviderType, StringComparison.InvariantCultureIgnoreCase) ||
                            CheckMixedLoggedInParentProviderType(provider, parentProviderTypes);
                    })
                    .ToList();

                if (viewModelProviderResults != null)
                {
                    returnList.AddRange(viewModelProviderResults);
                }
            }

            return returnList;
        }

        /// <summary>
        /// Does the funding search.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="fundingStreams">The funding stream configuration.</param>
        /// <param name="groupingReason">The grouping reason to find.</param>
        /// <param name="publishedDate">The published date.</param>
        /// <returns>The search provider result list.</returns>
        /// <exception cref="ArgumentOutOfRangeException">There are no publications for the date {publishedDate}.</exception>
        protected async Task<List<IFundingApiSearchFunding>> DoFundingSearch(
            string searchTerm,
            Dictionary<string, FundingStream> fundingStreams,
            string groupingReason,
            DateTime? publishedDate = null)
        {
            var returnList = new List<IFundingApiSearchFunding>();

            foreach (var fundingStreamCode in fundingStreams.Keys)
            {
                var fundingStream = fundingStreams[fundingStreamCode];
                var publication = fundingStream.GetLatestPublication(false, publishedDate);

                if (publication == null)
                {
                    continue;
                }

                var cutOffDate = FundingPeriodHelper.GetCutOffDateForPublication(publication);
                var activeFundingPeriodCodes =
                    FundingPeriodHelper.GetActiveFundingPeriodCodes(fundingStream, await PreviewModeEnabled());
                var fundingPeriodCodes = new List<string>
                {
                    FundingPeriodHelper.GetLatestFundingPeriodCodes_FundingPeriodFormat(
                        fundingStream.SettingValues.ToList(), activeFundingPeriodCodes).First()
                };

                var fundingStreamsRequest = new List<FundingApiSearchFundingStream>
                {
                    new FundingApiSearchFundingStream
                    {
                        BeforeDateTime = cutOffDate,
                        FundingStreamCode = fundingStreamCode,
                        GroupingType = null,
                        PeriodCodes = fundingPeriodCodes.ToArray()
                    }
                };

                var searchFilterList = new List<SearchFilter>();

                if (!string.IsNullOrEmpty(groupingReason))
                {
                    searchFilterList.Add(new SearchFilter
                    {
                        PropertyName = SearchFilterPropertyName.GroupingReason,
                        PropertyValue = groupingReason
                    });
                }

                if (searchFilterList.Any())
                {
                    fundingStreamsRequest.First().Filters = searchFilterList.ToArray();
                }

                var requestObj = new FundingApiSearchRequestObject
                {
                    FundingStreams = fundingStreamsRequest.ToArray(),
                    SearchTerm = searchTerm,
                    WaitForIndexBuild = false
                };

                var searchResponse = await GetApiService(fundingStreamCode, fundingStream.UseFakeApiService())
                    .SearchFunding(requestObj);

                var viewModelProviderResults = searchResponse?.Funding?.ToList();

                if (viewModelProviderResults != null)
                {
                    returnList.AddRange(viewModelProviderResults);
                }
            }

            return returnList;
        }

        /// <summary>
        /// Does the funding search.
        /// </summary>
        /// <param name="id">The funding id to search for.</param>
        /// <param name="fundingStreamCode">The funding stream code.</param>
        /// <param name="useStaticData">Use static Data.</param>
        /// <returns>The matched funding.</returns>
        /// <exception cref="ArgumentOutOfRangeException">There are no publications for the date {publishedDate}.</exception>
        protected async Task<IFundingApiSearchFunding> GetFunding(string id, string fundingStreamCode, bool useStaticData)
        {
            return await GetApiService(fundingStreamCode, useStaticData).GetFunding(id);
        }

        /// <summary>
        /// Does the provider funding search.
        /// </summary>
        /// <param name="id">The provider funding id to search for.</param>
        /// <param name="fundingStreamCode">The funding stream code.</param>
        /// <param name="useStaticData">Use static data.</param>
        /// <returns>The matched provider funding.</returns>
        /// <exception cref="ArgumentOutOfRangeException">There are no publications for the date {publishedDate}.</exception>
        protected async Task<IFundingApiSearchProviderFunding> GetProviderFunding(string id, string fundingStreamCode, bool useStaticData)
        {
            return await GetApiService(fundingStreamCode, useStaticData).GetProviderFunding(id);
        }

        /// <summary>
        /// Get user funding view details.
        /// </summary>
        /// <param name="userId">The user id to search for.</param>
        /// <param name="fundings">The funding ids to match on.</param>
        /// <returns>The result representing the count.</returns>
        protected async Task<IUserFundingViewCountResponse> GetUserFundingCount(string userId, List<IFundingApiSearchProviderFunding> fundings)
        {
            var result = new UserFundingViewCountResponse { UserId = userId };

            if (fundings != null)
            {
                var fundingStreamGroups = fundings.GroupBy(funding => funding.FundingStreamCode);

                foreach (var group in fundingStreamGroups)
                {
                    var fundingVersionDetails = group.AsQueryable().Select(funding => new FundingVersionDetail { FundingId = funding.Id, StatementChannelVersion = funding.StatementChannelVersion })
                        .Distinct().ToList();
                    var viewsCount = new UserFundingViewCountResponse();
                    if (fundingVersionDetails.Any())
                    {
                        viewsCount = await GetApiService(@group.Key, false).GetUserFundingViewCount(userId, fundingVersionDetails);
                    }

                    result.UnreadNewFundings += viewsCount.UnreadNewFundings;
                    result.UnreadUpdatedFundings += viewsCount.UnreadUpdatedFundings;
                }
            }

            return result;
        }

        /// <summary>
        /// Get the file size for the funding document.
        /// </summary>
        /// <param name="fundingStreamConfiguration">Funding stream configuration.</param>
        /// <param name="settingKey">The setting key to lookup.</param>
        /// <returns>Approximate file size in bytes.</returns>
        protected int? GetFileDownloadSizeInBytes(FundingStream fundingStreamConfiguration, string settingKey)
        {
            var setting = fundingStreamConfiguration.SettingValues?
                .FirstOrDefault(settingValue => settingValue.Setting.SettingName == settingKey)?.Value;

            if (int.TryParse(setting, out var settingInt))
            {
                return settingInt;
            }

            return null;
        }

        /// <summary>
        /// Convert a services funding stream to a web model funding stream.
        /// </summary>
        /// <param name="fundingStream">The services funding stream to convert.</param>
        /// <returns>A web model funding stream.</returns>
        protected Models.FundingStream.FundingStream AsWebModel(FundingStream fundingStream)
        {
            return FundingStreamHelper.AsWebModel(fundingStream, _mapper);
        }

        /// <summary>
        /// Convert a services funding stream to a web admin model funding stream.
        /// </summary>
        /// <param name="fundingStream">The services funding stream to convert.</param>
        /// <returns>A web admin model funding stream.</returns>
        protected Areas.Admin.Models.FundingStream.FundingStream AsWebAdminModel(FundingStream fundingStream)
        {
            return FundingStreamHelper.AsWebAdminModel(fundingStream, _mapper);
        }

        /// <summary>
        /// Convert a dictionary of services funding streams to web model funding streams.
        /// </summary>
        /// <param name="fundingStreams">The funding streams to convert.</param>
        /// <returns>A dictionary of funding streams.</returns>
        protected Dictionary<string, Models.FundingStream.FundingStream> AsWebModel(
            Dictionary<string, FundingStream> fundingStreams)
        {
            return FundingStreamHelper.AsWebModel(fundingStreams, _mapper);
        }

        /// <summary>
        /// Convert an ienumerable of services funding stream to web model funding streams.
        /// </summary>
        /// <param name="fundingStreams">The funding streams to convert.</param>
        /// <returns>An ienumerable list of funding streams.</returns>
        protected IEnumerable<Models.FundingStream.FundingStream> AsWebModel(
            IEnumerable<FundingStream> fundingStreams)
        {
            return FundingStreamHelper.AsWebModel(fundingStreams, _mapper);
        }

        /// <summary>
        /// Gets the funding stream code.
        /// </summary>
        /// <param name="fundingStreamNamePathPath">The funding stream name path path.</param>
        /// <param name="fundingStreams">The funding streams.</param>
        /// <returns>The funding stream code.</returns>
        protected string GetFundingStreamCode(string fundingStreamNamePathPath, Dictionary<string, FundingStream> fundingStreams)
        {
            foreach (var (key, value) in fundingStreams)
            {
                var loopFundingStreamNamePathPath = value?.FundingStreamName?.ToUIPathComponent();

                if (loopFundingStreamNamePathPath != null && loopFundingStreamNamePathPath.Equals(fundingStreamNamePathPath, StringComparison.InvariantCultureIgnoreCase))
                {
                    return key;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the matching funding for this provider.
        /// </summary>
        /// <param name="organisationUkprn">The provider's UKPRN.</param>
        /// <param name="fundingStream">The funding stream.</param>
        /// <param name="publication">The publication.</param>
        /// <returns>Task IFundingApiSearchResponseProviderFunding.</returns>
        protected Task<IFundingApiSearchResponseProviderFunding> ProviderFundingMatchesTask(
            string organisationUkprn,
            FundingStream fundingStream,
            Publication publication)
        {
            return _fundingApiService.SearchProviderFunding(
                new FundingApiSearchRequestObject
                {
                    FundingStreams = new[]
                {
                    new FundingApiSearchFundingStream
                    {
                        FundingStreamCode = fundingStream.FundingStreamCode,
                        BeforeDateTime = FundingPeriodHelper.GetCutOffDateForPublication(publication),
                        PeriodCodes = new[] { publication.FundingPeriodCode },
                        GroupingType = GroupingType.LocalAuthority,
                        Filters = new[]
                        {
                            new SearchFilter
                            {
                                PropertyName = SearchFilterPropertyName.PrimaryIdentifier,
                                PropertyValue = organisationUkprn
                            },
                            new SearchFilter
                            {
                                PropertyName = SearchFilterPropertyName.GroupingReason,
                                PropertyValue = "Information"
                            }
                        }
                    }
                },
                    BypassGrouping = true
                });
        }

        protected List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>> GroupPublicationsByYear(
            List<(int yearFrom, int yearTo)> yearsToInclude, FundingStream fundingStream, int currentYear)
        {
            var fundingPeriodPublications = new List<KeyValuePair<(int, int), List<Publication>>>();

            foreach (var (yearFrom, yearTo) in yearsToInclude)
            {
                var settingValues = fundingStream.SettingValues;

                if (HistoricAllocationsAreExternal(fundingStream, yearFrom, currentYear))
                {
                    fundingPeriodPublications.Add(
                        new KeyValuePair<(int, int), List<Publication>>((yearFrom, yearTo), new List<Publication>()));

                    continue;
                }

                var yearTypeCode = FundingPeriodHelper.GetYearSettingCode(fundingStream.SettingValues);
                var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(yearFrom, yearTo, yearTypeCode);

                var publicationsForYear = fundingStream.Publications
                    .Where(p => p.FundingPeriodCode == fundingPeriodCode)
                    .ToList();

                fundingPeriodPublications.Add(
                    new KeyValuePair<(int, int), List<Publication>>((yearFrom, yearTo), publicationsForYear));
            }

            return fundingPeriodPublications;
        }

        /// <summary>
        /// Gets the UKPRN for the current user.
        /// </summary>
        /// <param name="userDetails">The user details.</param>
        /// <returns>The UKPRN.</returns>
        protected string GetUkprn(User userDetails)
        {
            return userDetails.Ukprn.ToString();
        }

        /// <summary>
        /// Gets the UKPRN from the provider fundings string.
        /// </summary>
        /// <param name="providerFunding">The provider funding.</param>
        /// <returns>The UKPRN.</returns>
        protected string GetUkprnFromProviderFunding(string providerFunding)
        {
            var providerFundingParts = providerFunding.Split('-');
            if (providerFundingParts.Length > 3 && providerFundingParts[3].Length == 8)
            {
                return providerFundingParts[3];
            }

            return string.Empty;
        }

        /// <summary>
        /// Checks the MAT status of the current User.
        /// </summary>
        /// <param name="userDetails">The user to check.</param>
        /// <param name="fundingUiViewType">The funding UI view type.</param>
        /// <returns>True if the user's organisation is a MAT, otherwise false.</returns>
        protected async Task<bool> CheckMatStatus(User userDetails, FundingUIViewType fundingUiViewType)
        {
            var fundingStreams = await GetRelevantFundingStreams(fundingUiViewType);
            var dataRequirements = await GetDataRequirements(fundingStreams, userDetails, true);
            var fundingRequestObject = SquashDataRequirements(dataRequirements, "Funding");
            var fundingData = await _fundingApiService.SearchFunding(fundingRequestObject);

            // if there are no provider fundings it's not a MAT.
            if (fundingData?.Funding?.All(funding => funding.ProviderFundings == null || funding.ProviderFundings.Count() == 0) == true)
            {
                return false;
            }

            // if all provider fundings have the same ukprn as the user then it's a single academy not a MAT.
            if (fundingData?.Funding?.All(funding =>
                    funding.ProviderFundings?.All(providerFunding =>
                        GetUkprnFromProviderFunding(providerFunding) == userDetails.Ukprn.ToString()) == true) == true)
            {
                return false;
            }

            return fundingData?.Funding?.Any(funding =>
                       funding.GroupingType?.Equals(GroupingType.AcademyTrust, StringComparison.InvariantCultureIgnoreCase) ==
                       true) == true;
        }

        protected async Task<string> GetProviderName(User userDetails)
        {
            string providerName = null;
            var activeFundingStreams = await GetActiveFundingStreams();
            var fundingStreams = new Dictionary<string, FundingStream>();
            foreach (var fundingstream in activeFundingStreams)
            {
                fundingStreams.TryAdd(fundingstream.FundingStreamCode, fundingstream);
            }

            var userUkprn = GetUkprn(userDetails);
            if (!string.IsNullOrWhiteSpace(userUkprn))
            {
                var fundingData = await DoFundingSearch(userUkprn, fundingStreams, string.Empty);
                providerName = fundingData?.FirstOrDefault(fd => userUkprn.Equals(fd.GroupUkprn, StringComparison.OrdinalIgnoreCase))?.GroupName;

                if (string.IsNullOrWhiteSpace(providerName))
                {
                    var providerFundingData = await DoProviderFundingSearch(userUkprn, fundingStreams, GetProviderParentGroupingType(fundingStreams), string.Empty);
                    providerName = providerFundingData?.FirstOrDefault(pd => pd.OrganisationUkprn == userUkprn)?.OrganisationName;
                }
            }

            return providerName;
        }

        protected async Task<IEnumerable<FundingApiSearchRequestObject>> GetDataRequirements(
            Dictionary<string, FundingStream> fundingStreams,
            User currentUserDetails = null,
            bool currentUserPassed = false,
            string currentUserUkprn = null,
            bool isLaSsf = false)
        {
            var dataRequirements = new List<FundingApiSearchRequestObject>();
            var ukprn = currentUserDetails != null ? GetUkprn(currentUserDetails) : currentUserUkprn;
            var previewMode = await PreviewModeEnabled(currentUserDetails, currentUserPassed);

            foreach (var fundingStreamCode in fundingStreams.Keys)
            {
                var fundingStream = fundingStreams[fundingStreamCode];
                var publication = fundingStream.GetLatestPublication(previewMode);

                if (publication == null)
                {
                    continue;
                }

                var cutOffDate = FundingPeriodHelper.GetCutOffDateForPublication(publication);

                var filters = new[]
                {
                    new SearchFilter
                    {
                        PropertyName = SearchFilterPropertyName.Ukprn,
                        PropertyValue = ukprn
                    }
                };

                dataRequirements.AddRange(_fundingViewService.GetDataRequirements(
                    fundingStream,
                    publication.FundingPeriodCode,
                    cutOffDate,
                    FundingViewScope.OrganisationSummary,
                    filters,
                    null,
                    null,
                    isLaSsf ? GroupingType.LocalAuthoritySsf : GroupingType.AcademyTrust));
            }

            return dataRequirements;
        }

        /// <summary>
        /// Get the API service for funding.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. PSG).</param>
        /// <param name="useFakeApiService">Whether to use the fake Api Service.</param>
        /// <returns>A funding API service to use.</returns>
        protected IFundingApiService GetApiService(string fundingStreamCode, bool useFakeApiService)
        {
            if (!useFakeApiService)
            {
                return _fundingApiService;
            }

            // TODO - eventually remove the following 2 if statements
            if (fundingStreamCode.Equals("GAG", StringComparison.InvariantCultureIgnoreCase))
            {
                return new LocalGAG_FakeApiService(_httpApiService);
            }

            if (fundingStreamCode.Equals("1619", StringComparison.InvariantCultureIgnoreCase))
            {
                return new Local1619_FakeApiService(_httpApiService);
            }

            if (fundingStreamCode.Equals("NMSS", StringComparison.InvariantCultureIgnoreCase))
            {
                return new LocalNMSS_FakeApiService(_httpApiService);
            }

            if (fundingStreamCode.Equals("1416", StringComparison.InvariantCultureIgnoreCase))
            {
                return new Local1416_FakeApiService(_httpApiService);
            }

            if (fundingStreamCode.Equals("LAREC", StringComparison.InvariantCultureIgnoreCase))
            {
                return new LocalLAREC_FakeApiService(_httpApiService);
            }

            if (fundingStreamCode.Equals("PNA", StringComparison.InvariantCultureIgnoreCase))
            {
                return new LocalPNA_FakeApiService(_httpApiService);
            }

            if (fundingStreamCode.Equals("UIFSM", StringComparison.InvariantCultureIgnoreCase))
            {
                return new LocalUIFSM_FakeApiService(null, _fundingApiService);
            }

            return _fundingApiService;
        }

        private static Dictionary<ComponentType, Defaults> _componentDefaults;

        private static Dictionary<ComponentType, Defaults> GetDefaultsUsingReflection()
        {
            var enumType = typeof(ComponentType);
            var allMembers = enumType.GetMembers();

            var returnList = new Dictionary<ComponentType, Defaults>();

            foreach (var componentType in (ComponentType[])Enum.GetValues(typeof(ComponentType)))
            {
                var componentDefaults = GetDefaultsAttribute(enumType, allMembers, componentType.ToString());

                if (componentDefaults != null)
                {
                    returnList.Add(componentType, componentDefaults);
                }
            }

            return returnList;
        }

        private static Defaults GetDefaultsAttribute(Type enumType, MemberInfo[] allMembers, string typeString)
        {
            var enumValueMemberInfo = allMembers.FirstOrDefault(m => m.Name == typeString && m.DeclaringType == enumType);
            var customAttributes = enumValueMemberInfo?.GetCustomAttributes(typeof(Defaults), false);

            return customAttributes?.Any() == true
                ? customAttributes.Select(att => (Defaults)att).FirstOrDefault()
                : null;
        }

        private static void BuildFundingStreamDictionary(Dictionary<string, FundingStream> fundingStreams, Dictionary<string, string> returnList)
        {
            foreach (var fundingStream in fundingStreams)
            {
                var specificParentProviderType = fundingStream.Value.SettingValues
                    .FirstOrDefault(settingValue => settingValue.Setting.SettingName == "ParentProviderType")?
                    .Value;

                if (string.IsNullOrEmpty(specificParentProviderType))
                {
                    continue;
                }

                returnList.Add(fundingStream.Key, specificParentProviderType);
            }
        }

        private bool CheckMixedLoggedInParentProviderType(IFundingApiSearchProviderFunding provider, Dictionary<string, string> parentProviderTypes)
        {
            if (parentProviderTypes?.ContainsKey(PARENT_PROVIDER_TYPE_MIXED) == true)
            {
                return provider.GroupingReason == GroupingReason.Indicative && provider.ParentProviderType == GroupingType.Provider;
            }

            return false;
        }
    }
}