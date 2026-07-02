using Microsoft.AspNetCore.Mvc.Rendering;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments
{
    /// <summary>
    /// The next payment action base class.
    /// </summary>
    public abstract class NextPaymentActionBase
    {
        /// <summary>
        /// The view your funding next payment type service.
        /// </summary>
        private readonly INextPaymentTypeService _viewYourFundingNextPaymentTypeService;

        /// <summary>
        /// The view your funding next payment service.
        /// </summary>
        private readonly INextPaymentService _viewYourFundingNextPaymentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPaymentActionBase"/> class.
        /// </summary>
        /// <param name="viewYourFundingNextPaymentTypeService">The view your funding next payment type service.</param>
        /// <param name="viewYourFundingNextPaymentService">The view your funding next payment service.</param>
        protected NextPaymentActionBase(
            INextPaymentTypeService viewYourFundingNextPaymentTypeService,
            INextPaymentService viewYourFundingNextPaymentService)
        {
            _viewYourFundingNextPaymentTypeService = viewYourFundingNextPaymentTypeService;
            _viewYourFundingNextPaymentService = viewYourFundingNextPaymentService;
        }

        /// <summary>
        /// Gets the view your funding next payment view model.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <param name="nextPayment">The next payment.</param>
        /// <returns>
        /// The ViewYourFundingNextPaymentViewModel.
        /// </returns>
        public async Task<NextPaymentViewModel> GetViewYourFundingNextPaymentViewModel(
            int fundingStreamId,
            int id,
            ActionMode actionMode,
            Services.Models.NextPayment nextPayment)
        {
            var viewModel = new NextPaymentViewModel
            {
                NextPayment = new NextPayment
                {
                    Id = id,
                    FundingStreamId = fundingStreamId,
                    NextPaymentTypes = await GetNextPaymentTypes(),
                    NextPaymentDateDay = nextPayment.NextPaymentDate.Day.ToString(),
                    NextPaymentDateMonth = nextPayment.NextPaymentDate.Month.ToString(),
                    NextPaymentDateYear = nextPayment.NextPaymentDate.Year.ToString(),
                    NextPaymentTypeCode = nextPayment.NextPaymentType.TypeCode,
                    NextPaymentTypeDescription = nextPayment.NextPaymentType.Description,
                    FundingPeriodCode = nextPayment.FundingPeriodCode,
                    Active = nextPayment.Active,
                    NextPaymentTypeId = nextPayment.NextPaymentTypeId
                },
                ActionMode = actionMode,
                FundingStreamId = fundingStreamId,
                FundingStreamName = nextPayment.FundingStream.FundingStreamName
            };
            return viewModel;
        }

        /// <summary>
        /// Gets the next payment by identifier.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="nextPaymentId">The next payment identifier.</param>
        /// <returns>The ViewYourFunding_NextPayment.</returns>
        public async Task<Services.Models.NextPayment> GetNextPaymentById(int fundingStreamId, int nextPaymentId)
        {
            var nextPayments = await _viewYourFundingNextPaymentService.GetNextPayments(fundingStreamId);

            return nextPayments.FirstOrDefault(x => x.Id == nextPaymentId);
        }

        /// <summary>
        /// Gets the next payment types.
        /// </summary>
        /// <returns>
        /// The select list of Next Payment types.
        /// </returns>
        public async Task<IEnumerable<SelectListItem>> GetNextPaymentTypes()
        {
            var nextPaymentTypes = await _viewYourFundingNextPaymentTypeService.GetAllNextPaymentTypes();

            return nextPaymentTypes.Select(x => new SelectListItem { Text = $@"{x.TypeCode} - {x.Description}", Value = x.Id.ToString() });
        }
    }
}