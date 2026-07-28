using MapsterMapper;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPaymentType;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments
{
    /// <summary>
    /// The next payment type action class.
    /// </summary>
    /// <seealso cref="NextPaymentTypeActionBase" />
    /// <seealso cref="INextPaymentTypeAction" />
    public class NextPaymentTypeAddAction : NextPaymentTypeActionBase, INextPaymentTypeAction
    {
        /// <summary>
        /// The view your funding settings service.
        /// </summary>
        private readonly IAdminSettingsService _adminSettingsService;

        /// <summary>
        /// The view your funding next payment type service.
        /// </summary>
        private readonly INextPaymentTypeService _viewYourFundingNextPaymentTypeService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPaymentTypeAddAction"/> class.
        /// </summary>
        /// <param name="adminSettingsService">The view your funding admin settings.</param>
        /// <param name="viewYourFundingNextPaymentTypeService">The view your funding next payment type service.</param>
        /// <param name="mapper">The mapper.</param>
        public NextPaymentTypeAddAction(
            IAdminSettingsService adminSettingsService,
            INextPaymentTypeService viewYourFundingNextPaymentTypeService,
            IMapper mapper)
        : base(viewYourFundingNextPaymentTypeService)
        {
            _adminSettingsService = adminSettingsService;
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
            var fundingStream = await _adminSettingsService.GetFundingStreamById(fundingStreamId, FetchData.NextPayments_NextPaymentType, FetchData.NextPaymentTypes_NextPayments);

            if (fundingStream != null)
            {
                var viewModel = new NextPaymentTypeViewModel
                {
                    NextPaymentType = new NextPaymentType
                    {
                        FundingStreamId = fundingStreamId
                    },
                    ActionMode = ActionMode,
                    FundingStreamId = fundingStreamId,
                    FundingStreamName = fundingStream.FundingStreamName
                };

                return viewModel;
            }

            return null;
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
            var newNextPaymentType = _mapper.Map<Services.Models.NextPaymentType>(nextPaymentTypeViewModel.NextPaymentType);
            var result = await _viewYourFundingNextPaymentTypeService.CreateNextPaymentType(newNextPaymentType);

            if (result != null)
            {
                nextPaymentTypeViewModel.NextPaymentType.Id = result.Id;
                return true;
            }

            return false;
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
        public ActionMode ActionMode => ActionMode.Add;
    }
}