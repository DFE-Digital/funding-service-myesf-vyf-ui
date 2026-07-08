using PDS.ViewYourFunding.Services.Attributes;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using FundingViewData = PDS.ViewYourFunding.Services.DTOs.FundingViewData;

namespace PDS.ViewYourFunding.Services.Implementations.FundingView
{
    /// <summary>
    /// Class for generating funding view data.
    /// </summary>
    public class FundingViewDataGenerator : BaseViewYourFundingRenderer, IFundingViewDataGenerator
    {
        private readonly IComponentService _componentService;
        private readonly IComponentConfigurationService _componentConfigurationService;
        private readonly Dictionary<ComponentType, Defaults> _componentDefaults;

        private readonly UiModel _uiModel;
        private readonly IFundingApiSearchResponseFunding _fundingData;
        private readonly IFundingApiSearchResponseProviderFunding _providerFundingData;

        private readonly FundingStream _fundingStreamConfig;
        private readonly DateTime _publicationDate;
        private readonly DateTime? _previousPublicationDate;
        private readonly FundingDocument _fundingDocument;
        private readonly VarianceSelectionOption _selecetedVarianceOption;
        private readonly bool _isLatestOrFinalFundingForYear;
        private readonly bool _isCurrentYear;
        private readonly int? _publicationUiModelVersion;
        private readonly bool _isALoggedInView;
        private readonly string _fundingPeriodCode;
        private readonly bool _showSelectors;
        private readonly bool _viaChoicePage;
        private readonly bool _asStatementSpecification;
        private readonly bool _showData;

        private readonly string _searchTerm;
        private readonly string _selectedTab;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingViewDataGenerator"/> class.
        /// Create a new instance of a FundingViewDataGenerator.
        /// </summary>
        /// <param name="uiModel">The UI model at the top level.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <param name="fundingData">The funding data to use.</param>
        /// <param name="providerFundingData">The provider funding data to use.</param>
        /// <param name="publicationDate">The publication date (or null).</param>
        /// <param name="previousPublicationDate">The previous publication date (or null).</param>
        /// <param name="fundingStreamConfig">Config about hte funding stream.</param>
        /// <param name="componentDefaults">Defaults for components (e.g. number formats).</param>
        /// <param name="fundingDocument">Information about the funding document (if known).</param>
        /// <param name="isLatestOrFinalFundingForYear">Is the latest or final funding publication for the year.</param>
        /// <param name="isCurrentYear">Is the current year (true or false).</param>
        /// <param name="publicationUiModelVersion">The publication UI model version as set in the admin screens.</param>
        /// <param name="searchTerm">The search term that was used to get here.</param>
        /// <param name="selectedTab">The selected tab.</param>
        /// <param name="componentService">The component service.</param>
        /// <param name="componentConfigurationService">The component configuration service.</param>
        /// <param name="isALoggedInView">Is the view intended for logged in users.</param>
        /// <param name="showSelectors">Show the selectors (useful for debugging).</param>
        /// <param name="selectedVarianceOption">The selected variance option.</param>
        /// <param name="viaChoicePage">Whether we originally entered the site via the allocation type page (pre or post 16).</param>
        /// <param name="asStatementSpecification">Whether to show the statement specification or not.</param>
        /// <param name="showData">Whether to show data or not.</param>
        public FundingViewDataGenerator(
            UiModel uiModel,
            string fundingPeriodCode,
            IFundingApiSearchResponseFunding fundingData,
            IFundingApiSearchResponseProviderFunding providerFundingData,
            DateTime? publicationDate,
            DateTime? previousPublicationDate,
            FundingStream fundingStreamConfig,
            Dictionary<ComponentType, Defaults> componentDefaults,
            FundingDocument fundingDocument,
            bool isLatestOrFinalFundingForYear,
            bool isCurrentYear,
            int? publicationUiModelVersion,
            string searchTerm,
            string selectedTab,
            IComponentService componentService,
            IComponentConfigurationService componentConfigurationService,
            bool isALoggedInView,
            bool showSelectors,
            VarianceSelectionOption selectedVarianceOption,
            bool viaChoicePage,
            bool asStatementSpecification,
            bool showData)
        {
            _uiModel = uiModel;
            _fundingPeriodCode = fundingPeriodCode;
            _fundingData = fundingData;
            _providerFundingData = providerFundingData;
            _publicationDate = publicationDate ?? DateTime.MinValue;
            _previousPublicationDate = previousPublicationDate;
            _fundingStreamConfig = fundingStreamConfig;
            _componentDefaults = componentDefaults;
            _fundingDocument = fundingDocument;
            _isLatestOrFinalFundingForYear = isLatestOrFinalFundingForYear;
            _isCurrentYear = isCurrentYear;
            _publicationUiModelVersion = publicationUiModelVersion;
            _searchTerm = searchTerm;
            _selectedTab = selectedTab;
            _componentService = componentService;
            _componentConfigurationService = componentConfigurationService;
            _isALoggedInView = isALoggedInView;
            _showSelectors = showSelectors;
            _selecetedVarianceOption = selectedVarianceOption;
            _viaChoicePage = viaChoicePage;
            _asStatementSpecification = asStatementSpecification;
            _showData = showData;
        }

