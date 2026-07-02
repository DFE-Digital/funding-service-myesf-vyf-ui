using AutoMapper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.SettingTypes
{
    /// <summary>
    /// The setting type delete action class.
    /// </summary>
    /// <seealso cref="SettingTypeActionBase" />
    /// <seealso cref="ISettingTypeAction" />
    public class SettingTypeDeleteAction : SettingTypeActionBase, ISettingTypeAction
    {
        /// <summary>
        /// The view your funding setting type service.
        /// </summary>
        private readonly ISettingTypeService _settingTypesService;

        /// <summary>
        /// Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingTypeDeleteAction"/> class.
        /// </summary>
        /// <param name="adminSettingsService">The admin setting service.</param>
        /// <param name="settingTypesService">Setting type service.</param>
        /// <param name="mapper">The Mapper.</param>
        public SettingTypeDeleteAction(IAdminSettingsService adminSettingsService, ISettingTypeService settingTypesService, IMapper mapper)
        : base(settingTypesService)
        {
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
            var settingType = await GetSettingTypeById(settingTypeId);

            if (settingType == null)
            {
                return null;
            }

            return GetSettingTypeViewModel(settingTypeId, ActionMode, settingType);
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
            // Check if the SettingValue to delete exists in the database before deleting (unlikely but another admin user could potentially already have deleted it)
            var dbSettingTypeExists = await GetSettingTypeById(settingTypeViewModel.SettingType.Id);
            if (dbSettingTypeExists != null)
            {
                var settingType = _mapper.Map<Services.Models.SettingType>(settingTypeViewModel.SettingType);
                return await _settingTypesService.DeleteSettingType(settingType);
            }

            return false;
        }

        /// <summary>
        /// Checks if the action mode Applies to the class.
        /// </summary>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>
        /// True if the action mode applies to the action.
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
        public ActionMode ActionMode => ActionMode.Delete;
    }
}