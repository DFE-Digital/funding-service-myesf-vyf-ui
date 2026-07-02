using PDS.ViewYourFunding.Services.Enums;
using System.Collections.Generic;
using System.Drawing;

namespace PDS.ViewYourFunding.Services.DTOs
{
    /// <summary>
    /// Represents all the data about a cell.
    /// </summary>
    public class CellData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CellData"/> class.
        /// </summary>
        /// <param name="value">The value to show in the cell.</param>
        public CellData(object value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the color of the column (optional).
        /// </summary>
        public Color? Colour { get; set; }

        /// <summary>
        /// Gets or sets an image to put in the cell (optional).
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the value to put in the cell.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cell text should be bold or not.
        /// </summary>
        public bool Bold { get; set; }

        /// <summary>
        /// Gets or sets the font size, in pt.
        /// </summary>
        public double? FontSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the cell text should be underlined or not.
        /// </summary>
        public bool Underline { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the cell text should be wrapped or not.
        /// </summary>
        public bool Wrap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the cell text should be centered or not.
        /// </summary>
        public bool Centered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the cell text should be left aligned or not.
        /// </summary>
        public bool LeftAligned { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether the cell text should be right aligned or not.
        /// </summary>
        public bool RightAligned { get; set; }

        /// <summary>
        /// Gets or sets the data type (Number or Default) (optional).
        /// </summary>
        public FieldDataType DataType { get; set; }

        /// <summary>
        /// Gets or sets the number format for the column (e.g. '#,##0.000').
        /// </summary>
        public string NumberFormat { get; set; }

        /// <summary>
        /// Gets or sets border information for the cell.
        /// </summary>
        public List<Border> Borders { get; set; }

        /// <summary>
        /// Gets or sets specify that a dropdown should show.
        /// </summary>
        public DropDown DropDown { get; set; }

        /// <summary>
        /// Gets or sets a value for the Superscript.
        /// </summary>
        public string SuperScript { get; set; }
    }
}