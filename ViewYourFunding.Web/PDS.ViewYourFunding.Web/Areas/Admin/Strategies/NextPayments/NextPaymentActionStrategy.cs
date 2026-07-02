using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments
{
    /// <summary>
    /// The next payment strategy for handling related next payment actions.
    /// </summary>
    public class NextPaymentActionStrategy
    {
        /// <summary>
        /// Gets or sets the next payment actions.
        /// </summary>
        /// <value>
        /// The next payment actions.
        /// </value>
        public IList<INextPaymentAction> NextPaymentActions { get; set; }

        /// <summary>
        /// Gets or sets the next payment type actions.
        /// </summary>
        /// <value>
        /// The next payment type actions.
        /// </value>
        public IList<INextPaymentTypeAction> NextPaymentTypeActions { get; set; }
    }
}