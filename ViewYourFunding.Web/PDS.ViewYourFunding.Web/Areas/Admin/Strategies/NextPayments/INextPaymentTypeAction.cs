using PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPaymentType;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments
{
    /// <summary>
    /// The next payment type action class.
    /// </summary>
    public interface INextPaymentTypeAction
    {
        /// <summary>
        /// Actions the specified funding stream identifier and next payment type id.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="nextPaymentTypeId">The next payment type identifier.</param>
        /// <returns>The NextPaymentTypeViewModel. </returns>
        Task<NextPaymentTypeViewModel> Action(
            int fundingStreamId,
            int nextPaymentTypeId);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="nextPaymentTypeViewModel">The view your funding next payment type view model.</param>
        /// <returns>True if the changes are saved successfully.</returns>
        Task<bool> SaveChanges(NextPaymentTypeViewModel nextPaymentTypeViewModel);

        /// <summary>
        /// Checks if the action mode Applies to the class.
        /// </summary>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>True if the action mode applies to to the action.</returns>
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