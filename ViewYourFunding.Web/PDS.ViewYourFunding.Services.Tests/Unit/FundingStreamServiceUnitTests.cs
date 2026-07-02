using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Config;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Models;
using System.Threading.Tasks;
using Model = PDS.ViewYourFunding.Repositories.DataModels;


namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    [TestClass]
    public class FundingStreamServiceUnitTests
    {
        #region Private Fields

        private readonly Mock<IFundingStreamRepository> _fundingStreamRepository;

        private readonly IMapper _mapper;

        #endregion


        #region Mock Services
        private void SetupMockSettingsServices()
        {
            _fundingStreamRepository.Setup(x => x.UpdateFundingStream(It.IsAny<Model.FundingStream>())).ReturnsAsync(true);
            _fundingStreamRepository.Setup(x => x.DeleteFundingStream(It.IsAny<Model.FundingStream>())).ReturnsAsync(true);
            _fundingStreamRepository.Setup(x => x.CreateFundingStream(It.IsAny<Model.FundingStream>())).ReturnsAsync(new Model.FundingStream());
        }


        #endregion


        #region Constructor

        public FundingStreamServiceUnitTests()
        {
            _fundingStreamRepository = new Mock<IFundingStreamRepository>();
            _mapper = GetMapper();
        }

        #endregion


        #region Action Tests

        [TestMethod, TestCategory("Unit")]
        public async Task CreateFundingStream_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var fundingStreamService = new FundingStreamService(_fundingStreamRepository.Object, _mapper);

            //Act
            var actual = await fundingStreamService.CreateFundingStream(new FundingStream());

            //Assert
            actual.Should().BeOfType<FundingStream>();
            _fundingStreamRepository.Verify(x => x.CreateFundingStream(It.IsAny<Model.FundingStream>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task CreateFundingStream_FailstoCreate()
        {
            //Arrange
            Model.FundingStream fs = null;
            _fundingStreamRepository.Setup(x => x.CreateFundingStream(It.IsAny<Model.FundingStream>())).ReturnsAsync(fs);
            var fundingStreamService = new FundingStreamService(_fundingStreamRepository.Object, _mapper);

            //Act
            var actual = await fundingStreamService.CreateFundingStream(new FundingStream());

            //Assert
            Assert.IsTrue(actual == null);
            _fundingStreamRepository.Verify(x => x.CreateFundingStream(It.IsAny<Model.FundingStream>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateFundingStream_FailsToUpdate()
        {
            //Arrange
            _fundingStreamRepository.Setup(x => x.UpdateFundingStream(It.IsAny<Model.FundingStream>())).ReturnsAsync(false);
            var fundingStreamService = new FundingStreamService(_fundingStreamRepository.Object, _mapper);

            //Act
            var actual = await fundingStreamService.UpdateFundingStream(new FundingStream());

            //Assert
            Assert.IsTrue(actual == false);
            _fundingStreamRepository.Verify(x => x.UpdateFundingStream(It.IsAny<Model.FundingStream>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateFundingStream_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var fundingStreamService = new FundingStreamService(_fundingStreamRepository.Object, _mapper);

            //Act
            var actual = await fundingStreamService.UpdateFundingStream(new FundingStream());

            //Assert
            Assert.IsTrue(actual == true);
            _fundingStreamRepository.Verify(x => x.UpdateFundingStream(It.IsAny<Model.FundingStream>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteFundingStream_FailsToDelete()
        {
            //Arrange
            _fundingStreamRepository.Setup(x => x.DeleteFundingStream(It.IsAny<Model.FundingStream>())).ReturnsAsync(false);
            var fundingStreamService = new FundingStreamService(_fundingStreamRepository.Object, _mapper);

            //Act
            var actual = await fundingStreamService.DeleteFundingStream(new FundingStream());

            //Assert
            Assert.IsTrue(actual == false);
            _fundingStreamRepository.Verify(x => x.DeleteFundingStream(It.IsAny<Model.FundingStream>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteFundingStream_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var fundingStreamService = new FundingStreamService(_fundingStreamRepository.Object, _mapper);

            //Act
            var actual = await fundingStreamService.DeleteFundingStream(new FundingStream());

            //Assert
            Assert.IsTrue(actual == true);
            _fundingStreamRepository.Verify(x => x.DeleteFundingStream(It.IsAny<Model.FundingStream>()), Times.Once);
        }

        #endregion

        private IMapper GetMapper()
        {
            return new MapperConfiguration(x => x.AddProfile(new ServicesAutoMapperProfile())).CreateMapper();
        }
    }
}