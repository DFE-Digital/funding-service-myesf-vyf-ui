namespace PDS.VYF.Services.ServiceRegistrations
{
    using Microsoft.Extensions.DependencyInjection;
    using PDS.VYF.Services.Abstracts.AppServices;
    using PDS.VYF.Services.Implementations.AppServices;

    /// <summary>
    /// The application services registrations.
    /// </summary>
    public static class AppServicesRegistrations
    {
        /// <summary>
        /// Registers the application services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The same service object.</returns>
        public static IServiceCollection RegisterAppServices(this IServiceCollection services)
        {
            services.AddScoped<ILoggedInApiServices, LoggedInApiServices>();
            services.AddScoped<ISharedFundingViewServices, SharedFundingViewServices>();
            services.AddScoped<IChildFundingViewServices, ChildFundingViewServices>();
            services.AddScoped<IParentFundingViewServices, ParentFundingViewServices>();
            services.AddScoped<IChildOrParentNameServices, ChildOrParentNameServices>();

            return services;
        }
    }
}
