using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Common.Identity.Models;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Web.Models;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Web.Areas.Admin.Controllers;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Builder;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Services;
using PDS.ViewYourFunding.Web.Tests.Unit.Tiles;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class AdminControllerUnitTests
    {
        #region Mock Services

        private readonly Mock<ITileBuilder> _mockTileBuilder;

        private readonly Mock<IClaimsBasedIdentityService> _mockSecurityService;

        #endregion


        #region Constructor

        public AdminControllerUnitTests()
        {
            _mockTileBuilder = new Mock<ITileBuilder>(MockBehavior.Strict);
            _mockSecurityService = new Mock<IClaimsBasedIdentityService>(MockBehavior.Strict);
            SetupMockTileBuilder();
            SetupMockSecurityServices();
        }

        #endregion


        #region Action Tests

        [TestMethod, TestCategory("Unit")]
        public async Task Home_ResultExpected()
        {
            // Arrange
            var controller = GetAdminController();
            var expectedViewModel = new HomeViewModel
            {
                Tiles = GetTiles(),
                CurrentUser = new CurrentUserViewModel
                {
                    Ukprn = 12345678,
                    ProviderName = "Test Provider",
                    IsExternalUser = true,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName,
                    IsLoggedIn = true
                },
                ContactUsLink = "http://www.vyf-contactus.com/contactus",
                FeedbackLink = "http://www.vyf-survey.com/survey"
            };

            // Act
            var actual = await controller.Home();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<HomeViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        #endregion


        #region Private helpers

        private AdminController GetAdminController()
        {
            var controller = new AdminController(
                _mockSecurityService.Object,
                _mockTileBuilder.Object,
                GetMockConfigService().Object);

            return controller;
        }

        private Mock<IOptions<ApplicationConfiguration>> GetMockConfigService()
        {
            var mockConfigService = new Mock<IOptions<ApplicationConfiguration>>();

            mockConfigService.Setup(x => x.Value)
                .Returns(new ApplicationConfiguration
                {
                    ContactUsLink = "http://www.vyf-contactus.com/contactus",
                    FeedbackLink = "http://www.vyf-survey.com/survey",
                    ViewYourFundingApiBaseAddress = nameof(ApplicationConfiguration.ViewYourFundingApiBaseAddress)
                });

            return mockConfigService;
        }

        private void SetupMockSecurityServices()
        {
            _mockSecurityService
                .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(LoggedInUser);
        }

        private void SetupMockTileBuilder()
        {
            _mockTileBuilder.Setup(x => x.ForUser(It.IsAny<User>()).BuildTiles()).Returns(GetTiles);
        }

        private List<ITile> GetTiles()
        {
            const string expectedTileAlertText = "fake alert message";
            var mockTileAlert = Mock.Of<ITileAlert>();
            mockTileAlert.AlertText = expectedTileAlertText;
            var mockAlertService = Mock.Of<IAlertMessageService>();
            Mock.Get(mockAlertService).Setup(method => method.GetAlert(null)).Returns(mockTileAlert);
            var mockLinkGenerator = Mock.Of<LinkGenerator>();
            var mockHttpContextAccessor = Mock.Of<IHttpContextAccessor>();
            Mock.Get(mockHttpContextAccessor).Setup(method => method.HttpContext).Returns(new DefaultHttpContext());

            var fakeTile = new FakeTile(null, mockAlertService, mockLinkGenerator, mockHttpContextAccessor);

            return new List<ITile>
            {
                fakeTile,
                fakeTile
            };
        }

        private static readonly User LoggedInUser = new User
        {
            Ukprn = 12345678,
            ProviderName = "Test Provider",
            Email = "test@test.com",
            IsAuthenticated = true,
            IsExternalUser = true,
            FirstName = nameof(User.FirstName),
            FullName = nameof(User.FullName),
            LastName = nameof(User.LastName),
        };

        #endregion
    }
}