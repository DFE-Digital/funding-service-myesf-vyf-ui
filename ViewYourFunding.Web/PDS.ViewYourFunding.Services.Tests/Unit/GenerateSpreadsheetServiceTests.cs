using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Implementations;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    [TestClass]
    public class GenerateSpreadsheetServiceTests
    {
        private readonly Mock<ILoggerAdapter<GenerateSpreadsheetService>> _mockLogger;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly Mock<DelegatingHandler> _mockHandler;
        private HttpClient _httpClient;

        public GenerateSpreadsheetServiceTests()
        {
            _mockLogger = new Mock<ILoggerAdapter<GenerateSpreadsheetService>>(MockBehavior.Strict);
            _applicationConfiguration = SetupMockApplicationConfiguration();
            _mockHandler = new Mock<DelegatingHandler>(MockBehavior.Strict);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GenerateFundingStreamSpreadSheetByUrlAsyncAsync_Success()
        {
            //Arrange
            var generateSpreadsheetService = GetGenerateSpreadsheetServiceAndSetupGeneralMocks(HttpStatusCode.OK);

            //Act
            var result =
                await generateSpreadsheetService.GenerateFundingStreamSpreadSheetByUrlAsync("test", "test", "test");

            //Assert
            _mockLogger.Verify();
            _mockHandler.Verify();
            result.Success.Should().BeTrue();
        }


        [TestMethod, TestCategory("Unit")]
        public async Task GenerateFundingStreamSpreadSheetByUrlAsyncAsync_Failure()
        {
            //Arrange
            _mockLogger
                .Setup(s => s.LogError(It.IsAny<string>(), It.IsAny<Exception>()))
                .Verifiable();
            var generateSpreadsheetService = GetGenerateSpreadsheetServiceAndSetupGeneralMocks(HttpStatusCode.InternalServerError);

            //Act
            var result =
                await generateSpreadsheetService.GenerateFundingStreamSpreadSheetByUrlAsync("test", "test", "test");

            //Assert
            _mockLogger.Verify();
            _mockHandler.Verify();
            result.Success.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GenerateFundingStreamSpreadSheetByUrlAsyncAsync_Exception()
        {
            //Arrange
            _mockLogger
                .Setup(s => s.LogError(It.IsAny<string>(), It.IsAny<Exception>()))
                .Verifiable();
            var generateSpreadsheetService = GetGenerateSpreadsheetServiceAndSetupGeneralMocks(HttpStatusCode.InternalServerError, true);

            //Act
            var result =
                await generateSpreadsheetService.GenerateFundingStreamSpreadSheetByUrlAsync("test", "test", "test");

            //Assert
            _mockLogger.Verify();
            _mockHandler.Verify();
            result.Success.Should().BeFalse();
        }

        private GenerateSpreadsheetService GetGenerateSpreadsheetServiceAndSetupGeneralMocks(
            HttpStatusCode httpStatusCode, bool exception = false)
        {
            //Set up Mock Message handler
            if (exception)
            {
                _mockHandler
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .Throws(new Exception())
                    .Verifiable();
            }
            else
            {
                _mockHandler
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage(httpStatusCode))
                    .Verifiable();
            }

            _httpClient = new HttpClient(_mockHandler.Object);

            // logging
            _mockLogger
                .Setup(s => s.LogDebug(It.IsAny<string>()))
                .Verifiable();

            return new GenerateSpreadsheetService(
                _mockLogger.Object,
                _httpClient,
                _applicationConfiguration);
        }

        private ApplicationConfiguration SetupMockApplicationConfiguration()
        {
            return new ApplicationConfiguration
            {
                ContactUsLink = "http://www.vyf-contactus.com/contactus",
                FeedbackLink = "http://www.vyf-survey.com/survey",
                ViewYourFundingApiBaseAddress = "http://www.vyf-base-api/"
            };
        }
    }
}
