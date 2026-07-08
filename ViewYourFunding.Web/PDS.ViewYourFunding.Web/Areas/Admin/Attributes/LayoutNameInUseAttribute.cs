using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Attributes
{
    /// <summary>
    /// The layout name in use validation attribute class.
    /// </summary>
    /// <seealso cref="ValidationAttribute" />
    public class LayoutNameInUseAttribute : ValidationAttribute
    {
        /// <summary>
        /// The funding stream identifier property name.
        /// </summary>
        private readonly string _fundingStreamIdPropertyName;

        /// <summary>
        /// The fundingViewType identifier property name.
        /// </summary>
        private readonly string _fundingViewTypePropertyName;

        /// <summary>
        /// The fundingViewScope identifier property name.
        /// </summary>
        private readonly string _fundingViewScopePropertyName;

        /// <summary>
        /// The admin setting service.
        /// </summary>
        private IAdminSettingsService _adminSettingService;

        /// <summary>
        /// The layout management service.
        /// </summary>
        private ILayoutManagementService _layoutManagementService;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutNameInUseAttribute"/> class.
        /// </summary>
        /// <param name="fundingStreamIdPropertyName">Name of the funding stream identifier property.</param>
        /// <param name="fundingViewTypePropertyName">Name of the funding view type property.</param>
        /// <param name="fundingViewScopePropertyName">Name of the funding view scope property.</param>
        public LayoutNameInUseAttribute(string fundingStreamIdPropertyName, string fundingViewTypePropertyName, string fundingViewScopePropertyName)
        {
            _fundingStreamIdPropertyName = fundingStreamIdPropertyName;
            _fundingViewTypePropertyName = fundingViewTypePropertyName;
            _fundingViewScopePropertyName = fundingViewScopePropertyName;
        }

        #endregion


        /// <inheritdoc />
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            _adminSettingService = (IAdminSettingsService)validationContext.GetService(typeof(IAdminSettingsService));
            _layoutManagementService = (ILayoutManagementService)validationContext.GetService(typeof(ILayoutManagementService));

            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();

            var layoutName = (string)value;
            var fundingStreamIdProperty = type.GetProperty(_fundingStreamIdPropertyName);
            if (fundingStreamIdProperty == null)
            {
                return ValidationResult.Success;
            }

            var fundingViewTypeProperty = type.GetProperty(_fundingViewTypePropertyName);
            var fundingViewScopeProperty = type.GetProperty(_fundingViewScopePropertyName);
            var fundingViewType = (FundingViewType)fundingViewTypeProperty.GetValue(instance);
            var fundingViewScope = (FundingViewScope)fundingViewScopeProperty.GetValue(instance);

            var fundingStreamId = (int)fundingStreamIdProperty.GetValue(instance);

            var fundingStream = _adminSettingService?.GetFundingStreamById(
                fundingStreamId,
                FetchData.Publications,
                FetchData.Publications_PublicationLayouts).GetAwaiter().GetResult();

            var dbLayouts = _layoutManagementService.GetAllLayoutsAsync().GetAwaiter().GetResult();

            var existingLayout = dbLayouts?.ToList().FirstOrDefault(layout =>
                !string.IsNullOrWhiteSpace(layout.LayoutName) &&
                layout.LayoutName.Equals(layoutName, StringComparison.InvariantCultureIgnoreCase) &&
                layout.FundingStreamId == fundingStreamId &&
                IsEnumEqual(layout.FundingViewType, fundingViewType) &&
                IsEnumEqual(layout.FundingViewScope, fundingViewScope));

            if (existingLayout == null)
            {
                return ValidationResult.Success;
            }

            var status = GetLayoutStatus(existingLayout.Id, fundingStream.Publications);

            if (status == LayoutStatus.Published)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        private static bool IsEnumEqual<T>(string inputString, T inputEnum)
        {
            return (inputString ?? "0") == inputEnum.ToString();
        }

        private LayoutStatus GetLayoutStatus(string layoutId, ICollection<Services.Models.Publication> publications)
        {
            Guid.TryParse(layoutId, out var layoutGuid);
            var assignedPublication = publications.FirstOrDefault(p => p.PublicationLayouts.Any(pl => pl.LayoutId == layoutId));

            if (assignedPublication == null)
            {
                return LayoutStatus.Disabled;
            }

            return assignedPublication.Status == Services.Enums.PublicationStatus.Published
                ? LayoutStatus.Published
                : LayoutStatus.Preview;
        }
    }
}