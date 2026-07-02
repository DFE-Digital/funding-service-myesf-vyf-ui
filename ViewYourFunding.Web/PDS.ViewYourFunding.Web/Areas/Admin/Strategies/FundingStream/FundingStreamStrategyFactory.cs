using AutoMapper;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStream
{
    /// <summary>
    /// The funding Stream Strategy Factory.
    /// </summary>
    public static class FundingStreamStrategyFactory
    {
        /// <summary>
        /// Gets the publication action strategy.
        /// </summary>
        /// <param name="adminSettingsService">The view your funding admin settings service.</param>
        /// <param name="fundingStreamService">The view your funding settings service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>The PublicationActionStrategy.</returns>
        public static FundingStreamActionStrategy GetFundingStreamActionStrategy(
                IAdminSettingsService adminSettingsService,
                IFundingStreamService fundingStreamService,
                IMapper mapper,
                ILoggerAdapter<FundingStreamActionBase> logger)
        {
            return new FundingStreamActionStrategy
            {
                FundingStreamActions = new List<IFundingStreamAction>
                {
                    new FundingStreamEditAction(
                        adminSettingsService,
                        fundingStreamService,
                        mapper,
                        logger),
                    new FundingStreamAddAction(
                        fundingStreamService,
                        adminSettingsService,
                        mapper),
                    new FundingStreamDeleteAction(
                        fundingStreamService,
                        adminSettingsService,
                        mapper)
                }
            };
        }
    }
}