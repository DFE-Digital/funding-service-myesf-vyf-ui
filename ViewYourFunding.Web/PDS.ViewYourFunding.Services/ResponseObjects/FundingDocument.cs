using System;

namespace PDS.ViewYourFunding.Services.ResponseObjects
{
    /// <summary>
    /// Class to represent information about a funding document.
    /// </summary>
    public class FundingDocument
    {
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
        /// Gets or sets the name of the funding stream this document relates to, e.g. DSG.
        /// </summary>
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets the date this document was published.
        /// </summary>
        public DateTime? DocumentPublishedDate { get; set; }

        /// <summary>
        /// Gets or sets the file extension of this document, e.g. ODS.
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Gets or sets the size of this document in bytes.
        /// </summary>
        public long? FileSizeBytes { get; set; }

        /// <summary>
        /// Gets or sets the file path that can be used to download this document.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the cut-off date for funding data that was set when this document was published.
        /// </summary>
        public DateTime CutOffDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether its the latest published document for the year.
        /// </summary>
        public bool IsLatestDocument { get; set; }

        /// <summary>
        /// Gets or sets a value indicting whether its the final publication for the year.
        /// </summary>
        public bool? IsFinal { get; set; }
    }
}