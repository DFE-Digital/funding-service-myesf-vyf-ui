using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Common.Identity.Enums;
using PDS.ViewYourFunding.Web.Filters;
using PDS.ViewYourFunding.Web.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Filters
{
    [TestClass]
    public class UserRoleAuthorizationMiddlewareResultHandlerTests
    {
        #region Private members

        private readonly Mock<IAuthorizationMiddlewareResultHandler> _mockAuthorizationMiddlewareResultHandler;
        private readonly Mock<IUserRoleAuthorizationService> _mockUserRoleAuthorizationService;

        #endregion

        #region Constructor

        public UserRoleAuthorizationMiddlewareResultHandlerTests()
        {
            _mockAuthorizationMiddlewareResultHandler = new Mock<IAuthorizationMiddlewareResultHandler>(MockBehavior.Strict);
            _mockUserRoleAuthorizationService = new Mock<IUserRoleAuthorizationService>(MockBehavior.Strict);
            SetupMockServices();
        }

        #endregion

        [TestMethod, TestCategory("Unit")]
        public async Task UserRoleAuthorizationMiddlewareResultHandler_PolicyAuthorizationSuccess_Should_Invoke_DefaultHandler()
        {
            //Arrange
            var policyAuthorizationResult = PolicyAuthorizationResult.Success();
            var userRoleAuthorizationMiddlewareResultHandler = new UserRoleAuthorizationMiddlewareResultHandler(_mockUserRoleAuthorizationService.Object);
            userRoleAuthorizationMiddlewareResultHandler.Handler = _mockAuthorizationMiddlewareResultHandler.Object;

            //Act
            await userRoleAuthorizationMiddlewareResultHandler.HandleAsync(null, null, null, policyAuthorizationResult);

            //Assert
            _mockAuthorizationMiddlewareResultHandler.Verify(x => x.HandleAsync(It.IsAny<RequestDelegate>(), It.IsAny<HttpContext>(), It.IsAny<AuthorizationPolicy>(), It.IsAny<PolicyAuthorizationResult>()), Times.Once);
            _mockUserRoleAuthorizationService.Verify(x => x.RedirectToAccessDenied(It.IsAny<List<UserRole>>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UserRoleAuthorizationMiddlewareResultHandler_PolicyAuthorizationChallenge_Should_Invoke_DefaultHandler()
        {
            //Arrange
            var policyAuthorizationResult = PolicyAuthorizationResult.Challenge();
            var userRoleAuthorizationMiddlewareResultHandler = new UserRoleAuthorizationMiddlewareResultHandler(_mockUserRoleAuthorizationService.Object);
            userRoleAuthorizationMiddlewareResultHandler.Handler = _mockAuthorizationMiddlewareResultHandler.Object;

            //Act
            await userRoleAuthorizationMiddlewareResultHandler.HandleAsync(null, null, null, policyAuthorizationResult);

            //Assert
            _mockAuthorizationMiddlewareResultHandler.Verify(x => x.HandleAsync(It.IsAny<RequestDelegate>(), It.IsAny<HttpContext>(), It.IsAny<AuthorizationPolicy>(), It.IsAny<PolicyAuthorizationResult>()), Times.Once);
            _mockUserRoleAuthorizationService.Verify(x => x.RedirectToAccessDenied(It.IsAny<List<UserRole>>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UserRoleAuthorizationMiddlewareResultHandler_PolicyAuthorizationForbidden_Should_Invoke_CustomHandler()
        {
            //Arrange
            var authorizationRequirement = new UserRoleAuthorizationRequirement(new List<UserRole> { UserRole.SfsAdmin });
            var authorizationFailure = AuthorizationFailure.Failed(new List<IAuthorizationRequirement> { authorizationRequirement });
            var policyAuthorizationResult = PolicyAuthorizationResult.Forbid(authorizationFailure);
            var userRoleAuthorizationMiddlewareResultHandler = new UserRoleAuthorizationMiddlewareResultHandler(_mockUserRoleAuthorizationService.Object);
            userRoleAuthorizationMiddlewareResultHandler.Handler = _mockAuthorizationMiddlewareResultHandler.Object;

            //Act
            await userRoleAuthorizationMiddlewareResultHandler.HandleAsync(null, null, null, policyAuthorizationResult);

            //Assert
            _mockAuthorizationMiddlewareResultHandler.Verify(x => x.HandleAsync(It.IsAny<RequestDelegate>(), It.IsAny<HttpContext>(), It.IsAny<AuthorizationPolicy>(), It.IsAny<PolicyAuthorizationResult>()), Times.Never);
            _mockUserRoleAuthorizationService.Verify(x => x.RedirectToAccessDenied(It.IsAny<List<UserRole>>()), Times.Once);
        }

        #region Private helpers

        private void SetupMockServices()
        {
            _mockAuthorizationMiddlewareResultHandler.Setup(x => x.HandleAsync(It.IsAny<RequestDelegate>(), It.IsAny<HttpContext>(), It.IsAny<AuthorizationPolicy>(), It.IsAny<PolicyAuthorizationResult>())).Returns(Task.CompletedTask);
            _mockUserRoleAuthorizationService.Setup(x => x.RedirectToAccessDenied(It.IsAny<List<UserRole>>())).Returns(Task.CompletedTask);
        }

        #endregion
    }
}
