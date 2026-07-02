using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    /// <summary>
    /// The FundingDocumentServiceTests class.
    /// </summary>
    [TestClass]
    public class FundingDocumentServiceTests
    {
        /// <summary>
        /// The test funding stream code.
        /// </summary>
        private const string TestFundingStreamCode = "TEST";

        /// <summary>
        /// The test funding period code.
        /// </summary>
        private const string TestFundingPeriodCode = "AY-1920";

        /// <summary>
        /// The test file name1.
        /// </summary>
        private const string TestFileName1 = "ABC";

        /// <summary>
        /// The test file name2.
        /// </summary>
        private const string TestFileName2 = "DEF";

        /// <summary>
        /// The test file name3.
        /// </summary>
        private const string TestFileName3 = "GHI";

        /// <summary>
        /// The test published date1.
        /// </summary>
        private static readonly DateTime TestPublishedDate1 = new DateTime(2019, 10, 1);

        /// <summary>
        /// The test published date3.
        /// </summary>
        private static readonly DateTime TestPublishedDate3 = new DateTime(2019, 12, 1);

        /// <summary>
        /// The test cut off date1.
        /// </summary>
        private static readonly DateTime TestCutOffDate1 = new DateTime(2019, 10, 15);

        /// <summary>
        /// The test cut off date2.
        /// </summary>
        private static readonly DateTime TestCutOffDate2 = new DateTime(2019, 11, 15);

        /// <summary>
        /// The test cut off date3.
        /// </summary>
        private static readonly DateTime TestCutOffDate3 = new DateTime(2019, 12, 15);

        /// <summary>
        /// Gets the funding documents for single document returns the single document.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingDocuments_ForSingleDocument_ReturnsTheSingleDocument()
        {
            // Arrange
            var settingsService = new Mock<IUserJourneyService>();
            settingsService.Setup(s => s.GetSettings(TestFundingStreamCode))
                .ReturnsAsync(new List<SettingValue>
                {
                });

            var fundingViewService = new Mock<IFundingViewService>();

            var documentStorageService = new Mock<IFundingDocumentStorageService>();

            documentStorageService
                .Setup(s => s.GetFilenames(TestFundingPeriodCode, TestFundingStreamCode))
                .Returns(new List<string> { TestFileName1 })
                .Verifiable();

            documentStorageService.Setup(s => s.GetMetadata(TestFileName1))
                .ReturnsAsync(new FundingDocumentMetaResponse())
                .Verifiable();

            var documentService = new FundingDocumentService(documentStorageService.Object);

            // Act
            var documents = await documentService.GetFundingDocuments(TestFundingStreamCode, null, TestFundingPeriodCode);

            // Assert
            documentStorageService.Verify();
            documents.Should().HaveCount(1);
        }

        /// <summary>
        /// Gets the funding documents for multiple documents with the same published date returns the single correct document.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingDocuments_ForMultipleDocumentsWithTheSamePublishedDate_ReturnsTheSingleCorrectDocument()
        {
            // Arrange
            var settingsService = new Mock<IUserJourneyService>();
            settingsService.Setup(s => s.GetSettings(TestFundingStreamCode))
                .ReturnsAsync(new List<SettingValue>
                {
                });

            var fundingViewService = new Mock<IFundingViewService>();

            var documentStorageService = new Mock<IFundingDocumentStorageService>();

            documentStorageService
                .Setup(s => s.GetFilenames(TestFundingPeriodCode, TestFundingStreamCode))
                .Returns(new List<string> { TestFileName1, TestFileName2, TestFileName3 })
                .Verifiable();

            documentStorageService.Setup(s => s.GetMetadata(TestFileName1))
                .ReturnsAsync(new FundingDocumentMetaResponse
                {
                    DocumentPublishedDate = TestPublishedDate1,
                    CutOffDate = TestCutOffDate1
                })
                .Verifiable();

            documentStorageService.Setup(s => s.GetMetadata(TestFileName2))
                .ReturnsAsync(new FundingDocumentMetaResponse
                {
                    DocumentPublishedDate = TestPublishedDate1,
                    CutOffDate = TestCutOffDate2
                })
                .Verifiable();

            documentStorageService.Setup(s => s.GetMetadata(TestFileName3))
                .ReturnsAsync(new FundingDocumentMetaResponse
                {
                    DocumentPublishedDate = TestPublishedDate1,
                    CutOffDate = TestCutOffDate3
                })
                .Verifiable();

            var expectedDocuments = new List<FundingDocument>
            {
                new FundingDocument
                {
                    DocumentPublishedDate = TestPublishedDate1,
                    YearFrom = 2019,
                    YearTo = 2020,
                    CutOffDate = TestCutOffDate3,
                    IsFinal = true
                }
            };

            var documentService = new FundingDocumentService(documentStorageService.Object);

            // Act
            var documents = await documentService.GetFundingDocuments(TestFundingStreamCode, null, TestFundingPeriodCode);

            // Assert
            documentStorageService.Verify();
            documents.Should().BeEquivalentTo(expectedDocuments);
        }

        /// <summary>
        /// Gets the funding documents for multiple documents with multiple published dates returns the correct documents.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingDocuments_ForMultipleDocumentsWithMultiplePublishedDates_ReturnsTheCorrectDocuments()
        {
            // Arrange
            var settingsService = new Mock<IUserJourneyService>();
            settingsService.Setup(s => s.GetSettings(TestFundingStreamCode))
                .ReturnsAsync(new List<SettingValue>
                {
                });

            var fundingViewService = new Mock<IFundingViewService>();

            var documentStorageService = new Mock<IFundingDocumentStorageService>();

            documentStorageService
                .Setup(s => s.GetFilenames(TestFundingPeriodCode, TestFundingStreamCode))
                .Returns(new List<string> { TestFileName1, TestFileName2, TestFileName3 })
                .Verifiable();

            documentStorageService.Setup(s => s.GetMetadata(TestFileName1))
                .ReturnsAsync(new FundingDocumentMetaResponse
                {
                    DocumentPublishedDate = TestPublishedDate1,
                    CutOffDate = TestCutOffDate1
                })
                .Verifiable();

            documentStorageService.Setup(s => s.GetMetadata(TestFileName2))
                .ReturnsAsync(new FundingDocumentMetaResponse
                {
                    DocumentPublishedDate = TestPublishedDate1,
                    CutOffDate = TestCutOffDate2
                })
                .Verifiable();

            documentStorageService.Setup(s => s.GetMetadata(TestFileName3))
                .ReturnsAsync(new FundingDocumentMetaResponse
                {
                    DocumentPublishedDate = TestPublishedDate3,
                    CutOffDate = TestCutOffDate3
                })
                .Verifiable();

            var expectedDocuments = new List<FundingDocument>
            {
                new FundingDocument
                {
                    DocumentPublishedDate = TestPublishedDate1,
                    YearFrom = 2019,
                    YearTo = 2020,
                    CutOffDate = TestCutOffDate2,
                    IsFinal = false
                },
                new FundingDocument
                {
                    DocumentPublishedDate = TestPublishedDate3,
                    YearFrom = 2019,
                    YearTo = 2020,
                    CutOffDate = TestCutOffDate3,
                    IsFinal = true
                }
            };

            var documentService = new FundingDocumentService(documentStorageService.Object);

            // Act
            var documents = await documentService.GetFundingDocuments(TestFundingStreamCode, null, TestFundingPeriodCode);

            // Assert
            documentStorageService.Verify();
            documents.Should().BeEquivalentTo(expectedDocuments);
        }
    }
}