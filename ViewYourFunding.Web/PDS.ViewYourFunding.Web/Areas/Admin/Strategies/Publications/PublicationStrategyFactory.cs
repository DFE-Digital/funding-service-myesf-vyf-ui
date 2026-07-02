using AutoMapper;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Interfaces;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.Publications
{
    /// <summary>
    /// The Publication Strategy Factory.
    /// </summary>
    public static class PublicationStrategyFactory
    {
        /// <summary>
        /// Gets the publication action strategy.
        /// </summary>
        /// <param name="adminSettingsService">The view your funding settings service.</param>
        /// <param name="viewYourFundingPublicationService">The view your funding publication service.</param>
        /// <param name="fundingUiModelDetailsService">The funding UI Model Details service.</param>
        /// <param name="layoutManagementService">The layout management service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>The PublicationActionStrategy.</returns>
        public static PublicationActionStrategy GetPublicationActionStrategy(
            IAdminSettingsService adminSettingsService,
            IAdminPublicationService viewYourFundingPublicationService,
            IFundingUiModelDetailsService fundingUiModelDetailsService,
            ILayoutManagementService layoutManagementService,
            IMapper mapper,
            ILoggerAdapter<PublicationActionBase> logger)
        {
            return new PublicationActionStrategy
            {
                PublicationActions = new List<IPublicationAction>
                {
                    new PublicationEditAction(
                        viewYourFundingPublicationService,
                        fundingUiModelDetailsService,
                        layoutManagementService,
                        mapper,
                        logger),
                    new PublicationAddAction(
                        viewYourFundingPublicationService,
                        adminSettingsService,
                        fundingUiModelDetailsService,
                        layoutManagementService,
                        mapper),
                    new PublicationDeleteAction(
                        viewYourFundingPublicationService,
                        fundingUiModelDetailsService,
                        layoutManagementService,
                        mapper)
                }
            };
        }
    }
}