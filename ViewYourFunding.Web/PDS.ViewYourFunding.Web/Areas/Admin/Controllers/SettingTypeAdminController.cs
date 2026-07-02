using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.SettingTypes;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The UI controller for managing the Setting types in the View Your Funding area.
    /// </summary>
    [Authorize(Policy = nameof(UserRole.SfsAdmin))]
    [Area("Admin")]
    public class SettingTypeAdminController : BaseController
    {
        #region Private fields

        /// <summary>
        /// The setting type service.
        /// </summary>
        private readonly ISettingTypeService _settingTypesService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The setting type action strategy.
        /// </summary>
        private readonly SettingTypeActionStrategy _settingTypeActionStrategy;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingTypeAdminController"/> class.
        /// </summary>
        /// <param name="settingTypesService">The setting type service.</param>
        /// <param name="adminSettingsService">The view your funding settings.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="applicationConfigurationOptions">The application configuration options.</param>
        /// <param name="logger">The logger service.</param>
        public SettingTypeAdminController(
            ISettingTypeService settingTypesService,
            IAdminSettingsService adminSettingsService,
            IMapper mapper,
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            ILoggerAdapter<SettingTypeActionBase> logger)
           : base(securityService, applicationConfigurationOptions.Value.IsProductionEnvironment)
        {
            _settingTypesService = settingTypesService;
            _mapper = mapper;
            _settingTypeActionStrategy = SettingTypeStrategyFactory.GetSettingTypeTypeActionStrategy(
                adminSettingsService,
                settingTypesService,
                mapper,
                logger);
        }

        #endregion


        #region Actions

        /// <summary>
        /// Get the setting types.
        /// </summary>
        /// <returns>The MVC view result.</returns>
        [Route(
            ViewYourFundingConstants.Route_AdminSettingTypesHome,
            Name = ViewYourFundingConstants.RouteName_AdminSettingTypesHome)]
        public virtual async Task<IActionResult> Index()
        {
            var viewModel = await GetBasePageViewModel<SettingTypesIndexViewModel>();
            var settingTypes = await _settingTypesService.GetAllSettingTypes();
            viewModel.SettingTypes = _mapper.Map<IReadOnlyList<SettingType>>(settingTypes);
            return View(viewModel);
        }

        /// <summary>Get the view result for the given setting type action.</summary>
        /// <param name="settingTypeId">The setting type identifier.</param>
        /// <param name="isSettingTypeInUse">Is setting type being used.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>The View result.</returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_AdminSettingTypeAction, Name = ViewYourFundingConstants.RouteName_AdminSettingTypeAction)]
        public virtual async Task<IActionResult> Action(int settingTypeId, bool isSettingTypeInUse, ActionMode actionMode)
        {
            var viewModel = await _settingTypeActionStrategy.SettingTypeActions.First(x => x.AppliesTo(actionMode))
                .Action(settingTypeId);

            if (viewModel != null)
            {
                var baseViewModel = await GetBasePageViewModel<SettingTypeViewModel>();
                viewModel.CurrentUser = baseViewModel.CurrentUser;
                viewModel.SettingType.IsSettingTypeInUse = isSettingTypeInUse;
                if (actionMode == ActionMode.Delete)
                {
                    return View("AreYouSure", viewModel);
                }

                return View(viewModel);
            }

            return FallBackActionResult();
        }

        /// <summary>
        /// Process the submitted action model.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>The Are you Sure page view result if the model is valid.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(ViewYourFundingConstants.Route_AdminSettingTypeAction, Name = ViewYourFundingConstants.RouteName_AdminSettingTypeAction)]
        public virtual async Task<ActionResult> Action(SettingTypeViewModel model)
        {
            var baseViewModel = await GetBasePageViewModel<SettingTypeViewModel>();
            model.CurrentUser = baseViewModel.CurrentUser;
            if (ModelState.IsValid)
            {
                return AreYouSure(model);
            }

            return View(model);
        }

        /// <summary>
        /// The 'Are you sure?' page for confirming the action on the setting type.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>The MVC view result.</returns>
        [Route(ViewYourFundingConstants.Route_AdminSettingTypeAreYouSure, Name = ViewYourFundingConstants.RouteName_AdminSettingTypeAreYouSure)]
        public virtual ActionResult AreYouSure(SettingTypeViewModel model)
        {
            model.IsAreYouSurePage = true;
            return View("AreYouSure", model);
        }

        /// <summary>
        /// Persist the setting type action.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>The redirect to the confirmation page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(ViewYourFundingConstants.Route_AdminSettingTypeSaveChanges, Name = ViewYourFundingConstants.RouteName_AdminSettingTypeSaveChanges)]
        public virtual async Task<IActionResult> SaveChanges(SettingTypeViewModel model)
        {
            var baseViewModel = await GetBasePageViewModel<SettingTypeViewModel>();
            model.SettingType.LastUpdatedBy = model.CurrentUser?.FullName;

            var changesSaved = await _settingTypeActionStrategy.SettingTypeActions.First(x => x.AppliesTo(model.ActionMode))
                .SaveChanges(model);

            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminSettingTypeConfirmation, new
            {
                settingTypeId = model.SettingType.Id,
                settingTypeName = model.SettingType.SettingName,
                settingTypeDescription = model.SettingType.SettingDescription,
                valueDataType = model.SettingType.ValueDataType,
                isSettingTypeInUse = model.SettingType.IsSettingTypeInUse,
                actionMode = model.ActionMode,
                changesSaved
            });
        }

        /// <summary>
        /// Confirmation of the save operation.
        /// </summary>
        /// <param name="settingTypeId">The setting type identifier.</param>
        /// <param name="settingTypeName">The setting type name.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <param name="changesSaved">True is changes are saved.</param>
        /// <returns>The MVC view result.</returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_AdminSettingTypeConfirmation, Name = ViewYourFundingConstants.RouteName_AdminSettingTypeConfirmation)]
        public virtual async Task<IActionResult> Confirmation(int settingTypeId, string settingTypeName, ActionMode actionMode, bool changesSaved)
        {
            var viewModel = await GetBasePageViewModel<SettingTypeViewModel>();

            var submittedAtDisplayDate = string.Empty;
            if (changesSaved && actionMode != ActionMode.Delete)
            {
                var settingTypes = await _settingTypesService.GetAllSettingTypes();
                submittedAtDisplayDate = settingTypes.FirstOrDefault(x => x.Id == settingTypeId)?.LastUpdatedAt
                    .ToGmtStandardTime().ToDateTimeDisplayWithAt();
            }

            viewModel.SettingTypeName = settingTypeName;
            viewModel.ActionMode = actionMode;
            viewModel.ChangesSaved = changesSaved;
            viewModel.SubmittedAtDisplayDate = submittedAtDisplayDate;

            return View(viewModel);
        }

        #endregion


        #region Private Helpers

        /// <summary>Returns the fall back action result.</summary>
        /// <returns>The Redirect Action Result.</returns>
        private ActionResult FallBackActionResult()
        {
            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminSettingTypesHome);
        }

        #endregion

    }
}