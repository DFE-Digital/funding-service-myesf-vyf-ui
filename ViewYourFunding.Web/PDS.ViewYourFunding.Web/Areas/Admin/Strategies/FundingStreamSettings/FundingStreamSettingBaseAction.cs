using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStreamSettings
{
    /// <summary>
    /// The Funding stream setting add action class.
    /// </summary>
    /// <seealso cref="IFundingStreamSettingAction" />
    public class FundingStreamSettingBaseAction
    {
        /// <summary>
        /// Get the setting value.
        /// </summary>
        /// <param name="viewmodel">The funding stream setting are you sure viewModel.</param>
        /// <returns>A SettingValue.</returns>
        public SettingValue GetSettingValue(FundingStreamSettingAreYouSureViewModel viewmodel)
        {
            return new SettingValue
            {
                Id = viewmodel.SettingValueId,
                Value = viewmodel.NewValue,
                LastUpdatedBy = viewmodel.CurrentUser?.FullName,
                FundingStreamId = viewmodel.FundingStreamId,
                SettingId = viewmodel.SettingId
            };
        }
    }
}