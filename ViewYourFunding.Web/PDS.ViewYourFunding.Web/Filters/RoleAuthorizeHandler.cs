using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Filters
{
    /// <summary>
    /// The Role Authorize handler.
    /// </summary>
    /// <seealso cref="RoleAuthorizeRequirement" />
    public class RoleAuthorizeHandler : AuthorizationHandler<RoleAuthorizeRequirement>
    {
        /// <summary>
        /// The SFS roles claim type name.
        /// </summary>
        private const string SfsRolesClaimTypeName = "http://sfs-sfa.gov.uk/claims/role";

        /// <summary>
        /// Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        /// <returns>The result of requirement evaluation.</returns>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RoleAuthorizeRequirement requirement)
        {
            if (context.User.Claims.Any(
                x => x.Type == SfsRolesClaimTypeName &&
                     x.Value == requirement.RoleRequired))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}