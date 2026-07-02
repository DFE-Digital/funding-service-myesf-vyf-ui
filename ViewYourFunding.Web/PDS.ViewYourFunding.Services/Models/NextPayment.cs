using System;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Represents a next payment for the View Your Funding area.
    /// </summary>
    public class NextPayment
    {
        /// <summary>
        /// Gets or sets the distinct id for this instance of a nextpayment.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the next payment date.
        /// </summary>
        /// <value>
        /// The next payment date.
        /// </value>
        public DateTime NextPaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the next payment type identifier.
        /// </summary>
        /// <value>
        /// The next payment type identifier.
        /// </value>
        public int NextPaymentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the next payment type description.
        /// </summary>
        /// <value>
        /// The next payment type description.
        /// </value>
        public string NextPaymentTypeDescription { get; set; }

        /// <summary>
        /// Gets or sets the next payment type code.
        /// </summary>
        /// <value>
        /// The next payment type code.
        /// </value>
        public string NextPaymentTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the type of the next payment.
        /// </summary>
        /// <value>
        /// The type of the next payment.
        /// </value>
        public NextPaymentType NextPaymentType { get; set; }

        /// <summary>
        /// Gets or sets the funding period code, e.g. FY-2021 or AY-1920.
        /// </summary>
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NextPayment"/> is active.
        /// </summary>
        public virtual bool Active { get; set; }

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the funding stream.
        /// </summary>
        /// <value>
        /// The funding stream.
        /// </value>
        public FundingStream FundingStream { get; set; }

        /// <summary>
        /// Gets or sets when the next payment was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the next payment was last updated.
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the username of the last person to update the next payment.
        /// </summary>
        public string LastUpdatedBy { get; set; }
    }
}