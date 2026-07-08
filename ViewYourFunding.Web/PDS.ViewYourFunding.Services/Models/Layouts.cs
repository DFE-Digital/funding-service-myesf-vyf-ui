using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Class to hold layouts per page.
    /// </summary>
    public class Layouts
    {
        /// <summary>
        /// Gets or sets the total no. of Layouts.
        /// </summary>
        /// <value>
        /// Total no. of layouts.
        /// </value>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets the list of layouts.
        /// </summary>
        /// <value>
        /// list of layouts.
        /// </value>
        public IEnumerable<LayoutModel> LayoutModel { get; set; }

        /// <summary>
        /// Gets or sets the pagination details.
        /// </summary>
        /// <value>
        /// Pagination detail.
        /// </value>
        public Pagination PaginationDetail { get; set; }
    }
}