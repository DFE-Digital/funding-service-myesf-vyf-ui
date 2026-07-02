using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Web.Tests
{
    /// <summary>
    /// The Configuration helper.
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

        /// <summary>
        /// Get the database connection string.
        /// </summary>
        /// <param name="name">Connection string name.</param>
        /// <returns>Dat5abase connection string.</returns>
        public static string GetConnectionString(string name)
        {
            var config = GetIConfigurationRoot();
            return config.GetConnectionString(name);
        }

        /// <summary>
        /// Setup the DbContextOptions.
        /// </summary>
        /// <param name="name">Connection string name.</param>
        /// <returns>Returns the database context options.</returns>
        public static DbContextOptions<Context> DbContextOptions(string name)
        {
            var connectionString = ConfigHelper.GetConnectionString(name);

            return new DbContextOptionsBuilder<Context>()
                .UseSqlServer(connectionString)
                .Options;
        }
    }
}