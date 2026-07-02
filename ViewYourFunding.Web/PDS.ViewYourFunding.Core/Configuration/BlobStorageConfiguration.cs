namespace PDS.ViewYourFunding.Core.Configuration
{
    /// <summary>
    /// The class representing the Blob Storage Configuration.
    /// </summary>
    public class BlobStorageConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the name of the container.
        /// </summary>
        /// <value>
        /// The name of the container.
        /// </value>
        public string ContainerName { get; set; }
    }
}
