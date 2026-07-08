using Autofac;
using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace PDS.ViewYourFunding.Web.Config
{
    /// <summary>
    /// The Container Automapper extensions class.
    /// </summary>
    public static class ContainerAutoMapperExtensions
    {
        /// <summary>
        /// Registers the automatic mapper maps.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void RegisterAutoMapperMaps(this ContainerBuilder builder)
        {
            var assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                .Where(assembly => assembly.Name.StartsWith("PDS.ViewYourFunding.", StringComparison.InvariantCultureIgnoreCase));

            var localAutoMapperProfileTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(Profile).IsAssignableFrom(type) && type.IsPublic && !type.IsAbstract)
                .Distinct();

            var assembliesAutoMapperProfileTypes = assemblyNames
                .SelectMany(assemblyName => Assembly.Load(assemblyName).GetTypes())
                .Where(type => typeof(Profile).IsAssignableFrom(type) && type.IsPublic && !type.IsAbstract)
                .Distinct();

            var autoMapperProfiles = assembliesAutoMapperProfileTypes.Union(localAutoMapperProfileTypes)
                .Select(type => (Profile)Activator.CreateInstance(type)).ToList();

            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                foreach (var profile in autoMapperProfiles)
                {
                    cfg.AddProfile(profile);
                }
            }));

            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>().SingleInstance();
        }
    }
}