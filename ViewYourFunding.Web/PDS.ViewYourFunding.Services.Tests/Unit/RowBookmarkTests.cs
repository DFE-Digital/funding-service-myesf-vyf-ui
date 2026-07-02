using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    /// <summary>
    /// The RowBookmarksTests class.
    /// </summary>
    /// <seealso cref="RowBookmarks" />
    [TestClass]
    public class RowBookmarkTests : RowBookmarks
    {
        /// <summary>
        /// Gets the row with number honours number.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void GetRow_WithNumber_HonoursNumber()
        {
            // Arrange
            var expectedRowNumber = 3;

            // Act
            var row = GetRow(expectedRowNumber);

            // Assert
            row.RowNumber.Should().Be(expectedRowNumber);
        }

        /// <summary>
        /// Sets the row set number honours number.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void SetRow_SetNumber_HonoursNumber()
        {
            // Arrange
            var expectedRowNumber = 4;
            var cellReference = new CellReference($"C{expectedRowNumber}");

            // Act
            SetRow(expectedRowNumber, cellReference);

            // Assert
            cellReference.RowNumber.Should().Be(expectedRowNumber);
        }

        /// <summary>
        /// Saves the row to existing cell correctly increases reference.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Save_RowToExistingCell_CorrectlyIncreasesReference()
        {
            // Arrange
            var expectedColumnLetter = "B";
            var expectedRowNumber = 1;
            var cellReference = new CellReference($"{expectedColumnLetter}{expectedRowNumber}");

            // Act
            Save(cellReference);
            var cellReferenceFetched = GetRow(1);

            // Assert
            cellReferenceFetched.Letters().Should().Be(expectedColumnLetter);
        }
    }
}