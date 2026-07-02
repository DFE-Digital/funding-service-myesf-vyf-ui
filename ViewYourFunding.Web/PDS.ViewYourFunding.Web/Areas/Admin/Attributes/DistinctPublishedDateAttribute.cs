using PDS.ViewYourFunding.Services.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Attributes
{
    /// <summary>
    /// The Distinct Published Date Attribute to ensure there is always a distinct published date
    /// in the funding stream publications.
    /// </summary>
    /// <seealso cref="ValidationAttribute" />
    public class DistinctPublishedDateAttribute : ValidationAttribute
    {
        #region Private fields

        /// <summary>
        /// The funding stream identifier property name.
        /// </summary>
        private readonly string _fundingStreamIdPropertyName;

        /// <summary>
        /// The funding stream identifier property name.
        /// </summary>
        private readonly string _fundingPeriodPropertyName;

        /// <summary>
        /// The publication identifier property name.
        /// </summary>
        private readonly string _publicationIdPropertyName;

        /// <summary>
        /// The view your funding settings.
        /// </summary>
        private IAdminPublicationService _adminPublicationService;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DistinctPublishedDateAttribute"/> class.
        /// </summary>
        /// <param name="fundingStreamIdPropertyName">Name of the funding stream identifier property.</param>
        /// <param name="fundingPeriodPropertyName">Name of the funding period identifier property.</param>
        /// <param name="publicationIdPropertyName">Name of the publication identifier property.</param>
        public DistinctPublishedDateAttribute(string fundingStreamIdPropertyName, string fundingPeriodPropertyName, string publicationIdPropertyName)
        {
            _fundingStreamIdPropertyName = fundingStreamIdPropertyName;
            _fundingPeriodPropertyName = fundingPeriodPropertyName;
            _publicationIdPropertyName = publicationIdPropertyName;
        }

        #endregion


        #region Attribute Overrides

        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            _adminPublicationService =
                (IAdminPublicationService)validationContext.GetService(typeof(IAdminPublicationService));

            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();

            var newPublishedDate = (DateTime)value;
            var fundingStreamIdProperty = type.GetProperty(_fundingStreamIdPropertyName);
            var fundingPeriodProperty = type.GetProperty(_fundingPeriodPropertyName);
            if (fundingStreamIdProperty != null)
            {
                var publicationIdProperty = type.GetProperty(_publicationIdPropertyName);
                var publicationId = 0;

                if (publicationIdProperty != null)
                {
                    publicationId = (int)publicationIdProperty.GetValue(instance);
                }

                var fundingStreamId = (int)fundingStreamIdProperty.GetValue(instance);
                var fundingPeriodCode = (string)fundingPeriodProperty.GetValue(instance);
                var duplicate = _adminPublicationService?.GetPublications(fundingStreamId).GetAwaiter().GetResult()
                    .Any(x => x.PublishedDate == newPublishedDate && x.FundingPeriodCode == fundingPeriodCode && x.Id != publicationId);

                if (duplicate != null && duplicate.Value)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }

        #endregion
    }
}