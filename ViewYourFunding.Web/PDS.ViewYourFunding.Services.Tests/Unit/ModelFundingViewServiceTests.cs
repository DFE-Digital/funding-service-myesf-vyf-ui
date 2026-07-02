using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    /// <summary>
    /// The ModelFundingViewServiceTests class.
    /// </summary>
    /// <seealso cref="ModelFundingViewService" />
    [TestClass]
    public class ModelFundingViewServiceTests : ModelFundingViewService
    {
        #region Private fields

        private static Mock<ILayoutManagementService> _mockLayoutManagementService = new Mock<ILayoutManagementService>();
        private static Mock<IComponentConfigurationService> _mockComponentConfigurationService = new Mock<IComponentConfigurationService>();
        private static Mock<IGlobalSettingService> _mockGlobalSettingService = new Mock<IGlobalSettingService>();

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelFundingViewServiceTests"/> class.
        /// </summary>
        public ModelFundingViewServiceTests()
         : base(
            GlobalSetting().Object,
            FileStore().Object,
            new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
            DocumentManagement().Object,
            LoggingServiceMock().Object,
            FundingApiSingleFunding().Object,
            ComponentFactory().Object,
            LayoutManagement().Object,
            new MemoryCacheService(null, 0))
        {
        }

        #endregion


        #region Tests

        /// <summary>
        /// Generates the spreadsheet fixed year generates without error.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void GenerateSpreadsheet_FixedYear_GeneratesWithoutError()
        {
            // Arrange
            var fundingData = new Mock<IFundingApiSearchResponseFunding>().Object;
            var providerFundingData = new Mock<IFundingApiSearchResponseProviderFunding>().Object;
            var json = JsonConvert.SerializeObject(new UiModel
            {
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup
                    {
                        Title = "Sheet1",
                        Groups = new List<UiModelGroup>
                        {
                            new UiModelGroup
                            {
                                Title = "Test"
                            }
                        }
                    }
                }
            });

            // Act
            var spreadSheetByteArray = GenerateFundingDocument(
                fundingData,
                providerFundingData,
                new FundingStream { FundingStreamName = "Dedicated schools grant" },
                json,
                "AY-1920",
                2019,
                2020,
                DateTime.Now,
                FileFormat.ODS,
                string.Empty,
                string.Empty,
                false,
                false,
                true);

            // Assert
            spreadSheetByteArray.Should().NotBeNull();
        }

        /// <summary>
        /// Generates the spreadsheet to byte array fake data completes without error.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task GenerateSpreadsheetToByteArray_FakeData_CompletesWithoutError()
        {
            // Arrange
            var publication = new Publication
            {
                CutOffDate = new DateTime(2010, 12, 21),
                PublishedDate = new DateTime(2010, 12, 25),
                SpreadsheetModelVersion = null
            };

            // Act
            var spreadSheetByteArray = await GenerateFundingDocument(
                new FundingStream { FundingStreamCode = "DSG", FundingStreamName = "Dedicated schools grant" },
                "FY-1920",
                publication.PublishedDate,
                publication,
                FundingViewType.Spreadsheet,
                FundingViewScope.National,
                new[] { FileFormat.ODS });

            // Assert
            spreadSheetByteArray.Should().NotBeNull();
            spreadSheetByteArray.First().Data.Should().HaveCountGreaterThan(1);
        }

        /// <summary>
        /// Generates the spreadsheet to byte array fake data multiple organisations completes without error.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task GenerateSpreadsheetToByteArray_FakeDataMultipleOrganisations_CompletesWithoutError()
        {
            // Arrange
            var cutoffDate = new DateTime(2010, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);
            int? modelVersion = null;

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                FileStore().Object,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiMultipleFunding().Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            var publication = new Publication
            {
                CutOffDate = cutoffDate,
                PublishedDate = publicationDate,
                SpreadsheetModelVersion = modelVersion
            };

            // Act
            var spreadSheetByteArray = await service.GenerateFundingDocument(
                new FundingStream { FundingStreamCode = "DSG", FundingStreamName = "Dedicated schools grant" },
                "FY-2021",
                publication.PublishedDate,
                publication,
                FundingViewType.Spreadsheet,
                FundingViewScope.National,
                new[] { FileFormat.ODS });

            // Assert
            spreadSheetByteArray.Should().NotBeNull();
            spreadSheetByteArray.First().Data.Should().HaveCountGreaterThan(1);
        }

        [DataRow("1.5", "3.5", 1)] // lower boundaries of range
        [DataRow("2.5", "4.5", 1)] // upper boundaries of range
        [DataRow("2.0", "4.0", 1)] // middle of range
        [TestMethod, TestCategory("Unit")]
        public void GenerateFundingViewData_ForNationalThatHasValidSchemaTemplateAndModelVersionCombinations_CompletesWithoutError(string schemaVersion, string templateVersion, int modelVersion)
        {
            // Arrange
            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                FileStore("DSG_SchemaMin1-5Max2-5_TemplateMin3-5Max4-5_ModelVersion1.json").Object,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiMultipleFunding(schemaVersion, templateVersion).Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            // Act
            Func<Task> act = async () => await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[1] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication
                {
                    PublishedDate = publicationDate,
                    PublicationLayouts = new List<PublicationLayout>
                    {
                        new PublicationLayout
                        {
                            FundingViewType = FundingViewType.ViewData,
                            FundingViewScope = FundingViewScope.Organisation
                        }
                    }
                },
                modelVersion,
                FundingViewScope.National,
                null);

            // Assert
            act.Should().NotThrowAsync<Exception>();
            act.Should().NotBeNull();
        }

        [DataRow("1.1", "4.1", 1)] // invalid schema (valid range 1.5 to 2.5)
        [DataRow("2.1", "2.7", 1)] // invalid template (valid range 3.5 to 4.5)
        [DataRow("1.1", "2.7", 1)] // invalid schema and template
        [TestMethod, TestCategory("Unit")]
        public void GenerateFundingViewData_ForNationalThatHasInvalidSchemaTemplateCombinations_ThrowsException(string schemaVersion, string templateVersion, int modelVersion)
        {
            // Arrange
            var exceptionMessage = $"ViewData - Template found for fundingStreamCode: DSG SchemaVersion: {schemaVersion} templateVersion: {templateVersion} modelVersion {modelVersion} does not meet requirements";

            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                FileStore("DSG_SchemaMin1-5Max2-5_TemplateMin3-5Max4-5_ModelVersion1.json").Object,
                ComponentConfiguration().Object,
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiMultipleFunding(schemaVersion, templateVersion).Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            // Act
            Func<Task> act = async () => await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.National);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage(exceptionMessage);
        }

        [DataRow("2.1", "4.1", 2, "ViewData - No template found for fundingStreamCode: DSG SchemaVersion:  templateVersion:  modelVersion 2 fundingViewScope: National")]
        [DataRow("1.1", "2.7", 2, "ViewData - No template found for fundingStreamCode: DSG SchemaVersion:  templateVersion:  modelVersion 2 fundingViewScope: National")] // all invalid
        [DataRow("2.1", "2.7", 2, "ViewData - No template found for fundingStreamCode: DSG SchemaVersion:  templateVersion:  modelVersion 2 fundingViewScope: National")] // invalid template and model
        [TestMethod, TestCategory("Unit")]
        public void GenerateFundingViewData_ForNationalThatHasInvalidSchemaTemplateAndModelVersionCombinations_ThrowsException2(
            string schemaVersion,
            string templateVersion,
            int modelVersion,
            string exceptionMessage)
        {
            // Arrange
            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                FileStore("DSG_SchemaMin1-5Max2-5_TemplateMin3-5Max4-5_ModelVersion1.json").Object,
                ComponentConfiguration().Object,
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiMultipleFunding(schemaVersion, templateVersion).Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            // Act
            Func<Task> act = async () => await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.National);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage(exceptionMessage);
        }

        [DataRow("1.5", "3.5", 3)] // lower boundaries of range
        [DataRow("2.5", "4.5", 3)] // upper boundaries of range
        [DataRow("2.0", "4.0", 3)] // middle of range
        [TestMethod, TestCategory("Unit")]
        public void GenerateFundingViewData_ForOrganisationWithOneFileThatHasValidSchemaTemplateAndModelVersionCombinations_CompletesWithoutError(string schemaVersion, string templateVersion, int modelVersion)
        {
            // Arrange
            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                FileStore("DSG_SchemaMin1-5Max2-5_TemplateMin3-5Max4-5_ModelVersion3_Organisation.json").Object,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiMultipleFunding(schemaVersion, templateVersion).Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            // Act
            Func<Task> act = async () => await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.Organisation);

            // Assert
            act.Should().NotThrowAsync<Exception>();
            act.Should().NotBeNull();
        }

        [DataRow("1.1", "4.1", 5)] // invalid schema (valid range 1.5 to 2.5)
        [DataRow("2.1", "2.7", 5)] // invalid template (valid range 3.5 to 4.5)
        [DataRow("2.1", "4.1", 9)] // invalid modelVersion
        [DataRow("1.1", "2.7", 9)] // all invalid
        [DataRow("1.1", "2.7", 5)] // invalid schema and template
        [DataRow("1.1", "4.1", 9)] // invalid schema and model
        [DataRow("2.1", "2.7", 9)] // invalid template and model
        [TestMethod, TestCategory("Unit")]
        public void GenerateFundingViewData_ForOrganisationWithOneFileThatHasInvalidSchemaTemplateAndModelVersionCombinations_ThrowsException(string schemaVersion, string templateVersion, int modelVersion)
        {
            // Arrange
            var exceptionMessage = $"ViewData - No template found for fundingStreamCode: DSG SchemaVersion: {schemaVersion} templateVersion: {templateVersion} modelVersion {modelVersion.ToString()} fundingViewScope: Organisation";

            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                FileStore("DSG_SchemaMin1-5Max2-5_TemplateMin3-5Max4-5_ModelVersion5_Organisation.json").Object,
                ComponentConfiguration().Object,
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiMultipleFunding(schemaVersion, templateVersion).Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            // Act
            Func<Task> act = async () => await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.Organisation);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage(exceptionMessage);
        }

        [DataRow("1.5", "3.5", 5)] // lower boundaries of range
        [DataRow("2.5", "4.5", 5)] // upper boundaries of range
        [DataRow("2.0", "4.0", 5)] // middle of range
        [TestMethod, TestCategory("Unit")]
        public void GenerateFundingViewData_ForOrganisationSummaryWithOneFileThatHasValidSchemaTemplateAndModelVersionCombinations_CompletesWithoutError(string schemaVersion, string templateVersion, int modelVersion)
        {
            // Arrange
            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                FileStore("DSG_SchemaMin1-5Max2-5_TemplateMin3-5Max4-5_ModelVersion5_OrganisationSummary.json").Object,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiMultipleFunding(schemaVersion, templateVersion).Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            // Act
            Func<Task> act = async () => await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.OrganisationSummary);

            // Assert
            act.Should().NotThrowAsync<Exception>();
            act.Should().NotBeNull();
        }

        [DataRow("1.1", "4.1", 6)] // invalid schema (valid range 1.5 to 2.5)
        [DataRow("2.1", "2.7", 6)] // invalid template (valid range 3.5 to 4.5)
        [DataRow("2.1", "4.1", 8)] // invalid modelVersion
        [DataRow("1.1", "2.7", 8)] // all invalid
        [DataRow("1.1", "2.7", 6)] // invalid schema and template
        [DataRow("1.1", "4.1", 8)] // invalid schema and model
        [DataRow("2.1", "2.7", 8)] // invalid template and model
        [TestMethod, TestCategory("Unit")]
        public void GenerateFundingViewData_ForOrganisationSummaryWithOneFileThatHasInvalidSchemaTemplateAndModelVersionCombinations_ThrowsException(string schemaVersion, string templateVersion, int modelVersion)
        {
            // Arrange
            var exceptionMessage = $"ViewData - No template found for fundingStreamCode: DSG SchemaVersion: {schemaVersion} templateVersion: {templateVersion} modelVersion {modelVersion.ToString()} fundingViewScope: OrganisationSummary";

            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                FileStore("DSG_SchemaMin1-5Max2-5_TemplateMin3-5Max4-5_ModelMin5-Max7_OrganisationSummary.json").Object,
                ComponentConfiguration().Object,
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiMultipleFunding(schemaVersion, templateVersion).Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            // Act
            Func<Task> act = async () => await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.OrganisationSummary);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage(exceptionMessage);
        }

        [DataRow("1.5", "3.5", 5)] // lower boundaries of range
        [DataRow("2.5", "4.5", 5)] // upper boundaries of range
        [DataRow("2.0", "4.0", 5)] // middle of range
        [TestMethod, TestCategory("Unit")]
        public void GenerateFundingViewData_ForProviderWithOneFileThatHasValidSchemaTemplateAndModelVersionCombinations_CompletesWithoutError(string schemaVersion, string templateVersion, int modelVersion)
        {
            // Arrange
            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                FileStore("DSG_SchemaMin1-5Max2-5_TemplateMin3-5Max4-5_ModelVersion5_Provider.json").Object,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiMultipleFunding(schemaVersion, templateVersion).Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            // Act
            Func<Task> act = async () => await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.Provider);

            // Assert
            act.Should().NotThrowAsync<Exception>();
            act.Should().NotBeNull();
        }

        [TestMethod, TestCategory("Unit")]
        public void GenerateFundingViewData_ForProviderPDFWithAdditionalFundingStreams_CompletesWithoutError()
        {
            // Arrange
            var schemaVersion = "1.6";
            var templateVersion = "4.0";
            var modelVersion = 1;

            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);

            var fundingApi = FundingApiMultipleFunding(schemaVersion, templateVersion);

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                FileStore("1619_SchemaMin1-5Max2-5_TemplateMin3-5Max4-5_ModelVersion1_Provider.json").Object,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                fundingApi.Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            var fundingStreams = new FundingStream[]
            {
                new FundingStream { FundingStreamCode = "1619" },
                new FundingStream { FundingStreamCode = "NMSS" }
            };

            var filters = new List<SearchFilter>
            {
                new SearchFilter
                {
                    PropertyName = SearchFilterPropertyName.Id,
                    PropertyValue = "AY-2122-Information-303-1_0"
                }
            };

            // Act
            Func<Task> act = async () => await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "AS-2122",
                "1619",
                fundingStreams,
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.Provider,
                filters: filters.ToArray());

            // Assert
            act.Should().NotThrowAsync<Exception>();
            act.Should().NotBeNull();
            fundingApi.Verify(
                x => x.SearchProviderFunding(
                It.Is<FundingApiSearchRequestObject>(
                ro => ro.FundingStreams.Single().FundingStreamCode == "1619"),
                false), Times.Exactly(2));
            fundingApi.Verify(
                x => x.SearchProviderFunding(
                It.Is<FundingApiSearchRequestObject>(
                ro => ro.FundingStreams.Single().FundingStreamCode == "NMSS"),
                false), Times.Exactly(4));
        }


        [DataRow("1.1", "4.1", 6)] // invalid schema (valid range 1.5 to 2.5)
        [DataRow("2.1", "2.7", 6)] // invalid template (valid range 3.5 to 4.5)
        [DataRow("2.1", "4.1", 8)] // invalid modelVersion
        [DataRow("1.1", "2.7", 8)] // all invalid
        [DataRow("1.1", "2.7", 6)] // invalid schema and template
        [DataRow("1.1", "4.1", 8)] // invalid schema and model
        [DataRow("2.1", "2.7", 8)] // invalid template and model
        [TestMethod, TestCategory("Unit")]
        public void GenerateFundingViewData_ForProviderWithOneFileThatHasInvalidSchemaTemplateAndModelVersionCombinations_ThrowsException(string schemaVersion, string templateVersion, int modelVersion)
        {
            // Arrange
            var exceptionMessage = $"ViewData - No template found for fundingStreamCode: DSG SchemaVersion: {schemaVersion} templateVersion: {templateVersion} modelVersion {modelVersion.ToString()} fundingViewScope: Provider";

            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                FileStore("DSG_SchemaMin1-5Max2-5_TemplateMin3-5Max4-5_ModelMin5-Max7_Provider.json").Object,
                ComponentConfiguration().Object,
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiMultipleFunding(schemaVersion, templateVersion).Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            // Act
            Func<Task> act = async () => await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.Provider);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage(exceptionMessage);
        }

        [DataRow("1.5", "3.5", 7)] // lower boundaries of range
        [DataRow("2.5", "4.5", 10)] // upper boundaries of range
        [DataRow("2.0", "4.0", 10)] // middle of range
        [TestMethod, TestCategory("Unit")]
        public void GenerateFundingViewData_ForProviderWithMultipleFilesThatHasValidSchemaTemplateAndModelVersionCombinations_CompletesWithoutError(string schemaVersion, string templateVersion, int modelVersion)
        {
            // Arrange
            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);
            var fileStore = FileStore(
                "DSG_SchemaMin0-0Max1-4_TemplateMin0-0Max3-4_ModelVersion5_Provider.json",
                "DSG_SchemaMin1-5Max2-5_TemplateMin3-5Max4-5_ModelVersion7_Provider.json",
                "DSG_SchemaMin1-5Max2-5_TemplateMin3-5Max4-5_ModelVersion10_Provider.json").Object;

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                fileStore,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiMultipleFunding(schemaVersion, templateVersion).Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            // Act
            Func<Task> act = async () => await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.Provider);

            // Assert
            act.Should().NotThrowAsync<Exception>();
            act.Should().NotBeNull();
        }

        [DataRow("1.1", "4.1", 6)] // invalid schema (valid range 1.5 to 2.5)
        [DataRow("2.1", "2.7", 6)] // invalid template (valid range 3.5 to 4.5)
        [DataRow("2.1", "4.1", 8)] // invalid modelVersion
        [DataRow("1.1", "2.7", 8)] // all invalid
        [DataRow("1.1", "2.7", 6)] // invalid schema and template
        [DataRow("1.1", "4.1", 8)] // invalid schema and model
        [DataRow("2.1", "2.7", 8)] // invalid template and model
        [TestMethod, TestCategory("Unit")]
        public void GenerateFundingViewData_ForProviderWithMultipleFilesThatHasInvalidSchemaTemplateAndModelVersionCombinations_ThrowsException(string schemaVersion, string templateVersion, int modelVersion)
        {
            // Arrange
            var exceptionMessage = $"ViewData - No template found for fundingStreamCode: DSG SchemaVersion: {schemaVersion} templateVersion: {templateVersion} modelVersion {modelVersion.ToString()} fundingViewScope: Provider";

            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);

            var fileStore = FileStore(
                "DSG_SchemaMin0-0Max1-0_TemplateMin3-5Max4-5_ModelMin5-Max7_Provider.json",
                "DSG_SchemaMin1-5Max2-5_TemplateMin3-5Max4-5_ModelMin5-Max7_Provider.json",
                "DSG_SchemaMin0-0Max1-0_TemplateMin0-0Max1-0_ModelMin0-Max100.json",
                "DSG_SchemaMin0-0Max1-0_TemplateMin0-0Max1-0_ModelMin0-Max100_Organisation.json").Object;

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                fileStore,
                ComponentConfiguration().Object,
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiMultipleFunding(schemaVersion, templateVersion).Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            // Act
            Func<Task> act = async () => await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.Provider);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage(exceptionMessage);
        }

        /// <summary>
        /// Generates the funding view data for organisation completes without error.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task GenerateFundingViewData_ForOrganisation_UsingPreviewLayout_CompletesWithoutError()
        {
            // Arrange
            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);
            int? modelVersion = null;
            var previewLayoutViewModel = new PreviewLayoutModel
            {
                FundingStreamId = 1,
                FundingViewScope = FundingViewScope.Organisation,
                LayoutId = Guid.NewGuid().ToString(),
                IsPreview = true
            };

            _mockLayoutManagementService.Setup(mock => mock.GetLayoutAsync(It.IsAny<string>()))
                .ReturnsAsync(new LayoutModel { LayoutJsonData = "{}" });

            // Act
            var actual = await GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate, FundingStreamId = 1 },
                modelVersion,
                FundingViewScope.Organisation,
                previewLayoutModel: previewLayoutViewModel);

            // Assert
            actual.Should().NotBeNull();

            _mockLayoutManagementService.Verify(mock => mock.GetLayoutAsync(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Generates the funding view data for organisation completes without error.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task GenerateFundingViewData_ForOrganisation_CompletesWithoutError()
        {
            // Arrange
            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);
            int? modelVersion = null;

            // Act
            var actual = await GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.Organisation);

            // Assert
            actual.Should().NotBeNull();
        }

        /// <summary>
        /// Generates the funding view data for organisation that has an invalid model file name throws exception.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void GenerateFundingViewData_ForOrganisationThatHasAnInvalidModelFileName_ThrowsException()
        {
            // Arrange
            const string exceptionMessage = "ViewData - No template found for fundingStreamCode: DSG SchemaVersion: 1 templateVersion: 1 modelVersion  fundingViewScope: Organisation";

            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);
            int? modelVersion = null;

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                FileStore("BadFileName.json").Object,
                ComponentConfiguration().Object,
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiSingleFunding().Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            // Act
            Func<Task> act = async () => await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.Organisation);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage(exceptionMessage);
        }

        /// <summary>
        /// Generates the funding view data for organisation that has multiple schema and template version json files to choose from completes without error.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task GenerateFundingViewData_ForOrganisationThatHasMultipleSchemaAndTemplateVersionJsonFilesToChooseFrom_CompletesWithoutError()
        {
            // Arrange
            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);
            int? modelVersion = null;

            var jsonFiles = new[]
            {
                "DSG_SchemaMin0-0Max1-0_TemplateMin0-0Max1-0.json",
                "DSG_SchemaMin0-0Max1-0_TemplateMin0-0Max1-0_Organisation.json",
                "DSG_SchemaMin2-0Max100-1_TemplateMin2-0Max150-1.json",
                "DSG_SchemaMin2-0Max100-1_TemplateMin2-0Max150-1_Organisation.json",
            };

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                FileStore(jsonFiles).Object,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiSingleFunding("100.1", "150.1").Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            // Act
            var actual = await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.Organisation);

            // Assert
            actual.Should().NotBeNull();
        }

        /// <summary>
        /// Generates the funding view data for organisation with an invalid schema and template versions throws exception.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void GenerateFundingViewData_ForOrganisation_WithAnInvalidSchemaAndTemplateVersions_ThrowsException()
        {
            // Arrange
            const string exceptionMessage = "ViewData - No template found for fundingStreamCode: DSG SchemaVersion: 2 templateVersion: 2 modelVersion 2 fundingViewScope: Organisation";

            var cutoffDate = new DateTime(2020, 12, 21);
            var publicationDate = new DateTime(2010, 12, 25);
            int? modelVersion = 2;

            var service = new ModelFundingViewService(
                GlobalSetting().Object,
                FileStore().Object,
                ComponentConfiguration().Object,
                DocumentManagement().Object,
                LoggingServiceMock().Object,
                FundingApiSingleFunding("2.0", "2.0").Object,
                ComponentFactory().Object,
                LayoutManagement().Object,
                new MemoryCacheService(null, 0));

            // Act
            Func<Task> act = async () => await service.GenerateFundingViewData(
                new ComponentService(null, null),
                "FY-2021",
                "DSG",
                new FundingStream[] { new FundingStream { FundingStreamCode = "DSG" } },
                cutoffDate,
                new Publication { PublishedDate = publicationDate },
                modelVersion,
                FundingViewScope.Organisation,
                null);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage(exceptionMessage);
        }

        /// <summary>
        /// Generates the spreadsheet filename valid inputs produces expected output.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void GenerateSpreadsheetFilename_ValidInputs_ProducesExpectedOutput()
        {
            // Arrange
            const string expected = "DSG_FY-1920_20101221_000000.ods";

            var cutoffDate = new DateTime(2010, 12, 21);

            // Act
            var actual = FilenameHelper.BuildFundingDocumentFilename(
                "DSG",
                "FY-1920",
                cutoffDate,
                FileFormat.ODS,
                FundingViewType.Spreadsheet,
                FundingViewScope.National);

            // Assert
            actual.Should().Be(expected);
        }

        #endregion


        #region Helper Methods

        private static Mock<ILoggerAdapter<ModelFundingViewService>> LoggingServiceMock()
        {
            var item = new Mock<ILoggerAdapter<ModelFundingViewService>>();

            item.Setup(s => s.LogInformation(It.IsAny<string>()));

            return item;
        }

        private static Mock<IDocumentManagementService> DocumentManagement()
        {
            var item = new Mock<IDocumentManagementService>();

            var ary = new byte[] { 0, 1, 2 };

            item.Setup(s => s.CreateSpreadsheetWithData(It.IsAny<ISpreadsheet>(), It.IsAny<FileFormat>(), It.IsAny<bool>()))
                .Returns(ary);

            return item;
        }

        private static Mock<IComponentFactory> ComponentFactory()
        {
            return new Mock<IComponentFactory>();
        }

        private static Mock<IFundingApiService> FundingApiSingleFunding(string schemaVersion = "1.0", string templateVersion = "1.0")
        {
            var item = new Mock<IFundingApiService>();

            item.Setup(s => s.SearchFunding(It.IsAny<FundingApiSearchRequestObject>()))
                .ReturnsAsync(new FundingApiSearchResponse
                {
                    Funding = new List<IFundingApiSearchFunding>
                    {
                        new FundingApiSearchFunding
                        {
                            FundingValue = "{}",
                            SchemaVersion = schemaVersion,
                            TemplateVersion = templateVersion
                        }
                    },
                    ProviderFunding = null
                });

            item.Setup(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false))
                .ReturnsAsync(new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = new List<IFundingApiSearchProviderFunding>
                    {
                        new FundingApiSearchProviderFunding
                        {
                            FundingValue = "{}",
                            SchemaVersion = schemaVersion,
                            TemplateVersion = templateVersion
                        }
                    }
                });

            return item;
        }

        private static Mock<IFundingApiService> FundingApiMultipleFunding(string schemaVersion = "1.0", string templateVersion = "1.0")
        {
            var item = new Mock<IFundingApiService>();

            item.Setup(s => s.SearchFunding(It.IsAny<FundingApiSearchRequestObject>()))
                .ReturnsAsync(new FundingApiSearchResponse
                {
                    Funding = new List<IFundingApiSearchFunding>
                    {
                        new FundingApiSearchFunding
                        {
                            FundingValue = "{}",
                            GroupUkprn = "123",
                            SchemaVersion = schemaVersion,
                            TemplateVersion = templateVersion
                        },
                        new FundingApiSearchFunding
                        {
                            FundingValue = "{}",
                            GroupUkprn = "456",
                            SchemaVersion = schemaVersion,
                            TemplateVersion = templateVersion
                        }
                    },
                    ProviderFunding = null
                });

            item.Setup(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false))
                .ReturnsAsync(new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = new List<IFundingApiSearchProviderFunding>
                    {
                        new FundingApiSearchProviderFunding
                        {
                            FundingValue = "{}",
                            OrganisationUkprn = "123",
                            SchemaVersion = schemaVersion,
                            TemplateVersion = templateVersion
                        },
                        new FundingApiSearchProviderFunding
                        {
                            FundingValue = "{}",
                            OrganisationUkprn = "456",
                            SchemaVersion = schemaVersion,
                            TemplateVersion = templateVersion
                        }
                    }
                });

            return item;
        }

        private static Mock<IModelFileStoreService> FileStore(params string[] fileNames)
        {
            if (fileNames.Length == 0)
            {
                fileNames = new[]
                {
                    "DSG_SchemaMin0-0Max1-0_TemplateMin0-0Max1-0.json",
                    "DSG_SchemaMin0-0Max1-0_TemplateMin0-0Max1-0_Organisation.json"
                };
            }

            var mockFileStoreService = new Mock<IModelFileStoreService>();

            mockFileStoreService.Setup(s => s.GetModelFilenames(It.IsAny<string>()))
                .Returns(fileNames);

            mockFileStoreService.Setup(s => s.Exists(It.IsAny<string>())).Returns(true);

            var simpleUIModel = new UiModel
            {
                // Each group is a worksheet
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup
                    {
                        Dataset = new List<UiModelDataset>
                        {
                            new UiModelDataset()
                        },

                        // Headers
                        Groups = new List<UiModelGroup>
                        {
                            new UiModelGroup
                            {
                                Title = "1"
                            },
                            new UiModelGroup
                            {
                                Title = "2"
                            }
                        }
                    },
                }
            };

            var complicatedUIModel = new UiModel
            {
                Dataset = new List<UiModelDataset>
                {
                    new UiModelDataset
                    {
                        DatasetName = "providerFunding",
                        FundingStreamMappings = new[] { "Primary", "NMSS-1" }
                    },
                    new UiModelDataset
                    {
                        DatasetName = "providerFunding",
                        LimitedTo = "Dataset1_First_LaCode",
                        FundingStreamMappings = new[] { "NMSS-1" }
                    },
                    new UiModelDataset
                    {
                        DatasetName = "providerFunding",
                        LimitedTo = "Dataset1_First_LaName",
                        FundingStreamMappings = new[] { "NMSS-1" }
                    },
                    new UiModelDataset
                    {
                        DatasetName = "providerFunding",
                        LimitedTo = "Dataset1_First_ID"
                    },
                    new UiModelDataset
                    {
                        DatasetName = "providerFunding",
                        LimitedTo = "Dataset1_First_GroupName",
                        FundingStreamMappings = new[] { "NMSS-1" }
                    }
                },
                AdditionalFundingStreams = new List<UIModelAdditionalFundingStream>
                {
                    new UIModelAdditionalFundingStream
                    {
                        Id = "NMSS-1",
                        Code = "NMSS",
                        FundingPeriodPrefix = "AY"
                    }
                }.ToArray(),
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup
                    {
                        Title = "OKAY"
                    },
                }
            };

            var serialisedSimpleModel = JsonConvert.SerializeObject(simpleUIModel);
            var serialisedComplicatedModel = JsonConvert.SerializeObject(complicatedUIModel);

            foreach (var fileName in fileNames)
            {
                var useComplicatedModel = fileName.StartsWith("1619_");

                mockFileStoreService
                    .Setup(s => s.ReadFileAsString(fileName))
                    .Returns(useComplicatedModel ? serialisedComplicatedModel : serialisedSimpleModel);
            }

            return mockFileStoreService;
        }

        private static Mock<ILayoutManagementService> LayoutManagement()
        {
            _mockLayoutManagementService = new Mock<ILayoutManagementService>(MockBehavior.Strict);
            return _mockLayoutManagementService;
        }

        private static Mock<IComponentConfigurationService> ComponentConfiguration()
        {
            _mockComponentConfigurationService = new Mock<IComponentConfigurationService>(MockBehavior.Strict);
            return _mockComponentConfigurationService;
        }

        private static Mock<IGlobalSettingService> GlobalSetting()
        {
            _mockGlobalSettingService = CommonMocks.GlobalSettingService();
            return _mockGlobalSettingService;
        }

        private static Mock<IBasePathService> GetBasePathService()
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

        #endregion

    }
}