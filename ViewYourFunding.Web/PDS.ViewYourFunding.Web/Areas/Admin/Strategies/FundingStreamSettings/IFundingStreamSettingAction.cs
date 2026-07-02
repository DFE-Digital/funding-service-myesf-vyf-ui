using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStreamSettings
{
    /// <summary>
    /// The Funding stream setting action interface.
    /// </summary>
    public interface IFundingStreamSettingAction
    {
        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="fundingStreamSettingAreYouSureViewModel">The view your funding setting value view model.</param>
        /// <returns>Returns true, if saved successfully.</returns>
        Task<bool> SaveChanges(FundingStreamSettingAreYouSureViewModel fundingStreamSettingAreYouSureViewModel);

        /// <summary>
        /// Checks if it Applies to.
        /// </summary>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>true if it applies to the action mode.</returns>
        bool AppliesTo(FundingStreamSettingAction actionMode);

        /// <summary>
        /// Gets the action mode.
        /// </summary>
        /// <value>
        /// The action mode.
        /// </value>
        FundingStreamSettingAction ActionMode { get; }
    }
}