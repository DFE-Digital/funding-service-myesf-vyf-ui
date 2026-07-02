using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// The class for EmailEnabledFundingStreamAndPeriods.
    /// </summary>
    public class EmailEnabledFundingStreamAndPeriodsResult
    {
        /// <summary>
        /// Gets or sets the funding stream code found.
        /// </summary>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream name found.
        /// </summary>
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets the funding periods.
        /// </summary>
        /// <value>
        /// The funding periods.
        /// </value>
        public List<string> FundingPeriods { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has child view enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has child view enabled; otherwise, <c>false</c>.
        /// </value>
        public bool HasChildViewEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has parent view enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has parent view enabled; otherwise, <c>false</c>.
        /// </value>
        public bool HasParentViewEnabled { get; set; }

        /// <summary>
        /// Gets or sets the digital statements go live date.
        /// </summary>
        /// <value>
        /// The digital statements go live date.
        /// </value>
        public DateTime? DigitalStatementsGoLiveDate { get; set; }
    }
}
