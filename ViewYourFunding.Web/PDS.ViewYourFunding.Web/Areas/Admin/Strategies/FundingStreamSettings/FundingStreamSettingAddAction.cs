using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStreamSettings
{
    /// <summary>
    /// The Funding stream setting add action class.
    /// </summary>
    /// <seealso cref="IFundingStreamSettingAction" />
    public class FundingStreamSettingAddAction : FundingStreamSettingBaseAction, IFundingStreamSettingAction
    {
        /// <summary>
        /// The admin funding stream setting service.
        /// </summary>
        private readonly IAdminFundingStreamSettingService _adminFundingStreamSettingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingStreamSettingAddAction"/> class.
        /// </summary>
        /// <param name="adminFundingStreamSettingService">The admin funding stream setting service.</param>
        public FundingStreamSettingAddAction(IAdminFundingStreamSettingService adminFundingStreamSettingService)
        {
            _adminFundingStreamSettingService = adminFundingStreamSettingService;
        }

        /// <inheritdoc/>
        public async Task<bool> SaveChanges(FundingStreamSettingAreYouSureViewModel fundingStreamSettingAreYouSureViewModel)
        {
            var settingValue = GetSettingValue(fundingStreamSettingAreYouSureViewModel);
            var result = await _adminFundingStreamSettingService.AddAsync(settingValue);
            return !result.Id.Equals(default);
        }

        /// <inheritdoc/>
        public bool AppliesTo(FundingStreamSettingAction actionMode)
        {
            return actionMode == ActionMode;
        }

        /// <inheritdoc/>
        public FundingStreamSettingAction ActionMode => FundingStreamSettingAction.Add;
    }
}