using Microsoft.AspNetCore.Mvc.Rendering;
using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment
{
    /// <summary>
    /// The next payment class.
    /// </summary>
    /// <seealso cref="NextPaymentDatesViewModel" />
    public class NextPayment : NextPaymentDatesViewModel
    {
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
        /// Gets or sets a value indicating whether this <see cref="NextPaymentViewModel"/> is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the funding period code, e.g. FY-2021 or AY-1920.
        /// </summary>
        [Display(Name = "Funding Period Code")]
        [Required, MaxLength(16)]
        [RegularExpression(FundingPeriodConstants.FundingPeriodCodeRegexPattern, ErrorMessage = FundingPeriodConstants.FundingPeriodCodeErrorMessage)]
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the next payment types.
        /// </summary>
        /// <value>
        /// The next payment types.
        /// </value>
        public IEnumerable<SelectListItem> NextPaymentTypes { get; set; }

        /// <summary>
        /// Gets or sets the last updated by.
        /// </summary>
        /// <value>
        /// the last updated by.
        /// </value>
        public string LastUpdatedBy { get; set; }
    }
}