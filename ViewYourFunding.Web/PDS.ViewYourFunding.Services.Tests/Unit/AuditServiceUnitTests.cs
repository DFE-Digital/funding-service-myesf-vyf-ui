using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    [TestClass, TestCategory("Unit")]
    public class AuditServiceUnitTests
    {
        private readonly Mock<ICosmosDbService<DataImportAuditModel>> _mockCosmosDbService;

        public AuditServiceUnitTests()
        {
            _mockCosmosDbService = new Mock<ICosmosDbService<DataImportAuditModel>>(MockBehavior.Strict);
        }

        [TestMethod]
        public async Task GetAsync_ReturnsExpected()
        {
            // Arrange
            var service = GetAuditService();
            _mockCosmosDbService.Setup(
                mock => mock.RunQueryAsync<DataImportAuditModel>(It.IsAny<string>())).ReturnsAsync(GetDataImportAudit());


            // Act
            var layoutModel = await service.GetDataImportAudit();

            // Assert
            layoutModel.Should().BeEquivalentTo(GetDataImportAudit());

            _mockCosmosDbService.Verify();
        }

        [TestMethod]
        public async Task GetForFundingStereamAsync_ReturnsExpected()
        {
            // Arrange
            var service = GetAuditService();
            _mockCosmosDbService.Setup(
                mock => mock.RunQueryAsync<DataImportAuditModel>(It.IsAny<string>())).ReturnsAsync(GetDataImportAudit());


            // Act
            var layoutModel = await service.GetDataImportAudit("1619");

            // Assert
            layoutModel.Should().BeEquivalentTo(GetDataImportAudit());

            _mockCosmosDbService.Verify();
        }

        private List<DataImportAuditModel> GetDataImportAudit()
        {
            List<DataImportAuditModel> result = new List<DataImportAuditModel>
            {
                new DataImportAuditModel
                {
                    Id = "Id",
                    StartDateTime = new DateTime(2021, 5, 1),
                    Status = "Successful"
                }
            };

            return result;
        }

        private AuditService GetAuditService()
        {
            return new AuditService(_mockCosmosDbService.Object);
        }
    }
}
