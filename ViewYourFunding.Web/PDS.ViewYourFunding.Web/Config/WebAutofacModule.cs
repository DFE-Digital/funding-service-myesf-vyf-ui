using Autofac;
using Module = Autofac.Module;

namespace PDS.ViewYourFunding.Web.Config
{
    /// <summary>
    /// The services autofac module.
    /// </summary>
    /// <seealso cref="Module" />
    public class WebAutofacModule : Module
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
            builder.RegisterAutoMapperMaps();
            builder.RegisterTileDependencies();
        }
    }
}