using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Config;
using PDS.ViewYourFunding.Services.Implementations;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    [TestClass]
    public class AdminFundingStreamSettingServiceUnitTests
    {
        #region Private fields

        private readonly int _fundingStreamId = 0, _settingValueId = 1;

        private readonly Mock<ISettingValueRepository> _mockSettingValueRepository;

        #endregion


        #region Constructor

        public AdminFundingStreamSettingServiceUnitTests()
        {
            _mockSettingValueRepository = new Mock<ISettingValueRepository>(MockBehavior.Strict);
            SetupDefaultMockBehaviour();
        }

        #endregion

        #region Tests

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateAsync_ResultExpected()
        {
            // Arrange
            var adminFundingStreamSettingService = GetAdminFundingStreamSettingService();

            var model = new Models.SettingValue
            {
                Value = "test"
            };

            // Act
            var actual = await adminFundingStreamSettingService.UpdateAsync(model);

            // Assert
            actual.Should().BeTrue();
            _mockSettingValueRepository.Verify(repo => repo.UpdateSettingValue(It.IsAny<SettingValue>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateAsync_Fail_ResultExpected()
        {
            // Arrange
            _mockSettingValueRepository.Setup(repo => repo.UpdateSettingValue(It.IsAny<SettingValue>())).ReturnsAsync(false);
            var adminFundingStreamSettingService = GetAdminFundingStreamSettingService();

            var model = new Models.SettingValue
            {
                Value = "test"
            };

            // Act
            var actual = await adminFundingStreamSettingService.UpdateAsync(model);

            // Assert
            actual.Should().BeFalse();
            _mockSettingValueRepository.Verify(repo => repo.UpdateSettingValue(It.IsAny<SettingValue>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteAsync_ResultExpected()
        {
            // Arrange
            var adminFundingStreamSettingService = GetAdminFundingStreamSettingService();

            var model = new Models.SettingValue
            {
                Value = "test"
            };

            // Act
            var actual = await adminFundingStreamSettingService.DeleteAsync(model);

            // Assert
            actual.Should().BeTrue();
            _mockSettingValueRepository.Verify(repo => repo.DeleteSettingValue(It.IsAny<SettingValue>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteAsync_Fail_ResultExpected()
        {
            // Arrange
            _mockSettingValueRepository.Setup(repo => repo.DeleteSettingValue(It.IsAny<SettingValue>())).ReturnsAsync(false);
            var adminFundingStreamSettingService = GetAdminFundingStreamSettingService();

            var model = new Models.SettingValue
            {
                Value = "test"
            };

            // Act
            var actual = await adminFundingStreamSettingService.DeleteAsync(model);

            // Assert
            actual.Should().BeFalse();
            _mockSettingValueRepository.Verify(repo => repo.DeleteSettingValue(It.IsAny<SettingValue>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task AddAsync_ResultExpected()
        {
            // Arrange
            var adminFundingStreamSettingService = GetAdminFundingStreamSettingService();

            var model = new Models.SettingValue
            {
                Value = "test"
            };

            // Act
            var actual = await adminFundingStreamSettingService.AddAsync(model);

            // Assert
            actual.Should().NotBeNull();
            _mockSettingValueRepository.Verify(repo => repo.AddAsync(It.IsAny<SettingValue>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task AddAsync_Fail_ResultExpected()
        {
            // Arrange
            _mockSettingValueRepository.Setup(repo => repo.AddAsync(It.IsAny<SettingValue>())).ReturnsAsync((SettingValue)null);
            var adminFundingStreamSettingService = GetAdminFundingStreamSettingService();

            var model = new Models.SettingValue
            {
                Value = "test"
            };

            // Act
            var actual = await adminFundingStreamSettingService.AddAsync(model);

            // Assert
            actual.Should().BeNull();
            _mockSettingValueRepository.Verify(repo => repo.AddAsync(It.IsAny<SettingValue>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFirstOrDefaultAsync_ResultExpected()
        {
            // Arrange
            var adminFundingStreamSettingService = GetAdminFundingStreamSettingService();

            // Act
            var actual = await adminFundingStreamSettingService.GetFirstOrDefaultAsync(_settingValueId, _fundingStreamId);

            // Assert
            actual.Should().NotBeNull();
            _mockSettingValueRepository.Verify(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<SettingValue, bool>>>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFirstOrDefaultAsync_Fail_ResultExpected()
        {
            // Arrange
            _mockSettingValueRepository.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<SettingValue, bool>>>(), It.IsAny<string>())).ReturnsAsync((SettingValue)null);
            var adminFundingStreamSettingService = GetAdminFundingStreamSettingService();

            // Act
            var actual = await adminFundingStreamSettingService.GetFirstOrDefaultAsync(_settingValueId, _fundingStreamId);

            // Assert
            actual.Should().BeNull();
            _mockSettingValueRepository.Verify(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<SettingValue, bool>>>(), It.IsAny<string>()), Times.Once);
        }

        #endregion


        #region Private Helpers

        private static IMapper GetMapper()
        {
            return new MapperConfiguration(x => x.AddProfile(new ServicesAutoMapperProfile())).CreateMapper();
        }

        private void SetupDefaultMockBehaviour()
        {
            _mockSettingValueRepository.Setup(repo => repo.UpdateSettingValue(It.IsAny<SettingValue>())).ReturnsAsync(true);
            _mockSettingValueRepository.Setup(repo => repo.DeleteSettingValue(It.IsAny<SettingValue>())).ReturnsAsync(true);
            _mockSettingValueRepository.Setup(repo => repo.AddAsync(It.IsAny<SettingValue>())).ReturnsAsync(new SettingValue());
            _mockSettingValueRepository.Setup(repo => repo.RemoveAsync(It.IsAny<int>())).ReturnsAsync(1);
            _mockSettingValueRepository.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<SettingValue, bool>>>(), It.IsAny<string>())).ReturnsAsync(new SettingValue());
        }

        private AdminFundingStreamSettingService GetAdminFundingStreamSettingService()
        {
            var adminFundingStreamSettingService = new AdminFundingStreamSettingService(
                _mockSettingValueRepository.Object,
                GetMapper());
            return adminFundingStreamSettingService;
        }

        #endregion
    }
}