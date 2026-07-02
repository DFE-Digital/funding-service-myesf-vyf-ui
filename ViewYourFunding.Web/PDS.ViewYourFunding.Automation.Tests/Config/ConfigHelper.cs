using Microsoft.Extensions.Configuration;
using PDS.ViewYourFunding.Core.Configuration;

namespace PDS.ViewYourFunding.Automation.Tests.Config
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

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        /// <returns>The Application configuration.</returns>
        public static ApplicationConfiguration GetApplicationConfiguration()
        {
            var config = GetIConfigurationRoot();

            var applicationConfig = new ApplicationConfiguration();
            config.Bind(applicationConfig);

            return applicationConfig;
        }
    }
}
