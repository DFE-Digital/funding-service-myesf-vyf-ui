using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Config;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    [TestClass]
    public class UserJourneyServiceUnitTests
    {
        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingStream_WithSetupData_HasCorrectInfo()
        {
            // Arrange
            var settingsService = new UserJourneyService(
                GetCacheService(),
                GetImapper(),
                null,
                GetFundingRepository());

            // Act
            var fundingStream = await settingsService.GetFundingStream("DSG");

            // Assert
            fundingStream.FundingStreamName.Should().Be("Dedicated schools grant");
        }

        [TestMethod, TestCategory("Unit")]
        public void GetFundingStream_WithoutSetupData_ThrowsError()
        {
            // Arrange
            var settingsService = new UserJourneyService(
                GetCacheService(),
                GetImapper(),
                null,
                GetFundingRepository());

            // Act
            Func<Task> act = async () => await settingsService.GetFundingStream("ABC");

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("No funding stream can be found with code 'ABC'");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetPublications_WithSetupData_HasCorrectInfo()
        {
            // Arrange
            var settingsService = new UserJourneyService(
                GetCacheService(),
                GetImapper(),
                null,
                GetFundingRepository());

            // Act
            var publications = await settingsService.GetPublications("DSG");

            // Assert
            publications.Should().HaveCount(2);
        }

        [TestMethod, TestCategory("Unit")]
        public void GetPublications_WithoutSetupData_HasCorrectInfo()
        {
            // Arrange
            var settingsService = new UserJourneyService(
                GetCacheService(),
                GetImapper(),
                null,
                GetFundingRepository());

            // Act
            Func<Task> act = async () => await settingsService.GetFundingStream("DEF");

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("No funding stream can be found with code 'DEF'");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetSettings_WithSetupData_HasCorrectInfo()
        {
            // Arrange
            var settingsService = new UserJourneyService(
                GetCacheService(),
                GetImapper(),
                null,
                GetFundingRepository());

            // Act
            var settings = await settingsService.GetSettings("DSG");

            // Assert
            settings.Should().HaveCount(2);
        }

        private ICacheService GetCacheService()
        {
            return new MemoryCacheService(null, 0);
        }

        private IFundingStreamRepository GetFundingRepository()
        {
            var mockFundingRepo = new Mock<IFundingStreamRepository>();
            mockFundingRepo.Setup(x => x.GetAllFundingStreams(It.IsAny<bool>(), It.IsAny<FetchData[]>())).ReturnsAsync(GetFundingStreams());

            return mockFundingRepo.Object;
        }

        private IEnumerable<Repositories.DataModels.FundingStream> GetFundingStreams()
        {
            return new List<Repositories.DataModels.FundingStream>
            {
                new Repositories.DataModels.FundingStream
                {
                    FundingStreamName = "Dedicated schools grant",
                    FundingStreamCode = "DSG",
                    Publications = new List<Repositories.DataModels.Publication>
                    {
                        new Repositories.DataModels.Publication(),
                        new Repositories.DataModels.Publication()
                    },
                    SettingValues = new List<Repositories.DataModels.SettingValue>
                    {
                        new Repositories.DataModels.SettingValue(),
                        new Repositories.DataModels.SettingValue()
                    }
                }
            };
        }

        private IMapper GetImapper()
        {
            return new MapperConfiguration(x => x.AddProfile(new ServicesAutoMapperProfile())).CreateMapper();
        }
    }
}