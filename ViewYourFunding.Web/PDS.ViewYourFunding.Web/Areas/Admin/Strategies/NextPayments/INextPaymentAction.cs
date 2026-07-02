using PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments
{
    /// <summary>
    /// The Next Payment action interface.
    /// </summary>
    public interface INextPaymentAction
    {
        /// <summary>
        /// Actions the specified funding stream identifier and next payment id.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="nextPaymentId">The next payment identifier.</param>
        /// <returns>Next payemnt view model.</returns>
        Task<NextPaymentViewModel> Action(
            int fundingStreamId,
            int nextPaymentId);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="nextPaymentViewModel">The view your funding next payment view model.</param>
        /// <returns>Returns true, if saved successfully.</returns>
        Task<bool> SaveChanges(NextPaymentViewModel nextPaymentViewModel);

        /// <summary>
        /// Checks if it Applies to.
        /// </summary>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>true if it applies to the action mode.</returns>
        bool AppliesTo(ActionMode actionMode);

        /// <summary>
        /// Gets the action mode.
        /// </summary>
        /// <value>
        /// The action mode.
        /// </value>
        ActionMode ActionMode { get; }
    }
}