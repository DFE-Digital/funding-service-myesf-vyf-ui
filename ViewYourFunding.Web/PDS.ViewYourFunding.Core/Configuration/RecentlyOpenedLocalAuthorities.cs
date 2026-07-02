using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Core.Configuration
{
    /// <summary>
    /// Represents recently opened local authorities.
    /// </summary>
    public class RecentlyOpenedLocalAuthorities
    {
        /// <summary>
        /// Gets or sets the local authority code list.
        /// </summary>
        /// <value>
        /// The local authority code list.
        /// </value>
        public string LocalAuthorityCodeList { get; set; }

        /// <summary>
        /// Gets or sets the funding period code.
        /// </summary>
        /// <value>
        /// The funding period code.
        /// </value>
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets the local authority codes.
        /// </summary>
        /// <value>
        /// The local authority codes.
        /// </value>
        public IReadOnlyList<string> LocalAuthorityCodes => LocalAuthorityCodeList != null ? LocalAuthorityCodeList.Split(",") : Array.Empty<string>();
    }
}