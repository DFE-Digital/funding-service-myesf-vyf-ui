using Newtonsoft.Json;

namespace PDS.ViewYourFunding.Services.DTOs
{
    /// <summary>
    /// A dropdown box to display.
    /// </summary>
    public class DropDown
    {
        /// <summary>
        /// Gets or sets the input range of cells.
        /// </summary>
        [JsonProperty("inputRange")]
        public string InputRange { get; set; }

        /// <summary>
        /// Gets or sets the top offset for the dropdown box.
        /// </summary>
        [JsonProperty("top")]
        public int? Top { get; set; }

        /// <summary>
        /// Gets or sets a left offset for the dropdown box.
        /// </summary>
        [JsonProperty("left")]
        public int? Left { get; set; }

        /// <summary>
        /// Gets or sets height of the dropdown box.
        /// </summary>
        [JsonProperty("height")]
        public int? Height { get; set; }

        /// <summary>
        /// Gets or sets the width of the dropdown box.
        /// </summary>
        [JsonProperty("width")]
        public int? Width { get; set; }
    }
}