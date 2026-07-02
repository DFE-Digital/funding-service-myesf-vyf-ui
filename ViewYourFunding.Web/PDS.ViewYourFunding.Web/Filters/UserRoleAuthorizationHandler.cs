using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Filters
{
    /// <summary>
    /// The UserRoleAuthorzationHandler class.
    /// </summary>
    public class UserRoleAuthorizationHandler : AuthorizationHandler<UserRoleAuthorizationRequirement>
    {
        private const string SfsRolesClaimTypeName = "http://sfs-sfa.gov.uk/claims/role";

        /// <inheritdoc />
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRoleAuthorizationRequirement requirement)
        {
            if (context.User.Claims.Any())
            {
                if (context.User.Claims.Any(
                    x => x.Type == SfsRolesClaimTypeName &&
                         requirement.UserRoles.Select(x => x.ToString()).ToList().Contains(x.Value)))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
