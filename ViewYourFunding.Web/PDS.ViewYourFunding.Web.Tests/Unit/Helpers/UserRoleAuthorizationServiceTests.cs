using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Common.Identity.Enums;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Helpers;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Helpers
{
    [TestClass]
    public class UserRoleAuthorizationServiceTests
    {
        #region Private members

        private readonly Mock<IEncryptionService> _mockEncryptionService;

        #endregion


        #region Constructor

        public UserRoleAuthorizationServiceTests()
        {
            _mockEncryptionService = new Mock<IEncryptionService>(MockBehavior.Strict);
            SetupMockServices();
        }

        #endregion

        [TestMethod, TestCategory("Unit")]
        [DataRow("A", false, new string[] { "MYESF admin" })]
        [DataRow("A", true, null)]
        [DataRow("BB", false, new string[] { "View as provider" })]
        [DataRow("BB", true, new string[] { "View allocation statements" })]
        [DataRow("CCC", false, new string[] { "View as provider" })]
        [DataRow("CCC", true, new string[] { "View recoupment reports" })]
        [DataRow("D", false, new string[] { "MYESF admin", "Allocations administrator 1416", "Allocations administrator 1619" })]
        [DataRow("D", true, null)]
        public void DecryptUrlParameterToRequiredUserRoleNames_ExpectedResult(string urlParameter, bool isExternalUser, string[] expectedUserRoles)
        {
            // Arrange
            var userRoleAuthorizationService = new UserRoleAuthorizationService(_mockEncryptionService.Object, null, null);

            // Act
            var requiredUserRoleNames = userRoleAuthorizationService.DecryptUrlParameterToRequiredUserRoleNames(urlParameter, isExternalUser);

            // Assert
            requiredUserRoleNames.Should().BeEquivalentTo(expectedUserRoles);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(new UserRole[] { UserRole.SfsAdmin }, "A")]
        [DataRow(new UserRole[] { UserRole.ViewAsProvider, UserRole.ViewAllocationStatements }, "BB")]
        [DataRow(new UserRole[] { UserRole.ViewAsProvider, UserRole.ViewRecoupmentReports }, "CCC")]
        [DataRow(new UserRole[] { UserRole.SfsAdmin, UserRole.AllocationsAdministrator_1416, UserRole.AllocationsAdministrator_1619 }, "D")]
        public void EncryptRequiredUserRolesToUrlParameter_ExpectedResult(UserRole[] requiredUserRoles, string expectedUrlParameter)
        {
            // Arrange
            var userRoleAuthorizationService = new UserRoleAuthorizationService(_mockEncryptionService.Object, null, null);

            // Act
            var requiredUserRoleNames = userRoleAuthorizationService.EncryptRequiredUserRolesToUrlParameter(requiredUserRoles.ToList());

            // Assert
            requiredUserRoleNames.Should().BeEquivalentTo(expectedUrlParameter);
        }

        #region Private helpers

        private void SetupMockServices()
        {
            _mockEncryptionService.Setup(x => x.DecryptStringFromHex(It.IsAny<string>(), "A")).Returns("6");
            _mockEncryptionService.Setup(x => x.DecryptStringFromHex(It.IsAny<string>(), "BB")).Returns("27,5");
            _mockEncryptionService.Setup(x => x.DecryptStringFromHex(It.IsAny<string>(), "CCC")).Returns("33,5");
            _mockEncryptionService.Setup(x => x.DecryptStringFromHex(It.IsAny<string>(), "D")).Returns("6,18,19");

            _mockEncryptionService.Setup(x => x.EncryptStringToHex(It.IsAny<string>(), "6")).Returns("A");
            _mockEncryptionService.Setup(x => x.EncryptStringToHex(It.IsAny<string>(), "5,27")).Returns("BB");
            _mockEncryptionService.Setup(x => x.EncryptStringToHex(It.IsAny<string>(), "5,33")).Returns("CCC");
            _mockEncryptionService.Setup(x => x.EncryptStringToHex(It.IsAny<string>(), "6,18,19")).Returns("D");
        }

        #endregion
    }
}
