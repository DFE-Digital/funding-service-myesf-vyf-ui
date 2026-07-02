using Microsoft.Extensions.Configuration;
using PDS.ViewYourFunding.Core.Configuration;

namespace PDS.ViewYourFunding.Services.Tests.Config
{
    /// <summary>
    /// The Configuration helper.
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// Gets the configuration root.
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