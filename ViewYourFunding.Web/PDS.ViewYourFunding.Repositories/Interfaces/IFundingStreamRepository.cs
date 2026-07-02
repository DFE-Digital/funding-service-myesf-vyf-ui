using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Interfaces
{
    /// <summary>
    /// The funding stream repository interface.
    /// </summary>
    public interface IFundingStreamRepository : IRepository<FundingStream>
    {
        /// <summary>
        /// Gets all funding streams.
        /// </summary>
        /// <param name="activeOnly">Get only active funding streams.</param>
        /// <param name="fetchData">Extra data to fetch.</param>
        /// <returns>A list of funding streams.</returns>
        Task<IEnumerable<FundingStream>> GetAllFundingStreams(bool activeOnly, params FetchData[] fetchData);

        /// <summary>
        /// Updates the funding stream.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <returns>Returns true, if update is successful.</returns>
        Task<bool> UpdateFundingStream(FundingStream fundingStream);

        /// <summary>
        /// Deletes the funding stream.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <returns>Returns true if funding stream was deleted.</returns>
        Task<bool> DeleteFundingStream(FundingStream fundingStream);

        /// <summary>
        /// Creates the funding stream.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <returns>Returns The funding stream.</returns>
        Task<FundingStream> CreateFundingStream(FundingStream fundingStream);
    }
}