using AutoMapper;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPaymentType;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments
{
    /// <summary>
    /// The next payment type edit action class.
    /// </summary>
    /// <seealso cref="NextPaymentTypeActionBase" />
    /// <seealso cref="INextPaymentTypeAction" />
    public class NextPaymentTypeEditAction : NextPaymentTypeActionBase, INextPaymentTypeAction
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The view your funding next payment type service.
        /// </summary>
        private readonly INextPaymentTypeService _viewYourFundingNextPaymentTypeService;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILoggerAdapter<NextPaymentTypeActionBase> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPaymentTypeEditAction"/> class.
        /// </summary>
        /// <param name="viewYourFundingNextPaymentTypeService">The view your funding next payment type service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public NextPaymentTypeEditAction(
            INextPaymentTypeService viewYourFundingNextPaymentTypeService,
            IMapper mapper,
            ILoggerAdapter<NextPaymentTypeActionBase> logger)
        : base(viewYourFundingNextPaymentTypeService)
        {
            _mapper = mapper;
            _viewYourFundingNextPaymentTypeService = viewYourFundingNextPaymentTypeService;
            _logger = logger;
        }

        /// <summary>
        /// Actions the specified funding stream identifier and next payment type id.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="nextPaymentTypeId">The next payment type identifier.</param>
        /// <returns>
        /// The NextPaymentTypeViewModel.
        /// </returns>
        public async Task<NextPaymentTypeViewModel> Action(int fundingStreamId, int nextPaymentTypeId)
        {
            var nextPaymentType = await GetFundingNextPaymentTypeById(
                fundingStreamId,
                nextPaymentTypeId);

            if (nextPaymentType == null)
            {
                return null;
            }

            return GetNextPaymentTypeViewModel(fundingStreamId, nextPaymentTypeId, ActionMode, nextPaymentType);
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="nextPaymentTypeViewModel">The view your funding next payment type view model.</param>
        /// <returns>
        /// True if the changes are saved successfully.
        /// </returns>
        public async Task<bool> SaveChanges(NextPaymentTypeViewModel nextPaymentTypeViewModel)
        {
            var nextPaymentType = await GetFundingNextPaymentTypeById(
                nextPaymentTypeViewModel.FundingStreamId,
                nextPaymentTypeViewModel.NextPaymentType.Id);

            if (nextPaymentType == null)
            {
                return false;
            }

            var updatedNextPaymentType = _mapper.Map<Services.Models.NextPaymentType>(nextPaymentTypeViewModel.NextPaymentType);

            try
            {
                return await _viewYourFundingNextPaymentTypeService.UpdateNextPaymentType(updatedNextPaymentType);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Checks if the action mode Applies to the class.
        /// </summary>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>
        /// True if the action mode applies to to the action.
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