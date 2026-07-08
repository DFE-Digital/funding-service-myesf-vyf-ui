using AutoMapper;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.Publications
{
    /// <summary>
    /// The publication edit action.
    /// </summary>
    /// <seealso cref="PublicationActionBase" />
    /// <seealso cref="IPublicationAction" />
    public class PublicationEditAction : PublicationActionBase, IPublicationAction
    {
        /// <summary>
        /// The view your funding publication service.
        /// </summary>
        private readonly IAdminPublicationService _adminPublicationService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;


        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILoggerAdapter<PublicationActionBase> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationEditAction"/> class.
        /// </summary>
        /// <param name="adminPublicationService">The view your funding publication service.</param>
        /// <param name="fundingUiModelDetailsService">The funding UI Model details service.</param>
        /// <param name="layoutManagementService">The layout management service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public PublicationEditAction(
            IAdminPublicationService adminPublicationService,
            IFundingUiModelDetailsService fundingUiModelDetailsService,
            ILayoutManagementService layoutManagementService,
            IMapper mapper,
            ILoggerAdapter<PublicationActionBase> logger)
        : base(
            adminPublicationService,
            fundingUiModelDetailsService,
            layoutManagementService)
        {
            _adminPublicationService = adminPublicationService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<PublicationActionViewModel> Action(
            int fundingStreamId,
            int publicationId)
        {
            var publication = await GetPublicationById(fundingStreamId, publicationId);

            return await GetViewYourFundingPublicationViewModel(fundingStreamId, publicationId, ActionMode, publication, true);
        }

        /// <inheritdoc/>
        public async Task<bool> SaveChanges(PublicationActionViewModel viewYourFundingPublicationViewModel)
        {
            var dbPublication = await GetPublicationById(
                viewYourFundingPublicationViewModel.FundingStreamId,
                viewYourFundingPublicationViewModel.FundingPublication.Id);

            var updatedPublication = _mapper.Map<Publication>(viewYourFundingPublicationViewModel.FundingPublication);

            if (dbPublication == null)
            {
                return false;
            }

            var screenPublication = viewYourFundingPublicationViewModel.FundingPublication;
            screenPublication.Id = dbPublication.Id;

            ExpandViewModelToRows(screenPublication, updatedPublication);

            try
            {
                return await _adminPublicationService.UpdatePublication(updatedPublication);
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