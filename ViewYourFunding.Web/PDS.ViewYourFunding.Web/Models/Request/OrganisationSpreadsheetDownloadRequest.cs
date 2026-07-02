namespace PDS.ViewYourFunding.Web.Models.Request
{
    /// <summary>
    /// Request object for organisation spreadsheet download.
    /// </summary>
    public class OrganisationSpreadsheetDownloadRequest
    {
        /// <summary>
        /// Gets or sets funding stream code (e.g. DSG).
        /// </summary>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets year from (e.g. 2020).
        /// </summary>
        public int YearFrom { get; set; }

        /// <summary>
        /// Gets or sets year to (e.g. 2020).
        /// </summary>
        public int YearTo { get; set; }

        /// <summary>
        /// Gets or sets year type code (e.g. AY).
        /// </summary>
        public string YearTypeCode { get; set; }

        /// <summary>
        /// Gets or sets file format (e.g. ods).
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets local authority code (e.g. 203).
        /// </summary>
        public string LocalAuthorityCode { get; set; }

        /// <summary>
        /// Gets or sets the published date as a string parameter.
        /// </summary>
        public string PublishedDate { get; set; }

        /// <summary>
        /// Gets or sets the ukprn.
        /// </summary>
        public string Ukprn { get; set; }
    }
}