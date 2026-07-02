using PDS.ViewYourFunding.Services.Enums;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// The filter class.
    /// </summary>
    public class UiModelDataSetFilter
    {
        /// <summary>
        /// Gets or sets the property Name.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        public Operator Operation { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object Value { get; set; }
    }
}
