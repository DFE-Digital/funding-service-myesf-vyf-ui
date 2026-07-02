using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Interfaces.Models;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// Tasks to do with creating components.
    /// </summary>
    public interface IComponentFactory
    {
        /// <summary>
        /// Create a simple component.
        /// </summary>
        /// <param name="type">The type of component.</param>
        /// <param name="value">A value for the component.</param>
        /// <returns>A new component.</returns>
        public Component CreateSimple(ComponentType type, object value);
    }
}