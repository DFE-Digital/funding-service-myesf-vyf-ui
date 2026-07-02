using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using PDS.ViewYourFunding.Web.Helpers;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Extensions
{
    /// <summary>
    /// Extension class for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        private static readonly IDictionary<string, string> RedirectRules = new Dictionary<string, string>
        {
            { @"^single-funding-statement/latest/start", "/view-latest-funding" },
            { @"^single-funding-statement/latest(\??.*)?", "/view-latest-funding$1" },
            { @"^single-funding-statement", "view-latest-funding" },
        };

        /// <summary>
        /// Uses the VYF redirector.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>Instance of <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseVyfRedirector(this IApplicationBuilder builder)
        {
            var rewriteOptions = new RewriteOptions();

            foreach (var redirectRules in RedirectRules)
            {
                rewriteOptions = rewriteOptions.AddRedirectToProxiedHttps(
                    redirectRules.Key,
                    redirectRules.Value,
                    StatusCodes.Status301MovedPermanently);
            }

            builder.UseRewriter(rewriteOptions);
            return builder;
        }
    }
}