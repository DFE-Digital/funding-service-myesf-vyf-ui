using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Services;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Validators;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Tiles
{
    [TestClass]
    public class TileTests
    {
        [TestMethod, TestCategory("Unit")]
        public void Tile_SetMethod_SetsAlertMessage()
        {
            //Arrange
            var expectedTileAlertText = "fake alert message";
            var mockTileAlert = new Mock<ITileAlert>();
            mockTileAlert.Setup(x => x.AlertText).Returns(expectedTileAlertText);
            var mockAlertService = new Mock<IAlertMessageService>();
            mockAlertService.Setup(method => method.GetAlert(null)).Returns(mockTileAlert.Object);
            var mockLinkGenerator = new Mock<LinkGenerator>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(method => method.HttpContext).Returns(new DefaultHttpContext());

            var fakeTile = new FakeTile(null, mockAlertService.Object, mockLinkGenerator.Object, mockHttpContextAccessor.Object);

            //Act
            fakeTile.SetAlert();

            //Assert
            fakeTile.HasAlert().Should().Be(true);
            fakeTile.Alert.Should().NotBeNull();
            fakeTile.Alert.AlertText.Should().NotBeNullOrEmpty();
            fakeTile.Alert.AlertText.Should().Be(expectedTileAlertText);
        }

        [TestMethod, TestCategory("Unit")]
        public void Tile_SetMethod_SetsAlertMessage_Null_WhenAlertServiceInstanceIsNull()
        {
            //Arrange
            var mockLinkGenerator = new Mock<LinkGenerator>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(method => method.HttpContext).Returns(new DefaultHttpContext());

            var fakeTile = new FakeTile(null, null, mockLinkGenerator.Object, mockHttpContextAccessor.Object);

            //Act
            fakeTile.SetAlert();

            //Assert
            fakeTile.HasAlert().Should().Be(false);
            fakeTile.Alert.Should().BeNull();
        }

        [TestMethod, TestCategory("Unit")]
        public void Tile_IsAvailable_ReturnsTrue_WhenValidatorChecksPass()
        {
            //Arrange
            var mockValidator = new Mock<ITileDisplayRuleValidator>();
            mockValidator.Setup(method => method.Validate(null)).Returns(true);
            var mockLinkGenerator = new Mock<LinkGenerator>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(method => method.HttpContext).Returns(new DefaultHttpContext());

            var fakeTile = new FakeTile(mockValidator.Object, null, mockLinkGenerator.Object, mockHttpContextAccessor.Object);

            //Act
            var actual = fakeTile.IsAvailable(null);

            //Assert
            actual.Should().BeTrue();
        }

        [TestMethod, TestCategory("Unit")]
        public void Tile_IsAvailable_ReturnsFalse_WhenValidatorChecksDontPass()
        {
            //Arrange
            var mockValidator = new Mock<ITileDisplayRuleValidator>();
            mockValidator.Setup(method => method.Validate(null)).Returns(false);
            var mockLinkGenerator = new Mock<LinkGenerator>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(method => method.HttpContext).Returns(new DefaultHttpContext());

            var fakeTile = new FakeTile(mockValidator.Object, null, mockLinkGenerator.Object, mockHttpContextAccessor.Object);

            //Act
            var actual = fakeTile.IsAvailable(null);

            //Assert
            actual.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public void Tile_IsAvailable_ReturnsFalse_WhenValidatorInstanceIsNull()
        {
            //Arrange
            var mockLinkGenerator = new Mock<LinkGenerator>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(method => method.HttpContext).Returns(new DefaultHttpContext());

            var fakeTile = new FakeTile(null, null, mockLinkGenerator.Object, mockHttpContextAccessor.Object);

            //Act
            var actual = fakeTile.IsAvailable(null);

            //Assert
            actual.Should().BeFalse();
        }
    }
}