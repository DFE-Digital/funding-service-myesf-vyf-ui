using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <inheritdoc cref="IComponentConfigurationService"/>
    public class ComponentConfigurationService : BaseViewYourFundingRenderer, IComponentConfigurationService
    {
        private readonly IComponentService _componentService;
        private readonly IBasePathService _basePathService;
        private const string ShortLAType = "LA";
        private const string AlternativeLAType = "Local Authority";

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentConfigurationService"/> class.
        /// </summary>
        /// <param name="componentService">The component service.</param>
        /// <param name="basePathService">The base path service.</param>
        public ComponentConfigurationService(IComponentService componentService, IBasePathService basePathService)
        {
            _componentService = componentService;
            _basePathService = basePathService;
        }

        /// <inheritdoc />
        public ComponentConfiguration GetComponentConfiguration(
            FundingProperties fundingProperties,
            IFundingApiSearch data,
            UiModelDataset datasetDefinition,
            List<List<IFundingApiSearch>> allDatasetsData,
            UiModel uiModel,
            FundingStream fundingStreamConfig,
            FundingDocument fundingDocument,
            DateTime? publicationDate,
            DateTime? previousPublicationDate,
            string fundingPeriodCode,
            string asOfMonth,
            string asOfYear,
            string searchTerm,
            string selectedTab,
            int? publicationUiModelVersion,
            VarianceSelectionOption selectedVarianceOption,
            bool isLatestOrFinalFundingForYear,
            bool isCurrentYear,
            bool isALoggedInView,
            bool showSelectors,
            bool viaChoicePage,
            bool asStatementSpecification,
            bool showData,
            bool isHtml = true)
        {
            fundingProperties ??= GetFundingProperties(data, datasetDefinition);
            var providerType = GetProviderType(fundingProperties?.EntityType, fundingProperties?.EntitySubType);
            var providerTypeInternal = ProviderTypeInternal.FromExternal(fundingProperties?.EntityType, fundingProperties?.EntitySubType);
            var nextPaymentDateTypeCode = GetNextPaymentDateTypeCode(providerTypeInternal);
            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(fundingPeriodCode);

            var fundingStreamNameWithinSentence = fundingStreamConfig.FundingStreamName;

            if (!string.IsNullOrEmpty(fundingStreamConfig.FundingStreamNameWithinSentence))
            {
                fundingStreamNameWithinSentence = fundingStreamConfig.FundingStreamNameWithinSentence;
            }

            var schemaVersion = 1.0;

            if (data != null)
            {
                schemaVersion = data.GetFundingValueDataValidatedSchemaVersion();
            }

            var componentConfiguration = new ComponentConfiguration
            {
                BasePath = _basePathService.GetApplicationBasePath(),
                UrlForLoggedInView = _basePathService.GetUrlForLoggedInProviderPath(),
                Data = data,
                ShowNameAbbreviation = fundingStreamConfig.FundingStreamCodePubliclyKnown,
                PrimaryIdentifier = fundingProperties?.EntityIdentifier ?? fundingProperties?.EntityParentIdentifier,
                LocalAuthorityName = fundingProperties?.EntityParentName ?? fundingProperties?.EntityName,
                LocalAuthorityNameOverride = GetLocalAuthorityName(fundingProperties),
                ProviderLocalAuthorityName = fundingProperties?.LocalAuthorityName,
                LocalAuthorityCode = fundingProperties?.EntityParentIdentifier ?? fundingProperties?.EntityIdentifier,
                ProviderName = fundingProperties?.EntityName ?? fundingProperties?.EntityParentName,
                ProviderEstablishmentNumber = fundingProperties?.EntityAlternativeIdentifier,
                Year0 = yearFrom - 1,
                Year1 = yearFrom,
                Year2 = yearTo,
                FundingStreamCode = fundingStreamConfig.FundingStreamCode,
                FundingStreamName = fundingStreamConfig.FundingStreamName,
                FundingStreamNameWithinSentence = fundingStreamNameWithinSentence,
                FundingStreamCodeOrName = fundingStreamConfig.FundingStreamCodePubliclyKnown ?
                    fundingStreamConfig.FundingStreamCode : fundingStreamConfig.FundingStreamName,
                ShortFundingStreamNameHtml = FundingStreamHelper.GetFundingStreamCodeHtml(
                    fundingStreamConfig.FundingStreamName,
                    fundingStreamConfig.FundingStreamCode,
                    fundingStreamConfig.FundingStreamCodePubliclyKnown),
                ExpandedFundingStreamNameHtml = FundingStreamHelper.GetFundingStreamNameHtml(
                    fundingStreamConfig.FundingStreamName,
                    fundingStreamConfig.FundingStreamCode,
                    fundingStreamConfig.FundingStreamCodePubliclyKnown),
                FundingStreamPathPart = fundingStreamConfig.FundingStreamName?.ToUIPathComponent(),
                ViaChoicePage = viaChoicePage,
                FundingPeriodCode = fundingPeriodCode,
                YearType = FundingPeriodHelper.GetYearTypeNameFromCode(fundingPeriodCode),
                PublishedDate = publicationDate ?? DateTime.MinValue,
                PublicationUiModelVersion = publicationUiModelVersion,
                StatusChangedDate = fundingProperties?.StatusChangedDate,
                StatusChangedDateUiFormatted = fundingProperties?.StatusChangedDate.ToDateDisplay(),
                OrganisationClosed = "Closed".Equals(fundingProperties?.ProviderStatus, StringComparison.InvariantCultureIgnoreCase),
                NextPaymentDate = GetNextPaymentDate(fundingStreamConfig, fundingPeriodCode, nextPaymentDateTypeCode),
                NoNextPaymentForTheYearText = GetNoNextPaymentDateText(fundingPeriodCode),
                NoNextPaymentForOrganisationText = "There are no more scheduled payments for this organisation.",
                FundingDocument = fundingDocument,
                IsLatestOrFinalFundingForYear = isLatestOrFinalFundingForYear,
                IsCurrentYear = isCurrentYear,
                InitialTab = selectedTab,
                SearchTerm = searchTerm,
                AsOfYear = asOfYear,
                AsOfMonth = asOfMonth,
                IsALoggedInView = isALoggedInView,
                OpeningDay = fundingProperties?.DateOpened?.Day,
                OpeningMonth = fundingProperties?.DateOpened?.Month,
                OpeningYear = fundingProperties?.DateOpened?.Year,
                IsMainstreamAcademy = providerType == ProviderType.MainstreamAcademy,
                IsMainstreamFreeSchool = providerType == ProviderType.MainstreamFreeSchool,
                IsSpecialAcademy = providerType == ProviderType.SpecialAcademy,
                IsSpecialFreeSchool = providerType == ProviderType.SpecialFreeSchool,
                IsMainstreamAcademySponsored = fundingProperties?.EntitySubType == ProviderSubTypeExternal.AcademySponsored,
                IsSpecialPost16ProviderType = fundingProperties?.EntitySubType.IsSpecialPost16ProviderType() == true,
                IsAcademyProviderType = fundingProperties?.EntityType.IsAcademyProviderType() == true,
                IsFurtherEducationProviderType = fundingProperties?.EntityType.IsFurtherEducationProviderType() == true,
                IsSchoolProviderType = fundingProperties?.EntityType.IsSchoolProviderType() == true,
                IsLocalAuthorityProviderType = fundingProperties?.EntityType.IsLocalAuthorityProviderType() == true,
                IsNonProgrammeFundedProviderType = fundingProperties?.EntityType.IsNonProgrammeFundedProviderType() == true,
                ProviderOpenDate = fundingProperties?.DateOpened,
                ProviderOpenDateddMMMMyyyy = fundingProperties?.DateOpened?.ToString("dd MMMM yyyy"),
                ProviderUpin = fundingProperties?.EntityProviderUpin,
                ProviderUrn = fundingProperties?.EntityProviderUrn,
                ProviderType = fundingProperties?.EntityType,
                ProviderSubType = fundingProperties?.EntitySubType,
                RegionName = fundingProperties?.EntityRegionName,
                ShowSelectors = showSelectors,
                OriginalModel = uiModel,
                DaysInYear = DateTime.Now.NumberOfDaysInYear(),
                SchemaVersion = schemaVersion,
                AllDatasetsData = allDatasetsData,
                ShowStatementSpecification = asStatementSpecification,
                ShowData = showData,
                IsHtml = isHtml,
                EntityGroupUkprn = fundingProperties?.EntityGroupUkprn,
                VarianceSelectionOption = selectedVarianceOption
            };

            componentConfiguration.YearTypeCode = FundingPeriodHelper.GetCodeFromYearType(componentConfiguration.YearType);

            InYearOpenerCalculation(fundingProperties, yearFrom, componentConfiguration, yearTo);

            var phaseOfEducation = ExtractStringCalculationValue(componentConfiguration, TemplateCalculationId.PhaseOfEducation);
            componentConfiguration.ProviderIsPrimaryInstitution = IsPrimary(phaseOfEducation);
            componentConfiguration.ProviderIsSecondaryInstitution = IsSecondary(phaseOfEducation);
            componentConfiguration.ProviderIsAllThroughInstitution = IsAllThrough(phaseOfEducation);

            componentConfiguration.VarianceMessage = publicationDate.HasValue && previousPublicationDate.HasValue ?
            ComponentConfigurationHelper.GetVarianceMessage(previousPublicationDate.Value, yearFrom, yearTo, selectedVarianceOption) : null;

            componentConfiguration.IsLocalAuthority =
                ShortLAType.Equals(fundingProperties?.EntityType, StringComparison.InvariantCultureIgnoreCase)
                || AlternativeLAType.Equals(fundingProperties?.EntityType, StringComparison.InvariantCultureIgnoreCase)
                || (fundingProperties?.EntityType != null && fundingProperties.EntityType.StartsWith(GroupingType.LocalAuthority));

            componentConfiguration.IsIndicativeFunding =
                ProviderStatus.IndicativeStatuses.Any(status => status.Equals(fundingProperties?.ProviderStatus, StringComparison.InvariantCultureIgnoreCase))
                && GroupingReason.Indicative.Equals(fundingProperties?.EntityGroupingReason, StringComparison.InvariantCultureIgnoreCase);

            AddCensusVariables(componentConfiguration.Variables, componentConfiguration.PublishedDate, fundingPeriodCode);

            if (uiModel.Variables != null)
            {
                foreach (var variable in uiModel.Variables)
                {
                    // Skip over comment rows
                    if (string.IsNullOrEmpty(variable.Name))
                    {
                        continue;
                    }

                    var value = _componentService.EvaluateVariable(
                        variable,
                        componentConfiguration,
                        componentConfiguration.PublishedDate,
                        searchTerm,
                        Merge(componentConfiguration.ConstantDefinedVariables, componentConfiguration.Variables));

                    componentConfiguration.Variables.Add(variable.Name, value);
                }
            }

            return componentConfiguration;
        }

        /// <inheritdoc />
        public FundingProperties GetFundingProperties(IFundingApiSearch data, UiModelDataset dataset)
        {
            if (data == null)
            {
                return null;
            }

            if (dataset?.DatasetName?.Equals("providerfunding", StringComparison.InvariantCultureIgnoreCase) == true)
            {
                var providerData = (IFundingApiSearchProviderFunding)data;

                return new FundingProperties(
                    providerData.FundingValue,
                    providerData.OrganisationName,
                    providerData.ParentName,
                    providerData.OrganisationUkprn,
                    providerData.ParentPrimaryIdentifier,
                    providerData.OrganisationDfeNumber,
                    providerData.ProviderType,
                    providerData.ProviderSubType,
                    providerData.TotalAmount.ToString(),
                    providerData.ProviderStatus,
                    providerData.PhaseOfEducation,
                    providerData.DateOpened ?? DateTime.MinValue,
                    providerData.RegionName,
                    providerData.ProviderUpin,
                    providerData.ProviderUrn,
                    providerData.StatusChangedDate,
                    providerData.LocalAuthorityName,
                    null,
                    providerData.GroupingReason,
                    providerData.OpenReason,
                    providerData.CloseReason,
                    providerData.DateClosed ?? DateTime.MinValue);
            }

            var fundingData = (IFundingApiSearchFunding)data;

            return new FundingProperties(
                fundingData.FundingValue,
                fundingData.GroupName,
                null,
                fundingData.GroupCode ?? fundingData.GroupUkprn,
                null,
                fundingData.GroupCode,
                fundingData.GroupingType,
                fundingData.GroupingType,
                fundingData.TotalAmount.ToString(),
                null,
                null,
                DateTime.MinValue,
                null,
                null,
                null,
                fundingData.StatusChangedDate,
                null,
                fundingData.GroupUkprn,
                fundingData.GroupingReason,
                null,
                null,
                DateTime.MinValue);
        }

        private static string GetLocalAuthorityName(FundingProperties fundingProperties)
        {
            var localAuthorityName = fundingProperties?.EntityParentName ?? fundingProperties?.EntityName;

            if (localAuthorityName?.Equals(fundingProperties.EntityName, StringComparison.InvariantCultureIgnoreCase) == true &&
                !string.IsNullOrWhiteSpace(fundingProperties.LocalAuthorityName) &&
                !localAuthorityName.Equals(fundingProperties.LocalAuthorityName, StringComparison.InvariantCultureIgnoreCase))
            {
                localAuthorityName = fundingProperties.LocalAuthorityName;
            }

            return localAuthorityName;
        }

        /// <summary>
        /// Gets whether provider is a primary provider.
        /// </summary>
        /// <param name="phaseOfEducation">The phase of education.</param>
        /// <returns>True/False for whether provider is primary or not.</returns>
        private static bool IsPrimary(string phaseOfEducation)
        {
            return phaseOfEducation.Equals("Primary", StringComparison.InvariantCultureIgnoreCase) ||
                   phaseOfEducation.Equals("Middle deemed Secondary", StringComparison.InvariantCultureIgnoreCase) ||
                   phaseOfEducation.Equals("Middle deemed Primary", StringComparison.InvariantCultureIgnoreCase) ||
                   phaseOfEducation.Equals("All-through", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Gets whether provider is a secondary provider.
        /// </summary>
        /// <param name="phaseOfEducation">The phase of education.</param>
        /// <returns>True/False for whether provider is secondary or not.</returns>
        private static bool IsSecondary(string phaseOfEducation)
        {
            return phaseOfEducation.Equals("Secondary", StringComparison.InvariantCultureIgnoreCase) ||
                   phaseOfEducation.Equals("Middle deemed Primary", StringComparison.InvariantCultureIgnoreCase) ||
                   phaseOfEducation.Equals("Middle deemed Secondary", StringComparison.InvariantCultureIgnoreCase) ||
                   phaseOfEducation.Equals("All-through", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Gets whether provider is all through provider.
        /// </summary>
        /// <param name="phaseOfEducation">The phase of education.</param>
        /// <returns>True/False for whether provider is all through or not.</returns>
        private static bool IsAllThrough(string phaseOfEducation)
        {
            return phaseOfEducation.Equals("All-through", StringComparison.InvariantCultureIgnoreCase);
        }


        /// <summary>
        /// Adds census variables to the component configuration.
        /// </summary>
        /// <param name="variables">The variables to be updated.</param>
        /// <param name="publishedDate">The published date.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        private void AddCensusVariables(Dictionary<string, object> variables, DateTime publishedDate, string fundingPeriodCode)
        {
            if (string.IsNullOrEmpty(fundingPeriodCode))
            {
                return;
            }

            var (yearFrom, _) = FundingPeriodHelper.GetYearsFromCode(fundingPeriodCode);

            var yearBeforeFinancialYear = yearFrom - 1;
            var decemberBeforeFinancialYear = new DateTime(yearBeforeFinancialYear, 12, 1);
            var mayOfFinancialYear = new DateTime(yearFrom, 5, 1);
            var betweenDecemberAndMay = publishedDate >= decemberBeforeFinancialYear && publishedDate < mayOfFinancialYear;

            var censusYear = betweenDecemberAndMay ? yearFrom - 1 : yearFrom;

            variables.Add("censusYear", censusYear);
            variables.Add("R06Year1", censusYear - 1);
            variables.Add("R06Year2", censusYear);
        }

        /// <summary>
        /// Get the text to show if there is no next payment date for the year (inferred from funding period code).
        /// </summary>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <returns>The text to display.</returns>
        private string GetNoNextPaymentDateText(string fundingPeriodCode)
        {
            return PaymentTypeCode.GetNoNextPaymentForTheYearText(fundingPeriodCode);
        }

        /// <summary>
        /// Gets the next payment date.
        /// </summary>
        /// <param name="config">The FundingStream details.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <param name="nextPaymentDateTypeCode">The next payment date type code.</param>
        /// <returns>Return next payment date time.</returns>
        private DateTime? GetNextPaymentDate(FundingStream config, string fundingPeriodCode, string nextPaymentDateTypeCode)
        {
            var settingValue = config
                ?.SettingValues?
                .FirstOrDefault(sv => sv?.Setting?.SettingName == "NextPaymentDateTypeCode")?
                .Value;

            if (!string.IsNullOrEmpty(settingValue) && nextPaymentDateTypeCode == PaymentTypeCode.FundingSpecific)
            {
                nextPaymentDateTypeCode = settingValue;
            }

            return PaymentTypeCode.GetNextPaymentDate(
                    config?.NextPayments,
                    nextPaymentDateTypeCode,
                    fundingPeriodCode);
        }

        /// <summary>
        /// Gets provider type on basis of the sub type.
        /// </summary>
        /// <param name="providerType">The provider type.</param>
        /// <param name="providerSubtype">The provider sub type.</param>
        /// <returns>The mapped provider type.</returns>
        private ProviderType GetProviderType(string providerType, string providerSubtype)
        {
            // 16-19 send these through unconverted
            switch (providerSubtype)
            {
                case "11ACA":
                    return ProviderType.Academy;
                case "20MSS":
                    return ProviderType.MaintainedSpecialSchool;
                case "17NMF":
                    return ProviderType.SpecialAcademy;
                case "08SSF":
                    return ProviderType.SchoolSixthForm;
                case "01GFE":
                    return ProviderType.GeneralFEAndTertiary;
                case "22OTH":
                    return ProviderType.Other;
                case "02IPP":
                    return ProviderType.ILP;
                case "18ISP":
                    return ProviderType.SpecialPost16;
                case "15UTC":
                    return ProviderType.UniversityTechnicalCollege;
                case "03SFC":
                    return ProviderType.SixthFormCollege;
                case "12FSC":
                    return ProviderType.FreeSchool;
                case "04AHC":
                    return ProviderType.AgriculturalAndHorticulturalCollege;
                case "22AAP":
                    return ProviderType.AcademyAP;
                case "13SSA":
                    return ProviderType.StudioSchool;
                case "10LAU":
                    return ProviderType.LocalAuthority;
                case "07HEP":
                    return ProviderType.HigherEducationProvider;
                case "19FSS":
                    return ProviderType.SpecialFreeSchool;
                case "05ADC":
                    return ProviderType.ArtAndDesignCollege;
                case "14CTC":
                    return ProviderType.CityTechnologyCollege;
                case "06SDC":
                    return ProviderType.SpecialistDesignatedCollege;
                case "21NMS":
                    return ProviderType.NMSS;
                case "FS1619":
                    return ProviderType.SixteenNineteenFreeSchool;
                case "16NPF":
                    return ProviderType.NonProgrammeFundedProvider;
            }

            // 16-19 unconverted provider types.
            switch (providerType)
            {
                case "Acade":
                    return ProviderType.Academy;
                case "Furth":
                    return ProviderType.SixteenNineteenProvider;
                case "Local":
                    return ProviderType.LocalAuthority;
                case "Schoo":
                    return ProviderType.SchoolSixthForm;
                case "N1618":
                    return ProviderType.NonProgrammeFundedProvider;
                case "PVI":
                    return ProviderType.PrivateVoluntaryIndependentProvider;
            }

            var providerTypeDefault = default(ProviderType);

            switch (providerSubtype)
            {
                case ProviderSubTypeExternal.AcademyConverter:
                case ProviderSubTypeExternal.AcademySponsored:
                case ProviderSubTypeExternal.Academy16to19Converter:
                case ProviderSubTypeExternal.Academy16to19SponsorLed:
                    return ProviderType.MainstreamAcademy;

                case ProviderSubTypeExternal.FreeSchool:
                case ProviderSubTypeExternal.FreeSchool16To19:
                case ProviderSubTypeExternal.FreeSchoolStudioSchool:
                case ProviderSubTypeExternal.FreeSchoolUTC:
                    return ProviderType.MainstreamFreeSchool;

                case ProviderSubTypeExternal.AcademyAlternativeProvisionConverter:
                case ProviderSubTypeExternal.AcademyAlternativeProvisionSponsorLed:
                case ProviderSubTypeExternal.AcademySpecialConverter:
                case ProviderSubTypeExternal.AcademySpecialSponsoreLed:
                    return ProviderType.SpecialAcademy;

                case ProviderSubTypeExternal.FreeSchoolSpecial:
                case ProviderSubTypeExternal.FreeSchoolAlternativeProvision:
                    return ProviderType.SpecialFreeSchool;
                default:
                    return providerTypeDefault;
            }
        }

        /// <summary>
        /// Gets the next payment date type code.
        /// </summary>
        /// <param name="providerTypeInternal">The provider type internal.</param>
        /// <returns>The next payment type code.</returns>
        private string GetNextPaymentDateTypeCode(string providerTypeInternal)
        {
            switch (providerTypeInternal)
            {
                case ProviderTypeInternal.Academy:
                    return PaymentTypeCode.Academy;
                case ProviderTypeInternal.NonMaintainedSpecialSchool:
                    return PaymentTypeCode.NonMaintainedSpecialSchool;
                case ProviderTypeInternal.MaintainedSchool:
                    return PaymentTypeCode.MaintainedSchool;
                default:
                    return PaymentTypeCode.FundingSpecific;
            }
        }

        private int? ExtractCalculationValue(ComponentConfiguration componentConfiguration, int templateCalculationId)
        {
            var calcValue = componentConfiguration?.CalculationAndFundingLines?.Calculations?
                .FirstOrDefault(c => c.Key == templateCalculationId).Value?.Value;
            return calcValue != null ? Convert.ToInt32(calcValue) : (int?)null;
        }

        private string ExtractStringCalculationValue(ComponentConfiguration componentConfiguration, int templateCalculationId)
        {
            var calcValue = componentConfiguration?.CalculationAndFundingLines?.Calculations?
                .FirstOrDefault(c => c.Key == templateCalculationId).Value?.Value;
            return Convert.ToString(calcValue);
        }

        private void InYearOpenerCalculation(
            FundingProperties fundingProperties,
            int yearFrom,
            ComponentConfiguration componentConfiguration,
            int yearTo)
        {
            // Previous Year Posy April Openers
            var previousYearPostAprilStartDate = new DateTime(yearFrom, 4, 1);
            var previousYearPostAprilEndDate = new DateTime(yearFrom, 8, 31);
            var isPreviousYearPostAprilOpener = fundingProperties?.DateOpened >= previousYearPostAprilStartDate &&
                                                fundingProperties?.DateOpened <= previousYearPostAprilEndDate;

            // Open day vs Full year days
            var fullYearDays = ExtractCalculationValue(componentConfiguration, TemplateCalculationId.DaysInFullYear);
            var daysOpen = ExtractCalculationValue(componentConfiguration, TemplateCalculationId.DaysOpenInYear);

            // academic in year opener.
            var academicYearInYearOpener =
                fundingProperties?.DateOpened.IsAcademicYearInYearOpener(yearFrom, yearTo) == true;

            // Current Year in year Openers
            var currentYearStartDate = new DateTime(yearFrom, 9, 1);
            var currentYearEndDate = new DateTime(yearTo, 8, 31);
            componentConfiguration.IsCurrentYearOpener = fundingProperties?.DateOpened >= currentYearStartDate &&
                                                fundingProperties?.DateOpened <= currentYearEndDate;
            componentConfiguration.IsSecondYearInYearOpener = isPreviousYearPostAprilOpener;

            // isReBrokerageCloseReason Calculations
            var isReBrokerageCloseReason = fundingProperties?.CloseReason?.Equals("Fresh Start", StringComparison.OrdinalIgnoreCase) == true
                                            && fundingProperties?.DateClosed != DateTime.MinValue;

            componentConfiguration.InYearOpener = ComponentConfigurationHelper.IsInYearOpener(
                new InYearOpenerCalculatorSource
                {
                    OpeningReason = fundingProperties?.OpenReason,
                    ReBrokerageCloseReason = isReBrokerageCloseReason,
                    IsAcademicYearInYearOpener = academicYearInYearOpener,
                    IsPreviousYearPostAprilOpener = isPreviousYearPostAprilOpener,
                    IsOpenDaysEqualToFullYearDays = fullYearDays != daysOpen
                });


            componentConfiguration.IsPostAprilOpener = isPreviousYearPostAprilOpener;

            if (componentConfiguration.IsPostAprilOpener)
            {
                componentConfiguration.ProviderOpenDaysStartDate = new DateTime(yearFrom, 9, 1);
            }

            // Academy converter in year openers.
            if (!string.IsNullOrWhiteSpace(fundingProperties?.OpenReason))
            {
                componentConfiguration.IsAcademyConverterOrNewProvisionInYearOpener =
                    fundingProperties.DateOpened.IsAcademyConverterOrNewProvisionInYearOpener(
                        yearFrom,
                        yearTo,
                        fundingProperties.OpenReason);
            }
        }
    }
}