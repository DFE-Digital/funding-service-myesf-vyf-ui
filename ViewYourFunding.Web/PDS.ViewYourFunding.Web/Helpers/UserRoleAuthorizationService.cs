using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Pds.Core.Common.Identity.Enums;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Extensions;
using PDS.ViewYourFunding.Web.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Helpers
{
    public class UserRoleAuthorizationService : IUserRoleAuthorizationService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionResultExecutor<RedirectToActionResult> _actionResultExecutor;

        private readonly string _symmetricKey = "G+KaPdSgVkYp3s6v9y$B&E)H@McQeThW";

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleAuthorizationService"/> class.
        /// </summary>
        /// <param name="encryptionService">The encryption service.</param>
        /// <param name="httpContextAccessor">The http context accessor object.</param>
        /// <param name="actionResultExecutor">The action result executor object.</param>
        public UserRoleAuthorizationService(
            IEncryptionService encryptionService,
            IHttpContextAccessor httpContextAccessor,
            IActionResultExecutor<RedirectToActionResult> actionResultExecutor)
        {
            _encryptionService = encryptionService;
            _httpContextAccessor = httpContextAccessor;
            _actionResultExecutor = actionResultExecutor;
        }

        /// <inheritdoc />
        public List<string> DecryptUrlParameterToRequiredUserRoleNames(string urlParameter, bool isExternalUser)
        {
            var userRoles = _encryptionService.DecryptStringFromHex(_symmetricKey, urlParameter).Split(',').Select(x => (UserRole)int.Parse(x)).ToList();

            List<string> validRoles;
            if (!isExternalUser)
            {
                validRoles = userRoles.Where(x => x.GetUserType() == UserType.Internal || x.GetUserType() == UserType.AdminInternal).Select(x => x.GetDisplayName()).ToList();
            }
            else
            {
                validRoles = userRoles.Where(x => x.GetUserType() == UserType.External).Select(x => x.GetDisplayName()).ToList();
            }

            return validRoles.Any() ? validRoles : null;
        }

        /// <inheritdoc />
        public string EncryptRequiredUserRolesToUrlParameter(List<UserRole> requiredUserRoles)
        {
            var userRolesCommaSeparatedString = string.Join(',', requiredUserRoles.Select(x => ((int)x).ToString()).ToList());
            return _encryptionService.EncryptStringToHex(_symmetricKey, userRolesCommaSeparatedString);
        }

        /// <inheritdoc />
        public async Task RedirectToAccessDenied(List<UserRole> requiredUserRoles)
        {
            var actionResult = new RedirectToActionResult("StatusCode403_DSIRequiredUserRoles", "Error", new { code = EncryptRequiredUserRolesToUrlParameter(requiredUserRoles) });
            var routeData = _httpContextAccessor.HttpContext.GetRouteData() ?? new RouteData();
            var actionContext = new ActionContext(_httpContextAccessor.HttpContext, routeData, new ActionDescriptor());

            await _actionResultExecutor.ExecuteAsync(actionContext, actionResult);
        }
    }
}
