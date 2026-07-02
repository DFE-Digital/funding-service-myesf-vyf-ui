namespace PDS.VYF.Services.Abstracts.AppServices
{
    using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;
    using PDS.VYF.Services.Models.ResponseModels.ViewDataResponseModels;

    /// <summary>
    /// Represents the interface for parent funding view services.
    /// </summary>
    public interface IParentFundingViewServices
    {
        /// <summary>
        /// Retrieves the parent summary view data based on the provided request.
        /// </summary>
        /// <param name="request">The request model containing the necessary parameters.</param>
        /// <returns>The response model containing the parent summary view data.</returns>
        Task<ParentSummaryViewDataResponseModel> GetParentSummaryViewData(ParentSummaryViewDataRequestModel request);
    }
}
