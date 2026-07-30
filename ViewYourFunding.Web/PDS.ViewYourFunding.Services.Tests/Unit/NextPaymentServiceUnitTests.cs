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
    public class NextPaymentServiceUnitTests
    {
        #region Private Fields

        private readonly Mock<INextPaymentRepository> _nextPaymentRepository;

        private readonly int _fundingStreamId = 0;

        private readonly IMapper _mapper;

        #endregion

        #region Mock Services
        private void SetupMockSettingsServices()
        {
            _nextPaymentRepository.Setup(x => x.GetNextPayments(It.IsAny<int>())).ReturnsAsync(new List<Model.NextPayment>());
            _nextPaymentRepository.Setup(x => x.UpdateNextPayment(It.IsAny<Model.NextPayment>())).ReturnsAsync(true);
            _nextPaymentRepository.Setup(x => x.DeleteNextPayment(It.IsAny<Model.NextPayment>())).ReturnsAsync(true);
            _nextPaymentRepository.Setup(x => x.CreateNextPayment(It.IsAny<Model.NextPayment>())).ReturnsAsync(new Model.NextPayment());
        }
        #endregion


        #region Constructor

        public NextPaymentServiceUnitTests()
        {
            _nextPaymentRepository = new Mock<INextPaymentRepository>();
            _mapper = GetMapper();
        }

        #endregion

        #region Action Tests

        [TestMethod, TestCategory("Unit")]
        public async Task GetNextPayments_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var nextPaymentService = new NextPaymentService(_nextPaymentRepository.Object, _mapper);

            //Act
            var actual = await nextPaymentService.GetNextPayments(_fundingStreamId);

            //Assert
            actual.Should().BeOfType<List<NextPayment>>();
            _nextPaymentRepository.Verify(x => x.GetNextPayments(It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task CreateNextPayments_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var nextPaymentService = new NextPaymentService(_nextPaymentRepository.Object, _mapper);

            //Act
            var actual = await nextPaymentService.CreateNextPayment(new NextPayment());

            //Assert
            actual.Should().BeOfType<NextPayment>();
            _nextPaymentRepository.Verify(x => x.CreateNextPayment(It.IsAny<Model.NextPayment>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateNextPayments_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var nextPaymentService = new NextPaymentService(_nextPaymentRepository.Object, _mapper);

            //Act
            var actual = await nextPaymentService.UpdateNextPayment(new NextPayment());

            //Assert
            Assert.IsTrue(actual == true);
            _nextPaymentRepository.Verify(x => x.UpdateNextPayment(It.IsAny<Model.NextPayment>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteNextPayment_ReturnsExpectedResult()
        {
            //Arrange
            SetupMockSettingsServices();
            var nextPaymentService = new NextPaymentService(_nextPaymentRepository.Object, _mapper);

            //Act
            var actual = await nextPaymentService.DeleteNextPayment(new NextPayment());

            //Assert
            Assert.IsTrue(actual == true);
            _nextPaymentRepository.Verify(x => x.DeleteNextPayment(It.IsAny<Model.NextPayment>()), Times.Once);
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
