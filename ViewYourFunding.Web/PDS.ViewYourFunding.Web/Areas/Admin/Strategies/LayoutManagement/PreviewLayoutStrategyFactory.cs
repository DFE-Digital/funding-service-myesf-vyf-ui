using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStreamSettings;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.LayoutManagement
{
    /// <summary>
    /// The preview Layout Strategy Factory.
    /// </summary>
    public static class PreviewLayoutStrategyFactory
    {
        /// <summary>
        /// Gets the preview layout action strategy.
        /// </summary>
        /// <param name="adminSettingsService">The admin settings service.</param>
        /// <returns>The preview layout action strategy.</returns>
        public static PreviewLayoutActionStrategy GetFundingViewScopeActionStrategy(
            IAdminSettingsService adminSettingsService)
        {
            return new PreviewLayoutActionStrategy
            {
                PreviewLayoutActions = new List<IPreviewLayoutAction>
                {
                   new NationalLayoutAction(adminSettingsService),
                   new OrganisationLayoutAction(adminSettingsService),
                   new OrganisationHistoryLayoutAction(adminSettingsService),
                   new OrganisationHistorySingleYearLayoutAction(adminSettingsService),
                   new OrganisationSummaryLayoutAction(),
                   new ProviderHistoryLayoutAction(adminSettingsService),
                   new ProviderHistorySingleYearLayoutAction(adminSettingsService),
                   new ProviderSummaryLayoutAction(),
                   new ProviderLayoutAction(adminSettingsService)
                }
            };
        }
    }
}