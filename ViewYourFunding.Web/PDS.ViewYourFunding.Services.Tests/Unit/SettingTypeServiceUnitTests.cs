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
    public class SettingTypeServiceUnitTests
    {
        #region Private Fields

        private readonly Mock<ISettingTypeRepository> _settingTypeRepository;
        private readonly IMapper _mapper;

        #endregion


        #region Mock Services

        private void SetupMockSettingsServices()
        {
            _settingTypeRepository.Setup(x => x.UpdateSettingType(It.IsAny<Model.Setting>())).ReturnsAsync(true);
            _settingTypeRepository.Setup(x => x.DeleteSettingType(It.IsAny<Model.Setting>())).ReturnsAsync(true);
            _settingTypeRepository.Setup(x => x.CreateSettingType(It.IsAny<Model.Setting>())).ReturnsAsync(new Model.Setting());
        }

        #endregion


        #region Constructor

        public SettingTypeServiceUnitTests()
        {
            _settingTypeRepository = new Mock<ISettingTypeRepository>();
            _mapper = GetMapper();
        }

        #endregion


        #region Action Tests

        [TestMethod, TestCategory("Unit")]
        public async Task GetAllSettingTypes_ReturnsExpectedResult()
        {
            //Arrange
            List<Model.Setting> settingTypes = new List<Model.Setting>
            {
                    new Model.Setting()
                    {
                        Id = 1,
                        SettingName = "Setting Name",
                        SettingDescription = "Setting Description",
                    }
            };
            SetupMockSettingsServices();
            var settingTypeService = new SettingTypeService(_settingTypeRepository.Object, _mapper);
            _settingTypeRepository.Setup(x => x.GetAllSettingTypes()).ReturnsAsync(settingTypes);

            //Act
            var actual = await settingTypeService.GetAllSettingTypes();

            //Assert
            actual.Should().BeOfType<List<SettingType>>();
            actual.Count.Should().Be(1);
            _settingTypeRepository.Verify(x => x.GetAllSettingTypes(), Times.Once);

            _settingTypeRepository.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Never);
            _settingTypeRepository.Verify(x => x.CreateSettingType(It.IsAny<Model.Setting>()), Times.Never);
            _settingTypeRepository.Verify(x => x.UpdateSettingType(It.IsAny<Model.Setting>()), Times.Never);
            _settingTypeRepository.Verify(x => x.DeleteSettingType(It.IsAny<Model.Setting>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetSettingTypeById_ReturnsExpectedResult()
        {
            //Arrange
            Model.Setting settingType = new Model.Setting()
            {
                Id = 1,
                SettingName = "Setting Name",
                SettingDescription = "Setting Description",
            };
            SetupMockSettingsServices();
            var settingTypeService = new SettingTypeService(_settingTypeRepository.Object, _mapper);
            _settingTypeRepository.Setup(x => x.GetSettingTypeById(It.IsAny<int>())).ReturnsAsync(settingType);

            //Act
            var actual = await settingTypeService.GetSettingTypeById(1);

            //Assert
            actual.Should().BeOfType<SettingType>();
            _settingTypeRepository.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Once);

            _settingTypeRepository.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _settingTypeRepository.Verify(x => x.CreateSettingType(It.IsAny<Model.Setting>()), Times.Never);
            _settingTypeRepository.Verify(x => x.UpdateSettingType(It.IsAny<Model.Setting>()), Times.Never);
            _settingTypeRepository.Verify(x => x.DeleteSettingType(It.IsAny<Model.Setting>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetSettingTypeById_SettingTypeNotFound()
        {
            //Arrange
            Model.Setting settingType = null;
            SetupMockSettingsServices();
            var settingTypeService = new SettingTypeService(_settingTypeRepository.Object, _mapper);
            _settingTypeRepository.Setup(x => x.GetSettingTypeById(It.IsAny<int>())).ReturnsAsync(settingType);

            //Act
            var actual = await settingTypeService.GetSettingTypeById(1);

            //Assert
            Assert.IsTrue(actual == null);
            _settingTypeRepository.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Once);

            _settingTypeRepository.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _settingTypeRepository.Verify(x => x.CreateSettingType(It.IsAny<Model.Setting>()), Times.Never);
            _settingTypeRepository.Verify(x => x.UpdateSettingType(It.IsAny<Model.Setting>()), Times.Never);
            _settingTypeRepository.Verify(x => x.DeleteSettingType(It.IsAny<Model.Setting>()), Times.Never);
        }


        [TestMethod, TestCategory("Unit")]
        public async Task CreateSettingType_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var settingTypeService = new SettingTypeService(_settingTypeRepository.Object, _mapper);

            //Act
            var actual = await settingTypeService.CreateSettingType(new SettingType());

            //Assert
            actual.Should().BeOfType<SettingType>();
            _settingTypeRepository.Verify(x => x.CreateSettingType(It.IsAny<Model.Setting>()), Times.Once);

            _settingTypeRepository.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _settingTypeRepository.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Never);
            _settingTypeRepository.Verify(x => x.UpdateSettingType(It.IsAny<Model.Setting>()), Times.Never);
            _settingTypeRepository.Verify(x => x.DeleteSettingType(It.IsAny<Model.Setting>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task CreateSettingType_FailstoCreate()
        {
            //Arrange
            Model.Setting settingType = null;
            _settingTypeRepository.Setup(x => x.CreateSettingType(It.IsAny<Model.Setting>())).ReturnsAsync(settingType);
            var settingTypeService = new SettingTypeService(_settingTypeRepository.Object, _mapper);

            //Act
            var actual = await settingTypeService.CreateSettingType(new SettingType());

            //Assert
            Assert.IsTrue(actual == null);
            _settingTypeRepository.Verify(x => x.CreateSettingType(It.IsAny<Model.Setting>()), Times.Once);

            _settingTypeRepository.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _settingTypeRepository.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Never);
            _settingTypeRepository.Verify(x => x.UpdateSettingType(It.IsAny<Model.Setting>()), Times.Never);
            _settingTypeRepository.Verify(x => x.DeleteSettingType(It.IsAny<Model.Setting>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateSettingType_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var settingTypeService = new SettingTypeService(_settingTypeRepository.Object, _mapper);

            //Act
            var actual = await settingTypeService.UpdateSettingType(new SettingType());

            //Assert
            Assert.IsTrue(actual == true);
            _settingTypeRepository.Verify(x => x.UpdateSettingType(It.IsAny<Model.Setting>()), Times.Once);

            _settingTypeRepository.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _settingTypeRepository.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Never);
            _settingTypeRepository.Verify(x => x.CreateSettingType(It.IsAny<Model.Setting>()), Times.Never);
            _settingTypeRepository.Verify(x => x.DeleteSettingType(It.IsAny<Model.Setting>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateSettingType_FailsToUpdate()
        {
            //Arrange
            _settingTypeRepository.Setup(x => x.UpdateSettingType(It.IsAny<Model.Setting>())).ReturnsAsync(false);
            var settingTypeService = new SettingTypeService(_settingTypeRepository.Object, _mapper);

            //Act
            var actual = await settingTypeService.UpdateSettingType(new SettingType());

            //Assert
            Assert.IsTrue(actual == false);
            _settingTypeRepository.Verify(x => x.UpdateSettingType(It.IsAny<Model.Setting>()), Times.Once);

            _settingTypeRepository.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _settingTypeRepository.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Never);
            _settingTypeRepository.Verify(x => x.CreateSettingType(It.IsAny<Model.Setting>()), Times.Never);
            _settingTypeRepository.Verify(x => x.DeleteSettingType(It.IsAny<Model.Setting>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteSettingType_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var settingTypeService = new SettingTypeService(_settingTypeRepository.Object, _mapper);

            //Act
            var actual = await settingTypeService.DeleteSettingType(new SettingType());

            //Assert
            Assert.IsTrue(actual == true);
            _settingTypeRepository.Verify(x => x.DeleteSettingType(It.IsAny<Model.Setting>()), Times.Once);

            _settingTypeRepository.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _settingTypeRepository.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Never);
            _settingTypeRepository.Verify(x => x.CreateSettingType(It.IsAny<Model.Setting>()), Times.Never);
            _settingTypeRepository.Verify(x => x.UpdateSettingType(It.IsAny<Model.Setting>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteSettingType_FailsToDelete()
        {
            //Arrange
            _settingTypeRepository.Setup(x => x.DeleteSettingType(It.IsAny<Model.Setting>())).ReturnsAsync(false);
            var settingTypeService = new SettingTypeService(_settingTypeRepository.Object, _mapper);

            //Act
            var actual = await settingTypeService.DeleteSettingType(new SettingType());

            //Assert
            Assert.IsTrue(actual == false);
            _settingTypeRepository.Verify(x => x.DeleteSettingType(It.IsAny<Model.Setting>()), Times.Once);

            _settingTypeRepository.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _settingTypeRepository.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Never);
            _settingTypeRepository.Verify(x => x.CreateSettingType(It.IsAny<Model.Setting>()), Times.Never);
            _settingTypeRepository.Verify(x => x.UpdateSettingType(It.IsAny<Model.Setting>()), Times.Never);
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