using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// The view your funding next payment type service interface.
    /// </summary>
    public interface INextPaymentTypeService
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
        /// <returns>Returns true, if update was successful.</returns>
        Task<bool> UpdateNextPaymentType(NextPaymentType nextPaymentType);

        /// <summary>
        /// Deletes the Next Payment Type.
        /// </summary>
        /// <param name="nextPaymentType">The Next Payment Type.</param>
        /// <returns>Returns true, if deletion was successful.</returns>
        Task<bool> DeleteNextPaymentType(NextPaymentType nextPaymentType);

        /// <summary>
        /// Creates the Next Payment Type.
        /// </summary>
        /// <param name="nextPaymentType">The Next Payment Type.</param>
        /// <returns>The created Next Payment Type.</returns>
        Task<NextPaymentType> CreateNextPaymentType(NextPaymentType nextPaymentType);

        /// <summary>
        /// Returns the Next Payment Type.
        /// </summary>
        /// <returns>The list of all Next Payment Types.</returns>
        Task<List<NextPaymentType>> GetAllNextPaymentTypes();
    }
}
