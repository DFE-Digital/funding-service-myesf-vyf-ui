namespace PDS.VYF.Services.Abstracts.AppServices
{
    using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;
    using PDS.VYF.Services.Models.ResponseModels.ViewDataResponseModels;

    /// <summary>
    /// Represents the interface for child funding view services.
    /// </summary>
    public interface IChildFundingViewServices
    {
        /// <summary>
        /// Retrieves the detailed view data for a child.
        /// </summary>
        /// <param name="request">The request model containing the necessary parameters.</param>
        /// <returns>The response model containing the detailed view data.</returns>
        Task<ChildDetailedViewDataResponseModel> GetChildDetailedViewData(ChildDetailedViewDataRequestModel request);

        /// <summary>
        /// Retrieves the history view data for a child.
        /// </summary>
        /// <param name="request">The request model containing the necessary parameters.</param>
        /// <returns>The response model containing the history view data.</returns>
        Task<ChildHistoryViewDataResponseModel> GetChildHistoryViewData(ChildHistoryViewDataRequestModel request);

        /// <summary>
        /// Retrieves the summary view data for a child.
        /// </summary>
        /// <param name="request">The request model containing the necessary parameters.</param>
        /// <returns>The response model containing the summary view data.</returns>
        Task<ChildSummaryViewDataResponseModel> GetChildSummaryViewData(ChildSummaryViewDataRequestModel request);
    }
}
