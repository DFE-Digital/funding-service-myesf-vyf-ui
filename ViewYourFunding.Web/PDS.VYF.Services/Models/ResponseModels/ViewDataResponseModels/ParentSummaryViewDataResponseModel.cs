namespace PDS.VYF.Services.Models.ResponseModels.ViewDataResponseModels
{
    using PDS.ViewYourFunding.Services.DTOs;
    using PDS.ViewYourFunding.Services.Interfaces.Models;
    using PDS.ViewYourFunding.Services.Models;
    using System.Collections.Generic;

    /// <summary>
    /// The child summary view data response model.
    /// </summary>
    /// <seealso cref="PDS.VYF.Services.Models.ResponseModels.ViewDataResponseModels.ViewDataResponseModelBase" />
    public class ParentSummaryViewDataResponseModel : ViewDataResponseModelBase
    {
        /// <summary>
        /// Gets or sets the funding view data.
        /// </summary>
        /// <value>
        /// The funding view data.
        /// </value>
        public Dictionary<string, FundingViewData>? FundingViewData { get; set; }

        /// <summary>
        /// Gets or sets the provider funding data.
        /// </summary>
        /// <value>
        /// The provider funding data.
        /// </value>
        public List<IFundingApiSearchProviderFunding>? ProviderFundingData { get; set; }

        /// <summary>
        /// Gets or sets the relavant funding streams.
        /// </summary>
        /// <value>
        /// The relavant funding streams.
        /// </value>
        public HashSet<FundingStream> RelavantFundingStreams { get; set; } = new HashSet<FundingStream>();
    }
}
