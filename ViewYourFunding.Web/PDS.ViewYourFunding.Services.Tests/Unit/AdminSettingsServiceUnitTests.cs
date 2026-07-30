using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Config;
using PDS.ViewYourFunding.Services.Extensions;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    [TestClass]
    public class AdminSettingsServiceUnitTests
    {
        private readonly int _fundingStreamId = 0, _settingId = 0;
        private const string DsgFundingStreamName = "Dedicated schools grant";
        private const string DsgFundingStreamCode = "DSG";

        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingStream_WithSetupData_ReturnsExpected()
        {
            // Arrange
            var adminSettingService = new AdminSettingsService(
                GetMapper(),
                null,
                GetSettingRepo().Object,
                GetFundingStreamRepository().Object);

            // Act
            var fundingStream = await adminSettingService.GetFundingStream(DsgFundingStreamCode);

            // Assert
            fundingStream.FundingStreamName.Should().Be(DsgFundingStreamName);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingStreamById_WithSetupData_ReturnsExpected()
        {
            // Arrange
            var adminSettingService = new AdminSettingsService(
                GetMapper(),
                null,
                GetSettingRepo().Object,
                GetFundingStreamRepository().Object);

            // Act
            var fundingStream = await adminSettingService.GetFundingStreamById(1);

            // Assert
            fundingStream.FundingStreamName.Should().Be(DsgFundingStreamName);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetAllFundingStreams_ReturnsExpected()
        {
            // Arrange
            var adminSettingService = new AdminSettingsService(
                GetMapper(),
                null,
                GetSettingRepo().Object,
                GetFundingStreamRepository().Object);

            // Act
            var fundingStreams = await adminSettingService.GetAllFundingStreams(FetchDataHelper.AllFetchData);

            // Assert
            fundingStreams.Count.Should().Be(1);
            fundingStreams.First().Should().BeEquivalentTo(GetFundingStream());
        }

        [TestMethod, TestCategory("Unit")]
        public void GetFundingStream_WithoutSetupData_ThrowsError()
        {
            // Arrange
            var adminSettingService = new AdminSettingsService(
                GetMapper(),
                null,
                GetSettingRepo().Object,
                GetFundingStreamRepository().Object);

            // Act
            Func<Task> act = async () => await adminSettingService.GetFundingStream("ABC");

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("No funding stream can be found with code 'ABC'");
        }

        [TestMethod, TestCategory("Unit")]
        public void GetFundingStreamById_WithoutSetupData_ThrowsError()
        {
            // Arrange
            var adminSettingService = new AdminSettingsService(
                GetMapper(),
                null,
                GetSettingRepo().Object,
                GetFundingStreamRepository().Object);

            // Act
            Func<Task> act = async () => await adminSettingService.GetFundingStreamById(99);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("No funding stream can be found with id '99'");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetPublications_WithSetupData_ReturnsExpected()
        {
            // Arrange
            var adminSettingService = new AdminSettingsService(
                GetMapper(),
                null,
                GetSettingRepo().Object,
                GetFundingStreamRepository().Object);

            // Act
            var publications = await adminSettingService.GetPublications(DsgFundingStreamCode);

            // Assert
            publications.Should().HaveCount(2);
        }

        [TestMethod, TestCategory("Unit")]
        public void GetPublications_WithoutSetupData_ReturnsExpected()
        {
            // Arrange
            var adminSettingService = new AdminSettingsService(
                GetMapper(),
                null,
                GetSettingRepo().Object,
                GetFundingStreamRepository().Object);

            // Act
            Func<Task> act = async () => await adminSettingService.GetFundingStream("DEF");

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("No funding stream can be found with code 'DEF'");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetSettings_WithSetupData_ReturnsExpected()
        {
            // Arrange
            var adminSettingService = new AdminSettingsService(
                GetMapper(),
                null,
                GetSettingRepo().Object,
                GetFundingStreamRepository().Object);

            // Act
            var settings = await adminSettingService.GetSettings(DsgFundingStreamCode);

            // Assert
            settings.Should().HaveCount(5);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetSettingsById_WithSetupData_ReturnsExpected()
        {
            // Arrange
            var adminSettingService = new AdminSettingsService(
                GetMapper(),
                null,
                GetSettingRepo().Object,
                GetFundingStreamRepository().Object);

            // Act
            var settings = await adminSettingService.GetSettingsById(1);

            // Assert
            settings.Should().HaveCount(5);
        }


        [TestMethod, TestCategory("Unit")]
        public async Task GetSettingById_ResultExpected()
        {
            // Arrange
            var adminSettingService = new AdminSettingsService(
                 GetMapper(),
                 null,
                 GetSettingRepo().Object,
                 GetFundingStreamRepository().Object);

            //// Act
            var actual = await adminSettingService.GetSettingById(_fundingStreamId, _settingId);

            // Assert
            actual
              .Should().BeOfType<Models.SettingValue>();

            GetSettingRepo().Verify(x => x.GetSettingValueById(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        private static Models.FundingStream GetFundingStream()
        {
            return new Models.FundingStream
            {
                Id = 1,
                FundingStreamName = DsgFundingStreamName,
                FundingStreamCode = DsgFundingStreamCode,
                Active = false,
                LastUpdatedBy = null,
                CreatedAt = DateTime.MinValue,
                Publications = new List<Models.Publication>
                {
                    new Models.Publication
                    {
                        PublicationLayouts = new List<Models.PublicationLayout>()
                    },
                    new Models.Publication
                    {
                        PublicationLayouts = new List<Models.PublicationLayout>()
                    }
                },
                SettingValues = new List<Models.SettingValue>
                {
                    new Models.SettingValue(),
                    new Models.SettingValue(),
                    new Models.SettingValue(),
                    new Models.SettingValue(),
                    new Models.SettingValue()
                },
                NextPayments = new List<Models.NextPayment>(),
                NextPaymentTypes = new List<Models.NextPaymentType>(),
                RelevantForOrganisations_Public = true,
                RelevantForNational = true
            };
        }

        private static FundingStream GetFundingStreamFromRepository()
        {
            return new FundingStream
            {
                Id = 1,
                FundingStreamName = DsgFundingStreamName,
                FundingStreamCode = DsgFundingStreamCode,
                Active = false,
                LastUpdatedBy = null,
                CreatedAt = DateTime.MinValue,
                Publications = new List<Publication>
                {
                    new Publication(),
                    new Publication()
                },
                SettingValues = new List<SettingValue>
                {
                    new SettingValue(),
                    new SettingValue(),
                    new SettingValue(),
                    new SettingValue(),
                    new SettingValue()
                },
                RelevantForNational = true,
                RelevantForOrganisations_Public = true
            };
        }

        private static IMapper GetMapper()
        {
            var config = new TypeAdapterConfig();
            config.ConfigureServicesMappings();
            return new Mapper(config);
        }

        private Mock<IFundingStreamRepository> GetFundingStreamRepository()
        {
            var databaseMock = new Mock<IFundingStreamRepository>();

            databaseMock.Setup(x => x.GetAllFundingStreams(It.IsAny<bool>(), It.IsAny<FetchData[]>())).ReturnsAsync(
                new List<FundingStream>
                {
                    GetFundingStreamFromRepository()
                });

            return databaseMock;
        }

        private Mock<ISettingValueRepository> GetSettingRepo()
        {
            var settingRepo = new Mock<ISettingValueRepository>();

            settingRepo.Setup(x => x.GetSettingValueById(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new SettingValue());
            settingRepo.Setup(x => x.UpdateSettingValue(It.IsAny<SettingValue>())).ReturnsAsync(true);
            return settingRepo;
        }
    }
}