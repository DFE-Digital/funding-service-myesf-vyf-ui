using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Implementations.FundingView;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    /// <summary>
    /// The CellReferenceTests class.
    /// </summary>
    /// <seealso cref="CellReference" />
    [TestClass]
    public class CellReferenceTests : CellReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CellReferenceTests"/> class.
        /// </summary>
        public CellReferenceTests() : base("A1")
        {
        }

        /// <summary>
        /// as the get new a instance letter should be a.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void A_GetNewAInstance_LetterShouldBeA()
        {
            // Act
            var newCellReference = A();

            // Assert
            newCellReference.Letters().Should().Be("A");
        }

        /// <summary>
        /// as the get new a instance letter should be a and number should be set.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void A_GetNewAInstance_LetterShouldBeAAndNumberShouldBeSet()
        {
            // Arrange
            var rowNumber = 7;

            // Act
            var newCellReference = A(rowNumber);

            // Assert
            newCellReference.Letters().Should().Be("A");
            newCellReference.RowNumber.Should().Be(rowNumber);
        }

        /// <summary>
        /// Gets the index cell c1 index position should be2.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void GetIndex_CellC1_IndexPositionShouldBe2()
        {
            // Arrange
            var cellRef = new CellReference("C1");

            // Act
            var index = cellRef.GetIndex();

            // Assert
            index.Should().Be(2);
        }

        /// <summary>
        /// Converts to string_cellreferencepassed_shouldberetained.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void ToString_CellReferencePassed_ShouldBeRetained()
        {
            // Arrange
            var cellRef = new CellReference("B2");

            // Act
            var cellRefString = cellRef.ToString();

            // Assert
            cellRefString.Should().Be("B2");
        }

        /// <summary>
        /// Letterses the cell referenece passed letter should be recreated.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Letters_CellReferenecePassed_LetterShouldBeRecreated()
        {
            // Arrange
            var cellRef = new CellReference("B2");

            // Act
            var cellRefString = cellRef.Letters();

            // Assert
            cellRefString.Should().Be("B");
        }

        /// <summary>
        /// Nexts the column ask for next colunn should return same row in next column.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void NextColumn_AskForNextColunn_ShouldReturnSameRowInNextColumn()
        {
            // Arrange
            var cellRef1 = new CellReference("B2");

            // Act
            var cellRef2 = cellRef1.NextColumn();

            // Assert
            cellRef2.ToString().Should().Be("C2");
        }

        /// <summary>
        /// Nexts the row ask for next row should return next row with same column.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void NextRow_AskForNextRow_ShouldReturnNextRowWithSameColumn()
        {
            // Arrange
            var cellRef1 = new CellReference("B2");

            // Act
            var cellRef2 = cellRef1.NextRow();

            // Assert
            cellRef2.ToString().Should().Be("B3");
        }

        /// <summary>
        /// Adds the add1 to rows should return next row retaining column.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Add_Add1ToRows_ShouldReturnNextRowRetainingColumn()
        {
            // Arrange
            var cellRef1 = new CellReference("B2");

            // Act
            var cellRef2 = cellRef1.Add(1);

            // Assert
            cellRef2.ToString().Should().Be("C2");
        }
    }
}