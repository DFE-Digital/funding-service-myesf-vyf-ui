using PDS.ViewYourFunding.Services.Models;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// The funding UI (spreadsheet and view-data) model details service.
    /// </summary>
    public interface IFundingUiModelDetailsService
    {
        /// <summary>
        ///  Get the maximum UI and spreadsheet schema template version numbers by URL asynchronously.
        /// </summary>
        /// <param name="versionNumberUrl">The URL from which version numbers will be retrieved.</param>
        /// <returns>The maximum UI and spreadsheet schema template version numbers.</returns>
        Task<MaximumUiSpreadsheetVersion> GetMaximumUiAndSpreadsheetVersionNumbersByUrl(string versionNumberUrl);
    }
}