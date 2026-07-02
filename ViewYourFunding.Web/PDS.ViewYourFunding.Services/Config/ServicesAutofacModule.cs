using Autofac;
using Microsoft.Extensions.Options;
using Pds.Core.ApiClient.Services;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Interfaces;
using System;
using System.Net.Http;
using System.Reflection;
using Module = Autofac.Module;

namespace PDS.ViewYourFunding.Services.Config
{
    /// <summary>
    /// The services autofac module.
    /// </summary>
    /// <seealso cref="Module" />
    public class ServicesAutofacModule : Module
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.Register(c =>
            {
                var configuration = c.Resolve<IOptions<ApplicationConfiguration>>().Value;
                var globalCacheTimeToLive = configuration.GlobalCacheTimeToLive;

                return new MemoryCacheService(
                    c.Resolve<ILoggerAdapter<MemoryCacheService>>(),
                    globalCacheTimeToLive);
            }).As<ICacheService>().SingleInstance();

            builder.Register(c =>
            {
                var configuration = c.Resolve<IOptions<ApplicationConfiguration>>().Value;

                return new AzureBlobStorageFundingDocumentService(
                    configuration.BlobStorage.ServiceName,
                    configuration.BlobStorage.Key,
                    configuration.BlobStorage.ContainerName,
                    c.Resolve<ILoggerAdapter<AzureBlobStorageFundingDocumentService>>());
            }).As<IFundingDocumentStorageService>().SingleInstance();

            builder.Register(c =>
            {
                var path = System.IO.Path.GetDirectoryName(
                  Assembly.GetExecutingAssembly().Location).Replace(@"file:\", string.Empty);

                return new LocalModelFileStoreService(path);
            }).As<IModelFileStoreService>().SingleInstance();

            builder.Register(c =>
            {
                var configuration = c.Resolve<IOptions<ApplicationConfiguration>>().Value;
                var totalSeconds = configuration.GenerateSpreadsheetTimeoutSeconds;

                return new GenerateSpreadsheetService(
                    c.Resolve<ILoggerAdapter<GenerateSpreadsheetService>>(),
                    new HttpClient
                    {
                        Timeout = new TimeSpan(0, 0, totalSeconds)
                    },
                    configuration);
            }).AsImplementedInterfaces().SingleInstance();

            builder.Register(c => new FundingUiModelDetailsService(
                c.Resolve<ILoggerAdapter<FundingUiModelDetailsService>>(),
                new HttpClient(),
                c.Resolve<IOptions<ApplicationConfiguration>>())).AsImplementedInterfaces().SingleInstance();

            builder.RegisterGeneric(typeof(HttpSessionProvider<>)).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.Register(c =>
            {
                var aspose = new AsposeDocumentManagementService(c.Resolve<ILoggerAdapter<AsposeDocumentManagementService>>());
                aspose.EnableCellsLicense();

                return aspose;
            }).AsImplementedInterfaces().SingleInstance();

            builder.Register(c => new BackgroundTaskQueue()).AsImplementedInterfaces().SingleInstance();

            builder.RegisterGeneric(typeof(CosmosDbService<>))
                .AsImplementedInterfaces();

            builder.RegisterType<FundingApiService>().As<IFundingApiService>();
            builder.RegisterGeneric(typeof(AuthenticationService<>)).AsImplementedInterfaces().InstancePerDependency();
            builder.Register(c => new HttpClient())
                .As<HttpClient>();
        }
    }
}