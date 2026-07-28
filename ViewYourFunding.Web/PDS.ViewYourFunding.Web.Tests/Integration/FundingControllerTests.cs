using ExcelDataReader;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Tests.Constants;
using PDS.ViewYourFunding.Web.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Tests
{
    /// <summary>
    /// The Funding Controller tests.
    /// </summary>
    [TestClass]
    public class FundingControllerTests
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private ApplicationConfiguration _configuration;

        /// <summary>
        /// Setups the di.
        /// </summary>
        [TestInitialize]
        public void SetupDI()
        {
            _configuration = ConfigHelper.GetApplicationConfiguration();
        }

        #region Spreadsheets Tests

        /// <summary>
        /// Generates the spreadsheet DSGF y2021 generates without error.
        /// </summary>
        [TestMethod, TestCategory("Integration"), TestCategory("CoreIntegration")]
        public void GenerateSpreadsheet_DSGFY2021_GeneratesWithoutError()
        {
            // Arrange
            var controller = GetViewYourFundingDocumentController();

            var cutOffDate = new DateTime(2022, 1, 1);
            var publicationDate = new DateTime(2022, 1, 2);

            // Act
            Func<Task> act = async () =>
            {
                await controller.GenerateFundingDocument(
                    FundingStreamCode.DSG,
                    "FY-2021",
                    cutOffDate,
                    publicationDate,
                    1);
            };

            // Assert
            act.Should().NotThrowAsync<Exception>();
        }

        /// <summary>
        /// Gets the spreadsheet path DSGF y2021 spreadsheet renders with correct number of worksheets.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Integration"), TestCategory("CoreIntegration")]
        public async Task GetSpreadsheetPath_DSGFY2021_SpreadsheetRendersWithCorrectNumberOfWorksheets()
        {
            // Arrange
            var controller = GetViewYourFundingDocumentController();

            var cutOffDate = new DateTime(2022, 1, 1);
            var publicationDate = new DateTime(2022, 1, 2);
            int? modelVersion = null;

            // Act
            var pathResponse = await controller.GetFundingDocumentMetadata(
                FundingStreamCode.DSG,
                "FY-2021",
                cutOffDate,
                publicationDate,
                modelVersion,
                true);

            var path = pathResponse.First(res => res.FileExtension == FundingDocumentFileType.Spreadsheet_ExcelFormat).FilePath;

            var spreadsheet = GetSpreadsheet(path, FileFormat.XLS);

            // Assert
            spreadsheet.Tables.Count.Should().Be(6);

            path = pathResponse.First(res => res.FileExtension == FundingDocumentFileType.Spreadsheet_CSVFormat).FilePath;
            spreadsheet = GetSpreadsheet(path, FileFormat.CSV);

            // Assert
            spreadsheet.Tables.Count.Should().Be(1);
        }

        /// <summary>
        /// Downloads the spreadsheet valid internal name returns status okay.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task DownloadSpreadsheet_ValidInternalName_ReturnsStatusOkay()
        {
            // Arrange
            var internalFilename = "PSG_AY-1920_20200101_120000_Organisation_303.ods";

            var storageService = new Mock<IFundingDocumentStorageService>(MockBehavior.Strict);
            storageService
                .Setup(storageServiceLambda => storageServiceLambda.GetFile(It.IsAny<string>()))
                .ReturnsAsync(new MemoryStream());

            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(userJourneyServiceLambda => userJourneyServiceLambda.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>());

            var controller = new ViewYourFundingDocumentController(
                new Mock<IFundingViewService>().Object,
                storageService.Object,
                userJourneyService.Object,
                new Mock<ILoggerAdapter<ViewYourFundingDocumentController>>().Object,
                new Mock<IMapper>().Object);

            // Act
            var responseMessage = await controller.DownloadSpreadsheet(internalFilename, new DateTime(2020, 2, 25));

            // Assert
            responseMessage.Should().BeOfType<FileStreamResult>()
                .Which.FileDownloadName.Should().Be("_2019-to-2020_published-25-02-2020.ods");

            responseMessage.Should().BeOfType<FileStreamResult>()
                .Which.ContentType.Should().Be("application/vnd.oasis.opendocument.spreadsheet; charset=utf-8");
        }

        /// <summary>
        /// GenerateSpreadsheet generates without error.
        /// </summary>
        [TestMethod, TestCategory("Integration")]
        public void GenerateSpreadsheet_PSGAY1920_GeneratesWithoutError()
        {
            // Arrange
            var controller = GetViewYourFundingDocumentController();

            var cutOffDate = new DateTime(2022, 1, 1);
            var publicationDate = new DateTime(2022, 1, 2);
            int? modelVersion = null;

            // Act
            Func<Task> act = async () =>
            {
                await controller.GenerateFundingDocument(
                    FundingStreamCode.PEAndSport,
                    "AY-1920",
                    cutOffDate,
                    publicationDate,
                    modelVersion);
            };

            // Assert
            act.Should().NotThrowAsync();
        }

        /// <summary>
        /// GenerateSpreadsheet provider generates without error.
        /// </summary>
        [TestMethod, TestCategory("Integration"), TestCategory("CoreIntegration")]
        public void GenerateSpreadsheet_PSGAY1920Provider_GeneratesWithoutError()
        {
            // Arrange
            var controller = GetViewYourFundingDocumentController();

            var cutOffDate = new DateTime(2022, 1, 1);
            var publicationDate = new DateTime(2022, 1, 2);
            const int modelVersion = 1;

            // Act
            Func<Task> act = async () =>
            {
                await controller.GenerateFundingDocument(
                    FundingStreamCode.PEAndSport,
                    "AY-1920",
                    cutOffDate,
                    publicationDate,
                    modelVersion,
                    FundingViewType.Spreadsheet,
                    FundingViewScope.Provider,
                    JsonConvert.SerializeObject(new[]
                    {
                        new SearchFilter
                        {
                            PropertyName = SearchFilterPropertyName.Ukprn,
                            PropertyValue = "10074807"
                        }
                    }));
            };

            // Assert
            act.Should().NotThrowAsync();
        }

        /// <summary>
        /// GenerateSpreadsheet organisation generates without error.
        /// </summary>
        [TestMethod, TestCategory("Integration"), TestCategory("CoreIntegration")]
        public void GenerateSpreadsheet_PSGAY1920Organisation_GeneratesWithoutError()
        {
            // Arrange
            var controller = GetViewYourFundingDocumentController();

            var cutOffDate = new DateTime(2022, 1, 1);
            var publicationDate = new DateTime(2022, 1, 2);
            const int modelVersion = 1;

            // Act
            Func<Task> act = async () =>
            {
                await controller.GenerateFundingDocument(
                    FundingStreamCode.PEAndSport,
                    "AY-1920",
                    cutOffDate,
                    publicationDate,
                    modelVersion,
                    FundingViewType.Spreadsheet,
                    FundingViewScope.Organisation,
                    JsonConvert.SerializeObject(new[]
                    {
                        new SearchFilter
                        {
                            PropertyName = SearchFilterPropertyName.PrimaryIdentifier,
                            PropertyValue = "303"
                        }
                    }));
            };

            // Assert
            act.Should().NotThrowAsync();
        }

        /// <summary>
        /// Generates the spreadsheet DSGF y2021 organisation generates without error.
        /// </summary>
        [TestMethod, TestCategory("Integration"), TestCategory("CoreIntegration")]
        public void GenerateSpreadsheet_DSGFY2021Organisation_GeneratesWithoutError()
        {
            // Arrange
            var controller = GetViewYourFundingDocumentController();

            var cutOffDate = new DateTime(2022, 1, 1);
            var publicationDate = new DateTime(2022, 1, 2);
            const int modelVersion = 1;

            // Act
            Func<Task> act = async () =>
            {
                await controller.GenerateFundingDocument(
                    FundingStreamCode.DSG,
                    "FY-2021",
                    cutOffDate,
                    publicationDate,
                    modelVersion,
                    FundingViewType.Spreadsheet,
                    FundingViewScope.Organisation,
                    JsonConvert.SerializeObject(new[]
                    {
                        new SearchFilter
                        {
                            PropertyName = SearchFilterPropertyName.ParentPrimaryIdentifier,
                            PropertyValue = "303"
                        }
                    }));
            };

            // Assert
            act.Should().NotThrowAsync();
        }

        /// <summary>
        /// Gets the spreadsheet path psga y1920 spreadsheet renders with correct number of worksheets.
        /// </summary>
        /// <returns><see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Integration"), TestCategory("CoreIntegration")]
        public async Task GetSpreadsheetPath_PSGAY1920_SpreadsheetRendersWithCorrectNumberOfWorksheets()
        {
            // Arrange
            var controller = GetViewYourFundingDocumentController();

            var cutOffDate = new DateTime(2022, 1, 1);
            var publicationDate = new DateTime(2022, 1, 2);
            int? modelVersion = null;

            // Act
            var pathResponse = await controller.GetFundingDocumentMetadata(
                FundingStreamCode.PEAndSport,
                "AY-1920",
                cutOffDate,
                publicationDate,
                modelVersion,
                true);

            var path = pathResponse.First(res => res.FileExtension == FundingDocumentFileType.Spreadsheet_ExcelFormat).FilePath;
            var spreadsheet = GetSpreadsheet(path, FileFormat.XLS);

            // Assert
            spreadsheet.Tables.Count.Should().BeGreaterThanOrEqualTo(6); // If there is closures data there will be 7, else 6

            path = pathResponse.First(res => res.FileExtension == FundingDocumentFileType.Spreadsheet_CSVFormat).FilePath;
            spreadsheet = GetSpreadsheet(path, FileFormat.CSV);

            // Assert
            spreadsheet.Tables.Count.Should().Be(1);
        }

        [TestMethod, TestCategory("Integration"), TestCategory("CoreIntegration")]
        public void GetMaxUIAndSpreadsheetVersionNumbers_ForDSG_EnsureCorrectMaxValuesReturned()
        {
            // Arrange
            var controller = GetViewYourFundingDocumentController();

            // Act
            var maxUIAndSpreadsheetVersionNumbers = controller.GetMaxUiAndSpreadsheetVersionNumbers(FundingStreamCode.DSG, null);

            // Assert
            maxUIAndSpreadsheetVersionNumbers.MaximumSpreadsheetVersion.Should().Be(9);
            maxUIAndSpreadsheetVersionNumbers.MaximumUiVersion.Should().BeNull();
        }

        [TestMethod, TestCategory("Integration"), TestCategory("CoreIntegration")]
        public void GetMaxUIAndSpreadsheetVersionNumbers_ForPeAndSports_EnsureCorrectMaxValuesReturned()
        {
            // Arrange
            var controller = GetViewYourFundingDocumentController();

            // Act
            var maxUIAndSpreadsheetVersionNumbers = controller.GetMaxUiAndSpreadsheetVersionNumbers(FundingStreamCode.PEAndSport, null);

            // Assert
            maxUIAndSpreadsheetVersionNumbers.MaximumSpreadsheetVersion.Should().Be(1);
            maxUIAndSpreadsheetVersionNumbers.MaximumUiVersion.Should().BeNull();
        }

        #endregion Spreadsheets Tests

        /// <summary>
        /// Gets the spreadsheet.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fileFormat">File format e.g. CSV, xls, ods.</param>
        /// <returns>The Data set.</returns>
        private static DataSet GetSpreadsheet(string path, FileFormat fileFormat)
        {
            var spreadsheet = new DataSet();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            HttpClient client = new HttpClient();

            var webStream = client.GetStreamAsync(path).Result;
            using (var copiedStream = new MemoryStream())
            {
                webStream.CopyTo(copiedStream);

                var spreadsheetReader = CreateSpreadsheetReader(fileFormat, copiedStream);

                using (var reader = spreadsheetReader)
                {
                    spreadsheet = reader.AsDataSet();
                }
            }

            return spreadsheet;
        }

        /// <summary>
        /// Creates a speadsheet reader.
        /// </summary>
        /// <param name="fileFormat">File format e.g. CSV, xls, ods.</param>
        /// <param name="fileStream">The filestream.</param>
        /// <returns>A spreadsheet reader.</returns>
        private static IExcelDataReader CreateSpreadsheetReader(FileFormat fileFormat, Stream fileStream)
        {
            switch (fileFormat)
            {
                case FileFormat.CSV:
                    return ExcelReaderFactory.CreateCsvReader(fileStream);
                case FileFormat.XLS:
                    return ExcelReaderFactory.CreateReader(fileStream);
            }

            return ExcelReaderFactory.CreateReader(fileStream);
        }

        /// <summary>
        /// Mock of the logging service for model funding view service.
        /// </summary>
        /// <returns>A mock logger.</returns>
        private static Mock<ILoggerAdapter<ModelFundingViewService>> LoggingServiceMockMFVS()
        {
            var item = new Mock<ILoggerAdapter<ModelFundingViewService>>();

            item.Setup(s => s.LogInformation(It.IsAny<string>()));

            return item;
        }

        /// <summary>
        /// Mock of the logging service for view your funding document control.
        /// </summary>
        /// <returns>A mock logger.</returns>
        private static Mock<ILoggerAdapter<ViewYourFundingDocumentController>> LoggingServiceMockVYFDC()
        {
            var item = new Mock<ILoggerAdapter<ViewYourFundingDocumentController>>();

            item.Setup(s => s.LogInformation(It.IsAny<string>()));

            return item;
        }

        /// <summary>
        /// Fundings the API.
        /// </summary>
        /// <returns>The Mock Funding APi.</returns>
        private static Mock<IFundingApiService> FundingApi()
        {
            var item = new Mock<IFundingApiService>();

            item.Setup(s => s.SearchFunding(It.IsAny<FundingApiSearchRequestObject>()))
                .ReturnsAsync(new FundingApiSearchResponse
                {
                    Funding = new List<IFundingApiSearchFunding>
                    {
                        new FundingApiSearchFunding()
                    },
                    ProviderFunding = null
                });

            item.Setup(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false))
                .ReturnsAsync(new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = new List<IFundingApiSearchProviderFunding>
                    {
                        new FundingApiSearchProviderFunding()
                    }
                });

            return item;
        }

        /// <summary>
        /// Gets the view your funding document controller.
        /// </summary>
        /// <returns>the Viewyourfunding document controller.</returns>
        private ViewYourFundingDocumentController GetViewYourFundingDocumentController()
        {
            var aspose = new AsposeDocumentManagementService(null);
            aspose.EnableCellsLicense();

            var worksheetTestsDirectory =
                Path.GetDirectoryName(Assembly.GetAssembly(typeof(ViewYourFundingDocumentController)).Location);
            var localModelFileStoreService = new LocalModelFileStoreService(worksheetTestsDirectory);

            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            IList<FundingStream> fundingStreams = new List<FundingStream>
            {
                new FundingStream
                {
                    FundingStreamCode = "PSG",
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue()
                    }
                },
                new FundingStream
                {
                    FundingStreamCode = "DSG",
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue()
                    }
                }
            };
            userJourneyService.Setup(ujs => ujs.GetFundingStreams()).ReturnsAsync(fundingStreams);

            var mapper = new Mock<IMapper>();
            var mappedFundingStreams = new List<PDS.ViewYourFunding.Web.Models.FundingStream.FundingStream>
            {
                new PDS.ViewYourFunding.Web.Models.FundingStream.FundingStream
                {
                    FundingStreamCode = "PSG"
                },
                new PDS.ViewYourFunding.Web.Models.FundingStream.FundingStream
                {
                    FundingStreamCode = "DSG"
                }
            };

            mapper.Setup(m => m.Map<List<Web.Models.FundingStream.FundingStream>>(It.IsAny<List<FundingStream>>()))
                .Returns(mappedFundingStreams);

            var componentConfigurationService = new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object);
            var mockGlobalSettingService = CommonMocks.GlobalSettingService();

            var controller = new ViewYourFundingDocumentController(
                new ModelFundingViewService(
                    mockGlobalSettingService.Object,
                    localModelFileStoreService,
                    componentConfigurationService,
                    aspose,
                    LoggingServiceMockMFVS().Object,
                    FundingApi().Object,
                    null,
                    null,
                    new MemoryCacheService(null, 0)),
                new AzureBlobStorageFundingDocumentService(
                    _configuration.BlobStorage.ServiceName,
                    _configuration.BlobStorage.Key,
                    _configuration.BlobStorage.ContainerName,
                    new Mock<ILoggerAdapter<AzureBlobStorageFundingDocumentService>>().Object),
                userJourneyService.Object,
                LoggingServiceMockVYFDC().Object,
                mapper.Object);
            return controller;
        }

        private Mock<IBasePathService> GetBasePathService()
        {
            var basePathService = new Mock<IBasePathService>(MockBehavior.Strict);
            basePathService
                .Setup(s => s.GetApplicationBasePath())
                .Returns("Base Path");
            basePathService
                .Setup(s => s.GetUrlForLoggedInProviderPath())
                .Returns("Url For Logged In Provider Path");

            return basePathService;
        }
    }
}