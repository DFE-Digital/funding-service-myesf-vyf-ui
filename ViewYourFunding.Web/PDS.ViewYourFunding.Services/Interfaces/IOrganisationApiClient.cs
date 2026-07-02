using PDS.ViewYourFunding.Services.Models;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// Interface for the organisation api client.
    /// </summary>
    public interface IOrganisationApiClient
    {
        /// <summary>
        /// Gets the organisation with the given identifier.
        /// </summary>
        /// <param name="identifier">The organisation identifier to lookup.</param>
        /// <returns>The organisation with the given identifier.</returns>
        Task<Organisation> GetOrganisation(OrganisationIdentifier identifier);
    }
}