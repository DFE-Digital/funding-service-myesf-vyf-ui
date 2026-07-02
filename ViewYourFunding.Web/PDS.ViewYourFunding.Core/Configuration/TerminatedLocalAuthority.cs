using System;

namespace PDS.ViewYourFunding.Core.Configuration
{
    /// <summary>
    /// Represents a Terminated LocalAuthority.
    /// </summary>
    public class TerminatedLocalAuthority
    {
        /// <summary>
        /// Gets or sets the local authority code.
        /// </summary>
        /// <value>
        /// The local authority code.
        /// </value>
        public string LocalAuthorityCode { get; set; }

        /// <summary>
        /// Gets or sets the funding period code.
        /// </summary>
        /// <value>
        /// The funding period code.
        /// </value>
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the final publication date.
        /// </summary>
        /// <value>
        /// The final publication date.
        /// </value>
        public DateTime FinalPublicationDate { get; set; }
    }
}