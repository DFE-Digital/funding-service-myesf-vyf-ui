using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Rules
{
    /// <summary>
    /// Digital Allocation tile setting must be enabled rule.
    /// </summary>
    /// <seealso cref="IRule" />
    public class DigitalAllocationTileSettingMustBeEnabledRule : IRule
    {
        private readonly IDynamicSettingsService _dynamicSettingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalAllocationTileSettingMustBeEnabledRule"/> class.
        /// </summary>
        /// <param name="dynamicSettingsService">The dynamic settings service.</param>
        public DigitalAllocationTileSettingMustBeEnabledRule(IDynamicSettingsService dynamicSettingsService)
        {
            _dynamicSettingsService = dynamicSettingsService;
        }

        /// <inheritdoc/>
        public bool IsSatisfied(IUserContext providerContext = null)
        {
            return DigitalAllocationTileSettingIsEnabled();
        }

        /// <summary>
        /// Determines if the digital allocation tile setting is enabled.
        /// </summary>
        /// <returns>Boolean true or false.</returns>
        private bool DigitalAllocationTileSettingIsEnabled()
        {
            return false;
        }
    }
}