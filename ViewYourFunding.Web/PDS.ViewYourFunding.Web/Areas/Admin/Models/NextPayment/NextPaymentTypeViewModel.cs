using PDS.ViewYourFunding.Web.Areas.Admin.Attributes;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment
{
    /// <summary>
    /// The view model for the next payment type.
    /// </summary>
    public class NextPaymentTypeViewModel
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
        public string TypeCode { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
    }
}
