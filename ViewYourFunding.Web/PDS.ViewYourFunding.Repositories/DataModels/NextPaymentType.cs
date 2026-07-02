using PDS.ViewYourFunding.Repositories.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDS.ViewYourFunding.Repositories.DataModels
{
    /// <summary>
    /// Represents a next payment type for the View Your Funding area.
    /// </summary>
    public class NextPaymentType : TableWithIntegerId
    {
        /// <summary>
        /// Gets or sets the database generated ID.
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets the type code.
        /// </summary>
        /// <value>
        /// The type code.
        /// </value>
        [Required, MaxLength(32), Column(Order = 0)]
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
        [Required, Column(Order = 1)]
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the funding stream.
        /// </summary>
        /// <value>
        /// The funding stream.
        /// </value>
        public FundingStream FundingStream { get; set; }

        /// <summary>
        /// Gets or sets the next payments.
        /// </summary>
        /// <value>
        /// The next payments.
        /// </value>
        public ICollection<NextPayment> NextPayments { get; set; }

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
        /// Gets or sets the user name of the last person to update the next payment type.
        /// </summary>
        [Required, MaxLength(128)]
        public string LastUpdatedBy { get; set; }
    }
}
