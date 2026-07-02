namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement
{
    /// <summary>
    /// Represents the filter.
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Gets or sets the filter id.
        /// </summary>
        /// <value>
        /// The filter id.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the filter Name.
        /// </summary>
        /// <value>
        /// The filter Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether filter is selected.
        /// </summary>
        /// <value>
        /// The filter selected.
        /// </value>
        public bool Selected { get; set; }
    }
}