using Aspose.Cells;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using System;
using System.IO;

namespace PDS.ViewYourFunding.Services.Implementations.FundingView
{
    /// <summary>
    /// Aspose implementation of the IDocumentManagementService.
    /// </summary>
    public class AsposeDocumentManagementService : IDocumentManagementService
    {
        /// <summary>
        /// Gets or sets the logging service.
        /// </summary>
        protected ILoggerAdapter<AsposeDocumentManagementService> LoggerService { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsposeDocumentManagementService"/> class.
        /// Construct an instance of the Aspose spreadsheet generator.
        /// </summary>
        /// <param name="loggerService">The service to use for logging.</param>
        public AsposeDocumentManagementService(ILoggerAdapter<AsposeDocumentManagementService> loggerService = null)
        {
            LoggerService = loggerService;
        }

        #region IDocumentManagementService Implementation

        /// <summary>
        /// Create a spreadsheet from the data supplied.
        /// </summary>
        /// <param name="spreadsheet">The data to generate the spreadsheet with. Each dictionary entry represents one worksheet + An images collection - a
        /// dictionary where the key is the identifier for the image.</param>
        /// <param name="fileFormat">File format e.g. csv, ods. by default use a popular vendor specific version (e.g. XLS 2003).</param>
        /// <param name="removeFormulas">Whether to remove all formulas and replace them with their evaluated values.</param>
        /// <returns>The spreadsheet as a byte array.</returns>
        public byte[] CreateSpreadsheetWithData(ISpreadsheet spreadsheet, FileFormat fileFormat, bool removeFormulas = false)
        {
            var workbookFormat = GetFileFormat(fileFormat);
            var workbook = new Workbook(workbookFormat);

            var firstWorksheet = true;
            var palletteCount = 0;

            foreach (var sheetName in spreadsheet.Worksheets.Keys)
            {
                var data = spreadsheet.Worksheets[sheetName];
                var sheet = firstWorksheet ? workbook.Worksheets[0] : workbook.Worksheets.Add(sheetName);

                if (firstWorksheet)
                {
                    sheet.Name = sheetName;
                }

                firstWorksheet = false;

                foreach (var field in data.Cells)
                {
                    var cell = sheet.Cells[field.Key];
                    var cellData = field.Value;

                    if (cellData.DropDown != null)
                    {
                        var comboBox = sheet.Shapes.AddComboBox(
                            cell.Row,
                            cellData.DropDown.Top ?? 0,
                            cell.Column,
                            cellData.DropDown.Left ?? 0,
                            cellData.DropDown.Width ?? 15,
                            cellData.DropDown.Height ?? 15);

                        comboBox.LinkedCell = field.Key;
                        comboBox.InputRange = cellData.DropDown.InputRange;
                    }

                    if (!string.IsNullOrEmpty(cellData.Image))
                    {
                        if (spreadsheet.Images == null)
                        {
                            throw new Exception("Images collection not passed");
                        }

                        var image = spreadsheet.Images[cellData.Image];
                        sheet.Pictures.Add(cell.Row, cell.Column, GetMemoryStream(image));
                    }

                    var fieldValue = cellData.Value;

                    if (fieldValue != null && cellData.DataType == FieldDataType.Number
                        && double.TryParse(fieldValue.ToString(), out var valueAsDouble))
                    {
                        cell.PutValue(valueAsDouble);
                    }
                    else if (fieldValue?.ToString().StartsWith("=") == true)
                    {
                        var fieldValueString = fieldValue.ToString();
                        cell.Formula = fieldValueString;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(cellData.SuperScript))
                        {
                            var text = fieldValue.ToString() + cellData.SuperScript;
                            cell.PutValue(text);
                            cell.Characters(text.Length - cellData.SuperScript.Length, text.Length).Font.IsSuperscript = true;
                        }
                        else
                        {
                            cell.PutValue(fieldValue ?? string.Empty);
                        }
                    }

                    var style = cell.GetStyle();

                    if (!string.IsNullOrWhiteSpace(cellData.NumberFormat))
                    {
                        style.SetCustom(cellData.NumberFormat, false);
                    }

                    if (cellData.Colour.HasValue)
                    {
                        style.Pattern = BackgroundType.Solid;

                        if (!workbook.IsColorInPalette(cellData.Colour.Value))
                        {
                            workbook.ChangePalette(cellData.Colour.Value, palletteCount++);
                        }

                        style.ForegroundColor = cellData.Colour.Value;
                    }

                    if (cellData.Centered)
                    {
                        style.HorizontalAlignment = TextAlignmentType.Center;
                        style.VerticalAlignment = TextAlignmentType.Center;
                        style.IsTextWrapped = true;
                    }

                    if (cellData.LeftAligned)
                    {
                        style.HorizontalAlignment = TextAlignmentType.Left;
                        style.VerticalAlignment = TextAlignmentType.Center;
                    }

                    if (cellData.RightAligned)
                    {
                        style.HorizontalAlignment = TextAlignmentType.Right;
                        style.VerticalAlignment = TextAlignmentType.Center;
                    }

                    if (cellData.Wrap)
                    {
                        style.IsTextWrapped = true;
                    }

                    if (cellData.Borders != null)
                    {
                        foreach (var border in cellData.Borders)
                        {
                            style.SetBorder(
                                (BorderType)Enum.Parse(typeof(BorderType), border.Edge),
                                CellBorderType.Thin,
                                System.Drawing.Color.Black);
                        }
                    }

                    style.Font.IsBold = cellData.Bold;
                    style.Font.Underline = cellData.Underline ? FontUnderlineType.Single : FontUnderlineType.None;

                    if (cellData.FontSize.HasValue)
                    {
                        style.Font.DoubleSize = cellData.FontSize.Value;
                    }

                    cell.SetStyle(style);
                }

                if (data.MergeInfo != null)
                {
                    foreach (var mergeDataInstance in data.MergeInfo)
                    {
                        sheet.Cells.Merge(
                            mergeDataInstance.StartRow,
                            mergeDataInstance.StartCell,
                            mergeDataInstance.NumberRows,
                            mergeDataInstance.NumberCells);
                    }
                }

                if (data.FrozenInfo != null)
                {
                    foreach (var frozenDataInstance in data.FrozenInfo)
                    {
                        sheet.FreezePanes(frozenDataInstance.StartRow, frozenDataInstance.StartCell, 1, 0);
                    }
                }

                if (data.Columns != null)
                {
                    foreach (var column in data.Columns)
                    {
                        if (column.WidthInches != null)
                        {
                            sheet.Cells.SetColumnWidthInch(column.Position, column.WidthInches.Value);
                        }

                        if (column.NumberFormat != null)
                        {
                            var columnStyle = workbook.CreateStyle();
                            var flag = new StyleFlag { NumberFormat = true };
                            columnStyle.Custom = column.NumberFormat; // "0.000";

                            sheet.Cells.Columns[column.Position].ApplyStyle(columnStyle, flag);
                        }
                    }
                }

                if (data.Heights != null)
                {
                    foreach (var height in data.Heights)
                    {
                        sheet.Cells.SetRowHeightInch(height.Row, height.HeightInches);
                    }
                }

                if (data.AutoFilter)
                {
                    var startRow = data.AutoFilterStartRow == 0 ? 1 : data.AutoFilterStartRow;
                    sheet.AutoFilter.Range = $"A{startRow}:{sheet.Cells.LastCell.Name}";
                }

                if (!string.IsNullOrWhiteSpace(data.HeaderRow))
                {
                    var rows = data.HeaderRow.Split(',');
                    for (var i = 0; i < rows.Length; i++)
                    {
                        sheet.Cells.InsertRow(i);
                        sheet.Cells[i, 0].Value = rows[i];
                        var style = sheet.Cells[i, 0].GetStyle();
                        style.Font.IsBold = true;
                        sheet.Cells[i, 0].SetStyle(style);
                    }
                }
            }

            if (removeFormulas)
            {
                RemoveFormulas(workbook);
            }

            var outputStream = new MemoryStream();
            workbook.Save(outputStream, SaveFileFormat(fileFormat));

            return outputStream.ToArray();
        }

