using PDS.ViewYourFunding.Services.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Attributes
{
    /// <summary>
    /// The Distinct Funding Stream Name Attribute to ensure there is always a distinct funding stream name.
    /// in the funding stream .
    /// </summary>
    /// <seealso cref="ValidationAttribute" />
    public class DistinctFundingStreamNameAttribute : ValidationAttribute
    {
        /// <summary>
        /// The funding stream identifier property name.
        /// </summary>
        private readonly string _fundingStreamIdPropertyName;

        /// <summary>
        /// The admin setting service.
        /// </summary>
        private IAdminSettingsService _adminSettingsService;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DistinctFundingStreamNameAttribute"/> class.
        /// </summary>
        /// <param name="fundingStreamIdPropertyName">Name of the funding stream identifier property.</param>
        public DistinctFundingStreamNameAttribute(string fundingStreamIdPropertyName)
        {
            _fundingStreamIdPropertyName = fundingStreamIdPropertyName;
        }

        #endregion


        #region Attribute Overrides

        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            _adminSettingsService = (IAdminSettingsService)validationContext.GetService(typeof(IAdminSettingsService));

            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();
            var newFundingStreamName = (string)value;
            var fundingStreamIdProperty = type.GetProperty(_fundingStreamIdPropertyName);
            var fundingStreamId = 0;

            if (fundingStreamIdProperty != null)
            {
                fundingStreamId = (int)fundingStreamIdProperty.GetValue(instance);
            }

            var duplicate = _adminSettingsService?.GetAllFundingStreams().GetAwaiter().GetResult().Any(
                fundingStream =>
                fundingStream.FundingStreamName.Equals(newFundingStreamName, StringComparison.OrdinalIgnoreCase) &&
                fundingStream.Id != fundingStreamId);

            if (duplicate.HasValue && duplicate.Value)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        #endregion
    }
}