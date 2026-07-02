using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// An interface exposing get/set operations for publications in the View Your Funding area.
    /// </summary>
    public interface IAdminPublicationService
    {
        /// <summary>
        /// Gets all of the publications for a given funding stream.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream ID.</param>
        /// <returns>A list containing all of the publications for the given funding stream.</returns>
        Task<IList<Publication>> GetPublications(int fundingStreamId);

        /// <summary>
        /// Gets all of the publications.
        /// </summary>
        /// <returns>A list containing all of the publications.</returns>
        Task<IReadOnlyList<Publication>> GetAll();

        /// <summary>
        /// Updates the publication.
        /// </summary>
        /// <param name="publication">The publication.</param>
        /// <returns>True if the update was successful.</returns>
        Task<bool> UpdatePublication(Publication publication);

        /// <summary>
        /// Deletes the publication.
        /// </summary>
        /// <param name="publication">The publication.</param>
        /// <returns>The deleted publication.</returns>
        Task<bool> DeletePublication(Publication publication);

        /// <summary>
        /// Creates the publication.
        /// </summary>
        /// <param name="publication">The publication.</param>
        /// <returns>The added publication.</returns>
        Task<Publication> CreatePublication(Publication publication);
    }
}