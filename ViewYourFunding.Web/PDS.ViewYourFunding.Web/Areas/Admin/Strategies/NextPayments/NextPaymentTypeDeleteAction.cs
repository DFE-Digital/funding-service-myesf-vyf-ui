using MapsterMapper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPaymentType;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments
{
    /// <summary>
    /// The next payment delete action class.
    /// </summary>
    /// <seealso cref="NextPaymentTypeActionBase" />
    /// <seealso cref="INextPaymentTypeAction" />
    public class NextPaymentTypeDeleteAction : NextPaymentTypeActionBase, INextPaymentTypeAction
    {
        /// <summary>
        /// The view your funding next payment type service.
        /// </summary>
        private readonly INextPaymentTypeService _viewYourFundingNextPaymentTypeService;

        /// <summary>
        /// Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPaymentTypeDeleteAction"/> class.
        /// </summary>
        /// <param name="viewYourFundingNextPaymentTypeService">The view your funding next payment type service.</param>
        /// <param name="mapper">The Mapper.</param>
        public NextPaymentTypeDeleteAction(INextPaymentTypeService viewYourFundingNextPaymentTypeService, IMapper mapper)
        : base(viewYourFundingNextPaymentTypeService)
        {
            _viewYourFundingNextPaymentTypeService = viewYourFundingNextPaymentTypeService;
            _mapper = mapper;
        }

        /// <summary>
        /// Actions the specified funding stream identifier and next payment type id.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="nextPaymentTypeId">The next payment type identifier.</param>
        /// <returns>
        /// The NextPaymentTypeViewModel.
        /// </returns>
        public async Task<NextPaymentTypeViewModel> Action(
            int fundingStreamId,
            int nextPaymentTypeId)
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

            if (nextPaymentType != null)
            {
                var updatedNextPaymentType = _mapper.Map<Services.Models.NextPaymentType>(nextPaymentTypeViewModel.NextPaymentType);
                return await _viewYourFundingNextPaymentTypeService.DeleteNextPaymentType(updatedNextPaymentType);
            }

            return false;
        }

        /// <summary>
        /// Checks if the action mode Applies to the class.
        /// </summary>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>
        /// True if the action mode applies to the action.
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
        public ActionMode ActionMode => ActionMode.Delete;
    }
}