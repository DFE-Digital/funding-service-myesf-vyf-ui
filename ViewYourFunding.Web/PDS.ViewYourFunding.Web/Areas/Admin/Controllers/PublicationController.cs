using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.Publications;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Publication = PDS.ViewYourFunding.Services.Models.Publication;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The UI controller for managing the funding stream publications in the View Your Funding area.
    /// </summary>
    [Authorize(Policy = nameof(UserRole.SfsAdmin))]
    [Area("Admin")]
    public class PublicationController : BaseController
    {
        #region Private fields

        /// <summary>
        /// The Api Date Format.
        /// </summary>
        private const string ApiDateFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// The view your funding settings service.
        /// </summary>
        private readonly IAdminSettingsService _adminSettingsService;

        /// <summary>
        /// The view your funding publication service.
        /// </summary>
        private readonly IAdminPublicationService _adminPublicationService;

        /// <summary>
        /// The generate spreadsheet service.
        /// </summary>
        private readonly IGenerateSpreadsheetService _generateSpreadsheetService;

        /// <summary>
        /// The publication spreadsheet meta service.
        /// </summary>
        private readonly IPublicationSpreadsheetMetaDataService _publicationSpreadsheetMetaService;

        /// <summary>
        /// The back ground task queue service.
        /// </summary>
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        private readonly ILayoutManagementService _layoutManagementService;

        /// <summary>
        /// The base path service.
        /// </summary>
        private readonly IBasePathService _basePathService;

        private readonly ICacheService _cacheService;

        /// <summary>
        /// The publication action strategy.
        /// </summary>
        private readonly PublicationActionStrategy _publicationActionStrategy;

        private readonly ILoggerAdapter<PublicationActionBase> _logger;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationController"/> class.
        /// </summary>
        /// <param name="adminSettingsService">The view your funding settings.</param>
        /// <param name="viewYourFundingPublicationService">The view your funding publication service.</param>
        /// <param name="fundingUiModelDetailsService">The funding Ui Model Details service.</param>
        /// <param name="generateSpreadsheetService">The generate spreadsheet service.</param>
        /// <param name="publicationSpreadsheetMetaService">The publication spreadsheet meta service.</param>
        /// <param name="backgroundTaskQueue">The background queue service.</param>
        /// <param name="layoutManagementService">the layout management service.</param>
        /// <param name="basePathService">The base path service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="applicationConfigurationOptions">The application configuration options.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="logger">The logger service.</param>
        public PublicationController(
            IAdminSettingsService adminSettingsService,
            IAdminPublicationService viewYourFundingPublicationService,
            IFundingUiModelDetailsService fundingUiModelDetailsService,
            IGenerateSpreadsheetService generateSpreadsheetService,
            IPublicationSpreadsheetMetaDataService publicationSpreadsheetMetaService,
            IBackgroundTaskQueue backgroundTaskQueue,
            ILayoutManagementService layoutManagementService,
            IBasePathService basePathService,
            IMapper mapper,
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            ICacheService cacheService,
            ILoggerAdapter<PublicationActionBase> logger)
            : base(securityService, applicationConfigurationOptions.Value.IsProductionEnvironment)
        {
            _adminSettingsService = adminSettingsService;
            _adminPublicationService = viewYourFundingPublicationService;
            _generateSpreadsheetService = generateSpreadsheetService;
            _publicationSpreadsheetMetaService = publicationSpreadsheetMetaService;
            _backgroundTaskQueue = backgroundTaskQueue;
            _layoutManagementService = layoutManagementService;
            _basePathService = basePathService;
            _publicationActionStrategy = PublicationStrategyFactory.GetPublicationActionStrategy(
                adminSettingsService,
                viewYourFundingPublicationService,
                fundingUiModelDetailsService,
                layoutManagementService,
                mapper,
                logger);
            _cacheService = cacheService;
            _logger = logger;
        }

        #endregion


        #region Actions

        /// <summary>Get the view result for the given publication action.</summary>
        /// <param name="publicationId">The publication identifier.</param>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>The View result.</returns>
        [HttpGet]
        [Route(
            ViewYourFundingConstants.Route_AdminPublicationAction,
            Name = ViewYourFundingConstants.RouteName_AdminPublicationAction)]
        public virtual async Task<IActionResult> Action(int publicationId, int fundingStreamId, ActionMode actionMode)
        {
            var viewModel = await _publicationActionStrategy.PublicationActions.First(action => action.AppliesTo(actionMode))
                .Action(fundingStreamId, publicationId);
            var baseViewModel = await GetBasePageViewModel<PublicationActionViewModel>();
            viewModel.CurrentUser = baseViewModel.CurrentUser;

            if (viewModel.FundingPublication != null)
            {
                if (actionMode == ActionMode.Delete)
                {
                    await AddLayoutModels(viewModel);
                    return View("AreYouSure", viewModel);
                }

                return View(viewModel);
            }

            return FallBackActionResult(fundingStreamId);
        }

        /// <summary>
        /// Process the submitted action model.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>The Are you Sure page view result if the model is valid.</returns>
        [HttpPost]
        [Route(
            ViewYourFundingConstants.Route_AdminPublicationAction,
            Name = ViewYourFundingConstants.RouteName_AdminPublicationAction)]
        public virtual async Task<IActionResult> Action(PublicationActionViewModel model)
        {
            var baseViewModel = await GetBasePageViewModel<PublicationActionViewModel>();
            model.CurrentUser = baseViewModel.CurrentUser;

            if (ModelState.IsValid)
            {
                await AddLayoutModels(model);
                return AreYouSure(model);
            }

            await AddLayoutModels(model);

            return View(model);
        }


        /// <summary>
        /// The 'Are you sure?' page for confirming the action on the publication.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>The MVC view result.</returns>
        [Route(
            ViewYourFundingConstants.Route_AdminPublicationAreYouSure,
            Name = ViewYourFundingConstants.RouteName_AdminPublicationAreYouSure)]
        public virtual IActionResult AreYouSure(PublicationActionViewModel model)
        {
            model.IsAreYouSurePage = true;
            return View("AreYouSure", model);
        }

        /// <summary>
        /// Persist the publication action.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>The redirect to the confirmation page.</returns>
        [HttpPost]
        [Route(
            ViewYourFundingConstants.Route_AdminPublicationSaveChanges,
            Name = ViewYourFundingConstants.RouteName_AdminPublicationSaveChanges)]
        public virtual async Task<IActionResult> SaveChanges(PublicationActionViewModel model)
        {
            var baseViewModel = await GetBasePageViewModel<PublicationActionViewModel>();
            model.FundingPublication.LastUpdatedBy = baseViewModel.CurrentUser?.FullName;

            var changesSaved = await _publicationActionStrategy.PublicationActions
                .First(action => action.AppliesTo(model.ActionMode))
                .SaveChanges(model);

            _cacheService?.ClearCache();

            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminPublicationConfirmation, new
            {
                model.FundingStreamId,
                model.FundingStreamName,
                model.ActionMode,
                publicationId = model.FundingPublication.Id,
                changesSaved
            });
        }

        /// <summary>
        /// Confirmation of the save operation.
        /// </summary>
        /// <param name="publicationId">The publication identifier.</param>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="fundingStreamName">The funding stream name.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <param name="changesSaved">if set to true if the changes were saved successfully.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The MVC view result.</returns>
        [HttpGet]
        [Route(
            ViewYourFundingConstants.Route_AdminPublicationConfirmation,
            Name = ViewYourFundingConstants.RouteName_AdminPublicationConfirmation)]
        public virtual async Task<IActionResult> Confirmation(
            int publicationId,
            int fundingStreamId,
            string fundingStreamName,
            ActionMode actionMode,
            bool changesSaved,
            string errorMessage)
        {
            var viewModel = await GetBasePageViewModel<PublicationActionViewModel>();

            var submittedAtDisplayDate = string.Empty;

            if (changesSaved && actionMode != ActionMode.Delete)
            {
                var publications = await _adminPublicationService.GetPublications(fundingStreamId);

                var publication = publications.FirstOrDefault(x => x.Id == publicationId);

                if (publication != null)
                {
                    submittedAtDisplayDate = publication.LastUpdatedAt
                        .ToGmtStandardTime().ToDateTimeDisplayWithAt();
                    if (actionMode == ActionMode.GenerateSpreadSheet)
                    {
                        GetSpreadsheetGenerationData(viewModel, publication);
                    }
                }
            }

            viewModel.FundingStreamId = fundingStreamId;
            viewModel.FundingStreamName = fundingStreamName;
            viewModel.ActionMode = actionMode;
            viewModel.ChangesSaved = changesSaved;
            viewModel.ErrorMessage = errorMessage;
            viewModel.SubmittedAtDisplayDate = submittedAtDisplayDate;

            return View(viewModel);
        }

        /// <summary>
        /// Generates the spreadsheet for the publication.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream id.</param>
        /// <param name="publicationId">The publication id.</param>
        /// <returns>The Redirect Action result.</returns>
        [Route(
            ViewYourFundingConstants.Route_AdminPublicationGenerateSpreadsheet,
            Name = ViewYourFundingConstants.RouteName_AdminPublicationGenerateSpreadsheet)]
        public async Task<IActionResult> GenerateSpreadsheet(int fundingStreamId, int publicationId)
        {
            var publications = await _adminPublicationService.GetPublications(fundingStreamId);

            var publication = publications.FirstOrDefault(x => x.Id == publicationId);

            var result = publication != null ? await GenerateSpreadSheet(publication) : null;

            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminPublicationConfirmation, new
            {
                fundingStreamId,
                actionMode = ActionMode.GenerateSpreadSheet,
                publicationId,
                changesSaved = result?.Success,
                result?.ErrorMessage
            });
        }

        /// <summary>
        /// Get the created date of the file, or null (if its not set or file not found).
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code to check for (e.g. PSG).</param>
        /// <param name="fundingPeriodCode">The funding period code to check for (e.g. AY-1920).</param>
        /// <param name="publishedDate">The published date in a date time parse-able format.</param>
        /// <param name="startDateTime">The start date time when polling started from the client side.</param>
        /// <returns>A string of the created date of the file, or null.</returns>
        [HttpGet]
        [Route(
            ViewYourFundingConstants.Route_AdminPublicationGetSpreadsheetCreateDate,
            Name = ViewYourFundingConstants.RouteName_AdminPublicationGetSpreadsheetCreateDate)]
        public async Task<string> GetSpreadsheetCreateDate(string fundingStreamCode, string fundingPeriodCode, string publishedDate, string startDateTime)
        {
            var metadata = await _publicationSpreadsheetMetaService.GetPublicationSpreadsheetMetaDataAsync(new Publication
            {
                FundingStream = new FundingStream
                {
                    FundingStreamCode = fundingStreamCode
                },
                FundingPeriodCode = fundingPeriodCode,
                PublishedDate = DateTime.Parse(publishedDate)
            });

            var result = metadata?.CreatedDateTime.HasValue == true ?
                metadata.CreatedDateTime.Value.ToIncrementalDateFormat() : null;

            _logger.LogInformation($"GetSpreadsheetCreateDate Start Date : {startDateTime} and last created spreadsheet at {result}");

            return result;
        }

        #endregion


        #region Private Helpers

        private static FundingViewScope GetFundingViewScope(string fundingViewScope)
        {
            Enum.TryParse<FundingViewScope>(fundingViewScope, out var result);

            return result;
        }

        private static FundingViewType GetFundingViewType(string fundingViewType)
        {
            Enum.TryParse<FundingViewType>(fundingViewType, out var result);

            return result;
        }

        /// <summary>
        /// Gets the spreadsheet URL with parameters.
        /// </summary>
        /// <param name="baseSpreadsheetUrl">The base spreadsheet URL.</param>
        /// <param name="publication">The publication.</param>
        /// <returns>Returns Url to generate spreadsheet.</returns>
        private static string GetSpreadSheetUrlWithParameters(
            string baseSpreadsheetUrl,
            Publication publication)
        {
            var publicationDate = publication.PublishedDate;

            var publicationCutOffDate = publication.CutOffDate ?? publication.PublishedDate;

            return string.Format(
                baseSpreadsheetUrl,
                publication.FundingStream.FundingStreamCode,
                publication.FundingPeriodCode,
                publicationCutOffDate.ToString(ApiDateFormat),
                publicationDate.ToString(ApiDateFormat),
                publication.SpreadsheetModelVersion);
        }

        private static void GetSpreadsheetGenerationData(PublicationActionViewModel viewModel, Publication publication)
        {
            viewModel.FundingStreamCode = publication.FundingStream.FundingStreamCode;
            viewModel.FundingPublication = new PublicationViewModel
            {
                FundingPeriodCode = publication.FundingPeriodCode,
                PublishedDateDay = publication.PublishedDate.Day.ToString(),
                PublishedDateMonth = publication.PublishedDate.Month.ToString(),
                PublishedDateYear = publication.PublishedDate.Year.ToString()
            };
        }

        private async Task AddLayoutModels(PublicationActionViewModel model)
        {
            var layouts = await _layoutManagementService.GetAllLayoutsAsync(
                new List<Expression<Func<LayoutModel, bool>>>
                {
                    layout => layout.FundingStreamId == model.FundingStreamId
                        && (layout.DeletedDateTime == null || !layout.DeletedDateTime.IsDefined())
                });

            model.FundingPublication.LayoutUiModels = layouts.Select(MapLayoutUiModel).ToList();
        }

        /// <summary>Returns the fall back action result.</summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <returns>The Redirect Action Result.</returns>
        private ActionResult FallBackActionResult(int fundingStreamId)
        {
            if (fundingStreamId != 0)
            {
                return RedirectToRoute(
                    ViewYourFundingConstants.RouteName_AdminSettingsFundingStream,
                    new { fundingStreamId });
            }

            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminSettingsHome);
        }

        /// <summary>
        /// Generates the spreadsheet.
        /// </summary>
        /// <param name="publication">The publication.</param>
        /// <returns>Returns bool to indicate whether spreadsheet was generated.</returns>
        private async Task<GenerateSpreadsheetResult> GenerateSpreadSheet(Publication publication)
        {
            var generateSpreadSheetUrlSettingValue =
                await _adminSettingsService.GetSettingsById(publication.FundingStream.Id);

            var generateSpreadSheetUrlSetting = generateSpreadSheetUrlSettingValue.FirstOrDefault(setting =>
                setting.Setting.SettingName.Equals(
                    ServiceConstants.UpdateSpreadsheetUrlSettingName,
                    StringComparison.InvariantCultureIgnoreCase));

            if (generateSpreadSheetUrlSetting == null)
            {
                var basePath = _basePathService.GetApplicationBasePath();

                // Use default path
                generateSpreadSheetUrlSetting = new SettingValue
                {
                    Value = $"{basePath}/api/funding/GenerateFundingDocument?fundingStreamCode={0}&fundingPeriodCode={1}&cutoffDate={2}&publicationDate={3}&modelVersion={4}&waitForIndexBuild=true"
                };
            }

            var modifiedSpreadsheetUrl = GetSpreadSheetUrlWithParameters(
                generateSpreadSheetUrlSetting.Value,
                publication);

            _backgroundTaskQueue.QueueTask(async token =>
             {
                 await _generateSpreadsheetService.GenerateFundingStreamSpreadSheetByUrlAsync(
                     modifiedSpreadsheetUrl,
                     generateSpreadSheetUrlSetting.Setting.SettingName,
                     generateSpreadSheetUrlSetting.FundingStream.FundingStreamName);
             });

            return new GenerateSpreadsheetResult
            {
                Success = true
            };
        }

        private LayoutUiModel MapLayoutUiModel(LayoutModel layoutModel)
        {
            return new LayoutUiModel
            {
                LayoutName = layoutModel.LayoutName,
                LayoutId = layoutModel.Id,
                LastModifiedDateTime = layoutModel.LastModifiedDateTime.ConvertUtcDateTimeToGmtDateTime(),
                FundingViewType = GetFundingViewType(layoutModel.FundingViewType),
                FundingViewScope = GetFundingViewScope(layoutModel.FundingViewScope)
            };
        }

        #endregion
    }
}