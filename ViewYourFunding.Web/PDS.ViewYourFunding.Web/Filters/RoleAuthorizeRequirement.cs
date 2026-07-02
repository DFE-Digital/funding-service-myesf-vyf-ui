using Microsoft.AspNetCore.Authorization;

namespace PDS.ViewYourFunding.Web.Filters
{
    /// <summary>
    /// The toggle authorize requirement.
    /// </summary>
    public class RoleAuthorizeRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Gets or sets the role required.
        /// </summary>
        /// <value>
        /// The role required.
        /// </value>
        public string RoleRequired { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAuthorizeRequirement"/> class.
        /// </summary>
        /// <param name="roleRequired">The role required.</param>
        public RoleAuthorizeRequirement(string roleRequired) => RoleRequired = roleRequired;
    }
}