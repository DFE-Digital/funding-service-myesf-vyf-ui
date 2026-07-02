using Microsoft.AspNetCore.Rewrite;
using PDS.ViewYourFunding.Web.Models.Helpers;

namespace PDS.ViewYourFunding.Web.Helpers
{
    /// <summary>
    ///  Adapted from https://stackoverflow.com/questions/46701670/net-core-https-with-aws-load-balancer-and-elastic-beanstalk-doesnt-work.
    /// </summary>
    public static class RedirectToProxiedHttpsExtensions
    {
        /// <summary>
        /// Add redirect to proxied https.
        /// </summary>
        /// <param name="options">A rewriteoptons object.</param>
        /// <param name="regex">The input regex.</param>
        /// <param name="replacement">The replacement pattern.</param>
        /// <param name="statusCode">The status code to return.</param>
        /// <returns>A rewriteoptions object.</returns>
        public static RewriteOptions AddRedirectToProxiedHttps(
            this RewriteOptions options,
            string regex,
            string replacement,
            int statusCode)
        {
            options.Rules.Add(new RedirectToProxiedHttpsRule(regex, replacement, statusCode));
            return options;
        }
    }
}