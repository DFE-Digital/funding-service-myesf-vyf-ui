using PDS.ViewYourFunding.Services.DTOs;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Interfaces.Models
{
    /// <summary>
    /// Represents sheet data.
    /// </summary>
    public interface IWorksheet
    {
        /// <summary>
        /// Gets or sets merge info for the sheet.
        /// </summary>
        List<MergeData> MergeInfo { get; set; }

        /// <summary>
        /// Gets or sets frozen info for the sheet.
        /// </summary>
        List<FrozenData> FrozenInfo { get; set; }

        /// <summary>
        /// Gets or sets columns in the sheet.
        /// </summary>
        List<Column> Columns { get; set; }

        /// <summary>
        /// Gets or sets height information for the sheet.
        /// </summary>
        List<Height> Heights { get; set; }

        /// <summary>
        /// Gets or sets data for the sheet.
        /// </summary>
        Dictionary<string, CellData> Cells { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether should the worksheet have auto filters applied to it.
        /// </summary>
        bool AutoFilter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the row to start auto filter.
        /// </summary>
        int AutoFilterStartRow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the header row.
        /// </summary>
        string HeaderRow { get; set; }
    }
}