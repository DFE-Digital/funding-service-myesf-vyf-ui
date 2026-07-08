using PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStream;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStream
{
    /// <summary>
    /// The funding stream action interface.
    /// </summary>
    public interface IFundingStreamAction
    {
        /// <summary>
        /// Actions the specified funding stream identifier .
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <returns>The FundingStreamViewModel.</returns>
        Task<FundingStreamViewModel> Action(
            int fundingStreamId);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="fundingStreamViewModel">The view your funding stream view model.</param>
        /// <returns>True if the changes are saved successfully.</returns>
        Task<bool> SaveChanges(FundingStreamViewModel fundingStreamViewModel);

        /// <summary>
        /// Checks if it the passed action Applies to this instance.
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