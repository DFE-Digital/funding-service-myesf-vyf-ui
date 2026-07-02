using System;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Component parts of a funding document filename.
    /// </summary>
    public class FileNameComponents
    {
        /// <summary>
        /// Gets or sets the funding stream code (e.g. DSG).
        /// </summary>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the funding period code (e.g. FY-1920).
        /// </summary>
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the publication date from the filename.
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the extension (see FundingDocumentFileType class).
        /// </summary>
        public string Extension { get; set; }
    }
}