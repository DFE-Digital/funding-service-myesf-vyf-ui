using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Identity.Claims.Interfaces;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.RequestObjects;
using PDS.ViewYourFunding.Services.ResponseObjects;
using PDS.ViewYourFunding.Web.Attributes;
using PDS.ViewYourFunding.Web.Constants;
using PDS.ViewYourFunding.Web.Helpers;
using PDS.ViewYourFunding.Web.Models.Request;
using PDS.ViewYourFunding.Web.Models.Shared;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PDS.ViewYourFunding.Web.Controllers
{
    /// <summary>
    /// The MVC controller for the View Your Funding area of the service.
    /// </summary>
    public class ViewYourFundingController : BaseFundingController
    {
        #region Private Fields

        /// <summary>
        /// The feedback link.
        /// </summary>
        private readonly string _feedbackLink;

        /// <summary>
        /// The contact us link.
        /// </summary>
        private readonly string _contactUsLink;

        /// <summary>
        /// The funding document service.
        /// </summary>
        private readonly IFundingDocumentService _fundingDocumentService;

        /// <summary>
        /// The funding Api service.
        /// </summary>
        private readonly IFundingApiService _fundingApiService;

        /// <summary>
        /// The funding view service.
        /// </summary>
        private readonly IFundingViewService _fundingViewService;

        /// <summary>
        /// The component service.
        /// </summary>
        private readonly IComponentService _componentService;

        /// <summary>
        /// The configuration service.
        /// </summary>
        private readonly ApplicationConfiguration _applicationConfiguration;

        /// <summary>
        /// The terminated local authority.
        /// </summary>
        private readonly TerminatedLocalAuthority _terminatedLocalAuthority;

        /// <summary>
        /// The recently opened local authorities.
        /// </summary>
        private readonly RecentlyOpenedLocalAuthorities _recentlyOpenedLocalAuthorities;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewYourFundingController"/> class.
        /// </summary>
        /// <param name="componentService">The component service to use.</param>
        /// <param name="securityService">The security service to use.</param>
        /// <param name="fundingDocumentService">The service to use for funding documents.</param>
        /// <param name="fundingApiService">The API service to use for searching for funding.</param>
        /// <param name="settingsService">The settings service to use.</param>
        /// <param name="fundingViewService">The funding view service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="globalSettingService">The global setting service to use.</param>
        /// <param name="applicationConfigurationOptions">The configuration service.</param>
        public ViewYourFundingController(
            IComponentService componentService,
            IClaimsBasedIdentityService securityService,
            IFundingDocumentService fundingDocumentService,
            IFundingApiService fundingApiService,
            IUserJourneyService settingsService,
            IFundingViewService fundingViewService,
            IMapper mapper,
            ICacheService cacheService,
            IGlobalSettingService globalSettingService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions)
            : base(securityService, applicationConfigurationOptions, settingsService, mapper, fundingApiService, cacheService, fundingViewService, globalSettingService)
        {
            _componentService = componentService;
            _fundingDocumentService = fundingDocumentService;
            _fundingApiService = fundingApiService;
            _feedbackLink = applicationConfigurationOptions.Value.FeedbackLink;
            _fundingViewService = fundingViewService;
            _applicationConfiguration = applicationConfigurationOptions.Value;
            _contactUsLink = applicationConfigurationOptions.Value.ContactUsLink;
            _terminatedLocalAuthority = _applicationConfiguration.TerminatedLocalAuthority;
            _recentlyOpenedLocalAuthorities = _applicationConfiguration.RecentlyOpenedLocalAuthorities;
        }

        #endregion


        #region Entry point to all journeys and View Funding at National level

        /// <summary>
        /// Login action.
        /// </summary>
        /// <returns>The login page view.</returns>
        [Authorize]
        [Route(ViewYourFundingConstants.Route_Login, Name = ViewYourFundingConstants.RouteName_Login)]
        public IActionResult Login()
        {
            return RedirectToRoute(ViewYourFundingConstants.RouteName_Start);
        }

        /// <summary>
        /// A logout action - this may or may not be used (perhaps the one in Sfs.Web would be used instead).
        /// </summary>
        /// <returns>The start page view.</returns>
        [Route(ViewYourFundingConstants.Route_Logout, Name = ViewYourFundingConstants.RouteName_Logout)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            var result = SignOut(
               CookieAuthenticationDefaults.AuthenticationScheme,
               OpenIdConnectDefaults.AuthenticationScheme);

            return await Task.FromResult<ActionResult>(result);
        }

        /// <summary>
        /// A required action to handle part of the logout flow.
        /// </summary>
        /// <returns>The start page view.</returns>
        [Route(ViewYourFundingConstants.Route_PostLogout, Name = ViewYourFundingConstants.RouteName_PostLogout)]
        public IActionResult PostLogout()
        {
            return RedirectToHome;
        }

        /// <summary>
        /// A required action to handle part of the logout flow.
        /// </summary>
        /// <returns>The start page view.</returns>
        [Route(ViewYourFundingConstants.Route_PostLogoutRedirect, Name = ViewYourFundingConstants.RouteName_PostLogoutRedirect)]
        public IActionResult PostLogoutRedirect()
        {
            return LogoutMyesfAndRedirectToMyesfStartpage();
        }

        /// <summary>
        /// The MVC action for the start page.
        /// </summary>
        /// <returns>The start page view.</returns>
        [Route(ViewYourFundingConstants.Route_Start, Name = ViewYourFundingConstants.RouteName_Start)]
        [Route(ViewYourFundingConstants.Route_Start_New, Name = ViewYourFundingConstants.RouteName_Start_New)]
        public virtual async Task<IActionResult> Start()
        {
            var viewModel = await GetBasePageViewModel<StartPageViewModel>();
            viewModel.FundingStreams = AsWebModel(await GetActiveFundingStreams()).Where(fundingStream =>
                fundingStream.RelevantForNational
                || fundingStream.RelevantForOrganisations_Public
                || fundingStream.RelevantForProviders_Public)
               .ToList();

            return View(viewModel);
        }

        /// <summary>
        /// The MVC action for the 'Choose how to view funding' page.
        /// </summary>
        /// <param name="error">Whether or not to display the validation error messages.</param>
        /// <returns>The 'Choose how to view funding' page view.</returns>
        [Route(ViewYourFundingConstants.Route_ViewingChoice, Name = ViewYourFundingConstants.RouteName_ViewingChoice)]
        public virtual async Task<IActionResult> ViewingChoice(bool error = false)
        {
            var viewModel = await GetBasePageViewModel<ViewingChoiceViewModel>();

            viewModel.ValidationError = error;

            return View(viewModel);
        }

        /// <summary>
        /// The MVC action for routing the selection made on the 'Choose how to view funding' page (used when JavaScript is disabled).
        /// </summary>
        /// <param name="choice">The choice selected on the previous page.</param>
        /// <returns>A redirect to the relevant action based on the choice made.</returns>
        [Route(ViewYourFundingConstants.Route_ViewingChoiceChosen, Name = ViewYourFundingConstants.RouteName_ViewingChoiceChosen)]
        [HttpPost]
        public virtual IActionResult ViewingChoiceChosen(string choice)
        {
            if (ViewYourFundingConstants.OptionMap_ViewingChoice
                .TryGetValue(choice ?? string.Empty, out string routeName))
            {
                return RedirectToRoute(routeName);
            }

            return RedirectToRoute(ViewYourFundingConstants.RouteName_ViewingChoice, new { error = true });
        }

        /// <summary>
        /// The MVC action for the 'Select a funding type' page.
        /// </summary>
        /// <param name="error">Whether or not to display the validation error messages.</param>
        /// <returns>The 'Select a funding type' page view.</returns>
        [Route(ViewYourFundingConstants.Route_WhichAllocation, Name = ViewYourFundingConstants.RouteName_WhichAllocation)]
        public virtual async Task<IActionResult> WhichAllocation(bool error = false)
        {
            var userDetails = await GetUserAsync();
            var viewModel = await GetBasePageViewModel<WhichAllocationViewModel>(userDetails, true);
            viewModel.ValidationError = error;

            var allActiveFundingStreamsData = await GetRelevantFundingStreams(Enums.FundingUIViewType.National, userDetails, true);
            viewModel.LatestYear = await GetFundingStreamsLatestYear(allActiveFundingStreamsData);

            return View(viewModel);
        }

        /// <summary>
        /// The MVC action for routing the selection made on the 'Select a funding type' page (used when JavaScript is disabled).
        /// </summary>
        /// <param name="fundingAllocationChoice">The choice selected on the previous page.</param>
        /// <returns>A redirect to the relevant action based on the choice made.</returns>
        [Route(ViewYourFundingConstants.Route_WhichAllocationChosen, Name = ViewYourFundingConstants.RouteName_WhichAllocationChosen)]
        [HttpPost]
        public virtual async Task<IActionResult> WhichAllocationChosen(string fundingAllocationChoice)
        {
            var allActiveFundingStreamsData = await GetRelevantFundingStreams(Enums.FundingUIViewType.National);
            var latestYear = await GetFundingStreamsLatestYear(allActiveFundingStreamsData);

            if (ViewYourFundingConstants.OptionMap_WhichAllocation(latestYear)
               .TryGetValue(fundingAllocationChoice ?? string.Empty, out var whichAllocationOptionAction))
            {
                return RedirectToRoute(whichAllocationOptionAction.RouteName, whichAllocationOptionAction.RouteValues);
            }

            return RedirectToRoute(ViewYourFundingConstants.RouteName_WhichAllocation, new { error = true });
        }

        /// <summary>
        /// The MVC action for the 'National Funding Allocations' page.
        /// </summary>
        /// <param name="fundingStreamCode">The code of the funding allocations being displayed.</param>
        /// <param name="yearFrom">The start year of the funding allocations being displayed.</param>
        /// <param name="yearTo">The end year of the funding allocations being displayed.</param>
        /// <param name="previewLayoutModel">The preview layout model.</param>
        /// <returns>The National Funding Allocations page view.</returns>
        [PreviewLayoutAction]
        [Route(ViewYourFundingConstants.Route_NationalFundingAllocation, Name = ViewYourFundingConstants.RouteName_NationalFundingAllocation)]
        public virtual async Task<IActionResult> NationalFundingAllocation(
            string fundingStreamCode,
            int yearFrom,
            int yearTo,
            PreviewLayoutModel previewLayoutModel = null)
        {
            var errorMessagePrefix = "A problem occurred during processing. ";

            var userDetails = await GetUserAsync();
            var viewModel = await GetBasePageViewModel<NationalFundingAllocationViewModel>(userDetails, true);

            // Normalise funding stream code to uppercase.
            fundingStreamCode = fundingStreamCode.ToUpperInvariant();

            // Get the configuration data related to this funding stream
            var fundingStream = await GetNationalFundingStream(fundingStreamCode, userDetails);

            viewModel.FundingStreamCode = fundingStreamCode;
            viewModel.FundingStreamName = fundingStream?.FundingStreamName;
            viewModel.YearFrom = yearFrom;
            viewModel.YearTo = yearTo;
            viewModel.ValidationError = false;
            viewModel.ValidationErrorMessage = string.Empty;

            if (fundingStream == null)
            {
                viewModel.ValidationError = true;
                viewModel.ValidationErrorMessage = $"{errorMessagePrefix}No funding stream configurations were found for funding stream '{fundingStreamCode}'.";

                return View(viewModel);
            }

            viewModel.CanUseShortCode = fundingStream.FundingStreamCodePubliclyKnown;

            if (fundingStream?.Publications?.Any() != true)
            {
                viewModel.ValidationError = true;
                viewModel.ValidationErrorMessage = $"{errorMessagePrefix}No publications were found for funding stream '{fundingStreamCode}'.";

                return View(viewModel);
            }

            var fundingPeriodCode = GetFundingPeriodCode(fundingStream, yearFrom, yearTo);
            var publications = fundingStream.Publications
                .Where(publication => publication.Year1 == yearFrom && publication.Year2 == yearTo)
                .ToList();

            var previewModeEnabled = await PreviewModeEnabled(userDetails, true);

            var activeFundingPeriodCodes = FundingPeriodHelper.GetActiveFundingPeriodCodes(fundingStream, previewModeEnabled);

            // Set the spreadsheet publication badge for 'Latest' or 'Final'
            var latestFundingPeriodCodes = FundingPeriodHelper.GetLatestFundingPeriodCodes_FundingPeriodFormat(fundingStream.SettingValues, activeFundingPeriodCodes);
            var isCurrentYear = latestFundingPeriodCodes.Contains(fundingPeriodCode);

            // Get the spreadsheets related to this funding stream
            var spreadsheets = await GetSpreadsheets(fundingStreamCode, fundingStream.FundingStreamName, fundingPeriodCode, yearFrom, yearTo);

            if (spreadsheets == null || !spreadsheets.Any())
            {
                viewModel.ValidationError = true;
                viewModel.ValidationErrorMessage = $"{errorMessagePrefix}No spreadsheets were found for funding stream code '{fundingStreamCode}', funding stream name '{fundingStream.FundingStreamName}' and funding period code '{fundingPeriodCode}'.";

                return View(viewModel);
            }

            // Get the spreadsheets that have the same published date as the publications published date
            var matchingSpreadsheetsForPublications = spreadsheets
                .Where(s => publications.Any(p => p.PublishedDate == s.DocumentPublishedDate))
                .OrderByDescending(spreadsheet => spreadsheet.DocumentPublishedDate).ToList();

            if (matchingSpreadsheetsForPublications == null || !matchingSpreadsheetsForPublications.Any())
            {
                viewModel.ValidationError = true;
                viewModel.ValidationErrorMessage = $"{errorMessagePrefix}No Publications were found with a published date matching any of the Spreadsheet published dates for funding stream '{fundingStreamCode}'.";

                return View(viewModel);
            }

            // Set the 'Latest' or 'Final' badge on the latest spreadsheet for the year
            await SetLatestSpreadsheetFinalAndLatestFlags(matchingSpreadsheetsForPublications, fundingStream, fundingPeriodCode);
            viewModel.Spreadsheets = matchingSpreadsheetsForPublications;

            // publications is null and value checked above so no need for null check here.
            var publication = publications.FirstOrDefault();
            var cutOffDate = publication.CutOffDate.HasValue ? publication.CutOffDate.Value : new DateTime(yearTo, 12, 31);
            var componentDefaults = GetComponentDefaults();

            var showSelectors = await GetShowSelectorsState();
            var asStatementSpecification = await GetStatementSpecificationState();
            var showData = await GetShowData();

            var fundingViewData = await _fundingViewService.GenerateFundingViewData(
                _componentService,
                publication.FundingPeriodCode,
                fundingStream.FundingStreamCode,
                new[] { fundingStream },
                cutOffDate,
                new Publication
                {
                    PublishedDate = publication.PublishedDate,
                    FundingStreamId = publication.FundingStreamId,
                    PublicationLayouts = new List<PublicationLayout>
                    {
                        new PublicationLayout
                        {
                            FundingViewType = FundingViewType.ViewData,
                            FundingViewScope = FundingViewScope.National,
                            LayoutId = AsWebModel(fundingStream).NationalUiLayoutId
                        }
                    }
                },
                publication.UIModelVersion,
                FundingViewScope.National,
                componentDefaults,
                null,
                false,
                isCurrentYear,
                previewLayoutModel: previewLayoutModel,
                showSelectors: showSelectors,
                asStatementSpecification: asStatementSpecification,
                showData: showData);

            viewModel.FundingViewData = fundingViewData;

            viewModel.HistoricYears = GetHistoricYears(yearFrom);
            var currentYears =
                FundingPeriodHelper.GetYearsFromCode(
                    fundingStream.CurrentFundingPeriodCode_FromPublications(previewModeEnabled));
            var latestHistoricYears = GetHistoricYears(currentYears.yearFrom);

            foreach (var historicYear in latestHistoricYears.Where(historicYear => historicYear.yearFrom != yearFrom))
            {
                viewModel.LatestHistoricYears.Add(historicYear);
            }

            var latestYears = FundingPeriodHelper.GetLatestYears(fundingStream.SettingValues, activeFundingPeriodCodes)
                .OrderByDescending(year => year.yearFrom).ToList();
            var futureYears = new List<(int, int)>();
            var currentYearsBeforeThisYear = new List<(int, int)>();

            foreach (var years in latestYears)
            {
                if (years.yearFrom == yearFrom)
                {
                    continue;
                }

                if (isCurrentYear)
                {
                    currentYearsBeforeThisYear.Add(years);
                    viewModel.HistoricYears.Remove(years);
                }
                else if (years.yearFrom > yearFrom)
                {
                    futureYears.Add(years);
                }
            }

            // Chnage to make sure only three years worth of data is available on history results page
            DisplayHelper.UpdateAllocationHistoryListToDisplay(futureYears, currentYearsBeforeThisYear, viewModel.HistoricYears);

            var pageData = viewModel.FundingViewData?.Components?.FirstOrDefault()?.PageData;
            if (pageData != null)
            {
                pageData.Add(nameof(viewModel.HistoricYears), viewModel.HistoricYears);
                pageData.Add(nameof(viewModel.LatestHistoricYears), viewModel.LatestHistoricYears);
                pageData.Add("FutureYears", futureYears);
                pageData.Add("CurrentYearsBeforeThisYear", currentYearsBeforeThisYear);
                pageData.Add("currentYearFrom", yearFrom);
                pageData.Add("currentYearTo", yearTo);
            }

            return View(viewModel);
        }

        /// <summary>
        /// The MVC action to download a provider level document.
        /// </summary>
        /// <param name="request">The provider spreadsheet download request.</param>
        /// <param name="previewLayoutModel">The preview layout model.</param>
        /// <returns>A spreadsheet for a provider level document.</returns>
        [PreviewLayoutAction]
        [Route(ViewYourFundingConstants.Route_ProviderSpreadsheetDownload, Name = ViewYourFundingConstants.RouteName_ProviderSpreadsheetDownload)]
        public virtual async Task<IActionResult> ProviderSpreadsheetDownload([FromRoute] ProviderSpreadsheetDownloadRequest request, PreviewLayoutModel previewLayoutModel = null)
        {
            var fundingStreams_public = await GetRelevantFundingStreams(Enums.FundingUIViewType.Providers_Public);

            if (fundingStreams_public?.ContainsKey(request.FundingStreamCode) == false)
            {
                throw new InvalidOperationException("This operation isn't valid.");
            }

            var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(request.YearFrom, request.YearTo, request.YearTypeCode);
            var contentType = string.IsNullOrWhiteSpace(request.Format) ? FundingDocumentFileType.FileFormats[FundingDocumentFileType.Spreadsheet_OpenFormat] : FundingDocumentFileType.FileFormats[request.Format];
            var publishedDate = request.PublishedDate.ToRouteParameterDate();
            Enum.TryParse(request.Format, true, out FileFormat fileFormat);
            var previewModeEnabled = await PreviewModeEnabled();
            var fundingStream = fundingStreams_public[request.FundingStreamCode];

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
                FundingViewScope.Provider,
                new[] { fileFormat },
                filters,
                previewLayoutModel)).First();

            return File(fundingDocument.Data, contentType, fundingDocument.Filename);
        }

        /// <summary>
        /// The MVC action for the 'Under construction' page.
        /// </summary>
        /// <returns>The 'Under construction' page view.</returns>
        [Route(ViewYourFundingConstants.Route_UnderConstruction, Name = ViewYourFundingConstants.RouteName_UnderConstruction)]
        public virtual async Task<IActionResult> UnderConstruction()
        {
            var viewModel = await GetBasePageViewModel<UnderConstructionViewModel>();

            return View(viewModel);
        }

        #endregion


        #region View funding at organisation level journey

        /// <summary>
        /// The MVC action for the 'View funding at organisation level' page.
        /// </summary>
        /// <param name="validationErrorInputId">Which search box has been submitted.</param>
        /// <returns>The 'View funding at organisation level' page view.</returns>
        [Route(ViewYourFundingConstants.Route_FindAnOrganisation, Name = ViewYourFundingConstants.RouteName_FindAnOrganisation)]
        public virtual async Task<IActionResult> FindAnOrganisation(string validationErrorInputId = null)
        {
            var viewModel = await GetBasePageViewModel<FindAnOrganisationViewModel>();

            viewModel.ValidationErrorInputId = validationErrorInputId;

            return View(viewModel);
        }

        /// <summary>
        /// The MVC Action for the Provider 'Schools or academy search' page.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>The Redirect result depending on the results.</returns>
        [Route(ViewYourFundingConstants.Route_ProviderSearch, Name = ViewYourFundingConstants.RouteName_ProviderSearch)]
        public virtual async Task<IActionResult> ProviderSearch(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToRoute(
                    ViewYourFundingConstants.RouteName_FindAnOrganisation,
                    new { validationErrorInputId = "provider" });
            }

            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Providers_Public);

            var viewModelProviderResults = await DoProviderFundingSearch(
                searchTerm,
                fundingStreams,
                GetProviderParentGroupingType(fundingStreams),
                GroupingReason.Information);

            if (viewModelProviderResults?.Any() != true)
            {
                return RedirectToRoute(
                    ViewYourFundingConstants.RouteName_ProviderNoResults,
                    new { searchTerm });
            }

            var resultsGroupedByUkprn = viewModelProviderResults.GroupBy(result => result.OrganisationUkprn).ToList();

            if (resultsGroupedByUkprn.Count == 1)
            {
                return RedirectToRoute(
                    ViewYourFundingConstants.RouteName_ProviderStatement,
                    new { organisationUkprn = viewModelProviderResults.First().OrganisationUkprn });
            }

            return RedirectToRoute(
                ViewYourFundingConstants.RouteName_ProviderDidYouMean,
                new { searchTerm });
        }

        /// <summary>
        /// The MVC Action for the Provider 'Schools or academy search results' page.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="appliedFilterModel">The query filter view model.</param>
        /// <returns>The 'Provider results' or 'Provider Details' view.</returns>
        [Route(ViewYourFundingConstants.Route_ProviderDidYouMean, Name = ViewYourFundingConstants.RouteName_ProviderDidYouMean)]
        public virtual async Task<IActionResult> ProviderDidYouMean(string searchTerm, QueryFilterViewModel appliedFilterModel)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToRoute(
                    ViewYourFundingConstants.RouteName_FindAnOrganisation,
                    new { validationErrorInputId = "provider" });
            }

            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Providers_Public);

            var viewModelProviderResults = await DoProviderFundingSearch(
                searchTerm,
                fundingStreams,
                GetProviderParentGroupingType(fundingStreams),
                GroupingReason.Information);

            if (viewModelProviderResults?.Any() != true)
            {
                return RedirectToRoute(
                    ViewYourFundingConstants.RouteName_ProviderNoResults,
                    new { searchTerm });
            }

            if (viewModelProviderResults.Count == 1)
            {
                return RedirectToRoute(
                    ViewYourFundingConstants.RouteName_ProviderStatement,
                    new { organisationUkprn = viewModelProviderResults.First().OrganisationUkprn });
            }

            var viewModel = await GetBasePageViewModel<ProviderResultsViewModel>();
            viewModel.QueryFilterViewModel = BuildQueryFilter(viewModelProviderResults, searchTerm);

            if (appliedFilterModel.QueryFilter?.Filters
                .Any(filter => filter.Values.Any()) == true)
            {
                viewModelProviderResults = QueryFilterHelper.FilterSearchResults(appliedFilterModel.QueryFilter, viewModelProviderResults);
                viewModel.QueryFilterViewModel.QueryFilter.Merge(appliedFilterModel.QueryFilter);
            }

            var groupedResults = viewModelProviderResults
                .Where(result => result.TotalAmount > 0 || !"Closed".Equals(result.ProviderStatus, StringComparison.InvariantCultureIgnoreCase))
                .GroupBy(result => result.OrganisationUkprn)
                .Select(group => group.First())
                .ToList();

            viewModel.ProviderResults = groupedResults;
            viewModel.SearchTerm = searchTerm;
            viewModel.BackToTopLinkMinimumCount = _applicationConfiguration.BackToTopLinkMinimumCount;

            return View(viewModel);
        }

        /// <summary>
        /// The MVC Action for the Provider 'Schools or academy no search results' page.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>The 'no provider search result' view.</returns>
        [Route(ViewYourFundingConstants.Route_ProviderNoResults, Name = ViewYourFundingConstants.RouteName_ProviderNoResults)]
        public virtual async Task<IActionResult> ProviderNoResults(string searchTerm)
        {
            var viewModel = await GetBasePageViewModel<ProviderNoResultsViewModel>();
            viewModel.SearchTerm = searchTerm;

            return View(viewModel);
        }

        /// <summary>
        /// The MVC Action for the 'Provider details' page.
        /// </summary>
        /// <param name="organisationUkprn">The organisation Ukprn.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="previewLayoutModel">The Preview Layout model.</param>
        /// <returns>The 'Provider details' or 'Provider results' view.</returns>
        [PreviewLayoutAction]
        [Route(ViewYourFundingConstants.Route_ProviderStatement, Name = ViewYourFundingConstants.RouteName_ProviderStatement)]
        public virtual async Task<IActionResult> ProviderStatement(
            string organisationUkprn,
            string searchTerm = null,
            PreviewLayoutModel previewLayoutModel = null)
        {
            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Providers_Public);

            var fundingViewData = new Dictionary<string, FundingViewData>();
            var defaultConfigDictionary = GetComponentDefaults();

            if (string.IsNullOrWhiteSpace(organisationUkprn))
            {
                return RedirectToRoute(ViewYourFundingConstants.RouteName_ProviderDidYouMean, new { searchTerm = organisationUkprn });
            }

            var viewModelProviderResults = await DoProviderFundingSearch(
                organisationUkprn,
                fundingStreams,
                GetProviderParentGroupingType(fundingStreams),
                GroupingReason.Information);

            var results = viewModelProviderResults?.Where(provider => provider.OrganisationUkprn.Equals(organisationUkprn)).ToList();

            var model = await GetBasePageViewModel<ProviderStatementViewModel>();
            model.SearchTerm = searchTerm;
            model.OrganisationUkprn = organisationUkprn;

            var showSelectors = await GetShowSelectorsState();
            var asStatementSpecification = await GetStatementSpecificationState();
            var showData = await GetShowData();

            if (results != null)
            {
                foreach (var result in results.OrderByDescending(provider => provider.StatusChangedDate))
                {
                    var section = new ProviderStatementSectionViewModel
                    {
                        ProviderResult = result
                    };

                    var fundingStream = fundingStreams[result.FundingStreamCode];
                    var nextPaymentDateTypeCode = GetNextAllocationPaymentDateTypeCode(result);

                    if (!section.OrganisationClosed)
                    {
                        var fundingPeriodCode = fundingStream.CurrentFundingPeriodCode_FromPublications(await PreviewModeEnabled());

                        section.NextPaymentDate = PaymentTypeCode.GetNextPaymentDate(
                            fundingStream.NextPayments,
                            nextPaymentDateTypeCode,
                            fundingPeriodCode);

                        section.NoNextPaymentForTheYearText =
                            PaymentTypeCode.GetNoNextPaymentForTheYearText(fundingPeriodCode);
                    }

                    var publication = fundingStream.GetLatestPublication(await PreviewModeEnabled());

                    if (publication == null)
                    {
                        continue;
                    }

                    var cutOffDate = FundingPeriodHelper.GetCutOffDateForPublication(publication);

                    section.SearchTerm = searchTerm;
                    section.FundingStreamConfiguration = AsWebModel(fundingStream);
                    PopulateStreamDataForViewModel(section, publication.FundingPeriodCode);

                    var fundingDocument = GetProviderFundingDocument(
                        result,
                        publication.PublishedDate,
                        fundingStream,
                        false);

                    var filters = new[]
                    {
                        new SearchFilter
                        {
                            PropertyName = SearchFilterPropertyName.PrimaryIdentifier,
                            PropertyValue = organisationUkprn
                        }
                    };

                    var fundingStreamData = await _fundingViewService.GenerateFundingViewData(
                        _componentService,
                        publication.FundingPeriodCode,
                        fundingStream.FundingStreamCode,
                        new[] { fundingStream },
                        cutOffDate,
                        publication,
                        publication.UIModelVersion,
                        FundingViewScope.ProviderSummary,
                        defaultConfigDictionary,
                        fundingDocument,
                        true,
                        true,
                        filters: filters,
                        searchTerm: searchTerm,
                        bubbleUpException: false,
                        iFundingApiSearchProviderFunding: new[] { result },
                        previewLayoutModel: previewLayoutModel,
                        showSelectors: showSelectors,
                        asStatementSpecification: asStatementSpecification,
                        showData: showData);

                    if (fundingStreamData == null)
                    {
                        continue;
                    }

                    if (fundingViewData.ContainsKey(result.FundingStreamCode))
                    {
                        continue;
                    }

                    fundingViewData.Add(result.FundingStreamCode, fundingStreamData);
                }
            }

            model.FundingViewData = fundingViewData;

            if (fundingViewData?.Any() != true)
            {
                return RedirectToRoute(ViewYourFundingConstants.RouteName_ProviderDidYouMean, new { searchTerm = organisationUkprn });
            }

            return View(model);
        }

        /// <summary>
        /// The MVC Action for the 'Provider History' page.
        /// </summary>
        /// <param name="fundingStreamName">The funding stream name (made url safe e.g. pe-and-sport-premium).</param>
        /// <param name="organisationUkprn">The organisation Ukprn.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="previewLayoutModel">The preview layout viewmodel.</param>
        /// <returns>The 'Provider History' view.</returns>
        [PreviewLayoutAction]
        [Route(ViewYourFundingConstants.Route_ProviderHistory, Name = ViewYourFundingConstants.RouteName_ProviderHistory)]
        public virtual async Task<IActionResult> ProviderHistory(
            string fundingStreamName,
            string organisationUkprn,
            string searchTerm = null,
            PreviewLayoutModel previewLayoutModel = null)
        {
            if (string.IsNullOrEmpty(organisationUkprn))
            {
                return RedirectToRoute(ViewYourFundingConstants.RouteName_ProviderDidYouMean, new { searchTerm = organisationUkprn });
            }

            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Providers_Public);
            var fundingStreamCode = GetFundingStreamCode(fundingStreamName, fundingStreams);
            var fundingStream = fundingStreams[fundingStreamCode];

            var publication = fundingStream.GetLatestPublication(await PreviewModeEnabled());

            if (publication == null)
            {
                throw new Exception($"There are no publications for funding stream {fundingStreamCode}");
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

            var providerFundingMatchesTask = ProviderFundingMatchesTask(organisationUkprn, fundingStream, publication);

            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(publication.FundingPeriodCode);
            var years = FundingPeriodHelper.GetCurrentAndHistoricYears(yearFrom, yearTo, ViewYourFundingConstants.NumberOfYearsOfHistoricAllocationsToShow);
            var fundingPeriodPublications = GroupPublicationsByYear(years, fundingStream, yearFrom);

            var providerFundingMatches = await providerFundingMatchesTask;

            if (providerFundingMatches?.ProviderFunding?.Any() != true)
            {
                throw new Exception($"There is no allocation history for this provider {fundingStreamCode}");
            }

            CheckAndRemoveProviderFundingPublications(fundingPeriodPublications, providerFundingMatches);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = HttpUtility.HtmlEncode(searchTerm);
            }

            var viewModel = await GetProviderHistoryViewModel(
                organisationUkprn,
                searchTerm,
                providerFundingMatches,
                fundingPeriodPublications,
                fundingStream);

            var defaultConfigDictionary = GetComponentDefaults();

            var result = providerFundingMatches?.ProviderFunding?.FirstOrDefault();

            var showSelectors = await GetShowSelectorsState();
            var asStatementSpecification = await GetStatementSpecificationState();
            var showData = await GetShowData();

            viewModel.FundingViewData = await _fundingViewService.GenerateFundingViewData(
                _componentService,
                publication.FundingPeriodCode,
                fundingStream.FundingStreamCode,
                new[] { fundingStream },
                DateTime.MinValue,
                new Publication { PublishedDate = DateTime.MinValue, FundingStreamId = publication.FundingStreamId },
                null,
                FundingViewScope.ProviderHistory,
                defaultConfigDictionary,
                null,
                true,
                false,
                searchTerm: searchTerm,
                bubbleUpException: false,
                iFundingApiSearchProviderFunding: result != null ? new[] { result } : null,
                previewLayoutModel: previewLayoutModel,
                showSelectors: showSelectors,
                asStatementSpecification: asStatementSpecification,
                showData: showData);

            var pageData = viewModel.FundingViewData?.Components?.FirstOrDefault()?.PageData;
            pageData?.Add("FundingPeriodPublications", viewModel.FundingPeriodPublications);

            return View(viewModel);
        }

        /// <summary>
        /// Providers funding breakdown.
        /// </summary>
        /// <param name="fundingStreamName">The funding stream name (made url safe e.g. pe-and-sport-premium).</param>
        /// <param name="organisationUkprn">The organisation ukprn.</param>
        /// <param name="publishDate">The publish date.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>The action result.</returns>
        [Route(ViewYourFundingConstants.Route_ProviderFundingBreakdown, Name = ViewYourFundingConstants.RouteName_ProviderFundingBreakdown)]
        public virtual async Task<IActionResult> ProviderFundingBreakdown(
            string fundingStreamName,
            string organisationUkprn,
            string publishDate,
            int yearFrom,
            int yearTo,
            string searchTerm = null)
        {
            if (!string.IsNullOrWhiteSpace(organisationUkprn))
            {
                var publishedDate = publishDate.ToRouteParameterDate();

                var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Providers_Public);
                var fundingStreamCode = GetFundingStreamCode(fundingStreamName, fundingStreams);
                var fundingStream = fundingStreams[fundingStreamCode];

                var yearTypeCode = FundingPeriodHelper.GetYearSettingCode(fundingStream.SettingValues);
                var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(yearFrom, yearTo, yearTypeCode);

                var viewModelProviderResults = await DoProviderFundingSearch(
                    organisationUkprn,
                    fundingStreams,
                    GetProviderParentGroupingType(fundingStreams),
                    GroupingReason.Information,
                    fundingPeriodCode,
                    publishedDate);

                var providerDetails = viewModelProviderResults?
                    .OrderByDescending(provider => provider.StatusChangedDate)
                    .FirstOrDefault(provider => provider.OrganisationUkprn.Equals(organisationUkprn));

                if (providerDetails != null)
                {
                    var viewModel = await GetProviderFundingBreakdownViewModel(
                        yearFrom,
                        yearTo,
                        searchTerm,
                        publishedDate,
                        providerDetails,
                        fundingStream,
                        fundingPeriodCode);
                    var previewModeEnabled = await PreviewModeEnabled();

                    var publication = fundingStream.Publications
                        .Where(publication => publication.Status == PublicationStatus.Published
                            || (previewModeEnabled && publication.Status == PublicationStatus.Preview))
                        .FirstOrDefault(p => p.PublishedDate == publishedDate);

                    if (publication == null)
                    {
                        throw new ArgumentOutOfRangeException($"There are no publications for the date {publishedDate}");
                    }

                    var cutOffDate = FundingPeriodHelper.GetCutOffDateForPublication(publication);

                    var latestPublishedDateForPeriod = fundingStream.Publications
                        .Where(publication => publication.FundingPeriodCode == fundingPeriodCode
                            && (publication.Status == PublicationStatus.Published
                            || (previewModeEnabled && publication.Status == PublicationStatus.Preview)))
                        .Max(p => p.PublishedDate);

                    var isLatestOrFinalFundingForYear = latestPublishedDateForPeriod == publishedDate;
                    var isCurrentYear = fundingPeriodCode == publication.FundingPeriodCode;

                    var showSelectors = await GetShowSelectorsState();
                    var asStatementSpecification = await GetStatementSpecificationState();
                    var showData = await GetShowData();

                    var fundingViewDataTask = _fundingViewService.GenerateFundingViewData(
                        _componentService,
                        publication.FundingPeriodCode,
                        fundingStream.FundingStreamCode,
                        new[] { fundingStream },
                        cutOffDate,
                        publication,
                        publication.UIModelVersion,
                        FundingViewScope.Provider,
                        GetComponentDefaults(),
                        viewModel.ProviderStatementSection.Document,
                        isLatestOrFinalFundingForYear,
                        isCurrentYear,
                        searchTerm: searchTerm,
                        bubbleUpException: false,
                        iFundingApiSearchProviderFunding: new[] { providerDetails },
                        showSelectors: showSelectors,
                        asStatementSpecification: asStatementSpecification,
                        showData: showData);

                    viewModel.FundingViewData = await fundingViewDataTask;

                    return View(viewModel);
                }
            }

            return RedirectToRoute(ViewYourFundingConstants.RouteName_ProviderDidYouMean, new { searchTerm = organisationUkprn });
        }

        /// <summary>
        /// The MVC Action for the 'Provider Funding Breakdown' page.
        /// </summary>
        /// <param name="fundingStreamName">The funding stream name (e.g. Dedicated schools grant).</param>
        /// <param name="year1">The first year.</param>
        /// <param name="year2">The second year.</param>
        /// <param name="organisationUkprn">The organisation ukprn.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="previewLayoutModel">The preview layout model.</param>
        /// <returns>The 'Provider Funding Breakdown' view.</returns>
        [PreviewLayoutAction]
        [Route(ViewYourFundingConstants.Route_ProviderHistorySingleYear, Name = ViewYourFundingConstants.RouteName_ProviderHistorySingleYear)]
        public virtual async Task<IActionResult> ProviderHistorySingleYear(
            string fundingStreamName,
            int year1,
            int year2,
            string organisationUkprn,
            string searchTerm = null,
            PreviewLayoutModel previewLayoutModel = null)
        {
            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Providers_Public);
            var fundingStreamCode = GetFundingStreamCode(fundingStreamName, fundingStreams);

            if (!string.IsNullOrWhiteSpace(organisationUkprn))
            {
                var viewModelProviderResults = await DoProviderFundingSearch(
                    organisationUkprn,
                    fundingStreams,
                    GetProviderParentGroupingType(fundingStreams),
                    GroupingReason.Information);

                var providerDetails = viewModelProviderResults?.FirstOrDefault(provider => provider.OrganisationUkprn.Equals(organisationUkprn));

                if (providerDetails != null)
                {
                    var defaultConfigDictionary = GetComponentDefaults();

                    var fundingStream = fundingStreams[fundingStreamCode];

                    var originalYearType = FundingPeriodHelper.GetYearTypeCodeFromFundingPeriodCode(fundingStream.CurrentFundingPeriodCode_FromPublications(await PreviewModeEnabled()));
                    var newFundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(year1, year2, originalYearType);

                    var showSelectors = await GetShowSelectorsState();
                    var asStatementSpecification = await GetStatementSpecificationState();
                    var showData = await GetShowData();

                    var viewModel = await GetDownloadViewModel<ProviderHistorySingleYearViewModel>(year1, year2, fundingStream, true, false);
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        searchTerm = HttpUtility.HtmlEncode(searchTerm);
                    }

                    viewModel.SearchTerm = searchTerm;
                    viewModel.OrganisationUkprn = organisationUkprn;
                    viewModel.OrganisationName = providerDetails.OrganisationName;
                    viewModel.FundingStreamName = fundingStreams[fundingStreamCode].FundingStreamName;
                    viewModel.FundingStreamCode = fundingStreamCode;

                    viewModel.FundingViewData = await _fundingViewService.GenerateFundingViewData(
                        _componentService,
                        newFundingPeriodCode,
                        fundingStream.FundingStreamCode,
                        new[] { fundingStream },
                        DateTime.MinValue,
                        fundingStream.GetLatestPublication(await PreviewModeEnabled()),
                        null,
                        FundingViewScope.ProviderHistorySingleYear,
                        defaultConfigDictionary,
                        null,
                        true,
                        false,
                        searchTerm: searchTerm,
                        bubbleUpException: false,
                        iFundingApiSearchProviderFunding: providerDetails != null ? new[] { providerDetails } : null,
                        previewLayoutModel: previewLayoutModel,
                        showSelectors: showSelectors,
                        asStatementSpecification: asStatementSpecification,
                        showData: showData);

                    var pageData = viewModel.FundingViewData?.Components?.FirstOrDefault()?.PageData;
                    pageData?.Add("Spreadsheets", viewModel.Spreadsheets);

                    return View(viewModel);
                }
            }

            return RedirectToRoute(ViewYourFundingConstants.RouteName_ProviderDidYouMean, new { searchTerm = organisationUkprn });
        }

        /// <summary>
        /// The MVC Action for the Local Authority search. Result will redirect based on the number of search results found.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>A conditional redirect to the relevant route.</returns>
        [Route(ViewYourFundingConstants.Route_LocalAuthoritySearch, Name = ViewYourFundingConstants.RouteName_LocalAuthoritySearch)]
        public virtual async Task<IActionResult> LocalAuthoritySearch(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToRoute(
                    ViewYourFundingConstants.RouteName_FindAnOrganisation,
                    new { validationErrorInputId = "laSearch" });
            }

            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Organisations_Public);
            var searchResults = await _fundingApiService.SearchLocalAuthorities(
                new FundingApiSearchLocalAuthoritiesRequest
                {
                    SearchTerm = searchTerm,
                    FundingStreamConfiguration = fundingStreams,
                    PreviewModeEnabled = await PreviewModeEnabled()
                });

            if (searchResults?.LocalAuthorities?.Any() != true)
            {
                return RedirectToRoute(
                    ViewYourFundingConstants.RouteName_LocalAuthorityNoResults,
                    new { searchTerm });
            }

            if (searchResults.LocalAuthorities.Count == 1)
            {
                var localAuthority = searchResults.LocalAuthorities.Single();

                return RedirectToRoute(
                    ViewYourFundingConstants.RouteName_LocalAuthorityStatement,
                    new { localAuthorityCode = localAuthority.Key });
            }

            return RedirectToRoute(
                ViewYourFundingConstants.RouteName_LocalAuthorityDidYouMean,
                new { searchTerm });
        }

        /// <summary>
        /// The MVC action for the 'View funding breakdown' page.
        /// </summary>
        /// <param name="request">Parameters that will be passed into action method.</param>
        /// <param name="appliedFilters">A model representing the filters that have been applied.</param>
        /// <param name="previewLayoutModel">The Preview layout view model.</param>
        /// <returns>The 'View funding breakdown' page view.</returns>
        [PreviewLayoutAction]
        [Route(ViewYourFundingConstants.Route_LocalAuthorityFundingBreakdown, Name = ViewYourFundingConstants.RouteName_LocalAuthorityFundingBreakdown)]
        public async Task<IActionResult> LocalAuthorityFundingBreakdown(
            LocalAuthorityFundingBreakdownRequest request,
            QueryFilterViewModel appliedFilters = null,
            PreviewLayoutModel previewLayoutModel = null)
        {
            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Organisations_Public);
            var fundingStreamCode = GetFundingStreamCode(request.FundingStreamName, fundingStreams);
            request.FundingStreamCode = fundingStreamCode;

            if (!fundingStreams.TryGetValue(fundingStreamCode, out var fundingStream))
            {
                throw new Exception($"Configuration not found for {fundingStreamCode}");
            }

            var publishedDate = request.PublishedDate.ToRouteParameterDate();
            var previewModeEnabled = await PreviewModeEnabled();

            var yearTypeCode = FundingPeriodHelper.GetYearSettingCode(fundingStream.SettingValues);
            var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(request.YearFrom, request.YearTo, yearTypeCode);

            var publication = fundingStream.Publications
                .Where(publication => publication.Status == PublicationStatus.Published
                    || (previewModeEnabled && publication.Status == PublicationStatus.Preview))
                .FirstOrDefault(p => p.PublishedDate == publishedDate && p.FundingPeriodCode == fundingPeriodCode);

            if (publication == null)
            {
                throw new ArgumentOutOfRangeException($"There are no publications for the date {publishedDate}");
            }

            var cutOffDate = FundingPeriodHelper.GetCutOffDateForPublication(publication);

            var filters = new[]
            {
                new SearchFilter
                {
                    PropertyName = SearchFilterPropertyName.ParentPrimaryIdentifier,
                    PropertyValue = request.LocalAuthorityCode
                }
            };

            var viewModel = await GetBasePageViewModel<FundingBreakdownViewModel>();
            viewModel.IncludeHistory = request.IncludeHistory;
            viewModel.Document = GetFundingDocumentForOrganisation(
                fundingStream,
                fundingPeriodCode,
                publishedDate,
                request.LocalAuthorityCode);

            var latestPublicationForPeriod = await GetLatestPublicationConsideringTerminationDate(request.LocalAuthorityCode, fundingPeriodCode, fundingStream);

            viewModel.IsLatestOrFinalFundingForYear = latestPublicationForPeriod.PublishedDate == publishedDate;
            viewModel.IsCurrentYear = fundingPeriodCode == publication.FundingPeriodCode;

            var showSelectors = await GetShowSelectorsState();
            var asStatementSpecification = await GetStatementSpecificationState();
            var showData = await GetShowData();

            var fundingViewDataTask = _fundingViewService.GenerateFundingViewData(
                _componentService,
                publication.FundingPeriodCode,
                fundingStream.FundingStreamCode,
                new[] { fundingStream },
                cutOffDate,
                publication,
                publication.UIModelVersion,
                FundingViewScope.Organisation,
                GetComponentDefaults(),
                viewModel.Document,
                viewModel.IsLatestOrFinalFundingForYear,
                viewModel.IsCurrentYear,
                filters: filters,
                searchTerm: request.SearchTerm,
                selectedTab: request.Tab,
                bubbleUpException: false,
                previewLayoutModel: previewLayoutModel,
                showSelectors: showSelectors,
                asStatementSpecification: asStatementSpecification,
                showData: showData);

            viewModel.SearchTerm = request.SearchTerm;
            viewModel.YearFrom = request.YearFrom;
            viewModel.YearTo = request.YearTo;
            viewModel.LocalAuthorityCode = request.LocalAuthorityCode;
            viewModel.FundingStreamConfiguration = AsWebModel(fundingStream);
            viewModel.FundingPeriodCode = fundingPeriodCode;
            viewModel.Tab = request.Tab;

            var (asOfMonth, asOfYear) = FundingPublicationDateHelper.GetAcademicAsOfData(
                publishedDate,
                request.YearFrom,
                request.YearTo);

            viewModel.AsOfMonth = asOfMonth;
            viewModel.AsOfYear = asOfYear;

            var fundingViewData = await fundingViewDataTask;
            fundingViewData.FundingSubData = fundingViewData.FundingSubData?.Where(providerFunding =>
            {
                var providerType = ProviderTypeInternal
                    .FromExternal(providerFunding.ProviderType, providerFunding.ProviderSubType)
                    .RemoveWhitespace();

                return !(providerType == null || ("Closed".Equals(providerFunding.ProviderStatus, StringComparison.InvariantCultureIgnoreCase) && providerFunding.TotalAmount == 0));
            }).ToList();

            viewModel.LocalAuthorityName = fundingViewData?.LocalAuthorityName;

            viewModel.QueryFilterViewModel = new QueryFilterViewModel
            {
                RouteName = ViewYourFundingConstants.RouteName_LocalAuthorityFundingBreakdown,
                SearchTerm = request.SearchTerm,
                QueryFilter = QueryFilterHelper.BuildQueryFilter(fundingViewData)
            };

            if (appliedFilters?.QueryFilter?.Filters
                .Any(filter => filter.Values.Any()) == true)
            {
                fundingViewData = QueryFilterHelper.ApplyFilters(appliedFilters.QueryFilter, fundingViewData);
                viewModel.QueryFilterViewModel.QueryFilter.Merge(appliedFilters.QueryFilter);
            }

            viewModel.FundingViewData = fundingViewData;
            viewModel.InitialIndicativeOrEmpty = GetInitialIndicativeOrEmpty(fundingViewData);
            viewModel.AfterRecoupmentOrEmpty = GetAfterRecoupmentOrEmpty(fundingViewData);
            viewModel.IsAfterRecoupment = IsAfterRecoupment(fundingViewData);
            viewModel.ImportExportAdjustmentYear1 = GetImportExportAdjustmentYear(fundingViewData, viewModel.YearFrom);
            viewModel.ImportExportAdjustmentYear2 = GetImportExportAdjustmentYear(fundingViewData, viewModel.YearFrom) - 1;

            return View(viewModel);
        }

        /// <summary>
        /// The MVC action for the 'no results' page following a local authority search.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>The 'no results' view.</returns>
        [Route(ViewYourFundingConstants.Route_LocalAuthorityNoResults, Name = ViewYourFundingConstants.RouteName_LocalAuthorityNoResults)]
        public virtual async Task<IActionResult> LocalAuthorityNoResults(string searchTerm)
        {
            var viewModel = await GetBasePageViewModel<LocalAuthorityNoResultsViewModel>();
            viewModel.SearchTerm = searchTerm;

            return View(viewModel);
        }

        /// <summary>
        /// The MVC action for the local authority search 'did you mean' page.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>The 'did you mean' view if >1 LAs match, otherwise a redirect based on the number of matches.</returns>
        [Route(ViewYourFundingConstants.Route_LocalAuthorityDidYouMean, Name = ViewYourFundingConstants.RouteName_LocalAuthorityDidYouMean)]
        public virtual async Task<IActionResult> LocalAuthorityDidYouMean(string searchTerm)
        {
            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Organisations_Public);
            var searchResults = await _fundingApiService.SearchLocalAuthorities(
                new FundingApiSearchLocalAuthoritiesRequest
                {
                    SearchTerm = searchTerm,
                    FundingStreamConfiguration = fundingStreams,
                    PreviewModeEnabled = await PreviewModeEnabled()
                });

            if (searchResults?.LocalAuthorities?.Any() != true)
            {
                return RedirectToRoute(
                    ViewYourFundingConstants.RouteName_LocalAuthorityNoResults,
                    new { searchTerm });
            }

            if (searchResults.LocalAuthorities.Count == 1)
            {
                var localAuthority = searchResults.LocalAuthorities.Single();

                return RedirectToRoute(
                    ViewYourFundingConstants.RouteName_LocalAuthorityStatement,
                    new { localAuthorityCode = localAuthority.Key });
            }

            var viewModel = await GetBasePageViewModel<LocalAuthorityDidYouMeanViewModel>();
            viewModel.SearchTerm = searchTerm;
            viewModel.LocalAuthorities = searchResults.LocalAuthorities.OrderBy(la => la.Value).ToList();

            var backToTopLinkMinimumCountKey = _applicationConfiguration.BackToTopLinkMinimumCount;
            viewModel.BackToTopLinkMinimumCount = backToTopLinkMinimumCountKey;

            return View(viewModel);
        }

        /// <summary>
        /// The MVC action for the local authority statement page.
        /// </summary>
        /// <param name="localAuthorityCode">The local authority code.</param>
        /// <param name="searchTerm">If arriving at this page via a search, the search term that was entered.</param>
        /// <param name="previewLayoutModel">The preview layout model.</param>
        /// <returns>The local authority statement page view.</returns>
        [PreviewLayoutAction]
        [Route(ViewYourFundingConstants.Route_LocalAuthorityStatement, Name = ViewYourFundingConstants.RouteName_LocalAuthorityStatement)]
        public virtual async Task<IActionResult> LocalAuthorityStatement(
            string localAuthorityCode,
            string searchTerm = null,
            PreviewLayoutModel previewLayoutModel = null)
        {
            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Organisations_Public);

            var fundingViewData = new Dictionary<string, FundingViewData>();
            var fundingDocuments = new List<FundingDocument>();
            var defaultConfigDictionary = GetComponentDefaults();

            var generateTasks = new List<Task<FundingViewData>>();
            var dataRequirements = new List<FundingApiSearchRequestObject>();

            var showSelectors = await GetShowSelectorsState();
            var asStatementSpecification = await GetStatementSpecificationState();
            var showData = await GetShowData();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = HttpUtility.HtmlEncode(searchTerm);
            }

            foreach (var fundingStreamCode in fundingStreams.Keys)
            {
                var fundingStream = fundingStreams[fundingStreamCode];
                var activeFundingPeriodCodes = FundingPeriodHelper.GetActiveFundingPeriodCodes(fundingStream, await PreviewModeEnabled());
                var applicableFundingPeriods = FundingPeriodHelper.GetLatestFundingPeriodCodes_FundingPeriodFormat(fundingStream.SettingValues, activeFundingPeriodCodes);

                foreach (var fundingPeriod in applicableFundingPeriods)
                {
                    var publication = await GetLatestPublicationConsideringTerminationDate(localAuthorityCode, fundingPeriod, fundingStream);

                    if (publication == null)
                    {
                        continue;
                    }

                    var cutOffDate = FundingPeriodHelper.GetCutOffDateForPublication(publication);
                    var filters = new[]
                    {
                        new SearchFilter
                        {
                            PropertyName = SearchFilterPropertyName.ParentPrimaryIdentifier,
                            PropertyValue = localAuthorityCode
                        }
                    };

                    dataRequirements.AddRange(_fundingViewService.GetDataRequirements(
                        fundingStream,
                        publication.FundingPeriodCode,
                        cutOffDate,
                        FundingViewScope.OrganisationSummary,
                        filters,
                        null,
                        null));
                }
            }

            var fundingRequestObject = SquashDataRequirements(dataRequirements, "Funding");

            var fundingData = fundingRequestObject != null ?
                await _fundingApiService.SearchFunding(fundingRequestObject) : null;

            var providerFundingRequestObject = SquashDataRequirements(dataRequirements, "ProviderFunding");

            var providerfundingData = providerFundingRequestObject != null ?
                await _fundingApiService.SearchProviderFunding(providerFundingRequestObject) : null;

            foreach (var fundingStreamCode in fundingStreams.Keys)
            {
                var fundingStream = fundingStreams[fundingStreamCode];
                var activeFundingPeriodCodes = FundingPeriodHelper.GetActiveFundingPeriodCodes(fundingStream, await PreviewModeEnabled());

                var applicableFundingPeriods = FundingPeriodHelper.GetLatestFundingPeriodCodes_FundingPeriodFormat(fundingStream.SettingValues, activeFundingPeriodCodes);

                foreach (var fundingPeriod in applicableFundingPeriods)
                {
                    var publication = await GetLatestPublicationConsideringTerminationDate(localAuthorityCode, fundingPeriod, fundingStream);

                    if (publication == null)
                    {
                        continue;
                    }

                    var cutOffDate = FundingPeriodHelper.GetCutOffDateForPublication(publication);

                    var fundingDocument = GetFundingDocumentForOrganisation(
                        fundingStream,
                        publication.FundingPeriodCode,
                        publication.PublishedDate,
                        localAuthorityCode);

                    fundingDocuments.Add(fundingDocument);

                    var funding = fundingData?.Funding?
                        .Where(funding => funding.FundingStreamCode == fundingStreamCode && funding.FundingPeriodCode == fundingPeriod).ToArray();
                    var providerFunding = providerfundingData?.ProviderFunding?
                        .Where(funding => funding.FundingStreamCode == fundingStreamCode && funding.FundingPeriodCode == fundingPeriod).ToArray();

                    var filters = new[]
                    {
                        new SearchFilter
                        {
                            PropertyName = SearchFilterPropertyName.ParentPrimaryIdentifier,
                            PropertyValue = localAuthorityCode
                        }
                    };

                    generateTasks.Add(_fundingViewService.GenerateFundingViewData(
                        _componentService,
                        publication.FundingPeriodCode,
                        fundingStream.FundingStreamCode,
                        new[] { fundingStream },
                        cutOffDate,
                        publication,
                        publication.UIModelVersion,
                        FundingViewScope.OrganisationSummary,
                        defaultConfigDictionary,
                        fundingDocument,
                        true,
                        true,
                        filters: filters,
                        searchTerm: searchTerm,
                        bubbleUpException: false,
                        iFundingApiSearchFunding: funding,
                        iFundingApiSearchProviderFunding: providerFunding,
                        explicitFundingPassed: fundingRequestObject != null,
                        explicitProviderFundingPassed: providerFundingRequestObject != null,
                        previewLayoutModel: previewLayoutModel,
                        showSelectors: showSelectors,
                        asStatementSpecification: asStatementSpecification,
                        showData: showData));
                }
            }

            await Task.WhenAll(generateTasks);

            foreach (var generateTask in generateTasks)
            {
                var fundingStreamData = await generateTask;

                if (fundingStreamData == null || string.IsNullOrEmpty(fundingStreamData.FundingStreamCode))
                {
                    continue;
                }

                fundingViewData.Add($"{fundingStreamData.FundingPeriodCode}-{fundingStreamData.FundingStreamCode}", fundingStreamData);
            }

            if (fundingViewData?.Any() != true)
            {
                throw new Exception($"No funding data found for local authority {localAuthorityCode}.");
            }

            var orderedFundingViewData = fundingViewData.OrderByDescending(viewData => viewData.Key)
                                                        .ToDictionary(viewData => viewData.Key, viewData => viewData.Value);

            var viewModel = await GetBasePageViewModel<LocalAuthorityStatementViewModel>();

            viewModel.SearchTerm = searchTerm;
            viewModel.LocalAuthorityCode = localAuthorityCode;
            viewModel.LocalAuthorityName = fundingViewData.First().Value.EntityName;
            viewModel.FundingViewData = orderedFundingViewData;
            viewModel.FundingStreamConfiguration = AsWebModel(fundingStreams);
            viewModel.FundingDocuments = fundingDocuments;

            return View(viewModel);
        }

        /// <summary>
        /// The MVC Action for the local authority 'Funding Breakdown' page for a single year.
        /// </summary>
        /// <param name="fundingStreamName">The funding stream name (e.g. Dedicated schools grant).</param>
        /// <param name="year1">The 'from' year.</param>
        /// <param name="year2">The 'to' year.</param>
        /// <param name="localAuthorityCode">The local authority code (e.g. 202).</param>
        /// <param name="searchTerm">The search term (e.g. Camden).</param>
        /// <param name="previewLayoutModel">The Preview Layout view model.</param>
        /// <returns>A 'Local Authority Funding Breakdown' view.</returns>
        [PreviewLayoutAction]
        [Route(ViewYourFundingConstants.Route_LocalAuthorityHistorySingleYear, Name = ViewYourFundingConstants.RouteName_LocalAuthorityHistorySingleYear)]
        public virtual async Task<IActionResult> LocalAuthorityHistorySingleYear(
            string fundingStreamName,
            int year1,
            int year2,
            string localAuthorityCode,
            string searchTerm = null,
            PreviewLayoutModel previewLayoutModel = null)
        {
            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Organisations_Public);
            var fundingStreamCode = GetFundingStreamCode(fundingStreamName, fundingStreams);

            if (!fundingStreams.TryGetValue(fundingStreamCode, out var fundingStream))
            {
                throw new Exception($"Configuration not found for {fundingStreamCode}");
            }

            var publication = fundingStream.GetLatestPublication(await PreviewModeEnabled());

            if (publication == null)
            {
                throw new Exception($"There are no publications for funding stream {fundingStreamCode}");
            }

            var fundingSearchResult = await _fundingApiService.SearchFunding(new FundingApiSearchRequestObject
            {
                FundingStreams = new[]
                {
                    new FundingApiSearchFundingStream
                    {
                        FundingStreamCode = fundingStreamCode,
                        BeforeDateTime = FundingPeriodHelper.GetCutOffDateForPublication(publication),
                        PeriodCodes = new[] { publication.FundingPeriodCode },
                        Filters = new[]
                        {
                            new SearchFilter
                            {
                                PropertyName = SearchFilterPropertyName.PrimaryIdentifier,
                                PropertyValue = localAuthorityCode
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

            var originalYearType = FundingPeriodHelper.GetYearTypeCodeFromFundingPeriodCode(publication.FundingPeriodCode);
            var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(year1, year2, originalYearType);

            var funding = fundingSearchResult?.Funding?.FirstOrDefault();
            var localAuthorityName = funding?.GroupName;

            var showSelectors = await GetShowSelectorsState();
            var asStatementSpecification = await GetStatementSpecificationState();
            var showData = await GetShowData();

            var viewModel = await GetDownloadViewModel<LocalAuthorityHistorySingleYearViewModel>(year1, year2, fundingStream, false, false);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = HttpUtility.HtmlEncode(searchTerm);
            }

            viewModel.SearchTerm = searchTerm;
            viewModel.LocalAuthorityCode = localAuthorityCode;
            viewModel.LocalAuthorityName = localAuthorityName;
            viewModel.FundingStreamName = fundingStream.FundingStreamName;
            viewModel.FundingStreamCode = fundingStreamCode;
            viewModel.FundingViewData = await _fundingViewService.GenerateFundingViewData(
                _componentService,
                fundingPeriodCode,
                fundingStream.FundingStreamCode,
                new[] { fundingStream },
                publication.CutOffDate ?? publication.PublishedDate,
                publication,
                publication.UIModelVersion,
                FundingViewScope.OrganisationHistorySingleYear,
                GetComponentDefaults(),
                null,
                true,
                false,
                null,
                searchTerm: searchTerm,
                bubbleUpException: false,
                iFundingApiSearchFunding: funding != null ? new[] { funding } : null,
                previewLayoutModel: previewLayoutModel,
                showSelectors: showSelectors,
                asStatementSpecification: asStatementSpecification,
                showData: showData);

            var pageData = viewModel.FundingViewData?.Components?.FirstOrDefault()?.PageData;
            pageData?.Add("Spreadsheets", viewModel.Spreadsheets);

            return View(viewModel);
        }

        /// <summary>
        /// Shared action logic for the local authority 'allocation history' pages.
        /// </summary>
        /// <param name="localAuthorityCode">The local authority code (e.g. 202).</param>
        /// <param name="fundingStreamName">The funding stream name (e.g. Dedicated schools grant).</param>
        /// <param name="searchTerm">The search term (e.g. Camden).</param>
        /// <param name="previewLayoutModel">The Preview layout model.</param>
        /// <returns>A local authority 'allocation history' page view.</returns>
        [PreviewLayoutAction]
        [Route(ViewYourFundingConstants.Route_LocalAuthorityHistory, Name = ViewYourFundingConstants.RouteName_LocalAuthorityHistory)]
        public virtual async Task<IActionResult> LocalAuthorityHistory(
            string localAuthorityCode,
            string fundingStreamName,
            string searchTerm = null,
            PreviewLayoutModel previewLayoutModel = null)
        {
            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Organisations_Public);

            var fundingStreamCode = GetFundingStreamCode(fundingStreamName, fundingStreams);
            var fundingStream = fundingStreams[fundingStreamCode];
            var activeFundingPeriodCodes = FundingPeriodHelper.GetActiveFundingPeriodCodes(fundingStream, await PreviewModeEnabled());
            var latestFundingPeriodCodes = FundingPeriodHelper.GetLatestFundingPeriodCodes_FundingPeriodFormat(fundingStream.SettingValues, activeFundingPeriodCodes);
            var latestPublications = new List<Publication>();

            foreach (var fundingPeriodCode in latestFundingPeriodCodes)
            {
                var latestPublication = await GetLatestPublicationConsideringTerminationDate(localAuthorityCode, fundingPeriodCode, fundingStream);

                latestPublications.Add(latestPublication);
            }

            if (!latestPublications.Any())
            {
                throw new Exception($"There are no publications for funding stream {fundingStreamCode}");
            }

            var fundingApiSearchFundingStreamsForFundings = new List<FundingApiSearchFundingStream>();
            var fundingApiSearchFundingStreamsForProviderFundings = new List<FundingApiSearchFundingStream>();
            var fundingPeriodCodes = fundingStream.GetAllFundingPeriodCodesForPublications(await PreviewModeEnabled());


            foreach (var fundingPeriodCode in fundingPeriodCodes)
            {
                var latestPublication = await GetLatestPublicationConsideringTerminationDate(localAuthorityCode, fundingPeriodCode, fundingStream);

                if (latestFundingPeriodCodes.Contains(fundingPeriodCode))
                {
                    latestPublication.IsLatest = true;
                }
                else
                {
                    latestPublication.IsFinal = true;
                }

                foreach (var loopPublication in fundingStream.Publications)
                {
                    if (loopPublication.FundingPeriodCode == fundingPeriodCode)
                    {
                        if (loopPublication == latestPublication)
                        {
                            continue;
                        }

                        loopPublication.IsLatest = false;
                        loopPublication.IsFinal = false;
                    }
                }

                if (latestPublication == null)
                {
                    throw new Exception($"There are no publications for funding stream {fundingStreamCode}");
                }

                fundingApiSearchFundingStreamsForFundings.Add(new FundingApiSearchFundingStream
                {
                    FundingStreamCode = fundingStreamCode,
                    BeforeDateTime = FundingPeriodHelper.GetCutOffDateForPublication(latestPublication),
                    PeriodCodes = new[] { latestPublication.FundingPeriodCode },
                    Filters = new[]
                        {
                            new SearchFilter
                            {
                                PropertyName = SearchFilterPropertyName.PrimaryIdentifier,
                                PropertyValue = localAuthorityCode
                            },
                            new SearchFilter
                            {
                                PropertyName = SearchFilterPropertyName.GroupingReason,
                                PropertyValue = "Information"
                            }
                        }
                });

                fundingApiSearchFundingStreamsForProviderFundings.Add(new FundingApiSearchFundingStream
                {
                    FundingStreamCode = fundingStreamCode,
                    BeforeDateTime = FundingPeriodHelper.GetCutOffDateForPublication(latestPublication),
                    PeriodCodes = new[] { latestPublication.FundingPeriodCode },
                    Filters = new[]
                        {
                            new SearchFilter
                            {
                                PropertyName = SearchFilterPropertyName.ParentPrimaryIdentifier,
                                PropertyValue = localAuthorityCode
                            },
                            new SearchFilter
                            {
                                PropertyName = SearchFilterPropertyName.GroupingReason,
                                PropertyValue = "Information"
                            }
                        }
                });
            }

            var fundingSearchResult = await _fundingApiService.SearchFunding(new FundingApiSearchRequestObject
            {
                FundingStreams = fundingApiSearchFundingStreamsForFundings.ToArray(),
                BypassGrouping = true
            });

            var providerFundingSearchResult = await _fundingApiService.SearchProviderFunding(new FundingApiSearchRequestObject
            {
                FundingStreams = fundingApiSearchFundingStreamsForProviderFundings.ToArray(),
                BypassGrouping = true
            });

            if (fundingSearchResult?.Funding.Any() != true || providerFundingSearchResult?.ProviderFunding?.Any() != true)
            {
                throw new Exception($"There is no allocation history for this LA {fundingStreamCode}");
            }

            var latestYearFrom = 1;
            var latestYearTo = 1;

            foreach (var latestPublication in latestPublications)
            {
                var (pubYearFrom, pubYearTo) = FundingPeriodHelper.GetYearsFromCode(latestPublication.FundingPeriodCode);
                if (pubYearFrom > latestYearFrom)
                {
                    latestYearFrom = pubYearFrom;
                    latestYearTo = pubYearTo;
                }
            }

            var yearFrom = latestYearFrom;
            var yearTo = latestYearTo;
            var years = FundingPeriodHelper.GetCurrentAndHistoricYears(yearFrom, yearTo, ViewYourFundingConstants.NumberOfYearsOfHistoricAllocationsToShow);
            var fundingPeriodPublications = GroupPublicationsByYear(years, fundingStream, yearFrom);

            foreach (var fundingPeriodPublication in fundingPeriodPublications)
            {
                var publicationsToRemove = new List<Publication>();

                foreach (var publicationToCheck in fundingPeriodPublication.Value)
                {
                    var cutoffDate = FundingPeriodHelper.GetCutOffDateForPublication(publicationToCheck);

                    var cutoffDateAfterPublicationDateFunding =
                        fundingSearchResult.Funding.Where(funding => funding.FundingPeriodCode == publicationToCheck.FundingPeriodCode).Any(x => x.StatusChangedDate <= cutoffDate);
                    var cutoffDateAfterPublicationDateProviderFunding =
                        providerFundingSearchResult.ProviderFunding.Where(providerFunding => providerFunding.FundingPeriodCode == publicationToCheck.FundingPeriodCode).Any(x => x.StatusChangedDate <= cutoffDate);

                    // There is no match with a publication date before cut off date
                    if (!cutoffDateAfterPublicationDateFunding || !cutoffDateAfterPublicationDateProviderFunding)
                    {
                        publicationsToRemove.Add(publicationToCheck);
                    }
                    else if (publicationToCheck.FundingPeriodCode.Equals(_terminatedLocalAuthority.FundingPeriodCode) &&
                       localAuthorityCode.Equals(_terminatedLocalAuthority.LocalAuthorityCode) &&
                       publicationToCheck.PublishedDate > _terminatedLocalAuthority.FinalPublicationDate)
                    {
                        publicationsToRemove.Add(publicationToCheck);
                    }
                }

                // Remove any we need to
                foreach (var publicationToRemove in publicationsToRemove)
                {
                    fundingPeriodPublication.Value.Remove(publicationToRemove);
                }
            }

            // Remove any active (non-historic) allocation groups with no allocation records
            fundingPeriodPublications.RemoveAll(p => p.Value.Count == 0 && !HistoricAllocationsAreExternal(fundingStream, p.Key.yearFrom, yearFrom));

            var viewModel = await GetBasePageViewModel<LocalAuthorityHistoryViewModel>();
            var localAuthorityName = fundingSearchResult.Funding.First().GroupName; // We checked there wasn't zero earlier
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = HttpUtility.HtmlEncode(searchTerm);
            }

            viewModel.SearchTerm = searchTerm;
            viewModel.FundingPeriodPublications = fundingPeriodPublications;
            viewModel.LocalAuthorityCode = localAuthorityCode;
            viewModel.LocalAuthorityName = localAuthorityName;
            viewModel.FundingStreamConfiguration = AsWebModel(fundingStream);
            viewModel.FundingStreamName = fundingStream.FundingStreamName?.ToUIPathComponent();
            viewModel.SecondaryContentTitle = GetFullFundingStreamName(
                fundingStream.FundingStreamCode,
                fundingStream.FundingStreamName,
                fundingStream.FundingStreamCodePubliclyKnown);

            var filters = new[]
            {
                new SearchFilter
                {
                    PropertyName = SearchFilterPropertyName.ParentPrimaryIdentifier,
                    PropertyValue = localAuthorityCode
                }
            };

            var fundingResult = fundingSearchResult?.Funding?.FirstOrDefault();
            var providerFundingResult = providerFundingSearchResult?.ProviderFunding?.FirstOrDefault();

            var showSelectors = await GetShowSelectorsState();
            var asStatementSpecification = await GetStatementSpecificationState();
            var showData = await GetShowData();

            var publication = latestPublications.OrderByDescending(publication => publication.FundingPeriodCode)
                .First();

            viewModel.FundingViewData = await _fundingViewService.GenerateFundingViewData(
                _componentService,
                publication.FundingPeriodCode,
                fundingStream.FundingStreamCode,
                new[] { fundingStream },
                publication.CutOffDate ?? publication.PublishedDate,
                publication,
                publication.UIModelVersion,
                FundingViewScope.OrganisationHistory,
                GetComponentDefaults(),
                null,
                true,
                true,
                filters: filters,
                searchTerm: searchTerm,
                bubbleUpException: false,
                iFundingApiSearchFunding: fundingResult != null ? new[] { fundingResult } : null,
                iFundingApiSearchProviderFunding: providerFundingResult != null ? new[] { providerFundingResult } : null,
                previewLayoutModel: previewLayoutModel,
                showSelectors: showSelectors,
                asStatementSpecification: asStatementSpecification,
                showData: showData);

            var pageData = viewModel.FundingViewData?.Components?.FirstOrDefault()?.PageData;
            pageData?.Add("FundingPeriodPublications", viewModel.FundingPeriodPublications);

            int earliestYear;
            if (_recentlyOpenedLocalAuthorities?.LocalAuthorityCodes.Contains(localAuthorityCode) == true)
            {
                (earliestYear, _) = FundingPeriodHelper.GetYearsFromCode(_recentlyOpenedLocalAuthorities.FundingPeriodCode);

                // Remove any earlier year groups
                fundingPeriodPublications.RemoveAll(p => p.Key.yearFrom < earliestYear);
            }

            return View("LocalAuthorityHistory", viewModel);
        }

        /// <summary>
        /// The MVC action to download an organisation level document.
        /// </summary>
        /// <param name="request">The spreadsheet download request.</param>
        /// <param name="previewLayoutModel">The preview layout model.</param>
        /// <returns>>A spreadsheet for an organisation level document.</returns>
        [PreviewLayoutAction]
        [Route(ViewYourFundingConstants.Route_LocalAuthoritySpreadsheetDownload, Name = ViewYourFundingConstants.RouteName_LocalAuthoritySpreadsheetDownload)]
        public virtual async Task<IActionResult> LocalAuthoritySpreadsheetDownload(OrganisationSpreadsheetDownloadRequest request, PreviewLayoutModel previewLayoutModel = null)
        {
            var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(request.YearFrom, request.YearTo, request.YearTypeCode);
            var contentType = string.IsNullOrWhiteSpace(request.Format) ? FundingDocumentFileType.FileFormats[FundingDocumentFileType.Spreadsheet_OpenFormat] : FundingDocumentFileType.FileFormats[request.Format];

            var publishedDate = request.PublishedDate.ToRouteParameterDate();
            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.Organisations_Public);
            Enum.TryParse(request.Format, true, out FileFormat fileFormat);
            var previewModeEnabled = await PreviewModeEnabled();

            var fundingStream = fundingStreams[request.FundingStreamCode];
            var publication = fundingStream.Publications
                .Where(publication => publication.Status == PublicationStatus.Published
                    || (previewModeEnabled && publication.Status == PublicationStatus.Preview))
                .FirstOrDefault(p => p.PublishedDate == publishedDate);

            if (publication == null)
            {
                throw new ArgumentOutOfRangeException($"There are no publications for the date {request.PublishedDate}");
            }

            var filters = new[]
            {
                new SearchFilter
                {
                    PropertyName = SearchFilterPropertyName.ParentPrimaryIdentifier,
                    PropertyValue = request.LocalAuthorityCode
                }
            };

            var fundingDocument = (await _fundingViewService.GenerateFundingDocument(
                fundingStream,
                fundingPeriodCode,
                publishedDate,
                publication,
                FundingViewType.Spreadsheet,
                FundingViewScope.Organisation,
                new[] { fileFormat },
                filters,
                previewLayoutModel,
                showSelectors: await GetShowSelectorsState(),
                showStatementSpecification: await GetStatementSpecificationState(),
                showData: await GetShowData())).First();

            return File(fundingDocument.Data, contentType, fundingDocument.Filename);
        }

        /// <summary>
        /// The MVC action to download a national level document.
        /// </summary>
        /// <param name="request">The spreadsheet download request.</param>
        /// <param name="previewLayoutModel">The Preview layout model.</param>
        /// <returns>>A spreadsheet for a national level document.</returns>
        [PreviewLayoutAction]
        [Route(ViewYourFundingConstants.Route_PreviewNationalSpreadsheetDownload, Name = ViewYourFundingConstants.RouteName_PreviewNationalSpreadsheetDownload)]
        public virtual async Task<IActionResult> NationalSpreadsheetDownload(NationalSpreadsheetDownloadRequest request, PreviewLayoutModel previewLayoutModel = null)
        {
            var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(request.YearFrom, request.YearTo, request.YearTypeCode);
            var contentType = string.IsNullOrWhiteSpace(request.Format) ? FundingDocumentFileType.FileFormats[FundingDocumentFileType.Spreadsheet_OpenFormat] : FundingDocumentFileType.FileFormats[request.Format];
            var publishedDate = request.PublishedDate.ToRouteParameterDate();

            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.National);
            var previewModeEnabled = await PreviewModeEnabled();
            var fundingStream = fundingStreams[request.FundingStreamCode];
            var publication = fundingStream.Publications
                .Where(publication => publication.Status == PublicationStatus.Published
                    || (previewModeEnabled && publication.Status == PublicationStatus.Preview))
                .FirstOrDefault(p => p.PublishedDate == publishedDate);

            Enum.TryParse(request.Format, true, out FileFormat fileFormat);

            if (publication == null)
            {
                throw new ArgumentOutOfRangeException($"There are no publications for the date {request.PublishedDate}");
            }

            var fundingDocument = (await _fundingViewService.GenerateFundingDocument(
                fundingStream,
                fundingPeriodCode,
                publishedDate,
                publication,
                FundingViewType.Spreadsheet,
                FundingViewScope.National,
                new[] { fileFormat },
                null,
                previewLayoutModel)).First();

            return File(fundingDocument.Data, contentType, fundingDocument.Filename);
        }

        #endregion


        #region Private Methods

        private static void CheckAndRemoveProviderFundingPublications(
            List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>> fundingPeriodPublications,
            IFundingApiSearchResponseProviderFunding providerFundingMatches)
        {
            foreach (var fundingPeriodPublication in fundingPeriodPublications)
            {
                var publicationsToRemove = new List<Publication>();

                foreach (var publicationToCheck in fundingPeriodPublication.Value)
                {
                    var cutoffDate = FundingPeriodHelper.GetCutOffDateForPublication(publicationToCheck);
                    var cutoffDateAfterPublicationDate =
                        providerFundingMatches.ProviderFunding.Any(x => x.StatusChangedDate <= cutoffDate);

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

        private async Task<ProviderFundingBreakdownViewModel> GetProviderFundingBreakdownViewModel(
            int yearFrom,
            int yearTo,
            string searchTerm,
            DateTime publishedDate,
            IFundingApiSearchProviderFunding providerDetails,
            FundingStream fundingStream,
            string fundingPeriodCode)
        {
            var viewModel = await GetBasePageViewModel<ProviderFundingBreakdownViewModel>();
            viewModel.SearchTerm = searchTerm;
            viewModel.PublicationDate = publishedDate;

            var nextPaymentDateTypeCode = GetNextAllocationPaymentDateTypeCode(providerDetails);

            var section = new ProviderStatementSectionViewModel
            {
                ProviderResult = providerDetails,
                SearchTerm = searchTerm,
                FundingStreamConfiguration = AsWebModel(fundingStream)
            };

            if (!section.OrganisationClosed)
            {
                section.NextPaymentDate = PaymentTypeCode.GetNextPaymentDate(
                    fundingStream.NextPayments,
                    nextPaymentDateTypeCode,
                    fundingPeriodCode);

                section.NoNextPaymentForTheYearText = PaymentTypeCode.GetNoNextPaymentForTheYearText(
                    fundingPeriodCode);
            }

            PopulateStreamDataForViewModel(section, fundingPeriodCode, publishedDate);
            await PopulateProviderFundingViewModel(yearFrom, yearTo, section, viewModel, publishedDate, fundingStream);

            return viewModel;
        }

        private async Task<ProviderHistoryViewModel> GetProviderHistoryViewModel(
            string organisationUkprn,
            string searchTerm,
            IFundingApiSearchResponseProviderFunding providerFundingMatches,
            List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>> fundingPeriodPublications,
            FundingStream fundingStream)
        {
            var viewModel = await GetBasePageViewModel<ProviderHistoryViewModel>();
            var organisationName =
                providerFundingMatches.ProviderFunding.First().OrganisationName; // We checked there wasn't zero earlier

            viewModel.SearchTerm = searchTerm;
            viewModel.FundingPeriodPublications = fundingPeriodPublications;
            viewModel.OrganisationUkprn = organisationUkprn;
            viewModel.OrganisationName = organisationName;
            viewModel.SecondaryContentTitle = fundingStream.FundingStreamName;
            viewModel.FundingStreamConfiguration = AsWebModel(fundingStream);

            return viewModel;
        }

        private string GetNextAllocationPaymentDateTypeCode(IFundingApiSearchProviderFunding provider)
        {
            return ProviderTypeInternal.FromExternal(provider.ProviderType, provider.ProviderSubType) switch
            {
                ProviderTypeInternal.Academy => PaymentTypeCode.Academy,
                ProviderTypeInternal.NonMaintainedSpecialSchool => PaymentTypeCode.NonMaintainedSpecialSchool,
                _ => PaymentTypeCode.MaintainedSchool
            };
        }

        private new async Task<T> GetBasePageViewModel<T>(
            Pds.Core.Common.Identity.Models.User currentUser = null,
            bool currentUserPassed = false)
            where T : BaseViewYourFundingPageViewModel, new()
        {
            if (currentUser == null)
            {
                currentUser = await GetUserAsync();
                currentUserPassed = currentUser != null;
            }

            var baseViewModel = await base.GetBasePageViewModel<T>(currentUser, currentUserPassed);
            baseViewModel.FeedbackLink = _feedbackLink;
            baseViewModel.ContactUsLink = _contactUsLink;

            return baseViewModel;
        }

        /// <summary>
        /// Gets the current stream data for view model.
        /// </summary>
        /// <param name="section">The model.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <param name="publishedDate">The published date to use - optional.</param>
        private void PopulateStreamDataForViewModel(
            ProviderStatementSectionViewModel section,
            string fundingPeriodCode,
            DateTime? publishedDate = null)
        {
            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(fundingPeriodCode);
            section.CurrentYearStart = yearFrom;
            section.CurrentYearEnd = yearTo;

            var (asOfMonth, asOfYear) = FundingPublicationDateHelper.GetAcademicAsOfData(
                publishedDate ?? section.FundingStreamConfiguration.Publications.Max(p => p.PublishedDate),
                section.CurrentYearStart,
                section.CurrentYearEnd);

            section.CurrentAsOfMonth = asOfMonth;
            section.CurrentAsOfYear = asOfYear;
        }

        private string GetFullFundingStreamName(string fundingStreamCode, string fundingStreamName, bool canUseShortCode)
        {
            return ViewYourFundingConstants.ContentTitle_Common_NoYears(fundingStreamCode, fundingStreamName, canUseShortCode);
        }

        /// <summary>
        /// Builds the query filter.
        /// </summary>
        /// <param name="viewModelProviderResults">The view model provider results.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>The query filter view model.</returns>
        private QueryFilterViewModel BuildQueryFilter(List<IFundingApiSearchProviderFunding> viewModelProviderResults, string searchTerm)
        {
            return new QueryFilterViewModel
            {
                RouteName = ViewYourFundingConstants.RouteName_ProviderDidYouMean,
                SearchTerm = searchTerm,
                QueryFilter = QueryFilterHelper.BuildQueryFilter(viewModelProviderResults)
            };
        }

        /// <summary>
        /// Get the Download view model.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="fundingStream">The funding stream code (e.g. PSG).</param>
        /// <param name="isProviderFunding">Is it about provider funding.</param>
        /// <param name="isNational">Is it about national funding.</param>
        /// <returns>The Download or child view model. </returns>
        /// <exception cref="Exception">Funding data not yet available for academic year {yearFrom} to {yearTo}.</exception>
        private async Task<T> GetDownloadViewModel<T>(
            int yearFrom,
            int yearTo,
            FundingStream fundingStream,
            bool isProviderFunding,
            bool isNational)
            where T : DownloadViewModel, new()
        {
            var activeFundingPeriodCodes = FundingPeriodHelper.GetActiveFundingPeriodCodes(fundingStream, await PreviewModeEnabled());
            var (latestYearFrom, latestYearTo) = FundingPeriodHelper.GetLatestYears(
                fundingStream.SettingValues, activeFundingPeriodCodes).First();

            if (yearFrom > latestYearFrom || yearTo > latestYearTo)
            {
                throw new Exception($"Funding data not yet available for {yearFrom} to {yearTo}.");
            }

            var viewModel = await GetBasePageViewModel<T>();

            viewModel.YearFrom = yearFrom;
            viewModel.YearTo = yearTo;
            viewModel.LatestYearFrom = latestYearFrom;
            viewModel.LatestYearTo = latestYearTo;

            var fundingYearCode = FundingPeriodHelper.GetYearSettingCode(fundingStream.SettingValues);
            var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(yearFrom, yearTo, fundingYearCode);

            var fundingDocuments = await _fundingDocumentService.GetFundingDocuments(
                fundingStream.FundingStreamCode,
                fundingStream.FundingStreamName,
                fundingPeriodCode,
                fundingStream.Publications,
                fundingStream.GetPreferredDocumentFormat());

            viewModel.Spreadsheets = CreateOutputFilepaths(fundingDocuments);

            if (viewModel.Spreadsheets?.Any() != true)
            {
                throw new Exception($"No spreadsheets have been generated for publications for {fundingStream.FundingStreamCode} {fundingPeriodCode}");
            }

            return viewModel;
        }

        /// <summary>
        /// Populates the provider funding view model.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="section">The section.</param>
        /// <param name="viewModel">The view model.</param>
        /// <param name="publishedDate">The published date.</param>
        /// <param name="fundingStream">The funding stream.</param>
        private async Task PopulateProviderFundingViewModel(
            int yearFrom,
            int yearTo,
            ProviderStatementSectionViewModel section,
            ProviderFundingBreakdownViewModel viewModel,
            DateTime publishedDate,
            FundingStream fundingStream)
        {
            section.Document = GetProviderFundingDocument(section.ProviderResult, publishedDate, fundingStream, false);
            viewModel.ProviderStatementSection = section;

            viewModel.FundingStatus = await GetFundingBreakdownStatus(fundingStream, publishedDate);

            if (!viewModel.FundingStatus.Equals(FundingBreakdownStatus.NotLatest))
            {
                viewModel.NotLatestCssClass = string.Empty;
            }

            viewModel.YearFrom = yearFrom;
            viewModel.YearTo = yearTo;
            viewModel.FundingStreamName = fundingStream.FundingStreamName;
        }

        private async Task<string> GetFundingBreakdownStatus(FundingStream fundingStream, DateTime publishedDate)
        {
            var previewModeEnabled = await PreviewModeEnabled();
            var publication = fundingStream.Publications
                .Where(publication => publication.Status == PublicationStatus.Published
                    || (previewModeEnabled && publication.Status == PublicationStatus.Preview))
                .FirstOrDefault(p => p.PublishedDate == publishedDate);

            if (publication == null)
            {
                throw new ArgumentOutOfRangeException($"There are no publications for the date {publishedDate}");
            }

            var latestOrFinalPublicationDates = fundingStream.Publications
                .GroupBy(p => p.FundingPeriodCode)
                .Select(g => g.Max(gp => gp.PublishedDate));

            if (!latestOrFinalPublicationDates.Contains(publishedDate))
            {
                return FundingBreakdownStatus.NotLatest;
            }

            return publication.FundingPeriodCode == publication.FundingPeriodCode
                ? FundingBreakdownStatus.Latest
                : FundingBreakdownStatus.Final;
        }

        private FundingDocument GetFundingDocumentForOrganisation(
            FundingStream fundingStream,
            string fundingPeriodCode,
            DateTime publishedDate,
            string localAuthorityCode)
        {
            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(fundingPeriodCode);

            var routeToFundingDocument =
                Url?.RouteUrl(ViewYourFundingConstants.RouteName_LocalAuthoritySpreadsheetDownload, new
                {
                    fundingStream.FundingStreamCode,
                    LocalAuthorityCode = localAuthorityCode,
                    YearTypeCode = FundingPeriodHelper.GetYearSettingCode(fundingStream.SettingValues),
                    YearFrom = yearFrom,
                    YearTo = yearTo,
                    Format = FundingDocumentFileType.Spreadsheet_OpenFormat,
                    PublishedDate = publishedDate.ToRouteParameterString()
                });

            return new FundingDocument
            {
                DocumentPublishedDate = publishedDate,
                FileExtension = FundingDocumentFileType.Spreadsheet_OpenFormat,
                FileSizeBytes = GetFileDownloadSizeInBytes(fundingStream, "OrganisationDownloadSizeInBytes"),
                FilePath = routeToFundingDocument,
                FundingStreamCode = fundingStream.FundingStreamCode,
                YearFrom = yearFrom,
                YearTo = yearTo
            };
        }

        private bool IsAfterRecoupment(FundingViewData fundingViewData)
        {
            return fundingViewData?.PublicationUiModelVersion.HasValue == true && fundingViewData.PublicationUiModelVersion > 1;
        }

        private string GetAfterRecoupmentOrEmpty(FundingViewData fundingViewData)
        {
            return IsAfterRecoupment(fundingViewData) ? " after recoupment" : null;
        }

        private string GetInitialIndicativeOrEmpty(FundingViewData fundingViewData)
        {
            if (fundingViewData != null)
            {
                switch (fundingViewData.PublicationUiModelVersion)
                {
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        return "initial";
                    case 9:
                        return string.Empty;
                }
            }

            return "indicative";
        }

        private int GetImportExportAdjustmentYear(FundingViewData fundingViewData, int yearFrom)
        {
            if (fundingViewData != null)
            {
                switch (fundingViewData.PublicationUiModelVersion)
                {
                    case 1:
                    case 2:
                    case 3:
                        return yearFrom - 1;
                }
            }

            return yearFrom;
        }

        /// <summary>
        /// Change file UI filenames for the spreadsheets.
        /// </summary>
        /// <param name="spreadsheets">Spreadsheets to update the names for.</param>
        /// <returns>The updated collection.</returns>
        private IReadOnlyCollection<FundingDocument> CreateOutputFilepaths(IReadOnlyCollection<FundingDocument> spreadsheets)
        {
            if (spreadsheets == null)
            {
                return null;
            }

            foreach (var spreadsheet in spreadsheets)
            {
                var filename = Path.GetFileName(spreadsheet.FilePath);
                var date = spreadsheet.DocumentPublishedDate?.ToString("yyyy-MM-dd");
                spreadsheet.FilePath = Url?.RouteUrl(FundingConstants.RouteName_DownloadSpreadsheet, new { fileName = filename, publishedDate = date }) ?? spreadsheet.FilePath;
            }

            return spreadsheets;
        }

        private async Task<List<FundingStreamCurrentYearViewModel>> GetFundingStreamsLatestYear(Dictionary<string, FundingStream> fundingStreams)
        {
            var lstLatestYear = new List<FundingStreamCurrentYearViewModel>();

            foreach (var fundingStreamCode in fundingStreams.Keys)
            {
                var fundingStreamConfig = fundingStreams[fundingStreamCode];
                var activeFundingPeriodCodes = FundingPeriodHelper.GetActiveFundingPeriodCodes(fundingStreamConfig, await PreviewModeEnabled());

                var latestYears = FundingPeriodHelper.GetLatestYears(
                    fundingStreamConfig.SettingValues, activeFundingPeriodCodes);

                var (yearFrom, yearTo) = latestYears.OrderByDescending(years => years.yearFrom).First();

                lstLatestYear.Add(
                    new FundingStreamCurrentYearViewModel
                    {
                        FundingStreamCode = fundingStreamCode,
                        FundingStreamName = fundingStreamConfig.FundingStreamName,
                        YearStart = yearFrom,
                        YearEnd = yearTo
                    });
            }

            return lstLatestYear;
        }

        /// <summary>
        /// Gets the spreadsheet funding documents with the correct filepaths.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream of the spreadsheets.</param>
        /// <param name="fundingStreamName">The funding stream name of the spreadheets.</param>
        /// <param name="fundingPeriodCode">The funding period code of the spreadsheets.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <returns>IReadOnlyCollection.</returns>
        private async Task<IReadOnlyCollection<FundingDocument>> GetSpreadsheets(string fundingStreamCode, string fundingStreamName, string fundingPeriodCode, int yearFrom, int yearTo)
        {
            IReadOnlyCollection<FundingDocument> spreadsheets = null;

            var fundingDocuments = await _fundingDocumentService.GetFundingDocuments(fundingStreamCode, fundingStreamName, fundingPeriodCode);

            if (fundingDocuments?.Any() != true)
            {
                return spreadsheets;
            }

            spreadsheets = fundingDocuments
                .Where(document =>
                    document.YearFrom == yearFrom &&
                    document.YearTo == yearTo &&
                    document.FundingStreamCode == fundingStreamCode &&
                    document.FileExtension.Equals(FundingDocumentFileType.Spreadsheet_OpenFormat, StringComparison.InvariantCultureIgnoreCase))
                .OrderByDescending(spreadsheet => spreadsheet.DocumentPublishedDate)
                .ToList();

            return CreateOutputFilepaths(spreadsheets);
        }

        /// <summary>
        /// Set the 'Final' or 'Latest' badge on the latest spreadsheet.
        /// </summary>
        /// <param name="fundingDocuments">The list of spreadsheets.</param>
        /// <param name="fundingStream">The funding stream configuration for this funding stream.</param>
        /// <param name="fundingPeriodCode">The funding period code for this funding stream.</param>
        private async Task SetLatestSpreadsheetFinalAndLatestFlags(
            IList<FundingDocument> fundingDocuments,
            FundingStream fundingStream,
            string fundingPeriodCode)
        {
            var latestSpreadsheet = fundingDocuments.FirstOrDefault();
            var activeFundingPeriodCodes = FundingPeriodHelper.GetActiveFundingPeriodCodes(fundingStream, await PreviewModeEnabled());

            var latestYears = FundingPeriodHelper.GetLatestFundingPeriodCodes_FundingPeriodFormat(fundingStream.SettingValues, activeFundingPeriodCodes);

            // Set the speadsheet publication badge for 'Latest' or 'Final'
            var isACurrentYear = latestYears.Contains(fundingPeriodCode);

            if (isACurrentYear)
            {
                latestSpreadsheet.IsLatestDocument = true;
                latestSpreadsheet.IsFinal = false;
            }
            else
            {
                latestSpreadsheet.IsLatestDocument = false;
                latestSpreadsheet.IsFinal = true;
            }
        }

        /// <summary>
        /// Gets the funding steam related data.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code for the funding stream data.</param>
        /// <param name="userDetails">The user details.</param>
        /// <returns>A funding stream.</returns>
        private async Task<FundingStream> GetNationalFundingStream(
            string fundingStreamCode,
            Pds.Core.Common.Identity.Models.User userDetails)
        {
            var fundingStreams = await GetRelevantFundingStreams(Enums.FundingUIViewType.National, userDetails, true);

            return fundingStreams?.Any() == true ? fundingStreams[fundingStreamCode] : null;
        }

        /// <summary>
        /// Gets the funding period code for this funding stream.
        /// </summary>
        /// <param name="fundingStream">The funding stream configuration for this funding stream.</param>
        /// <param name="yearFrom">The start year for this funding stream period.</param>
        /// <param name="yearTo">The end year for this funding stream period.</param>
        /// <returns>string.</returns>
        private string GetFundingPeriodCode(FundingStream fundingStream, int yearFrom, int yearTo)
        {
            var yearSettingName = FundingPeriodHelper.GetYearSettingName(fundingStream.SettingValues);
            var yearTypeCode = FundingPeriodHelper.GetYearTypeCodeFromName(yearSettingName);
            var fundingPeriodCode = FundingPeriodHelper.GetCodeFromYears(yearFrom, yearTo, yearTypeCode);

            return fundingPeriodCode;
        }

        private IActionResult RedirectToHome
            => Redirect("/");

        private IActionResult LogoutMyesfAndRedirectToMyesfStartpage()
        {
            return Redirect(_applicationConfiguration.MyesfLogoutUrl);
        }


        private async Task<Publication> GetLatestPublicationConsideringTerminationDate(
            string localAuthorityCode,
            string fundingPeriodCode,
            FundingStream fundingStream)
        {
            var isTerminatedLocalAuthority =
                _terminatedLocalAuthority.LocalAuthorityCode.Equals(localAuthorityCode) &&
                _terminatedLocalAuthority.FundingPeriodCode.Equals(fundingPeriodCode);

            var latestPublication = isTerminatedLocalAuthority
                ? fundingStream.GetLatestPublication(
                    await PreviewModeEnabled(),
                    fundingPeriodCode: fundingPeriodCode,
                    finalPublicationDate: _terminatedLocalAuthority.FinalPublicationDate)
                : fundingStream.GetLatestPublication(
                    await PreviewModeEnabled(),
                    fundingPeriodCode: fundingPeriodCode);

            return latestPublication;
        }

        #endregion
    }
}