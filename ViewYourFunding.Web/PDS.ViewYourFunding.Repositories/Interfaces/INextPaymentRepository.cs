using PDS.ViewYourFunding.Repositories.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Interfaces
{
    /// <summary>
    /// The Database service for the next payment entities.
    /// </summary>
    public interface INextPaymentRepository : IRepository<NextPayment>
    {
        /// <summary>
        /// Gets all of the Next Payments for a given funding stream.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream ID.</param>
        /// <returns>A list containing all of the NextPayments for the given funding stream.</returns>
        Task<IList<NextPayment>> GetNextPayments(int fundingStreamId);

        /// <summary>
        /// Updates the Next Payment.
        /// </summary>
        /// <param name="nextPayment">The NextPayment.</param>
        /// <returns>Returns true, if update is successful.</returns>
        Task<bool> UpdateNextPayment(NextPayment nextPayment);

        /// <summary>
        /// Deletes the Next Payment.
        /// </summary>
        /// <param name="nextPayment">The NextPayment.</param>
        /// <returns>Returns true if next payment deleted.</returns>
        Task<bool> DeleteNextPayment(NextPayment nextPayment);

        /// <summary>
        /// Creates the NextPayment.
        /// </summary>
        /// <param name="nextPayment">The Next Payment.</param>
        /// <returns>The created Next Payment.</returns>
        Task<NextPayment> CreateNextPayment(NextPayment nextPayment);
    }
}
