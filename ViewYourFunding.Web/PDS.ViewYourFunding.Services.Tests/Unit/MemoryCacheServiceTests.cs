using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Cache;
using PDS.ViewYourFunding.Services.Implementations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    /// <summary>
    /// The MemoryCacheServiceTests class.
    /// </summary>
    [TestClass]
    public class MemoryCacheServiceTests
    {
        /// <summary>
        /// The cache service.
        /// </summary>
        private readonly MemoryCacheService _cacheService;

        /// <summary>
        /// The method call count.
        /// </summary>
        private int _methodCallCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheServiceTests"/> class.
        /// </summary>
        public MemoryCacheServiceTests()
        {
            var mockLogger = new Mock<ILoggerAdapter<MemoryCacheService>>(MockBehavior.Strict);
            mockLogger.Setup(x => x.LogInformation(It.IsAny<string>()));
            _cacheService = new MemoryCacheService(mockLogger.Object, 5);
            _methodCallCount = 0;
        }

        /// <summary>
        /// Adds the or get existing result returns expected value.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void AddOrGetExistingResult_ReturnsExpectedValue()
        {
            //Assign
            var key = $"{Guid.NewGuid()} {nameof(AddOrGetExistingResult_ReturnsExpectedValue)}";

            //Act
            var result1 = _cacheService.AddOrGetExistingResult(key, MethodToExecute, CacheExpirationPolicy.Absolute, TimeSpan.FromMinutes(1));
            var result2 = _cacheService.AddOrGetExistingResult(key, MethodToExecute, CacheExpirationPolicy.Sliding, TimeSpan.FromMinutes(1));

            //Assert
            result1.Should().Be(result2);
            _methodCallCount.Should().Be(1);
        }

        /// <summary>
        /// Adds the or get existing result asynchronous returns expected value.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task AddOrGetExistingResultAsync_ReturnsExpectedValue()
        {
            //Assign
            var key = $"{Guid.NewGuid()} {nameof(AddOrGetExistingResultAsync_ReturnsExpectedValue)}";

            //Act
            var result1 = await _cacheService.AddOrGetExistingResultAsync(key, MethodToExecuteAsync, CacheExpirationPolicy.Absolute, TimeSpan.FromMinutes(6));
            var result2 = await _cacheService.AddOrGetExistingResultAsync(key, MethodToExecuteAsync, CacheExpirationPolicy.Sliding, TimeSpan.FromMinutes(6));

            //Assert
            result1.Should().Be(result2);
            _methodCallCount.Should().Be(1);
        }

        /// <summary>
        /// Adds the or get existing result no cache expected.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void AddOrGetExistingResult_NoCacheExpected()
        {
            //Assign
            var key = $"{Guid.NewGuid()} {nameof(AddOrGetExistingResult_NoCacheExpected)}";

            //Act
            var result1 = _cacheService.AddOrGetExistingResult(key, MethodToExecute, CacheExpirationPolicy.Absolute, TimeSpan.Zero);
            var result2 = _cacheService.AddOrGetExistingResult(key, MethodToExecute, CacheExpirationPolicy.Sliding, TimeSpan.Zero);

            //Assert
            result1.Should().NotBe(result2);
            _methodCallCount.Should().Be(2);
        }

        /// <summary>
        /// Adds the or get existing result asynchronous no cache expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task AddOrGetExistingResultAsync_NoCacheExpected()
        {
            //Assign
            var key = $"{Guid.NewGuid()} {nameof(AddOrGetExistingResultAsync_NoCacheExpected)}";

            //Act
            var result1 = await _cacheService.AddOrGetExistingResultAsync(key, MethodToExecuteAsync, CacheExpirationPolicy.Absolute, TimeSpan.Zero);
            var result2 = await _cacheService.AddOrGetExistingResultAsync(key, MethodToExecuteAsync, CacheExpirationPolicy.Sliding, TimeSpan.Zero);

            //Assert
            result1.Should().NotBe(result2);
            _methodCallCount.Should().Be(2);
        }

        /// <summary>
        /// Adds the or get existing result no cross key contamination.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void AddOrGetExistingResult_NoCrossKeyContamination()
        {
            //Assign
            var key = $"{Guid.NewGuid()} {nameof(AddOrGetExistingResult_NoCrossKeyContamination)}-key-1";
            var key2 = $"{Guid.NewGuid()} {nameof(AddOrGetExistingResult_NoCrossKeyContamination)}-key-2";

            //Act
            var key1Result1 = _cacheService.AddOrGetExistingResult(key, MethodToExecute, CacheExpirationPolicy.Absolute, TimeSpan.FromMinutes(2));
            var key2Result1 = _cacheService.AddOrGetExistingResult(key2, MethodToExecute, CacheExpirationPolicy.Absolute, TimeSpan.FromMinutes(2));

            var key1Result2 = _cacheService.AddOrGetExistingResult(key, MethodToExecute, CacheExpirationPolicy.Absolute, TimeSpan.FromMinutes(2));
            var key2Result2 = _cacheService.AddOrGetExistingResult(key2, MethodToExecute, CacheExpirationPolicy.Absolute, TimeSpan.FromMinutes(2));

            //Assert
            key1Result1.Should().Be(key1Result2);
            key2Result1.Should().Be(key2Result2);
            key2Result2.Should().NotBe(key1Result2);
            _methodCallCount.Should().Be(2);
        }

        /// <summary>
        /// Adds the or get existing result asynchronous no cross key contamination.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task AddOrGetExistingResultAsync_NoCrossKeyContamination()
        {
            //Assign
            var key = $"{Guid.NewGuid()} {nameof(AddOrGetExistingResultAsync_NoCrossKeyContamination)}-key-1";
            var key2 = $"{Guid.NewGuid()} {nameof(AddOrGetExistingResultAsync_NoCrossKeyContamination)}-key-2";

            //Act
            var key1Result1 = await _cacheService.AddOrGetExistingResultAsync(key, MethodToExecuteAsync, CacheExpirationPolicy.Absolute, TimeSpan.FromMinutes(2));
            var key2Result1 = await _cacheService.AddOrGetExistingResultAsync(key2, MethodToExecuteAsync, CacheExpirationPolicy.Absolute, TimeSpan.FromMinutes(2));

            var key1Result2 = await _cacheService.AddOrGetExistingResultAsync(key, MethodToExecuteAsync, CacheExpirationPolicy.Absolute, TimeSpan.FromMinutes(2));
            var key2Result2 = await _cacheService.AddOrGetExistingResultAsync(key2, MethodToExecuteAsync, CacheExpirationPolicy.Absolute, TimeSpan.FromMinutes(2));

            //Assert
            key1Result1.Should().Be(key1Result2);
            key2Result1.Should().Be(key2Result2);
            key2Result2.Should().NotBe(key1Result2);
            _methodCallCount.Should().Be(2);
        }

        private double MethodToExecute()
        {
            Thread.Sleep(10);
            _methodCallCount++;
            return DateTime.Now.Ticks * new Random().NextDouble();
        }

        private async Task<string> MethodToExecuteAsync()
        {
            Thread.Sleep(10);
            _methodCallCount++;
            return await Task.Run(() => $"{DateTime.Now.Ticks * new Random().NextDouble()}-{nameof(MethodToExecuteAsync)}");
        }
    }
}
