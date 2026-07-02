using PDS.ViewYourFunding.Repositories.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Interfaces
{
    /// <summary>
    /// The Database service for the next payment type entities.
    /// </summary>
    public interface INextPaymentTypeRepository : IRepository<NextPaymentType>
    {
        /// <summary>
        /// Gets all of the Next Payment Types for a given funding stream.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream ID.</param>
        /// <returns>A list containing all of the Next Payment Types for the given funding stream.</returns>
        Task<IList<NextPaymentType>> GetNextPaymentTypes(int fundingStreamId);

        /// <summary>
        /// Updates the Next Payment Type.
        /// </summary>
        /// <param name="nextPaymentType">The Next Payment Type.</param>
        /// <returns>Returns true, if update is successful.</returns>
        Task<bool> UpdateNextPaymentType(NextPaymentType nextPaymentType);

        /// <summary>
        /// Deletes the Next Payment Type.
        /// </summary>
        /// <param name="nextPaymentType">The Next Payment Type.</param>
        /// <returns>Returns true if Next payment type deleted.</returns>
        Task<bool> DeleteNextPaymentType(NextPaymentType nextPaymentType);

        /// <summary>
        /// Creates the Next Payment Type.
        /// </summary>
        /// <param name="nextPaymentType">The Next Payment Type.</param>
        /// <returns>The created Next Payment Type.</returns>
        Task<NextPaymentType> CreateNextPaymentType(NextPaymentType nextPaymentType);

        /// <summary>
        /// Gets all of the Next Payment Types.
        /// </summary>
        /// <returns>
        /// A list containing all of the Next Payment Types.
        /// </returns>
        Task<IList<NextPaymentType>> GetAllNextPaymentTypes();
    }
}
