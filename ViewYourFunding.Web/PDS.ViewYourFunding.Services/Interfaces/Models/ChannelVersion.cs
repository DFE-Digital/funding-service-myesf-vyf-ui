using Newtonsoft.Json;

namespace PDS.ViewYourFunding.Services.Interfaces.Models
{
    /// <summary>
    /// Model to map channel version data.
    /// </summary>
    public class ChannelVersion
    {
        /// <summary>
        /// Gets or sets the channel version type.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the channel version value.
        /// </summary>
        [JsonProperty("value")]
        public int Value { get; set; }
    }
}
