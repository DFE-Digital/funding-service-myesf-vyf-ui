using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using PDS.ViewYourFunding.Web.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Filters
{
    /// <summary>
    /// The UserRoleAuthorizationMiddlewareResultHandler class.
    /// </summary>
    public class UserRoleAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        /// <summary>
        /// Gets or sets the default authorization middleware result handler.
        /// </summary>
        public IAuthorizationMiddlewareResultHandler Handler { get; set; }

        private readonly IUserRoleAuthorizationService _userRoleAuthorizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleAuthorizationMiddlewareResultHandler"/> class.
        /// </summary>
        /// <param name="userRoleAuthorizationService">The user role authorization service.</param>
        public UserRoleAuthorizationMiddlewareResultHandler(IUserRoleAuthorizationService userRoleAuthorizationService)
        {
            Handler = new AuthorizationMiddlewareResultHandler();
            _userRoleAuthorizationService = userRoleAuthorizationService;
        }

        /// <inheritdoc />
        public async Task HandleAsync(
            RequestDelegate requestDelegate,
            HttpContext httpContext,
            AuthorizationPolicy authorizationPolicy,
            PolicyAuthorizationResult policyAuthorizationResult)
        {
            if (policyAuthorizationResult.Forbidden && policyAuthorizationResult.AuthorizationFailure != null)
            {
                if (policyAuthorizationResult.AuthorizationFailure.FailedRequirements.Any(requirement => requirement is UserRoleAuthorizationRequirement))
                {
                    await _userRoleAuthorizationService.RedirectToAccessDenied(((UserRoleAuthorizationRequirement)policyAuthorizationResult.AuthorizationFailure.FailedRequirements.First()).UserRoles);
                    return;
                }
            }
            else
            {
                await Handler.HandleAsync(requestDelegate, httpContext, authorizationPolicy, policyAuthorizationResult);
            }
        }
    }
}
