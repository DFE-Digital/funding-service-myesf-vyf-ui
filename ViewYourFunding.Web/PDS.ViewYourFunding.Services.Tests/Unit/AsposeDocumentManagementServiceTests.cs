using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    /// <summary>
    /// The AsposeDocumentManagementServiceTests class.
    /// </summary>
    /// <seealso cref="AsposeDocumentManagementService" />
    [TestClass]
    public class AsposeDocumentManagementServiceTests : AsposeDocumentManagementService
    {
        /// <summary>
        /// Creates the spreadsheet with data generates without error.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void CreateSpreadsheetWithData_Generates_WithoutError()
        {
            // Arrange
            var spreadsheetData = new Spreadsheet(
                new Mock<IGlobalSettingService>().Object,
                new Mock<IModelFileStoreService>().Object,
                new Mock<IComponentConfigurationService>().Object,
                new UiModel(),
                new Mock<IFundingApiSearchResponseFunding>().Object,
                new Mock<IFundingApiSearchResponseProviderFunding>().Object,
                new FundingStream { FundingStreamName = "Dedicated schools grant" },
                "AY-1920",
                2019,
                2020,
                DateTime.Now,
                string.Empty,
                string.Empty,
                false,
                false,
                true);

            // Act
            var spreadsheet = CreateSpreadsheetWithData(spreadsheetData, Services.Enums.FileFormat.ODS, true);

            // Assert
            spreadsheet.Should().NotBeEmpty();
        }
    }
}