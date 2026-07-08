using Microsoft.AspNetCore.Rewrite;
using System.Text;
using System.Text.RegularExpressions;

namespace PDS.ViewYourFunding.Web.Models.Helpers
{
    /// <summary>
    /// Adapted from https://stackoverflow.com/questions/46701670/net-core-https-with-aws-load-balancer-and-elastic-beanstalk-doesnt-work.
    /// </summary>
    public class RedirectToProxiedHttpsRule : IRule
    {
        private readonly string regex;
        private readonly string replacement;
        private readonly int statusCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectToProxiedHttpsRule"/> class.
        /// </summary>
        /// <param name="regex">The regex pattern to match.</param>
        /// <param name="replacement">The replacement pattern to use.</param>
        /// <param name="statusCode">The status code (301 or 302).</param>
        public RedirectToProxiedHttpsRule(string regex, string replacement, int statusCode)
        {
            this.regex = regex;
            this.replacement = replacement;
            this.statusCode = statusCode;
        }

        /// <summary>
        /// Apply a redirect rule.
        /// </summary>
        /// <param name="context">RewriteContext.</param>
        public virtual void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;

            var path = (request.PathBase + request.Path).ToString().Substring(1);
            var regexMatches = Regex.Match(path, regex);

            if (!regexMatches.Success)
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            var redirectPath = Regex.Replace(path, regex, replacement);

            var redirectUrl = new StringBuilder()
                .Append(redirectPath)
                .Append(request.QueryString)
                .ToString();

            context.Result = RuleResult.EndResponse;

            var isPermanent = statusCode == 301;
            context.HttpContext.Response.Redirect(redirectUrl, isPermanent);
        }
    }
}