using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.Publications
{
    /// <summary>
    /// The publication action interface.
    /// </summary>
    public interface IPublicationAction
    {
        /// <summary>
        /// Actions the specified funding stream identifier and publication id.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="publicationId">The publication identifier.</param>
        /// <returns>The ViewYourFundingPublicationViewModel.</returns>
        Task<PublicationActionViewModel> Action(
            int fundingStreamId,
            int publicationId);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="viewYourFundingPublicationViewModel">The view your funding publication view model.</param>
        /// <returns>True if the changes are saved successfully.</returns>
        Task<bool> SaveChanges(PublicationActionViewModel viewYourFundingPublicationViewModel);

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