using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    /// <summary>
    /// The SpreadsheetTests class.
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Builds the with2 workssheets returns spreadsheet data with2 spreadsheets.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Build_With2Workssheets_ReturnsSpreadsheetDataWith2Spreadsheets()
        {
            // Arrange
            var uiModel = new UiModel
            {
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup("1", null),
                    new UiModelGroup("2", null)
                }
            };

            var fundings = new FundingApiSearchResponse();

            var spreadsheet = new Spreadsheet(GetGlobalSettingService().Object, null, new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object), uiModel, fundings, null, new FundingStream { FundingStreamName = "PE and sport premium" }, "AY-1920", 2019, 2020, DateTime.Now, string.Empty, string.Empty, false, false, true);
            spreadsheet.Build();

            spreadsheet.Worksheets.Count.Should().Be(2);
        }

        /// <summary>
        /// Builds providerfunding spreasheet with groups as rows.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Build_ProviderFundingSpreadsheet_WithGroupsAsRows()
        {
            // Arrange
            var uiModel = new UiModel
            {
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup("1", null)
                    {
                        RenderGroupsAsRows = true,
                        Dataset = new List<UiModelDataset>
                        {
                            new UiModelDataset
                            {
                                DatasetName = "ProviderFunding"
                            }
                        },
                        Groups = new List<UiModelGroup>
                        {
                            new UiModelGroup("3", null)
                        }
                    },
                    new UiModelGroup("2", null)
                    {
                        RenderGroupsAsRows = true,
                        Dataset = new List<UiModelDataset>
                        {
                            new UiModelDataset
                            {
                                DatasetName = "ProviderFunding"
                            }
                        },
                        Groups = new List<UiModelGroup>
                        {
                            new UiModelGroup("4", null)
                        }
                    }
                },
            };

            var fundings = new FundingApiSearchResponse();
            var providerfunding = new ProviderFundingApiSearchResponse
            {
                ProviderFunding = new List<FundingApiSearchProviderFunding>
                {
                  new FundingApiSearchProviderFunding { FundingStreamCode = "GAG" }
                }
            };

            var componentConfigurationService = new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object);

            var spreadsheet = new Spreadsheet(GetGlobalSettingService().Object, null, componentConfigurationService, uiModel, fundings, providerfunding, new FundingStream { FundingStreamName = "PE and sport premium" }, "AY-1920", 2019, 2020, DateTime.Now, string.Empty, string.Empty, false, false, true);
            spreadsheet.Build();

            spreadsheet.Worksheets.Count.Should().Be(2);
        }

        /// <summary>
        /// Builds the with2 workssheets provider funding returns spreadsheet data with2 spreadsheets.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Build_With2WorkssheetsProviderFunding_ReturnsSpreadsheetDataWith2Spreadsheets()
        {
            // Arrange
            var uiModel = new UiModel
            {
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup("1", null),
                    new UiModelGroup("2", null)
                }
            };

            var providerfunding = new ProviderFundingApiSearchResponse();

            var spreadsheet = new Spreadsheet(GetGlobalSettingService().Object, null, new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object), uiModel, null, providerfunding, new FundingStream { FundingStreamName = "PE and sport premium" }, "AY-1920", 2019, 2020, DateTime.Now, string.Empty, string.Empty, false, false, true);
            spreadsheet.Build();

            spreadsheet.Worksheets.Count.Should().Be(2);
        }

        /// <summary>
        /// Builds the with image in a cell returns the image in the right cell.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Build_WithImageInACell_ReturnsTheImageInTheRightCell()
        {
            // Arrange
            var imagePath = "Images/barcode.png";

            var uiModel = new UiModel
            {
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup("1", null),
                    new UiModelGroup("2", null)
                    {
                        Groups = new List<UiModelGroup>
                        {
                            new UiModelGroup
                            {
                                Image = imagePath
                            }
                        }
                    }
                }
            };

            var fundings = new FundingApiSearchResponse();

            var worksheetTestsDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(WorksheetTests)).Location);
            var localModelFileStoreService = new LocalModelFileStoreService(worksheetTestsDirectory, @"Unit");

            var spreadsheet = new Spreadsheet(GetGlobalSettingService().Object, localModelFileStoreService, new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object), uiModel, fundings, null, new FundingStream { FundingStreamName = "PE and sport premium" }, "AY-1920", 2019, 2020, DateTime.Now, string.Empty, string.Empty, false, false, true);

            // Act
            spreadsheet.Build();

            // Assert
            spreadsheet.Images.Count.Should().Be(1);
            spreadsheet.Worksheets.Count.Should().Be(2);
            spreadsheet.Worksheets["2"].Cells["A1"].Image.Should().Be(imagePath);
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

        private Mock<IGlobalSettingService> GetGlobalSettingService()
        {
            var mockGlobalSettingService = new Mock<IGlobalSettingService>(MockBehavior.Strict);
            mockGlobalSettingService.Setup(s => s.GetFirstOrDefault(9)).ReturnsAsync(new GlobalSetting { Value = "False" });
            return mockGlobalSettingService;
        }
    }
}