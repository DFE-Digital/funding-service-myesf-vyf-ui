using System.Text;
using System.Text.RegularExpressions;

namespace PDS.ViewYourFunding.Services.Implementations.FundingView
{
    /// <summary>
    /// Represent the cell reference in a spreadsheet.
    /// </summary>
    public class CellReference
    {
        /// <summary>
        /// Gets or sets the row number.
        /// </summary>
        public int RowNumber { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CellReference"/> class.
        /// Create a cell reference.
        /// </summary>
        /// <param name="cellReference">A cell reference string (e.g. B7).</param>
        public CellReference(string cellReference)
        {
            var characters = Regex.Replace(cellReference, @"[\d-]", string.Empty);
            var rowNumber = int.Parse(cellReference.Replace(characters, string.Empty));

            Characters = characters.ToCharArray();
            RowNumber = rowNumber;
        }

        /// <summary>
        /// Gets or sets the letters (e.g. for cell 'C' this would be ['C'] and for 'AY1' this would be ['A','Y']).
        /// </summary>
        private char[] Characters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CellReference"/> class.
        /// Create a cell reference to a cell that has 2 characters.
        /// </summary>
        /// <param name="prefixCharacter">The prefix letter (e.g. for cell 'AY1' this would be 'A').</param>
        /// <param name="character">The letter (e.g. for cell 'C' this would be 'C' and for 'AY1' this would be 'Y').>.</param>
        /// <param name="rowNumber">The row number.</param>
        private CellReference(char prefixCharacter, char character, int rowNumber)
        {
            Characters = new char[2] { prefixCharacter, character };
            RowNumber = rowNumber;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CellReference"/> class.
        /// Create a cell reference.
        /// </summary>
        /// <param name="cellCharacter">The letter (e.g. for cell 'C' this would be 'C').</param>
        /// <param name="rowNumber">The row number.</param>
        private CellReference(char cellCharacter, int rowNumber)
        {
            Characters = new char[1] { cellCharacter };
            RowNumber = rowNumber;
        }

        /// <summary>
        /// Cell reference for cell A1.
        /// </summary>
        /// <returns>A new cell reference to column A.</returns>
        public static CellReference A()
        {
            return A(0);
        }

        /// <summary>
        /// Cell reference for cell A[RowNumber].
        /// </summary>
        /// <param name="rowNumber">The row number.</param>
        /// <returns>A new cell reference to column A.</returns>
        public static CellReference A(int rowNumber)
        {
            return new CellReference('A', rowNumber);
        }

        /// <summary>
        /// Get the index of the letter (e.g. A is 0, B is 1).
        /// </summary>
        /// <returns>A letters position in the alphabet.</returns>
        public int GetIndex()
        {
            var letterA = A().Characters[0];

            var prefixPosition = Characters.Length > 1 ? Characters[0] - letterA + 1 : 0;
            return (prefixPosition * 26) + (Characters[Characters.Length - 1] - letterA);
        }

        /// <summary>
        /// Get a cell reference as a string.
        /// </summary>
        /// <returns>A spreadsheet cell reference (e.g. A1).</returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var character in Characters)
            {
                stringBuilder.Append(character);
            }

            return $"{stringBuilder.ToString()}{RowNumber}";
        }

        /// <summary>
        /// Get a cell reference as a string.
        /// </summary>
        /// <returns>The letters.</returns>
        public string Letters()
        {
            var stringBuilder = new StringBuilder();

            foreach (var character in Characters)
            {
                stringBuilder.Append(character);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Create a cell reference for the next column.
        /// </summary>
        /// <returns>A new cell reference.</returns>
        public CellReference NextColumn()
        {
            return FromPosition(GetIndex() + 1, RowNumber);
        }

        /// <summary>
        /// Create a cell reference for the next row.
        /// </summary>
        /// <returns>A new cell reference.</returns>
        public CellReference NextRow()
        {
            return FromPosition(GetIndex(), RowNumber + 1);
        }

        /// <summary>
        /// Create a cell reference relative to the current bookmark.
        /// </summary>
        /// <param name="offset">How many columns to skip.</param>
        /// <returns>A new cell reference.</returns>
        public CellReference Add(int offset)
        {
            return FromPosition(GetIndex() + offset, RowNumber);
        }

        /// <summary>
        /// Create a cell reference from a cell position.
        /// </summary>
        /// <param name="position">The cell position.</param>
        /// <param name="rowNumber">The row number.</param>
        /// <returns>A new cell reference.</returns>
        private static CellReference FromPosition(int position, int rowNumber)
        {
            var letterA = A().Characters[0];

            var unitDigit = (char)((position % 26) + letterA);
            var prefixPosition = position / 26;

            if (prefixPosition == 0)
            {
                return new CellReference(unitDigit, rowNumber);
            }

            var prefixDigit = (char)(prefixPosition + letterA - 1);
            return new CellReference(prefixDigit, unitDigit, rowNumber);
        }
    }
}