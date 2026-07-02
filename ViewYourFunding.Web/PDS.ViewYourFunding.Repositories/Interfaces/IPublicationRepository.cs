using PDS.ViewYourFunding.Repositories.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Interfaces
{
    /// <summary>
    /// The Publication Repository Interfaces.
    /// </summary>
    public interface IPublicationRepository : IRepository<Publication>
    {
        /// <summary>
        /// Gets the publications.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <returns>List of Publications.</returns>
        Task<IList<Publication>> GetPublications(int fundingStreamId);

        /// <summary>
        /// Updates the publication.
        /// </summary>
        /// <param name="publication">The publication.</param>
        /// <returns>True if changes were persisted.</returns>
        Task<bool> UpdatePublication(Publication publication);

        /// <summary>
        /// Deletes the publication.
        /// </summary>
        /// <param name="publication">The publication.</param>
        /// <returns>True if the deletion was successful.</returns>
        Task<bool> DeletePublication(Publication publication);

        /// <summary>
        /// Creates the publication.
        /// </summary>
        /// <param name="publication">The publication.</param>
        /// <returns>The created publication.</returns>
        Task<Publication> CreatePublication(Publication publication);
    }
}