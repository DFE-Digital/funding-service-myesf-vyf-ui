using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStreamSettings
{
    /// <summary>
    /// The Funding stream setting edit action.
    /// </summary>
    /// <seealso cref="IFundingStreamSettingAction" />
    public class FundingStreamSettingEditAction : FundingStreamSettingBaseAction, IFundingStreamSettingAction
    {
        /// <summary>
        /// The admin funding stream setting service.
        /// </summary>
        private readonly IAdminFundingStreamSettingService _adminFundingStreamSettingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingStreamSettingEditAction"/> class.
        /// </summary>
        /// <param name="adminFundingStreamSettingService">The admin funding stream setting service.</param>
        public FundingStreamSettingEditAction(IAdminFundingStreamSettingService adminFundingStreamSettingService)
        {
            _adminFundingStreamSettingService = adminFundingStreamSettingService;
        }

        /// <inheritdoc/>
        public async Task<bool> SaveChanges(FundingStreamSettingAreYouSureViewModel fundingStreamSettingAreYouSureViewModel)
        {
            var settingValue = GetSettingValue(fundingStreamSettingAreYouSureViewModel);
            return await _adminFundingStreamSettingService.UpdateAsync(settingValue);
        }

        /// <inheritdoc/>
        public bool AppliesTo(FundingStreamSettingAction actionMode)
        {
            return actionMode == ActionMode;
        }

        /// <inheritdoc/>
        public FundingStreamSettingAction ActionMode => FundingStreamSettingAction.Edit;
    }
}