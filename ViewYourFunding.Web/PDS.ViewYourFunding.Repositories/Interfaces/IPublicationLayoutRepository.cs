using PDS.ViewYourFunding.Repositories.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Interfaces
{
    /// <summary>
    /// The Publication Layout Repository Interfaces.
    /// </summary>
    public interface IPublicationLayoutRepository : IRepository<PublicationLayout>
    {
        /// <summary>
        /// Gets the publication layouts.
        /// </summary>
        /// <param name="publicationId">The publication id.</param>
        /// <returns>List of publication layouts.</returns>
        Task<IList<PublicationLayout>> GetPublicationLayouts(int publicationId);

        /// <summary>
        /// Updates the publication layout.
        /// </summary>
        /// <param name="publicationLayout">The publication layout.</param>
        /// <returns>True if changes were persisted.</returns>
        Task<bool> UpdatePublicationLayout(PublicationLayout publicationLayout);

        /// <summary>
        /// Deletes the publication layout.
        /// </summary>
        /// <param name="publicationLayout">The publication layout.</param>
        /// <returns>True if the deletion was successful.</returns>
        Task<bool> DeletePublicationLayout(PublicationLayout publicationLayout);

        /// <summary>
        /// Creates the publication layout.
        /// </summary>
        /// <param name="publicationLayout">The publication layout.</param>
        /// <returns>The created publication layout.</returns>
        Task<PublicationLayout> CreatePublicationLayout(PublicationLayout publicationLayout);
    }
}