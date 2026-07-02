using PDS.ViewYourFunding.Services.Interfaces.Models;

namespace PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects
{
    /// <inheritdoc cref="IFundingApiAddUserVisitResponse"/>
    public class FundingApiAddUserVisitResponse : IFundingApiAddUserVisitResponse
    {
        /// <inheritdoc/>
        public bool Success { get; set; }
    }
}
