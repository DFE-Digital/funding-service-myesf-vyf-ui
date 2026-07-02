using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Config;
using PDS.ViewYourFunding.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    [TestClass]
    public class AdminPublicationServiceUnitTests
    {
        private readonly Mock<IPublicationRepository> _mockPublicationRepository;
        private readonly Mock<IPublicationLayoutRepository> _mockPublicationLayoutRepository;
        private Models.Publication _inputPublication;

        private Publication _outputPublication;

        public AdminPublicationServiceUnitTests()
        {
            _mockPublicationRepository = new Mock<IPublicationRepository>(MockBehavior.Strict);
            _mockPublicationLayoutRepository = new Mock<IPublicationLayoutRepository>(MockBehavior.Strict);
            SetupMocks();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetPublications_WithSetupData_ReturnsExpected()
        {
            // Arrange
            _mockPublicationLayoutRepository
                .Setup(plr => plr.GetAllAsync(null, null, null))
                .Returns(Task.FromResult((IEnumerable<PublicationLayout>)new List<PublicationLayout>()));

            var publicationService = new AdminPublicationService(
                _mockPublicationRepository.Object,
                _mockPublicationLayoutRepository.Object,
                GetMapper());

            // Act
            var publications = await publicationService.GetPublications(1);

            // Assert
            publications.Count.Should().Be(2);
            publications.Should().BeEquivalentTo(GetPublications());

            _mockPublicationRepository.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetAll_WithSetupData_ReturnsExpected()
        {
            // Arrange
            _mockPublicationLayoutRepository
                .Setup(plr => plr.GetAllAsync(null, null, null))
                .Returns(Task.FromResult((IEnumerable<PublicationLayout>)new List<PublicationLayout>()));

            var publicationService = new AdminPublicationService(
                _mockPublicationRepository.Object,
                _mockPublicationLayoutRepository.Object,
                GetMapper());

            // Act
            var publications = await publicationService.GetAll();

            // Assert
            publications.Count.Should().Be(2);
            publications.Should().BeEquivalentTo(GetPublications());

            _mockPublicationRepository.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task CreatePublication_WithSetupData_ReturnsExpected()
        {
            // Arrange
            _inputPublication = GetPublication();
            _outputPublication = GetDatabasePublication();
            _outputPublication.CreatedAt = DateTime.Now;
            _outputPublication.LastUpdatedAt = DateTime.Now;

            _mockPublicationRepository
                .Setup(x => x.CreatePublication(It.IsAny<Publication>()))
                .ReturnsAsync(_outputPublication);

            var publicationService = new AdminPublicationService(
                _mockPublicationRepository.Object,
                _mockPublicationLayoutRepository.Object,
                GetMapper());

            // Act
            var result = await publicationService.CreatePublication(_inputPublication);

            // Assert
            result.Should().BeEquivalentTo(_outputPublication);
            _mockPublicationRepository.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdatePublication_ReturnsExpected()
        {
            // Arrange
            _inputPublication = GetPublication();
            _outputPublication = GetDatabasePublication();
            _outputPublication.LastUpdatedAt = DateTime.Now;

            _mockPublicationRepository
                .Setup(x => x.UpdatePublication(It.IsAny<Publication>()))
                .Returns(Task.FromResult<bool>(true));

            _mockPublicationLayoutRepository
                .Setup(plr => plr.GetAllAsync(null, null, null))
                .Returns(Task.FromResult((IEnumerable<PublicationLayout>)new List<PublicationLayout>()));

            _mockPublicationLayoutRepository
                .Setup(plr => plr.GetPublicationLayouts(0))
                .Returns(Task.FromResult((IList<PublicationLayout>)new List<PublicationLayout>()));

            var publicationService = new AdminPublicationService(
                _mockPublicationRepository.Object,
                _mockPublicationLayoutRepository.Object,
                GetMapper());

            // Act
            Func<Task> act = async () => await publicationService.UpdatePublication(_inputPublication);

            // Assert
            await act.Should().NotThrowAsync();
            _mockPublicationRepository.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeletePublication_Fails()
        {
            // Arrange
            _inputPublication = GetPublication();
            _mockPublicationRepository
                .Setup(x => x.DeletePublication(It.IsAny<Publication>()))
                .ReturnsAsync(false);
            var publicationService = new AdminPublicationService(
                _mockPublicationRepository.Object,
                _mockPublicationLayoutRepository.Object,
                GetMapper());

            // Act
            var result = await publicationService.DeletePublication(_inputPublication);

            // Assert
            result.Should().BeFalse();
            _mockPublicationRepository.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeletePublication_Success()
        {
            // Arrange
            _inputPublication = GetPublication();
            _mockPublicationRepository
                .Setup(x => x.DeletePublication(It.IsAny<Publication>()))
                .ReturnsAsync(true);
            var publicationService = new AdminPublicationService(
                _mockPublicationRepository.Object,
                _mockPublicationLayoutRepository.Object,
                GetMapper());

            // Act
            var result = await publicationService.DeletePublication(_inputPublication);

            // Assert
            result.Should().BeTrue();
            _mockPublicationRepository.Verify();
        }

        private static List<Publication> GetPublications()
        {
            return new List<Publication>
            {
                new Publication
                {
                    PublicationLayouts = new List<PublicationLayout>()
                },
                new Publication
                {
                    PublicationLayouts = new List<PublicationLayout>()
                }
            };
        }

        private static IMapper GetMapper()
        {
            return new MapperConfiguration(x => x.AddProfile(new ServicesAutoMapperProfile())).CreateMapper();
        }

        private Publication GetDatabasePublication()
        {
            return new Publication
            {
                FundingPeriodCode = nameof(Publication.FundingPeriodCode),
                LastUpdatedBy = nameof(Publication.LastUpdatedBy),
                PublicationLayouts = new List<PublicationLayout>()
            };
        }

        private Models.Publication GetPublication()
        {
            return new Models.Publication
            {
                FundingPeriodCode = nameof(Publication.FundingPeriodCode),
                LastUpdatedBy = nameof(Publication.LastUpdatedBy),
                PublicationLayouts = new List<Models.PublicationLayout>()
            };
        }

        private void SetupMocks()
        {
            _mockPublicationRepository.Setup(x => x.GetPublications(It.IsAny<int>())).ReturnsAsync(
                GetPublications);
            _mockPublicationRepository.Setup(x => x.GetAllAsync(
                It.IsAny<Expression<Func<Publication, bool>>>(),
                It.IsAny<Func<IQueryable<Publication>, IOrderedQueryable<Publication>>>(),
                It.IsAny<string>())).ReturnsAsync(
                GetPublications);
        }
    }
}