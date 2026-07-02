using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Common.Identity.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Builder;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Validators;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Tiles
{
    [TestClass]
    public class TileBuilderTests
    {
        private List<ITile> fakeTiles;
        private Mock<User> mockUser;
        private Mock<ITile> mockTile;

        [TestInitialize]
        public void Initialize()
        {
            mockTile = new Mock<ITile>();

            mockUser = new Mock<User>();

            fakeTiles = new List<ITile>();
        }

        [TestMethod, TestCategory("Unit")]
        public void TileBuilder_ForUser_ReturnsTileBuilder()
        {
            //Arrange
            var fakeBuilder = new HomeTileBuilder(fakeTiles);

            //Act
            var actualBuilder = fakeBuilder.ForUser(mockUser.Object);

            //Assert
            actualBuilder.Should().NotBeNull();
            actualBuilder.Should().BeSameAs(fakeBuilder);
        }

        [TestMethod, TestCategory("Unit")]
        public void TileBuilder_BuildTilesAsync_ReturnsTiles_WhenTileIsAvailable()
        {
            //Arrange
            var mockValidator = new Mock<ITileDisplayRuleValidator>();
            mockValidator.Setup(method => method.Validate(It.IsAny<IUserContext>())).Returns(true);
            var mockLinkGenerator = new Mock<LinkGenerator>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(method => method.HttpContext).Returns(new DefaultHttpContext());

            var fakeTile = new FakeTile(mockValidator.Object, null, mockLinkGenerator.Object, mockHttpContextAccessor.Object);
            fakeTiles.Add(fakeTile);
            var fakeBuilder = new HomeTileBuilder(fakeTiles);

            //Act
            var actualTiles = fakeBuilder.ForUser(mockUser.Object).BuildTiles();

            //Assert
            actualTiles.Should().NotBeNullOrEmpty();
            actualTiles.Should().HaveCount(1);
            actualTiles.Should().Contain(fakeTile);
            actualTiles.Should().Contain(tile => tile.Id == "fake");
        }

        [TestMethod, TestCategory("Unit")]
        public void TileBuilder_BuildTilesAsync_ReturnsEmptyList_WhenTileIsNotAvailable()
        {
            //Arrange
            mockTile.Setup(method => method.IsAvailable(It.IsAny<IUserContext>())).Returns(false);
            fakeTiles.Add(mockTile.Object);
            var fakeBuilder = new HomeTileBuilder(fakeTiles);

            //Act
            var actualTiles = fakeBuilder.ForUser(mockUser.Object).BuildTiles();

            //Assert
            actualTiles.Should().NotBeNull();
            actualTiles.Should().BeEmpty();
        }

        [TestMethod, TestCategory("Unit")]
        public void TileBuilder_BuildTilesAsync_ReturnsEmptyTilesCollection_WhenEmptyTilesCollection_PassedToTileBuilderConstructor()
        {
            //Arrange
            var fakeBuilder = new HomeTileBuilder(new List<ITile>());

            //Act
            var actualTiles = fakeBuilder.BuildTiles();

            //Assert
            actualTiles.Should().NotBeNull();
            actualTiles.Should().BeEmpty();
        }

        [TestMethod, TestCategory("Unit")]
        public void TileBuilder_BuildTilesAsync_ReturnsEmptyTilesCollection_WhenNullTilesCollectionPassedToTileBuilderConstructor()
        {
            //Arrange
            var fakeBuilder = new HomeTileBuilder(null);

            //Act
            var actualTiles = fakeBuilder.BuildTiles();

            //Assert
            actualTiles.Should().NotBeNull();
            actualTiles.Should().BeEmpty();
        }
    }
}