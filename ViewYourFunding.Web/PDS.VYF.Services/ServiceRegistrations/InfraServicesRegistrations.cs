namespace PDS.VYF.Services.ServiceRegistrations
{
    using Microsoft.Extensions.DependencyInjection;
    using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
    using PDS.VYF.Services.Abstracts.InfraServices.FilesServices;
    using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
    using PDS.VYF.Services.Implementations.InfraServices.DataApiClientServices;
    using PDS.VYF.Services.Implementations.InfraServices.FilesServices;
    using PDS.VYF.Services.Implementations.InfraServices.SettingsServices;

    /// <summary>
    /// The infra services registrations.
    /// </summary>
    public static class InfraServicesRegistrations
    {
        /// <summary>
        /// Registers the infra services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The same service object.</returns>
        public static IServiceCollection RegisterInfraServices(this IServiceCollection services)
        {
            services.AddScoped<IParentApiClientServices, ParentApiClientServices>();
            services.AddScoped<IChildApiClientServices, ChildApiClientServices>();
            services.AddScoped<IUserCountApiClientServices, UserCountApiClientServices>();
            services.AddScoped<IUIModelFilesServices, UIModelFilesServices>();

            services.AddSingleton<IFundingStreamSettingsServices, FundingStreamSettingsServices>();
            services.AddSingleton<IGlobalSettingsService, GlobalSettingsService>();

            return services;
        }
    }
}
