using Newtonsoft.Json;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A dropdown box to display.
    /// </summary>
    public class UiModelDropDown
    {
        /// <summary>
        /// Gets or sets the input range of cells (spreadsheet only).
        /// </summary>
        public string InputRange { get; set; }

        /// <summary>
        /// Gets or sets the top offset for the dropdown box.
        /// </summary>
        [JsonProperty("top")]
        public int? TopOffset { get; set; }

        /// <summary>
        /// Gets or sets a left offset for the dropdown box.
        /// </summary>
        [JsonProperty("left")]
        public int? LeftOffset { get; set; }

        /// <summary>
        /// Gets or sets height of the dropdown box.
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Gets or sets the width of the dropdown box.
        /// </summary>
        public int? Width { get; set; }
    }
}