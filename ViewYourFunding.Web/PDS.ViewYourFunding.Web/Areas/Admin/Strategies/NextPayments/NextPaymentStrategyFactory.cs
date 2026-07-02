using AutoMapper;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Interfaces;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments
{
    /// <summary>
    /// The Next Payment Strategy Factory.
    /// </summary>
    public static class NextPaymentStrategyFactory
    {
        /// <summary>
        /// Gets the next payment action strategy.
        /// </summary>
        /// <param name="adminSettingsService">The view your funding admin settings.</param>
        /// <param name="viewYourFundingNextPaymentService">The view your funding next payment service.</param>
        /// <param name="viewYourFundingNextPaymentTypeService">The view your funding next payment type service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger service.</param>
        /// <returns>The NextPaymentActionStrategy.</returns>
        public static NextPaymentActionStrategy GetNextPaymentActionStrategy(
            IAdminSettingsService adminSettingsService,
            INextPaymentService viewYourFundingNextPaymentService,
            INextPaymentTypeService viewYourFundingNextPaymentTypeService,
            IMapper mapper,
            ILoggerAdapter<NextPaymentActionBase> logger)
        {
            return new NextPaymentActionStrategy
            {
                NextPaymentActions = new List<INextPaymentAction>
                {
                    new NextPaymentEditAction(
                        viewYourFundingNextPaymentTypeService,
                        mapper,
                        viewYourFundingNextPaymentService,
                        logger),
                    new NextPaymentAddAction(
                        viewYourFundingNextPaymentTypeService,
                        adminSettingsService,
                        viewYourFundingNextPaymentService,
                        mapper),
                    new NextPaymentDeleteAction(
                        viewYourFundingNextPaymentTypeService,
                        viewYourFundingNextPaymentService,
                        mapper)
                }
            };
        }

        /// <summary>
        /// Gets the next payment type action strategy.
        /// </summary>
        /// <param name="adminSettingsService">The view your funding admin settings.</param>
        /// <param name="viewYourFundingNextPaymentTypeService">The view your funding next payment type service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger service.</param>
        /// <returns>The NextPaymentActionStrategy.</returns>
        public static NextPaymentActionStrategy GetNextPaymentTypeActionStrategy(
            IAdminSettingsService adminSettingsService,
            INextPaymentTypeService viewYourFundingNextPaymentTypeService,
            IMapper mapper,
            ILoggerAdapter<NextPaymentTypeActionBase> logger)
        {
            return new NextPaymentActionStrategy
            {
                NextPaymentTypeActions = new List<INextPaymentTypeAction>
                {
                    new NextPaymentTypeEditAction(
                        viewYourFundingNextPaymentTypeService,
                        mapper,
                        logger),
                    new NextPaymentTypeAddAction(
                        adminSettingsService,
                        viewYourFundingNextPaymentTypeService,
                        mapper),
                    new NextPaymentTypeDeleteAction(
                        viewYourFundingNextPaymentTypeService,
                        mapper)
                }
            };
        }
    }
}