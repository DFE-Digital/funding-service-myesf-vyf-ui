using AutoMapper;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Interfaces;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.SettingTypes
{
    /// <summary>
    /// The setting type strategy factory.
    /// </summary>
    public static class SettingTypeStrategyFactory
    {
        /// <summary>
        /// Gets the setting type action strategy.
        /// </summary>
        /// <param name="adminSettingsService">The view your funding admin settings.</param>
        /// <param name="settingTypeService">The view your funding setting type service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger service.</param>
        /// <returns>The SettingTypeActionStrategy.</returns>
        public static SettingTypeActionStrategy GetSettingTypeTypeActionStrategy(
            IAdminSettingsService adminSettingsService,
            ISettingTypeService settingTypeService,
            IMapper mapper,
            ILoggerAdapter<SettingTypeActionBase> logger)
        {
            var strategy = new SettingTypeActionStrategy
            {
                SettingTypeActions = new List<ISettingTypeAction>
                {
                    new SettingTypeEditAction(
                        settingTypeService,
                        mapper,
                        logger),
                    new SettingTypeAddAction(
                        adminSettingsService,
                        settingTypeService,
                        mapper),
                    new SettingTypeDeleteAction(
                        adminSettingsService,
                        settingTypeService,
                        mapper)
                }
            };

            return strategy;
        }
    }
}