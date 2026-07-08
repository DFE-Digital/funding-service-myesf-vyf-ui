using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.GlobalSettingAdmin;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.DataValueTypes;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalSetting = PDS.ViewYourFunding.Web.Areas.Admin.Models.GlobalSetting.GlobalSetting;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The UI controller for managing the global settings area.
    /// </summary>
    /// <seealso cref="BaseController" />
    [Authorize(Policy = nameof(UserRole.SfsAdmin))]
    [Area("Admin")]
    public class GlobalSettingAdminController : BaseController
    {
        #region Private Fields

        /// <summary>
        /// The global setting service.
        /// </summary>
        private readonly IGlobalSettingService _globalSettingService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The cache service.
        /// </summary>
        private readonly ICacheService _cacheService;

        /// <summary>
        /// The data type edit strategy.
        /// </summary>
        private readonly DataTypeEditStrategy _dataTypeEditStrategy;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalSettingAdminController" /> class.
        /// </summary>
        /// <param name="globalSettingService">The global setting service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="applicationConfigurationOptions">The application configuration options.</param>
        public GlobalSettingAdminController(
            IGlobalSettingService globalSettingService,
            IMapper mapper,
            IClaimsBasedIdentityService securityService,
            ICacheService cacheService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions)
            : base(securityService, applicationConfigurationOptions.Value.IsProductionEnvironment)
        {
            _globalSettingService = globalSettingService;
            _mapper = mapper;
            _dataTypeEditStrategy = DataTypeEditStrategyFactory.GetDataTypeEditStrategy();
            _cacheService = cacheService;
        }

        #endregion


        #region Actions

        /// <summary>
        /// The start action for global admin settings.
        /// </summary>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(
            ViewYourFundingConstants.Route_AdminGeneralSettingHome,
            Name = ViewYourFundingConstants.RouteName_AdminGeneralSettingHome)]
        public virtual async Task<IActionResult> Index()
        {
            var viewModel = await GetBasePageViewModel<GlobalSettingListViewModel>();
            viewModel.GlobalSettings = _mapper.Map<IReadOnlyList<GlobalSetting>>(await _globalSettingService.GetAllGlobalSettings());
            return View(viewModel);
        }

        /// <summary>
        /// Edits the specified global setting by its identifier.
        /// </summary>
        /// <param name="globalSettingId">The global setting identifier.</param>
        /// <returns>The IAction result.</returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_AdminGeneralSettingEdit, Name = ViewYourFundingConstants.RouteName_AdminGeneralSettingEdit)]
        public virtual async Task<IActionResult> Edit(int globalSettingId)
        {
            var globalSetting = await _globalSettingService.Get(globalSettingId);

            var globalSettingToEdit = _mapper.Map<GlobalSetting>(globalSetting);

            if (globalSettingToEdit != null)
            {
                var viewModel = _dataTypeEditStrategy.DataTypeEdits.First(
                        dataTypeEdit => dataTypeEdit.AppliesTo(globalSettingToEdit.EditType))
                    .Edit(
                        globalSettingToEdit.Id,
                        globalSettingToEdit.Value,
                        globalSettingToEdit.Description);

                var baseViewModel = await GetBasePageViewModel<DataTypeBaseEdit>();
                viewModel.CurrentUser = baseViewModel.CurrentUser;
                viewModel.IsCurrentPageEditPage = true;

                return View(viewModel);
            }

            return FallBackActionResult();
        }

        /// <summary>
        /// Process the submitted global settings edit model.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>
        /// The Are you Sure page view result if the model is valid.
        /// </returns>
        [HttpPost]
        [Route(ViewYourFundingConstants.Route_AdminGeneralSettingEdit, Name = ViewYourFundingConstants.RouteName_AdminGeneralSettingEdit)]
        public virtual async Task<IActionResult> Edit(DataTypeBaseEdit model)
        {
            var baseViewModel = await GetBasePageViewModel<DataTypeBaseEdit>();
            model.CurrentUser = baseViewModel.CurrentUser;
            if (ModelState.IsValid)
            {
                model.NewValueString = _dataTypeEditStrategy.DataTypeEdits
                    .First(x => x.AppliesTo(model.SettingEditType)).GetNewValue(model);
                return AreYouSure(model);
            }

            return View(model);
        }

        /// <summary>
        /// The 'Are you sure?' page for confirming the action on the global setting.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        public virtual IActionResult AreYouSure(DataTypeBaseEdit model)
        {
            return View("AreYouSure", model);
        }

        /// <summary>
        /// Persist the global setting action.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>
        /// The redirect to the confirmation page.
        /// </returns>
        [HttpPost]
        [Route(
            ViewYourFundingConstants.Route_AdminGeneralSettingSaveChanges,
            Name = ViewYourFundingConstants.RouteName_AdminGeneralSettingSaveChanges)]
        public virtual async Task<IActionResult> SaveChanges(DataTypeBaseEdit model)
        {
            var baseViewModel = await GetBasePageViewModel<GlobalSettingConfirmationViewModel>();
            var globalSetting = await _globalSettingService.Get(model.DataTypeId);
            var changesSaved = false;

            var globalSettingToSave =
                _mapper.Map<GlobalSetting>(globalSetting);

            if (globalSettingToSave != null)
            {
                globalSettingToSave.LastUpdatedBy = baseViewModel.CurrentUser?.FullName;
                globalSettingToSave.Value = model.NewValueString;

                changesSaved = await _globalSettingService.UpdateAsync(
                        _mapper.Map<Services.Models.GlobalSetting>(globalSettingToSave));

                var cacheKey = GlobalSettingTypeConstants.GetCacheKey(globalSettingToSave.Type);

                if (!string.IsNullOrEmpty(cacheKey))
                {
                    _cacheService.RemoveCacheItem(cacheKey);
                }
            }

            return RedirectToRoute(
                ViewYourFundingConstants.RouteName_AdminGeneralSettingConfirmation,
                new
                {
                    globalSettingId = model.DataTypeId,
                    changesSaved
                });
        }

        /// <summary>
        /// Confirmation of the save operation.
        /// </summary>
        /// <param name="globalSettingId">The global setting identifier.</param>
        /// <param name="changesSaved">if set to true if the changes were saved successfully.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_AdminGeneralSettingConfirmation, Name = ViewYourFundingConstants.RouteName_AdminGeneralSettingConfirmation)]
        public virtual async Task<IActionResult> Confirmation(
            int globalSettingId,
            bool changesSaved)
        {
            var globalSetting = await _globalSettingService.Get(globalSettingId);

            var viewModelGlobalSetting = _mapper.Map<GlobalSetting>(globalSetting);
            if (viewModelGlobalSetting?.Id > 0)
            {
                var viewModel = await GetBasePageViewModel<GlobalSettingConfirmationViewModel>();
                var submittedAtDisplayDate = changesSaved
                    ? viewModelGlobalSetting.UpdatedAt.ToGmtStandardTime().ToDateTimeDisplayWithAt()
                    : string.Empty;

                viewModel.GlobalSetting = viewModelGlobalSetting;
                viewModel.ChangesSaved = changesSaved;
                viewModel.SubmittedAtDisplayDate = submittedAtDisplayDate;

                return View(viewModel);
            }

            return FallBackActionResult();
        }

        #endregion


        #region Private Helpers

        private IActionResult FallBackActionResult()
        {
            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminGeneralSettingHome);
        }

        #endregion
    }
}