using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.PdfGenerationActions;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Exceptions;
using PDS.ViewYourFunding.Web.Helpers;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PublicationStatus = PDS.ViewYourFunding.Services.Enums.PublicationStatus;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The UI controller for managing the view your funding settings admin pages.
    /// </summary>
    /// <seealso cref="BaseController" />
    [Authorize(Policy = nameof(UserRole.SfsAdmin))]
    [Area("Admin")]
    public class PdfGenerationActionsController : AdminActionsBaseController
    {
        #region Private Fields

        /// <summary>
        /// The http feed reader endpoint.
        /// </summary>
        private readonly string _feedReaderUrl;

        /// <summary>
        /// The http pdf comparison endpoint.
        /// </summary>
        private readonly string _documentGeneratorPdfComparerUrl;

        /// <summary>
        /// The http funding pdf generation endpoint.
        /// </summary>
        private readonly string _documentGeneratorUrl;

        /// <summary>
        /// The generate funding report endpoint.
        /// </summary>
        private readonly string _documentGeneratorFundingReportsUrl;

        /// <summary>
        /// The http re-run pdf generation endpoint.
        /// </summary>
        private readonly string _documentGeneratorRerunUrl;

        /// <summary>
        /// The audit service.
        /// </summary>
        private readonly IAuditService _auditService;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfGenerationActionsController"/> class.
        /// </summary>
        /// <param name="securityService">The security service.</param>
        /// <param name="applicationConfigurationOptions">The application configuration options.</param>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="adminSettingsService">The admin settings service.</param>
        /// <param name="backgroundTaskQueue">The background task queue.</param>
        /// <param name="auditService">The audit service.</param>
        /// <param name="logger">The logger.</param>
        public PdfGenerationActionsController(
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            IHttpClientFactory httpClientFactory,
            IAdminSettingsService adminSettingsService,
            IBackgroundTaskQueue backgroundTaskQueue,
            IAuditService auditService,
            ILoggerAdapter<AdminActionsBaseController> logger)
            : base(securityService, applicationConfigurationOptions, httpClientFactory, adminSettingsService, backgroundTaskQueue, logger)
        {
            _feedReaderUrl = applicationConfigurationOptions.Value.FeedReaderUrl;
            _documentGeneratorPdfComparerUrl = applicationConfigurationOptions.Value.DocumentGeneratorPdfComparerUrl;
            _documentGeneratorUrl = applicationConfigurationOptions.Value.DocumentGeneratorUrl;
            _documentGeneratorFundingReportsUrl = applicationConfigurationOptions.Value.DocumentGeneratorFundingReportsUrl;
            _documentGeneratorRerunUrl = applicationConfigurationOptions.Value.DocumentGeneratorRerunUrl;
            _auditService = auditService;
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
        [Route(ViewYourFundingConstants.Route_PdfGenerationActionsHome, Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsHome)]
        public virtual async Task<IActionResult> Index()
        {
            var viewModel = await GetBasePageViewModel<PdfGenerationActionsListViewModel>();
            return View(viewModel);
        }

        /// <summary>
        /// Run Pdf Comparison action.
        /// </summary>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_PdfGenerationActionsRunPdfComparison, Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsRunPdfComparison)]
        public virtual async Task<IActionResult> RunPdfComparison()
        {
            var viewModel = await GetBasePageViewModel<RunPdfComparisonViewModel>();
            viewModel.FundingStreamCodesAndPeriodCodes = await GetFundingStreamCodesAndPeriodCodes();
            return View(viewModel);
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
        [Route(ViewYourFundingConstants.Route_PdfGenerationActionsRunPdfComparison, Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsRunPdfComparison)]
        public virtual async Task<IActionResult> RunPdfComparison(RunPdfComparisonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return View("AreYouSure", viewModel);
            }

            viewModel.FundingStreamCodesAndPeriodCodes = await GetFundingStreamCodesAndPeriodCodes();
            return View(viewModel);
        }

        /// <summary>
        /// Generate funding reports action.
        /// </summary>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(
            ViewYourFundingConstants.Route_PdfGenerationActionsGenerateFundingReports,
            Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsGenerateFundingReports)]
        public virtual async Task<IActionResult> GenerateFundingReports()
        {
            var viewModel = await GetBasePageViewModel<GenerateFundingReportsViewModel>();
            await AddFundingSelectLists(viewModel);
            return View(viewModel);
        }

        /// <summary>
        /// Generate Funding Reports action.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>Redirection to confirmation.</returns>
        [HttpPost]
        [Route(
            ViewYourFundingConstants.Route_PdfGenerationActionsGenerateFundingReports,
            Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsGenerateFundingReports)]
        public virtual IActionResult GenerateFundingReports(
            GenerateFundingReportsViewModel viewModel)
        {
            return View("AreYouSure", viewModel);
        }

        /// <summary>
        /// Generates the funding report.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>Redirection to confirmation page.</returns>
        /// <exception cref="RequestException">Generate funding report url not provided.</exception>
        [HttpPost]
        [Route(
            ViewYourFundingConstants.Route_PdfGenerationActionsGenerateFundingReport,
            Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsGenerateFundingReport)]
        public virtual IActionResult GenerateFundingReport(GenerateFundingReportsViewModel viewModel)
        {
            if (string.IsNullOrEmpty(_documentGeneratorFundingReportsUrl))
            {
                throw new RequestException("Document Generator Funding Reports url not provided");
            }

            var reportParameters = viewModel.ReportType switch
            {
                LocalAuthorityReports.StudentNumbersValue =>
                $"{LocalAuthorityReports.GroupTypeCodeParameter}={GroupingType.LocalAuthority}&{LocalAuthorityReports.GroupTypeReasonParameter}={GroupingReason.Information}&{LocalAuthorityReports.FundingPeriodIdParameter}={viewModel.FundingPeriodCode}",
                LocalAuthorityReports.SixthFormValue =>
                $"{LocalAuthorityReports.GroupTypeCodeParameter}={GroupingType.LocalAuthoritySsf}&{LocalAuthorityReports.GroupTypeReasonParameter}={GroupingReason.Contracting}&{LocalAuthorityReports.FundingPeriodIdParameter}={viewModel.FundingPeriodCode}",
                LocalAuthorityReports.SixthFormMssValue =>
                $"{LocalAuthorityReports.GroupTypeCodeParameter}={GroupingType.LocalAuthority}&{LocalAuthorityReports.ExcludedGroupTypeCodeParameter}={GroupingType.LocalAuthoritySsf}&{LocalAuthorityReports.GroupTypeReasonParameter}={GroupingReason.Contracting}&{LocalAuthorityReports.FundingPeriodIdParameter}={viewModel.FundingPeriodCode}",
                LocalAuthorityReports.LARECValue =>
                $"{LocalAuthorityReports.GroupTypeCodeParameter}={GroupingType.LocalAuthority}&{LocalAuthorityReports.GroupTypeReasonParameter}={GroupingReason.Information}&{LocalAuthorityReports.FundingPeriodIdParameter}={viewModel.FundingPeriodCode}",
                _ => string.Empty
            };

            var requestUrl = $"{_documentGeneratorFundingReportsUrl}&{reportParameters}";

            var success = QueueBackGroundTask(requestUrl);

            return RedirectToRoute(
                ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmation,
                new
                {
                    success,
                    PdfGenerationAction = PdfGenerationAction.PdfGenerateFundingReport
                });
        }

        /// <summary>
        /// Confirms the run PDF comparison.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The confirmation page.</returns>
        /// <exception cref="RequestException">PDF Comparison url not provided.</exception>
        [HttpPost]
        [Route(
            ViewYourFundingConstants.Route_PdfGenerationActionsConfirmRunPdfComparison,
            Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmRunPdfComparison)]
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
                $"&folderDestination={viewModel.DestinationFolder}";

            var requestUrl = UriHelper.BuildUri(_documentGeneratorPdfComparerUrl, parameters);
            var success = QueueBackGroundTask(requestUrl);

            return RedirectToRoute(
                ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmation,
                new
                {
                    success,
                    PdfGenerationAction = PdfGenerationAction.PdfComparison
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
        [Route(ViewYourFundingConstants.Route_PdfGenerationActionsConfirmRunFeedReader, Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmRunFeedReader)]
        public virtual IActionResult ConfirmRunFeedReader(RunFeedReaderViewModel viewModel)
        {
            if (string.IsNullOrEmpty(_feedReaderUrl))
            {
                throw new RequestException("Feed reader url not provided");
            }

            var requestUrl = _feedReaderUrl;
            requestUrl = $"{requestUrl}&{nameof(viewModel.FundingStreamCodes)}={viewModel.SelectedFundingStreamCodes}";

            if (viewModel.ByPassBookmark)
            {
                requestUrl = $"{requestUrl}&useBookmark=false";
            }

            var success = QueueBackGroundTask(requestUrl);

            return RedirectToRoute(
                ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmation,
                new
                {
                    success,
                    PdfGenerationAction = PdfGenerationAction.RunFeedReader
                });
        }

        /// <summary>
        /// Run feed reader action.
        /// </summary>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_PdfGenerationActionsRunFeedReader, Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsRunFeedReader)]
        public virtual async Task<IActionResult> RunFeedReader()
        {
            var viewModel = await GetBasePageViewModel<RunFeedReaderViewModel>();
            viewModel.FundingStreamCodesSource = await GetFundingStreamCodes(true);
            return View(viewModel);
        }

        /// <summary>
        /// Calls the feed reader.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The MVC view result.</returns>
        [HttpPost]
        [Route(ViewYourFundingConstants.Route_PdfGenerationActionsRunFeedReader, Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsRunFeedReader)]
        public virtual async Task<IActionResult> RunFeedReader(RunFeedReaderViewModel viewModel)
        {
            if (viewModel.FundingStreamCodes?.Any() == true)
            {
                viewModel.SelectedFundingStreamCodes = string.Join(',', viewModel.FundingStreamCodes);
            }

            if (!string.IsNullOrWhiteSpace(viewModel.SelectedFundingStreamCodes))
            {
                return View("AreYouSure", viewModel);
            }

            ModelState.AddModelError(nameof(viewModel.SelectedFundingStreamCodes), "Please select a minimum of one funding stream code.");

            viewModel.FundingStreamCodesSource = await GetFundingStreamCodes(true);

            return View(viewModel);
        }

        /// <summary>
        /// Confirmation of the pdf generation operation.
        /// </summary>
        /// <param name="success">if set to true the action has ran successfully.</param>
        /// <param name="pdfGenerationAction">The Pdf generation action.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_PdfGenerationActionsConfirmation, Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmation)]
        public virtual async Task<IActionResult> Confirmation(bool success, PdfGenerationAction pdfGenerationAction)
        {
            var viewModel = await GetBasePageViewModel<PdfGenerationOperationConfirmationViewModel>();
            viewModel.Success = success;
            viewModel.PdfGenerationAction = pdfGenerationAction;

            return View(viewModel);
        }

        /// <summary>
        /// The generate single pdf action.
        /// </summary>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_PdfGenerationActionsGenerateSinglePdf, Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsGenerateSinglePdf)]
        public virtual async Task<IActionResult> GenerateSinglePdf()
        {
            var viewModel = await GetBasePageViewModel<GenerateSinglePdfViewModel>();
            await AddGenerateSinglePdfLists(viewModel);

            return View(viewModel);
        }

        /// <summary>
        /// Runs single pdf generation.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>Redirection to confirmation page if successful.</returns>
        [HttpPost]
        [Route(ViewYourFundingConstants.Route_PdfGenerationActionsGenerateSinglePdf, Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsGenerateSinglePdf)]
        public virtual async Task<IActionResult> GenerateSinglePdf(GenerateSinglePdfViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return View("AreYouSure", viewModel);
            }

            await AddGenerateSinglePdfLists(viewModel);

            return View(viewModel);
        }

        /// <summary>
        /// Confirms the single PDF generation.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The confirmation page.</returns>
        /// <exception cref="RequestException">PDF generation url not provided.</exception>
        [HttpPost]
        [Route(ViewYourFundingConstants.Route_PdfGenerationActionsConfirmGenerateSinglePdf, Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmGenerateSinglePdf)]
        public virtual IActionResult ConfirmGenerateSinglePdf(GenerateSinglePdfViewModel viewModel)
        {
            if (string.IsNullOrEmpty(_documentGeneratorUrl))
            {
                throw new RequestException("Document Generator url not provided");
            }

            var fundingStreamAndPeriodCodeParts = viewModel.FundingStreamCodeAndPeriodCode.Split(':');

            var parameters = $"providerFundingId={viewModel.ProviderFundingId}" +
            $"&fundingStreamCode={fundingStreamAndPeriodCodeParts[0]}" +
            $"&fundingPeriodCode={fundingStreamAndPeriodCodeParts[1]}" +
            $"&ukprn={viewModel.Ukprn}" +
            $"&cutoffDate={viewModel.CutOffDate}" +
            $"&providerType={viewModel.ProviderType}" +
            $"&providerSubType={viewModel.ProviderSubType}";

            var requestUrl = UriHelper.BuildUri(_documentGeneratorUrl, parameters);
            var success = QueueBackGroundTask(requestUrl);

            return RedirectToRoute(
                ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmation,
                new
                {
                    success,
                    PdfGenerationAction = PdfGenerationAction.GenerateSinglePdf
                });
        }

        /// <summary>
        /// Last feed reader run status.
        /// </summary>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_PdfGenerationActionsFeedReaderLastRun, Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsFeedReaderLastRun)]
        public virtual async Task<IActionResult> FeedReaderLastRun()
        {
            var viewModel = await GetBasePageViewModel<FeedReaderLastRunViewModel>();
            var queryResult = await _auditService.GetDataImportAudit();
            viewModel.Audit = queryResult.First();
            var (startDateTime, endDateTime) = FormatDateTimes(viewModel.Audit.StartDateTime, viewModel.Audit.EndDateTime);
            viewModel.Audit.StartDateTime = startDateTime;
            viewModel.Audit.EndDateTime = endDateTime;
            return View(viewModel);
        }

        /// <summary>
        /// The re-run Pdf Generation action.
        /// </summary>
        /// <returns>The Mvc view result with options to choose for the re-run pdf generation.</returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_PdfGenerationActionsRerunPdfGeneration, Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsRerunPdfGeneration)]
        public virtual async Task<IActionResult> RerunPdfGeneration()
        {
            var viewModel = await GetBasePageViewModel<RerunPdfGenerationViewModel>();
            viewModel.FundingStreamCodes = await GetFundingStreamCodes();
            return View(viewModel);
        }

        /// <summary>
        /// The Re-Run Pdf Generation action.
        /// </summary>
        /// <param name="viewModel">The view model with chosen options for rerun pdf generation.</param>
        /// <returns>Redirects to confirmaton page if successful.</returns>
        [HttpPost]
        [Route(ViewYourFundingConstants.Route_PdfGenerationActionsRerunPdfGeneration, Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsRerunPdfGeneration)]
        public virtual async Task<IActionResult> RerunPdfGeneration(RerunPdfGenerationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return View("AreYouSure", viewModel);
            }

            viewModel.FundingStreamCodes = await GetFundingStreamCodes();

            return View(viewModel);
        }

        /// <summary>
        /// The confirmation for re-run pdf generation.
        /// </summary>
        /// <param name="viewModel">The Mvc view model wiith chosen options for re-run pdf generation.</param>
        /// <returns>The confirmation of the requested action.</returns>
        /// <exception cref="RequestException">ReRun Pdf generation url not provided.</exception>
        [HttpPost]
        [Route(ViewYourFundingConstants.Route_PdfGenerationActionsConfirmRerunPdfGeneration, Name = ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmRerunPdfGeneration)]
        public virtual IActionResult ConfirmRerunPdfGeneration(RerunPdfGenerationViewModel viewModel)
        {
            if (string.IsNullOrEmpty(_documentGeneratorRerunUrl))
            {
                throw new RequestException("Document Generator Rerun url not provided.");
            }

            var parameters = $"fundingStreamCode={viewModel.FundingStreamCode}" +
            $"&sinceCreatedDate={viewModel.SinceCreatedDate}" +
            $"&endDateTime={viewModel.EndDateTime}" +
            $"&resetProviderFunding={viewModel.ResetProviderFunding}" +
            $"&resetFunding={viewModel.ResetFunding}";

            var requestUrl = UriHelper.BuildUri(_documentGeneratorRerunUrl, parameters);
            var success = QueueBackGroundTask(requestUrl);

            return RedirectToRoute(
                ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmation,
                new
                {
                    success,
                    PdfGenerationAction = PdfGenerationAction.RerunPdfGeneration
                });
        }

        #endregion


        #region Private Methods

        private async Task AddFundingSelectLists(GenerateFundingReportsViewModel viewModel)
        {
            viewModel.FundingPeriodCodes = await GetFundingPeriodCodes();
            viewModel.ReportTypes = LocalAuthorityReports.GetReportTypes();
        }

        private async Task AddGenerateSinglePdfLists(GenerateSinglePdfViewModel viewModel)
        {
            viewModel.FundingStreamCodeAndPeriodCodes = await GetFundingStreamCodesAndPeriodCodes();
            viewModel.ProviderTypes = GetProviderTypesAndSubTypes();
            viewModel.ProviderSubTypes = GetProviderTypesAndSubTypes();
        }

        private async Task<IEnumerable<SelectListItem>> GetFundingStreamCodesAndPeriodCodes()
        {
            var fundingStreams = await GetRelevantFundingStreams(SettingName.PDFDocumentGenerationEnabled);

            var result = new List<SelectListItem>();
            foreach (var fundingStream in fundingStreams)
            {
                result.AddRange(fundingStream.Publications
                    .Where(publication => publication.Status == PublicationStatus.Published)
                    .Select(publication => publication.FundingPeriodCode)
                    .Distinct()
                    .Select(fundingPeriodCode =>
                        new SelectListItem(
                            $"{fundingStream.FundingStreamCode}:{fundingPeriodCode}",
                            $"{fundingStream.FundingStreamCode}:{fundingPeriodCode}")));
            }

            return result;
        }

        private async Task<IEnumerable<SelectListItem>> GetFundingPeriodCodes()
        {
            var fundingStreams = await GetRelevantFundingStreams(SettingName.FundingReportEnabled);

            var result = new List<SelectListItem>();
            foreach (var fundingStream in fundingStreams)
            {
                result.AddRange(fundingStream.Publications
                    .Where(publication => publication.Status == PublicationStatus.Published)
                    .Select(publication => publication.FundingPeriodCode)
                    .Distinct()
                    .Select(fundingPeriodCode =>
                        new SelectListItem(
                            $"{fundingStream.FundingStreamCode}-{fundingPeriodCode}",
                            $"{fundingPeriodCode}")));
            }

            return result;
        }

        private IEnumerable<SelectListItem> GetProviderTypesAndSubTypes()
        {
            return new List<SelectListItem>
            {
                 new SelectListItem { Text = "General", Value = "General" },
                 new SelectListItem { Text = "Indicative", Value = "Indicative" }
            };
        }

        #endregion
    }
}