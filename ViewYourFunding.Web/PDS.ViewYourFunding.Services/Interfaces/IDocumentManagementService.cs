using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Interfaces.Models;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// Api for manipulating and getting information from spreadsheets.
    /// </summary>
    public interface IDocumentManagementService
    {
        /// <summary>
        /// Create a spreadsheet from the data supplied.
        /// </summary>
        /// <param name="spreadsheet">The data to generate the spreadsheet with. Each dictionary entry represents one worksheet + An images collection - a
        /// dictionary where the key is the identifier for the image.</param>
        /// <param name="fileFormat">File format e.g. csv, ods. by default use a popular vendor specific version (e.g. XLS 2003).</param>
        /// <param name="removeFormulas">Whether to remove all formulas and replace them with their evaluated values.</param>
        /// <returns>The spreadsheet as a byte array.</returns>
        byte[] CreateSpreadsheetWithData(ISpreadsheet spreadsheet, FileFormat fileFormat, bool removeFormulas = false);

        /// <summary>
        /// Enable the license.
        /// </summary>
        /// <returns>Bool if okay, false if not.</returns>
        bool EnableCellsLicense();
    }
}