using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStreamSettings
{
    /// <summary>
    /// The funding stream setting strategy for handling related funding stream setting actions.
    /// </summary>
    public class FundingStreamSettingActionStrategy
    {
        /// <summary>
        /// Gets or sets the funding stream setting actions.
        /// </summary>
        /// <value>
        /// The funding stream setting actions.
        /// </value>
        public IReadOnlyList<IFundingStreamSettingAction> FundingStreamSettingActions { get; set; }
    }
}