using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPaymentType;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments
{
    /// <summary>
    /// The next payment type action base class.
    /// </summary>
    public abstract class NextPaymentTypeActionBase
    {
        /// <summary>
        /// The view your funding next payment type service.
        /// </summary>
        private readonly INextPaymentTypeService _viewYourFundingNextPaymentTypeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPaymentTypeActionBase"/> class.
        /// </summary>
        /// <param name="viewYourFundingNextPaymentTypeService">The view your funding next payment type service.</param>
        protected NextPaymentTypeActionBase(INextPaymentTypeService viewYourFundingNextPaymentTypeService)
        {
            _viewYourFundingNextPaymentTypeService = viewYourFundingNextPaymentTypeService;
        }

        /// <summary>
        /// Gets the view your funding next payment type view model.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <param name="nextPaymentType">Type of the next payment.</param>
        /// <returns>the NextPaymentTypeViewModel.</returns>
        public static NextPaymentTypeViewModel GetNextPaymentTypeViewModel(
            int fundingStreamId,
            int id,
            ActionMode actionMode,
            Services.Models.NextPaymentType nextPaymentType)
        {
            var viewModel = new NextPaymentTypeViewModel
            {
                NextPaymentType = new NextPaymentType
                {
                    Id = id,
                    FundingStreamId = fundingStreamId,
                    TypeCode = nextPaymentType.TypeCode,
                    Description = nextPaymentType.Description
                },
                ActionMode = actionMode,
                FundingStreamId = fundingStreamId,
                FundingStreamName = nextPaymentType.FundingStream.FundingStreamName
            };
            return viewModel;
        }

        /// <summary>
        /// Gets the funding next payment type by identifier.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="nextPaymentTypeId">The next payment type identifier.</param>
        /// <returns>The ViewYourFunding_NextPaymentType.</returns>
        public async Task<Services.Models.NextPaymentType> GetFundingNextPaymentTypeById(
            int fundingStreamId,
            int nextPaymentTypeId)
        {
            var nextPaymentTypes = await _viewYourFundingNextPaymentTypeService.GetNextPaymentTypes(fundingStreamId);

            return nextPaymentTypes.FirstOrDefault(x => x.Id == nextPaymentTypeId);
        }
    }
}