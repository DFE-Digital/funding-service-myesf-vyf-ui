using System;

namespace PDS.ViewYourFunding.Web.Helpers
{
    /// <summary>
    /// Helper class for URI related modifications.
    /// </summary>
    public static class UriHelper
    {
        /// <summary>
        /// Uri Helper to add a new parameter.
        /// </summary>
        /// <param name="inputUri">The input uri.</param>
        /// <param name="queryToAppend">The parameter.</param>
        /// <returns>The new Uri.</returns>
        public static string BuildUri(string inputUri, string queryToAppend)
        {
            var uriBuilder = new UriBuilder(inputUri);

            if (uriBuilder.Query != null && uriBuilder.Query.Length > 1)
            {
                uriBuilder.Query = uriBuilder.Query.Substring(1) + "&" + queryToAppend;
            }
            else
            {
                uriBuilder.Query = queryToAppend;
            }

            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}