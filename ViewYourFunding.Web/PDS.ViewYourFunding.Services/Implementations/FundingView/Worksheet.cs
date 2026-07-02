using Newtonsoft.Json.Linq;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using CellData = PDS.ViewYourFunding.Services.DTOs.CellData;
using Column = PDS.ViewYourFunding.Services.DTOs.Column;
using DropDown = PDS.ViewYourFunding.Services.DTOs.DropDown;
using FrozenData = PDS.ViewYourFunding.Services.DTOs.FrozenData;
using Height = PDS.ViewYourFunding.Services.DTOs.Height;
using MergeData = PDS.ViewYourFunding.Services.DTOs.MergeData;

namespace PDS.ViewYourFunding.Services.Implementations.FundingView
{
    /// <summary>
    /// Represents sheet data.
    /// </summary>
    public class Worksheet : BaseViewYourFundingRenderer, IWorksheet
    {
        /// <summary>
        /// The number parser.
        /// </summary>
        public static Regex NumberParser = new Regex(@"\d+", RegexOptions.Compiled);

        #region Private Properties

        /// <summary>
        /// Gets or sets the global setting service.
        /// </summary>
        private IGlobalSettingService GlobalSettingService { get; set; }

        /// <summary>
        /// Gets or sets the Component Configuration Service.
        /// </summary>
        private IComponentConfigurationService ComponentConfigurationService { get; set; }

        /// <summary>
        /// Gets or sets the UiModelDataset.
        /// </summary>
        private UiModelDataset UiModelDataset { get; set; }

        /// <summary>
        /// Gets or sets the worksheet datasets.
        /// </summary>
        private List<UiModelDataset> WorksheetDatasets { get; set; }

        /// <summary>
        /// Gets or sets the data necessary to be able to build this worksheet.
        /// </summary>
        private UiModelGroup WorksheetGroup { get; set; }

        /// <summary>
        /// Gets or sets a link to the worksheet's parent spreadsheet.
        /// </summary>
        private Spreadsheet ParentSpreadsheet { get; set; }

        /// <summary>
        /// Gets or sets the current row number we are onto for rendering.
        /// </summary>
        private int CurrentRowNumber { get; set; }

        /// <summary>
        /// Gets or sets bookmarks describing how far into rendering a row we've got.
        /// </summary>
        private RowBookmarks RowBookmarks { get; set; }

        /// <summary>
        /// Gets or sets the publication date to give on the spreadsheet.
        /// </summary>
        private DateTime PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the component configuration.
        /// </summary>
        private ComponentConfiguration ComponentConfiguration { get; set; }

