using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PDS.ViewYourFunding.Services.Config;
using System;

namespace PDS.ViewYourFunding.Services.DependencyInjection
{
    /// <summary>
    /// Extensions class for <see cref="IServiceCollection"/> for registering the feature's services.
    /// </summary>
    public static class FeatureServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services for the current feature to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the feature's services to.</param>
        /// <param name="configureOptions">An action that will hydrate an instance.</param>
        public static void AddServicesConfiguration(
            this IServiceCollection services,
            Action<ServicesConfiguration> configureOptions)
        {
            AddConfiguration(services, configureOptions);
            services.Configure<OrganisationApiClientConfiguration>(options =>
            {
                var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                config.Bind($"{nameof(Services)}:{nameof(ServicesConfiguration.OrganisationApiClient)}", options);
            });
            services.Configure<AdminApiClientConfiguration>(options =>
            {
                var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                config.Bind($"{nameof(Services)}:{nameof(ServicesConfiguration.AdminApiClient)}", options);
            });
        }

        private static void AddConfiguration(
            IServiceCollection services,
            Action<ServicesConfiguration> configureOptions)
        {
            services.Configure(configureOptions);
        }
    }
}