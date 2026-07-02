using PDS.ViewYourFunding.Services.DTOs;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// Interface for generating funding view data.
    /// </summary>
    public interface IFundingViewDataGenerator
    {
        /// <summary>
        /// Generate the funding view data.
        /// </summary>
        /// <returns>The funding view data.</returns>
        FundingViewData Generate();
    }
}