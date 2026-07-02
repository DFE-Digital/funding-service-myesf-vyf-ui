using PDS.ViewYourFunding.Services.Interfaces.Models;

namespace PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects
{
    /// <inheritdoc cref="IUserFundingViewCountResponse"/>
    public class UserFundingViewCountResponse : IUserFundingViewCountResponse
    {
        /// <inheritdoc />
        public string UserId { get; set; }

        /// <inheritdoc />
        public int UnreadNewFundings { get; set; }

        /// <inheritdoc />
        public int UnreadUpdatedFundings { get; set; }
    }
}