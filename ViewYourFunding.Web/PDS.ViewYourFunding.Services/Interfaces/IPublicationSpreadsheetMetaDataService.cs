using PDS.ViewYourFunding.Services.Models;
using System.Threading.Tasks;
using ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// The Publication spreadsheet meta data service interface.
    /// </summary>
    public interface IPublicationSpreadsheetMetaDataService
    {
        /// <summary>
        /// Gets the publication spreadsheet meta data.
        /// </summary>
        /// <param name="publication">The publication.</param>
        /// <returns>The publication spreadsheet meta.</returns>
        Task<PublicationSpreadsheetMetaData> GetPublicationSpreadsheetMetaDataAsync(Publication publication);
    }
}