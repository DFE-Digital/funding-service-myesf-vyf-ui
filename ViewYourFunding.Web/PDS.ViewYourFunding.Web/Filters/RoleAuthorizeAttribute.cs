using Microsoft.AspNetCore.Authorization;

namespace PDS.ViewYourFunding.Web.Filters
{
    /// <summary>
    /// The VYF user role based authorisation.
    /// </summary>
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        private const string PolicyPrefix = "RoleAuthorize";

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="roleRequired">The role required.</param>
        public RoleAuthorizeAttribute(string roleRequired) => RoleRequired = roleRequired;

        /// <summary>
        /// Gets or sets the role required.
        /// </summary>
        /// <value>
        /// The role required.
        /// </value>
        public string RoleRequired
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Policy.Substring(PolicyPrefix.Length)))
                {
                    return Policy.Substring(PolicyPrefix.Length);
                }

                return string.Empty;
            }
            set => Policy = $"{PolicyPrefix}{value}";
        }
    }
}