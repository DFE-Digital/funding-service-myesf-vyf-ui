using PDS.ViewYourFunding.Services.Implementations.FundingView;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Bookmarks describing how far into rendering a row we've got.
    /// </summary>
    public class RowBookmarks
    {
        /// <summary>
        /// Gets the cell references.
        /// </summary>
        /// <value>
        /// The cell references.
        /// </value>
        public Dictionary<int, CellReference> CellReferences { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RowBookmarks"/> class.
        /// Create a row bookmarks collection.
        /// </summary>
        public RowBookmarks()
        {
            CellReferences = new Dictionary<int, CellReference>();
        }

        /// <summary>
        /// Get a rows current column bookmark.
        /// </summary>
        /// <param name="rowNumber">Which row to look at.</param>
        /// <returns>A cell reference.</returns>
        public CellReference GetRow(int rowNumber)
        {
            // Add new row info if needed
            while (rowNumber >= CellReferences.Count)
            {
                var nextCellReferenceRowNumber = CellReferences.Count + 1;
                var nextCellColumnPosition = CellReferences.ContainsKey(nextCellReferenceRowNumber - 1) ?
                    CellReferences[nextCellReferenceRowNumber - 1].GetIndex() : 0;

                CellReferences.Add(nextCellReferenceRowNumber, CellReference.A(nextCellReferenceRowNumber).Add(nextCellColumnPosition));
            }

            return CellReferences[rowNumber];
        }

        /// <summary>
        /// Set (insert or update) a cell reference.
        /// </summary>
        /// <param name="rowNumber">Which row to look at.</param>
        /// <param name="cellReference">A cell reference.</param>
        public void SetRow(int rowNumber, CellReference cellReference)
        {
            // Add new row info if needed
            while (rowNumber >= CellReferences.Count)
            {
                var nextCellReferenceRowNumber = CellReferences.Count + 1;
                var nextCellColumnPosition = CellReferences.ContainsKey(nextCellReferenceRowNumber - 1) ?
                    CellReferences[nextCellReferenceRowNumber - 1].GetIndex() : 0;

                CellReferences.Add(nextCellReferenceRowNumber, CellReference.A(nextCellReferenceRowNumber).Add(nextCellColumnPosition));
            }

            CellReferences[rowNumber] = cellReference;
        }

        /// <summary>
        /// Add a cell reference.
        /// </summary>
        /// <param name="cellReference">A cell reference.</param>
        public void Save(CellReference cellReference)
        {
            var nextCellReferenceRowNumber = CellReferences.Count + 1;

            CellReferences.Add(nextCellReferenceRowNumber, cellReference);
        }
    }
}