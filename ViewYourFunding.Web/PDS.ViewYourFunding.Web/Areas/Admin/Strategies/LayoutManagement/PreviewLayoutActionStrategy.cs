using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.LayoutManagement;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStreamSettings
{
    /// <summary>
    /// The preview layout action strategy for handling related preview layout actions.
    /// </summary>
    public class PreviewLayoutActionStrategy
    {
        /// <summary>
        /// Gets or sets the preview layout actions.
        /// </summary>
        /// <value>
        /// The preview layout actions.
        /// </value>
        public IReadOnlyList<IPreviewLayoutAction> PreviewLayoutActions { get; set; }
    }
}