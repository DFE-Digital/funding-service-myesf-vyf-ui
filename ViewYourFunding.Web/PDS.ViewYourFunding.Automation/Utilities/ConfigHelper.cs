using Microsoft.Extensions.Configuration;

namespace ViewYourFunding.Automation.Utilities
{
    /// <summary>
    /// The Config helper.
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// Gets the i configuration root.
        /// </summary>
        /// <returns>The configuration.</returns>
        public static IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
