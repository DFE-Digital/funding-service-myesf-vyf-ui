namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement
{
    /// <summary>
    /// Class to hold pagination details.
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the total no. of pages.
        /// </summary>
        /// <value>
        /// The total no. of pages.
        /// </value>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the total no. of records.
        /// </summary>
        /// <value>
        /// The total no. of records.
        /// </value>
        public int ResultCount { get; set; }

        /// <summary>
        /// Gets or sets the index of page.
        /// </summary>
        /// <value>
        /// The index of page.
        /// </value>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the first record no. on a page.
        /// </summary>
        /// <value>
        /// The first record no.
        /// </value>
        public int FirstRecordNo { get; set; }

        /// <summary>
        /// Gets or sets the last record no. on a page.
        /// </summary>
        /// <value>
        /// The last Record no.
        /// </value>
        public int LastRecordNo { get; set; }

        /// <summary>
        /// Gets or sets the first page of pagination.
        /// </summary>
        /// <value>
        /// The first page of pagination.
        /// </value>
        public int FirstPage { get; set; }

        /// <summary>
        /// Gets or sets the last page of pagination.
        /// </summary>
        /// <value>
        /// The last page of pagination.
        /// </value>
        public int LastPage { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        /// <value>
        /// The pageSize.
        /// </value>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets a value indicating whether has previous page.
        /// </summary>
        /// <value>
        /// Bool value to indicate whether has previous page.
        /// </value>
        public bool HasPreviousPage
        {
            get
            {
                return PageNumber > 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether has next page.
        /// </summary>
        /// <value>
        /// Bool value to indicate whether has next page.
        /// </value>
        public bool HasNextPage
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }
    }
}