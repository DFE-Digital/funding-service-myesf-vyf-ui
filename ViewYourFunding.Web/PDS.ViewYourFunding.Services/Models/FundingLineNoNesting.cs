using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A funding line (simplified).
    /// </summary>
    public class FundingLineNoNesting
    {
        /// <summary>
        /// Gets or sets a template line id.
        /// </summary>
        public int TemplateLineId { get; set; }

        /// <summary>
        /// Gets or sets an object that contains a value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is payment type.
        /// </summary>
        /// <value>
        ///   True if this instance is payment type; otherwise, false.
        /// </value>
        public bool IsPaymentType => "payment".Equals(Type, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Gets or sets the distribution periods.
        /// </summary>
        /// <value>
        /// The distribution periods.
        /// </value>
        public List<DistributionPeriod> DistributionPeriods { get; set; } = new List<DistributionPeriod>();
    }
}