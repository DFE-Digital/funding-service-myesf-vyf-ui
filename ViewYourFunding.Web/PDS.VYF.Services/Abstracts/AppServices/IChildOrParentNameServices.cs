namespace PDS.VYF.Services.Abstracts.AppServices
{
    /// <summary>
    /// Represents a service for retrieving the name of a parent or child based on the UKPRN.
    /// </summary>
    public interface IChildOrParentNameServices
    {
        /// <summary>
        /// Retrieves the name of a parent or child based on the UKPRN.
        /// </summary>
        /// <param name="ukprn">The UKPRN to retrieve the name for.</param>
        /// <returns>The name of the parent or child, or null if not found.</returns>
        Task<string?> GetParentOrChildName(string ukprn);
    }
}
