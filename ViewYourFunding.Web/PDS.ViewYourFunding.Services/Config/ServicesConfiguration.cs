namespace PDS.ViewYourFunding.Services.Config
{
    /// <summary>
    /// Structure to hold configuration for the service layer.
    /// </summary>
    public class ServicesConfiguration
    {
        /// <summary>
        /// Gets or sets the organisation API client configuration.
        /// </summary>
        /// <value>
        /// The organisation API client configuration.
        /// </value>
        public OrganisationApiClientConfiguration OrganisationApiClient { get; set; }

        /// <summary>
        /// Gets or sets the admin API client.
        /// </summary>
        /// <value>
        /// The admin API client.
        /// </value>
        public AdminApiClientConfiguration AdminApiClient { get; set; }
    }
}