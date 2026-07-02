using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pds.Core.Common.Identity.Enums;
using PDS.ViewYourFunding.Web.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Filters
{
    [TestClass]
    public class UserRoleAuthorizationHandlerTests
    {
        #region Private members

        private const string UserRolesClaimTypeName = "http://sfs-sfa.gov.uk/claims/role";

        #endregion

        [TestMethod, TestCategory("Unit")]
        [DataRow(new UserRole[] { UserRole.SfsAdmin }, UserRole.ViewRecoupmentReports)]
        [DataRow(new UserRole[] { UserRole.ViewAsProvider, UserRole.ViewAllocationStatements }, UserRole.SfsAdmin)]
        [DataRow(new UserRole[] { UserRole.SfsAdmin, UserRole.AllocationsAdministrator_1416, UserRole.AllocationsAdministrator_1619 }, UserRole.ViewAsProvider)]
        public async Task UserRoleAuthorizationHandler_UserAuthenticated_WrongRole_Should_Fail(UserRole[] requiredUserRoles, UserRole currentUserRole)
        {
            //Arrange
            var requirements = new[] { new UserRoleAuthorizationRequirement(requiredUserRoles.ToList()) };
            var user = GetCurrentUser(true, currentUserRole);
            var context = new AuthorizationHandlerContext(requirements, user, null);
            var userRoleAuthorizationHandler = new UserRoleAuthorizationHandler();

            //Act
            await userRoleAuthorizationHandler.HandleAsync(context);

            //Assert
            context.HasSucceeded.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UserRoleAuthorizationHandler_UserNotAuthenticated_Should_Fail()
        {
            //Arrange
            var requirements = new[] { new UserRoleAuthorizationRequirement(new List<UserRole> { UserRole.SfsAdmin }) };
            var user = GetCurrentUser(false, UserRole.SfsAdmin);
            var context = new AuthorizationHandlerContext(requirements, user, null);
            var userRoleAuthorizationHandler = new UserRoleAuthorizationHandler();

            //Act
            await userRoleAuthorizationHandler.HandleAsync(context);

            //Assert
            context.HasSucceeded.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(new UserRole[] { UserRole.SfsAdmin }, UserRole.SfsAdmin)]
        [DataRow(new UserRole[] { UserRole.ViewAsProvider, UserRole.ViewAllocationStatements }, UserRole.ViewAsProvider)]
        [DataRow(new UserRole[] { UserRole.ViewAsProvider, UserRole.ViewAllocationStatements }, UserRole.ViewAllocationStatements)]
        [DataRow(new UserRole[] { UserRole.ViewAsProvider, UserRole.ViewRecoupmentReports }, UserRole.ViewAsProvider)]
        [DataRow(new UserRole[] { UserRole.ViewAsProvider, UserRole.ViewRecoupmentReports }, UserRole.ViewRecoupmentReports)]
        [DataRow(new UserRole[] { UserRole.SfsAdmin, UserRole.AllocationsAdministrator_1416, UserRole.AllocationsAdministrator_1619 }, UserRole.SfsAdmin)]
        [DataRow(new UserRole[] { UserRole.SfsAdmin, UserRole.AllocationsAdministrator_1416, UserRole.AllocationsAdministrator_1619 }, UserRole.AllocationsAdministrator_1416)]
        [DataRow(new UserRole[] { UserRole.SfsAdmin, UserRole.AllocationsAdministrator_1416, UserRole.AllocationsAdministrator_1619 }, UserRole.AllocationsAdministrator_1619)]
        public async Task UserRoleAuthorizationHandler_UserAuthenticated_Should_Succeed(UserRole[] requiredUserRoles, UserRole currentUserRole)
        {
            //Arrange
            var requirements = new[] { new UserRoleAuthorizationRequirement(requiredUserRoles.ToList()) };
            var user = GetCurrentUser(true, currentUserRole);
            var context = new AuthorizationHandlerContext(requirements, user, null);
            var userRoleAuthorizationHandler = new UserRoleAuthorizationHandler();

            //Act
            await userRoleAuthorizationHandler.HandleAsync(context);

            //Assert
            context.HasSucceeded.Should().BeTrue();
        }

        #region Private helpers

        private ClaimsPrincipal GetCurrentUser(bool authenticated, UserRole userRole)
        {
            if (authenticated)
            {
                var user = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new[]
                        {
                            new Claim(ClaimsIdentity.DefaultNameClaimType, nameof(ClaimsIdentity.DefaultNameClaimType)),
                            new Claim(UserRolesClaimTypeName, userRole.ToString())
                        },
                        "Basic"));
                return user;
            }

            return new ClaimsPrincipal(
                new ClaimsIdentity());
        }

        #endregion
    }
}
