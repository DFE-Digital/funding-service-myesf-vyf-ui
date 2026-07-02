using PDS.ViewYourFunding.Services.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Attributes
{
    /// <summary>
    /// The distinct next payment type code attribute.
    /// </summary>
    /// <seealso cref="ValidationAttribute" />
    public class DistinctNextPaymentTypeCodeAttribute : ValidationAttribute
    {
        /// <summary>
        /// The funding stream identifier property name.
        /// </summary>
        private readonly string _fundingStreamIdPropertyName;

        /// <summary>
        /// The nextPaymentTypeId identifier property name.
        /// </summary>
        private readonly string _nextPaymentTypeIdPropertyName;

        /// <summary>
        /// The next payment type service.
        /// </summary>
        private INextPaymentTypeService _viewYourFundingNextPaymentTypeService;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DistinctNextPaymentTypeCodeAttribute"/> class.
        /// </summary>
        /// <param name="fundingStreamIdPropertyName">Name of the funding stream identifier property.</param>
        /// <param name="nextPaymentTypeIdPropertyName">Name of the next Payment Type identifier property.</param>
        public DistinctNextPaymentTypeCodeAttribute(string fundingStreamIdPropertyName, string nextPaymentTypeIdPropertyName)
        {
            _fundingStreamIdPropertyName = fundingStreamIdPropertyName;
            _nextPaymentTypeIdPropertyName = nextPaymentTypeIdPropertyName;
        }

        #endregion

        #region Attribute Overrides

        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            _viewYourFundingNextPaymentTypeService = (INextPaymentTypeService)validationContext.GetService(typeof(INextPaymentTypeService));

            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();

            var newNextPaymentType = (string)value;
            var fundingStreamIdProperty = type.GetProperty(_fundingStreamIdPropertyName);
            if (fundingStreamIdProperty == null)
            {
                return ValidationResult.Success;
            }

            var nextPaymentTypeIdProperty = type.GetProperty(_nextPaymentTypeIdPropertyName);
            var nextPaymentTypeId = 0;

            if (nextPaymentTypeIdProperty != null)
            {
                nextPaymentTypeId = (int)nextPaymentTypeIdProperty.GetValue(instance);
            }

            var fundingStreamId = (int)fundingStreamIdProperty.GetValue(instance);
            var duplicate = _viewYourFundingNextPaymentTypeService?.GetNextPaymentTypes(
                fundingStreamId).GetAwaiter().GetResult().Any(
                x =>
                    x.TypeCode.Equals(newNextPaymentType, StringComparison.OrdinalIgnoreCase) &&
                    x.Id != nextPaymentTypeId);

            if (duplicate.HasValue && duplicate.Value)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        #endregion
    }
}