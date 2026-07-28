using MapsterMapper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStream;
using System.Threading.Tasks;
using Model = PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStream;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStream
{
    /// <summary>
    /// The fundung stream add action class.
    /// </summary>
    /// <seealso cref="FundingStreamActionBase" />
    /// <seealso cref="IFundingStreamAction" />
    public class FundingStreamAddAction : FundingStreamActionBase, IFundingStreamAction
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
        /// The view your funding admin settings  service.
        /// </summary>
        private readonly IAdminSettingsService _adminSettingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingStreamAddAction"/> class.
        /// </summary>
        /// <param name="fundingStreamService">The view your funding stream service.</param>
        /// <param name="adminSettingsService">The admin setting service.</param>
        /// <param name="mapper">The mapper.</param>
        public FundingStreamAddAction(
            IFundingStreamService fundingStreamService,
            IAdminSettingsService adminSettingsService,
            IMapper mapper)
        : base(adminSettingsService, fundingStreamService)
        {
            _adminSettingsService = adminSettingsService;
            _fundingStreamService = fundingStreamService;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public Task<FundingStreamViewModel> Action(int fundingStreamId)
        {
            var viewModel = new FundingStreamViewModel
            {
                ActionMode = ActionMode.Add,
                FundingStream = new Model.FundingStream()
            };

            return Task.FromResult(viewModel);
        }

        /// <inheritdoc/>
        public async Task<bool> SaveChanges(FundingStreamViewModel fundingStreamViewModel)
        {
            var newFundingStream = _mapper.Map<Services.Models.FundingStream>(fundingStreamViewModel.FundingStream);

            var result = await _fundingStreamService.CreateFundingStream(newFundingStream);

            if (result != null)
            {
                fundingStreamViewModel.FundingStream.Id = result.Id;
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool AppliesTo(ActionMode actionMode)
        {
            return actionMode == ActionMode;
        }

        /// <inheritdoc/>
        public ActionMode ActionMode => ActionMode.Add;
    }
}