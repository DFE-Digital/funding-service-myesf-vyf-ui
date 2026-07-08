using AutoMapper;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.SettingTypes
{
    /// <summary>
    /// The setting type edit action class.
    /// </summary>
    /// <seealso cref="SettingTypeEditAction" />
    /// <seealso cref="ISettingTypeAction" />
    public class SettingTypeEditAction : SettingTypeActionBase, ISettingTypeAction
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The view your funding setting type service.
        /// </summary>
        private readonly ISettingTypeService _settingTypesService;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILoggerAdapter<SettingTypeActionBase> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingTypeEditAction"/> class.
        /// </summary>
        /// <param name="settingTypesService">The view your funding setting type service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public SettingTypeEditAction(
            ISettingTypeService settingTypesService,
            IMapper mapper,
            ILoggerAdapter<SettingTypeActionBase> logger)
        : base(settingTypesService)
        {
            _mapper = mapper;
            _settingTypesService = settingTypesService;
            _logger = logger;
        }

        /// <summary>
        /// Actions the specified funding stream identifier and setting type id.
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
            var settingType = await GetSettingTypeById(
                settingTypeViewModel.SettingType.Id);

            if (settingType == null)
            {
                return false;
            }

            var updatedSettingType = _mapper.Map<Services.Models.SettingType>(settingTypeViewModel.SettingType);

            return await _settingTypesService.UpdateSettingType(updatedSettingType);
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
        public ActionMode ActionMode => ActionMode.Edit;
    }
}