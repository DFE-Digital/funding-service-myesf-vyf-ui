using Microsoft.AspNetCore.Authorization;
using Pds.Core.Common.Identity.Enums;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Filters
{
    /// <summary>
    /// The UserRoleAuthorizationRequirement class.
    /// </summary>
    public class UserRoleAuthorizationRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Gets or sets the list of user roles required for the authorization to succeed.
        /// </summary>
        public List<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleAuthorizationRequirement"/> class.
        /// </summary>
        /// <param name="userRoles">The list of required user rules for authorization to succeed.</param>
        public UserRoleAuthorizationRequirement(List<UserRole> userRoles)
        {
            UserRoles = userRoles;
        }
    }
}
