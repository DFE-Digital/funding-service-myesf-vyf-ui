using PDS.ViewYourFunding.Services.Models;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// An interface exposing crud operations for the funding stream settings.
    /// </summary>
    public interface IFundingStreamService
    {
        /// <summary>
        /// Updates the Funding stream.
        /// </summary>
        /// <param name="fundingStream">The Funding Stream.</param>
        /// <returns>Returns true, if update was successful.</returns>
        Task<bool> UpdateFundingStream(FundingStream fundingStream);

        /// <summary>
        /// Deletes the Funding Stream.
        /// </summary>
        /// <param name="fundingStream">The Funding Stream.</param>
        /// <returns>Returns true, if deletion was successful.</returns>
        Task<bool> DeleteFundingStream(FundingStream fundingStream);

        /// <summary>
        /// Creates the Funding Stream.
        /// </summary>
        /// <param name="fundingStream">The Funding Stream.</param>
        /// <returns>The created Funding Stream.</returns>
        Task<FundingStream> CreateFundingStream(FundingStream fundingStream);
    }
}