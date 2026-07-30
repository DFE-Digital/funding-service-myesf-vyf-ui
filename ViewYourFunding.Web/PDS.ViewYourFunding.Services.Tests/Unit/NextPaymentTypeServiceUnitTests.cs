using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Config;
using PDS.ViewYourFunding.Services.Extensions;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model = PDS.ViewYourFunding.Repositories.DataModels;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    [TestClass]
    public class NextPaymentTypeServiceUnitTests
    {
        #region Private Fields

        private readonly Mock<INextPaymentTypeRepository> _nextPaymentTypeRepository;
        private readonly IMapper _mapper;

        private int _fundingStreamId = 0;

        #endregion

        #region Mock Services
        private void SetupMockSettingsServices()
        {
            _nextPaymentTypeRepository.Setup(x => x.GetNextPaymentTypes(It.IsAny<int>())).ReturnsAsync(new List<Model.NextPaymentType>());
            _nextPaymentTypeRepository.Setup(x => x.UpdateNextPaymentType(It.IsAny<Model.NextPaymentType>())).ReturnsAsync(true);
            _nextPaymentTypeRepository.Setup(x => x.DeleteNextPaymentType(It.IsAny<Model.NextPaymentType>())).ReturnsAsync(true);
            _nextPaymentTypeRepository.Setup(x => x.CreateNextPaymentType(It.IsAny<Model.NextPaymentType>())).ReturnsAsync(new Model.NextPaymentType());
            _nextPaymentTypeRepository.Setup(x => x.GetAllNextPaymentTypes()).ReturnsAsync(new List<Model.NextPaymentType>());
        }

        #endregion


        #region Constructor

        public NextPaymentTypeServiceUnitTests()
        {
            _nextPaymentTypeRepository = new Mock<INextPaymentTypeRepository>();
            _mapper = GetMapper();
        }

        #endregion

        #region Action Tests

        [TestMethod, TestCategory("Unit")]
        public async Task GetNextPaymentTypes_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var nextPaymentTypeService = new NextPaymentTypeService(_nextPaymentTypeRepository.Object, _mapper);

            //Act
            var actual = await nextPaymentTypeService.GetNextPaymentTypes(_fundingStreamId);

            //Assert
            actual.Should().BeOfType<List<NextPaymentType>>();
            _nextPaymentTypeRepository.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetAllNextPaymentTypes_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var nextPaymentTypeService = new NextPaymentTypeService(_nextPaymentTypeRepository.Object, _mapper);

            //Act
            var actual = await nextPaymentTypeService.GetAllNextPaymentTypes();

            //Assert
            actual.Should().BeOfType<List<NextPaymentType>>();
            _nextPaymentTypeRepository.Verify(x => x.GetAllNextPaymentTypes(), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task CreateNextPaymentTypes_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var nextPaymentTypeService = new NextPaymentTypeService(_nextPaymentTypeRepository.Object, _mapper);

            //Act
            var actual = await nextPaymentTypeService.CreateNextPaymentType(new NextPaymentType());

            //Assert
            actual.Should().BeOfType<NextPaymentType>();
            _nextPaymentTypeRepository.Verify(x => x.CreateNextPaymentType(It.IsAny<Model.NextPaymentType>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateNextPaymentTypes_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var nextPaymentTypeService = new NextPaymentTypeService(_nextPaymentTypeRepository.Object, _mapper);

            //Act
            var actual = await nextPaymentTypeService.UpdateNextPaymentType(new NextPaymentType());

            //Assert
            Assert.IsTrue(actual == true);
            _nextPaymentTypeRepository.Verify(x => x.UpdateNextPaymentType(It.IsAny<Model.NextPaymentType>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteNextPaymentTypes_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var nextPaymentTypeService = new NextPaymentTypeService(_nextPaymentTypeRepository.Object, _mapper);

            //Act
            var actual = await nextPaymentTypeService.DeleteNextPaymentType(new NextPaymentType());

            //Assert
            Assert.IsTrue(actual == true);
            _nextPaymentTypeRepository.Verify(x => x.DeleteNextPaymentType(It.IsAny<Model.NextPaymentType>()), Times.Once);
        }


        #endregion

        private IMapper GetMapper()
        {
            var config = new TypeAdapterConfig();
            config.ConfigureServicesMappings();
            return new Mapper(config);
        }
    }
}
