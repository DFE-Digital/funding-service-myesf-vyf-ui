using PDS.ViewYourFunding.Repositories.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDS.ViewYourFunding.Repositories.DataModels
{
    /// <summary>
    /// Represents a next payment for the View Your Funding area.
    /// </summary>
    public class NextPayment : TableWithIntegerId
    {
        /// <summary>
        /// Gets or sets the database generated Id.
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets the next payment date.
        /// </summary>
        /// <value>
        /// The next payment date.
        /// </value>
        [Required, Column(Order = 0, TypeName = "Date"), DataType(DataType.Date)]
        public DateTime NextPaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the next payment type identifier.
        /// </summary>
        /// <value>
        /// The next payment type identifier.
        /// </value>
        [Required, Column(Order = 1)]
        public int NextPaymentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the next payment.
        /// </summary>
        /// <value>
        /// The type of the next payment.
        /// </value>
        public NextPaymentType NextPaymentType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NextPayment"/> is active.
        /// </summary>
        [Required]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        [Required, Column(Order = 2)]
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the funding stream.
        /// </summary>
        /// <value>
        /// The funding stream.
        /// </value>
        public FundingStream FundingStream { get; set; }

        /// <summary>
        /// Gets or sets the funding period code, e.g. FY-2021.
        /// </summary>
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets when the next payment was created.
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the next payment was last updated.
        /// </summary>
        [Required]
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the username of the last person to update the next payment.
        /// </summary>
        [Required, MaxLength(128)]
        public string LastUpdatedBy { get; set; }
    }
}