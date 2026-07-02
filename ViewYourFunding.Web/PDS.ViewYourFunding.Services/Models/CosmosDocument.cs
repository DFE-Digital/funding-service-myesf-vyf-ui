using Newtonsoft.Json;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// The cosmos document base class.
    /// </summary>
    public abstract class CosmosDocument
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        /// PLEASE NOTE: Left the json property on it this is the partition key property
        /// Without this the add/update operation will fail.
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets the name of the collection.
        /// </summary>
        /// <value>
        /// The name of the collection.
        /// </value>
        public abstract string CollectionName { get; }
    }
}