        /// <summary>
        /// Generate the funding view data.
        /// </summary>
        /// <returns>The funding view data.</returns>
        public FundingViewData Generate()
        {
            if (_uiModel == null || _fundingStreamConfig == null)
            {
                return null;
            }

            var allDatasetDefinitions = GetAllDatasetDefinitions(
                _uiModel.Groups,
                _uiModel.Dataset ?? new List<UiModelDataset> { new UiModelDataset { DatasetName = "funding" } });

            var allDatasetsData = GetAllDatasetsData(
                _fundingData?.Funding,
                _providerFundingData?.ProviderFunding,
                allDatasetDefinitions,
                _uiModel.AdditionalFundingStreams,
                _fundingStreamConfig.FundingStreamCode);

            if (allDatasetsData == null || allDatasetsData.Count == 0)
            {
                throw new Exception("No data available");
            }

            return PopulateFundingViewData(allDatasetsData, _uiModel.Groups, allDatasetDefinitions);
        }

        private FundingViewData PopulateFundingViewData(
            List<List<IFundingApiSearch>> allDatasetsData,
            List<UiModelGroup> groups,
            List<UiModelDataset> datasetDefinitions)
        {
            var firstFunding = allDatasetsData?.FirstOrDefault()?
                .OrderByDescending(funding => FundingVersionHelper.Parse(funding.FundingVersion)).FirstOrDefault();

            var fundingProperties = _componentConfigurationService.GetFundingProperties(
                firstFunding,
                datasetDefinitions.FirstOrDefault());

            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(_fundingPeriodCode);
            var (asOfMonth, asOfYear) = FundingPublicationDateHelper.GetAcademicAsOfData(
                _publicationDate,
                yearFrom,
                yearTo,
                _fundingStreamConfig.FundingStreamCodePubliclyKnown);

            var componentConfiguration = _componentConfigurationService.GetComponentConfiguration(
                fundingProperties,
                firstFunding,
                datasetDefinitions?.FirstOrDefault(),
                allDatasetsData,
                _uiModel,
                _fundingStreamConfig,
                _fundingDocument,
                _publicationDate,
                _previousPublicationDate,
                _fundingPeriodCode,
                asOfMonth,
                asOfYear,
                _searchTerm,
                _selectedTab,
                _publicationUiModelVersion,
                _selecetedVarianceOption,
                _isLatestOrFinalFundingForYear,
                _isCurrentYear,
                _isALoggedInView,
                _showSelectors,
                _viaChoicePage,
                _asStatementSpecification,
                _showData,
                true);

            ComponentConfiguration previousComponentConfiguration = null;

            if (allDatasetsData?.FirstOrDefault()?.Count > 1)
            {
                var secondDatasetFunding = allDatasetsData?.FirstOrDefault().Skip(1)?.FirstOrDefault();

                var secondFundingProperties = _componentConfigurationService.GetFundingProperties(
                    secondDatasetFunding,
                    datasetDefinitions.FirstOrDefault());

                previousComponentConfiguration = _componentConfigurationService.GetComponentConfiguration(
                    secondFundingProperties,
                    secondDatasetFunding,
                    datasetDefinitions.FirstOrDefault(),
                    allDatasetsData,
                    _uiModel,
                    _fundingStreamConfig,
                    _fundingDocument,
                    _publicationDate,
                    _previousPublicationDate,
                    _fundingPeriodCode,
                    asOfMonth,
                    asOfYear,
                    _searchTerm,
                    _selectedTab,
                    _publicationUiModelVersion,
                    _selecetedVarianceOption,
                    _isLatestOrFinalFundingForYear,
                    _isCurrentYear,
                    _isALoggedInView,
                    _showSelectors,
                    _viaChoicePage,
                    _asStatementSpecification,
                    _showData,
                    true);
            }

            var result = new FundingViewData
            {
                FundingStreamCode = _fundingStreamConfig.FundingStreamCode,
                FundingPeriodCode = _fundingPeriodCode
            };

            if (groups != null)
            {
                foreach (var group in groups)
                {
                    var component = _componentService.GetComponent(
                        group,
                        componentConfiguration,
                        previousComponentConfiguration,
                        _componentDefaults,
                        _publicationDate,
                        _searchTerm,
                        componentConfiguration.Variables);

                    result.Components?.Add(component);
                }
            }

            result.EntityName = fundingProperties?.EntityName;

            var parentIsSameOrganisation = fundingProperties?.EntityParentIdentifier != null &&
                (fundingProperties?.EntityIdentifier == fundingProperties?.EntityParentIdentifier
                    || fundingProperties?.EntityAlternativeIdentifier == fundingProperties?.EntityParentIdentifier);

            // Use the parent orgs name as its generally more UI friendly (e.g. CAMDEN LONDON BOROUGH COUNCIL vs Camden).
            if (parentIsSameOrganisation && !string.IsNullOrEmpty(fundingProperties.EntityParentName))
            {
                result.EntityName = fundingProperties?.EntityParentName;
            }

            result.EntityPrimaryIdentifier = fundingProperties?.EntityIdentifier;
            result.EntityAlternativeIdentifier = fundingProperties?.EntityAlternativeIdentifier;
            result.EntityType = fundingProperties?.EntityType;
            result.EntitySubType = fundingProperties?.EntitySubType;
            result.InYearOpener = componentConfiguration.InYearOpener;
            result.IsIndicativeFunding = componentConfiguration.IsIndicativeFunding;
            result.LocalAuthorityName = componentConfiguration.IsLocalAuthority ? componentConfiguration.ProviderLocalAuthorityName ?? componentConfiguration.LocalAuthorityNameOverride : null;

            var totalAmountString = fundingProperties?.TotalAmount;
            if (decimal.TryParse(totalAmountString, out var totalAmount))
            {
                result.TotalAmount = totalAmount;
            }

            SetupLegacyStuff(result, groups, firstFunding, componentConfiguration, componentConfiguration.Variables);
            return result;
        }

        private void SetupLegacyStuff(
            FundingViewData result,
            List<UiModelGroup> groups,
            IFundingApiSearch fundingValue,
            ComponentConfiguration componentConfiguration,
            Dictionary<string, object> variables)
        {
            if (groups == null || !groups.Any())
            {
                return;
            }

            foreach (var datasetGroup in groups)
            {
                if (datasetGroup.Dataset?.SingleOrDefault() != null)
                {
                    result.FundingSubData = _providerFundingData?.ProviderFunding
                        .Where(
                            providerFunding =>
                                providerFunding.GroupingReason?.Equals("Information", StringComparison.InvariantCultureIgnoreCase) == true &&
                                providerFunding.ParentProviderType?.Equals("LocalAuthority", StringComparison.InvariantCultureIgnoreCase) == true)
                        .ToList();

                    continue;
                }

                var fundingKey = datasetGroup.Id;

                if (string.IsNullOrEmpty(fundingKey))
                {
                    continue;
                }

                var selectedFundingValue = _componentService.GetFundingValue(
                    datasetGroup,
                    componentConfiguration,
                    _publicationDate,
                    _searchTerm,
                    variables);

                result.FundingValues.Add(fundingKey, selectedFundingValue);
            }
        }
    }
}