        #endregion


        #region licensing

        /// <summary>
        /// Enables the license if it can be found.
        /// </summary>
        /// <returns><c>true</c> if the license was found; otherwise, <c>false</c>.</returns>
        public bool EnableCellsLicense()
        {
            LoggerService?.LogInformation("Enabling Aspose Cells license");

            try
            {
                using (var stream = typeof(AsposeDocumentManagementService).Assembly
                    .GetManifestResourceStream("PDS.ViewYourFunding.Services.Resources.Aspose.Total.lic"))
                {
                    if (stream != null)
                    {
                        var cellsLicense = new Aspose.Cells.License();
                        cellsLicense.SetLicense(stream);

                        LoggerService?.LogInformation("Aspose Cells license was applied");
                    }
                }
            }
            catch (Exception exception)
            {
                LoggerService?.LogError(exception, "Cannot apply Aspose Cells license");
                return false;
            }

            return true;
        }


        #endregion


        #region Private Helpers

        /// <summary>
        /// Get memory stream from a byte array.
        /// </summary>
        /// <param name="data">Byte array containing file data.</param>
        /// <returns>A memory stream.</returns>
        private MemoryStream GetMemoryStream(byte[] data)
        {
            var memoryStream = new MemoryStream();
            memoryStream.Write(data, 0, data.Length);

            return memoryStream;
        }

        /// <summary>
        /// Remove all formulas from the workbook's cells and replace with the result of the calculation.
        /// </summary>
        /// <param name="workbook">The workbook.</param>
        private void RemoveFormulas(Workbook workbook)
        {
            workbook.Settings.CreateCalcChain = false;
            workbook.CalculateFormula();

            foreach (var worksheet in workbook.Worksheets)
            {
                var cells = worksheet.Cells;
                cells.RemoveFormulas();
            }
        }

        /// <summary>
        /// Returns file format.
        /// </summary>
        /// <param name="fileFormat">The fileformat.</param>
        private FileFormatType GetFileFormat(FileFormat fileFormat)
        {
            switch (fileFormat)
            {
                case FileFormat.ODS:
                    return FileFormatType.ODS;
                case FileFormat.CSV:
                    return FileFormatType.CSV;
                default:
                    return FileFormatType.Excel97To2003;
            }
        }

        /// <summary>
        /// Returns the file format to save the workbook.
        /// </summary>
        /// <param name="fileFormat">The file format.</param>
        private SaveFormat SaveFileFormat(FileFormat fileFormat)
        {
            switch (fileFormat)
            {
                case FileFormat.ODS:
                    return SaveFormat.ODS;
                case FileFormat.CSV:
                    return SaveFormat.CSV;
                default:
                    return SaveFormat.Excel97To2003;
            }
        }

        #endregion
    }
}