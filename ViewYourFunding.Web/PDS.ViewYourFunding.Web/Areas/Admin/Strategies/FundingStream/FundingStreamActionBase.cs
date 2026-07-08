using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStream;
using System.Threading.Tasks;
using Service = PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStream
{
    /// <summary>
    /// The funding stream action base class.
    /// </summary>
    public abstract class FundingStreamActionBase
    {
        /// <summary>
        /// The view your funding stream service.
        /// </summary>
        private readonly IFundingStreamService _fundingStreamService;

        /// <summary>
        /// The view your funding admin settings  service.
        /// </summary>
        private readonly IAdminSettingsService _adminSettingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingStreamActionBase"/> class.
        /// </summary>
        /// <param name="fundingStreamService">The view your funding stream service.</param>
        /// <param name="adminSettingsService">The admin setting service.</param>
        protected FundingStreamActionBase(
           IAdminSettingsService adminSettingsService,
           IFundingStreamService fundingStreamService)
        {
            _adminSettingsService = adminSettingsService;
            _fundingStreamService = fundingStreamService;
        }


        /// <summary>
        /// Gets the view your funding stream view model.
        /// </summary>
        /// <param name="actionMode">The action mode.</param>
        /// <param name="fundingStream">The fundingStream.</param>
        /// <returns>
        /// The FundingStreamActionViewModel.
        /// </returns>
        public Task<FundingStreamViewModel> GetViewYourFundingStreamViewModel(
            ActionMode actionMode,
            Service.FundingStream fundingStream)
        {
            if (fundingStream == null)
            {
                return Task.FromResult(new FundingStreamViewModel());
            }

            var viewModel = new FundingStreamViewModel
            {
                FundingStream = new PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStream.FundingStream
                {
                    FundingStreamCode = fundingStream.FundingStreamCode,
                    FundingStreamName = fundingStream.FundingStreamName,
                    FundingStreamNameWithinSentence = fundingStream.FundingStreamNameWithinSentence,
                    FundingStreamBusinessAllocationName = fundingStream.FundingStreamBusinessAllocationName,
                    FundingStreamCodePubliclyKnown = fundingStream.FundingStreamCodePubliclyKnown,
                    RelevantForOrganisations_LoggedIn = fundingStream.RelevantForOrganisations_LoggedIn,
                    RelevantForOrganisations_Public = fundingStream.RelevantForOrganisations_Public,
                    RelevantForProviders_LoggedIn = fundingStream.RelevantForProviders_LoggedIn,
                    RelevantForNational = fundingStream.RelevantForNational,
                    RelevantForProviders_Public = fundingStream.RelevantForProviders_Public,
                    HistoryIndependentOfPublications = fundingStream.HistoryIndependentOfPublications,
                    Id = fundingStream.Id,
                    Active = fundingStream.Active,
                },
                ActionMode = actionMode
            };

            return Task.FromResult(viewModel);
        }

        /// <summary>
        /// Gets the funding stream by identifier.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <returns>The Funding stream.</returns>
        public async Task<Service.FundingStream> GetFundingStreamById(
            int fundingStreamId)
        {
            return await _adminSettingsService.GetFundingStreamById(fundingStreamId);
        }
    }
}