using PDS.ViewYourFunding.Services.Models;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// Generate Spreadsheet Service Interface.
    /// </summary>
    public interface IGenerateSpreadsheetService
    {
        /// <summary>
        /// Generates the funding stream spread sheet by URL asynchronously.
        /// </summary>
        /// <param name="spreadsheetUrl">The spreadsheet URL.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="fundingStreamName">Name of the funding stream.</param>
        /// <returns>Awaitable task containing metadata about the result.</returns>
        Task<GenerateSpreadsheetResult> GenerateFundingStreamSpreadSheetByUrlAsync(string spreadsheetUrl, string settingName, string fundingStreamName);
    }
}