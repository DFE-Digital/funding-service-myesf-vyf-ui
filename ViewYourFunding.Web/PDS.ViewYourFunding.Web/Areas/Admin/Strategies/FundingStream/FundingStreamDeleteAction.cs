using AutoMapper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStream;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStream
{
    /// <summary>
    /// The fundung stream delete action class.
    /// </summary>
    /// <seealso cref="FundingStreamActionBase" />
    /// <seealso cref="IFundingStreamAction" />
    public class FundingStreamDeleteAction : FundingStreamActionBase, IFundingStreamAction
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The view your funding admin setting service.
        /// </summary>
        private readonly IFundingStreamService _fundingStreamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingStreamDeleteAction"/> class.
        /// </summary>
        /// <param name="fundingStreamService">The funding stream service.</param>
        /// <param name="adminSettingsService">The admin setting service.</param>
        /// <param name="mapper">The mapper.</param>
        public FundingStreamDeleteAction(
            IFundingStreamService fundingStreamService,
            IAdminSettingsService adminSettingsService,
            IMapper mapper)
           : base(adminSettingsService, fundingStreamService)
        {
            _fundingStreamService = fundingStreamService;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<FundingStreamViewModel> Action(
            int fundingStreamId)
        {
            var fundingStream = await GetFundingStreamById(fundingStreamId);

            if (fundingStream == null)
            {
                return null;
            }

            return await GetViewYourFundingStreamViewModel(
                ActionMode,
                fundingStream);
        }

        /// <inheritdoc/>
        public async Task<bool> SaveChanges(FundingStreamViewModel fundingStreamViewModel)
        {
            var fundingStream = await GetFundingStreamById(fundingStreamViewModel.FundingStream.Id);

            var updatedFundingStream = _mapper.Map<Services.Models.FundingStream>(fundingStreamViewModel.FundingStream);

            if (fundingStream != null)
            {
                return await _fundingStreamService.DeleteFundingStream(updatedFundingStream);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool AppliesTo(ActionMode actionMode)
        {
            return actionMode == ActionMode;
        }

        /// <inheritdoc/>
        public ActionMode ActionMode => ActionMode.Delete;
    }
}