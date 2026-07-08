#define vyfv2
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Utils;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Cache;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Attributes;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.Requests;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Exceptions;
using PDS.ViewYourFunding.Web.Helpers;
using PDS.ViewYourFunding.Web.Models.Request;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProviderFundingBreakdownViewModel = PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.ProviderFundingBreakdownViewModel;
using ProviderHistoryViewModel = PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.ProviderHistoryViewModel;
using ProviderStatementViewModel = PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.ProviderStatementViewModel;
using User = Pds.Core.Common.Identity.Models.User;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Controllers
{
    /// <summary>
    /// The UI controller for logged in providers.
    /// </summary>
    [Area("LoggedIn")]
    [Authorize(nameof(UserRole.ViewAllocationStatements))]
    [ServiceFilter(typeof(ProviderViewToggledCheckAttribute))]
    public class ProviderController : BaseFundingController
    {
        /// <summary>
        /// The funding version one.
        /// </summary>
        private const string FundingVersionOne = "1_0";

        /// <summary>
        /// The feedback link.
        /// </summary>
        private readonly string _feedbackLink;

        /// <summary>
        /// The contact us link.
        /// </summary>
        private readonly string _contactUsLink;

        /// <summary>
        /// The home link.
        /// </summary>
        private readonly string _homeLink;

        /// <summary>
        /// The choice page link.
        /// </summary>
        private readonly string _choicePageLink;

        /// <summary>
        /// The funding view service.
        /// </summary>
        private readonly IFundingViewService _fundingViewService;

        /// <summary>
        /// The component service.
        /// </summary>
        private readonly IComponentService _componentService;

        /// <summary>
        /// The funding api service.
        /// </summary>
        private readonly IFundingApiService _fundingApiService;

        /// <summary>
        /// The system provider.
        /// </summary>
        private readonly ISystemProvider _systemProvider;

        private readonly ICacheService _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderController"/> class.
        /// </summary>
        /// <param name="componentService">The component service to use.</param>
        /// <param name="securityService">The security service to use.</param>
        /// <param name="settingsService">The settings service to use.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="fundingApiService">The API service to use for searching for funding.</param>
        /// <param name="fundingViewService">The funding view service.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="globalSettingService">The global setting service to use.</param>
        /// <param name="applicationConfigurationOptions">The configuration service.</param>
        /// <param name="systemProvider">The system provider.</param>
        public ProviderController(
            IComponentService componentService,
            IClaimsBasedIdentityService securityService,
            IUserJourneyService settingsService,
            IMapper mapper,
            IFundingApiService fundingApiService,
            IFundingViewService fundingViewService,
            ICacheService cacheService,
            IGlobalSettingService globalSettingService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            ISystemProvider systemProvider)
            : base(
                  securityService,
                  applicationConfigurationOptions,
                  settingsService,
                  mapper,
                  fundingApiService,
                  cacheService,
                  fundingViewService,
                  globalSettingService)
        {
            _componentService = componentService;
            _fundingViewService = fundingViewService;
            _cacheService = cacheService;
            _contactUsLink = applicationConfigurationOptions.Value.ContactUsLink;
            _feedbackLink = applicationConfigurationOptions.Value.FeedbackLinkForLoggedInView;
            _homeLink = applicationConfigurationOptions.Value.LoggedInProviderHomeLink;
            _fundingApiService = fundingApiService;
            _systemProvider = systemProvider;

            var homeLinkIsAbsolute = _homeLink.Contains("http", StringComparison.InvariantCultureIgnoreCase);

            if (homeLinkIsAbsolute)
            {
                var homeLinkUri = new Uri(_homeLink, UriKind.Absolute);
                _choicePageLink = new UriBuilder(homeLinkUri.Scheme, homeLinkUri.Host, homeLinkUri.Port, "choose-a-statement-type")
                    .ToString().Replace(":80/", "/").Replace(":443/", "/");
            }
            else
            {
                _choicePageLink = "/choose-a-statement-type";
            }
        }

        /// <summary>
        /// The MVC action for the provider statement page.
        /// </summary>
        /// <param name="viaChoicePage">Entered via the allocation choice page.</param>
        /// <returns>The start page view.</returns>
#if !vyfv2
        [Route(LoggedInConstants.Route_ProviderStatement, Name = LoggedInConstants.RouteName_ProviderStatement)]
#endif
        public async Task<IActionResult> ProviderStatement(bool viaChoicePage = false)
        {
            var userDetails = await GetUserAsync();

            if (await CheckMatStatus(userDetails, Enums.FundingUIViewType.Providers_LoggedIn))
            {
                if (viaChoicePage)
                {
                    return RedirectToRoute(LoggedInConstants.RouteName_MultipleAcademyTrustStatement, new { viaChoicePage = viaChoicePage });
                }

                return RedirectToRoute(LoggedInConstants.RouteName_MultipleAcademyTrustStatement);
            }

            var viewModel = await GetBasePageViewModel<ProviderStatementViewModel>(userDetails, true);
            viewModel.FromMatStatementsPage = await CheckMatStatus(userDetails, Enums.FundingUIViewType.Providers_LoggedIn);
            viewModel.ContactUsLink = _contactUsLink;
            viewModel.FeedbackLink = _feedbackLink;
            viewModel.HomeLink = _homeLink;
            viewModel.ViaChoicePage = viaChoicePage;
            viewModel.ChoicePageLink = _choicePageLink;

            var fundingViewDetail = await GetFundingViewDetails();
            var ukprn = GetUkprn(userDetails);
            var providerFundings = await DoProviderFundingSearch(
                ukprn,
                fundingViewDetail.FundingStreams,
                GetProviderParentGroupingTypeWithIndicative(fundingViewDetail.FundingStreams),
                string.Empty,
                byPassGrouping: true);

            var providerFundingViewDataRequests = new List<FundingViewData>();

            var fundingStreamCodeGroups = providerFundings.GroupBy(provFunding => provFunding.FundingStreamCode);

            foreach (var group in fundingStreamCodeGroups)
            {
                var uniqueProviderFundingList = GetDistinctProviderFundings(group.AsEnumerable());
                var providerFunding = uniqueProviderFundingList.OrderByDescending(provFunding => provFunding.StatusChangedDate)
                    .First();
                var fundingViewScope = GetProviderSummaryFundingViewScope(providerFunding);

                var isFundingUpdated = !providerFunding.IsFirstStatementChannelVersion;
                var isInitialFunding = providerFunding.IsFirstStatementChannelVersion;

                var fundingStream = fundingViewDetail.FundingStreams[providerFunding.FundingStreamCode];
                var publication = fundingStream.GetLatestPublication(await PreviewModeEnabled());

                if (publication == null)
                {
                    continue;
                }

                var filters = new[]
                {
                    new SearchFilter
                    {
                        PropertyName = SearchFilterPropertyName.PrimaryIdentifier,
                        PropertyValue = providerFunding.OrganisationUkprn
                    }
                };

                var fundingDocumentFileType = fundingStream.SettingValues.FirstOrDefault(sv =>
                    sv.Setting.SettingName == "FundingDocumentFileType")?.Value;

                if (string.IsNullOrEmpty(fundingDocumentFileType))
                {
                    fundingDocumentFileType = FundingDocumentFileType.Spreadsheet_OpenFormat;
                }

                var publishedDate = fundingStream.HistoryIndependentOfPublications ? providerFunding.StatusChangedDate : publication.PublishedDate;
                var fundingDocument = GetProviderFundingDocument(
                    providerFunding,
                    publishedDate,
                    fundingStream,
                    true,
                    fundingDocumentFileType);

                var hasUserVisitedFunding = false;

                if (isFundingUpdated || isInitialFunding)
                {
                    hasUserVisitedFunding = await _fundingApiService.HasUserVisitedFunding(userDetails.Principal, providerFunding.Id);
                }

                var cacheKey =
                    $"{publication.FundingPeriodCode}-{fundingViewScope}-{fundingStream.FundingStreamCode}-{ukprn}-{FundingPeriodHelper.GetCutOffDateForPublication(publication)}-{hasUserVisitedFunding}";

                var fundingViewData = await _cacheService.AddOrGetExistingResultAsync(
                    cacheKey,
                    () => _fundingViewService.GenerateFundingViewData(
                        _componentService,
                        publication.FundingPeriodCode,
                        fundingStream.FundingStreamCode,
                        fundingViewDetail.FundingStreams.Values.ToArray(),
                        FundingPeriodHelper.GetCutOffDateForPublication(publication),
                        publication,
                        publication.UIModelVersion,
                        fundingViewScope,
                        GetComponentDefaults(),
                        fundingDocument,
                        true,
                        true,
                        filters: filters,
                        searchTerm: null,
                        bubbleUpException: false,
                        iFundingApiSearchProviderFunding: uniqueProviderFundingList?.ToArray(),
                        explicitProviderFundingPassed: true,
                        viaChoicePage: viewModel.ViaChoicePage,
                        showSelectors: fundingViewDetail.ShowSelectors,
                        asStatementSpecification: fundingViewDetail.StatementSpecificationState,
                        showData: fundingViewDetail.ShowData),
                    CacheExpirationPolicy.Sliding);

                providerFundingViewDataRequests.Add(fundingViewData);
            }

            foreach (var fundingViewDataResponse in providerFundingViewDataRequests)
            {
                if (fundingViewDataResponse == null || viewModel.ProviderFundingViewData.ContainsKey(fundingViewDataResponse.FundingStreamCode))
                {
                    continue;
                }

                viewModel.ProviderFundingViewData.Add(fundingViewDataResponse.FundingStreamCode, fundingViewDataResponse);
            }

            var fundingViewDataRequests = await GetLocalAuthorityStatement(viaChoicePage, userDetails, fundingViewDetail);

            viewModel.OrganisationName = providerFundings.FirstOrDefault()?.OrganisationName;

            foreach (var fundingViewDataResponse in fundingViewDataRequests)
            {
                if (fundingViewDataResponse == null || viewModel.FundingViewData.ContainsKey(fundingViewDataResponse.FundingStreamCode))
                {
                    continue;
                }

                viewModel.FundingViewData.Add(fundingViewDataResponse.FundingStreamCode, fundingViewDataResponse);

                if (string.IsNullOrEmpty(viewModel.OrganisationName))
                {
                    viewModel.OrganisationName = fundingViewDataResponse.EntityName;
                }
            }

            viewModel.ProviderUrn = providerFundings.FirstOrDefault()?.ProviderUrn;
            viewModel.DisplayNoAllocationMessage = !viewModel.ProviderFundingViewData.Any() && !viewModel.FundingViewData.Any();

            return View(viewModel);
        }

        /// <summary>
        /// The Logged in variance selection action.
        /// </summary>
        /// <param name="providerFundingBreakdownRequest">The provider funding breakdown request.</param>
        /// <returns>The MVC View result.</returns>
        //[Route(LoggedInConstants.Route_VarianceSelection, Name = LoggedInConstants.RouteName_VarianceSelection)]
        [Obsolete("Comparison View is hidden for GAG digital MVS Go Live.")]
        public async Task<IActionResult> VarianceSelection(ProviderFundingBreakdownRequest providerFundingBreakdownRequest)
        {
            var userDetails = await GetUserAsync();
            var currentUserOrganisationUkprn = GetUkprn(userDetails);

            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Providers_LoggedIn);

            var fundingStreamCode =
                GetFundingStreamCode(providerFundingBreakdownRequest.FundingStreamNamePathPart, fundingStreams);

            if (fundingStreamCode == null || !fundingStreams.TryGetValue(
                fundingStreamCode,
                out var fundingStream))
            {
                throw new Exception($"Funding stream not found for {providerFundingBreakdownRequest.FundingStreamNamePathPart}");
            }

            var organisationUkprn = !string.IsNullOrWhiteSpace(providerFundingBreakdownRequest.Ukprn)
                ? providerFundingBreakdownRequest.Ukprn
                : currentUserOrganisationUkprn;

            var publishedDate = providerFundingBreakdownRequest.PublishedDate?.ToRouteParameterDate();

            var searchFundingStreamConfiguration = new Dictionary<string, FundingStream>
            {
                {
                    fundingStreamCode,
                    fundingStream
                }
            };

            var results = await DoProviderFundingSearch(
                organisationUkprn,
                searchFundingStreamConfiguration,
                GetProviderParentGroupingType(fundingStreams),
                string.Empty,
                byPassGrouping: true);

            results = GetDistinctProviderFundings(results);

            if (results.Count() == 1)
            {
                return RedirectToAction(nameof(ProviderFundingBreakDown), providerFundingBreakdownRequest);
            }

            var viewModel = await GetBasePageViewModel<VarianceSelectionViewModel>(userDetails, true);

            viewModel.Options = new List<VarianceSelectionOption>();

            var providerFundingsApplicable = results.Where(provFunding => provFunding.StatusChangedDate.Date <= publishedDate).OrderByDescending(r => r.StatusChangedDate);
            var groupedResults = providerFundingsApplicable.GroupBy(provFunding => provFunding.FundingPeriodCode).OrderByDescending(r => r.Key);
            var currentYearFundings = groupedResults.First();

            if (groupedResults.Count() > 1)
            {
                viewModel.Options.Add(VarianceSelectionOption.FinalStatementPreviousYear);
                viewModel.YearPrevious = providerFundingBreakdownRequest.YearFrom - 1;

                viewModel.FinalStatementPublishedDate = groupedResults.Skip(1).First().Max(p => p.StatusChangedDate);
            }

            if (currentYearFundings.Count() > 1)
            {
                var latestStatusChangeDate = currentYearFundings.Max(p => p.StatusChangedDate);
                var previousCurrentYearFundings = currentYearFundings.Where(provFunding => provFunding.StatusChangedDate < latestStatusChangeDate);

                if (previousCurrentYearFundings.Any())
                {
                    viewModel.Options.Add(VarianceSelectionOption.PreviousStatementCurrentYear);
                    viewModel.PreviousStatementPublishedDate = previousCurrentYearFundings.First().StatusChangedDate;
                }
            }

            if (!viewModel.Options.Any())
            {
                return RedirectToAction(nameof(ProviderFundingBreakDown), providerFundingBreakdownRequest);
            }

            viewModel.Options.Add(VarianceSelectionOption.NoComparison);

            viewModel.FromMatStatementsPage = await CheckMatStatus(userDetails, Enums.FundingUIViewType.Providers_LoggedIn);
            viewModel.ContactUsLink = _contactUsLink;
            viewModel.FeedbackLink = _feedbackLink;
            viewModel.HomeLink = _homeLink;
            viewModel.ViaChoicePage = providerFundingBreakdownRequest.ViaChoicePage;
            viewModel.ChoicePageLink = _choicePageLink;
            viewModel.YearFrom = providerFundingBreakdownRequest.YearFrom;
            viewModel.YearTo = providerFundingBreakdownRequest.YearTo;
            viewModel.FundingStreamNamePathPart = providerFundingBreakdownRequest.FundingStreamNamePathPart;
            viewModel.FundingStream = fundingStream;
            viewModel.Ukprn = providerFundingBreakdownRequest.Ukprn;
            viewModel.Tab = providerFundingBreakdownRequest.Tab;
            viewModel.ViaChoicePage = providerFundingBreakdownRequest.ViaChoicePage;
            viewModel.IncludeHistory = providerFundingBreakdownRequest.IncludeHistory;
            viewModel.PublishedDate = providerFundingBreakdownRequest.PublishedDate;
            viewModel.ViaVariancePage = true;

            return View(viewModel);
        }

        /// <summary>
        /// The Logged in variance selection action.
        /// </summary>
        /// <param name="providerFundingBreakdownRequest">The provider funding breakdown request.</param>
        /// <returns>The MVC View result.</returns>
        //[HttpPost]
        //[Route(LoggedInConstants.Route_VarianceSelection, Name = LoggedInConstants.RouteName_VarianceSelection)]
        [Obsolete("Comparison View is hidden for GAG digital MVS Go Live.")]
        public IActionResult VarianceSelectionPost(ProviderFundingBreakdownRequest providerFundingBreakdownRequest)
        {
            return RedirectToAction(nameof(ProviderFundingBreakDown), providerFundingBreakdownRequest);
        }

        /// <summary>
        /// The Logged in provider funding breakdown action.
        /// </summary>
        /// <param name="providerFundingBreakdownRequest">The provider funding breakdown request.</param>
        /// <returns>The MVC View result.</returns>
#if !vyfv2
        [Route(
            LoggedInConstants.Route_ProviderFundingBreakdown,
            Name = LoggedInConstants.RouteName_ProviderFundingBreakdown)]
#endif
        public async Task<IActionResult> ProviderFundingBreakDown(ProviderFundingBreakdownRequest providerFundingBreakdownRequest)
        {
            var userDetails = await GetUserAsync();
            var currentUserOrganisationUkprn = GetUkprn(userDetails);

            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Providers_LoggedIn);

            var fundingStreamCode =
                GetFundingStreamCode(providerFundingBreakdownRequest.FundingStreamNamePathPart, fundingStreams);

            if (fundingStreamCode == null || !fundingStreams.TryGetValue(
                fundingStreamCode,
                out var fundingStream))
            {
                throw new Exception($"Funding stream not found for {providerFundingBreakdownRequest.FundingStreamNamePathPart}");
            }

            var yearTypeCode = FundingPeriodHelper.GetYearSettingCode(fundingStream.SettingValues);
            var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(providerFundingBreakdownRequest.YearFrom, providerFundingBreakdownRequest.YearTo, yearTypeCode);

            var viewModel = await GetBasePageViewModel<ProviderFundingBreakdownViewModel>(userDetails, true);
            viewModel.FeedbackLink = _feedbackLink;
            viewModel.HomeLink = _homeLink;
            viewModel.FundingStream = fundingStream;

            var organisationUkprn = !string.IsNullOrWhiteSpace(providerFundingBreakdownRequest.Ukprn)
                ? providerFundingBreakdownRequest.Ukprn
                : currentUserOrganisationUkprn;
            if (currentUserOrganisationUkprn != organisationUkprn)
            {
                var orgIsPartOfMat = await IsPartOfMAT(fundingStreams, currentUserOrganisationUkprn, organisationUkprn, fundingStreamCode);
                if (!orgIsPartOfMat)
                {
                    viewModel.DisplayUnauthorisedAccessErrorMessage = true;
                    return View(viewModel);
                }
            }

            viewModel.FromMatStatementsPage = await CheckMatStatus(userDetails, Enums.FundingUIViewType.Providers_LoggedIn);
            viewModel.ContactUsLink = _contactUsLink;
            viewModel.ViaChoicePage = providerFundingBreakdownRequest.ViaChoicePage;
            viewModel.ChoicePageLink = _choicePageLink;
            viewModel.OrganisationUkPrn = organisationUkprn;
            viewModel.IncludeHistory = providerFundingBreakdownRequest.IncludeHistory;
            viewModel.FundingStreamNamePathPart = providerFundingBreakdownRequest.FundingStreamNamePathPart;
            viewModel.PublishedDate = providerFundingBreakdownRequest.PublishedDate;

            var publishedDate = providerFundingBreakdownRequest.PublishedDate?.ToRouteParameterDate();
            Publication publication = null;
            if (!fundingStream.HistoryIndependentOfPublications)
            {
                publication = fundingStream.GetLatestPublication(await PreviewModeEnabled(), publishedDate, fundingPeriodCode);
                if (publication == null)
                {
                    throw new RequestException(
                        $"Publications not found for {fundingStream.FundingStreamName}");
                }
            }

            var fundingViewData = new Dictionary<string, FundingViewData>();
            var searchFundingStreamConfigurations = new Dictionary<string, FundingStream>
            {
                {
                    fundingStreamCode,
                    fundingStream
                }
            };

            var results = await DoProviderFundingSearch(
                organisationUkprn,
                searchFundingStreamConfigurations,
                GetProviderParentGroupingTypeWithIndicative(fundingStreams),
                string.Empty,
                byPassGrouping: true);

            results = GetDistinctProviderFundings(results);
            var providerFunding = results.FirstOrDefault(x => x.StatusChangedDate.Date == publishedDate);
            if (providerFunding == null)
            {
                throw new RequestException(
                    $"Provider funding not found for {providerFundingBreakdownRequest.FundingStreamNamePathPart}");
            }

            var fundingViewScope = GetProviderBreakdownFundingViewScope(providerFunding);
            var latestDateForPeriod = results.Where(result => result.FundingPeriodCode == providerFunding.FundingPeriodCode).Max(p => p.StatusChangedDate);
            var isLatestOrFinalFundingForYear = latestDateForPeriod.Date == publishedDate;

            var activeFundingPeriodCodes = FundingPeriodHelper.GetActiveFundingPeriodCodes(fundingStream, await PreviewModeEnabled());

            var latestFundingPeriodCodes = FundingPeriodHelper.GetLatestFundingPeriodCodes_FundingPeriodFormat(fundingStream.SettingValues, activeFundingPeriodCodes);

            var filters = new[]
            {
                new SearchFilter
                {
                    PropertyName = SearchFilterPropertyName.PrimaryIdentifier,
                    PropertyValue = organisationUkprn
                }
            };

            var fundingDocumentFileType = fundingStream.SettingValues.FirstOrDefault(sv =>
                sv.Setting.SettingName == "FundingDocumentFileType")?.Value;

            var fundingDocument = GetProviderFundingDocument(
                providerFunding,
                publishedDate.GetValueOrDefault(),
                fundingStream,
                true,
                fundingDocumentFileType);

            var showSelectors = await GetShowSelectorsState();
            var asStatementSpecification = await GetStatementSpecificationState();
            var showData = await GetShowData();

            var providerFundingsApplicable = results.Where(provFunding => provFunding.StatusChangedDate.Date <= publishedDate).OrderByDescending(r => r.StatusChangedDate);

            var providerFundingsDetails = providerFundingBreakdownRequest.ViaVariancePage ?
                GetProviderFundingsForVariance(providerFundingsApplicable, providerFundingBreakdownRequest.SelectedVarianceOption ?? VarianceSelectionOption.NoComparison)
                : (null, providerFundingsApplicable.ToArray());

            var cacheKey =
                $"{publication?.FundingPeriodCode}-{fundingViewScope}-{fundingStream.FundingStreamCode}-{organisationUkprn}-{providerFundingBreakdownRequest.PublishedDate}";

            var fundingStreamData = await _cacheService.AddOrGetExistingResultAsync(
                cacheKey,
                () => _fundingViewService.GenerateFundingViewData(
                _componentService,
                fundingPeriodCode,
                fundingStream.FundingStreamCode,
                fundingStreams.Values.ToArray(),
                FundingPeriodHelper.GetCutOffDateForPublication(publication),
                publication,
                publication?.UIModelVersion,
                fundingViewScope,
                GetComponentDefaults(),
                fundingDocument,
                isLatestOrFinalFundingForYear,
                latestFundingPeriodCodes.Contains(providerFunding.FundingPeriodCode),
                providerFundingsDetails.Item1,
                filters,
                null,
                providerFundingBreakdownRequest.Tab,
                false,
                iFundingApiSearchProviderFunding: providerFundingsDetails.Item2,
                explicitProviderFundingPassed: true,
                selectedVarianceOption: providerFundingBreakdownRequest.ViaVariancePage ? providerFundingBreakdownRequest.SelectedVarianceOption ?? VarianceSelectionOption.NoComparison : VarianceSelectionOption.NoComparison,
                viaChoicePage: viewModel.ViaChoicePage,
                showSelectors: showSelectors,
                asStatementSpecification: asStatementSpecification,
                showData: showData),
                CacheExpirationPolicy.Sliding);

            fundingViewData.Add(providerFunding.FundingStreamCode, fundingStreamData);

            viewModel.OrganisationName = providerFunding.OrganisationName;
            viewModel.ProviderUrn = providerFunding.ProviderUrn;
            viewModel.YearFrom = providerFundingBreakdownRequest.YearFrom;
            viewModel.YearTo = providerFundingBreakdownRequest.YearTo;
            viewModel.Tab = providerFundingBreakdownRequest.Tab;
            viewModel.FundingViewData = fundingViewData.First().Value;
            viewModel.ViaVariancePage = providerFundingBreakdownRequest.ViaVariancePage;
            viewModel.IsIndicative = IsFundingStatusIndicative(providerFunding);

            await AddUserVisitDetails(userDetails.Principal, providerFunding.Id);

            var pageData = viewModel.FundingViewData?.Components?.FirstOrDefault()?.PageData;

            if (pageData?.ContainsKey("FileSizeBytes") == false)
            {
                pageData?.Add("FileSizeBytes", GetProviderDownloadSize(viewModel));
            }

            if (pageData?.ContainsKey("FileExtension") == false)
            {
                pageData?.Add("FileExtension", GetFileExtension(viewModel));
            }

            return View(viewModel);
        }

        /// <summary>
        /// The Logged in provider history action.
        /// </summary>
        /// <param name="ukprn">The provider ukprn.</param>
        /// <param name="fundingStreamNamePathPart">The funding stream name (made url safe e.g. pe-and-sport-premium).</param>
        /// <param name="viaChoicePage">Entered via the allocation choice page.</param>
        /// <returns>The 'Provider History' view.</returns>
