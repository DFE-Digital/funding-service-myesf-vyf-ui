using Mapster;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring Mapster mappings
    /// used by the Services layer.
    /// </summary>
    public static class ServicesTypeAdapterConfigExtensions
    {
        /// <summary>
        /// Registers all Mapster mappings between repository data models
        /// and service models.
        /// </summary>
        /// <param name="config">
        /// The <see cref="TypeAdapterConfig"/> instance to configure.
        /// </param>
        /// using Mapster;
        public static void ConfigureServicesMappings(this TypeAdapterConfig config)
        {
            TypeAdapterConfig.GlobalSettings.AllowImplicitSourceInheritance = true;

            config.Default.AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);
            config.Default.PreserveReference(true);

            config.NewConfig<Repositories.DataModels.FundingStream, FundingStream>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.PublicationLayout, PublicationLayout>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.Publication, Publication>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.Setting, Setting>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.SettingValue, SettingValue>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.NextPaymentType, NextPaymentType>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.GlobalSetting, GlobalSetting>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.Setting, SettingType>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.NextPayment, NextPayment>()
                .Map(
                    dest => dest.NextPaymentTypeDescription,
                    src => src.NextPaymentType.Description)
                .Map(
                    dest => dest.NextPaymentTypeCode,
                    src => src.NextPaymentType.TypeCode)
                .TwoWays();
        }
    }
}
