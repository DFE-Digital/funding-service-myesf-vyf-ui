namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement
{
    /// <summary>
    /// Holds data for select items.
    /// </summary>
    public class FundingViewScopeSelectItem
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the applicable types strings (for the data- attribute).
        /// </summary>
        public string ApplicableForTypesString { get; set; }

        /// <summary>
        /// Gets or sets the name types strings (for the data- attribute).
        /// </summary>
        public string NamesForTypes { get; set; }
    }
}