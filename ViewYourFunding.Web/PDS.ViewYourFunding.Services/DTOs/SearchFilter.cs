using PDS.ViewYourFunding.Services.Enums;

namespace PDS.ViewYourFunding.Services.DTOs
{
    /// <summary>
    /// An optional filter to restrict the search by.
    /// </summary>
    public class SearchFilter
    {
        /// <summary>
        /// Gets or sets the search property name.
        /// </summary>
        public SearchFilterPropertyName PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the property value to filter by.
        /// </summary>
        public string PropertyValue { get; set; }
    }
}