using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Services.Enums;
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
    /// The WorksheetTests class.
    /// </summary>
    [TestClass]
    public class WorksheetTests
    {
        /// <summary>
        /// Draws the multiple headers with distinct titles returns correct title in position.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Draw_MultipleHeadersWithDistinctTitles_ReturnsCorrectTitleInPosition()
        {
            // Arrange / Act
            var worksheet = DrawWorksheet_Fundings();

            // Assert
            worksheet.Cells.Count.Should().Be(5);
            worksheet.Cells["E1"].Value.Should().Be("TitleE");
        }

        /// <summary>
        /// Draws the multiple coluumns and multple rows returns correct number of cells and correct result in position.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Draw_MultipleColuumnsAndMultpleRows_ReturnsCorrectNumberOfCellsAndCorrectResultInPosition()
        {
            // Arrange
            var groupCode = "ABC1";

            var fundings = new FundingApiSearchResponse
            {
                Funding = new List<FundingApiSearchFunding>
                {
                    new FundingApiSearchFunding(),
                    new FundingApiSearchFunding(),
                    new FundingApiSearchFunding(),
                    new FundingApiSearchFunding(),
                    new FundingApiSearchFunding
                    {
                        GroupCode = groupCode,
                        GroupName = "SOMETHING",
                        Id = "NEEDS TO BE SET"
                    }
                }
            };

            // Act
            var worksheet = DrawWorksheet_Fundings(fundings);

            // Assert
            worksheet.Cells.Count.Should().Be(30);
            worksheet.Cells["E6"].Value.Should().Be(groupCode);
        }

        /// <summary>
        /// Draws the multiple coluumns and multple rows provider funding returns correct number of cells and correct result in position.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Draw_MultipleColuumnsAndMultpleRowsProviderFunding_ReturnsCorrectNumberOfCellsAndCorrectResultInPosition()
        {
            // Arrange
            var providerType = "ABC1";

            var providerFunding = new ProviderFundingApiSearchResponse
            {
                ProviderFunding = new List<FundingApiSearchProviderFunding>
                {
                    new FundingApiSearchProviderFunding(),
                    new FundingApiSearchProviderFunding(),
                    new FundingApiSearchProviderFunding(),
                    new FundingApiSearchProviderFunding(),
                    new FundingApiSearchProviderFunding
                    {
                        ProviderType = providerType,
                        Id = "NEEDS TO BE SET",
                        OrganisationName = "NEEDS TO BE SET"
                    }
                }
            };

            // Act
            var worksheet = DrawWorksheet_ProviderFunding(providerFunding);

            // Assert
            worksheet.Cells.Count.Should().Be(30);
            worksheet.Cells["E6"].Value.Should().Be(providerType);
        }


        [TestMethod, TestCategory("Unit")]
        [DataRow(0, "Lump sum")]
        [DataRow(1, "Taper")]
        [DataRow(2, "National funding formula")]
        public void Draw_ReturnsSparsityData(int dataValue, string methodology)
        {
            // Arrange
            var sparsityMethodology = methodology;

            var providerFunding = new ProviderFundingApiSearchResponse
            {
                ProviderFunding = new List<FundingApiSearchProviderFunding>
                {
                    new FundingApiSearchProviderFunding
                    {
                        Id = "NEEDS TO BE SET",
                        OrganisationName = "NEEDS TO BE SET",
                        FundingValue = "{\"fundingTemplate\": {\"fundingLines\": [{\"templateLineId\": 20,\"value\": dataValue}]}}".Replace("dataValue", dataValue.ToString())
                    }
                }
            };

            var uiModel = new UiModel();

            var worksheetTestsDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(WorksheetTests)).Location);
            var localModelFileStoreService = new LocalModelFileStoreService(worksheetTestsDirectory);
            var componentConfigurationService = new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object);

            var spreadsheet = new Spreadsheet(GetGlobalSettingService().Object, localModelFileStoreService, componentConfigurationService, uiModel, null, providerFunding, new FundingStream { FundingStreamName = "Dedicated schools grant" }, "AY-1920", 2019, 2020, DateTime.Now, string.Empty, string.Empty, false, false, true);
            var worksheetGroup = new UiModelGroup
            {
                Title = "THIS IS THE TITLE",
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup("TitleA", "$..fundingLines[?(@.templateLineId == 20)].value")
                    {
                        Datastyle = new UiModelStyle
                        {
                            EnumFormat = FieldViewDataEnumFormat.SparsityMethodology
                        }
                    }
                },
                Dataset = new List<UiModelDataset>
                {
                    new UiModelDataset
                    {
                        DatasetName = "ProviderFunding"
                    }
                }
            };

            var worksheet = new Worksheet(spreadsheet, worksheetGroup, DateTime.Now, componentConfigurationService, GetGlobalSettingService().Object);
            worksheet.Draw();

            // Assert
            worksheet.Cells.Count.Should().Be(2);
            worksheet.Cells["A2"].Value.Should().Be(sparsityMethodology);
        }

        /// <summary>
        /// Draws the multiple coluumns and multple rows and foter returns footer in correct place.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Draw_MultipleColuumnsAndMultpleRowsAndFoter_ReturnsFooterInCorrectPlace()
        {
            // Arrange
            var footerText = "THIS IS THE FOOTER TEXT";

            var fundings = new FundingApiSearchResponse
            {
                Funding = new List<FundingApiSearchFunding>
                {
                    new FundingApiSearchFunding(),
                    new FundingApiSearchFunding()
                }
            };

            // Act
            var worksheet = DrawWorksheet_Fundings(fundings, footerText);

            // Assert
            worksheet.Cells.Count.Should().Be(17);
            worksheet.Cells["A5"].Value.Should().Be(footerText);
        }

        /// <summary>
        /// Draws the columns with width set returns column with width set in correct position.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Draw_ColumnsWithWidthSet_ReturnsColumnWithWidthSetInCorrectPosition()
        {
            // Arrange / Act
            var worksheet = DrawWorksheet_Fundings();

            // Assert
            worksheet.Columns.Count.Should().Be(1);
            worksheet.Columns[0].WidthInches.Should().Be(2.2);
        }

        /// <summary>
        /// Draws the rows with heigh set returns height on corect row.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Draw_RowsWithHeighSet_ReturnsHeightOnCorectRow()
        {
            // Arrange / Act
            var worksheet = DrawWorksheet_Fundings();

            // Assert
            worksheet.Heights.Count.Should().Be(1);
            worksheet.Heights[0].HeightInches.Should().Be(3.3);
        }

        /// <summary>
        /// Draws some cells merged correct cells merged.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Draw_SomeCellsMerged_CorrectCellsMerged()
        {
            // Arrange / Act
            var worksheet = DrawWorksheet_Fundings();

            // Assert
            worksheet.MergeInfo.Count.Should().Be(1);
            worksheet.MergeInfo[0].NumberRows.Should().Be(3);
        }

        /// <summary>
        /// Draws the title set to something returns spreadsheet with title set.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Draw_TitleSetToSomething_ReturnsSpreadsheetWithTitleSet()
        {
            // Arrange / Act
            var worksheet = DrawWorksheet_Fundings();

            // Assert
            worksheet.Title.Should().Be("THIS IS THE TITLE");
        }

        #region Private methods

        private Worksheet DrawWorksheet_Fundings(FundingApiSearchResponse fundings = null, string footerText = null)
        {
            if (fundings == null)
            {
                fundings = new FundingApiSearchResponse
                {
                    Funding = new List<FundingApiSearchFunding>()
                };
            }

            var uiModel = new UiModel();

            var worksheetTestsDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(WorksheetTests)).Location);
            var localModelFileStoreService = new LocalModelFileStoreService(worksheetTestsDirectory);
            var componentConfigurationService = new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object);

            var spreadsheet = new Spreadsheet(GetGlobalSettingService().Object, localModelFileStoreService, componentConfigurationService, uiModel, fundings, null, new FundingStream { FundingStreamName = "Dedicated schools grant" }, "AY-1920", 2019, 2020, DateTime.Now, string.Empty, string.Empty, false, false, true);
            var worksheetGroup = new UiModelGroup
            {
                Title = "THIS IS THE TITLE",
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup("TitleA", "$..groupCode")
                    {
                        Style = new UiModelStyle
                        {
                            WidthInches = 2.2,
                            HeightInches = 3.3
                        },
                        Rowspan = 3
                    },
                    new UiModelGroup("TitleB", "$..groupCode"),
                    new UiModelGroup("TitleC", "$..groupCode"),
                    new UiModelGroup("TitleD", "$..groupCode"),
                    new UiModelGroup("TitleE", "$..groupCode")
                },
                Dataset = new List<UiModelDataset>
                {
                    new UiModelDataset()
                }
            };

            if (!string.IsNullOrEmpty(footerText))
            {
                worksheetGroup.Footer = new List<string>
                {
                    footerText
                }.ToArray();
            }

            var worksheet = new Worksheet(spreadsheet, worksheetGroup, DateTime.Now, componentConfigurationService, GetGlobalSettingService().Object);
            worksheet.Draw();

            return worksheet;
        }

        private Worksheet DrawWorksheet_ProviderFunding(ProviderFundingApiSearchResponse providerFunding = null, string footerText = null)
        {
            if (providerFunding == null)
            {
                providerFunding = new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = new List<FundingApiSearchProviderFunding>()
                };
            }

            var uiModel = new UiModel();

            var worksheetTestsDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(WorksheetTests)).Location);
            var localModelFileStoreService = new LocalModelFileStoreService(worksheetTestsDirectory);
            var componentConfigurationService = new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object);

            var spreadsheet = new Spreadsheet(GetGlobalSettingService().Object, localModelFileStoreService, componentConfigurationService, uiModel, null, providerFunding, new FundingStream { FundingStreamName = "Dedicated schools grant" }, "AY-1920", 2019, 2020, DateTime.Now, string.Empty, string.Empty, false, false, true);
            var worksheetGroup = new UiModelGroup
            {
                Title = "THIS IS THE TITLE",
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup("TitleA", "$..providerType")
                    {
                        Style = new UiModelStyle
                        {
                            WidthInches = 2.2,
                            HeightInches = 3.3
                        },
                        Rowspan = 3
                    },
                    new UiModelGroup("TitleB", "$..providerType"),
                    new UiModelGroup("TitleC", "$..providerType"),
                    new UiModelGroup("TitleD", "$..providerType"),
                    new UiModelGroup("TitleE", "$..providerType")
                },
                Dataset = new List<UiModelDataset>
                {
                    new UiModelDataset
                    {
                        DatasetName = "ProviderFunding"
                    }
                }
            };

            if (!string.IsNullOrEmpty(footerText))
            {
                worksheetGroup.Footer = new List<string>
                {
                    footerText
                }.ToArray();
            }

            var worksheet = new Worksheet(spreadsheet, worksheetGroup, DateTime.Now, componentConfigurationService, GetGlobalSettingService().Object);
            worksheet.Draw();

            return worksheet;
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

        #endregion Private methods
    }
}