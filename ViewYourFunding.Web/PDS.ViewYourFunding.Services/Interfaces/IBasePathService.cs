namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// BasePath Service.
    /// </summary>
    public interface IBasePathService
    {
        /// <summary>
        /// Gets the application base path.
        /// </summary>
        /// <returns>The base path.</returns>
        string GetApplicationBasePath();

        /// <summary>
        /// Gets the logged in provider path.
        /// </summary>
        /// <returns>The logged in provider path.</returns>
        string GetUrlForLoggedInProviderPath();
    }
}