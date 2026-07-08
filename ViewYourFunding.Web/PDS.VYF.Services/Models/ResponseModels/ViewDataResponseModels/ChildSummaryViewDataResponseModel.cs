namespace PDS.VYF.Services.Models.ResponseModels.ViewDataResponseModels
{
    using PDS.ViewYourFunding.Services.DTOs;
    using PDS.VYF.Services.Enums;

    /// <summary>
    /// The child summary view data response model.
    /// </summary>
    /// <seealso cref="PDS.VYF.Services.Models.ResponseModels.ViewDataResponseModels.ViewDataResponseModelBase" />
    public class ChildSummaryViewDataResponseModel : ViewDataResponseModelBase
    {
        /// <summary>
        /// Gets or sets the funding view data.
        /// </summary>
        /// <value>
        /// The funding view data.
        /// </value>
        public Dictionary<string, FundingViewData>? FundingViewData { get; set; }

        /// <summary>
        /// Gets the statement visit information.
        /// </summary>
        /// <value>
        /// The statement visit information.
        /// </value>
        public Dictionary<string, StatementVisitInfoEnum> StatementVisitInfo { get; private set; } = new();

        /// <summary>
        /// Gets or sets a value indicating whether [via choice page].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [via choice page]; otherwise, <c>false</c>.
        /// </value>
        public bool ViaChoicePage { get; set; }
    }
}
