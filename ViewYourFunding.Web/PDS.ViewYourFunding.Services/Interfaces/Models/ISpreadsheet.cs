using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Interfaces.Models
{
    /// <summary>
    /// All the data required to represent a spreadsheet.
    /// </summary>
    public interface ISpreadsheet
    {
        /// <summary>
        /// Gets or sets worksheets inside this spreadsheet.
        /// </summary>
        Dictionary<string, IWorksheet> Worksheets { get; set; }

        /// <summary>
        /// Gets or sets image resources used in this spreadsheet.
        /// </summary>
        Dictionary<string, byte[]> Images { get; set; }
    }
}