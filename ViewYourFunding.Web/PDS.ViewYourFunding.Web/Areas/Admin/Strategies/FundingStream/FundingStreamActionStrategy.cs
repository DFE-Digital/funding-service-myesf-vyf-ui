using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStream
{
    /// <summary>
    /// The funding stream strategy for handling related funding stream actions.
    /// </summary>
    public class FundingStreamActionStrategy
    {
        /// <summary>
        /// Gets or sets the funding stream actions.
        /// </summary>
        /// <value>
        /// The funding stream actions.
        /// </value>
        public IList<IFundingStreamAction> FundingStreamActions { get; set; }
    }
}