using PDS.ViewYourFunding.Services.Interfaces;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStreamSettings
{
    /// <summary>
    /// The funding stream setting Strategy Factory.
    /// </summary>
    public static class FundingStreamSettingStrategyFactory
    {
        /// <summary>
        /// Gets the funding stream setting action strategy.
        /// </summary>
        /// <param name="adminFundingStreamSettingService">The admin funding stream setting service.</param>
        /// <returns>The funding stream setting actions.</returns>
        public static FundingStreamSettingActionStrategy GetFundingStreamSettingActionStrategy(
            IAdminFundingStreamSettingService adminFundingStreamSettingService)
        {
            return new FundingStreamSettingActionStrategy
            {
                FundingStreamSettingActions = new List<IFundingStreamSettingAction>
                {
                    new FundingStreamSettingAddAction(adminFundingStreamSettingService),
                    new FundingStreamSettingDeleteAction(adminFundingStreamSettingService),
                    new FundingStreamSettingEditAction(adminFundingStreamSettingService)
                }
            };
        }
    }
}