using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// Service to talk to an api endpoint over HTTP.
    /// </summary>
    public interface IHttpApiService
    {
        /// <summary>Issues a GET request to return a single result.</summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="queryStringParams">The query string params.</param>
        /// <returns>A single result.</returns>
        Task<T> GetRequestSingleResult<T>(string queryStringParams);

        /// <summary>Posts a request to a service.</summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="url">The url.</param>
        /// <param name="formData">Form data parameters to post to the endpoint.</param>
        /// <param name="requestType">The request type (default is "application/x-www-form-urlencoded").</param>
        /// <returns>The return type.</returns>
        Task<T> PostRequest<T>(string url, string formData, string requestType = "application/x-www-form-urlencoded");

        /// <summary>
        /// Get the string result of making a request to the specified url.
        /// </summary>
        /// <param name="url">The url to request.</param>
        /// <returns>A string containing the response content.</returns>
        Task<string> GetResponse(string url);

        /// <summary>Posts a request to a service.</summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="url">The url.</param>
        /// <param name="formData">Form data parameters to post to the endpoint.</param>
        /// <param name="requestType">The request type (default is "application/x-www-form-urlencoded").</param>
        /// <returns>The return type.</returns>
        Task<T> PostRequestToUserFundingView<T>(string url, string formData, string requestType = "application/x-www-form-urlencoded");

        /// <summary>
        /// Get the bool result of making a request to the specified url.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="url">The url to request.</param>
        /// <returns>A bool containing the response content.</returns>
        Task<T> GetResponseFromUserFundingView<T>(string url);
    }
}