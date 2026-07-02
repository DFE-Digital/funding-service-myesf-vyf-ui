using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    /// <summary>
    /// The UIModelStyleTests class.
    /// </summary>
    /// <seealso cref="UiModelStyle" />
    [TestClass]
    public class UIModelStyleTests : UiModelStyle
    {
        /// <summary>
        /// Styles the cell set property wrap property is set.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void StyleCell_SetProperty_WrapPropertyIsSet()
        {
            // Arrange
            var cellData = new CellData(1);
            var prevResult = cellData.Wrap;

            Wrap = true;

            // Act
            StyleCell(cellData);

            // Assert
            prevResult.Should().BeFalse();
            Wrap.Should().BeTrue();
        }

        /// <summary>
        /// Styles the cell set property colour property is set.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void StyleCell_SetProperty_ColourPropertyIsSet()
        {
            // Arrange
            var cellData = new CellData(1);

            BackgroundColor = new[] { 0, 1, 2, 3 };

            // Act
            StyleCell(cellData);

            // Assert
            cellData.Colour.Should().Be(System.Drawing.Color.FromArgb(0, 1, 2, 3));
        }

        /// <summary>
        /// Styles the cell set properies multiple properties are set.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void StyleCell_SetProperies_MultiplePropertiesAreSet()
        {
            // Arrange
            var cellData = new CellData(1);

            Bold = true;
            Wrap = true;

            // Act
            StyleCell(cellData);

            // Assert
            cellData.Bold.Should().BeTrue();
            cellData.Wrap.Should().BeTrue();
        }
    }
}