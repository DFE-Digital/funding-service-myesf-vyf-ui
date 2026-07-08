using AutoMapper;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments
{
    /// <summary>
    /// The next payment add action class.
    /// </summary>
    /// <seealso cref="NextPaymentActionBase" />
    /// <seealso cref="INextPaymentAction" />
    public class NextPaymentAddAction : NextPaymentActionBase, INextPaymentAction
    {
        /// <summary>
        /// The view your funding next payment service.
        /// </summary>
        private readonly INextPaymentService _viewYourFundingNextPaymentService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The view your funding admin setting service.
        /// </summary>
        private readonly IAdminSettingsService _adminSettingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPaymentAddAction"/> class.
        /// </summary>
        /// <param name="viewYourFundingNextPaymentTypeService">The view your funding next payment type service.</param>
        /// <param name="adminSettingsService">The view your funding admin settings.</param>
        /// <param name="viewYourFundingNextPaymentService">The view your funding next payment service.</param>
        /// <param name="mapper">The mapper.</param>
        public NextPaymentAddAction(
            INextPaymentTypeService viewYourFundingNextPaymentTypeService,
            IAdminSettingsService adminSettingsService,
            INextPaymentService viewYourFundingNextPaymentService,
            IMapper mapper)
        : base(
            viewYourFundingNextPaymentTypeService,
            viewYourFundingNextPaymentService)
        {
            _adminSettingsService = adminSettingsService;
            _viewYourFundingNextPaymentService = viewYourFundingNextPaymentService;
            _mapper = mapper;
        }

        /// <summary>
        /// Actions the specified funding stream identifier and next payment id.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="nextPaymentId">The next payment identifier.</param>
        /// <returns>The ViewYourFundingNextPaymentViewModel.</returns>
        public async Task<NextPaymentViewModel> Action(
            int fundingStreamId,
            int nextPaymentId)
        {
            var fundingStream = await _adminSettingsService.GetFundingStreamById(fundingStreamId, FetchData.NextPayments_NextPaymentType, FetchData.NextPaymentTypes_NextPayments);

            if (fundingStream != null)
            {
                var viewModel = new NextPaymentViewModel
                {
                    NextPayment = new NextPayment
                    {
                        FundingStreamId = fundingStreamId,
                        NextPaymentTypes = await GetNextPaymentTypes()
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
        /// <param name="viewYourFundingNextPaymentViewModel">The view your funding next payment view model.</param>
        /// <returns>True if the changes are saved successfully.</returns>
        public async Task<bool> SaveChanges(NextPaymentViewModel viewYourFundingNextPaymentViewModel)
        {
            var newNextPayment = _mapper.Map<Services.Models.NextPayment>(viewYourFundingNextPaymentViewModel.NextPayment);
            newNextPayment.NextPaymentType = null;
            var result = await _viewYourFundingNextPaymentService.CreateNextPayment(newNextPayment);

            if (result != null)
            {
                viewYourFundingNextPaymentViewModel.NextPayment.Id = result.Id;
                return true;
            }

            return false;
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
        public ActionMode ActionMode => ActionMode.Add;
    }
}