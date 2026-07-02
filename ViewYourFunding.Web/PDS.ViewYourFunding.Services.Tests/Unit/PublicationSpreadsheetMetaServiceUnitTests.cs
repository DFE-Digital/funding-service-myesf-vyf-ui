using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    [TestClass]
    public class PublicationSpreadsheetMetaServiceUnitTests
    {
        private readonly Mock<IFundingDocumentStorageService> _mockFundingDocumentStorageService;

        public PublicationSpreadsheetMetaServiceUnitTests()
        {
            _mockFundingDocumentStorageService = new Mock<IFundingDocumentStorageService>(MockBehavior.Strict);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetPublicationSpreadsheetMetaAsync_ReturnsExpectedValue()
        {
            //Arrange
            var service = new PublicationSpreadsheetMetaDataService(_mockFundingDocumentStorageService.Object);
            _mockFundingDocumentStorageService.Setup(x => x.GetMetadata(It.IsAny<string>()))
                .ReturnsAsync(new FundingDocumentMetaResponse
                {
                    CreatedDate = DateTime.Now.ToUniversalTime()
                });

            //Act
            var result = await service.GetPublicationSpreadsheetMetaDataAsync(new Publication
            {
                FundingStream = new FundingStream()
            });

            //Assert
            result.CreatedDateTime.Should().NotBeNull();
            _mockFundingDocumentStorageService.Verify(x => x.GetMetadata(It.IsAny<string>()), Times.Once);
        }
    }
}