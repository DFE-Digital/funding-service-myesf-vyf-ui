using PDS.ViewYourFunding.Services.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Attributes
{
    /// <summary>
    /// The distinct setting type code attribute.
    /// </summary>
    /// <seealso cref="ValidationAttribute" />
    public class DistinctSettingTypeAttribute : ValidationAttribute
    {
        /// <summary>
        /// The settingTypeId identifier property name.
        /// </summary>
        private readonly string _settingTypeIdPropertyName;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DistinctSettingTypeAttribute"/> class.
        /// </summary>
        /// <param name="settingTypeIdPropertyName">Name of the setting Type identifier property.</param>
        public DistinctSettingTypeAttribute(string settingTypeIdPropertyName)
        {
            _settingTypeIdPropertyName = settingTypeIdPropertyName;
        }

        #endregion

        #region Attribute Overrides

        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var settingService = (ISettingTypeService)validationContext.GetService(typeof(ISettingTypeService));

            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();

            var newSettingType = (string)value;

            var settingTypeIdProperty = type.GetProperty(_settingTypeIdPropertyName);
            var settingTypeId = 0;

            if (settingTypeIdProperty != null)
            {
                settingTypeId = (int)settingTypeIdProperty.GetValue(instance);
            }

            var duplicate = settingService?.GetAllSettingTypes().GetAwaiter().GetResult().Any(
                settingType =>
                    settingType.SettingName.Equals(newSettingType, StringComparison.OrdinalIgnoreCase) &&
                    settingType.Id != settingTypeId);

            if (duplicate.HasValue && duplicate.Value)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        #endregion
    }
}