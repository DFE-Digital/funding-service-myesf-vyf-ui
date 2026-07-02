using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Filters
{
    /// <summary>
    /// The RoleAuthorize Policy Provider.
    /// </summary>
    /// <seealso cref="IAuthorizationPolicyProvider" />
    internal class RoleAuthorizePolicyProvider : IAuthorizationPolicyProvider
    {
        private const string POLICY_PREFIX = "RoleAuthorize";

        /// <summary>
        /// Gets the fallback policy provider.
        /// </summary>
        /// <value>
        /// The fallback policy provider.
        /// </value>
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAuthorizePolicyProvider"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public RoleAuthorizePolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        /// <summary>
        /// Gets the default authorization policy.
        /// </summary>
        /// <returns>
        /// The default authorization policy.
        /// </returns>
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        /// <summary>
        /// Gets the fallback authorization policy.
        /// </summary>
        /// <returns>
        /// The fallback authorization policy.
        /// </returns>
        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        /// <inheritdoc/>
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(policyName.Substring(POLICY_PREFIX.Length)))
            {
                var policy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme);
                policy.AddRequirements(new RoleAuthorizeRequirement(policyName.Substring(POLICY_PREFIX.Length)));
                return Task.FromResult(policy.Build());
            }

            return Task.FromResult<AuthorizationPolicy>(null);
        }
    }
}