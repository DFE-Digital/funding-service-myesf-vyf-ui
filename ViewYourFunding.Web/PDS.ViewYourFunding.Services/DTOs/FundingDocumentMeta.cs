using PDS.ViewYourFunding.Services.Enums;
using System;

namespace PDS.ViewYourFunding.Services.DTOs
{
    /// <summary>
    /// Funding document data and relevant corresponding metadata.
    /// </summary>
    public class FundingDocumentMeta
    {
        /// <summary>
        /// Gets or sets byte array containing the funding document.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets or sets publication date of the spreadsheet.
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets output filename to use.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the file format.
        /// </summary>
        public FileFormat FileFormat { get; set; }
    }
}