using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Controllers;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers
{
    /// <summary>
    /// The Clear Cache controller tests.
    /// </summary>
    [TestClass]
    public class CacheControllerTests
    {
        /// <summary>
        /// The caching service.
        /// </summary>
        private readonly Mock<ICacheService> _mockCachingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheControllerTests"/> class.
        /// </summary>
        public CacheControllerTests()
        {
            _mockCachingService = new Mock<ICacheService>(MockBehavior.Strict);
            SetupMocks();
        }

        /// <summary>
        /// Clears the cache returns expected result.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void ClearCache_Returns_ExpectedResult()
        {
            // Arrange
            var controller = new CacheController(_mockCachingService.Object);

            // Act
            var result = controller.ClearCache();
            var okResult = result as OkObjectResult;

            // Assert
            okResult?.Value.Should().BeEquivalentTo("Cache Cleared Successfully.");
            _mockCachingService.Verify(x => x.ClearCache(), Times.Once);
        }

        /// <summary>
        /// Setups the mocks.
        /// </summary>
        private void SetupMocks()
        {
            _mockCachingService.Setup(x => x.ClearCache()).Verifiable();
        }
    }
}