#if !vyfv2
        [Route(LoggedInConstants.Route_ProviderHistory, Name = LoggedInConstants.RouteName_ProviderHistory)]
#endif
        public virtual async Task<IActionResult> ProviderHistory(
            string ukprn,
            string fundingStreamNamePathPart,
            bool viaChoicePage = false)
        {
            var userDetails = await GetUserAsync();
            var currentUserOrganisationUkprn = GetUkprn(userDetails);

            if (string.IsNullOrEmpty(currentUserOrganisationUkprn))
            {
                throw new RequestException($"There is no UKPRN for this provider.");
            }

            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Providers_LoggedIn);

            if (fundingStreams == null)
            {
                throw new RequestException("There are no funding streams for this provider.");
            }

            var fundingStreamCode = GetFundingStreamCode(fundingStreamNamePathPart, fundingStreams);
            if (string.IsNullOrEmpty(fundingStreamCode))
            {
                throw new RequestException($"There is no funding stream code for funding stream {fundingStreamNamePathPart}.");
            }

            if (ukprn != currentUserOrganisationUkprn)
            {
                var orgIsPartOfMat = await IsPartOfMAT(fundingStreams, currentUserOrganisationUkprn, ukprn, fundingStreamCode);
                if (!orgIsPartOfMat)
                {
                    var providerHistoryViewModel = await GetBasePageViewModel<ProviderHistoryViewModel>(userDetails, true);
                    providerHistoryViewModel.FeedbackLink = _feedbackLink;
                    providerHistoryViewModel.HomeLink = _homeLink;
                    providerHistoryViewModel.FundingStream = AsWebAdminModel(fundingStreams[fundingStreamCode]);
                    providerHistoryViewModel.DisplayUnauthorisedAccessErrorMessage = true;
                    return View(providerHistoryViewModel);
                }
            }

            var fundingStream = fundingStreams[fundingStreamCode];
            var fundingStreamConfig = new Dictionary<string, FundingStream> { { fundingStreamCode, fundingStream } };
            var publication = fundingStream.GetLatestPublication(await PreviewModeEnabled());

            if (publication == null)
            {
                throw new RequestException($"There are no publications for funding stream {fundingStreamCode}");
            }

            publication.IsLatest = true;

            foreach (var loopPublication in fundingStream.Publications)
            {
                if (loopPublication == publication)
                {
                    continue;
                }

                loopPublication.IsLatest = false;
            }

            // Get the provider funding data for the publication
            var providerFundings = await DoProviderFundingSearch(
                ukprn,
                fundingStreamConfig,
                GetProviderParentGroupingTypeWithIndicative(fundingStreams),
                string.Empty,
                byPassGrouping: true);

            providerFundings = GetDistinctProviderFundings(providerFundings);

            if (providerFundings?.Any() != true)
            {
                throw new RequestException($"There is no allocation history for this provider {fundingStreamCode}");
            }

            var fundingPeriodPublications = new List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>>();
            var fundingPeriodProviderFundings = new List<KeyValuePair<(int yearFrom, int yearTo), List<ProviderFundingViewModel>>>();

            var previewEnabled = await PreviewModeEnabled();

            if (fundingStream.HistoryIndependentOfPublications)
            {
                fundingPeriodProviderFundings =
                    ProviderFundingHelper.GetFundingsGroupedByPeriod(providerFundings, fundingStream, previewEnabled);
            }
            else
            {
                var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(publication.FundingPeriodCode);
                var years = FundingPeriodHelper.GetCurrentAndHistoricYears(yearFrom, yearTo, ViewYourFundingConstants.NumberOfYearsOfHistoricAllocationsToShow);
                fundingPeriodPublications = GroupPublicationsByYear(years, fundingStream, yearFrom);
                if (fundingPeriodPublications?.Any() != true)
                {
                    throw new RequestException($"There are no funding period publications for this funding stream {fundingStreamCode}");
                }

                CheckAndRemoveProviderFundingPublications(fundingPeriodPublications, providerFundings);
            }

            var viewModel = await GetProviderHistoryViewModel(
                ukprn,
                providerFundings?.First().OrganisationName,
                fundingPeriodPublications,
                fundingPeriodProviderFundings,
                fundingStream,
                providerFundings.First().ProviderUrn);

            if (viewModel == null)
            {
                throw new RequestException($"Failed to get the provider history view model {fundingStreamCode}.");
            }

            var defaultConfigDictionary = GetComponentDefaults();
            if (defaultConfigDictionary == null)
            {
                throw new RequestException($"Failed to get the component defaults. {fundingStreamCode}.");
            }

            var result = providerFundings.FirstOrDefault();

            viewModel.ContactUsLink = _contactUsLink;
            viewModel.FeedbackLink = _feedbackLink;
            viewModel.HomeLink = _homeLink;
            viewModel.ViaChoicePage = viaChoicePage;
            viewModel.ChoicePageLink = _choicePageLink;

            var showSelectors = await GetShowSelectorsState();
            var asStatementSpecification = await GetStatementSpecificationState();
            var showData = await GetShowData();

            var cacheKey =
                $"{publication?.FundingPeriodCode}-{FundingViewScope.LoggedInProviderHistory}-{fundingStream.FundingStreamCode}-{ukprn}";

            viewModel.FundingViewData = await _cacheService.AddOrGetExistingResultAsync(
                cacheKey,
                () => _fundingViewService.GenerateFundingViewData(
                _componentService,
                publication.FundingPeriodCode,
                fundingStream.FundingStreamCode,
                fundingStreams.Values.ToArray(),
                DateTime.MinValue,
                new Publication { FundingStreamId = publication.FundingStreamId },
                null,
                FundingViewScope.LoggedInProviderHistory,
                defaultConfigDictionary,
                null,
                true,
                false,
                null,
                bubbleUpException: false,
                iFundingApiSearchProviderFunding: result != null ? providerFundings.ToArray() : null,
                previewLayoutModel: null,
                viaChoicePage: viewModel.ViaChoicePage,
                showSelectors: showSelectors,
                asStatementSpecification: asStatementSpecification,
                showData: showData),
                CacheExpirationPolicy.Sliding);

            var pageData = viewModel.FundingViewData?.Components?.FirstOrDefault()?.PageData;

            if (pageData?.ContainsKey("FundingPeriodPublications") == false)
            {
                pageData?.Add("FundingPeriodPublications", viewModel.FundingPeriodPublications);
            }

            if (pageData?.ContainsKey("FundingPeriodProviderFundings") == false)
            {
                pageData?.Add("FundingPeriodProviderFundings", viewModel.FundingPeriodProviderFundings);
            }

            return View(viewModel);
        }

        /// <summary>
        /// The MVC action to download a provider level document.
        /// </summary>
        /// <param name="request">The provider spreadsheet download request.</param>
        /// <param name="previewLayoutModel">The preview layout model.</param>
        /// <returns>A spreadsheet for a provider level document.</returns>
        //[PreviewLayoutAction]
        //[Route(LoggedInConstants.Route_ProviderSpreadsheetDownload, Name = LoggedInConstants.RouteName_ProviderSpreadsheetDownload)]
        [Obsolete("Download the raw data (CSV) is hidden for GAG digital MVS Go Live.")]
        public virtual async Task<IActionResult> ProviderSpreadsheetDownload([FromRoute] ProviderSpreadsheetDownloadRequest request, PreviewLayoutModel previewLayoutModel = null)
        {
            var userDetails = await GetUserAsync();
            var currentUserOrganisationUkprn = GetUkprn(userDetails);

            var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(request.YearFrom, request.YearTo, request.YearTypeCode);
            var contentType = string.IsNullOrWhiteSpace(request.Format) ? FundingDocumentFileType.FileFormats[FundingDocumentFileType.Spreadsheet_OpenFormat] : FundingDocumentFileType.FileFormats[request.Format];
            var publishedDate = request.PublishedDate.ToRouteParameterDate();
            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Providers_LoggedIn);
            Enum.TryParse(request.Format, true, out FileFormat fileFormat);
            var previewModeEnabled = await PreviewModeEnabled();
            var fundingStream = fundingStreams[request.FundingStreamCode];

            Publication publication = null;
            if (!fundingStream.HistoryIndependentOfPublications)
            {
                publication = fundingStream.Publications
                    .Where(pub => pub.Status == PublicationStatus.Published
                                  || (previewModeEnabled && pub.Status == PublicationStatus.Preview))
                    .FirstOrDefault(p => p.PublishedDate == publishedDate);

                if (publication == null)
                {
                    throw new RequestException($"There are no publications for the date {request.PublishedDate}");
                }
            }

            var organisationUkprn = !string.IsNullOrWhiteSpace(request.Ukprn) ? request.Ukprn : currentUserOrganisationUkprn;
            if (currentUserOrganisationUkprn != organisationUkprn)
            {
                var orgIsPartOfMat = await IsPartOfMAT(fundingStreams, currentUserOrganisationUkprn, organisationUkprn, request.FundingStreamCode);
                if (!orgIsPartOfMat)
                {
                    throw new UnauthorizedAccessException("You are not authorised to perform this action.");
                }
            }

            var providerFundings = await DoProviderFundingSearch(
                organisationUkprn,
                fundingStreams,
                GetProviderParentGroupingType(fundingStreams),
                string.Empty,
                byPassGrouping: true);

            providerFundings = GetDistinctProviderFundings(providerFundings);

            var providerFunding = providerFundings?.FirstOrDefault(x => x.StatusChangedDate.Date == publishedDate);

            if (providerFunding == null)
            {
                throw new RequestException($"There is no provider funding for the status change date {publishedDate} for provider with ukprn {userDetails.Ukprn}");
            }

            var fundingViewScope = GetProviderSpreadSheetDownloadFundingViewScope(providerFunding);
            var filters = new[]
            {
                new SearchFilter
                {
                    PropertyName = SearchFilterPropertyName.Ukprn,
                    PropertyValue = request.Ukprn
                }
            };

            var fundingDocument = (await _fundingViewService.GenerateFundingDocument(
                fundingStream,
                fundingPeriodCode,
                publishedDate,
                publication,
                FundingViewType.Spreadsheet,
                fundingViewScope,
                new[] { fileFormat },
                filters,
                previewLayoutModel)).First();

            await AddUserVisitDetails(userDetails.Principal, request.Id);
            return File(fundingDocument.Data, contentType, fundingDocument.Filename);
        }

        private static (DateTime?, IFundingApiSearchProviderFunding[]) GetProviderFundingsForVariance(IEnumerable<IFundingApiSearchProviderFunding> fundingApiSearchProviderFundings, VarianceSelectionOption varianceSelectionOption)
        {
            DateTime? previousPublishedDate = null;
            var groupedFundings = fundingApiSearchProviderFundings
                .GroupBy(provFunding => provFunding.FundingPeriodCode)
                .OrderByDescending(group => group.Key);

            var currentYearProviderFundings = groupedFundings.First();
            var latestStatusChangeDate = currentYearProviderFundings.Max(provFunding => provFunding.StatusChangedDate);
            var previousYearProviderFundings = groupedFundings.Count() > 1 ? groupedFundings.Skip(1).First() : null;

            var providerFundings = new List<IFundingApiSearchProviderFunding>();

            switch (varianceSelectionOption)
            {
                case VarianceSelectionOption.NoComparison:
                    providerFundings.AddRange(currentYearProviderFundings.Where(provFinding => provFinding.StatusChangedDate == latestStatusChangeDate));
                    previousPublishedDate = DateTime.MinValue;
                    break;
                case VarianceSelectionOption.PreviousStatementCurrentYear:
                    var previousCurrentYearFundings = currentYearProviderFundings
                        .Where(provFunding => provFunding.StatusChangedDate < latestStatusChangeDate);

                    previousPublishedDate = previousCurrentYearFundings?.Max(providerFunding => providerFunding.StatusChangedDate);
                    providerFundings.AddRange(currentYearProviderFundings);

                    break;
                case VarianceSelectionOption.FinalStatementPreviousYear:
                    if (previousYearProviderFundings != null)
                    {
                        previousPublishedDate = previousYearProviderFundings?.Max(providerFunding => providerFunding.StatusChangedDate);
                        providerFundings.AddRange(currentYearProviderFundings.Where(provFinding => provFinding.StatusChangedDate == latestStatusChangeDate));
                        providerFundings.AddRange(previousYearProviderFundings);
                    }

                    break;
            }

            return (previousPublishedDate, providerFundings.ToArray());
        }

        private static FundingViewScope GetProviderBreakdownFundingViewScope(IFundingApiSearchProviderFunding providerFunding)
        {
            var fundingViewScope = IsFundingStatusIndicative(providerFunding) ? FundingViewScope.LoggedInIndicativeProvider : FundingViewScope.LoggedInProvider;
            return fundingViewScope;
        }

        private static bool IsFundingStatusIndicative(IFundingApiSearchProviderFunding providerFunding)
        {
            return ProviderStatus.IndicativeStatuses.Any(status => status.Equals(providerFunding?.ProviderStatus, StringComparison.InvariantCultureIgnoreCase))
                            && GroupingReason.Indicative.Equals(providerFunding?.GroupingReason, StringComparison.InvariantCultureIgnoreCase);
        }

        private static FundingViewScope GetProviderSpreadSheetDownloadFundingViewScope(IFundingApiSearchProviderFunding providerFunding)
        {
            var fundingViewScope = IsFundingStatusIndicative(providerFunding) ? FundingViewScope.LoggedInIndicativeProvider : FundingViewScope.Provider;
            return fundingViewScope;
        }

        private static FundingViewScope GetProviderSummaryFundingViewScope(IFundingApiSearchProviderFunding providerFunding)
        {
            var fundingViewScope = IsFundingStatusIndicative(providerFunding) ? FundingViewScope.LoggedInIndicativeProviderSummary : FundingViewScope.LoggedInProviderSummary;
            return fundingViewScope;
        }

        private static string GetFileExtension(ProviderFundingBreakdownViewModel viewModel)
        {
            return viewModel.FundingStream.SettingValues.Where(s => s.Setting.SettingName == "FundingDocumentFileType").FirstOrDefault()?.Value?.ToUpper();
        }

        private static string GetProviderDownloadSize(ProviderFundingBreakdownViewModel viewModel)
        {
            var settingValue = viewModel.FundingStream.SettingValues.Where(s => s.Setting.SettingName == "ProviderDownloadSizeInBytes").FirstOrDefault()?.Value;
            var hasValue = int.TryParse(settingValue, out int downloadSize);
            var fileSizeInKB = hasValue ? Math.Floor(downloadSize / 1024.0).ToString("#,#") : "--";
            return fileSizeInKB + "KB";
        }

        private async Task<bool> IsPartOfMAT(
            Dictionary<string, FundingStream> fundingStreams,
            string organisationUkprn,
            string ukprn,
            string incomingFundingStreamCode)
        {
            // TODO: Temporary conditional code, to be removed as soon as relationship between MAT and provider is created.
            if (incomingFundingStreamCode == "NMSS")
            {
                return true;
            }

            // Remove till here.
            var dataRequirements = new List<FundingApiSearchRequestObject>();

            foreach (var fundingStreamCode in fundingStreams.Keys)
            {
                var fundingStream = fundingStreams[fundingStreamCode];
                var publication = fundingStream.GetLatestPublication(await PreviewModeEnabled());

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
                        PropertyValue = organisationUkprn
                    }
                };

                dataRequirements.AddRange(_fundingViewService.GetDataRequirements(
                    fundingStream,
                    publication.FundingPeriodCode,
                    FundingPeriodHelper.GetCutOffDateForPublication(publication),
                    FundingViewScope.OrganisationSummary,
                    filters,
                    null,
                    null,
                    GroupingType.AcademyTrust));
            }

            var matches = dataRequirements?.Where(dataRequirement => dataRequirement.Type == "Funding");

            if (matches?.Any() != true)
            {
                return false;
            }

            var fundingApiSearchRequest = dataRequirements.First();
            fundingApiSearchRequest.FundingStreams = matches.SelectMany(match => match.FundingStreams).Distinct().ToArray();

            var fundingData = await _fundingApiService.SearchFunding(fundingApiSearchRequest);

            var ukprnList = new List<string>();

            foreach (var funding in fundingData?.Funding?.Select(funding => funding.ProviderFundings) ?? Enumerable.Empty<IEnumerable<string>>())
            {
                var providerUkprnList = funding.Select(GetUkprnFromProviderFunding);
                foreach (var ukPrn in providerUkprnList)
                {
                    if (!string.IsNullOrWhiteSpace(ukPrn))
                    {
                        ukprnList.Add(ukPrn);
                    }
                }
            }

            return ukprnList.Contains(ukprn);
        }

        /// <summary>
        /// Gets the provider history view model.
        /// </summary>
        /// <param name="organisationUkprn">The ukprn of the logged-in provider.</param>
        /// <param name="organisationName">The organisationName.</param>
        /// <param name="fundingPeriodPublications">The funding stream period publications.</param>
        /// <param name="fundingPeriodProviderFundings">The provider funding viewmodel.</param>
        /// <param name="fundingStream">The funding stream.</param>
        /// <returns>A ProviderHistoryViewModel model.</returns>
        private async Task<ProviderHistoryViewModel> GetProviderHistoryViewModel(
            string organisationUkprn,
            string organisationName,
            List<KeyValuePair<
                (int yearFrom, int yearTo),
                List<Publication>>> fundingPeriodPublications,
            List<KeyValuePair<
                (int yearFrom, int yearTo),
                List<ProviderFundingViewModel>>> fundingPeriodProviderFundings,
            FundingStream fundingStream,
            string providerUrn)
        {
            var userDetails = await GetUserAsync();

            var viewModel = await GetBasePageViewModel<ProviderHistoryViewModel>(userDetails, true);
            viewModel.FromMatStatementsPage = await CheckMatStatus(userDetails, Enums.FundingUIViewType.Providers_LoggedIn);
            viewModel.FundingPeriodPublications = fundingPeriodPublications;
            viewModel.FundingPeriodProviderFundings = fundingPeriodProviderFundings;
            viewModel.OrganisationUkprn = organisationUkprn;
            viewModel.OrganisationName = organisationName;
            viewModel.SecondaryContentTitle = fundingStream.FundingStreamName;
            viewModel.FundingStream = AsWebAdminModel(fundingStream);
            viewModel.ProviderUrn = providerUrn;

            return viewModel;
        }

        /// <summary>
        /// Adds User funding visit details if it's not already there.
        /// </summary>
        /// <returns>The asynchronous task.</returns>
        private async Task AddUserVisitDetails(string principal, string fundingId)
        {
            var hasUserVisitedFunding = await _fundingApiService.HasUserVisitedFunding(principal, fundingId);

            if (!hasUserVisitedFunding)
            {
                await _fundingApiService.AddUserFundingViewDetail(new AddUserFundingViewRequest { UserId = principal, FundingId = fundingId, ViewedAt = _systemProvider.DateTime.Now() });
            }
        }

        /// <summary>
        /// The MVC action for the local authority statement page.
        /// </summary>
        /// <param name="viaChoicePage">Entered via the allocation choice page.</param>
        /// <param name="userDetails">The user details.</param>
        /// <returns>The start page view.</returns>
        private async Task<List<FundingViewData>> GetLocalAuthorityStatement(bool viaChoicePage, User userDetails, FundingViewDetail fundingViewDetail)
        {
            var fundings = await DoFundingSearch(
                GetUkprn(userDetails),
                fundingViewDetail.FundingStreams,
                GroupingReason.Contracting);

            var fundingViewDataRequests = new List<FundingViewData>();

            var fundingStreamCodeGroups = fundings.GroupBy(funding => funding.FundingStreamCode);

            foreach (var group in fundingStreamCodeGroups)
            {
                var groupType = group.Where(funding => funding.GroupingType == GroupingType.LocalAuthoritySsf).Any() ? GroupingType.LocalAuthoritySsf :
                    group.Where(funding => funding.GroupingType == GroupingType.LocalAuthority).Any() ? GroupingType.LocalAuthority : string.Empty;

                if (!string.IsNullOrEmpty(groupType))
                {
                    var funding = group.Where(funding => funding.GroupingType == groupType).OrderByDescending(funding => funding.StatusChangedDate).FirstOrDefault();

                    var fundingInitial = group.Where(funding => funding.GroupingType == groupType).OrderBy(funding => funding.StatusChangedDate).First();

                    var isFundingUpdated = funding.Id != fundingInitial.Id;
                    var isInitialFunding = funding.Id == fundingInitial.Id;

                    var fundingStream = fundingViewDetail.FundingStreams[funding.FundingStreamCode];
                    var publication = fundingStream.GetLatestPublication(await PreviewModeEnabled());

                    if (publication == null || !fundingStream.RelevantForOrganisations_LoggedIn)
                    {
                        continue;
                    }

                    var filters = new[]
                    {
                    new SearchFilter
                    {
                        PropertyName = SearchFilterPropertyName.PrimaryIdentifier,
                        PropertyValue = funding.GroupUkprn
                    }
                    };

                    var fundingDocumentFileType = fundingStream.SettingValues.FirstOrDefault(sv =>
                        sv.Setting.SettingName == "FundingDocumentFileType")?.Value;

                    if (string.IsNullOrEmpty(fundingDocumentFileType))
                    {
                        fundingDocumentFileType = FundingDocumentFileType.Spreadsheet_OpenFormat;
                    }

                    var publishedDate = fundingStream.HistoryIndependentOfPublications ? funding.StatusChangedDate : publication.PublishedDate;
                    var fundingDocument = GetFundingDocumentForOrganisation(
                        fundingStream,
                        funding.FundingPeriodCode,
                        publishedDate,
                        funding.GroupCode,
                        funding.Id,
                        funding.GroupUkprn,
                        true,
                        fundingDocumentFileType);

                    var hasUserVisitedFunding = false;

                    if (isFundingUpdated || isInitialFunding)
                    {
                        hasUserVisitedFunding = await _fundingApiService.HasUserVisitedFunding(userDetails.Principal, funding.Id);
                    }

                    var cacheKey =
                        $"{publication.FundingPeriodCode}-{FundingViewScope.LoggedInOrganisationSummary}-{fundingStream.FundingStreamCode}-{GetUkprn(userDetails)}-{FundingPeriodHelper.GetCutOffDateForPublication(publication)}";

                    var fundingViewData = await _cacheService.AddOrGetExistingResultAsync(
                        cacheKey,
                        () => _fundingViewService.GenerateFundingViewData(
                            _componentService,
                            publication.FundingPeriodCode,
                            fundingStream.FundingStreamCode,
                            fundingViewDetail.FundingStreams.Values.ToArray(),
                            FundingPeriodHelper.GetCutOffDateForPublication(publication),
                            publication,
                            publication.UIModelVersion,
                            FundingViewScope.LoggedInOrganisationSummary,
                            GetComponentDefaults(),
                            fundingDocument,
                            true,
                            true,
                            null,
                            filters,
                            null,
                            bubbleUpException: false,
                            iFundingApiSearchFunding: group.Where(funding => funding.GroupingType == groupType).OrderByDescending(x => x.StatusChangedDate).ToArray(),
                            explicitFundingPassed: true,
                            viaChoicePage: viaChoicePage,
                            showSelectors: fundingViewDetail.ShowSelectors,
                            asStatementSpecification: fundingViewDetail.StatementSpecificationState,
                            showData: fundingViewDetail.ShowData),
                        CacheExpirationPolicy.Sliding);

                    fundingViewDataRequests.Add(fundingViewData);
                }
            }

            return fundingViewDataRequests;
        }

        private async Task<FundingViewDetail> GetFundingViewDetails()
        {
            var fundingViewDetail = new FundingViewDetail
            {
                FundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Providers_LoggedIn),
                ShowSelectors = await GetShowSelectorsState(),
                StatementSpecificationState = await GetStatementSpecificationState(),
                ShowData = await GetShowData()
            };

            return fundingViewDetail;
        }
    }
}