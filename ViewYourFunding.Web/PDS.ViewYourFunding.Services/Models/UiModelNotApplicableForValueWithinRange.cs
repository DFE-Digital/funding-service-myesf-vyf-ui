using Newtonsoft.Json;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Not applicable for the range within limit class.
    /// </summary>
    public class UiModelNotApplicableForValueWithinRange
    {
        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        [JsonProperty("minimum")]
        public int? Minimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        [JsonProperty("maximum")]
        public int? Maximum { get; set; }
    }
}