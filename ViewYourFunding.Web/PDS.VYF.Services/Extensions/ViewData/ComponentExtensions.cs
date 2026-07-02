namespace PDS.VYF.Services.Extensions.ViewData
{
    using PDS.ViewYourFunding.Services.Interfaces.Models;

    /// <summary>
    /// Extension methods for the Component class.
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// Gets all components recursively from the given component.
        /// </summary>
        /// <param name="component">The component to start from.</param>
        /// <returns>All components including nested components.</returns>
        public static IEnumerable<Component> GetAllComponents(this Component component)
        {
            return component.Components.Concat(component.Components.SelectMany(n => n.GetAllComponents()));
        }

        /// <summary>
        /// Gets all components recursively from the given collection of components.
        /// </summary>
        /// <param name="components">The collection of components to start from.</param>
        /// <returns>All components including nested components.</returns>
        public static IEnumerable<Component> GetAllComponents(this IEnumerable<Component> components)
        {
            return components.SelectMany(n => n.GetAllComponents());
        }
    }
}
