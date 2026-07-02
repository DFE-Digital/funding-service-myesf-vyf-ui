using System.Collections.Generic;

namespace PDS.ViewYourFunding.Automation.Pages.LoggedIn
{
    /// <summary>
    /// The Table Row Data class.
    /// </summary>
    public class TableRowData
    {
        /// <summary>
        /// Gets or sets the row items.
        /// </summary>
        /// <value>
        /// The row items.
        /// </value>
        public IEnumerable<string> RowItems { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a header row.
        /// </summary>
        /// <value>
        ///   True if it is a header row.
        /// </value>
        public bool HeaderRow { get; set; }
    }
}