using System.Collections.Generic;

namespace PDS.ViewYourFunding.Automation.Pages.LoggedIn
{
    /// <summary>
    /// Table Data used to Test table content.
    /// </summary>
    public class TableData
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the table row data.
        /// </summary>
        /// <value>
        /// The table row data.
        /// </value>
        public IEnumerable<TableRowData> TableRowData { get; set; } = new List<TableRowData>();
    }
}