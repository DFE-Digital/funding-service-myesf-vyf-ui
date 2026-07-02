using System;

namespace PDS.ViewYourFunding.Services.DTOs
{
    /// <summary>
    /// Information about a funding document (e.g. a spreadsheet).
    /// </summary>
    public class FundingDocumentMetaResponse
    {
        /// <summary>
        /// Gets or sets the file path that can be used to download this document.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the file extension of this document, e.g. ODS.
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Gets or sets the size of this document in bytes.
        /// </summary>
        public long? FileSizeBytes { get; set; }

        /// <summary>
        /// Gets or sets the funding period code (e.g. FY-1920).
        /// </summary>
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the start year of the funding period this document relates to.
        /// </summary>
        public int YearFrom { get; set; }

        /// <summary>
        /// Gets or sets the end year of the funding period this document relates to.
        /// </summary>
        public int YearTo { get; set; }

        /// <summary>
        /// Gets or sets the code of the funding stream this document relates to, e.g. DSG.
        /// </summary>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the date this document was published.
        /// </summary>
        public DateTime DocumentPublishedDate { get; set; }

        /// <summary>
        /// Gets or sets the cut-off date for funding data that was set when this document was published.
        /// </summary>
        public DateTime CutOffDate { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        public DateTime? ModifiedDate { get; set; }
    }
}