using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Filters
{
    /// <summary>
    /// Validate the UI model version and spreadsheet model version numbers.
    /// </summary>
    public class UiSpreadSheetModelVersionAttribute : ValidationAttribute
    {
        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var publication = (PublicationViewModel)validationContext.ObjectInstance;

            if (value == null)
            {
                return ValidationResult.Success;
            }

            int inputValue;
            if (!int.TryParse(value.ToString(), out inputValue))
            {
                return new ValidationResult("Enter a number");
            }

            if (validationContext.MemberName == nameof(publication.UIModelVersion))
            {
                if (inputValue < 1 || inputValue > publication.UIModelMaxVersion)
                {
                    var message = $"Range is between 1 and {publication.UIModelMaxVersion}";
                    if (publication.UIModelMaxVersion == 1)
                    {
                        message = "Maximum value is 1";
                    }

                    return new ValidationResult(message);
                }
            }

            if (validationContext.MemberName == nameof(publication.SpreadsheetModelVersion))
            {
                var message = $"Range is between 1 and {publication.SpreadsheetModelMaxVersion}";
                if (inputValue < 1 || inputValue > publication.SpreadsheetModelMaxVersion)
                {
                    if (publication.SpreadsheetModelMaxVersion == 1)
                    {
                        message = "Maximum value is 1";
                    }

                    return new ValidationResult(message);
                }
            }

            return ValidationResult.Success;
        }
    }
}