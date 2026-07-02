namespace PDS.ViewYourFunding.Web
{
    /// <summary>
    /// Endpoint configuration for a webserver.
    /// </summary>
    public class EndpointConfiguration
    {
        /// <summary>
        /// Gets or sets the host (e.g. myesf.local).
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port (e.g. null or 80).
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// Gets or sets the scheme (e.g. http or https).
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Gets or sets the store name to lookup certs in (e.g. MY).
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// Gets or sets the store location (e.g. CurrentUser).
        /// </summary>
        public string StoreLocation { get; set; }

        /// <summary>
        /// Gets or sets the file path to a cert (optional).
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the password for a cert (optional).
        /// </summary>
        public string Password { get; set; }
    }
}