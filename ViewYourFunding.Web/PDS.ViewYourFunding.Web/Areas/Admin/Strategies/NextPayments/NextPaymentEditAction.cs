using AutoMapper;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments
{
    /// <summary>
    /// The next payment edit action.
    /// </summary>
    /// <seealso cref="NextPaymentActionBase" />
    /// <seealso cref="INextPaymentAction" />
    public class NextPaymentEditAction : NextPaymentActionBase, INextPaymentAction
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The view your funding next payment service.
        /// </summary>
        private readonly INextPaymentService _viewYourFundingNextPaymentService;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILoggerAdapter<NextPaymentActionBase> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPaymentEditAction"/> class.
        /// </summary>
        /// <param name="viewYourFundingNextPaymentTypeService">The view your funding next payment type service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="viewYourFundingNextPaymentService">The view your funding next payment service.</param>
        /// <param name="logger">The logger.</param>
        public NextPaymentEditAction(
            INextPaymentTypeService viewYourFundingNextPaymentTypeService,
            IMapper mapper,
            INextPaymentService viewYourFundingNextPaymentService,
            ILoggerAdapter<NextPaymentActionBase> logger)
        : base(
            viewYourFundingNextPaymentTypeService,
            viewYourFundingNextPaymentService)
        {
            _mapper = mapper;
            _viewYourFundingNextPaymentService = viewYourFundingNextPaymentService;
            _logger = logger;
        }

        /// <summary>
        /// Actions the specified funding stream identifier.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>The ViewYourFundingNextPaymentViewModel.</returns>
        public async Task<NextPaymentViewModel> Action(
            int fundingStreamId,
            int id)
        {
            var nextPayment = await GetNextPaymentById(fundingStreamId, id);
            if (nextPayment == null)
            {
                return null;
            }

            return await GetViewYourFundingNextPaymentViewModel(fundingStreamId, id, ActionMode, nextPayment);
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="nextPaymentViewModel">The view your funding next payment view model.</param>
        /// <returns>True if the changes are saved successfully.</returns>
        public async Task<bool> SaveChanges(NextPaymentViewModel nextPaymentViewModel)
        {
            var nextPayment = await GetNextPaymentById(
                nextPaymentViewModel.FundingStreamId,
                nextPaymentViewModel.NextPayment.Id);

            var updatedNextPayment = _mapper.Map<Services.Models.NextPayment>(nextPaymentViewModel.NextPayment);
            updatedNextPayment.NextPaymentType = null;
            if (nextPayment == null)
            {
                return false;
            }

            try
            {
                return await _viewYourFundingNextPaymentService.UpdateNextPayment(updatedNextPayment);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Checks if it Applies to.
        /// </summary>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>
        /// true if it applies to the action mode.
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