using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Utils;
using PDS.ViewYourFunding.Core.Configuration;
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
using PDS.ViewYourFunding.Web.Attributes;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Exceptions;
using PDS.ViewYourFunding.Web.Helpers;
using PDS.ViewYourFunding.Web.Models.Request;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LARecoupmentHistoryViewModel = PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.LARecoupmentHistoryViewModel;
using LocalAuthorityFundingBreakdownRequest = PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.Requests.LocalAuthorityFundingBreakdownRequest;
using LocalAuthorityHistoryViewModel = PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.LocalAuthorityHistoryViewModel;
using LocalAuthorityRecoupmentDetailRequest = PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.Requests.LocalAuthorityRecoupmentDetailRequest;
using User = Pds.Core.Common.Identity.Models.User;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Controllers
{
    /// <summary>
    /// The UI controller for logged in providers.
    /// </summary>
    [Area("LoggedIn")]
    [ServiceFilter(typeof(ProviderViewToggledCheckAttribute))]
    public class LocalAuthorityController : BaseFundingController
    {
        private const string LaRecoupmentFundingStreamCode = "LAREC";

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

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalAuthorityController"/> class.
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
        public LocalAuthorityController(
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
        /// The Logged in local authority history action.
        /// </summary>
        /// <param name="ukprn">The local authority ukprn.</param>
        /// <param name="fundingStreamNamePathPart">The funding stream name (made url safe e.g. pe-and-sport-premium).</param>
        /// <param name="viaChoicePage">Entered via the allocation choice page.</param>
        /// <returns>The 'Provider History' view.</returns>
        [Authorize(nameof(UserRole.ViewAllocationStatements))]
        [Route(LoggedInConstants.Route_LocalAuthorityHistory, Name = LoggedInConstants.RouteName_LocalAuthorityHistory)]
        public virtual async Task<IActionResult> LocalAuthorityHistory(
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

            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Organisations_LoggedIn);

            if (!fundingStreams.Any())
            {
                throw new RequestException($"There are no funding streams for {fundingStreamNamePathPart} for {ukprn}.");
            }

            if (ukprn != currentUserOrganisationUkprn)
            {
                throw new UnauthorizedAccessException("You are not authorised to perform this action.");
            }

            var fundingStreamCode = GetFundingStreamCode(fundingStreamNamePathPart, fundingStreams);
            if (string.IsNullOrEmpty(fundingStreamCode))
            {
                throw new RequestException($"There is no funding stream code for funding stream {fundingStreamNamePathPart}.");
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
            var fundings = await DoFundingSearch(
                ukprn,
                fundingStreamConfig,
                GroupingReason.Contracting);

            var groupType = GetGroupType(fundings, userDetails.Ukprn);

            if (fundings?.Any() != true)
            {
                throw new RequestException($"There is no allocation history for this local authority with ukprn: {ukprn} for funding stream code: {fundingStreamCode}");
            }

            fundings = fundings.Where(fundingItem => fundingItem.GroupingType == groupType)
                .OrderByDescending(fundingItem => fundingItem.StatusChangedDate.Date).ToList();

            var fundingPeriodPublications = new List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>>();
            var fundingPeriodProviderFundings = new List<KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>>();

            if (fundingStream.HistoryIndependentOfPublications)
            {
                fundingPeriodProviderFundings =
                    OrganisationFundingHelper.GetFundingsGroupedByPeriod(fundings, fundingStream, await PreviewModeEnabled());
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

                CheckAndRemoveFundingPublications(fundingPeriodPublications, fundings);
            }

            var viewModel = await GetLocalAuthorityHistoryViewModel(
                ukprn,
                fundings?.First().GroupName,
                fundingPeriodPublications,
                fundingPeriodProviderFundings,
                fundingStream,
                viaChoicePage);

            var defaultConfigDictionary = GetComponentDefaults();

            var result = fundings.FirstOrDefault();

            var showSelectors = await GetShowSelectorsState();
            var asStatementSpecification = await GetStatementSpecificationState();
            var showData = await GetShowData();

            viewModel.FundingViewData = await _fundingViewService.GenerateFundingViewData(
                _componentService,
                publication.FundingPeriodCode,
                fundingStream.FundingStreamCode,
                fundingStreams.Values.ToArray(),
                FundingPeriodHelper.GetCutOffDateForPublication(publication),
                publication,
                null,
                FundingViewScope.LoggedInOrganisationHistory,
                defaultConfigDictionary,
                null,
                true,
                false,
                null,
                bubbleUpException: false,
                iFundingApiSearchFunding: result != null ? new[] { result } : null,
                previewLayoutModel: null,
                viaChoicePage: viewModel.ViaChoicePage,
                showSelectors: showSelectors,
                asStatementSpecification: asStatementSpecification,
                showData: showData);

            var pageData = viewModel.FundingViewData?.Components?.FirstOrDefault()?.PageData;
            pageData?.Add(nameof(viewModel.FundingPeriodPublications), viewModel.FundingPeriodPublications);
            pageData?.Add(nameof(viewModel.FundingPeriodLocalAuthorityFundings), viewModel.FundingPeriodLocalAuthorityFundings);
            return View(viewModel);
        }

        /// <summary>
        /// The MVC action for the local authority breakdown page.
        /// </summary>
        /// <param name="localAuthorityFundingBreakdownRequest">The localAuthority FundingBreakdown Request.</param>
        /// <returns>The local authority breakdown page view.</returns>
        [Authorize(nameof(UserRole.ViewAllocationStatements))]
        [Route(
            LoggedInConstants.Route_LocalAuthorityFundingBreakdown,
            Name = LoggedInConstants.RouteName_LocalAuthorityFundingBreakdown)]
        public async Task<IActionResult> LocalAuthorityFundingBreakdown(
            LocalAuthorityFundingBreakdownRequest localAuthorityFundingBreakdownRequest)
        {
            var userDetails = await GetUserAsync();

            var viewModel = await GetBasePageViewModel<LocalAuthorityBreakdownViewModel>(userDetails, true);
            viewModel.ContactUsLink = _contactUsLink;
            viewModel.FeedbackLink = _feedbackLink;
            viewModel.HomeLink = _homeLink;
            viewModel.ViaChoicePage = localAuthorityFundingBreakdownRequest.ViaChoicePage;
            viewModel.ChoicePageLink = _choicePageLink;
            viewModel.IncludeHistory = localAuthorityFundingBreakdownRequest.IncludeHistory;
            viewModel.OrganisationUkPrn = GetUkprn(userDetails);
            var fundingViewDetail = await GetFundingViewDetails();
            var fundingStreamCode =
                GetFundingStreamCode(
                    localAuthorityFundingBreakdownRequest.FundingStreamNamePathPart,
                    fundingViewDetail.FundingStreams);
            if (string.IsNullOrWhiteSpace(fundingStreamCode) || !fundingViewDetail.FundingStreams.TryGetValue(fundingStreamCode, out var fundingStream))
            {
                throw new RequestException(
                    $"Funding stream not found for {localAuthorityFundingBreakdownRequest.FundingStreamNamePathPart}");
            }


            var publishedDate = localAuthorityFundingBreakdownRequest.PublishedDate?.ToRouteParameterDate();

            var fundingViewDataRequest = await GetFundingViewData(
                localAuthorityFundingBreakdownRequest.ViaChoicePage,
                userDetails,
                fundingStream,
                fundingViewDetail,
                viewModel,
                publishedDate);

            var fundingViewDataResponse = await fundingViewDataRequest;

            viewModel.FundingViewData = fundingViewDataResponse;
            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(viewModel.Funding.FundingPeriodCode);
            viewModel.YearFrom = yearFrom;
            viewModel.YearTo = yearTo;

            if (string.IsNullOrEmpty(viewModel.OrganisationName))
            {
                viewModel.OrganisationName = fundingViewDataResponse.EntityName;
            }

            viewModel.FundingStream = fundingStream;
            await AddUserVisitDetails(userDetails.Principal, viewModel.Funding.Id);

            return View(viewModel);
        }

        /// <summary>
        /// The MVC action to download a provider level document.
        /// </summary>
        /// <param name="request">The provider spreadsheet download request.</param>
        /// <param name="previewLayoutModel">The preview layout model.</param>
        /// <returns>A spreadsheet for a provider level document.</returns>
        [PreviewLayoutAction]
        [Authorize(nameof(UserRole.ViewAllocationStatements))]
        [Route(LoggedInConstants.Route_OrganisationSpreadsheetDownload, Name = LoggedInConstants.RouteName_OrganisationSpreadsheetDownload)]
        public virtual async Task<IActionResult> OrganisationSpreadsheetDownload([FromRoute] OrganisationSpreadsheetDownloadRequest request, PreviewLayoutModel previewLayoutModel = null)
        {
            var userDetails = await GetUserAsync();

            var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(request.YearFrom, request.YearTo, request.YearTypeCode);
            var contentType = string.IsNullOrWhiteSpace(request.Format) ? FundingDocumentFileType.FileFormats[FundingDocumentFileType.Spreadsheet_OpenFormat] : FundingDocumentFileType.FileFormats[request.Format];
            var publishedDate = request.PublishedDate.ToRouteParameterDate();
            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Organisations_LoggedIn);
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
                    throw new ArgumentOutOfRangeException($"There are no publications for the date {request.PublishedDate}");
                }
            }

            var filters = new SearchFilter[]
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
                FundingViewScope.LoggedInOrganisationSsf,
                new[] { fileFormat },
                filters,
                previewLayoutModel)).First();

            return File(fundingDocument.Data, contentType, fundingDocument.Filename);
        }

        /// <summary>
        /// The Logged in local authority recoupment summary action.
        /// </summary>
        /// <param name="ukprn">The local authority ukprn.</param>
        /// <returns>The 'Recoupment Reports' view.</returns>
        [Authorize(nameof(UserRole.ViewRecoupmentReports))]
        [Route(LoggedInConstants.Route_LocalAuthorityRecoupmentSummary, Name = LoggedInConstants.RouteName_LocalAuthorityRecoupmentSummary)]
        public virtual async Task<IActionResult> LocalAuthorityRecoupmentSummary(string ukprn)
        {
            var userDetails = await GetUserAsync();

            var viewModel = await GetBasePageViewModel<LocalAuthorityRecoupmentSummaryViewModel>(userDetails, true);
            viewModel.ContactUsLink = _contactUsLink;
            viewModel.FeedbackLink = _feedbackLink;
            viewModel.HomeLink = _homeLink;
            viewModel.ChoicePageLink = _choicePageLink;
            viewModel.OrganisationUkPrn = GetUkprn(userDetails);

            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Organisations_LoggedIn);
            FundingStream fundingStream = null;
            try
            {
                fundingStream = fundingStreams[LaRecoupmentFundingStreamCode];
            }
            catch (Exception)
            {
                throw new RequestException($"There are no funding streams available for this local authority with ukprn: {ukprn} for funding stream code: {LaRecoupmentFundingStreamCode}");
            }

            var fundingStreamConfig = new Dictionary<string, FundingStream> { { LaRecoupmentFundingStreamCode, fundingStream } };

            //Temporary for real data search.
            var laCode = "334";

            // Get the provider funding data for the publication
            var fundings = await DoFundingSearch(
                laCode,
                fundingStreamConfig,
                GroupingReason.Information);

            var groupType = GetGroupType(fundings, userDetails.Ukprn);

            if (fundings?.Any() != true)
            {
                throw new RequestException($"There are no recoupment reports for this local authority with ukprn: {ukprn} for funding stream code: {LaRecoupmentFundingStreamCode}");
            }

            fundings = fundings.Where(fundingItem => fundingItem.GroupingType == groupType)
                .OrderByDescending(fundingItem => fundingItem.StatusChangedDate.Date).ToList();

            var latestPublications = fundingStream.GetLatestPublication(await PreviewModeEnabled());

            var publications = fundingStream.GetPublicationsForPeriod(await PreviewModeEnabled(), fundingPeriodCode: latestPublications.FundingPeriodCode);

            if (publications == null)
            {
                throw new RequestException($"There are no publications for funding stream {LaRecoupmentFundingStreamCode}");
            }

            var publicationCount = 0;

            foreach (var publication in publications)
            {
                var isLatest = false;

                publicationCount++;

                publication.IsLatest = false;

                if (publicationCount == 1)
                {
                    publication.IsLatest = true;
                    isLatest = true;
                }

                var defaultConfigDictionary = GetComponentDefaults();

                var result = fundings.FirstOrDefault(f => f.StatusChangedDate <= publication.PublishedDate);

                var showSelectors = await GetShowSelectorsState();
                var asStatementSpecification = await GetStatementSpecificationState();
                var showData = await GetShowData();

                var viewFundingData = await _fundingViewService.GenerateFundingViewData(
                    _componentService,
                    publication.FundingPeriodCode,
                    fundingStream.FundingStreamCode,
                    fundingStreams.Values.ToArray(),
                    FundingPeriodHelper.GetCutOffDateForPublication(publication),
                    publication,
                    null,
                    FundingViewScope.LoggedInOrganisationSummary,
                    defaultConfigDictionary,
                    null,
                    isLatest,
                    false,
                    null,
                    bubbleUpException: false,
                    iFundingApiSearchFunding: result != null ? new[] { result } : null,
                    previewLayoutModel: null,
                    viaChoicePage: viewModel.ViaChoicePage,
                    showSelectors: showSelectors,
                    asStatementSpecification: asStatementSpecification,
                    showData: showData);

                viewModel.FundingViewData.Add($"{LaRecoupmentFundingStreamCode}-{publicationCount}", viewFundingData);
            }

            return View(viewModel);
        }

        [Route(LoggedInConstants.Route_LARecoupmentHistory, Name = LoggedInConstants.RouteName_LARecoupmentHistory)]
        public virtual async Task<IActionResult> LARecoupmentHistory(
          string ukprn,
          string fundingStreamNamePathPart,
          bool viaChoicePage = false)
        {
            var userDetails = await GetUserAsync();

            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Organisations_LoggedIn);

            if (!fundingStreams.Any())
            {
                throw new RequestException($"There are no funding streams for {fundingStreamNamePathPart} for {ukprn}.");
            }

            FundingStream fundingStream = null;
            try
            {
                fundingStream = fundingStreams[LaRecoupmentFundingStreamCode];
            }
            catch (Exception)
            {
                throw new RequestException($"There are no funding streams available for this local authority with ukprn: {ukprn} for funding stream code: {LaRecoupmentFundingStreamCode}");
            }

            var fundingStreamConfig = new Dictionary<string, FundingStream> { { LaRecoupmentFundingStreamCode, fundingStream } };

            //Temporary for real data search.
            var laCode = "334";

            // Get the provider funding data for the publication
            var fundings = await DoFundingSearch(
                laCode,
                fundingStreamConfig,
                GroupingReason.Information);

            if (fundings?.Any() != true)
            {
                throw new RequestException($"There are no recoupment reports for this local authority with ukprn: {ukprn} for funding stream code: {LaRecoupmentFundingStreamCode}");
            }

            var groupType = GetGroupType(fundings, userDetails.Ukprn);

            fundings = fundings.Where(fundingItem => fundingItem.GroupingType == groupType)
                .OrderByDescending(fundingItem => fundingItem.StatusChangedDate.Date).ToList();

            var fundingPeriodPublications = new List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>>();
            var fundingPeriodProviderFundings = new List<KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>>();

            var latestPublication = fundingStream.GetLatestPublication(await PreviewModeEnabled());

            if (latestPublication == null)
            {
                throw new RequestException($"There are no publications for funding stream {LaRecoupmentFundingStreamCode}");
            }

            var earliestPublication = fundingStream.GetEarliestPublication(await PreviewModeEnabled());

            var (latestYearFrom, latestYearTo) = FundingPeriodHelper.GetYearsFromCode(latestPublication.FundingPeriodCode);
            var (earliestYearFrom, earliestYearTo) = FundingPeriodHelper.GetYearsFromCode(earliestPublication.FundingPeriodCode);
            var years = FundingPeriodHelper.GetCurrentAndHistoricYears(latestYearFrom, latestYearTo, ViewYourFundingConstants.NumberOfYearsOfHistoricAllocationsToShow);
            fundingPeriodPublications = GroupPublicationsByYear(years, fundingStream, earliestYearFrom);
            if (fundingPeriodPublications?.Any() != true)
            {
                throw new RequestException($"There are no funding period publications for this funding stream");
            }

            CheckAndRemoveFundingPublications(fundingPeriodPublications, fundings);

            var viewModel = await GetLARecoupmentHistoryViewModel(
                ukprn,
                fundings?.First().GroupName,
                fundingPeriodPublications,
                fundingPeriodProviderFundings,
                fundingStream,
                viaChoicePage);

            var defaultConfigDictionary = GetComponentDefaults();

            var result = fundings.FirstOrDefault();

            var showSelectors = await GetShowSelectorsState();
            var asStatementSpecification = await GetStatementSpecificationState();
            var showData = await GetShowData();

            viewModel.FundingViewData = await _fundingViewService.GenerateFundingViewData(
                _componentService,
                latestPublication.FundingPeriodCode,
                fundingStream.FundingStreamCode,
                fundingStreams.Values.ToArray(),
                FundingPeriodHelper.GetCutOffDateForPublication(latestPublication),
                latestPublication,
                null,
                FundingViewScope.LoggedInOrganisationHistory,
                defaultConfigDictionary,
                null,
                true,
                false,
                null,
                bubbleUpException: false,
                iFundingApiSearchFunding: result != null ? new[] { result } : null,
                previewLayoutModel: null,
                viaChoicePage: viewModel.ViaChoicePage,
                showSelectors: showSelectors,
                asStatementSpecification: asStatementSpecification,
                showData: showData);

            var pageData = viewModel.FundingViewData?.Components?.FirstOrDefault()?.PageData;
            pageData?.Add(nameof(viewModel.FundingPeriodPublications), viewModel.FundingPeriodPublications);
            pageData?.Add(nameof(viewModel.FundingPeriodLocalAuthorityFundings), viewModel.FundingPeriodLocalAuthorityFundings);

            return View(viewModel);
        }

        /// <summary>
        /// The Logged in local authority recoupment detail action.
        /// </summary>
        /// <param name="request">The local authority recoupment detail request.</param>
        /// <returns>The 'Recoupment Detail' view.</returns>
        [Authorize(nameof(UserRole.ViewRecoupmentReports))]
        [Route(LoggedInConstants.Route_LocalAuthorityRecoupmentDetail, Name = LoggedInConstants.RouteName_LocalAuthorityRecoupmentDetail)]
        public virtual async Task<IActionResult> LocalAuthorityRecoupmentDetail(LocalAuthorityRecoupmentDetailRequest request)
        {
            var userDetails = await GetUserAsync();

            var viewModel = await GetBasePageViewModel<LocalAuthorityRecoupmentDetailViewModel>(userDetails, true);
            viewModel.ContactUsLink = _contactUsLink;
            viewModel.FeedbackLink = _feedbackLink;
            viewModel.HomeLink = _homeLink;
            viewModel.ChoicePageLink = _choicePageLink;
            viewModel.OrganisationUkPrn = GetUkprn(userDetails);
            viewModel.YearFrom = request.YearFrom;
            viewModel.YearTo = request.YearTo;
            viewModel.PublishedDate = request.PublishedDate;
            viewModel.Tab = request.Tab;

            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Organisations_LoggedIn);

            if (!fundingStreams.Any())
            {
                throw new RequestException("Funding streams not found for logged in organisation");
            }

            if (fundingStreams.Any() && !fundingStreams.ContainsKey(request.FundingStreamCode))
            {
                throw new RequestException(
                                   $"Funding stream not found for {request.FundingStreamCode}");
            }

            var fundingStream = fundingStreams[request.FundingStreamCode];
            var fundingStreamConfig = new Dictionary<string, FundingStream> { { request.FundingStreamCode, fundingStream } };
            var publishedDate = request.PublishedDate?.ToRouteParameterDate();
            var publication = fundingStream.Publications.Where(x => x.PublishedDate.Date == publishedDate).FirstOrDefault();

            if (publication == null)
            {
                throw new RequestException($"There are no publications for funding stream {request.FundingStreamCode}");
            }

            //Temporary for real data search.
            var laCode = "334";

            // Get the provider funding data for the publication
            var fundings = await DoFundingSearch(
                laCode,
                fundingStreamConfig,
                GroupingReason.Information);

            var groupType = GetGroupType(fundings, userDetails.Ukprn);

            if (fundings?.Any() != true)
            {
                throw new RequestException($"There are no recoupment reports for this local authority with ukprn: {request.Ukprn} for funding stream code: {request.FundingStreamCode}");
            }

            fundings = fundings.Where(fundingItem => fundingItem.GroupingType == groupType)
                .OrderByDescending(fundingItem => fundingItem.StatusChangedDate.Date).ToList();

            var defaultConfigDictionary = GetComponentDefaults();

            var result = fundings.FirstOrDefault(f => f.StatusChangedDate <= publication.PublishedDate);

            var showSelectors = await GetShowSelectorsState();
            var asStatementSpecification = await GetStatementSpecificationState();
            var showData = await GetShowData();

            var fundingDocument = GetFundingDocumentForOrganisation(
                fundingStream,
                publication.FundingPeriodCode,
                publishedDate.GetValueOrDefault(),
                laCode);

            viewModel.FundingStream = fundingStream;
            viewModel.Funding = result;
            viewModel.Document = fundingDocument;

            var filters = new SearchFilter[]
            {
                new SearchFilter
                {
                    PropertyName = SearchFilterPropertyName.ParentPrimaryIdentifier,
                    PropertyValue = laCode
                }
            };

            viewModel.FundingViewData = await _fundingViewService.GenerateFundingViewData(
                _componentService,
                publication.FundingPeriodCode,
                fundingStream.FundingStreamCode,
                fundingStreams.Values.ToArray(),
                FundingPeriodHelper.GetCutOffDateForPublication(publication),
                publication,
                null,
                FundingViewScope.LoggedInOrganisation,
                defaultConfigDictionary,
                fundingDocument,
                publication.IsLatest ?? false,
                false,
                null,
                bubbleUpException: false,
                iFundingApiSearchFunding: result != null ? new[] { result } : null,
                previewLayoutModel: null,
                viaChoicePage: viewModel.ViaChoicePage,
                showSelectors: showSelectors,
                asStatementSpecification: asStatementSpecification,
                showData: showData,
                filters: filters,
                selectedTab: request.Tab);

            if (string.IsNullOrEmpty(viewModel.OrganisationName))
            {
                viewModel.OrganisationName = viewModel.FundingViewData?.EntityName;
            }

            return View(viewModel);
        }

        /// <summary>
        /// Get the group type.
        /// </summary>
        /// <param name="fundings">The fundings.</param>
        /// <param name="ukPrn">The ukprn.</param>
        /// <returns>Return group type.</returns>
        private static string GetGroupType(List<IFundingApiSearchFunding> fundings, int? ukPrn)
        {
            var groupType = fundings.Any(funding => funding.GroupingType == GroupingType.LocalAuthoritySsf)
                ? GroupingType.LocalAuthoritySsf
                : string.Empty;

            if (
                string.IsNullOrWhiteSpace(groupType)
                && fundings.Any(funding => funding.GroupingType == GroupingType.LocalAuthority))
            {
                groupType = GroupingType.LocalAuthority;
            }

            if (string.IsNullOrEmpty(groupType))
            {
                throw new RequestException($"No expected grouping type data found for {ukPrn}");
            }

            return groupType;
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
        /// Get funding view data.
        /// </summary>
        /// <param name="viaChoicePage">The viaChoicePage.</param>
        /// <param name="userDetails">The userDetails.</param>
        /// <param name="fundingStreamInput">The fundingStreamInput.</param>
        /// <param name="fundingViewDetail">The fundingViewDetail.</param>
        /// <param name="viewModel">The viewModel.</param>
        /// <param name="requestPublishedDate">The requestPublishedDate.</param>
        /// <returns>Return funding view data.</returns>
        private async Task<Task<FundingViewData>> GetFundingViewData(
            bool viaChoicePage,
            User userDetails,
            FundingStream fundingStreamInput,
            FundingViewDetail fundingViewDetail,
            LocalAuthorityBreakdownViewModel viewModel,
            DateTime? requestPublishedDate)
        {
            var fundingStreamDictionary = new Dictionary<string, FundingStream>
            {
                {
                    fundingStreamInput.FundingStreamCode, fundingStreamInput
                }
            };

            var fundings = await DoFundingSearch(
                GetUkprn(userDetails),
                fundingStreamDictionary,
                GroupingReason.Contracting);

            if (!fundings.Any())
            {
                throw new RequestException($"No funding data found for {userDetails.Ukprn}");
            }

            var groupType = GetGroupType(fundings, userDetails.Ukprn);

            var funding = fundings.Where(fundingItem => fundingItem.GroupingType == groupType)
                .OrderByDescending(fundingItem => fundingItem.StatusChangedDate.Date == requestPublishedDate).First();

            viewModel.Funding = funding;

            var fundingInitial = fundings.Where(fundingItem => fundingItem.GroupingType == groupType)
                .OrderBy(fundingItem => fundingItem.StatusChangedDate == requestPublishedDate).First();

            var isFundingUpdated = funding.Id != fundingInitial.Id;
            var isInitialFunding = funding.Id == fundingInitial.Id;

            var latestDateForPeriod = fundings.Where(result => result.FundingPeriodCode == funding.FundingPeriodCode && result.GroupingType == groupType).Max(p => p.StatusChangedDate);
            var isLatestOrFinalFundingForYear = latestDateForPeriod.Date == requestPublishedDate;

            var fundingStream = fundingViewDetail.FundingStreams[funding.FundingStreamCode];
            var publication = fundingStream.GetLatestPublication(await PreviewModeEnabled());
            var isCurrentYear = funding.FundingPeriodCode == publication.FundingPeriodCode;

            var fundingDocumentFileType = fundingStream.SettingValues.FirstOrDefault(sv =>
                sv.Setting.SettingName == "FundingDocumentFileType")?.Value;

            if (string.IsNullOrEmpty(fundingDocumentFileType))
            {
                fundingDocumentFileType = FundingDocumentFileType.Spreadsheet_OpenFormat;
            }

            var publishedDate = requestPublishedDate.Value;

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
                hasUserVisitedFunding =
                    await _fundingApiService.HasUserVisitedFunding(userDetails.Principal, funding.Id);
            }

            var selectedFundings = fundings.Where(funding => funding.GroupingType == groupType && funding.StatusChangedDate.Date <= publishedDate.Date)
                    .OrderByDescending(x => x.StatusChangedDate).ToArray();
            var fundingViewData = _fundingViewService.GenerateFundingViewData(
                _componentService,
                publication.FundingPeriodCode,
                fundingStream.FundingStreamCode,
                fundingViewDetail.FundingStreams.Values.ToArray(),
                publishedDate,
                publication,
                publication.UIModelVersion,
                groupType == GroupingType.LocalAuthoritySsf ? FundingViewScope.LoggedInOrganisationSsf : FundingViewScope.LoggedInOrganisationMss,
                GetComponentDefaults(),
                fundingDocument,
                isLatestOrFinalFundingForYear,
                isCurrentYear,
                null,
                null,
                bubbleUpException: false,
                iFundingApiSearchFunding: selectedFundings,
                explicitFundingPassed: true,
                viaChoicePage: viaChoicePage,
                showSelectors: fundingViewDetail.ShowSelectors,
                asStatementSpecification: fundingViewDetail.StatementSpecificationState,
                showData: fundingViewDetail.ShowData);

            return fundingViewData;
        }

        /// <summary>
        /// Get funding view details.
        /// </summary>
        /// <returns>Return funding view details.</returns>
        private async Task<FundingViewDetail> GetFundingViewDetails()
        {
            var fundingViewDetail = new FundingViewDetail
            {
                FundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Organisations_LoggedIn),
                ShowSelectors = await GetShowSelectorsState(),
                StatementSpecificationState = await GetStatementSpecificationState(),
                ShowData = await GetShowData()
            };

            return fundingViewDetail;
        }

        /// <summary>
        /// Gets the local authority history view model.
        /// </summary>
        /// <param name="organisationUkprn">The ukprn of the logged-in Local authority.</param>
        /// <param name="organisationName">The organisationName.</param>
        /// <param name="fundingPeriodPublications">The funding stream period publications.</param>
        /// <param name="fundingPeriodLocalAuthorityFundings">The Local authority funding viewmodel.</param>
        /// <param name="fundingStream">The funding stream.</param>
        /// <returns>A LocalAuthorityHistoryViewModel model.</returns>
        private async Task<LocalAuthorityHistoryViewModel> GetLocalAuthorityHistoryViewModel(
            string organisationUkprn,
            string organisationName,
            List<KeyValuePair<
                (int yearFrom, int yearTo),
                List<Publication>>> fundingPeriodPublications,
            List<KeyValuePair<
                (int yearFrom, int yearTo),
                List<LocalAuthorityFundingViewModel>>> fundingPeriodLocalAuthorityFundings,
            FundingStream fundingStream,
            bool viaChoicePage)
        {
            var userDetails = await GetUserAsync();

            var viewModel = await GetBasePageViewModel<LocalAuthorityHistoryViewModel>(userDetails, true);
            viewModel.FromMatStatementsPage = await CheckMatStatus(userDetails, Enums.FundingUIViewType.Organisations_LoggedIn);
            viewModel.FundingPeriodPublications = fundingPeriodPublications;
            viewModel.FundingPeriodLocalAuthorityFundings = fundingPeriodLocalAuthorityFundings;
            viewModel.OrganisationUkprn = organisationUkprn;
            viewModel.OrganisationName = organisationName;
            viewModel.SecondaryContentTitle = fundingStream.FundingStreamName;
            viewModel.FundingStream = AsWebAdminModel(fundingStream);
            viewModel.ContactUsLink = _contactUsLink;
            viewModel.FeedbackLink = _feedbackLink;
            viewModel.HomeLink = _homeLink;
            viewModel.ViaChoicePage = viaChoicePage;
            viewModel.ChoicePageLink = _choicePageLink;
            return viewModel;
        }

        /// <summary>
        /// Gets the la recoupment history view model.
        /// </summary>
        /// <param name="organisationUkprn">The ukprn of the logged-in Local authority.</param>
        /// <param name="organisationName">The organisationName.</param>
        /// <param name="fundingPeriodPublications">The funding stream period publications.</param>
        /// <param name="fundingPeriodLocalAuthorityFundings">The Local authority funding viewmodel.</param>
        /// <param name="fundingStream">The funding stream.</param>
        /// <returns>A LocalAuthorityHistoryViewModel model.</returns>
        private async Task<LARecoupmentHistoryViewModel> GetLARecoupmentHistoryViewModel(
            string organisationUkprn,
            string organisationName,
            List<KeyValuePair<
                (int yearFrom, int yearTo),
                List<Publication>>> fundingPeriodPublications,
            List<KeyValuePair<
                (int yearFrom, int yearTo),
                List<LocalAuthorityFundingViewModel>>> fundingPeriodLocalAuthorityFundings,
            FundingStream fundingStream,
            bool viaChoicePage)
        {
            var userDetails = await GetUserAsync();

            var viewModel = await GetBasePageViewModel<LARecoupmentHistoryViewModel>(userDetails, true);
            viewModel.FromMatStatementsPage = await CheckMatStatus(userDetails, Enums.FundingUIViewType.Organisations_LoggedIn);
            viewModel.FundingPeriodPublications = fundingPeriodPublications;
            viewModel.FundingPeriodLocalAuthorityFundings = fundingPeriodLocalAuthorityFundings;
            viewModel.OrganisationUkprn = organisationUkprn;
            viewModel.OrganisationName = organisationName;
            viewModel.SecondaryContentTitle = fundingStream.FundingStreamName;
            viewModel.FundingStream = AsWebAdminModel(fundingStream);
            viewModel.ContactUsLink = _contactUsLink;
            viewModel.FeedbackLink = _feedbackLink;
            viewModel.HomeLink = _homeLink;
            viewModel.ViaChoicePage = viaChoicePage;
            viewModel.ChoicePageLink = _choicePageLink;
            return viewModel;
        }
    }
}