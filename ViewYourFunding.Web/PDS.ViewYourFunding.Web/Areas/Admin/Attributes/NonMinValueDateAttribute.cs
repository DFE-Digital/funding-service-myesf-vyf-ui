using System;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Attributes
{
    /// <summary>
    /// The Non Minimum Value Date validation attribute.
    /// </summary>
    /// <seealso cref="ValidationAttribute" />
    public class NonMinValueDateAttribute : ValidationAttribute
    {
        #region Attribute Overrides

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateValue = (DateTime)value;
            if (dateValue.Equals(DateTime.MinValue))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        #endregion
    }
}