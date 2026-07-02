using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Interfaces
{
    /// <summary>
    /// An interface to describe a razor view renderer.
    /// </summary>
    public interface IRazorViewToStringRenderer
    {
        /// <summary>
        /// Render a view to a string.
        /// </summary>
        /// <typeparam name="TModel">The returned type.</typeparam>
        /// <param name="viewName">The name of the view to fetch.</param>
        /// <param name="model">The return type.</param>
        /// <returns>A string, wrapped in a task.</returns>
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }
}