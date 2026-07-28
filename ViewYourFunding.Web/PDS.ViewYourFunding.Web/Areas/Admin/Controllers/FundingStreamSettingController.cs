using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Helpers;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStreamSettings;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewYourFunding.Services.Models;
using Model = PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The UI controller for managing the settings in the View Your Funding area.
    /// </summary>
    [Authorize(Policy = nameof(UserRole.SfsAdmin))]
    [Area("Admin")]
    public class FundingStreamSettingController : BaseController
    {
        #region Private Fields

        /// <summary>
        /// The view your funding settings repository.
        /// </summary>
        private readonly IAdminSettingsService _adminSettingsService;

        /// <summary>
        /// The admin funding stream setting service.
        /// </summary>
        private readonly IAdminFundingStreamSettingService _adminFundingStreamSettingService;

        /// <summary>
        /// The setting type service.
        /// </summary>
        private readonly ISettingTypeService _settingTypeService;

        /// <summary>
        /// The layout management service.
        /// </summary>
        private readonly ILayoutManagementService _layoutManagementService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The funding document storage service.
        /// </summary>
        private readonly IPublicationSpreadsheetMetaDataService _publicationSpreadsheetMetaService;

        /// <summary>
        /// The cache service.
        /// </summary>
        private readonly ICacheService _cacheService;

        /// <summary>
        /// The funding stream setting action strategy.
        /// </summary>
        private readonly FundingStreamSettingActionStrategy _fundingStreamSettingActionStrategy;

        /// <summary>
        /// The national layout setting value data types.
        /// </summary>
        private readonly IReadOnlyList<SettingValueDataType> _nationalLayoutSettingValueDataTypes
            = new List<SettingValueDataType>
            {
                SettingValueDataType.NationalSpreadsheetLayout,
                SettingValueDataType.NationalLayout
            };

        /// <summary>
        /// The national layout setting value data types.
        /// </summary>
        private readonly IReadOnlyList<Services.Enums.SettingValueDataType> _nationalLayoutServiceSettingValueDataTypes
            = new List<Services.Enums.SettingValueDataType>
            {
                Services.Enums.SettingValueDataType.NationalSpreadsheetLayout,
                Services.Enums.SettingValueDataType.NationalLayout
            };

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingStreamSettingController"/> class.
        /// </summary>
        /// <param name="adminSettingsService">The admin settings service.</param>
        /// <param name="adminFundingStreamSettingService">The admin funding stream settings service.</param>
        /// <param name="settingTypesService">The setting type service.</param>
        /// <param name="layoutManagementService">The layout management service.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="publicationSpreadsheetMetaService">The funding document storage service.</param>
        /// <param name="applicationConfigurationOptions">The application configuration options.</param>
        /// <param name="cacheService">The cache service.</param>
        public FundingStreamSettingController(
            IAdminSettingsService adminSettingsService,
            IAdminFundingStreamSettingService adminFundingStreamSettingService,
            ISettingTypeService settingTypesService,
            ILayoutManagementService layoutManagementService,
            IClaimsBasedIdentityService securityService,
            IMapper mapper,
            IPublicationSpreadsheetMetaDataService publicationSpreadsheetMetaService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            ICacheService cacheService)
        : base(securityService, applicationConfigurationOptions.Value.IsProductionEnvironment)
        {
            _adminSettingsService = adminSettingsService;
            _adminFundingStreamSettingService = adminFundingStreamSettingService;
            _settingTypeService = settingTypesService;
            _layoutManagementService = layoutManagementService;
            _mapper = mapper;
            _publicationSpreadsheetMetaService = publicationSpreadsheetMetaService;
            _cacheService = cacheService;
            _fundingStreamSettingActionStrategy =
                FundingStreamSettingStrategyFactory.GetFundingStreamSettingActionStrategy(adminFundingStreamSettingService);
        }

        #endregion


        #region Actions

        /// <summary>
        /// The landing page for the user to choose a funding stream for which to view/edit settings.
        /// </summary>
        /// <returns>The MVC view result.</returns>
        [Route(ViewYourFundingConstants.Route_AdminSettingsHome, Name = ViewYourFundingConstants.RouteName_AdminSettingsHome)]
        public async Task<IActionResult> Index()
        {
            var viewModel = await GetBasePageViewModel<FundingStreamsListViewModel>();
            var fundingStreams = await _adminSettingsService.GetAllFundingStreams(
                Repositories.Enums.FetchData.Publications,
                Repositories.Enums.FetchData.NextPayments_NextPaymentType,
                Repositories.Enums.FetchData.NextPaymentTypes_NextPayments,
                Repositories.Enums.FetchData.SettingValues_Setting);
            viewModel.FundingStreams = fundingStreams.OrderBy(fundingStream => fundingStream.FundingStreamName).ToList();
            return View(viewModel);
        }

        /// <summary>
        /// The page listing the settings for a given funding stream.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream Id.</param>
        /// <returns>The MVC view result.</returns>
        [Route(ViewYourFundingConstants.Route_AdminSettingsFundingStream, Name = ViewYourFundingConstants.RouteName_AdminSettingsFundingStream)]
        public virtual async Task<IActionResult> FundingStream(int fundingStreamId)
        {
            var fundingStream = await _adminSettingsService.GetFundingStreamById(
                fundingStreamId,
                Repositories.Enums.FetchData.Publications,
                Repositories.Enums.FetchData.SettingValues_Setting);

            if (fundingStream?.Id.Equals(default) == false)
            {
                var viewModel = await GetBasePageViewModel<FundingStreamSettingsViewModel>();
                viewModel.FundingPublications = await GetPublicationsWithMetaData(fundingStream.Publications);
                viewModel.FundingStreamSettings = await GetFundingStreamSettings(fundingStream.SettingValues.Where(fsSetting => fsSetting.Setting.ValuesAreEditable).ToList());
                viewModel.FundingStream = new Model.FundingStream
                {
                    FundingStreamId = fundingStream.Id,
                    FundingStreamName = fundingStream.FundingStreamName,
                    FundingStreamCode = fundingStream.FundingStreamCode,
                    Active = fundingStream.Active
                };
                await SetAddNewSettingFlag(fundingStream, viewModel);
                return View(viewModel);
            }

            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminSettingsHome);
        }

        /// <summary>
        /// Gets the page the specified funding stream setting action.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="settingValueId">The setting value identifier.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>The MVC View result.</returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_AdminFundingStreamSettingAction, Name = ViewYourFundingConstants.RouteName_AdminFundingStreamSettingAction)]
        public virtual async Task<IActionResult> Action(int fundingStreamId, int settingValueId, FundingStreamSettingAction actionMode)
        {
            var fundingStreamSetting = await _adminFundingStreamSettingService.GetFirstOrDefaultAsync(settingValueId, fundingStreamId);

            if (fundingStreamSetting != null)
            {
                if (actionMode == FundingStreamSettingAction.Delete)
                {
                    var areYouSureViewModel = await GetBasePageViewModel<FundingStreamSettingAreYouSureViewModel>();
                    UpdateAreYouSureViewMode(areYouSureViewModel, fundingStreamSetting, fundingStreamSetting.Value);
                    areYouSureViewModel.ActionMode = actionMode;
                    if (_nationalLayoutServiceSettingValueDataTypes.Contains(fundingStreamSetting.Setting.ValueDataType))
                    {
                        await SetupAreYouSureNationalLayoutViewModelProperties(areYouSureViewModel);
                    }

                    return View("AreYouSure", areYouSureViewModel);
                }

                var viewModel = await GetBasePageViewModel<FundingStreamSettingActionViewModel>();
                viewModel.ActionMode = actionMode;
                await SetupFundingStreamSettingActionViewModel(fundingStreamId, viewModel, fundingStreamSetting);
                return View(viewModel);
            }

            return RedirectToRoute(
                ViewYourFundingConstants.RouteName_AdminSettingsFundingStream,
                new
                {
                    fundingStreamId
                });
        }

        /// <summary>
        /// Actions the specified funding stream setting view model.
        /// </summary>
        /// <param name="model">The view model.</param>
        /// <returns>The MVC View result.</returns>
        [HttpPost]
        [Route(ViewYourFundingConstants.Route_AdminFundingStreamSettingAction, Name = ViewYourFundingConstants.RouteName_AdminFundingStreamSettingAction)]
        public async Task<IActionResult> Action(FundingStreamSettingActionViewModel model)
        {
            ClearModelErrors(model);

            if (ModelState.IsValid)
            {
                var viewModel = await GetFundingStreamSettingAreYouSureViewModel(model);
                if (_nationalLayoutSettingValueDataTypes.Contains(model.SettingValueDataType))
                {
                    await SetupAreYouSureNationalLayoutViewModelProperties(viewModel);
                }

                return View("AreYouSure", viewModel);
            }

            var baseViewModel = await GetBasePageViewModel<FundingStreamSettingActionViewModel>();
            model.CurrentUser = baseViewModel.CurrentUser;

            await SetupNationalLayoutViewModelProperties(model);
            return View(model);
        }

        /// <summary>
        /// Saves the setting value action to the data store.
        /// </summary>
        /// <param name="viewmodel">The view-model to persist the update of a setting value.</param>
        /// <returns>The redirect to the settings page for the current funding stream.</returns>
        [Route(ViewYourFundingConstants.Route_AdminFundingStreamSettingSaveChanges, Name = ViewYourFundingConstants.RouteName_AdminSettingSaveChanges)]
        public virtual async Task<IActionResult> SaveChanges(FundingStreamSettingAreYouSureViewModel viewmodel)
        {
            var baseViewModel = await GetBasePageViewModel<FundingStreamSettingAreYouSureViewModel>();
            viewmodel.CurrentUser = baseViewModel.CurrentUser;
            var success = await _fundingStreamSettingActionStrategy.FundingStreamSettingActions
                .First(action => action.AppliesTo(viewmodel.ActionMode)).SaveChanges(viewmodel);

            _cacheService.ClearCache();

            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminFundingStreamSettingConfirmation, new
            {
                viewmodel.FundingStreamId,
                success,
                viewmodel.ActionMode
            });
        }

        /// <summary>
        /// The confirmation page action.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="success">Success status. True if the operation was a success.</param>
        /// <param name="actionMode">Action mode e.g. Edit, Delete or Add.</param>
        /// <returns>Shows the confirmation page.</returns>
        [Route(
            ViewYourFundingConstants.Route_AdminFundingStreamSettingConfirmation,
            Name = ViewYourFundingConstants.RouteName_AdminFundingStreamSettingConfirmation)]
        public virtual async Task<ActionResult> Confirmation(
            int fundingStreamId,
            bool success,
            FundingStreamSettingAction actionMode)
        {
            var fundingStream = await _adminSettingsService.GetFundingStreamById(fundingStreamId);

            if (fundingStream != null)
            {
                var viewModel = await GetBasePageViewModel<FundingStreamSettingConfirmationViewModel>();
                viewModel.ActionMode = actionMode;
                viewModel.FundingStreamId = fundingStreamId;
                viewModel.SubmittedAtDisplayDate = DateTime.Now.ToDateTimeDisplayWithAt();
                viewModel.FundingStreamName = fundingStream.FundingStreamName;
                viewModel.ChangesSaved = success;
                return View(viewModel);
            }

            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminSettingsHome);
        }

        /// <summary>
        /// Sets the type of the funding stream setting.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <returns>The MVC View result.</returns>
        [Route(ViewYourFundingConstants.Route_AdminFundingStreamSettingSetType, Name = ViewYourFundingConstants.RouteName_AdminFundingStreamSettingSetType)]
        public async Task<IActionResult> SetSettingType(int fundingStreamId)
        {
            var fundingStream = await _adminSettingsService.GetFundingStreamById(fundingStreamId, Repositories.Enums.FetchData.SettingValues_Setting);
            if (fundingStream?.Id.Equals(default) == false)
            {
                var viewModel = await GetBasePageViewModel<SetSettingTypeViewModel>();
                viewModel.FundingStreamId = fundingStreamId;
                var settingTypesAvailable = await _settingTypeService.GetAvailableSettingTypes(
                    fundingStream.SettingValues.Select(
                        settingValue => settingValue.SettingId));

                viewModel.SettingTypes = settingTypesAvailable.Select(
                    settingType =>
                    new SelectListItem(
                        settingType.SettingName,
                        settingType.Id.ToString()));

                return View(viewModel);
            }

            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminSettingsHome);
        }

        /// <summary>
        /// Action for adding the value to the setting type.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The MVC View result.</returns>
        [Route(ViewYourFundingConstants.Route_AdminFundingStreamSettingAdd, Name = ViewYourFundingConstants.RouteName_AdminFundingStreamSettingAdd)]
        public async Task<IActionResult> Add(SetSettingTypeViewModel model)
        {
            var fundingStream = await _adminSettingsService.GetFundingStreamById(model.FundingStreamId, Repositories.Enums.FetchData.SettingValues_Setting);
            if (fundingStream != null)
            {
                var settingTypeModel = await _settingTypeService.GetSettingTypeById(model.SettingTypeId);
                var viewModel = await SetUpFundingStreamSettingActionViewModel(settingTypeModel, fundingStream.Id);

                return View("Action", viewModel);
            }

            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminSettingsHome);
        }

        #endregion


        #region Private Helpers

        private static void UpdateAreYouSureViewMode(FundingStreamSettingAreYouSureViewModel viewModel, SettingValue settingValue, string newValue)
        {
            viewModel.CurrentValue = settingValue.Value;
            viewModel.FundingStreamId = settingValue.FundingStreamId;
            viewModel.NewValue = newValue;
            viewModel.SettingId = settingValue.SettingId;
            viewModel.SettingName = settingValue.Setting.SettingName;
            viewModel.FundingStreamName = settingValue.FundingStream.FundingStreamName;
            viewModel.SettingDescription = settingValue.Setting.SettingDescription;
            viewModel.SettingValueId = settingValue.Id;
        }

        private static IReadOnlyList<PublicationViewModel> GetPublications(
            ICollection<Publication> publications,
            IReadOnlyDictionary<Publication, PublicationSpreadsheetMetaData> metadataDictionary)
        {
            return publications.Select(publication => new PublicationViewModel
            {
                Id = publication.Id,
                FundingStreamId = publication.FundingStream.Id,
                PublishedDateDay = publication.PublishedDate.Day.ToString(),
                PublishedDateMonth = publication.PublishedDate.Month.ToString(),
                PublishedDateYear = publication.PublishedDate.Year.ToString(),
                CutOffDateDay = publication.CutOffDate?.Day.ToString(),
                CutOffDateMonth = publication.CutOffDate?.Month.ToString(),
                CutOffDateYear = publication.CutOffDate?.Year.ToString(),
                FundingPeriodCode = publication.FundingPeriodCode,
                Description = publication.Description,
                SpreadsheetModelVersion = publication.SpreadsheetModelVersion,
                UIModelVersion = publication.UIModelVersion,
                Status = publication.Status,
                NewestSpreadsheetCreatedDate = metadataDictionary[publication].CreatedDateTime
            }).OrderBy(pub => pub.PublishedDate).ToList();
        }

        private async Task<Dictionary<Publication, PublicationSpreadsheetMetaData>> GetSpreadsheetMetadata(
            ICollection<Publication> vyfPublications)
        {
            var metadataDictionary = new Dictionary<Publication, PublicationSpreadsheetMetaData>();

            foreach (var publication in vyfPublications)
            {
                metadataDictionary.Add(publication, await _publicationSpreadsheetMetaService.GetPublicationSpreadsheetMetaDataAsync(publication));
            }

            return metadataDictionary;
        }

        private async Task<IReadOnlyList<PublicationViewModel>> GetPublicationsWithMetaData(
            ICollection<Publication> servicePublications)
        {
            var publicationMetaData = await GetSpreadsheetMetadata(servicePublications);
            return GetPublications(servicePublications, publicationMetaData).ToList();
        }


        private async Task<FundingStreamSettingAreYouSureViewModel> GetFundingStreamSettingAreYouSureViewModel(
            FundingStreamSettingActionViewModel model)
        {
            var viewModel = await GetBasePageViewModel<FundingStreamSettingAreYouSureViewModel>();
            viewModel.FundingStreamId = model.FundingStreamId;
            viewModel.NewValue = model.NewSettingValue;
            viewModel.SettingId = model.SettingId;
            viewModel.SettingName = model.SettingName;
            viewModel.FundingStreamName = model.FundingStreamName;
            viewModel.SettingDescription = model.SettingDescription;
            viewModel.ActionMode = model.ActionMode;
            viewModel.SettingValueId = model.SettingValueId;
            viewModel.CurrentValue = model.CurrentValue;
            return viewModel;
        }

        private async Task SetupFundingStreamSettingActionViewModel(
            int fundingStreamId,
            FundingStreamSettingActionViewModel viewModel,
            SettingValue fundingStreamSetting)
        {
            viewModel.SettingValueDataType = _mapper.Map<SettingValueDataType>(fundingStreamSetting.Setting.ValueDataType);
            viewModel.FundingStreamId = fundingStreamId;
            viewModel.FundingStreamName = fundingStreamSetting.FundingStream.FundingStreamName;
            viewModel.CurrentValue = fundingStreamSetting.Value;
            viewModel.SettingName = fundingStreamSetting.Setting.SettingName;
            viewModel.SettingDescription = fundingStreamSetting.Setting.SettingDescription;
            viewModel.SettingValueId = fundingStreamSetting.Id;

            if (_nationalLayoutSettingValueDataTypes.Contains(viewModel.SettingValueDataType))
            {
                await SetupNationalLayoutViewModelProperties(viewModel);
                viewModel.CurrentValue = await GetLayoutFriendlyName(viewModel.CurrentValue);
            }
        }


        private async Task<FundingStreamSettingActionViewModel> SetUpFundingStreamSettingActionViewModel(SettingType settingTypeModel, int fundingStreamId)
        {
            var viewModel = await GetBasePageViewModel<FundingStreamSettingActionViewModel>();
            viewModel.FundingStreamId = fundingStreamId;
            viewModel.SettingName = settingTypeModel.SettingName;
            viewModel.SettingDescription = settingTypeModel.SettingDescription;
            viewModel.SettingId = settingTypeModel.Id;
            viewModel.SettingValueDataType = _mapper.Map<SettingValueDataType>(settingTypeModel.ValueDataType);

            if (_nationalLayoutSettingValueDataTypes.Contains(viewModel.SettingValueDataType))
            {
                await SetupNationalLayoutViewModelProperties(viewModel);
            }

            return viewModel;
        }

        private async Task SetupNationalLayoutViewModelProperties(FundingStreamSettingActionViewModel viewModel)
        {
            var filters = new List<Expression<Func<LayoutModel, bool>>>
            {
                layout =>
                    layout.FundingStreamId == viewModel.FundingStreamId &&
                    (layout.DeletedDateTime == null || !layout.DeletedDateTime.IsDefined()) &&
                    layout.FundingViewScope == Services.Enums.FundingViewScope.National.ToString()
            };

            var layouts = await _layoutManagementService.GetAllLayoutsAsync(filters);

            viewModel.LayoutUiModels = layouts.Select(LayoutHelper.MapLayoutUiModel).ToList();
        }

        private async Task SetupAreYouSureNationalLayoutViewModelProperties(
            FundingStreamSettingAreYouSureViewModel viewModel)
        {
            viewModel.IsNationalLayoutSetting = true;
            viewModel.NationalLayoutFriendlyName = await GetLayoutFriendlyName(viewModel.NewValue);
        }

        private async Task<string> GetLayoutFriendlyName(string layoutId)
        {
            if (layoutId == null)
            {
                return string.Empty;
            }

            var layout = await _layoutManagementService.GetLayoutAsync(layoutId);
            return layout?.LayoutName;
        }

        private async Task<IReadOnlyList<FundingStreamSettingViewModel>> GetFundingStreamSettings(List<SettingValue> settingValues)
        {
            var fundingStreamSettings = new List<FundingStreamSettingViewModel>();
            foreach (var settingValue in settingValues)
            {
                var fundingStreamSetting = new FundingStreamSettingViewModel
                {
                    Name = settingValue.Setting.SettingName,
                    Description = settingValue.Setting.SettingDescription,
                    SettingValueId = settingValue.Id,
                    Value = settingValue.Value
                };

                if (_nationalLayoutServiceSettingValueDataTypes.Contains(settingValue.Setting.ValueDataType))
                {
                    fundingStreamSetting.Value = await GetLayoutFriendlyName(fundingStreamSetting.Value);
                }

                fundingStreamSettings.Add(fundingStreamSetting);
            }

            return fundingStreamSettings;
        }

        private void ClearModelErrors(FundingStreamSettingActionViewModel model)
        {
            if (model.SettingValueDataType != SettingValueDataType.String)
            {
                ModelState.Remove(nameof(FundingStreamSettingActionViewModel.NewStringValue));
            }

            if (model.SettingValueDataType != SettingValueDataType.NationalLayout)
            {
                ModelState.Remove(nameof(FundingStreamSettingActionViewModel.NewNationalLayoutIdValue));
            }

            if (model.SettingValueDataType != SettingValueDataType.NationalSpreadsheetLayout)
            {
                ModelState.Remove(nameof(FundingStreamSettingActionViewModel.NewNationalSpreadsheetLayoutIdValue));
            }
        }

        private async Task SetAddNewSettingFlag(Services.Models.FundingStream fundingStream, FundingStreamSettingsViewModel viewModel)
        {
            var settingTypesAvailable = await _settingTypeService.GetAvailableSettingTypes(
                fundingStream.SettingValues.Select(
                    settingValue => settingValue.SettingId));
            viewModel.CanAddNewSettings = settingTypesAvailable.Any();
        }

        #endregion
    }
}