using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Attributes
{
    /// <summary>
    /// The file extension validation attribute class.
    /// </summary>
    /// <seealso cref="ValidationAttribute" />
    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllowedFileExtensionsAttribute"/> class.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public AllowedFileExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        /// <inheritdoc />
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}