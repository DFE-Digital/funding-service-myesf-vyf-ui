using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi;
using Pds.Core.Common.Identity.Constants;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.DfESignIn;
using Pds.Core.Identity.Claims.Registration;
using Pds.Core.Logging;
using Pds.Core.SecurityAssurances.Middlewares;
using Pds.Core.SecurityAssurances.Middlewares.Options;
using Pds.Core.Telemetry.ApplicationInsights;
using Pds.Core.Utils;
using Pds.Core.Web.Filters;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Repositories.Config;
using PDS.ViewYourFunding.Repositories.Implementations;
using PDS.ViewYourFunding.Repositories.Migrations;
using PDS.ViewYourFunding.Services.Binders;
using PDS.ViewYourFunding.Services.Config;
using PDS.ViewYourFunding.Services.DependencyInjection;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Binders;
using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Attributes;
using PDS.ViewYourFunding.Web.Attributes;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Extensions;
using PDS.ViewYourFunding.Web.Filters;
using PDS.ViewYourFunding.Web.Helpers;
using PDS.ViewYourFunding.Web.Interfaces;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using PDS.VYF.Services.ServiceRegistrations;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web
{
    /// <summary>
    /// The startup Class.
    /// </summary>
    public class Startup
    {
        public static string PathPrefix { get; set; }

        public static string UrlForLoggedInProvider { get; set; }

        public static string RouteStart { get; set; }

        private const string CurrentApiVersion = "v1.0.0";
        private const string ApiTitle = "View your funding global settings API.";
        private const string NewBaseAddress = "/view-latest-funding";
        private const string OldBaseAddress = "/single-funding-statement/latest";

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationConfiguration>(options => { Configuration.Bind(options); });

            services.Configure<Authentication>(options => { Configuration.Bind(nameof(Authentication), options); });

            services.Configure<CosmosDbConfiguration>(options =>
            {
                Configuration.Bind(nameof(CosmosDbConfiguration), options);
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedProto |
                    ForwardedHeaders.XForwardedHost;
            });

            services.AddPdsApplicationInsightsTelemetry(BuildAppInsightsConfiguration);
            services.AddControllersWithViews(options =>
                {
                    options.Filters.Add<CookiePreferencesActionFilterAttribute>();
                });

            services.AddControllersWithViews(options =>
            {
                options.ModelBinderProviders.Insert(0, new QueryFilterBinderProvider());
                options.ModelBinderProviders.Insert(1, new DataValueEditorBinderProvider());
                options.ModelBinderProviders.Insert(2, new FilterTypeBinderProvider());
            });

            services.AddSingleton(Task.Run(InitializeCosmosClientInstanceAsync).GetAwaiter().GetResult());
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

            TokenAuthorizeAttribute.ExpectedAuthKey = Configuration["RequestAuthorisationKey"];
            services.AddScoped<ProviderViewToggledCheckAttribute>();
            services.AddScoped<MultipleAcademyTrustViewCheckAttribute>();

            services.AddDfESignInAuthentication(options => Configuration.Bind("DfESignIn", options));

            services
                .AddClaimsBasedIdentityService(options => Configuration.Bind($"{nameof(Services)}:{nameof(ServicesConfiguration.AdminApiClient)}", options));

            services.AddDbContext<Context>(
                options =>
                {
                    options.UseSqlServer(
                        Configuration.GetConnectionString("vyf"),
                        sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(
                                10,
                                TimeSpan.FromSeconds(30),
                                null);
                        });
                }, ServiceLifetime.Transient);

            services.AddLoggerAdapter();
            services.AddHostedService<QueuedHostedService>();

            AddPolicies(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(CurrentApiVersion, new OpenApiInfo { Title = ApiTitle, Version = CurrentApiVersion });
            });
            services.AddServicesConfiguration(options => Configuration.Bind(nameof(Services), options));
            services.AddPdsUtils();

            services.RegisterInfraServices();
            services.RegisterAppServices();
            services.AddSession(opt =>
            {
                opt.Cookie.IsEssential = true;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<RepositoriesAutofacModule>();
            builder.RegisterModule<ServicesAutofacModule>();
            builder.RegisterModule<WebAutofacModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute(ViewYourFundingConstants.Route_ErrorPage);
                app.UseExceptionHandler(ViewYourFundingConstants.Route_ErrorPage);

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseMiddleware<SecurityHeadersMiddleware>(Options.Create(
                new SecurityHeadersOptions
                {
                    ContentSecurityPolicyDirectives = "default-src 'self'; script-src 'self' 'unsafe-inline' www.google-analytics.com *.cloudflare.com *.fontawesome.com https://*.clarity.ms https://c.bing.com; script-src-elem 'self' 'unsafe-inline' www.google-analytics.com *.cloudflare.com *.fontawesome.com https://*.clarity.ms https://c.bing.com; style-src 'self' 'unsafe-inline' *.cloudflare.com *.fontawesome.com; font-src 'self' *.cloudflare.com *.fontawesome.com data:; connect-src 'self' www.google-analytics.com *.fontawesome.com https://*.clarity.ms https://c.bing.com; img-src 'self' www.google-analytics.com https://*.clarity.ms https://c.bing.com"
                }));
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{CurrentApiVersion}/swagger.json", ApiTitle);
            });

            var culture = new CultureInfo(EditTypeConstants.EnGbCultureName);
            var supportedCultures = new[] { culture };

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture),

                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,

                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            // Setup the DB Migrations and Data seeding.
            DatabaseConfiguration.Initialize(app.ApplicationServices);

            var useNewUrl = Task.Run(() => UseNewUrl(app.ApplicationServices)).GetAwaiter().GetResult();

            // WAF in PreProd and Prod expects the assets on this request path
            if (useNewUrl)
            {
                app.UsePathBase(NewBaseAddress);
                PathPrefix = $"{NewBaseAddress}/";
                UrlForLoggedInProvider = "/view-latest-funding/pre-16-16-19-statements";
                RouteStart = string.Empty;

                app.UseVyfRedirector();
            }
            else
            {
                app.UsePathBase(OldBaseAddress);
                PathPrefix = $"{OldBaseAddress}/";
                UrlForLoggedInProvider = "/single-funding-statement/latest/logged-in-provider-statement";
                RouteStart = "start";
            }

            ProviderViewToggledCheckAttribute.ProviderViewToggledOn = Task
                .Run(() => LoggedInViewAvailable(app.ApplicationServices, 4)).GetAwaiter().GetResult();
            MultipleAcademyTrustViewCheckAttribute.MultipleAcademyTrustViewToggledOn = Task
                .Run(() => LoggedInViewAvailable(app.ApplicationServices, 8)).GetAwaiter().GetResult();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            PreventDefaultNoCachingCacheHeaders(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }

        private static async Task<bool> LoggedInViewAvailable(IServiceProvider serviceProvider, int type)
        {
            using var serviceScope = serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<Context>();
            var setting = await context.GlobalSettings.FirstOrDefaultAsync(s => s.Type == type);

            return setting != null && setting.Value.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase);
        }

        private static async Task<bool> UseNewUrl(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<Context>();
                var setting = await context.GlobalSettings.FirstOrDefaultAsync(s => s.Type == 5);

                if (setting != null)
                {
                    return setting.Value.Equals(NewBaseAddress, StringComparison.OrdinalIgnoreCase);
                }
            }

            return false;
        }

        private void AddPolicies(IServiceCollection services)
        {
            PolicyConstants.PolicyInformation.ForEach(policyInfo =>
            {
                services.AddAuthorization(options =>
                    options.AddPolicy(policyInfo.Name, policy =>
                        policy.Requirements.Add(new UserRoleAuthorizationRequirement(policyInfo.UserRoles))));
            });

            services.AddSingleton<IAuthorizationHandler, UserRoleAuthorizationHandler>();
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, UserRoleAuthorizationMiddlewareResultHandler>();
            services.AddSingleton<IEncryptionService, EncryptionService>();
            services.AddSingleton<IUserRoleAuthorizationService, UserRoleAuthorizationService>();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(nameof(Pds.Core.Identity.Claims.Constants.Roles.SfsAdminRole), policy =>
            //        policy.RequireClaim(Pds.Core.Identity.Claims.Constants.PdsClaimTypes.Role, Pds.Core.Identity.Claims.Constants.Roles.SfsAdminRole));
            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(nameof(UserRole.ViewAllocationStatements), policy =>
            //        policy.RequireAssertion(context =>
            //            HasClaim(context.User, UserRole.ViewAsProvider) ||
            //            HasClaim(context.User, UserRole.ViewAllocationStatements)));
            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(nameof(UserRole.ViewRecoupmentReports), policy =>
            //        policy.RequireAssertion(context =>
            //            HasClaim(context.User, UserRole.ViewAsProvider) ||
            //            HasClaim(context.User, UserRole.ViewRecoupmentReports)));
            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(PolicyConstants.AllocationsAdministrator, policy =>
            //        policy.RequireAssertion(context =>
            //            HasClaimMatching(context.User, Pds.Core.Identity.Claims.Constants.Roles.SfsAdminRole) ||
            //             HasClaimStartingWith(context.User, $"{PolicyConstants.AllocationsAdministrator}_")));
            //});
        }

        private bool HasClaim(ClaimsPrincipal user, UserRole role)
        {
            return user.HasClaim(PdsClaimTypes.Role, role.ToString());
        }

        private bool HasClaimMatching(ClaimsPrincipal user, string role)
        {
            return user.Claims.Any(claim => claim.Value.Equals(role, StringComparison.InvariantCultureIgnoreCase));
        }

        private bool HasClaimStartingWith(ClaimsPrincipal user, string roleSuffix)
        {
            return user.Claims.Any(claim => claim.Value.StartsWith(roleSuffix, StringComparison.InvariantCultureIgnoreCase));
        }

        private void PreventDefaultNoCachingCacheHeaders(IApplicationBuilder app)
        {
            app.Use(async (context, nextMiddleware) =>
            {
                context.Response.OnStarting(() =>
                {
                    var headers = context.Response.Headers;

                    var cacheControlSetToDefaultNoCaching =
                        headers.ContainsKey(HeaderNames.CacheControl)
                        && headers[HeaderNames.CacheControl] == "no-cache, no-store"
                        && headers.ContainsKey(HeaderNames.Pragma)
                        && headers[HeaderNames.Pragma] == "no-cache";

                    if (cacheControlSetToDefaultNoCaching)
                    {
                        headers.Remove(HeaderNames.Pragma);
                        headers[HeaderNames.CacheControl] = "private";
                    }

                    return Task.FromResult(0);
                });

                await nextMiddleware();
            });
        }

        private async Task<CosmosClient> InitializeCosmosClientInstanceAsync()
        {
            var appConfig = new ApplicationConfiguration();
            Configuration.Bind(appConfig);
            var clientBuilder = new Microsoft.Azure.Cosmos.Fluent.CosmosClientBuilder(appConfig.CosmosDbConfiguration.ConnectionString);

            CosmosClient client;

            if (string.Equals(appConfig.CosmosDbConfiguration.CosmosConnectionMode, "gateway", StringComparison.OrdinalIgnoreCase))
            {
                client = clientBuilder
                            .WithConnectionModeGateway()
                            .Build();
            }
            else
            {
                client = clientBuilder
                            .WithConnectionModeDirect()
                            .Build();
            }

            var database = await client.CreateDatabaseIfNotExistsAsync(appConfig.CosmosDbConfiguration.DatabaseName);
            foreach (var collectionName in appConfig.CosmosDbConfiguration.CollectionNames)
            {
                await database.Database.CreateContainerIfNotExistsAsync(collectionName, "/id");
            }

            LayoutModel.LayoutCollectionName = appConfig.CosmosDbConfiguration.LayoutCollection;
            DataImportAuditModel.AuditCollectionName = appConfig.CosmosDbConfiguration.AuditCollection;
            ProviderFundingModel.ProviderFundingCollectionName = appConfig.CosmosDbConfiguration.ProviderFundingCollection;

            return client;
        }

        private void BuildAppInsightsConfiguration(PdsApplicationInsightsConfiguration options)
        {
            Configuration.Bind("PdsApplicationInsights", options);
            options.Component = this.GetType().Assembly.GetName().Name;
        }
    }
}