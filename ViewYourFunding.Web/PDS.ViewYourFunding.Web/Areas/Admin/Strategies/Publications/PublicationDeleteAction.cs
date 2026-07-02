using AutoMapper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.Publications
{
    /// <summary>
    /// The publication Delete Action class.
    /// </summary>
    /// <seealso cref="PublicationActionBase" />
    /// <seealso cref="IPublicationAction" />
    public class PublicationDeleteAction : PublicationActionBase, IPublicationAction
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
        /// Initializes a new instance of the <see cref="PublicationDeleteAction"/> class.
        /// </summary>
        /// <param name="adminPublicationService">The view your funding publication service.</param>
        /// <param name="fundingUiModelDetailsService">The funding UI Model details service.</param>
        /// <param name="layoutManagementService">The layout management service.</param>
        /// <param name="mapper">The mapper.</param>
        public PublicationDeleteAction(
            IAdminPublicationService adminPublicationService,
            IFundingUiModelDetailsService fundingUiModelDetailsService,
            ILayoutManagementService layoutManagementService,
            IMapper mapper)
            : base(
                adminPublicationService,
                fundingUiModelDetailsService,
                layoutManagementService)
        {
            _adminPublicationService = adminPublicationService;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<PublicationActionViewModel> Action(
            int fundingStreamId,
            int publicationId)
        {
            var publication = await GetPublicationById(fundingStreamId, publicationId);

            return await GetViewYourFundingPublicationViewModel(
                fundingStreamId,
                publicationId,
                ActionMode,
                publication,
                false);
        }

        /// <inheritdoc/>
        public async Task<bool> SaveChanges(PublicationActionViewModel viewYourFundingPublicationViewModel)
        {
            var publication = await GetPublicationById(
                viewYourFundingPublicationViewModel.FundingStreamId,
                viewYourFundingPublicationViewModel.FundingPublication.Id);

            var updatedPublication = _mapper.Map<Publication>(viewYourFundingPublicationViewModel.FundingPublication);
            if (publication != null)
            {
                ExpandViewModelToRows(viewYourFundingPublicationViewModel.FundingPublication, updatedPublication);
                return await _adminPublicationService.DeletePublication(updatedPublication);
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