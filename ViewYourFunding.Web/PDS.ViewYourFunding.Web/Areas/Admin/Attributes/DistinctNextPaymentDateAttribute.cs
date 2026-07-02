using PDS.ViewYourFunding.Services.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Attributes
{
    /// <summary>
    /// The Distinct NextPayment Date Attribute to ensure there is always a distinct NextPayment date.
    /// in the funding stream nextPayments.
    /// </summary>
    /// <seealso cref="ValidationAttribute" />
    public class DistinctNextPaymentDateAttribute : ValidationAttribute
    {
        #region Private fields

        /// <summary>
        /// The funding stream identifier property name.
        /// </summary>
        private readonly string _fundingStreamIdPropertyName;

        /// <summary>
        /// The nextPayment identifier property name.
        /// </summary>
        private readonly string _nextPaymentIdPropertyName;

        /// <summary>
        /// The nextPayment type identifier property name.
        /// </summary>
        private readonly string _nextPaymentTypeIdPropertyName;

        /// <summary>
        /// The next payment service.
        /// </summary>
        private INextPaymentService _nextPaymentService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DistinctNextPaymentDateAttribute"/> class.
        /// </summary>
        /// <param name="fundingStreamIdPropertyName">Name of the funding stream identifier property.</param>
        /// <param name="nextPaymentTypeIdPropertyName">Name of next payment type identifier property.</param>
        /// <param name="nextPaymentIdPropertyName">Name of the next Payment identifier property.</param>
        public DistinctNextPaymentDateAttribute(string fundingStreamIdPropertyName, string nextPaymentTypeIdPropertyName, string nextPaymentIdPropertyName)
        {
            _fundingStreamIdPropertyName = fundingStreamIdPropertyName;
            _nextPaymentIdPropertyName = nextPaymentIdPropertyName;
            _nextPaymentTypeIdPropertyName = nextPaymentTypeIdPropertyName;
        }

        #endregion

        #region Attribute Overrides

        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            _nextPaymentService = (INextPaymentService)validationContext.GetService(typeof(INextPaymentService));

            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();

            var newNextPaymentDate = (DateTime)value;
            var fundingStreamIdProperty = type.GetProperty(_fundingStreamIdPropertyName);
            if (fundingStreamIdProperty == null)
            {
                return ValidationResult.Success;
            }

            var nextPaymentIdProperty = type.GetProperty(_nextPaymentIdPropertyName);
            var nextPaymentId = 0;

            if (nextPaymentIdProperty != null)
            {
                nextPaymentId = (int)nextPaymentIdProperty.GetValue(instance);
            }

            var nextPaymentTypeIdProperty = type.GetProperty(_nextPaymentTypeIdPropertyName);
            var nextPaymentTypeId = 0;

            if (nextPaymentTypeIdProperty != null)
            {
                nextPaymentTypeId = (int)nextPaymentTypeIdProperty.GetValue(instance);
            }

            var fundingStreamId = (int)fundingStreamIdProperty.GetValue(instance);
            var duplicate = _nextPaymentService?.GetNextPayments(
                fundingStreamId).GetAwaiter().GetResult().Any(
                nextPayment =>
                    nextPayment.NextPaymentDate == newNextPaymentDate &&
                    nextPayment.NextPaymentTypeId == nextPaymentTypeId &&
                    nextPayment.Id != nextPaymentId);

            if (duplicate.HasValue && duplicate.Value)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
        #endregion
    }
}