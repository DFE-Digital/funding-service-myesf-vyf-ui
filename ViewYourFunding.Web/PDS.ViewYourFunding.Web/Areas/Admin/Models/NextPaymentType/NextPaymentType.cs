using PDS.ViewYourFunding.Web.Areas.Admin.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPaymentType
{
    /// <summary>
    /// The Next payment Type class.
    /// </summary>
    public class NextPaymentType
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the type code.
        /// </summary>
        /// <value>
        /// The type code.
        /// </value>
        [DistinctNextPaymentTypeCode(nameof(FundingStreamId), nameof(Id), ErrorMessage = "Please use a distinct type code for this funding stream.")]
        [Required, Display(Name = "Type Code")]
        public string TypeCode { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the IsNextPaymentTypeInUse.
        /// </summary>
        /// <value>
        /// The IsNextPaymentTypeInUse.
        /// </value>
        public bool IsNextPaymentTypeInUse { get; set; }

        /// <summary>
        /// Gets or sets the LastUpdatedBy.
        /// </summary>
        /// <value>
        /// The LastUpdatedBy.
        /// </value>
        public string LastUpdatedBy { get; set; }
    }
}