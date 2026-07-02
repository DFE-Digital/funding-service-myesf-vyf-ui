namespace PDS.VYF.Services.Config
{
    using Autofac;

    public class NewServicesAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.ThisAssembly).AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
