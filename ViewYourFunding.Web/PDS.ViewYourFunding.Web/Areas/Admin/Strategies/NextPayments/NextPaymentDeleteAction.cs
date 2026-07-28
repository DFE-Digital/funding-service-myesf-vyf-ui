using MapsterMapper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments
{
    /// <summary>
    /// The Next Payment Delete Action class.
    /// </summary>
    /// <seealso cref="NextPaymentActionBase" />
    /// <seealso cref="INextPaymentAction" />
    public class NextPaymentDeleteAction : NextPaymentActionBase, INextPaymentAction
    {
        /// <summary>
        /// The view your funding next payment service.
        /// </summary>
        private readonly INextPaymentService _viewYourFundingNextPaymentService;

        /// <summary>
        /// Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPaymentDeleteAction"/> class.
        /// </summary>
        /// <param name="viewYourFundingNextPaymentTypeService">The view your funding next payment type service.</param>
        /// <param name="viewYourFundingNextPaymentService">The view your funding next payment service.</param>
        /// <param name="mapper">The mapper.</param>
        public NextPaymentDeleteAction(
            INextPaymentTypeService viewYourFundingNextPaymentTypeService,
            INextPaymentService viewYourFundingNextPaymentService,
            IMapper mapper)
        : base(
            viewYourFundingNextPaymentTypeService,
            viewYourFundingNextPaymentService)
        {
            _viewYourFundingNextPaymentService = viewYourFundingNextPaymentService;
            _mapper = mapper;
        }

        /// <summary>
        /// Actions the specified funding stream identifier.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="nextPaymentId">The next payment identifier.</param>
        /// <returns>Next payment view model.</returns>
        public async Task<NextPaymentViewModel> Action(
            int fundingStreamId,
            int nextPaymentId)
        {
            var nextPayment = await GetNextPaymentById(
                fundingStreamId,
                nextPaymentId);

            if (nextPayment == null)
            {
                return null;
            }

            return await GetViewYourFundingNextPaymentViewModel(fundingStreamId, nextPaymentId, ActionMode, nextPayment);
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="nextPaymentViewModel">The view your funding next payment view model.</param>
        /// <returns>Returns true, if saved successfully.</returns>
        public async Task<bool> SaveChanges(NextPaymentViewModel nextPaymentViewModel)
        {
            var nextPayment = await GetNextPaymentById(
                nextPaymentViewModel.FundingStreamId,
                nextPaymentViewModel.NextPayment.Id);

            if (nextPayment != null)
            {
                var updatedNextPayment = _mapper.Map<Services.Models.NextPayment>(nextPaymentViewModel.NextPayment);
                updatedNextPayment.NextPaymentType = null;

                return await _viewYourFundingNextPaymentService.DeleteNextPayment(updatedNextPayment);
            }

            return false;
        }

        /// <summary>
        /// Checks if the action mode applies to this class instance.
        /// </summary>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>True if it applies to this class instance.</returns>
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
        public ActionMode ActionMode => ActionMode.Delete;
    }
}