        #endregion Private Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="Worksheet"/> class.
        /// Create an instance of the sheet data setting up defaults.
        /// </summary>
        /// <param name="parentSpreadsheet">A link to the parent spreadsheet object.</param>
        /// <param name="worksheetGroup">The group containing the worksheet data.</param>
        /// <param name="publicationDate">The publication date to give on the spreadsheet.</param>
        /// <param name="componentConfigurationService">The Component Configuration Service.</param>
        /// <param name="globalSettingService">The Global Setting Service.</param>
        public Worksheet(
            Spreadsheet parentSpreadsheet,
            UiModelGroup worksheetGroup,
            DateTime publicationDate,
            IComponentConfigurationService componentConfigurationService,
            IGlobalSettingService globalSettingService)
        {
            WorksheetGroup = worksheetGroup;
            ParentSpreadsheet = parentSpreadsheet;

            AutoFilter = worksheetGroup.AutoFilter;
            AutoFilterStartRow = worksheetGroup.AutoFilterStartRow;
            HeaderRow = worksheetGroup.HeaderRow;

            Title = PerformReplacements(
                WorksheetGroup.Title,
                new ComponentConfiguration
                {
                    FundingPeriodCode = ParentSpreadsheet.FundingPeriodCode,
                    PublishedDate = publicationDate
                });

            Cells = new Dictionary<string, CellData>();
            MergeInfo = new List<MergeData>();
            FrozenInfo = new List<FrozenData>();
            Columns = new List<Column>();
            Heights = new List<Height>();
            RowBookmarks = new RowBookmarks();
            PublicationDate = publicationDate;
            ComponentConfigurationService = componentConfigurationService;
            GlobalSettingService = globalSettingService;
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets merge info for the sheet.
        /// </summary>
        public List<MergeData> MergeInfo { get; set; }

        /// <summary>
        /// Gets or sets frozen info for the sheet.
        /// </summary>
        public List<FrozenData> FrozenInfo { get; set; }

        /// <summary>
        /// Gets or sets columns in the sheet.
        /// </summary>
        public List<Column> Columns { get; set; }

        /// <summary>
        /// Gets or sets height information for the sheet.
        /// </summary>
        public List<Height> Heights { get; set; }

        /// <summary>
        /// Gets or sets data for the sheet.
        /// </summary>
        public Dictionary<string, CellData> Cells { get; set; }

        /// <summary>
        /// Gets or sets title of the worksheet.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether should the worksheet have auto filters applied to it.
        /// </summary>
        public bool AutoFilter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the row to start auto filter.
        /// </summary>
        public int AutoFilterStartRow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the header row.
        /// </summary>
        public string HeaderRow { get; set; }

        #endregion Public Properties

        /// <summary>
        /// Draw the worksheet.
        /// </summary>
        /// <returns>True if there is any data to draw, or false if not.</returns>
        public bool Draw()
        {
            const int firstRowNumber = 1;
            CurrentRowNumber = firstRowNumber;

            WorksheetDatasets = GetAllDatasetDefinitions(new List<UiModelGroup> { WorksheetGroup });
            UiModelDataset = WorksheetDatasets?.FirstOrDefault();

            var firstDataSet = GetAllDatasetsData(
                ParentSpreadsheet.FundingData?.Funding,
                ParentSpreadsheet.ProviderFundingData?.ProviderFunding,
                WorksheetDatasets,
                new UIModelAdditionalFundingStream[0] { },
                ParentSpreadsheet.FundingStream.FundingStreamCode)?.FirstOrDefault();

            var firstDataSetItem = firstDataSet?.FirstOrDefault();
            var headersLevel1 = WorksheetGroup.Groups;

            var fundingProperties = ComponentConfigurationService.GetFundingProperties(firstDataSetItem, UiModelDataset);
            ComponentConfiguration = GetComponentConfiguration(fundingProperties, firstDataSetItem);

            if (WorksheetGroup.RenderGroupsAsRows)
            {
                HeadersReplacements(headersLevel1, firstRowNumber, firstDataSet);
            }
            else
            {
                DrawHeaders(headersLevel1, firstRowNumber, firstDataSet);
            }

            var hasData = DrawData(WorksheetGroup.RenderGroupsAsRows);
            DrawFooter();

            return hasData;
        }

        /// <summary>
        /// Draw the headers of the document.
        /// </summary>
        /// <param name="cellGroups">The groups of cells.</param>
        /// <param name="rowNumber">The row number to draw the headers at.</param>
        /// <param name="firstDataSet">The first dataset (used by some replacements).</param>
        private void DrawHeaders(List<UiModelGroup> cellGroups, int rowNumber, List<IFundingApiSearch> firstDataSet)
        {
            if (cellGroups == null)
            {
                return;
            }

            for (int idx = 0, len = cellGroups.Count; idx < len; idx++)
            {
                var uiModelGroup = cellGroups[idx];

                AddHeaderDataCell(uiModelGroup, rowNumber, firstDataSet);
                DrawHeaders(uiModelGroup.Groups, rowNumber + 1, firstDataSet);
            }
        }

        /// <summary>
        /// Draw the headers of the document.
        /// </summary>
        /// <param name="cellGroups">The groups of cells.</param>
        /// <param name="rowNumber">The row number to draw the headers at.</param>
        /// <param name="firstDataSet">The first dataset (used by some replacements).</param>
        private void HeadersReplacements(List<UiModelGroup> cellGroups, int rowNumber, List<IFundingApiSearch> firstDataSet)
        {
            if (cellGroups == null)
            {
                return;
            }

            for (int idx = 0, len = cellGroups.Count; idx < len; idx++)
            {
                var uiModelGroup = cellGroups[idx];

                PerformHeaderReplacements(uiModelGroup, rowNumber, firstDataSet);
                HeadersReplacements(uiModelGroup.Groups, rowNumber + 1, firstDataSet);
            }
        }

        /// <summary>
        /// Draw the footer of the sheet.
        /// </summary>
        private void DrawFooter()
        {
            if (WorksheetGroup.Footer?.Any() != true)
            {
                return;
            }

            Cells.Add($"A{CurrentRowNumber}", new CellData(string.Empty));

            for (int idx = 1, len = WorksheetGroup.Footer.Length; idx <= len; idx++)
            {
                var footerText = WorksheetGroup.Footer[idx - 1];
                var footerTextReplaced = PerformReplacements(
                    footerText,
                    new ComponentConfiguration
                    {
                        FundingPeriodCode = ParentSpreadsheet.FundingPeriodCode,
                        PublishedDate = PublicationDate
                    });

                Cells.Add($"A{CurrentRowNumber + idx}", new CellData(footerTextReplaced));
            }
        }

        /// <summary>
        /// Add data about a cell to the sheet data.
        /// </summary>
        /// <param name="uiModelGroup">The current group to process.</param>
        /// <param name="rowNumber">The row number to draw the headers at.</param>
        /// <param name="firstDataSet">The first dataset (used by some replacements).</param>
        private void PerformHeaderReplacements(UiModelGroup uiModelGroup, int rowNumber, List<IFundingApiSearch> firstDataSet)
        {
            var hasData = firstDataSet?.Any() == true;

            if (uiModelGroup.OnlyShowIfData && !hasData)
            {
                return;
            }

            var rowBookmark = RowBookmarks.GetRow(rowNumber);
            var lastRowNumber = rowBookmark.RowNumber + (firstDataSet != null ? firstDataSet.Count : 0);

            var text = PerformReplacements(
                uiModelGroup.Title,
                ComponentConfiguration,
                rowBookmark,
                rowBookmark.RowNumber + 1,
                lastRowNumber);

            // Excel doesn't seem to like to apply styling to totally empty fields
            if (string.IsNullOrEmpty(text))
            {
                text = " ";
            }

            uiModelGroup.Title = text;

            if (uiModelGroup.DropDown != null)
            {
                uiModelGroup.DropDown.InputRange = PerformReplacements(
                      uiModelGroup.DropDown.InputRange,
                      ComponentConfiguration,
                      rowBookmark,
                      rowBookmark.RowNumber + 1,
                      lastRowNumber);
            }

            var uiModelStyles = GetHeaderStyles(uiModelGroup, ParentSpreadsheet.UiModel.Classes);

            AddHeaderStyles(uiModelStyles, rowBookmark, rowNumber);

            var rowSpan = uiModelGroup.Rowspan ?? 1;
            var currentRowNumber = rowNumber + rowSpan - 1;

            ProcessDetailReplacements(
               uiModelGroup,
               currentRowNumber,
               firstDataSet);

            var colSpan = GetColSpan(uiModelGroup);

            if ((uiModelGroup.Colspan != null && uiModelGroup.Colspan > 0) || rowSpan > 1)
            {
                AddColRowSpanCellReferences(
                    uiModelGroup,
                    colSpan,
                    rowNumber);

                MergeInfo.Add(new MergeData()
                {
                    NumberCells = colSpan,
                    NumberRows = rowSpan,
                    StartCell = rowBookmark.GetIndex(),
                    StartRow = rowNumber - 1,
                    Name = uiModelGroup.Title?.ToString()
                });
            }
        }

        /// <summary>
        /// Process Details replacemenmts.
        /// </summary>
        /// <param name="uiModelGroup">A group.</param>
        /// <param name="currentRowNumber">Current row number.</param>
        /// <param name="firstDataSet">The first dataset (used by some replacements).</param>
        private void ProcessDetailReplacements(UiModelGroup uiModelGroup, int currentRowNumber, List<IFundingApiSearch> firstDataSet)
        {
            if (uiModelGroup.Detail?.Any() == true)
            {
                foreach (var detail in uiModelGroup.Detail)
                {
                    if (detail.Style?.RowsAbove != null && detail.Style.RowsAbove.Value > 0)
                    {
                        for (int idx = 0, len = detail.Style.RowsAbove.Value; idx < len; idx++)
                        {
                            PerformHeaderReplacements(new UiModelGroup(), ++currentRowNumber, firstDataSet);
                        }
                    }

                    PerformHeaderReplacements(detail, ++currentRowNumber, firstDataSet);
                }
            }
        }

        /// <summary>
        /// Add colspan rowspan cell refrences.
        /// </summary>
        /// <param name="uiModelGroup">A group.</param>
        /// <param name="colSpan">colspan.</param>
        /// <param name="rowNumber">The row number to draw the headers at.</param>
        private void AddColRowSpanCellReferences(UiModelGroup uiModelGroup, int colSpan, int rowNumber)
        {
            var rowSpan = uiModelGroup.Rowspan ?? 1;
            var rowSpanWithNewRows = uiModelGroup.RowSpanWithNewRows ?? true;
            if (rowSpan > 1 && rowSpanWithNewRows)
            {
                while (rowNumber + rowSpan > RowBookmarks.CellReferences.Count)
                {
                    var highestRowNumber = RowBookmarks.CellReferences.Count + 1;
                    var newCellReference = CellReference.A(highestRowNumber).Add(colSpan);

                    RowBookmarks.Save(newCellReference);
                }
            }
        }

        /// <summary>
        /// Add css columns .
        /// </summary>
        /// <param name="uiModelStyles">list of group styles to process.</param>
        /// <param name="rowBookmark">cell reference.</param>
        /// <param name="rowNumber">The row number to draw the headers at.</param>
        private void AddHeaderStyles(List<UiModelStyle> uiModelStyles, CellReference rowBookmark, int rowNumber)
        {
            foreach (var uiModelStyle in uiModelStyles)
            {
                if (uiModelStyle?.HeightInches.HasValue ?? false)
                {
                    Heights.Add(new Height
                    {
                        Row = rowNumber - 1,
                        HeightInches = uiModelStyle.HeightInches.Value
                    });
                }

                if (uiModelStyle?.WidthInches.HasValue ?? false)
                {
                    Columns.Add(new Column
                    {
                        Position = rowBookmark.GetIndex(),
                        WidthInches = uiModelStyle.WidthInches.Value
                    });
                }

                if (uiModelStyle?.Frozen == true)
                {
                    FrozenInfo.Add(new FrozenData
                    {
                        StartCell = rowBookmark.GetIndex(),
                        StartRow = rowNumber
                    });
                }
            }
        }


        /// <summary>
        /// Add data about a cell to the sheet data.
        /// </summary>
        /// <param name="uiModelGroup">The current group to process.</param>
        /// <param name="rowNumber">The row number to draw the headers at.</param>
        /// <param name="firstDataSet">The first dataset (used by some replacements).</param>
        private void AddHeaderDataCell(UiModelGroup uiModelGroup, int rowNumber, List<IFundingApiSearch> firstDataSet)
        {
            var hasData = firstDataSet?.Any() == true;

            if (uiModelGroup.OnlyShowIfData && !hasData)
            {
                return;
            }

            var rowBookmark = RowBookmarks.GetRow(rowNumber);

            var lastRowNumber = rowBookmark.RowNumber + (firstDataSet != null ? firstDataSet.Count : 0);

            var text = PerformReplacements(
                uiModelGroup.Title,
                ComponentConfiguration,
                rowBookmark,
                rowBookmark.RowNumber + 1,
                lastRowNumber);

            // Excel doesn't seem to like to apply styling to totally empty fields
            if (string.IsNullOrEmpty(text))
            {
                text = " ";
            }

            var cellData = new CellData(text)
            {
                Image = uiModelGroup.Image
            };

            if (uiModelGroup.DropDown != null)
            {
                cellData.DropDown = new DropDown
                {
                    InputRange = PerformReplacements(
                        uiModelGroup.DropDown.InputRange,
                        ComponentConfiguration,
                        rowBookmark,
                        rowBookmark.RowNumber + 1,
                        lastRowNumber),
                    Left = uiModelGroup.DropDown.LeftOffset,
                    Top = uiModelGroup.DropDown.TopOffset,
                    Width = uiModelGroup.DropDown.Width,
                    Height = uiModelGroup.DropDown.Height
                };
            }

            cellData.SuperScript = uiModelGroup.SuperScript;

            if (!string.IsNullOrEmpty(cellData.Image))
            {
                if (!ParentSpreadsheet.Images.ContainsKey(cellData.Image))
                {
                    var imageBytes = ParentSpreadsheet.ModelFileStoreService.ReadFileAsByteArray($"{FundingViewType.Spreadsheet}/{cellData.Image}");
                    ParentSpreadsheet.Images.Add(cellData.Image, imageBytes);
                }
            }

            var uiModelStyles = GetHeaderStyles(uiModelGroup, ParentSpreadsheet.UiModel.Classes);

            foreach (var uiModelStyle in uiModelStyles)
            {
                uiModelStyle.StyleCell(cellData);
            }

            AddHeaderStyles(uiModelStyles, rowBookmark, rowNumber);

            var rowSpan = uiModelGroup.Rowspan ?? 1;
            var currentRowNumber = rowNumber + rowSpan - 1;

            ProcessDetailsCells(uiModelGroup, currentRowNumber, firstDataSet);

            var colSpan = GetColSpan(uiModelGroup);
            var anyColOrRowSpan = colSpan > 1 || rowSpan > 1;

            if (anyColOrRowSpan)
            {
                AddColRowSpanCellReferences(
                   uiModelGroup,
                   colSpan,
                   rowNumber);

                MergeInfo.Add(new MergeData()
                {
                    NumberCells = colSpan,
                    NumberRows = rowSpan,
                    StartCell = rowBookmark.GetIndex(),
                    StartRow = rowNumber - 1,
                    Name = uiModelGroup.Title?.ToString()
                });
            }

            Cells.Add(rowBookmark.ToString(), cellData);
            RowBookmarks.SetRow(rowNumber, rowBookmark.Add(colSpan));
        }

        /// <summary>
        /// Process Details replacemenmts.
        /// </summary>
        /// <param name="uiModelGroup">A group.</param>
        /// <param name="currentRowNumber">Current row number.</param>
        /// <param name="firstDataSet">The first dataset (used by some replacements).</param>
        private void ProcessDetailsCells(UiModelGroup uiModelGroup, int currentRowNumber, List<IFundingApiSearch> firstDataSet)
        {
            if (uiModelGroup.Detail?.Any() == true)
            {
                foreach (var detail in uiModelGroup.Detail)
                {
                    if (detail.Style?.RowsAbove != null && detail.Style.RowsAbove.Value > 0)
                    {
                        for (int idx = 0, len = detail.Style.RowsAbove.Value; idx < len; idx++)
                        {
                            AddHeaderDataCell(new UiModelGroup(), ++currentRowNumber, firstDataSet);
                        }
                    }

                    AddHeaderDataCell(detail, ++currentRowNumber, firstDataSet);
                }
            }
        }

        /// <summary>
        /// Returns the row number the data section starts at.
        /// </summary>
        /// <returns>The row number the data section starts at.</returns>
        private int StartOfDataSectionRowNumber()
        {
            var highestNumber = 1;

            foreach (var cell in Cells)
            {
                // Get the row number from the cell reference (e.g. 9 from A9).
                var currentNumber = int.Parse(NumberParser.Match(cell.Key).Value, NumberFormatInfo.InvariantInfo);

                if (currentNumber > highestNumber)
                {
                    highestNumber = currentNumber;
                }
            }

            return highestNumber + 1;
        }

        /// <summary>
        /// Bring together all the data about a cell, inclidng its value and style etc....
        /// </summary>
        /// <param name="uiModelGroup">The current group to process.</param>
        /// <param name="dataset">A set of data (e.g. local authority rows).</param>
        /// <returns>A fully hydrated cell data object.</returns>
        private CellData CreateCellData(UiModelGroup uiModelGroup, UiModelDataset dataset)
        {
            var shouldIgnoreLine = ComponentConfiguration.PrimaryIdentifier == null
                && (ComponentConfiguration.ProviderName == null && ComponentConfiguration.LocalAuthorityName == null);

            if (shouldIgnoreLine)
            {
                var emptyCellData = new CellData(null);
                ApplyStylesToCell(uiModelGroup, dataset, emptyCellData, true);
                return emptyCellData;
            }

            var formulaMeta = GetFormulaMeta(uiModelGroup, dataset);
            var formula = formulaMeta.Item;
            var formulaIsOverride = formulaMeta.IsOverride;

            var selectorMeta = GetSelectorMeta(uiModelGroup, dataset);
            var selector = selectorMeta.Item;
            var selectorIsOverride = selectorMeta.IsOverride;

            var cellValue = (formula != null && (formulaIsOverride || !selectorIsOverride))
                ? GetCellValueForFormula(uiModelGroup, dataset, formula)
                : GetCellValueForSelector(uiModelGroup, dataset, selector);

            if (uiModelGroup.Datastyle?.EnumFormat == FieldViewDataEnumFormat.SparsityMethodology
                && int.TryParse(cellValue.ToString(), out var enumValue))
            {
                cellValue = enumValue.ToSparsityMethodologyDisplayValue();
            }
            else if (uiModelGroup.Datastyle?.NumberFormat == FieldViewDataNumberFormat.PercentageWith1DecimalPlace
                     && double.TryParse(cellValue.ToString(), out var percentageValue))
            {
                cellValue = percentageValue.ToPercentageWith1DecimalPlace();
            }
            else if (uiModelGroup.Datastyle?.NumberFormat == FieldViewDataNumberFormat.PercentageWith2DecimalPlaces
                     && double.TryParse(cellValue.ToString(), out var percentage2dpValue))
            {
                cellValue = percentage2dpValue.ToPercentageWith2DecimalPlaces();
            }

            if (uiModelGroup.Datastyle?.SpreadsheetNumberFormat == true
                     && double.TryParse(cellValue.ToString(), out var doubleToFormatValue))
            {
                if (uiModelGroup.Datastyle?.NumberFormat == FieldViewDataNumberFormat.ThousandsSeparatedNoDP)
                {
                    cellValue = doubleToFormatValue.ToThousandsSeparatedNoDP();
                }

                if (uiModelGroup.Datastyle?.NumberFormat == FieldViewDataNumberFormat.ThousandsSeperated2DP)
                {
                    cellValue = doubleToFormatValue.ToThousandsSeperated2DP();
                }

                if (uiModelGroup.Datastyle?.NumberFormat == FieldViewDataNumberFormat.OneDPWithoutTrailingZeroes)
                {
                    cellValue = doubleToFormatValue.To1DPWithoutTrailingZeroes();
                }

                if (uiModelGroup.Datastyle?.NumberFormat == FieldViewDataNumberFormat.TwoDPWithoutTrailingZeroes)
                {
                    cellValue = doubleToFormatValue.To2DPWithoutTrailingZeroes();
                }

                if (uiModelGroup.Datastyle?.NumberFormat == FieldViewDataNumberFormat.ThousandsSeparated5DP)
                {
                    cellValue = doubleToFormatValue.ToSpecifiedDecimalPlaces(5);
                }

                if (uiModelGroup.Datastyle?.NumberFormat == FieldViewDataNumberFormat.ZeroDecimalPlaces)
                {
                    cellValue = doubleToFormatValue.ZeroDecimalPlaces();
                }
            }

            UiModelGroup overrideUiModel = null;

            if (dataset.Id != null && uiModelGroup.Overrides?.TryGetValue(dataset.Id, out overrideUiModel) == true)
            {
                if (overrideUiModel.Datastyle?.DataFormatOverride == FieldViewDataNumberFormat.RateWith2DP
                         && double.TryParse(cellValue.ToString(), out var doubleValue))
                {
                    cellValue = Math.Round(doubleValue, 2);
                }

                if (overrideUiModel.Datastyle?.DataFormatOverride == FieldViewDataNumberFormat.PercentageRateWith2DP
                    && double.TryParse(cellValue.ToString(), out var percentageDoubleValue))
                {
                    cellValue = Math.Round(percentageDoubleValue, 4);
                }
            }

            var cellData = new CellData(cellValue);
            int[] backGroundColor = null;
            if (uiModelGroup.HideForNotApplicableValues)
            {
                if (!string.IsNullOrWhiteSpace(cellData.Value?.ToString()) && uiModelGroup.Datastyle != null && uiModelGroup.Datastyle.BackgroundColor.Any())
                {
                    backGroundColor = uiModelGroup.Datastyle.BackgroundColor;
                    uiModelGroup.Datastyle.BackgroundColor = null;
                }
            }

            ApplyStylesToCell(uiModelGroup, dataset, cellData);

            if (backGroundColor != null)
            {
                uiModelGroup.Datastyle.BackgroundColor = backGroundColor;
            }

            if (!ParentSpreadsheet.ShowData && cellData?.Value != null)
            {
                cellData.Value = string.Empty;
            }

            if (ParentSpreadsheet.ShowStatementSpecification && cellData?.Value != null)
            {
                if (!string.IsNullOrEmpty(uiModelGroup.Selector))
                {
                    var value = ComponentHelper.SimplifySelector(uiModelGroup.Selector, false);
                    cellData.Value += ComponentHelper.GetSelectorExplanation(
                        value,
                        new Component(new ComponentConfiguration { IsHtml = false }));
                }
            }

            return cellData;
        }

        /// <summary>
        /// Bring together all the data about a cell, including its value and style etc....
        /// </summary>
        /// <param name="uiModelGroup">The current group to process.</param>
        /// <param name="dataset">A set of data (e.g. local authority rows).</param>
        /// <returns>A fully hydrated cell data object.</returns>
        private CellData GetCellData(UiModelGroup uiModelGroup, UiModelDataset dataset)
        {
            CellData cellData;

            if (!string.IsNullOrWhiteSpace(uiModelGroup?.Expression))
            {
                var expression = GetCellValueForExpression(uiModelGroup?.Expression);
                cellData = new CellData(expression?.ToString());
            }
            else
            {
                cellData = new CellData(uiModelGroup?.Title?.ToString());
            }

            ApplyStylesToCell(uiModelGroup, dataset, cellData);
            return cellData;
        }

        private object GetCellValueForFormula(UiModelGroup uiModelGroup, UiModelDataset dataset, UiModelFormula formula)
        {
            // Process each selector to get the value
            var selectorResults = formula.Selectors?.Select(selector => GetCellValueForSelector(uiModelGroup, dataset, selector));

            // Inject the selector results into the expression
            var expression = string.Format(formula.Expression, selectorResults?.ToArray() ?? new object[0]);

            if (formula.DecimalPlaces.HasValue)
            {
                var multiplyDivideBy = Math.Pow(10, formula.DecimalPlaces.Value);
                expression = $"CONVERT(({expression}) * {multiplyDivideBy}, System.Int64) / {multiplyDivideBy}";
            }

            // Evaluate the expression and return the result
            using (var dataTable = new DataTable())
            {
                return dataTable.Compute(expression, string.Empty);
            }
        }

        private object GetCellValueForExpression(string expression)
        {
            var expressionValue = PerformReplacements(expression, ComponentConfiguration);
            return expressionValue?.ToString();
        }

        private object GetCellValueForSelector(UiModelGroup uiModelGroup, UiModelDataset dataset, string selector)
        {
            selector = PerformReplacements(selector, ComponentConfiguration, canShowSelector: false);
            var selectorResult = Select(ComponentConfiguration, selector);

            if (selectorResult == null)
            {
                return null;
            }

            if (ComponentHelper.IsNumber(selectorResult, false))
            {
                var selectorResultNumeric = selectorResult is double ? (double)selectorResult : Convert.ToDouble(selectorResult);

                if (uiModelGroup.ShowBlanksForNullOrZero && selectorResultNumeric == 0)
                {
                    return string.Empty;
                }

                selectorResultNumeric /= uiModelGroup.Scale ?? 1;

                if (uiModelGroup.Absolute)
                {
                    selectorResultNumeric = Math.Abs(selectorResultNumeric);
                }

                if (uiModelGroup.NotApplicableForValueBelow.HasValue)
                {
                    var valuesToCompare = string.IsNullOrWhiteSpace(uiModelGroup.DependentSelectors) ? new[] { selectorResultNumeric } : GetDependentsValues(uiModelGroup);

                    if (valuesToCompare.Any(value => uiModelGroup.NotApplicableForValueBelow.Value > value))
                    {
                        return "x";
                    }
                }

                if (uiModelGroup.NotApplicableForValueWithinRange != default)
                {
                    var valuesToCompare = string.IsNullOrWhiteSpace(uiModelGroup.DependentSelectors) ? new[] { selectorResultNumeric } : GetDependentsValues(uiModelGroup);
                    var evaluation = uiModelGroup.GetNotApplicableWithinRangeValueEvaluation(valuesToCompare);

                    if (!string.IsNullOrEmpty(evaluation))
                    {
                        return evaluation;
                    }
                }

                return selectorResultNumeric;
            }

            var selectorResultString = selectorResult.ToString();

            if (IsDataValueUppercase(uiModelGroup, dataset))
            {
                selectorResultString = selectorResultString.ToUpper();
            }

            return selectorResultString;
        }

        private IEnumerable<double> GetDependentsValues(UiModelGroup uiModelGroup)
        {
            var dependentSelectorsValues = uiModelGroup.DependentSelectors.Split("|").Select(selector => Select(ComponentConfiguration, selector));
            return dependentSelectorsValues.Select(value => value is double ? (double)value : Convert.ToDouble(value));
        }

        private void ApplyStylesToCell(UiModelGroup uiModelGroup, UiModelDataset dataset, CellData cellData, bool emptyCell = false)
        {
            var dataStyles = new List<UiModelStyle>();

            if (!emptyCell)
            {
                dataStyles = GetDataStyles(uiModelGroup.Id, uiModelGroup.ClassName, uiModelGroup, ParentSpreadsheet.UiModel.Classes, dataset);
            }

            foreach (var dataStyle in dataStyles)
            {
                dataStyle.StyleCell(cellData);
            }
        }

        /// <summary>
        /// Is the specified property a form of null in the json.
        /// </summary>
        /// <param name="propertyName">Name of the property to check.</param>
        /// <param name="dataJson">The json data.</param>
        /// <returns>True if null or JTokenType.Null, false if not.</returns>
        private bool PropertyIsNull(string propertyName, JObject dataJson)
        {
            return dataJson[propertyName] == null || dataJson[propertyName]?.Type == JTokenType.Null;
        }

        /// <summary>
        /// Draw the data section (in HTML if this was a table, this would be the tbody).
        /// </summary>
        /// <param name="renderGroupsAsRows">bool value to indicate whether Groups to be rendered as rows.</param>
        /// <returns>The row number after this data has been drawn.</returns>
        /// <returns>True if there is any data to draw, or false if not.</returns>
        private bool DrawData(bool renderGroupsAsRows)
        {
            if (WorksheetGroup.Dataset == null)
            {
                return false;
            }

            CurrentRowNumber = renderGroupsAsRows ? 1 : StartOfDataSectionRowNumber();

            var hasData = false;

            if (WorksheetGroup.Dataset.Count > 1 && renderGroupsAsRows)
            {
                hasData = DrawMultiDatasetRowGroups();
            }
            else
            {
                foreach (var dataset in WorksheetGroup.Dataset)
                {
                    var result = DrawDataset(dataset, renderGroupsAsRows);

                    if (result)
                    {
                        hasData = true;
                    }
                }
            }

            return hasData;
        }

        /// <summary>
        /// Draw an individual dataset (such as all the local authorities).
        /// </summary>
        /// <param name="dataset">A set of data (e.g. local authority rows).</param>
        /// <param name="renderGroupsAsRows">bool to indicate whether Groups to be rendered as rows.</param>
        /// <returns>True if there is any data to draw, or false if not.</returns>
        private bool DrawDataset(UiModelDataset dataset, bool renderGroupsAsRows)
        {
            var hasData = false;

            if (dataset.DatasetName?.Equals("providerfunding", StringComparison.InvariantCultureIgnoreCase) == true)
            {
                var allDatasetDefinitions = GetAllDatasetDefinitions(new List<UiModelGroup> { WorksheetGroup });
                UiModelDataset = allDatasetDefinitions?.FirstOrDefault();

                var iFundingApiSearchProviderFundingType = typeof(IFundingApiSearchProviderFunding);
                var relevantProviderFunding = GetProviderFundingDataForDataset(
                    ParentSpreadsheet.ProviderFundingData.ProviderFunding,
                    dataset,
                    new UIModelAdditionalFundingStream[0] { });

                if (relevantProviderFunding.Any())
                {
                    hasData = true;
                }

                if (renderGroupsAsRows)
                {
                    foreach (var row in relevantProviderFunding)
                    {
                        var fundingProperties = ComponentConfigurationService.GetFundingProperties(row, dataset);
                        ComponentConfiguration = GetComponentConfiguration(fundingProperties, row);

                        var currentCellReference = CellReference.A(CurrentRowNumber);
                        AddRowsToReport(row, WorksheetGroup.Groups, dataset, currentCellReference);
                    }
                }
                else
                {
                    foreach (var row in relevantProviderFunding)
                    {
                        var fundingProperties = ComponentConfigurationService.GetFundingProperties(row, dataset);
                        ComponentConfiguration = GetComponentConfiguration(fundingProperties, row);

                        var currentCellReference = CellReference.A(CurrentRowNumber);
                        var rowAdded = true;

                        foreach (var worksheetGroup in WorksheetGroup.Groups)
                        {
                            // Not null or empty - null equates to undefined here
                            if (HasFormulaOrSelector(worksheetGroup, dataset) || worksheetGroup.Groups != null)
                            {
                                if (ShowGroup(worksheetGroup))
                                {
                                    ProcessGroups(worksheetGroup, dataset, ref currentCellReference);
                                }
                                else
                                {
                                    rowAdded = false;
                                }
                            }
                            else
                            {
                                currentCellReference = currentCellReference.NextColumn();
                            }
                        }

                        if (rowAdded)
                        {
                            CurrentRowNumber = currentCellReference.NextRow().RowNumber;
                        }
                    }
                }

                return hasData;
            }

            var relevantFundings = GetFundingDataForDataset(ParentSpreadsheet.FundingData.Funding, dataset, new UIModelAdditionalFundingStream[0] { });

            if (relevantFundings.Any())
            {
                hasData = true;
            }

            if (renderGroupsAsRows)
            {
                foreach (var row in relevantFundings)
                {
                    var fundingProperties = ComponentConfigurationService.GetFundingProperties(row, dataset);
                    ComponentConfiguration = GetComponentConfiguration(fundingProperties, row);

                    var currentCellReference = CellReference.A(CurrentRowNumber);
                    AddRowsToReport(row, WorksheetGroup.Groups, dataset, currentCellReference);
                }
            }
            else
            {
                foreach (var row in relevantFundings)
                {
                    var fundingProperties = ComponentConfigurationService.GetFundingProperties(row, dataset);
                    ComponentConfiguration = GetComponentConfiguration(fundingProperties, row);

                    var currentCellReference = CellReference.A(CurrentRowNumber);

                    foreach (var worksheetGroup in WorksheetGroup.Groups)
                    {
                        // Not null or empty - null equates to undefined here
                        if (HasFormulaOrSelector(worksheetGroup, dataset) || worksheetGroup.Groups != null)
                        {
                            ProcessGroups(worksheetGroup, dataset, ref currentCellReference);
                        }
                        else
                        {
                            currentCellReference = currentCellReference.NextColumn();
                        }
                    }

                    CurrentRowNumber = currentCellReference.NextRow().RowNumber;
                }
            }

            return hasData;
        }

        private bool DrawMultiDatasetRowGroups()
        {
            var allDatasetsData = GetAllDatasetsData(
                ParentSpreadsheet.FundingData?.Funding,
                ParentSpreadsheet.ProviderFundingData?.ProviderFunding,
                WorksheetDatasets,
                ParentSpreadsheet.UiModel.AdditionalFundingStreams,
                ParentSpreadsheet.FundingStream.FundingStreamCode);

            foreach (var row in WorksheetGroup.Groups)
            {
                var groupsWithFormula = row.Groups.Where(group => group.Formula != null);
                if (groupsWithFormula.Any())
                {
                    foreach (var column in groupsWithFormula)
                    {
                        var contexts = column.Formula.Contexts;
                        var selectors = column.Formula.Selectors;
                        var formulaValues = new List<object>();

                        var index = 0;

                        foreach (var contextString in contexts)
                        {
                            var indexString = contextString.Replace("dataset", string.Empty);
                            var (currentDataset, targetDataset) = GetDatasets(allDatasetsData, indexString);
                            var currentSelector = selectors[index];
                            IFundingApiSearch dataItem;
                            if (targetDataset.Any())
                            {
                                var itemIndex = Convert.ToInt32(indexString.Substring(indexString.IndexOf('[')).Replace("[", string.Empty).Replace("]", string.Empty));

                                dataItem = targetDataset[itemIndex];
                            }
                            else
                            {
                                if (currentDataset.DatasetName.Equals("providerfunding", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    dataItem = new FundingApiSearchProviderFunding();
                                }
                                else
                                {
                                    dataItem = new FundingApiSearchFunding();
                                }
                            }

                            GetComponentConfiguration(currentDataset, dataItem);

                            var selector = PerformReplacements(currentSelector, ComponentConfiguration, canShowSelector: false);

                            var result = Select(ComponentConfiguration, selector);
                            formulaValues.Add(result);

                            index++;
                        }

                        var expression = string.Format(column.Formula.Expression, formulaValues?.ToArray() ?? new object[0]);

                        object formulaResult;

                        using (var dataTable = new DataTable())
                        {
                            formulaResult = dataTable.Compute(expression, string.Empty);
                        }

                        var cellValue = Convert.ToDouble(formulaResult).ToThousandsSeparatedNoDP();
                        column.Title = cellValue;
                        column.Formula = null;
                    }

                    AddRow(row);
                }
                else if (row.Context == null)
                {
                    AddRow(row);
                }
                else
                {
                    if (row.Context is string contextString && contextString.StartsWith("dataset", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var indexString = contextString.Replace("dataset", string.Empty);
                        if (indexString.IndexOf('[') > -1 && indexString.IndexOf(']') > -1)
                        {
                            var (currentDataset, targetDataset) = GetDatasets(allDatasetsData, indexString);
                            var itemIndex = Convert.ToInt32(indexString.Substring(indexString.IndexOf('[')).Replace("[", string.Empty).Replace("]", string.Empty));
                            IFundingApiSearch dataItem;
                            if (targetDataset.Any())
                            {
                                dataItem = targetDataset[itemIndex];
                            }
                            else
                            {
                                if (currentDataset.DatasetName.Equals("providerfunding", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    dataItem = new FundingApiSearchProviderFunding();
                                }
                                else
                                {
                                    dataItem = new FundingApiSearchFunding();
                                }
                            }

                            GetComponentConfiguration(currentDataset, dataItem);

                            AddRow(row);
                        }
                        else
                        {
                            var (currentDataset, targetDataset) = GetDatasets(allDatasetsData, indexString);
                            foreach (var dataItem in targetDataset)
                            {
                                GetComponentConfiguration(currentDataset, dataItem);

                                AddRow(row);
                            }
                        }
                    }
                }
            }

            return allDatasetsData.First().Any();
        }

        private void GetComponentConfiguration(UiModelDataset currentDataset, IFundingApiSearch dataItem)
        {
            var fundingProperties = ComponentConfigurationService.GetFundingProperties(dataItem, currentDataset);
            ComponentConfiguration = GetComponentConfiguration(fundingProperties, dataItem);
        }

        private (UiModelDataset currentDataset, List<IFundingApiSearch> targetDataset) GetDatasets(List<List<IFundingApiSearch>> allDatasetsData, string indexString)
        {
            var datasetOneBasedIndex = indexString.IndexOf('[') > -1 ? Convert.ToInt32(indexString.Substring(0, indexString.IndexOf('['))) : Convert.ToInt32(indexString.Substring(0));
            var currentDataset = WorksheetDatasets[datasetOneBasedIndex - 1];
            var targetDataset = allDatasetsData[datasetOneBasedIndex - 1];
            return (currentDataset, targetDataset);
        }

        private void AddRow(UiModelGroup row)
        {
            var currentCellReference = CellReference.A(CurrentRowNumber);
            AddRowsToReport(new FundingApiSearchFunding(), new List<UiModelGroup> { row }, WorksheetDatasets.First(), currentCellReference);
        }

        /// <summary>
        /// Renders groups as Rows.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="uiModelGroups">The list ofcurrent group to process.</param>
        /// <param name="dataset">A set of data (e.g. local authority rows).</param>
        /// <param name="currentCellReference">The currrent cell reference letter.</param>
        private void AddRowsToReport(
            IFundingApiSearch data,
            List<UiModelGroup> uiModelGroups,
            UiModelDataset dataset,
            CellReference currentCellReference)
        {
            if (uiModelGroups == null)
            {
                return;
            }

            foreach (var group in uiModelGroups)
            {
                var statingposition = CurrentRowNumber;
                var componentConfiguration = ComponentConfiguration;

                if (!string.IsNullOrWhiteSpace(group.VisibilityCondition))
                {
                    if (ShowGroup(group))
                    {
                        AddRowsToReport(data, group.Groups, dataset, currentCellReference);
                    }
                }
                else
                {
                    // Not null or empty - null equates to undefined here
                    if (group.Groups != null)
                    {
                        currentCellReference = CellReference.A(CurrentRowNumber);
                        ProcessGroupsAsRows(group, dataset, ref currentCellReference);

                        CurrentRowNumber += 1;
                    }
                }
            }
        }

        /// <summary>
        /// Process a group of cells, including processing individual cells.
        /// </summary>
        /// <param name="uiModelGroup">The current group to process.</param>
        /// <param name="dataset">A set of data (e.g. local authority rows).</param>
        /// <param name="currentCellReference">The currrent cell reference letter.</param>
        private void ProcessGroups(
            UiModelGroup uiModelGroup,
            UiModelDataset dataset,
            ref CellReference currentCellReference)
        {
            if (HasFormulaOrSelector(uiModelGroup, dataset))
            {
                var cellData = CreateCellData(uiModelGroup, dataset);

                Cells.Add($"{currentCellReference.ToString()}", cellData);
                currentCellReference = currentCellReference.NextColumn();
            }

            if (uiModelGroup.Groups != null)
            {
                foreach (var subgroup in uiModelGroup.Groups)
                {
                    ProcessGroups(subgroup, dataset, ref currentCellReference);
                }
            }
        }

        private bool ShowGroup(UiModelGroup group)
        {
            if (!string.IsNullOrWhiteSpace(group.VisibilityCondition))
            {
                var expressionResult = EvaluateWhereClause(
                       group.VisibilityCondition,
                       ComponentConfiguration,
                       ComponentConfiguration.Variables);

                if (!(expressionResult is bool evaluationResult))
                {
                    var values = EvaluateValues(new List<object> { expressionResult }, new Component(ComponentConfiguration));

                    if (!(values.FirstOrDefault() is bool valueResult))
                    {
                        throw new Exception("Conditional evaluation not completed.");
                    }
                    else
                    {
                        if (valueResult)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (evaluationResult)
                    {
                        return true;
                    }
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Process a group of cells, including processing individual cells.
        /// </summary>
        /// <param name="uiModelGroup">The current group to process.</param>
        /// <param name="dataset">A set of data (e.g. local authority rows).</param>
        /// <param name="currentCellReference">The currrent cell reference letter.</param>
        private void ProcessGroupsAsRows(
            UiModelGroup uiModelGroup,
            UiModelDataset dataset,
            ref CellReference currentCellReference)
        {
            if (uiModelGroup.Groups != null)
            {
                foreach (var subgroup in uiModelGroup.Groups)
                {
                    if (HasFormulaOrSelector(subgroup, dataset))
                    {
                        var cellData = CreateCellData(subgroup, dataset);

                        Cells.Add($"{currentCellReference.ToString()}", cellData);
                        currentCellReference = currentCellReference.NextColumn();
                    }
                    else
                    {
                        var cellData = GetCellData(subgroup, dataset);

                        Cells.Add($"{currentCellReference.ToString()}", cellData);
                        currentCellReference = currentCellReference.NextColumn();
                    }
                }
            }
        }

        /// <summary>
        /// Builds up the component configuration.
        /// </summary>
        /// <param name="fundingProperties">The funding properties.</param>
        /// <param name="data">The data to work against.</param>
        /// <returns>The Component Configuration.</returns>
        private ComponentConfiguration GetComponentConfiguration(FundingProperties fundingProperties, IFundingApiSearch data)
        {
            return ComponentConfigurationService.GetComponentConfiguration(
                fundingProperties,
                data,
                UiModelDataset,
                null,
                ParentSpreadsheet.UiModel,
                ParentSpreadsheet.FundingStream,
                null,
                ParentSpreadsheet.PublicationDate,
                null,
                ParentSpreadsheet.FundingPeriodCode,
                ParentSpreadsheet.AsOfMonth,
                ParentSpreadsheet.AsOfYear,
                null,
                null,
                null,
                VarianceSelectionOption.NoComparison,
                true,
                true,
                true,
                ParentSpreadsheet.ShowSelectors,
                false,
                ParentSpreadsheet.ShowStatementSpecification,
                ParentSpreadsheet.ShowData,
                false);
        }
    }
}