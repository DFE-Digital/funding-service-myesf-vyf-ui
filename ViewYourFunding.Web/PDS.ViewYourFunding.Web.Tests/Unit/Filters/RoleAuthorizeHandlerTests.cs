using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Web.Filters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Filters
{
    /// <summary>
    /// The Role Authorize Handler Tests.
    /// </summary>
    [TestClass]
    public class RoleAuthorizeHandlerTests
    {
        /// <summary>
        /// The SFS roles claim type name.
        /// </summary>
        private const string SfsRolesClaimTypeName = "http://sfs-sfa.gov.uk/claims/role";

        /// <summary>
        /// The SFS admin role.
        /// </summary>
        private const string SfsAdminRole = "sfsAdmin";

        /// <summary>
        /// The non SFS admin role.
        /// </summary>
        private const string NonSfsAdminRole = "nonSfsAdmin";

        /// <summary>
        /// RoleAuthorizationHandler should fail when the user is authenticated and has wrong role.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task RoleAuthorizationHandler_UserAuthenticated_WrongRole_Should_Fail()
        {
            //Arrange
            var requirements = new[]
            {
                new RoleAuthorizeRequirement(SfsAdminRole)
            };

            var user = GetUser(true, NonSfsAdminRole);
            var context = new AuthorizationHandlerContext(requirements, user, null);
            var roleAuthorizeHandler = new RoleAuthorizeHandler();

            //Act
            await roleAuthorizeHandler.HandleAsync(context);

            //Assert
            context.HasFailed.Should().BeTrue();
        }

        /// <summary>
        /// RoleAuthorizationHandler should fail when the user is not authenticated.
        /// </summary>
        /// <returns>The expected requirement result.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task RoleAuthorizationHandler_UserNotAuthenticated_Should_Fail()
        {
            //Arrange
            var requirements = new[]
            {
                new RoleAuthorizeRequirement(SfsAdminRole)
            };

            var user = GetUser(false, string.Empty);
            var context = new AuthorizationHandlerContext(requirements, user, null);
            var roleAuthorizeHandler = new RoleAuthorizeHandler();

            //Act
            await roleAuthorizeHandler.HandleAsync(context);

            //Assert
            context.HasFailed.Should().BeTrue();
        }

        /// <summary>
        /// RoleAuthorizationHandler should succeed user is authenticated and has the right role.
        /// </summary>
        /// <returns>The expected requirement result.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task RoleAuthorizationHandler_UserAuthenticated_Should_Succeed()
        {
            //Arrange
            var requirements = new[]
            {
                new RoleAuthorizeRequirement(SfsAdminRole)
            };

            var user = GetUser(true, SfsAdminRole);
            var context = new AuthorizationHandlerContext(requirements, user, null);
            var roleAuthorizeHandler = new RoleAuthorizeHandler();

            //Act
            await roleAuthorizeHandler.HandleAsync(context);

            //Assert
            context.HasSucceeded.Should().BeTrue();
        }

        private static ClaimsPrincipal GetUser(bool authenticated, string userRole)
        {
            if (authenticated)
            {
                var user = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new[]
                        {
                            new Claim(ClaimsIdentity.DefaultNameClaimType, nameof(ClaimsIdentity.DefaultNameClaimType)),
                            new Claim(SfsRolesClaimTypeName, userRole)
                        },
                        "Basic"));
                return user;
            }

            return new ClaimsPrincipal(
                new ClaimsIdentity());
        }
    }
}
