using Autofac;

namespace PDS.ViewYourFunding.Repositories.Config
{
    /// <summary>
    /// The Repositories autofac module.
    /// </summary>
    /// <seealso cref="Module" />
    public class RepositoriesAutofacModule : Module
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
        }
    }
}