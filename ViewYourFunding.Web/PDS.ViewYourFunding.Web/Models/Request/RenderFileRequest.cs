using System;

namespace PDS.ViewYourFunding.Web.Models.Request
{
    /// <summary>
    /// Request object for render file request.
    /// </summary>
    public class RenderFileRequest
    {
        /// <summary>
        /// Gets or sets the funding id.
        /// </summary>
        public string FundingId { get; set; }

        /// <summary>
        /// Gets or sets the provider funding id.
        /// </summary>
        public string ProviderFundingId { get; set; }

        /// <summary>
        /// Gets or sets the funding stream code (e.g. DSG).
        /// </summary>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the funding period code (e.g. AY-1920).
        /// </summary>
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the publication date (e.g. 2020-01-01).
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the cut off date (e.g. 2020-01-01).
        /// </summary>
        public DateTime CutOffDate { get; set; }

        /// <summary>
        /// Gets or sets the layout ID (a guid).
        /// </summary>
        public string LayoutId { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }
    }
}