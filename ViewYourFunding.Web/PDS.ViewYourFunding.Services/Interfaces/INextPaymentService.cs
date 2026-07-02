using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// The view your funding next payment service interface.
    /// </summary>
    public interface INextPaymentService
    {
        /// <summary>
        /// Gets all of the Next Payment Types for a given funding stream.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream Id.</param>
        /// <returns>A list containing all of the Next Payment Types for the given funding stream.</returns>
        Task<IList<NextPayment>> GetNextPayments(int fundingStreamId);

        /// <summary>
        /// Updates the Next Payment.
        /// </summary>
        /// <param name="nextPaymentType">The Next Payment Type.</param>
        /// <returns>Returns true, if update is successful.</returns>
        Task<bool> UpdateNextPayment(NextPayment nextPaymentType);

        /// <summary>
        /// Deletes the Next Payment.
        /// </summary>
        /// <param name="nextPaymentType">The Next Payment Type.</param>
        /// <returns>Returns True, if the deletion was successful.</returns>
        Task<bool> DeleteNextPayment(NextPayment nextPaymentType);

        /// <summary>
        /// Creates the Next Payment.
        /// </summary>
        /// <param name="nextPaymentType">The Next Payment Type.</param>
        /// <returns>The created Next Payment.</returns>
        Task<NextPayment> CreateNextPayment(NextPayment nextPaymentType);
    }
}
