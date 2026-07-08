using AutoMapper;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Helpers;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.Publications
{
    /// <summary>
    /// The publication add action class.
    /// </summary>
    /// <seealso cref="PublicationActionBase" />
    /// <seealso cref="IPublicationAction" />
    public class PublicationAddAction : PublicationActionBase, IPublicationAction
    {
        /// <summary>
        /// The view your funding settings service.
        /// </summary>
        private readonly IAdminSettingsService _adminSettingsService;

        /// <summary>
        /// The view your funding publication service.
        /// </summary>
        private readonly IAdminPublicationService _adminPublicationService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationAddAction"/> class.
        /// </summary>
        /// <param name="adminPublicationService">The view your funding publication service.</param>
        /// <param name="adminSettingsService">The view your funding settings service.</param>
        /// <param name="fundingUiModelDetailsService">The funding UI Model details service.</param>
        /// <param name="layoutManagementService">The layout management service.</param>
        /// <param name="mapper">The mapper.</param>
        public PublicationAddAction(
            IAdminPublicationService adminPublicationService,
            IAdminSettingsService adminSettingsService,
            IFundingUiModelDetailsService fundingUiModelDetailsService,
            ILayoutManagementService layoutManagementService,
            IMapper mapper)
            : base(
                adminPublicationService,
                fundingUiModelDetailsService,
                layoutManagementService)
        {
            _adminSettingsService = adminSettingsService;
            _adminPublicationService = adminPublicationService;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<PublicationActionViewModel> Action(
            int fundingStreamId,
            int nextPaymentId)
        {
            var fundingStream = await _adminSettingsService.GetFundingStreamById(fundingStreamId, FetchData.Publications, FetchData.Publications_PublicationLayouts);
            var maxUiAndSpreadsheetVersionNumbers = await GetMaximumUiAndSpreadsheetVersionNumbers(fundingStream.FundingStreamCode, null);

            if (!fundingStream.Id.Equals(default))
            {
                var layouts = await GetFundingStreamLayouts(fundingStreamId);
                var viewModel = new PublicationActionViewModel
                {
                    FundingPublication = new PublicationViewModel
                    {
                        FundingStreamId = fundingStreamId,
                        UIModelMaxVersion = maxUiAndSpreadsheetVersionNumbers?.MaximumUiVersion,
                        SpreadsheetModelMaxVersion = maxUiAndSpreadsheetVersionNumbers?.MaximumSpreadsheetVersion,
                        LayoutUiModels = layouts.Select(LayoutHelper.MapLayoutUiModel).ToList()
                    },
                    ActionMode = ActionMode,
                    FundingStreamId = fundingStreamId,
                    FundingStreamName = fundingStream.FundingStreamName
                };

                return viewModel;
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<bool> SaveChanges(PublicationActionViewModel viewYourFundingPublicationViewModel)
        {
            var newPublication = _mapper.Map<Publication>(viewYourFundingPublicationViewModel.FundingPublication);

            ExpandViewModelToRows(viewYourFundingPublicationViewModel.FundingPublication, newPublication);
            var result = await _adminPublicationService.CreatePublication(newPublication);

            if (result != null)
            {
                viewYourFundingPublicationViewModel.FundingPublication.Id = result.Id;
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