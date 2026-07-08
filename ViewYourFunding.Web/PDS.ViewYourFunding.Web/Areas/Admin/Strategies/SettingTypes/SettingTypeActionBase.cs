using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType;
using System.Threading.Tasks;
using Service = PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.SettingTypes
{
    /// <summary>
    /// The setting type action base class.
    /// </summary>
    public abstract class SettingTypeActionBase
    {
        /// <summary>
        /// The setting type service.
        /// </summary>
        private readonly ISettingTypeService _settingTypesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingTypeActionBase"/> class.
        /// </summary>
        /// <param name="settingTypesService">The view your funding setting  service.</param>
        protected SettingTypeActionBase(ISettingTypeService settingTypesService)
        {
            _settingTypesService = settingTypesService;
        }

        /// <summary>
        /// Gets the setting type view model.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <param name="settingType">Setting type.</param>
        /// <returns>the SettingTypeViewModel.</returns>
        public static SettingTypeViewModel GetSettingTypeViewModel(
            int id,
            ActionMode actionMode,
            Service.SettingType settingType)
        {
            var viewModel = new SettingTypeViewModel
            {
                SettingType = new SettingType
                {
                    Id = id,
                    SettingDescription = settingType.SettingDescription,
                    SettingName = settingType.SettingName,
                    ValueDataType = (SettingValueDataType)settingType.ValueDataType,
                    ValuesAreEditable = settingType.ValuesAreEditable,
                },
                ActionMode = actionMode,
            };
            return viewModel;
        }

        /// <summary>
        /// Gets the SettingType by identifier.
        /// </summary>
        /// <param name="id">The SettingType identifier.</param>
        /// <returns>The SettingType.</returns>
        public async Task<Service.SettingType> GetSettingTypeById(int id)
        {
            return await _settingTypesService.GetSettingTypeById(id);
        }
    }
}