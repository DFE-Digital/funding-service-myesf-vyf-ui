using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System;

namespace PDS.ViewYourFunding.Web.Models.ViewYourFunding
{
    /// <summary>
    /// The view model containing the data required to render the '_FundingDocumentLink' partial view.
    /// </summary>
    public class FundingDocumentLinkViewModel
    {
        private const double NumberOfBytesInAKilobyte = 1024.0;

        /// <summary>
        /// Gets or sets a value that will be injected into the markup to ensure each document has a unique ID on the page.
        /// </summary>
        public int DocumentId { get; set; } = 0;

        /// <summary>
        /// Gets or sets the funding document.
        /// </summary>
        public FundingDocument Document { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether or not this is the latest version of the document within the year.
        /// </summary>
        public bool IsLatestVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether whether or not this document is for the current year.
        /// </summary>
        public bool IsCurrentYear { get; set; }

        /// <summary>
        /// Gets or sets the title format string to use for the document link anchor tag. Parameters: {0} = Start Year, {1} = End Year.
        /// </summary>
        public string LinkTitleFormat { get; set; }

        /// <summary>
        /// Gets the title to use for the document link anchor tag.
        /// </summary>
        public string LinkTitle => string.Format(LinkTitleFormat, Document.YearFrom, Document.YearTo);

        /// <summary>
        /// Gets or sets the inner HTML format string to use for the document link anchor tag. Parameters: {0} = Start Year, {1} = End Year.
        /// </summary>
        public string LinkInnerHtmlFormat { get; set; }

        /// <summary>
        /// Gets the inner HTML to use for the document link anchor tag.
        /// </summary>
        public string LinkInnerHtml => string.Format(LinkInnerHtmlFormat, Document.YearFrom, Document.YearTo);

        /// <summary>
        /// Gets or sets the document title format string. Parameters: {0} = Start Year, {1} = End Year.
        /// </summary>
        public string DocumentTitleFormat { get; set; }

        /// <summary>
        /// Gets the title of the document.
        /// </summary>
        public string DocumentTitle => string.Format(DocumentTitleFormat, Document.YearFrom, Document.YearTo);

        /// <summary>
        /// Gets the date the document was published, formatted as 'd MMMM yyyy'.
        /// </summary>
        public string PublishedDate => Document.DocumentPublishedDate?.ToDateDisplay();

        /// <summary>
        /// Gets the file size of the document, rounded to the nearest 1KB.
        /// </summary>
        public int? FileSizeKB => Document.FileSizeBytes.HasValue ? (int)Math.Round(Document.FileSizeBytes.Value / NumberOfBytesInAKilobyte, 0) : (int?)null;

        /// <summary>
        /// Gets or sets a value indicating whether whether or not to show the document published date and (conditionally) the status tag.
        /// </summary>
        public bool ShowPublishedDateAndTag { get; set; }
    }
}