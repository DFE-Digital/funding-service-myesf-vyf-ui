using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Represents a next payment type for the View Your Funding area.
    /// </summary>
    public class NextPaymentType
    {
        /// <summary>
        /// Gets or sets the distinct id for this instance of a next payment type.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Gets or sets the type code.
        /// </summary>
        /// <value>
        /// The type code.
        /// </value>
        public string TypeCode { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        [Required]
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the funding stream.
        /// </summary>
        /// <value>
        /// The funding stream.
        /// </value>
        public FundingStream FundingStream { get; set; }

        /// <summary>
        /// Gets or sets when the next payment type was created.
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the next payment type was last updated.
        /// </summary>
        [Required]
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the username of the last person to update the next payment type.
        /// </summary>
        [Required, MaxLength(128)]
        public string LastUpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the next payments.
        /// </summary>
        /// <value>
        /// The next payments.
        /// </value>
        public ICollection<NextPayment> NextPayments { get; set; }
    }
}
