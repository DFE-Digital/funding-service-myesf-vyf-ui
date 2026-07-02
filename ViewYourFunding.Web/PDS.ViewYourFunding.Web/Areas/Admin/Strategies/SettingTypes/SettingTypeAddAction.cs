using AutoMapper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.SettingTypes
{
    /// <summary>
    /// The setting type action class.
    /// </summary>
    /// <seealso cref="SettingTypeActionBase" />
    /// <seealso cref="ISettingTypeAction" />
    public class SettingTypeAddAction : SettingTypeActionBase, ISettingTypeAction
    {
        /// <summary>
        /// The view your funding settings service.
        /// </summary>
        private readonly IAdminSettingsService _adminSettingsService;

        /// <summary>
        /// The view your funding setting type service.
        /// </summary>
        private readonly ISettingTypeService _settingTypesService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingTypeAddAction"/> class.
        /// </summary>
        /// <param name="adminSettingsService">The view your funding admin settings.</param>
        /// <param name="settingTypesService">The view your funding setting type service.</param>
        /// <param name="mapper">The mapper.</param>
        public SettingTypeAddAction(
            IAdminSettingsService adminSettingsService,
            ISettingTypeService settingTypesService,
            IMapper mapper)
        : base(settingTypesService)
        {
            _adminSettingsService = adminSettingsService;
            _settingTypesService = settingTypesService;
            _mapper = mapper;
        }

        /// <summary>
        /// Actions the specified setting type id.
        /// </summary>
        /// <param name="settingTypeId">The setting type identifier.</param>
        /// <returns>
        /// The SettingTypeViewModel.
        /// </returns>
        public async Task<SettingTypeViewModel> Action(int settingTypeId)
        {
            var viewModel = new SettingTypeViewModel
            {
                ActionMode = ActionMode.Add,
                SettingType = new SettingType { Id = settingTypeId }
            };

            return await Task.FromResult(viewModel);
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="settingTypeViewModel">The view your funding setting type view model.</param>
        /// <returns>
        /// True if the changes are saved successfully.
        /// </returns>
        public async Task<bool> SaveChanges(SettingTypeViewModel settingTypeViewModel)
        {
            var settingType = _mapper.Map<Services.Models.SettingType>(settingTypeViewModel.SettingType);
            var result = await _settingTypesService.CreateSettingType(settingType);

            if (result != null)
            {
                settingTypeViewModel.SettingType.Id = result.Id;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the action mode Applies to the class.
        /// </summary>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>
        /// True if the action mode applies to to the action.
        /// </returns>
        public bool AppliesTo(ActionMode actionMode)
        {
            return actionMode == ActionMode;
        }

        /// <summary>
        /// Gets the action mode.
        /// </summary>
        /// <value>
        /// The action mode.
        /// </value>
        public ActionMode ActionMode => ActionMode.Add;
    }
}