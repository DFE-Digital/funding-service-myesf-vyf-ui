namespace PDS.VYF.Services.Implementations.AppServices
{
    using PDS.ViewYourFunding.Services.Attributes;
    using PDS.ViewYourFunding.Services.Constants;
    using PDS.ViewYourFunding.Services.DTOs;
    using PDS.ViewYourFunding.Services.Enums;
    using PDS.ViewYourFunding.Services.Implementations.FundingView;
    using PDS.ViewYourFunding.Services.Interfaces;
    using PDS.VYF.Services.Abstracts.AppServices;
    using PDS.VYF.Services.Abstracts.InfraServices.FilesServices;
    using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
    using PDS.VYF.Services.Extensions.ModelMapping;
    using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;
    using System.Reflection;

    /// <summary>
    /// The class for Shared Funding View Services.
    /// </summary>
    /// <seealso cref="PDS.VYF.Services.Abstracts.AppServices.ISharedFundingViewServices" />
    public class SharedFundingViewServices : ISharedFundingViewServices
    {
        private readonly IComponentService componentService;
        private readonly IComponentConfigurationService componentConfigurationService;
        private readonly IUIModelFilesServices uIModelFilesServices;
        private readonly ICacheService cacheService;
        private readonly IGlobalSettingsService globalSettingsService;

        private Dictionary<ComponentType, Defaults>? componentDefaults;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedFundingViewServices"/> class.
        /// </summary>
        /// <param name="componentService">The component service.</param>
        /// <param name="componentConfigurationService">The component configuration service.</param>
        /// <param name="uIModelFilesServices">The u i model files services.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="globalSettingsService">The global settings service.</param>
        public SharedFundingViewServices(
            IComponentService componentService,
            IComponentConfigurationService componentConfigurationService,
            IUIModelFilesServices uIModelFilesServices,
            ICacheService cacheService,
            IGlobalSettingsService globalSettingsService)
        {
            this.componentService = componentService;
            this.componentConfigurationService = componentConfigurationService;
            this.uIModelFilesServices = uIModelFilesServices;
            this.cacheService = cacheService;
            this.globalSettingsService = globalSettingsService;
        }

        public virtual async Task<FundingViewData> GetFundingViewData(ViewDataRequestBase viewDataRequest)
        {
            var uiModelResponse = await this.uIModelFilesServices.GetUiModel(viewDataRequest);
            var showData = await this.globalSettingsService.GetValueAsBool(GlobalSettingTypeConstants.ShowDataTypeId);
            var showSelector = await this.globalSettingsService.GetValueAsBool(GlobalSettingTypeConstants.DisplaySelectorsTypeId);
            var asStatementSpecification = await this.globalSettingsService.GetValueAsBool(GlobalSettingTypeConstants.DisplayStatementSpecificationTypeId);

            var fundingViewDataGenerator = new FundingViewDataGenerator(
                uiModelResponse.UiModel,
                viewDataRequest.FundingPeriodCode,
                viewDataRequest.FundingData,
                viewDataRequest.ProviderFundingData?.Combine(viewDataRequest.PreviousProviderFundingData),
                viewDataRequest.PublicationDate,
                viewDataRequest.PreviousPublicationDate,
                viewDataRequest.FundingStreamConfig,
                this.GetComponentDefaults(),
                viewDataRequest.FundingDocument,
                viewDataRequest.IsLatestOrFinalFundingForYear,
                viewDataRequest.IsCurrentYear,
                viewDataRequest.PublicationUiModelVersion,
                viewDataRequest.SearchTerm,
                viewDataRequest.SelectedTab,
                this.componentService,
                this.componentConfigurationService,
                viewDataRequest.IsALoggedInView,
                showSelector,
                viewDataRequest.SelectedVarianceOption ?? VarianceSelectionOption.NoComparison,
                viewDataRequest.ViaChoicePage,
                asStatementSpecification,
                showData);

            return fundingViewDataGenerator.Generate();
        }

        /// <summary>
        /// Gets the component defaults.
        /// </summary>
        /// <returns>Default Components.</returns>
        internal Dictionary<ComponentType, Defaults> GetComponentDefaults()
        {
            if (this.componentDefaults == null)
            {
                this.componentDefaults = this.GetDefaultsUsingReflection();
            }

            return this.componentDefaults;
        }

        /// <summary>
        /// Gets the defaults using reflection.
        /// </summary>
        /// <returns>Component Defaults using reflection.</returns>
        internal Dictionary<ComponentType, Defaults> GetDefaultsUsingReflection()
        {
            var enumType = typeof(ComponentType);
            var allMembers = enumType.GetMembers();

            var returnList = new Dictionary<ComponentType, Defaults>();

            foreach (var componentType in (ComponentType[])Enum.GetValues(typeof(ComponentType)))
            {
                var componentDefaults = this.GetDefaultsAttribute(enumType, allMembers, componentType.ToString());

                if (componentDefaults != null)
                {
                    returnList.Add(componentType, componentDefaults);
                }
            }

            return returnList;
        }

        private Defaults? GetDefaultsAttribute(Type enumType, MemberInfo[] allMembers, string typeString)
        {
            var enumValueMemberInfo = allMembers.FirstOrDefault(m => m.Name == typeString && m.DeclaringType == enumType);
            var customAttributes = enumValueMemberInfo?.GetCustomAttributes(typeof(Defaults), false);

            return customAttributes?.Any() == true
                ? customAttributes?.Select(att => (Defaults)att)?.FirstOrDefault() ?? null
                : null;
        }
    }
}
