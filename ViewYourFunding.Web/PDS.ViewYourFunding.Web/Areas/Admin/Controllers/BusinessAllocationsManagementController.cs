using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.BusinessAllocationsManagement;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Exceptions;
using PDS.ViewYourFunding.Web.Helpers;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PublicationStatus = PDS.ViewYourFunding.Services.Enums.PublicationStatus;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The UI controller for managing the allocation data.
    /// </summary>
    /// <seealso cref="BaseController" />
    [Authorize(Policy = PolicyConstants.AllocationsAdministrator)]
    [Area("Admin")]
    public class BusinessAllocationsManagementController : AdminActionsBaseController
    {
        /// <summary>
        /// The http feed reader endpoint.
        /// </summary>
        private readonly string _feedReaderUrl;

        /// <summary>
        /// The http pdf comparison endpoint.
        /// </summary>
        private readonly string _documentGeneratorPdfComparerUrl;

        /// <summary>
        /// The admin settings service.
        /// </summary>
        private readonly IAdminSettingsService _adminSettingsService;

        /// <summary>
        /// The audit service.
        /// </summary>
        private readonly IAuditService _auditService;

        private readonly string _contactUsLink;

        private readonly string _serviceNowLink;

        private readonly IProviderFundingService _providerFundingService;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessAllocationsManagementController"/> class.
        /// </summary>
        /// <param name="securityService">The security service.</param>
        /// <param name="applicationConfigurationOptions">The application configuration options.</param>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="adminSettingsService">The admin settings service.</param>
        /// <param name="backgroundTaskQueue">The background task queue.</param>
        /// <param name="auditService">The audit service.</param>
        /// <param name="providerFundingService">The provider funding service.</param>
        /// <param name="logger">The logger.</param>
        public BusinessAllocationsManagementController(
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            IHttpClientFactory httpClientFactory,
            IAdminSettingsService adminSettingsService,
            IBackgroundTaskQueue backgroundTaskQueue,
            IAuditService auditService,
            IProviderFundingService providerFundingService,
            ILoggerAdapter<AdminActionsBaseController> logger)
            : base(securityService, applicationConfigurationOptions, httpClientFactory, adminSettingsService, backgroundTaskQueue, logger)
        {
            _feedReaderUrl = applicationConfigurationOptions.Value.FeedReaderUrl;
            _documentGeneratorPdfComparerUrl = applicationConfigurationOptions.Value.DocumentGeneratorPdfComparerUrl;
            _adminSettingsService = adminSettingsService;
            _auditService = auditService;
            _providerFundingService = providerFundingService;
            _contactUsLink = applicationConfigurationOptions.Value.ContactUsLink;
            _serviceNowLink = applicationConfigurationOptions.Value.ServiceNowLink;
        }

        #endregion


        #region Actions

        /// <summary>
        /// The start action for view your funding admin settings.
        /// </summary>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsAdminHome, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsAdminHome)]
        public virtual async Task<IActionResult> Index()
        {
            var viewModel = await GetBasePageViewModel<BusinessAllocationsManagementHomeViewModel>();
            return View(viewModel);
        }

        /// <summary>
        /// Run feed reader action.
        /// </summary>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsAdminRunFeedReader, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsAdminRunFeedReader)]
        public virtual async Task<IActionResult> RunFeedReader()
        {
            var viewModel = await GetBasePageViewModel<RunFeedReaderViewModel>();

            viewModel.FundingStreamCodesSource = await GetFundingStreamSelectItemList();

            return View(viewModel);
        }

        /// <summary>
        /// Last feed reader run status.
        /// </summary>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsFeedReaderLastRun, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsFeedReaderLastRun)]
        public virtual async Task<IActionResult> FeedReaderLastRun()
        {
            var availableFundingStreams = (await GetFundingStreamSelectItemList())
                .Select(fundingStream => fundingStream.Value);

            var viewModel = await GetBasePageViewModel<FeedReaderLastRunViewModel>();

            foreach (var fundingStreamCode in availableFundingStreams)
            {
                await GetAudits(viewModel, fundingStreamCode);
            }

            viewModel.ShowAllFundingStreams = true;
            viewModel.FundingStreamCodes = availableFundingStreams;
            return View(viewModel);
        }

        /// <summary>
        /// Last feed reader run status.
        /// </summary>
        /// /// <param name="fundingStreamCodes">The Funding Stream Codes.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsFeedReaderLastRunConfirm, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsFeedReaderLastRunConfirm)]
        public virtual async Task<IActionResult> FeedReaderLastRun(string fundingStreamCodes)
        {
            var viewModel = await GetBasePageViewModel<FeedReaderLastRunViewModel>();

            foreach (var fundingStreamCode in fundingStreamCodes.Split(',').ToList())
            {
                await GetAudits(viewModel, fundingStreamCode);
            }

            viewModel.SelectedFundingStreamCodes = fundingStreamCodes;
            viewModel.FundingStreamCodes = fundingStreamCodes.Split(',').ToList();
            return View(viewModel);
        }

        /// <summary>
        /// Last feed reader run status.
        /// </summary>
        /// <param name="runFeedReaderViewModel">The run view model.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpPost]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsFeedReaderLastRun, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsFeedReaderLastRun)]
        public virtual async Task<IActionResult> FeedReaderLastRun(RunFeedReaderViewModel runFeedReaderViewModel)
        {
            if (runFeedReaderViewModel.FundingStreamCodes == null)
            {
                runFeedReaderViewModel = await GetBasePageViewModel<RunFeedReaderViewModel>();
                ModelState.AddModelError(nameof(runFeedReaderViewModel.FundingStreamCodes), "Please select at least one funding stream");
                runFeedReaderViewModel.FundingStreamCodesSource = await GetFundingStreamSelectItemList();
                return View("RunFeedReader", runFeedReaderViewModel);
            }

            var viewModel = await GetBasePageViewModel<FeedReaderLastRunViewModel>();

            foreach (var fundingStreamCode in runFeedReaderViewModel.FundingStreamCodes)
            {
                await GetAudits(viewModel, fundingStreamCode);
            }

            viewModel.ShowAllFundingStreams = false;
            viewModel.FundingStreamCodes = runFeedReaderViewModel.FundingStreamCodes;

            return View(viewModel);
        }

        /// <summary>
        /// Calls the feed reader.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The MVC view result.</returns>
        [HttpPost]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsSubmitRequest, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsSubmitRequest)]
        public virtual IActionResult SubmitRequest(FeedReaderLastRunViewModel viewModel)
        {
            if (string.IsNullOrEmpty(_feedReaderUrl))
            {
                throw new RequestException("Feed reader url not provided");
            }

            var requestUrl = _feedReaderUrl;
            requestUrl = $"{requestUrl}&{nameof(viewModel.FundingStreamCodes)}={string.Join(",", viewModel.FundingStreamCodes)}&useBookmark=true";

            var success = QueueBackGroundTask(requestUrl);

            return RedirectToRoute(
                ViewYourFundingConstants.RouteName_BusinessAllocationsActionsConfirmation,
                new
                {
                    success,
                    BusinessAllocationsAction = BusinessAllocationsAction.RunFeedReader,
                    FundingStreamCodeAndPeriodCode = string.Join(',', viewModel.FundingStreamCodes)
                });
        }

        /// <summary>
        /// Run feed reader action.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpPost]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsConfirmRunFeedReader, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsConfirmRunFeedReader)]
        public virtual IActionResult ConfirmRunFeedReader(FeedReaderLastRunViewModel viewModel)
        {
            if (string.IsNullOrEmpty(_feedReaderUrl))
            {
                throw new RequestException("Feed reader url not provided");
            }

            var requestUrl = _feedReaderUrl;
            requestUrl = $"{requestUrl}&{nameof(viewModel.FundingStreamCodes)}={viewModel.SelectedFundingStreamCodes}&useBookmark=true";

            var success = QueueBackGroundTask(requestUrl);

            return RedirectToRoute(
                ViewYourFundingConstants.RouteName_BusinessAllocationsActionsConfirmation,
                new
                {
                    success,
                    BusinessAllocationsAction = BusinessAllocationsAction.RunFeedReader
                });
        }

        /// <summary>
        /// Confirmation of the business allocations operation.
        /// </summary>
        /// <param name="success">if set to true the action has ran successfully.</param>
        /// <param name="businessAllocationsAction">The business allocations action.</param>
        /// <param name="fundingStreamCodeAndPeriodCode">Funding Stream Code And Period Code.</param>
        /// <param name="sourceFolder">The source folder.</param>
        /// <param name="targetFolder">The target folder.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsActionsConfirmation, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsActionsConfirmation)]
        public virtual async Task<IActionResult> Confirmation(bool success, BusinessAllocationsAction businessAllocationsAction, string fundingStreamCodeAndPeriodCode, string sourceFolder, string targetFolder)
        {
            var viewModel = await GetBasePageViewModel<BusinessAllocationsConfirmationViewModel>();
            var availableFundingStreams = await GetFundingStreamSelectItemList();
            viewModel.SourceFolder = sourceFolder;
            viewModel.TargetFolder = targetFolder;
            viewModel.Success = success;
            viewModel.BusinessAllocationsAction = businessAllocationsAction;
            viewModel.SelectedFundingStreamCodes = fundingStreamCodeAndPeriodCode;
            viewModel.FundingStreamCodeAndPeriodCode = businessAllocationsAction == BusinessAllocationsAction.RunFeedReader ? string.Join(", ", availableFundingStreams.Where(fundingStream => fundingStreamCodeAndPeriodCode.Contains(fundingStream.Value)).Select(x => x.Text).ToList()) : fundingStreamCodeAndPeriodCode;
            viewModel.ContactUsLink = viewModel.CurrentUser.IsExternalUser
                ? _contactUsLink
                : _serviceNowLink;
            return View(viewModel);
        }


        /// <summary>
        /// Run Pdf Comparison action.
        /// </summary>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsRunPdfComparison, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsRunPdfComparison)]
        public virtual async Task<IActionResult> RunPdfComparison()
        {
            var viewModel = await GetBasePageViewModel<RunPdfComparisonViewModel>();
            viewModel.FundingStreamCodesAndPeriodCodes = await GetFundingStreamCodesAndPeriodCodes();
            return View(viewModel);
        }

        /// <summary>
        /// Run Pdf Comparison action.
        /// </summary>
        /// <param name="fundingStreamCodeAndPeriodCode">The Funding Stream Code And PeriodCode.</param>
        /// <param name="sourceFolder">The Source Folder.</param>
        /// <param name="targetFolder">The Target Folder.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsRunPdfComparisonConfirm, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsRunPdfComparisonConfirm)]
        public virtual async Task<IActionResult> RunPdfComparison(string fundingStreamCodeAndPeriodCode, string sourceFolder, string targetFolder)
        {
            if (string.IsNullOrWhiteSpace(fundingStreamCodeAndPeriodCode) || string.IsNullOrWhiteSpace(sourceFolder) || string.IsNullOrWhiteSpace(targetFolder))
            {
                return RedirectToRoute(ViewYourFundingConstants.RouteName_BusinessAllocationsRunPdfComparison);
            }

            var viewModel = await GetBasePageViewModel<RunPdfConfirmComparisonViewModel>();
            viewModel.FundingStreamCodeAndPeriodCode = fundingStreamCodeAndPeriodCode;
            viewModel.FundingStreamCodesAndPeriodCodes = await GetFundingStreamCodesAndPeriodCodes();
            viewModel.SourceFolder = sourceFolder;
            viewModel.TargetFolder = targetFolder;

            return View("AreYouSure", viewModel);
        }

        /// <summary>
        /// Runs the PDF comparison.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <exception cref="RequestException">PDF Comparison url not provided.</exception>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpPost]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsRunPdfComparison, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsRunPdfComparison)]
        public virtual async Task<IActionResult> RunPdfComparison(RunPdfComparisonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return View("AreYouSure", await GetConfirmComparisonViewModel(viewModel));
            }

            viewModel.FundingStreamCodesAndPeriodCodes = await GetFundingStreamCodesAndPeriodCodes();
            return View(viewModel);
        }

        /// <summary>
        /// Confirms the run PDF comparison.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The confirmation page.</returns>
        /// <exception cref="RequestException">PDF Comparison url not provided.</exception>
        [HttpPost]
        [Route(
            ViewYourFundingConstants.Route_BusinessAllocationsConfirmRunPdfComparison,
            Name = ViewYourFundingConstants.RouteName_BusinessAllocationsConfirmRunPdfComparison)]
        public virtual IActionResult ConfirmRunPdfComparison(RunPdfComparisonViewModel viewModel)
        {
            if (string.IsNullOrEmpty(_documentGeneratorPdfComparerUrl))
            {
                throw new RequestException("Document Generator PDF Comparer url not provided");
            }

            var fundingStreamAndPeriodCodeParts = viewModel.FundingStreamCodeAndPeriodCode.Split(':');

            var parameters = $"fundingStreamCode={fundingStreamAndPeriodCodeParts[0]}" +
                $"&fundingPeriodCode={fundingStreamAndPeriodCodeParts[1]}" +
                $"&folderSource={viewModel.SourceFolder}" +
                $"&folderDestination={viewModel.TargetFolder}";

            var requestUrl = UriHelper.BuildUri(_documentGeneratorPdfComparerUrl, parameters);
            var success = QueueBackGroundTask(requestUrl);

            return RedirectToRoute(
                ViewYourFundingConstants.RouteName_BusinessAllocationsActionsConfirmation,
                new
                {
                    success,
                    BusinessAllocationsAction = BusinessAllocationsAction.PdfComparison,
                    FundingStreamCodeAndPeriodCode = viewModel.FundingStreamCodeAndPeriodCode,
                    SourceFolder = viewModel.SourceFolder,
                    TargetFolder = viewModel.TargetFolder
                });
        }

        /// <summary>
        /// Select Funding Stream action.
        /// </summary>
        /// <param name="error">Whether or not to display the validation error messages.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsSelectFundingStream, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsSelectFundingStream)]
        public virtual async Task<IActionResult> SelectFundingStream(bool error = false)
        {
            var viewModel = await GetBasePageViewModel<SelectFundingStreamViewModel>();
            viewModel.FundingStreams = await GetFundingStreamSelectList();
            viewModel.ValidationError = error;

            if (viewModel.FundingStreams.Count() == 1)
            {
                return RedirectToRoute(
                ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderAndYear,
                new
                {
                    fundingStreamCode = viewModel.FundingStreams.FirstOrDefault().Value
                });
            }

            return View(viewModel);
        }

        /// <summary>
        /// Select Funding Stream action.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpPost]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsSelectFundingStream, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsSelectFundingStream)]
        public virtual async Task<IActionResult> SelectFundingStream(SelectFundingStreamViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.SelectedFundingStreamCode))
            {
                viewModel.ValidationError = true;
                return RedirectToRoute(ViewYourFundingConstants.RouteName_BusinessAllocationsSelectFundingStream, new { error = true });
            }

            viewModel.FundingStreamCodesSource = await GetFundingStreamCodesAndPeriodCodes();

            return RedirectToRoute(
                ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderAndYear,
                new
                {
                    fundingStreamCode = viewModel.SelectedFundingStreamCode
                });
        }

        /// <summary>
        /// Search By Provider And Year action.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code.</param>
        /// <param name="error">Whether or not to display the validation error messages.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsSearchByProviderAndYear, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderAndYear)]
        public virtual async Task<IActionResult> SearchByProviderAndYear(string fundingStreamCode, bool error = false)
        {
            var fundingPeriodCodes = await GetFundingPeriodCodes(fundingStreamCode);

            var viewModel = await GetBasePageViewModel<SearchByProviderAndYearViewModel>();
            viewModel.FundingStreamCode = fundingStreamCode;
            viewModel.FundingStreamPeriodCodes = fundingPeriodCodes;
            viewModel.ValidationError = error;

            return View(viewModel);
        }

        /// <summary>
        /// Search By Provider And Year action.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpPost]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsSearchByProviderAndYear, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderAndYear)]
        public virtual async Task<IActionResult> SearchByProviderAndYear(SearchByProviderAndYearViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(viewModel.SelectedFundingPeriodCode))
                {
                    viewModel.ValidationError = true;
                    return RedirectToRoute(ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderAndYear, new { fundingStreamCode = viewModel.FundingStreamCode, error = true });
                }

                viewModel.FundingStreamPeriodCodes = await GetFundingStreamCodesAndPeriodCodes();

                return RedirectToRoute(
                    ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderAndYearDetails,
                    new
                    {
                        fundingStreamCode = viewModel.FundingStreamCode,
                        ukprn = viewModel.Ukprn,
                        fundingPeriodCode = viewModel.SelectedFundingPeriodCode
                    });
            }

            viewModel.FundingStreamPeriodCodes = await GetFundingPeriodCodes(viewModel.FundingStreamCode);

            return View(viewModel);
        }

        /// <summary>
        /// Search By Provider And Year Details action.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code.</param>
        /// <param name="ukprn">The ukprn.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsSearchByProviderAndYearDetails, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderAndYearDetails)]
        public virtual async Task<IActionResult> SearchByProviderAndYearDetails(string fundingStreamCode, string ukprn, string fundingPeriodCode)
        {
            var providerFundings = await _providerFundingService.GetProviderFundings(fundingStreamCode, ukprn, fundingPeriodCode);

            var viewModel = await GetBasePageViewModel<SearchByProviderAndYearDetailsViewModel>();
            viewModel.FundingStreamCode = fundingStreamCode;
            viewModel.FundingStreamBusinessAllocationName = await GetFundingStreamBusinessAllocationName(fundingStreamCode);
            viewModel.ProviderFundings = providerFundings;
            viewModel.ProviderFundingName = providerFundings?.FirstOrDefault()?.Provider?.Name;
            viewModel.Ukprn = ukprn;
            viewModel.SelectedFundingPeriodCode = fundingPeriodCode;
            viewModel.FormattedFundingPeriodCode = FundingPeriodHelper.GetYearAndTypeNameFromCode(fundingPeriodCode);

            return View(viewModel);
        }

        /// <summary>
        /// Search By Provider Data Result action.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_BusinessAllocationsSearchByProviderDataResult, Name = ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderDataResult)]
        public virtual async Task<IActionResult> SearchByProviderDataResult(string id)
        {
            var providerFunding = await _providerFundingService.GetProviderFundingsById(id);

            var providerFundingAll = await _providerFundingService.GetProviderFundingFromAllById(id);

            var viewModel = await GetBasePageViewModel<SearchByProviderDataResultViewModel>();
            viewModel.ProviderFunding = providerFunding?.FirstOrDefault();
            viewModel.DataValue = providerFundingAll;
            viewModel.FormattedDataValue = JValue.Parse(providerFundingAll.Substring(1, providerFundingAll.Length - 2)).ToString(Formatting.Indented);
            viewModel.FundingStreamBusinessAllocationName = await GetFundingStreamBusinessAllocationName(viewModel.ProviderFunding.FundingStreamCode);
            viewModel.FormattedFundingPeriodCode = FundingPeriodHelper.GetYearAndTypeNameFromCode(viewModel.ProviderFunding.FundingPeriodId);

            return View(viewModel);
        }

        private async Task<IEnumerable<SelectListItem>> GetFundingStreamCodesAndPeriodCodes()
        {
            var fundingStreams = await GetAllFundingStreams();
            var roles = (await GetUserAsync()).Roles;
            var isAdmin = roles.Any(role => nameof(UserRole.SfsAdmin).Equals(role, StringComparison.OrdinalIgnoreCase));

            var result = new List<SelectListItem>();

            foreach (var fundingStream in fundingStreams)
            {
                result.AddRange(fundingStream.Publications
                    .Where(publication => publication.Status == PublicationStatus.Published && (isAdmin || roles.Any(role => role.Contains(fundingStream.FundingStreamCode))))
                    .Select(publication => publication.FundingPeriodCode)
                    .Distinct()
                    .Select(fundingPeriodCode =>
                        new SelectListItem(
                            $"{fundingStream.FundingStreamCode}:{fundingPeriodCode}",
                            $"{fundingStream.FundingStreamCode}:{fundingPeriodCode}")));
            }

            return result;
        }

        private async Task GetAudits(FeedReaderLastRunViewModel viewModel, string fundingStreamCode)
        {
            var queryResult = await _auditService.GetDataImportAudit(fundingStreamCode);

            if (queryResult.Any())
            {
                var fundingStream = await _adminSettingsService.GetFundingStream(fundingStreamCode);
                var audit = queryResult.First();
                var (startDateTime, endDateTime) = FormatDateTimes(audit.StartDateTime, audit.EndDateTime);
                audit.StartDateTime = startDateTime;
                audit.EndDateTime = endDateTime;

                viewModel.Audit.Add(fundingStream.FundingStreamBusinessAllocationName, audit);
            }
        }

        private async Task<RunPdfConfirmComparisonViewModel> GetConfirmComparisonViewModel(RunPdfComparisonViewModel model)
        {
            var viewModel = await GetBasePageViewModel<RunPdfConfirmComparisonViewModel>();
            viewModel.FundingStreamCodeAndPeriodCode = model.FundingStreamCodeAndPeriodCode;
            viewModel.FundingStreamCodesAndPeriodCodes = model.FundingStreamCodesAndPeriodCodes;
            viewModel.SourceFolder = model.SourceFolder;
            viewModel.TargetFolder = model.TargetFolder;

            return viewModel;
        }

        private async Task<IEnumerable<SelectListItem>> GetFundingStreamSelectList()
        {
            var fundingStreams = await _adminSettingsService.GetAllFundingStreams();

            var roles = (await GetUserAsync()).Roles;
            var isAdmin = roles.Any(role => nameof(UserRole.SfsAdmin).Equals(role, StringComparison.OrdinalIgnoreCase));

            return fundingStreams.Where(f => (isAdmin || roles.Any(role => role.Contains(f.FundingStreamCode)))).Select(fundingStream => new SelectListItem
            {
                Text = fundingStream.FundingStreamBusinessAllocationName,
                Value = fundingStream.FundingStreamCode
            });
        }

        private async Task<IEnumerable<SelectListItem>> GetFundingPeriodCodes(string fundingStreamCode)
        {
            var publications = await _adminSettingsService.GetPublications(fundingStreamCode);

            return publications.Select(publication => publication.FundingPeriodCode)
                .Distinct()
                .Select(fundingPeriodCode => new SelectListItem($"{FundingPeriodHelper.GetYearAndTypeNameFromCode(fundingPeriodCode)} ({fundingPeriodCode})", fundingPeriodCode));
        }

        private async Task<string> GetFundingStreamBusinessAllocationName(string fundingStreamCode)
        {
            var fundingStream = await _adminSettingsService.GetFundingStream(fundingStreamCode);
            return fundingStream.FundingStreamBusinessAllocationName;
        }

        #endregion

    }
}