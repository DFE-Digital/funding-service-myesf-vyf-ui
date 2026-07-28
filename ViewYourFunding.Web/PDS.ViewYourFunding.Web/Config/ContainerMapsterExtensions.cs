using Autofac;
using Mapster;
using MapsterMapper;
using PDS.ViewYourFunding.Web.Extensions;

namespace PDS.ViewYourFunding.Web.Config
{
    public static class ContainerMapsterExtensions
    {
        public static void RegisterMapster(this ContainerBuilder builder)
        {
            var config = new TypeAdapterConfig();

            config.Configure();

            builder.RegisterInstance(config)
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<ServiceMapper>()
                .As<IMapper>()
                .SingleInstance();
        }
    }
}