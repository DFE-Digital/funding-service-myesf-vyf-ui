using MapsterMapper;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStream;
using System;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStream
{
    /// <summary>
    /// The fundung stream edit action class.
    /// </summary>
    /// <seealso cref="FundingStreamActionBase" />
    /// <seealso cref="IFundingStreamAction" />
    public class FundingStreamEditAction : FundingStreamActionBase, IFundingStreamAction
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
        /// The logger.
        /// </summary>
        private readonly ILoggerAdapter<FundingStreamActionBase> _logger;

        /// <summary>
        /// The view your funding admin settings  service.
        /// </summary>
        private readonly IAdminSettingsService _adminSettingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingStreamEditAction"/> class.
        /// </summary>
        /// <param name="fundingStreamService">The funding stream service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="adminSettingsService">The admin setting service.</param>
        /// <param name="logger">The logger.</param>
        public FundingStreamEditAction(
           IAdminSettingsService adminSettingsService,
           IFundingStreamService fundingStreamService,
           IMapper mapper,
           ILoggerAdapter<FundingStreamActionBase> logger)
        : base(adminSettingsService, fundingStreamService)
        {
            _adminSettingsService = adminSettingsService;
            _fundingStreamService = fundingStreamService;
            _mapper = mapper;
            _logger = logger;
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

            return await GetViewYourFundingStreamViewModel(ActionMode, fundingStream);
        }

        /// <inheritdoc/>
        public async Task<bool> SaveChanges(FundingStreamViewModel fundingStreamViewModel)
        {
            var fundingStream = await GetFundingStreamById(
               fundingStreamViewModel.FundingStream.Id);

            if (fundingStream == null)
            {
                return false;
            }

            var updatedFundingStream = _mapper.Map<Services.Models.FundingStream>(fundingStreamViewModel.FundingStream);

            try
            {
                return await _fundingStreamService.UpdateFundingStream(updatedFundingStream);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                return false;
            }
        }

        /// <inheritdoc/>
        public bool AppliesTo(ActionMode actionMode)
        {
            return actionMode == ActionMode;
        }

        /// <inheritdoc/>
        public ActionMode ActionMode => ActionMode.Edit;
    }
}