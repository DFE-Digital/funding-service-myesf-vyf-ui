namespace PDS.VYF.Services.Abstracts.AppServices
{
    using PDS.ViewYourFunding.Services.DTOs;
    using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;

    /// <summary>
    /// Represents the interface for shared funding view services.
    /// </summary>
    public interface ISharedFundingViewServices
    {
        /// <summary>
        /// Gets the funding view data based on the provided view data request.
        /// </summary>
        /// <param name="viewDataRequest">The view data request.</param>
        /// <returns>The funding view data.</returns>
        Task<FundingViewData> GetFundingViewData(ViewDataRequestBase viewDataRequest);
    }
}
