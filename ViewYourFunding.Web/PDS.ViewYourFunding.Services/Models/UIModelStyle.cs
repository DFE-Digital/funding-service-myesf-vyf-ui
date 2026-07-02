using PDS.ViewYourFunding.Services.Enums;
using System;
using System.Linq;
using Border = PDS.ViewYourFunding.Services.DTOs.Border;
using CellData = PDS.ViewYourFunding.Services.DTOs.CellData;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// The styles to apply to a group header/data. All are optional.
    /// </summary>
    public class UiModelStyle
    {
        /// <summary>
        /// Gets or sets the width in inches (optional).
        /// </summary>
        public double? WidthInches { get; set; }

        /// <summary>
        /// Gets or sets the height in inches (optional).
        /// </summary>
        public double? HeightInches { get; set; }

        /// <summary>
        /// Gets or sets whether the text should wrap (optional).
        /// </summary>
        public bool? Wrap { get; set; }

        /// <summary>
        /// Gets or sets whether the text should be bold (optional).
        /// </summary>
        public bool? Bold { get; set; }

        /// <summary>
        /// Gets or sets the font size, in pt.
        /// </summary>
        public double? FontSize { get; set; }

        /// <summary>
        /// Gets or sets whether the text should be underlined (optional).
        /// </summary>
        public bool? Underline { get; set; }

        /// <summary>
        /// Gets or sets whether the text should be uppercase (optional).
        /// </summary>
        public bool? Uppercase { get; set; }

        /// <summary>
        /// Gets or sets how to align the text (e.g. centre) (optional).
        /// </summary>
        public string Alignment { get; set; }

        /// <summary>
        /// Gets or sets the background colour, as a RGB array (optional).
        /// </summary>
        public int[] BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the boolean format for display (e.g. Yes, No) (optional).
        /// </summary>
        public string BooleanFormat { get; set; }

        /// <summary>
        /// Gets or sets the enum format for display (e.g. Sparsity Methodology from CFS data) (optional).
        /// </summary>
        public string EnumFormat { get; set; }

        /// <summary>
        /// Gets or sets the number format for display (e.g. ###,#00.00) (optional).
        /// </summary>
        public string NumberFormat { get; set; }

        /// <summary>
        /// Gets or sets the data format override for rendering (optional).
        /// </summary>
        /// <value>
        /// The data format override.
        /// </value>
        public string DataFormatOverride { get; set; }

        /// <summary>
        /// Gets or sets the borders to add to a group (optional).
        /// </summary>
        public UiModelBorder[] Borders { get; set; }

        /// <summary>
        /// Gets or sets how many rows to add above this data (optional).
        /// </summary>
        public int? RowsAbove { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is this row frozen?.
        /// </summary>
        public bool Frozen { get; set; }

        /// <summary>
        /// Gets or sets the data type (number or default) (optional).
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the specified number format for is to be used for spreadsheets.
        /// </summary>
        public bool SpreadsheetNumberFormat { get; set; }

        /// <summary>
        /// Style a cell using the style information passed.
        /// </summary>
        /// <param name="data">The cell data we already have.</param>
        public void StyleCell(CellData data)
        {
            if (BackgroundColor != null && BackgroundColor.Length >= 4)
            {
                data.Colour = System.Drawing.Color.FromArgb(
                    BackgroundColor[0],
                    BackgroundColor[1],
                    BackgroundColor[2],
                    BackgroundColor[3]);
            }

            if (NumberFormat != null)
            {
                data.NumberFormat = NumberFormat;
            }

            if (Bold != null)
            {
                data.Bold = Bold.Value;
            }

            if (FontSize != null)
            {
                data.FontSize = FontSize;
            }

            if (Underline != null)
            {
                data.Underline = Underline.Value;
            }

            if (Alignment != null)
            {
                data.Centered = Alignment == "centered";
            }

            if (Alignment != null)
            {
                data.LeftAligned = Alignment == "left";
            }

            if (Alignment != null)
            {
                data.RightAligned = Alignment == "right";
            }

            if (Wrap != null)
            {
                data.Wrap = Wrap.Value;
            }

            if (Borders != null)
            {
                data.Borders = Borders.Select(border => new Border()
                {
                    Edge = border.Edge
                }).ToList();
            }

            if (!string.IsNullOrEmpty(DataType))
            {
                data.DataType = (FieldDataType)Enum.Parse(typeof(FieldDataType), DataType);
            }
        }
    }
}