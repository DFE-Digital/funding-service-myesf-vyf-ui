using Newtonsoft.Json;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Attributes;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Implementations.Hacks;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.Models.Internal;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FundingDocumentMeta = PDS.ViewYourFunding.Services.DTOs.FundingDocumentMeta;
using FundingViewData = PDS.ViewYourFunding.Services.DTOs.FundingViewData;
using GroupingType = PDS.ViewYourFunding.Services.Constants.GroupingType;
using SearchFilter = PDS.ViewYourFunding.Services.DTOs.SearchFilter;

namespace PDS.ViewYourFunding.Services.Implementations.FundingView
{
    /// <summary>
    /// A service to create views of funding data using models.
    /// </summary>
    public class ModelFundingViewService : IFundingViewService
    {
        #region Injected dependencies

        /// <summary>
        /// Gets or sets the global setting service.
        /// </summary>
        private readonly IGlobalSettingService _globalSettingService;

        /// <summary>
        /// Gets or sets the layout management service.
        /// </summary>
        private readonly ILayoutManagementService _layoutManagementService;

        /// <summary>
        /// Gets or sets service to allow lookup and retrieval of assets.
        /// </summary>
        private readonly IModelFileStoreService _modelFileStoreService;

        /// <summary>
        /// Gets or sets the Component Configuration Service.
        /// </summary>
        private readonly IComponentConfigurationService _componentConfigurationService;

        /// <summary>
        /// Gets or sets the spreadsheet generating service.
        /// </summary>
        private readonly IDocumentManagementService _documentManagementService;

        /// <summary>
        /// Gets or sets the logging service.
        /// </summary>
        private readonly ILoggerAdapter<ModelFundingViewService> _loggerService;

        /// <summary>
        /// Gets or sets the funding api service to use to fetch data from.
        /// </summary>
        private readonly IFundingApiService _fundingApiService;

        /// <summary>
        /// Gets or sets the component factory.
        /// </summary>
        private readonly IComponentFactory _componentFactory;

        /// <summary>
        /// The caching service.
        /// </summary>
        private readonly ICacheService _cacheService;

        #endregion Injected dependencies


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelFundingViewService"/> class.
        /// </summary>
        /// <param name="globalSettingService">The service to get global settings.</param>
        /// <param name="modelFileStoreService">The service to use to retrieve UI models.</param>
        /// <param name="componentConfigurationService">The Component Configuration Service.</param>
        /// <param name="documentManagementService">The document management service to use (e.g. Aspose).</param>
        /// <param name="loggerService">The service to use for logging.</param>
        /// <param name="fundingApiService">The funding api service to use to fetch data from.</param>
        /// <param name="componentFactory">The component factory to use.</param>
        /// <param name="layoutManagementService">The layout management service to use.</param>
        /// <param name="cacheService">The cache service.</param>
        public ModelFundingViewService(
            IGlobalSettingService globalSettingService,
            IModelFileStoreService modelFileStoreService,
            IComponentConfigurationService componentConfigurationService,
            IDocumentManagementService documentManagementService,
            ILoggerAdapter<ModelFundingViewService> loggerService,
            IFundingApiService fundingApiService,
            IComponentFactory componentFactory,
            ILayoutManagementService layoutManagementService,
            ICacheService cacheService)
        {
            _globalSettingService = globalSettingService;
            _modelFileStoreService = modelFileStoreService;
            _componentConfigurationService = componentConfigurationService;
            _documentManagementService = documentManagementService;
            _loggerService = loggerService;
            _fundingApiService = fundingApiService;
            _componentFactory = componentFactory;
            _layoutManagementService = layoutManagementService;
            _cacheService = cacheService;
        }

        #endregion Constructor


        #region IFundingViewService implementation

        /// <inheritdoc/>
        public async Task<List<FundingDocumentMeta>> GenerateFundingDocument(
            FundingStream fundingStream,
            string fundingPeriodCode,
            DateTime publishedDate,
            Publication publication,
            FundingViewType fundingDocumentType,
            FundingViewScope fundingDocumentScope,
            FileFormat[] fileFormats,
            SearchFilter[] filters = null,
            PreviewLayoutModel previewLayoutModel = null,
            bool showSelectors = false,
            bool showStatementSpecification = false,
            bool showData = true,
            IFundingApiSearchFunding[] iFundingApiSearchFunding = null,
            IFundingApiSearchProviderFunding[] iFundingApiSearchProviderFunding = null,
            bool explicitFundingPassed = false,
            bool explicitProviderFundingPassed = false)
        {
            _loggerService.LogInformation("DEBUG1 Method GenerateFundingDocument() start");

            var knownToRequireFundingData = RetrieveFundingDataForSchemaTemplateVersionInfo(fundingDocumentScope) || explicitFundingPassed;
            var knownToRequireProviderFundingData = !knownToRequireFundingData
                && fundingDocumentScope != FundingViewScope.National
                && !fundingDocumentScope.ToString().Contains("History", StringComparison.InvariantCultureIgnoreCase);

            var fundingPeriodCodes = new string[] { fundingPeriodCode };
            var requiresFundingData = RetrieveFundingDataForSchemaTemplateVersionInfo(fundingDocumentScope);
            var filtersOrig = (SearchFilter[])filters?.Clone();

            string groupingType = null;

            if (!requiresFundingData)
            {
                groupingType = fundingStream.ParentGroupTypeFilterSetting();
                var groupingReason = fundingStream.GroupingReasonFilterSetting();

                if (!string.IsNullOrWhiteSpace(groupingReason))
                {
                    var newFilters = filtersOrig?.ToList() ?? new List<SearchFilter>();

                    newFilters.Add(new SearchFilter
                    {
                        PropertyName = SearchFilterPropertyName.GroupingReason,
                        PropertyValue = groupingReason
                    });

                    filtersOrig = newFilters.ToArray();
                }
            }

            var fundingStreams = new List<FundingApiSearchFundingStream>
            {
                new FundingApiSearchFundingStream
                {
                    FundingStreamCode = fundingStream.FundingStreamCode,
                    BeforeDateTime = publication?.CutOffDate ?? publishedDate,
                    Filters = filtersOrig,
                    GroupingType = groupingType,
                    PeriodCodes = fundingPeriodCodes
                }
            };

            var requestObj = new FundingApiSearchRequestObject
            {
                SearchTerm = filters?.FirstOrDefault(f => f.PropertyName == SearchFilterPropertyName.Ukprn)?.PropertyValue,
                WaitForIndexBuild = true,
                FundingStreams = fundingStreams.ToArray()
            };

            var apiService = GetApiService(fundingStream.FundingStreamCode, fundingStream.UseFakeApiService());

            IFundingApiSearchResponseFunding fundingData;

            if (iFundingApiSearchFunding != null)
            {
                fundingData = new FundingApiSearchResponse
                {
                    Funding = iFundingApiSearchFunding
                };
            }
            else if (explicitFundingPassed)
            {
                fundingData = new FundingApiSearchResponse
                {
                    Funding = new List<IFundingApiSearchFunding>()
                };
            }
            else
            {
                fundingData = knownToRequireFundingData ? await apiService.SearchFunding(requestObj) : null;
            }

            IFundingApiSearchResponseProviderFunding providerFundingData;

            if (iFundingApiSearchProviderFunding != null)
            {
                providerFundingData = new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = iFundingApiSearchProviderFunding
                };
            }
            else if (explicitProviderFundingPassed)
            {
                providerFundingData = new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = new List<IFundingApiSearchProviderFunding>()
                };
            }
            else
            {
                providerFundingData = knownToRequireProviderFundingData || (filtersOrig != null && filtersOrig.Count() > 0) ? await apiService.SearchProviderFunding(requestObj, fundingStream.UseLatestFundingData()) : null;
            }

            if (fundingStream.HistoryIndependentOfPublications && providerFundingData != null)
            {
                providerFundingData.ProviderFunding =
                    providerFundingData.ProviderFunding.Where(x => x.StatusChangedDate.Date == publishedDate);
            }

            SchemaTemplateVersion schemaTemplateVersion;

            if (knownToRequireFundingData)
            {
                schemaTemplateVersion = GetSchemaTemplateVersion(fundingData);
            }
            else if (knownToRequireProviderFundingData)
            {
                schemaTemplateVersion = GetSchemaTemplateVersion(providerFundingData);
            }
            else
            {
                schemaTemplateVersion = new SchemaTemplateVersion
                {
                    SchemaVersion = null,
                    TemplateVersion = null
                };
            }

            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(fundingPeriodCode);
            var (asOfMonth, asOfYear) = FundingPublicationDateHelper.GetAcademicAsOfData(publishedDate, yearFrom, yearTo, true);

            var results = await GenerateFundingDocumentToByteArray(
                fundingData,
                providerFundingData,
                fundingStream,
                fundingPeriodCode,
                publication?.CutOffDate ?? publishedDate,
                schemaTemplateVersion.SchemaVersion,
                schemaTemplateVersion.TemplateVersion,
                publication?.SpreadsheetModelVersion,
                publishedDate,
                publication,
                fundingDocumentType,
                fundingDocumentScope,
                fileFormats,
                asOfMonth,
                asOfYear,
                filters,
                previewLayoutModel,
                showSelectors,
                showStatementSpecification,
                showData,
                explicitFundingPassed,
                explicitProviderFundingPassed);

            _loggerService.LogInformation("DEBUG1 Method GenerateFundingDocument() end");
            return results;
        }

        /// <inheritdoc/>
        public MaximumUiSpreadsheetVersion GetMaximumUIAndSpreadsheetVersionNumbers(string fundingStreamCode, string fundingPeriodCode)
        {
            var maxModelVersionUi = GetMaximumTemplateSchemaVersion(fundingStreamCode, fundingPeriodCode, FundingViewType.ViewData);
            var maxModelVersionSpreadsheet = GetMaximumTemplateSchemaVersion(fundingStreamCode, fundingPeriodCode, FundingViewType.Spreadsheet);

            return new MaximumUiSpreadsheetVersion
            {
                MaximumUiVersion = maxModelVersionUi,
                MaximumSpreadsheetVersion = maxModelVersionSpreadsheet
            };
        }

        /// <inheritdoc/>
        public List<FundingApiSearchRequestObject> GetDataRequirements(
            FundingStream fundingStreamConfig,
            string fundingPeriodCode,
            DateTime cutOffDate,
            FundingViewScope fundingViewScope,
            SearchFilter[] filters = null,
            IFundingApiSearchFunding iFundingApiSearchFunding = null,
            IFundingApiSearchProviderFunding iFundingApiSearchProviderFunding = null,
            string groupType = GroupingType.LocalAuthority)
        {
            var fundingPeriodCodes = new string[] { fundingPeriodCode };

            var requiresFundingData = RetrieveFundingDataForSchemaTemplateVersionInfo(fundingViewScope);
            var requiresProviderFundingData = !requiresFundingData
                && fundingViewScope != FundingViewScope.National
                && !fundingViewScope.ToString().Contains("History", StringComparison.InvariantCultureIgnoreCase);

            var fundingStreams = new List<FundingApiSearchFundingStream>
            {
                new FundingApiSearchFundingStream
                {
                    FundingStreamCode = fundingStreamConfig.FundingStreamCode,
                    BeforeDateTime = cutOffDate,
                    Filters = filters,
                    GroupingType = groupType,
                    PeriodCodes = fundingPeriodCodes
                }
            };

            var returnList = new List<FundingApiSearchRequestObject>();

            if (iFundingApiSearchFunding == null && requiresFundingData)
            {
                var requestObj = new FundingApiSearchRequestObject
                {
                    SearchTerm = null,
                    WaitForIndexBuild = true,
                    FundingStreams = fundingStreams.ToArray(),
                    Type = "Funding"
                };

                returnList.Add(requestObj);
            }

            if (iFundingApiSearchProviderFunding == null && requiresProviderFundingData)
            {
                var requestObj = new FundingApiSearchRequestObject
                {
                    SearchTerm = null,
                    WaitForIndexBuild = true,
                    FundingStreams = fundingStreams.ToArray(),
                    Type = "ProviderFunding"
                };

                returnList.Add(requestObj);
            }

            return returnList;
        }

        /// <inheritdoc/>
        public async Task<FundingViewData> GenerateFundingViewData(
            IComponentService componentService,
            string fundingPeriodCode,
            string fundingStreamCode,
            FundingStream[] allFundingStreams,
            DateTime cutOffDate,
            Publication publication,
            int? modelVersion,
            FundingViewScope fundingViewScope,
            Dictionary<ComponentType, Defaults> componentDefaults = null,
            FundingDocument fundingDocument = null,
            bool isLatestOrFinalFundingForYear = false,
            bool isCurrentYear = false,
            DateTime? previousPublicationDate = null,
            SearchFilter[] filters = null,
            string searchTerm = null,
            string selectedTab = null,
            bool bubbleUpException = true,
            IFundingApiSearchFunding[] iFundingApiSearchFunding = null,
            IFundingApiSearchProviderFunding[] iFundingApiSearchProviderFunding = null,
            bool explicitFundingPassed = false,
            bool explicitProviderFundingPassed = false,
            PreviewLayoutModel previewLayoutModel = null,
            VarianceSelectionOption selectedVarianceOption = VarianceSelectionOption.NoComparison,
            bool viaChoicePage = false,
            bool showSelectors = false,
            bool asStatementSpec = false,
            bool showData = true)
        {
            var fundingPeriodCodes = new string[] { fundingPeriodCode };

            var knownToRequireFundingData = RetrieveFundingDataForSchemaTemplateVersionInfo(fundingViewScope) || explicitFundingPassed;
            var knownToRequireProviderFundingData = !knownToRequireFundingData
                && fundingViewScope != FundingViewScope.National
                && !fundingViewScope.ToString().Contains("History", StringComparison.InvariantCultureIgnoreCase);

            var fundingStreamConfig = allFundingStreams
                .First(fundingStream => fundingStream.FundingStreamCode == fundingStreamCode);

            var fundingStreams = new List<FundingApiSearchFundingStream>
            {
                new FundingApiSearchFundingStream
                {
                    FundingStreamCode = fundingStreamConfig.FundingStreamCode,
                    BeforeDateTime = cutOffDate,
                    Filters = filters,
                    GroupingType = GroupingType.LocalAuthority,
                    PeriodCodes = fundingPeriodCodes
                }
            };

            var requestObj = new FundingApiSearchRequestObject
            {
                SearchTerm = null,
                WaitForIndexBuild = true,
                FundingStreams = fundingStreams.ToArray()
            };

            var apiService = GetApiService(fundingStreamConfig.FundingStreamCode, fundingStreamConfig.UseFakeApiService());
            IFundingApiSearchResponseFunding fundingData;

            if (iFundingApiSearchFunding != null)
            {
                fundingData = new FundingApiSearchResponse
                {
                    Funding = iFundingApiSearchFunding
                };
            }
            else if (explicitFundingPassed)
            {
                fundingData = new FundingApiSearchResponse
                {
                    Funding = new List<IFundingApiSearchFunding>()
                };
            }
            else
            {
                fundingData = knownToRequireFundingData ? await apiService.SearchFunding(requestObj) : null;
            }

            IFundingApiSearchResponseProviderFunding providerFundingData;

            if (iFundingApiSearchProviderFunding != null)
            {
                providerFundingData = new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = iFundingApiSearchProviderFunding
                };
            }
            else if (explicitProviderFundingPassed)
            {
                providerFundingData = new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = new List<IFundingApiSearchProviderFunding>()
                };
            }
            else
            {
                providerFundingData = knownToRequireProviderFundingData ? await apiService.SearchProviderFunding(requestObj, fundingStreamConfig.UseLatestFundingData()) : null;
            }

            SchemaTemplateVersion schemaTemplateVersion;

            if (knownToRequireFundingData)
            {
                schemaTemplateVersion = GetSchemaTemplateVersion(fundingData);
            }
            else if (knownToRequireProviderFundingData)
            {
                schemaTemplateVersion = GetSchemaTemplateVersion(providerFundingData);
            }
            else
            {
                schemaTemplateVersion = new SchemaTemplateVersion
                {
                    SchemaVersion = null,
                    TemplateVersion = null
                };
            }

            return schemaTemplateVersion != null ?
                await GenerateFundingViewData(
                    componentService,
                    fundingPeriodCode,
                    fundingStreamCode,
                    fundingData,
                    providerFundingData,
                    allFundingStreams,
                    cutOffDate,
                    publication,
                    previousPublicationDate,
                    schemaTemplateVersion.SchemaVersion,
                    schemaTemplateVersion.TemplateVersion,
                    modelVersion,
                    fundingViewScope,
                    componentDefaults,
                    fundingDocument,
                    isLatestOrFinalFundingForYear,
                    isCurrentYear,
                    filters,
                    searchTerm,
                    selectedTab,
                    bubbleUpException,
                    previewLayoutModel,
                    selectedVarianceOption,
                    viaChoicePage,
                    showSelectors,
                    asStatementSpec,
                    showData,
                    explicitFundingPassed,
                    explicitProviderFundingPassed) : null;
        }

        #endregion IFundingViewService implementation


        #region Protected methods

        /// <summary>
        /// Generate the funding document for the given funding data using the specified model.
        /// </summary>
        /// <param name="fundingData">The data to include in the spreadsheet.</param>
        /// <param name="providerFundingData">The provider funding data to include in the spreadsheet.</param>
        /// <param name="fundingStream">The funding stream config.</param>
        /// <param name="uiModelJson">A Json representation of the UI model to use.</param>
        /// <param name="fundingPeriodCode">A funding period code (e.g. DSG).</param>
        /// <param name="periodStartYear">The year of the start of the funding period for the spreadsheet, e.g. for FY1819 it would be 2018.</param>
        /// <param name="periodEndYear">The year of the end of the funding period for the spreadsheet, e.g. for FY1819 it would be 2019.</param>
        /// <param name="publicationDate">The publication date to give on the spreadsheet.</param>
        /// <param name="fileFormat">File format e.g. csv, ods. by default use a popular vendor specific version (e.g. XLS 2003).</param>
        /// <param name="asOfMonth">The 'as of' month used in the UI.</param>
        /// <param name="asOfYear">The 'as of' year used in the UI.</param>
        /// <param name="showSelectors">Whether to show selectors or not.</param>
        /// <param name="showStatementSpecification">Whether to show statement specification or not.</param>
        /// <param name="showData">Whether to show data or not.</param>
        /// <returns>A byte array containing the spreadsheet.</returns>
        protected byte[] GenerateFundingDocument(
            IFundingApiSearchResponseFunding fundingData,
            IFundingApiSearchResponseProviderFunding providerFundingData,
            FundingStream fundingStream,
            string uiModelJson,
            string fundingPeriodCode,
            int periodStartYear,
            int periodEndYear,
            DateTime publicationDate,
            FileFormat fileFormat,
            string asOfMonth,
            string asOfYear,
            bool showSelectors,
            bool showStatementSpecification,
            bool showData)
        {
            var uiModel = JsonConvert.DeserializeObject<UiModel>(uiModelJson);

            if (uiModel == null)
            {
                throw new Exception("UI model is null");
            }

            return GenerateFundingDocument(
                fundingData,
                providerFundingData,
                uiModel,
                fundingStream,
                fundingPeriodCode,
                periodStartYear,
                periodEndYear,
                publicationDate,
                fileFormat,
                asOfMonth,
                asOfYear,
                showSelectors,
                showData,
                showStatementSpecification);
        }

        #endregion Protected methods


        #region Private helper methods

        /// <summary>
        /// Checks if the funding view scope refers to a 'logged-in' view.
        /// </summary>
        /// <param name="fundingViewScope">The funding scope to check.</param>
        /// <returns>true if the fundingViewScope refers to a 'logged-in' view.</returns>
        private static bool IsALoggedInView(FundingViewScope fundingViewScope)
        {
            return fundingViewScope.GetType().GetMember(fundingViewScope.ToString()).FirstOrDefault()?
                       .GetCustomAttributes().Any(attr => attr is LoggedInViewAttribute) == true;
        }

        private static bool MultiDataSetGroupAsRowSpreadSheet(UiModel uiModel)
        {
            return uiModel.Type.Equals("spreadsheetWithMultiDataWorkSheet", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Helper method to determine whether funding data should be initially loaded, in order to determine the schema/template version information.
        /// </summary>
        /// <param name="fundingViewScope">The scopes of the requested funding view.</param>
        /// <returns>If funding data is required, then true. If provider funding data is required, then false.</returns>
        private static bool RetrieveFundingDataForSchemaTemplateVersionInfo(FundingViewScope fundingViewScope)
        {
            return fundingViewScope == FundingViewScope.Organisation
                   || fundingViewScope == FundingViewScope.OrganisationSummary
                   || fundingViewScope == FundingViewScope.LoggedInOrganisationSummary
                   || fundingViewScope == FundingViewScope.LoggedInOrganisationSsf;
        }

        private Dictionary<string, IFundingApiService> GetApiServices(Dictionary<string, FundingStream> fundingStreams)
        {
            var returnList = new Dictionary<string, IFundingApiService>();

            foreach (var fundingStream in fundingStreams)
            {
                var apiService = GetApiService(fundingStream.Value.FundingStreamCode, fundingStream.Value.UseFakeApiService());
                returnList.Add(fundingStream.Key, apiService);
            }

            return returnList;
        }

        private IFundingApiService GetApiService(string fundingStreamCode, bool useFakeApiService)
        {
            if (!useFakeApiService)
            {
                return _fundingApiService;
            }

            // TODO - eventually remove the following if statements
            if (fundingStreamCode.Equals("PP", StringComparison.InvariantCultureIgnoreCase))
            {
                return new LocalPPG_FakeApiService(null, _fundingApiService);
            }

            if (fundingStreamCode.Equals("DSG", StringComparison.InvariantCultureIgnoreCase))
            {
                return new LocalDSG_FakeApiService(null, _fundingApiService);
            }

            if (fundingStreamCode.Equals("GAG", StringComparison.InvariantCultureIgnoreCase))
            {
                return new LocalGAG_FakeApiService(null);
            }
            else if (fundingStreamCode.Equals("1619", StringComparison.InvariantCultureIgnoreCase))
            {
                return new Local1619_FakeApiService(null);
            }
            else if (fundingStreamCode.Equals("NMSS", StringComparison.InvariantCultureIgnoreCase))
            {
                return new LocalNMSS_FakeApiService(null);
            }
            else if (fundingStreamCode.Equals("1416", StringComparison.InvariantCultureIgnoreCase))
            {
                return new Local1416_FakeApiService(null);
            }
            else if (fundingStreamCode.Equals("LAREC", StringComparison.InvariantCultureIgnoreCase))
            {
                return new LocalLAREC_FakeApiService(null);
            }
            else if (fundingStreamCode.Equals("PNA", StringComparison.InvariantCultureIgnoreCase))
            {
                return new LocalPNA_FakeApiService(null);
            }
            else if (fundingStreamCode.Equals("UIFSM", StringComparison.InvariantCultureIgnoreCase))
            {
                return new LocalUIFSM_FakeApiService(null, _fundingApiService);
            }

            return _fundingApiService;
        }

        /// <summary>
        /// Generate the funding document for the given funding data using the specified model.
        /// </summary>
        /// <param name="fundingData">The data to include in the spreadsheet.</param>
        /// <param name="providerFundingData">The provider funding data to include in the spreadsheet.</param>
        /// <param name="uiModel">The UI model to use.</param>
        /// <param name="fundingStream">The funding stream config.</param>
        /// <param name="fundingPeriodCode">A funding period code (e.g. DSG).</param>
        /// <param name="periodStartYear">The year of the start of the funding period for the spreadsheet, e.g. for FY1819 it would be 2018.</param>
        /// <param name="periodEndYear">The year of the end of the funding period for the spreadsheet, e.g. for FY1819 it would be 2019.</param>
        /// <param name="publicationDate">The publication date to give on the spreadsheet.</param>
        /// <param name="fileFormat">File format e.g. csv, ods. by default use a popular vendor specific version (e.g. XLS 2003).</param>
        /// <param name="asOfMonth">'As of' month (e.g. Apr).</param>
        /// <param name="asOfYear">'As of' year (e.g. 2009).</param>
        /// <param name="showSelectors">Show selectors or not.</param>
        /// <param name="showStatementSpecification">Whether to show statement specification or not.</param>
        /// <param name="showData">Whether to show data or not.</param>
        /// <returns>A byte array containing the spreadsheet.</returns>
        private byte[] GenerateFundingDocument(
            IFundingApiSearchResponseFunding fundingData,
            IFundingApiSearchResponseProviderFunding providerFundingData,
            UiModel uiModel,
            FundingStream fundingStream,
            string fundingPeriodCode,
            int periodStartYear,
            int periodEndYear,
            DateTime publicationDate,
            FileFormat fileFormat,
            string asOfMonth,
            string asOfYear,
            bool showSelectors,
            bool showStatementSpecification,
            bool showData)
        {
            if (uiModel == null)
            {
                throw new Exception("UI model is null");
            }

            var spreadsheet = new Spreadsheet(
                _globalSettingService,
                _modelFileStoreService,
                _componentConfigurationService,
                uiModel,
                fundingData,
                providerFundingData,
                fundingStream,
                fundingPeriodCode,
                periodStartYear,
                periodEndYear,
                publicationDate,
                asOfMonth,
                asOfYear,
                showSelectors,
                showStatementSpecification,
                showData);

            spreadsheet.Build();

            return RenderSpreadsheet(spreadsheet, fileFormat);
        }

        /// <summary>
        /// Build a spreadsheet  using data.
        /// </summary>
        /// <param name="spreadsheet">The data to build the spreadsheet from.</param>
        /// <param name="fileFormat">File format e.g. csv, ods. by default use a popular vendor specific version (e.g. XLS 2003).</param>
        /// <returns>The generated spreadsheet as a byte array.</returns>
        private byte[] RenderSpreadsheet(Spreadsheet spreadsheet, FileFormat fileFormat)
        {
            return _documentManagementService.CreateSpreadsheetWithData(spreadsheet, fileFormat, true);
        }

        /// <summary>
        /// Get the schema template version for some funding data.
        /// </summary>
        /// <param name="fundingData">Funding data to look at.</param>
        /// <returns>Schema template version.</returns>
        private SchemaTemplateVersion GetSchemaTemplateVersion(IFundingApiSearchResponseFunding fundingData)
        {
            var firstFunding = fundingData.Funding.FirstOrDefault();

            if (firstFunding == null)
            {
                return null;
            }

            return new SchemaTemplateVersion
            {
                SchemaVersion = double.Parse(firstFunding.SchemaVersion ?? "1.0"),
                TemplateVersion = double.Parse(firstFunding.TemplateVersion ?? "1.0")
            };
        }

        /// <summary>
        /// Get the schema template version for some provider funding data.
        /// </summary>
        /// <param name="providerFundingData">Provider funding data to look at.</param>
        /// <returns>Schema template version.</returns>
        private SchemaTemplateVersion GetSchemaTemplateVersion(IFundingApiSearchResponseProviderFunding providerFundingData)
        {
            var firstProviderFunding = providerFundingData.ProviderFunding?.FirstOrDefault();

            if (firstProviderFunding == null)
            {
                return null;
            }

            return new SchemaTemplateVersion
            {
                SchemaVersion = double.Parse(firstProviderFunding.SchemaVersion ?? "1.0"),
                TemplateVersion = double.Parse(firstProviderFunding.TemplateVersion ?? "1.0")
            };
        }

        /// <summary>
        /// Get the relevant grouping field for this funding. In most cases - group code (e.g. 202) will be what we should group on - but in edge cases its UKPRN.
        /// </summary>
        /// <param name="funding">Funding being downloaded.</param>
        /// <returns>Relevant grouping field for document generation.</returns>
        private string GetRelevantGroupingField(IFundingApiSearchFunding funding)
        {
            return !string.IsNullOrEmpty(funding.GroupCode) ? funding.GroupCode : funding.GroupUkprn;
        }

        /// <summary>
        /// Generate a funding document to a byte array.
        /// </summary>
        /// <param name="fundingData">The data to include in the spreadsheet.</param>
        /// <param name="providerFundingData">The provider funding data to include in the spreadsheet.</param>
        /// <param name="fundingStream">The funding stream config (e.g. PE and sport premium).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. AY-1920).</param>
        /// <param name="cutoffDate">Only get funding published before this date and time.</param>
        /// <param name="schemaVersion">The schema version to use.</param>
        /// <param name="templateVersion">The template version to use.</param>
        /// <param name="modelVersion">Model version number of spreadsheet or UI.</param>
        /// <param name="publishedDate">The published date - as on publication or status changed date.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="fundingDocumentType">The funding document type (e.g. Spreadsheet).</param>
        /// <param name="fundingDocumentScope">The funding document scope (e.g. National).</param>
        /// <param name="fileFormats">File format e.g. csv, ods. by default use a popular vendor specific version (e.g. XLS 2003).</param>
        /// <param name="asOfMonth">'As of' month (e.g. Apr).</param>
        /// <param name="asOfYear">'As of' year (e.g. 2009).</param>
        /// <param name="filters">Optional filters.</param>
        /// <param name="previewLayoutModel">The preview layout model.</param>
        /// <param name="showSelectors">Whether to show selectors or not.</param>
        /// <param name="showStatementSpecification">Whether to show statement specification or not.</param>
        /// <param name="showData">Whether to show data or not.</param>
        /// <param name="explicitFundingPassed">Was funding data passed? (even if it was null).</param>
        /// <param name="explicitProviderFundingPassed">Was provider funding data passed? (even if it was null).</param>
        /// <returns>A URI for the spreadsheet.</returns>
        private async Task<List<FundingDocumentMeta>> GenerateFundingDocumentToByteArray(
            IFundingApiSearchResponseFunding fundingData,
            IFundingApiSearchResponseProviderFunding providerFundingData,
            FundingStream fundingStream,
            string fundingPeriodCode,
            DateTime cutoffDate,
            double? schemaVersion,
            double? templateVersion,
            int? modelVersion,
            DateTime publishedDate,
            Publication publication,
            FundingViewType fundingDocumentType,
            FundingViewScope fundingDocumentScope,
            FileFormat[] fileFormats,
            string asOfMonth,
            string asOfYear,
            SearchFilter[] filters = null,
            PreviewLayoutModel previewLayoutModel = null,
            bool showSelectors = false,
            bool showStatementSpecification = false,
            bool showData = true,
            bool explicitFundingPassed = false,
            bool explicitProviderFundingPassed = false)
        {
            _loggerService.LogInformation("DEBUG1 Method GenerateFundingDocumentToByteArray() start");

            var uiModelJson = await GetUiModelJson(
                fundingStream.FundingStreamCode,
                fundingPeriodCode,
                schemaVersion,
                templateVersion,
                modelVersion,
                fundingDocumentType,
                fundingDocumentScope,
                publication,
                previewLayoutModel);

            var uiModel = JsonConvert.DeserializeObject<UiModel>(uiModelJson?.Json);

            var fundingStreams = new List<FundingApiSearchFundingStream>
            {
                new FundingApiSearchFundingStream
                {
                    FundingStreamCode = fundingStream.FundingStreamCode,
                    BeforeDateTime = cutoffDate,
                    Filters = filters,
                    GroupingType = null,
                    PeriodCodes = new[] { fundingPeriodCode }
                }
            };

            var requestObj = new FundingApiSearchRequestObject
            {
                SearchTerm = null,
                WaitForIndexBuild = true,
                FundingStreams = fundingStreams.ToArray()
            };

            var apiService = GetApiService(fundingStream.FundingStreamCode, fundingStream.UseFakeApiService());

            if (uiModel.Type != null && MultiDataSetGroupAsRowSpreadSheet(uiModel))
            {
                (fundingData, providerFundingData) = await FetchDataForDatasets(
                        uiModel,
                        fundingData,
                        providerFundingData,
                        explicitFundingPassed,
                        explicitProviderFundingPassed,
                        filters,
                        new Dictionary<string, FundingStream> { { "Primary", fundingStream } },
                        new Dictionary<string, IFundingApiService> { { "Primary", apiService } },
                        cutoffDate,
                        fundingPeriodCode);
            }
            else
            {
                if (fundingData == null && NeedsFundingData(uiModel))
                {
                    var filtersOrig = (SearchFilter[])filters?.Clone();

                    // If we filtered the provider funding data on Parent Primary ID (e.g. LA code),
                    // then we need to filter the funding data on the Primary ID.
                    if (filters?.Length == 1 && filters.Single().PropertyName == SearchFilterPropertyName.ParentPrimaryIdentifier)
                    {
                        filters.Single().PropertyName = SearchFilterPropertyName.PrimaryIdentifier;
                    }

                    fundingData = await apiService.SearchFunding(requestObj);
                    filters = filtersOrig;
                }

                if (providerFundingData == null && NeedsProviderFundingData(uiModel))
                {
                    var filtersOrig = filters == null ? null : (SearchFilter[])filters.Clone();

                    // If we filtered the funding data on Primary ID (e.g. LA code),
                    // then we need to filter the provider funding data on the Parent Primary ID.
                    if (filters?.Length == 1 && filters.Single().PropertyName == SearchFilterPropertyName.PrimaryIdentifier)
                    {
                        filters.Single().PropertyName = SearchFilterPropertyName.ParentPrimaryIdentifier;
                    }

                    providerFundingData = await apiService.SearchProviderFunding(requestObj, fundingStream.UseLatestFundingData());
                    providerFundingData.ProviderFunding =
                        providerFundingData.ProviderFunding.OrderBy(provider => provider.OrganisationName);
                    filters = filtersOrig;
                }
            }

            var dataSchemaVersion = fundingData != null ? fundingData?.Funding?.FirstOrDefault()?.SchemaVersion
                    : providerFundingData?.ProviderFunding?.FirstOrDefault()?.SchemaVersion;
            var dataTemplateVersion = fundingData != null ? fundingData?.Funding?.FirstOrDefault()?.TemplateVersion
                : providerFundingData?.ProviderFunding?.FirstOrDefault()?.TemplateVersion;

            var modelDetails = uiModelJson?.TemplateFileDetails;

            CheckVersionRequirements(
                FundingViewType.ViewData,
                fundingStream.FundingStreamCode,
                modelDetails?.MinSchemaVersion,
                modelDetails?.MaxSchemaVersion,
                dataSchemaVersion,
                modelDetails?.MinTemplateVersion,
                modelDetails?.MaxTemplateVersion,
                dataTemplateVersion,
                modelVersion);

            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(fundingPeriodCode);

            var singleOrganisation = fundingData?.Funding?.GroupBy(funding => GetRelevantGroupingField(funding)).Count() == 1;
            var singleProvider = providerFundingData?.ProviderFunding?.GroupBy(funding => funding.OrganisationUkprn).Count() == 1;
            string filename;

            var returnList = new List<FundingDocumentMeta>();

            foreach (var fileFormat in fileFormats)
            {
                if (singleOrganisation || singleProvider)
                {
                    string organisationName;

                    if (singleOrganisation)
                    {
                        organisationName = fundingData.Funding.First().SearchableGroupName;
                    }
                    else
                    {
                        var providerFunding = providerFundingData.ProviderFunding.First();
                        organisationName = providerFunding.SearchableOrganisationName;

                        var parentIsSameOrganisation = providerFunding.ParentPrimaryIdentifier != null &&
                            (providerFunding.ParentPrimaryIdentifier == providerFunding.OrganisationUkprn
                                || providerFunding.ParentPrimaryIdentifier == providerFunding.OrganisationDfeNumber);

                        // Use the parent orgs name as its generally more UI friendly (e.g. CAMDEN LONDON BOROUGH COUNCIL vs Camden).
                        if (parentIsSameOrganisation && !string.IsNullOrEmpty(providerFunding.ParentName))
                        {
                            organisationName = providerFunding.ParentName;
                        }
                    }

                    filename = FilenameHelper.BuildOutputSpreadsheetFilename(
                        fundingStream.FundingStreamName,
                        fundingPeriodCode,
                        publishedDate,
                        organisationName,
                        fileFormat.ToString());
                }
                else
                {
                    filename = FilenameHelper.BuildOutputSpreadsheetFilename(
                        fundingStream.FundingStreamName,
                        fundingPeriodCode,
                        publishedDate,
                        fileFormat.ToString());
                }

                var result = new FundingDocumentMeta
                {
                    Data = GenerateFundingDocument(
                        fundingData,
                        providerFundingData,
                        uiModel,
                        fundingStream,
                        fundingPeriodCode,
                        yearFrom,
                        yearTo,
                        publishedDate,
                        fileFormat,
                        asOfMonth,
                        asOfYear,
                        showSelectors,
                        showStatementSpecification,
                        showData),
                    PublicationDate = publishedDate,
                    Filename = filename,
                    FileFormat = fileFormat
                };

                returnList.Add(result);
            }

            _loggerService.LogInformation("DEBUG1 Method GenerateFundingDocumentToByteArray() start");

            return returnList;
        }

        private async Task<FundingViewData> GenerateFundingViewData(
            IComponentService componentService,
            string fundingPeriodCode,
            string fundingStreamCode,
            IFundingApiSearchResponseFunding fundingData,
            IFundingApiSearchResponseProviderFunding providerFundingData,
            FundingStream[] allFundingStreams,
            DateTime cutoffDate,
            Publication publication,
            DateTime? previousPublicationDate,
            double? schemaVersion,
            double? templateVersion,
            int? modelVersion,
            FundingViewScope fundingViewScope,
            Dictionary<ComponentType, Defaults> componentDefaults,
            FundingDocument fundingDocument,
            bool isLatestOrFinalFundingForYear,
            bool isCurrentYear,
            SearchFilter[] filters,
            string searchTerm,
            string selectedTab,
            bool bubbleUpException,
            PreviewLayoutModel previewLayoutModel,
            VarianceSelectionOption selectedVarianceOption,
            bool viaChoicePage,
            bool showSelectors,
            bool asStatementSpecification,
            bool showData,
            bool explicitFundingPassed,
            bool explicitProviderFundingPassed)
        {
            try
            {
                var uiModelJson = await GetUiModelJson(
                    fundingStreamCode,
                    fundingPeriodCode,
                    schemaVersion,
                    templateVersion,
                    modelVersion,
                    FundingViewType.ViewData,
                    fundingViewScope,
                    publication,
                    previewLayoutModel);

                var uiModel = !string.IsNullOrEmpty(uiModelJson?.Json)
                    ? JsonConvert.DeserializeObject<UiModel>(uiModelJson?.Json) : null;

                var relevantFundingStreams = new Dictionary<string, FundingStream>
                {
                    { "Primary", allFundingStreams.First(fundingStream => fundingStream.FundingStreamCode == fundingStreamCode) }
                };

                if (uiModel.AdditionalFundingStreams?.Any() == true)
                {
                    foreach (var additionalFundingStreamLoopItem in uiModel.AdditionalFundingStreams)
                    {
                        var additionalFundingStream = allFundingStreams.First(fs => fs.FundingStreamCode == additionalFundingStreamLoopItem.Code);
                        relevantFundingStreams.Add(additionalFundingStreamLoopItem.Id, additionalFundingStream);
                    }
                }

                var apiServices = GetApiServices(relevantFundingStreams);

                var requiresFundingData = NeedsFundingData(uiModel);
                var fetchFundingData = requiresFundingData && (fundingData == null || apiServices.Count > 1);

                var requiresProviderFundingData = NeedsProviderFundingData(uiModel);
                var fetchProviderFundingData = requiresProviderFundingData && (providerFundingData == null || apiServices.Count > 1);

                if (fetchFundingData || fetchProviderFundingData)
                {
                    (fundingData, providerFundingData) = await FetchDataForDatasets(
                        uiModel,
                        fundingData,
                        providerFundingData,
                        explicitFundingPassed,
                        explicitProviderFundingPassed,
                        filters,
                        relevantFundingStreams,
                        apiServices,
                        cutoffDate,
                        fundingPeriodCode);
                }

                var dataSchemaVersion = fundingData != null ? fundingData?.Funding?.FirstOrDefault()?.SchemaVersion
                    : providerFundingData?.ProviderFunding?.FirstOrDefault()?.SchemaVersion;
                var dataTemplateVersion = fundingData != null ? fundingData?.Funding?.FirstOrDefault()?.TemplateVersion
                    : providerFundingData?.ProviderFunding?.FirstOrDefault()?.TemplateVersion;

                var modelDetails = uiModelJson?.TemplateFileDetails;

                CheckVersionRequirements(
                    FundingViewType.ViewData,
                    fundingStreamCode,
                    modelDetails?.MinSchemaVersion,
                    modelDetails?.MaxSchemaVersion,
                    dataSchemaVersion,
                    modelDetails?.MinTemplateVersion,
                    modelDetails?.MaxTemplateVersion,
                    dataTemplateVersion,
                    modelVersion);

                var fundingViewDataGenerator = new FundingViewDataGenerator(
                    uiModel,
                    fundingPeriodCode,
                    fundingData,
                    providerFundingData,
                    publication?.PublishedDate,
                    previousPublicationDate,
                    relevantFundingStreams.First().Value,
                    componentDefaults,
                    fundingDocument,
                    isLatestOrFinalFundingForYear,
                    isCurrentYear,
                    modelVersion,
                    searchTerm,
                    selectedTab,
                    componentService,
                    _componentConfigurationService,
                    IsALoggedInView(fundingViewScope),
                    showSelectors,
                    selectedVarianceOption,
                    viaChoicePage,
                    asStatementSpecification,
                    showData);

                var fundingViewData = fundingViewDataGenerator.Generate();

                if (fundingViewData != null)
                {
                    fundingViewData.PublicationDate = publication?.PublishedDate ?? DateTime.Today;
                    fundingViewData.PublicationUiModelVersion = modelVersion;
                }

                return fundingViewData;
            }
            catch (Exception exception)
            {
                _loggerService?.LogError(exception, exception.Message);

                if (bubbleUpException || _componentFactory == null)
                {
                    throw;
                }

                var componentType = ComponentType.Error_Exception;

                if (exception is FileNotFoundException)
                {
                    componentType = ComponentType.Error_ModelNotFound;
                }
                else if (exception is JsonReaderException)
                {
                    componentType = ComponentType.Error_ModelError;
                }

                return new FundingViewData
                {
                    FundingStreamCode = fundingStreamCode,
                    Components = new List<Component>
                    {
                        _componentFactory.CreateSimple(componentType, exception)
                    }
                };
            }
        }

        private async Task<(IFundingApiSearchResponseFunding, IFundingApiSearchResponseProviderFunding)> FetchDataForDatasets(
            UiModel uiModel,
            IFundingApiSearchResponseFunding fundingData,
            IFundingApiSearchResponseProviderFunding providerFundingData,
            bool explicitFundingPassed,
            bool explicitProviderFundingPassed,
            SearchFilter[] filters,
            Dictionary<string, FundingStream> relevantFundingStreams,
            Dictionary<string, IFundingApiService> apiServices,
            DateTime cutoffDate,
            string fundingPeriodCode)
        {
            var datasetResults =
                new Dictionary<int, (IFundingApiSearchResponseFunding, IFundingApiSearchResponseProviderFunding)>();

            var datasets = BaseViewYourFundingRenderer.GetAllDatasetDefinitions(uiModel.Groups, uiModel.Dataset);

            if (datasets == null)
            {
                datasets = new List<UiModelDataset>
                {
                    new UiModelDataset()
                };
            }

            foreach (var dataset in datasets.OrderBy(d => d.Position))
            {
                var requestObjectsForDataset = GetRequestObjectsForDataset(
                    datasets,
                    dataset,
                    filters,
                    relevantFundingStreams,
                    apiServices,
                    cutoffDate,
                    fundingPeriodCode,
                    datasetResults);

                var isProviderFunding = dataset.DatasetName?.Equals(
                    "providerFunding",
                    StringComparison.InvariantCultureIgnoreCase) == true;

                var loopIdx = 0;

                foreach (var requestObject in requestObjectsForDataset.RequestObjects)
                {
                    var isFirstDataset = (requestObjectsForDataset.DatasetPosition == 0 && loopIdx == 0) || dataset.Position == 1;

                    var alreadyHaveFundingData = !isProviderFunding && (explicitProviderFundingPassed || fundingData != null);
                    var alreadyHaveProviderFundingData = isProviderFunding && (explicitFundingPassed || providerFundingData != null);
                    var dontFetchMoreData = isFirstDataset && (alreadyHaveFundingData || alreadyHaveProviderFundingData);

                    var apiService = requestObjectsForDataset.ApiServices[loopIdx++];

                    if (isProviderFunding)
                    {
                        var additionalFunding = !dontFetchMoreData || providerFundingData == null ?
                            await apiService.SearchProviderFunding(requestObject) : null;

                        if (dontFetchMoreData)
                        {
                            datasetResults.Add(requestObjectsForDataset.DatasetPosition, (null, providerFundingData));
                        }
                        else
                        {
                            if (datasetResults.ContainsKey(requestObjectsForDataset.DatasetPosition))
                            {
                                var existingProviderFundings = datasetResults[requestObjectsForDataset.DatasetPosition].Item2.ProviderFunding.ToList();
                                existingProviderFundings.AddRange(additionalFunding.ProviderFunding);

                                datasetResults[requestObjectsForDataset.DatasetPosition].Item2.ProviderFunding = existingProviderFundings;
                            }
                            else
                            {
                                datasetResults.Add(requestObjectsForDataset.DatasetPosition, (null, additionalFunding));
                            }
                        }

                        if (providerFundingData == null)
                        {
                            providerFundingData = additionalFunding;
                        }
                        else if (additionalFunding != null)
                        {
                            var existingProviderFundings = providerFundingData?.ProviderFunding?.ToList();
                            existingProviderFundings.AddRange(additionalFunding.ProviderFunding);

                            providerFundingData.ProviderFunding = existingProviderFundings;
                        }
                    }
                    else
                    {
                        var additionalFunding = !dontFetchMoreData || fundingData == null ?
                            await apiService.SearchFunding(requestObject) : null;

                        if (dontFetchMoreData)
                        {
                            datasetResults.Add(requestObjectsForDataset.DatasetPosition, (fundingData, null));
                        }
                        else
                        {
                            if (datasetResults.ContainsKey(requestObjectsForDataset.DatasetPosition))
                            {
                                var existingFundings = datasetResults[requestObjectsForDataset.DatasetPosition].Item1.Funding.ToList();
                                existingFundings.AddRange(additionalFunding.Funding);

                                datasetResults[requestObjectsForDataset.DatasetPosition].Item1.Funding = existingFundings;
                            }
                            else
                            {
                                datasetResults.Add(requestObjectsForDataset.DatasetPosition, (additionalFunding, null));
                            }
                        }

                        if (fundingData == null)
                        {
                            fundingData = additionalFunding;
                        }
                        else if (!isFirstDataset)
                        {
                            var existingFundings = fundingData?.Funding?.ToList();
                            existingFundings.AddRange(additionalFunding.Funding);

                            fundingData.Funding = existingFundings;
                        }
                    }
                }
            }

            return (fundingData, providerFundingData);
        }

        private FundingApiRequestObjectCollectionForDataset GetRequestObjectsForDataset(
            List<UiModelDataset> datasets,
            UiModelDataset dataset,
            SearchFilter[] filters,
            Dictionary<string, FundingStream> relevantFundingStreams,
            Dictionary<string, IFundingApiService> apiServices,
            DateTime cutoffDate,
            string fundingPeriodCode,
            Dictionary<int, (IFundingApiSearchResponseFunding, IFundingApiSearchResponseProviderFunding)> datasetResults)
        {
            var datasetPosition = datasets.IndexOf(dataset);
            var filtersToApply = filters?.ToList() ?? new List<SearchFilter>();
            var fundingStreams = new List<FundingStream> { relevantFundingStreams["Primary"] };
            var apiService = new List<IFundingApiService> { apiServices["Primary"] };

            if (dataset.FundingStreamMappings?.Any() == true)
            {
                fundingStreams = relevantFundingStreams
                    .Where(fs => dataset.FundingStreamMappings.Contains(fs.Key))
                    .Select(fs => fs.Value)
                    .ToList();

                apiService = apiServices
                    .Where(ser => dataset.FundingStreamMappings.Contains(ser.Key))
                    .Select(ser => ser.Value)
                    .ToList();
            }

            var isProviderFunding = dataset.DatasetName?.Equals("providerFunding", StringComparison.InvariantCultureIgnoreCase) == true;

            var idPropertyName = isProviderFunding ?
                SearchFilterPropertyName.PrimaryIdentifier : SearchFilterPropertyName.ParentPrimaryIdentifier;
            var alternativeIdPropertyName = isProviderFunding ?
                SearchFilterPropertyName.ParentPrimaryIdentifier : SearchFilterPropertyName.PrimaryIdentifier;

            // If we filtered the provider funding data on Parent Primary ID (e.g. LA code),
            // then we need to filter the funding data on the Primary ID.
            if (filtersToApply.Any(filter => filter.PropertyName == idPropertyName) == true)
            {
                var matchedFilters = filtersToApply.Where(searchFilter =>
                    searchFilter.PropertyName == idPropertyName);

                foreach (var matchedFilter in matchedFilters)
                {
                    matchedFilter.PropertyName = alternativeIdPropertyName;
                }
            }

            if (!string.IsNullOrEmpty(dataset.LimitedTo))
            {
                var limitedToParts = dataset.LimitedTo.Split('_');

                var orderNumber = int.Parse(limitedToParts[0].Replace("Dataset", string.Empty)) - 1;
                var datasetResult = datasetResults[orderNumber];

                var paramName = limitedToParts[2];

                switch (paramName.ToLowerInvariant())
                {
                    case "lacode":
                        var laCode = datasetResult.Item1 != null ?
                            datasetResult.Item1?.Funding?.FirstOrDefault()?.GroupCode
                            : datasetResult.Item2?.ProviderFunding?.FirstOrDefault()?.ParentPrimaryIdentifier;

                        filtersToApply.Add(new SearchFilter
                        {
                            PropertyName = isProviderFunding ?
                                SearchFilterPropertyName.ParentPrimaryIdentifier : SearchFilterPropertyName.PrimaryIdentifier,
                            PropertyValue = laCode
                        });

                        break;
                    case "laname":
                        var laName = datasetResult.Item1 != null ?
                            datasetResult.Item1?.Funding?.FirstOrDefault()?.GroupName
                            : datasetResult.Item2?.ProviderFunding?.FirstOrDefault()?.LocalAuthorityName;

                        filtersToApply.Add(new SearchFilter
                        {
                            PropertyName = SearchFilterPropertyName.GroupName,
                            PropertyValue = laName
                        });

                        break;
                    case "id":
                        var id = datasetResult.Item1 != null ?
                            datasetResult.Item1?.Funding?.FirstOrDefault()?.Id
                            : datasetResult.Item2?.ProviderFunding?.FirstOrDefault()?.ParentId;

                        filtersToApply.Add(new SearchFilter
                        {
                            PropertyName = SearchFilterPropertyName.Id,
                            PropertyValue = id
                        });

                        break;
                    case "groupname":
                        var groupName = datasetResult.Item1 != null ?
                            datasetResult.Item1?.Funding?.FirstOrDefault()?.GroupName
                            : datasetResult.Item2?.ProviderFunding?.FirstOrDefault()?.ParentName;

                        filtersToApply.Add(new SearchFilter
                        {
                            PropertyName = SearchFilterPropertyName.GroupName,
                            PropertyValue = groupName
                        });

                        break;
                }
            }

            var requestObjects = fundingStreams.Select(fundingStream => new FundingApiSearchRequestObject
            {
                SearchTerm = null,
                WaitForIndexBuild = true,
                FundingStreams = new[]
                {
                    new FundingApiSearchFundingStream
                    {
                        FundingStreamCode = fundingStream.FundingStreamCode,
                        BeforeDateTime = cutoffDate,
                        Filters = CustomiseFiltersForType(
                            filtersToApply,
                            fundingStream.FundingStreamCode,
                            GetFundingPeriodCode(fundingPeriodCode, fundingStream)),
                        GroupingType = null,
                        PeriodCodes = new[] { GetFundingPeriodCode(fundingPeriodCode, fundingStream) }
                    }
                }
            });

            return new FundingApiRequestObjectCollectionForDataset
            {
                ApiServices = apiService,
                RequestObjects = requestObjects.ToList(),
                DatasetPosition = datasetPosition
            };
        }

        private SearchFilter[] CustomiseFiltersForType(List<SearchFilter> filters, string fundingStreamCode, string fundingPeriodCode)
        {
            if (filters == null)
            {
                return null;
            }

            var returnList = new List<SearchFilter>();

            foreach (var filter in filters)
            {
                if (filter.PropertyName != SearchFilterPropertyName.Id
                    || string.IsNullOrEmpty(filter.PropertyValue))
                {
                    returnList.Add(filter);
                    continue;
                }

                var parts = filter.PropertyValue.Split('-', 4);
                var value = $"{fundingStreamCode}-{fundingPeriodCode}-{parts[3]}";

                returnList.Add(new SearchFilter
                {
                    PropertyName = filter.PropertyName,
                    PropertyValue = value
                });
            }

            return returnList.ToArray();
        }

        private string GetFundingPeriodCode(string fundingPeriodCode, FundingStream fundingStream)
        {
            var fundingPeriodCodeForStream = fundingPeriodCode;
            var fundingPeriodCodeForStreamParts = fundingPeriodCode.Split('-');
            var fundingPeriodCodeForStreamPrefix = fundingPeriodCodeForStreamParts[0];
            var currentFundingPeriodCodePrefix = fundingStream.CurrentFundingPeriodCode_FromPublications(false)?.Split('-')[0];

            if (!string.IsNullOrWhiteSpace(currentFundingPeriodCodePrefix) &&
                !fundingPeriodCodeForStreamPrefix.Equals(
                    currentFundingPeriodCodePrefix, StringComparison.InvariantCultureIgnoreCase))
            {
                fundingPeriodCodeForStream = $"{currentFundingPeriodCodePrefix}-{fundingPeriodCodeForStreamParts[1]}";
            }

            return fundingPeriodCodeForStream;
        }

        private void CheckVersionRequirements(
            FundingViewType fundingViewType,
            string fundingStreamCode,
            double? minSchema,
            double? maxSchema,
            string schemaVersion,
            double? minTemplate,
            double? maxTemplate,
            string templateVersion,
            int? modelVersion)
        {
            var schemaCheck = minSchema == null || maxSchema == null || string.IsNullOrEmpty(schemaVersion);

            if (!schemaCheck)
            {
                var dblVersion = double.Parse(schemaVersion);

                if (minSchema > dblVersion || dblVersion > maxSchema)
                {
                    throw new Exception($"{fundingViewType} - Template found for fundingStreamCode: {fundingStreamCode} SchemaVersion: {schemaVersion} templateVersion: {templateVersion} modelVersion {modelVersion} does not meet requirements");
                }
            }

            var templateCheck = minTemplate == null || maxTemplate == null || string.IsNullOrEmpty(templateVersion);

            if (!templateCheck)
            {
                var dblVersion = double.Parse(templateVersion);

                if (minTemplate > dblVersion || dblVersion > maxTemplate)
                {
                    throw new Exception($"{fundingViewType} - Template found for fundingStreamCode: {fundingStreamCode} SchemaVersion: {schemaVersion} templateVersion: {templateVersion} modelVersion {modelVersion} does not meet requirements");
                }
            }
        }

        /// <summary>
        /// Do we need to fetch provider funding data.
        /// </summary>
        /// <param name="uiModel">The UI model.</param>
        /// <returns>True if we need to fetch provider funding data.</returns>
        private bool NeedsProviderFundingData(UiModel uiModel)
        {
            if (uiModel.Dataset?.Any(d => "providerFunding".Equals(
                d.DatasetName,
                StringComparison.InvariantCultureIgnoreCase)) == true)
            {
                return true;
            }

            if (uiModel?.Groups != null)
            {
                return uiModel.Groups.Any(NeedsProviderFundingData);
            }

            return false;
        }

        /// <summary>
        /// Do we need to fetch provider funding data.
        /// </summary>
        /// <param name="uiModelGroup">The UI model group.</param>
        /// <returns>True if we need to fetch provider funding data.</returns>
        private bool NeedsProviderFundingData(UiModelGroup uiModelGroup)
        {
            if (uiModelGroup.Dataset?.Any(d => "providerFunding".Equals(
                d.DatasetName,
                StringComparison.InvariantCultureIgnoreCase)) == true)
            {
                return true;
            }

            if (uiModelGroup.Groups?.Any() != true)
            {
                return false;
            }

            return uiModelGroup.Groups.Any(NeedsProviderFundingData);
        }

        /// <summary>
        /// Do we need to fetch funding data.
        /// </summary>
        /// <param name="uiModel">The UI model.</param>
        /// <returns>True if we need to fetch funding data.</returns>
        private bool NeedsFundingData(UiModel uiModel)
        {
            if (uiModel?.Dataset?.Any(d => string.IsNullOrEmpty(d.DatasetName) || "funding".Equals(
                d.DatasetName,
                StringComparison.InvariantCultureIgnoreCase)) == true)
            {
                return true;
            }

            if (uiModel?.Groups != null)
            {
                return uiModel.Groups.Any(NeedsFundingData);
            }

            return false;
        }

        /// <summary>
        /// Do we need to fetch funding data.
        /// </summary>
        /// <param name="uiModelGroup">The UI model group.</param>
        /// <returns>True if we need to fetch funding data.</returns>
        private bool NeedsFundingData(UiModelGroup uiModelGroup)
        {
            if (uiModelGroup.Dataset?.Any(d => string.IsNullOrEmpty(d.DatasetName) || "funding".Equals(
                d.DatasetName,
                StringComparison.InvariantCultureIgnoreCase)) == true)
            {
                return true;
            }

            if (uiModelGroup.Groups?.Any() != true)
            {
                return false;
            }

            return uiModelGroup.Groups.Any(g => NeedsFundingData(g));
        }

        /// <summary>
        /// Get the path to the UI model / template.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. FY-2122).</param>
        /// <param name="schemaVersion">The schema version of the funding.</param>
        /// <param name="templateVersion">The template version of the funding.</param>
        /// <param name="modelVersion">Model version number of spreadsheet or UI.</param>
        /// <param name="fundingViewType">The funding view type (e.g. Spreadsheet).</param>
        /// <param name="fundingViewScope">A funding view scope (e.g. National).</param>
        /// <returns>A file path to look up the UI model.</returns>
        private UiModelJsonResponse GetRelevantTemplateFilePath(
            string fundingStreamCode,
            string fundingPeriodCode,
            double? schemaVersion,
            double? templateVersion,
            int? modelVersion,
            FundingViewType fundingViewType,
            FundingViewScope fundingViewScope)
        {
            var allFilePaths = GetModelFilenames(fundingViewType.ToString(), fundingPeriodCode);
            var matchingFilePaths = new List<TemplateFileDetails>();

            const string FILE_PART_SEPERATOR_SCHEMA_MIN = "schemamin";
            const string FILE_PART_SEPERATOR_TEMPLATE_MIN = "templatemin";
            const string FILE_PART_SEPERATOR_MAX = "max";
            const string FILE_PART_SEPERATOR_MODEL_VERSION = "ModelVersion";
            const int FILE_PARTS_LENGTH_WTIH_MODEL_VERSION = 5;
            const int FILE_PARTS_LENGTH_WTIHOUT_MODEL_VERSION = 4;
            const int FILE_PART_SCHEMA = 1;
            const int FILE_PART_TEMPLATE = 2;
            const int FILE_PART_MODEL = 3;

            foreach (var filePath in allFilePaths)
            {
                try
                {
                    var fileName = Path.GetFileName(filePath);
                    var fileNameParts = fileName.Split('_');
                    var fileNameContainsModelVersion = fileName.Contains(FILE_PART_SEPERATOR_MODEL_VERSION);
                    var fileNamePartsExpectedLength = fileNameContainsModelVersion ? FILE_PARTS_LENGTH_WTIH_MODEL_VERSION : FILE_PARTS_LENGTH_WTIHOUT_MODEL_VERSION;
                    var lastFilePart = fileNamePartsExpectedLength - 1;

                    if (fileNameParts.Length < (fileNamePartsExpectedLength - 1))
                    {
                        throw new Exception($"Filename format is incorrect. Invalid FileName: {filePath}");
                    }

                    var fundingStreamCodeFileNamePart = fileNameParts.First();

                    // Filter by funding stream code matching
                    if (!fundingStreamCodeFileNamePart.Equals(fundingStreamCode, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    var scopeFileNamePart = fileNameParts.Length >= fileNamePartsExpectedLength
                        ? fileNameParts[lastFilePart].Split('.').First() : "National";

                    // Filter by document scope (e.g. National)
                    if (!fundingViewScope.ToString().Equals(scopeFileNamePart, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    var schemaFileNamePart = fileNameParts[FILE_PART_SCHEMA].ToLower();

                    var minSchema = double.Parse(ReplaceHyphenWithPeriod(schemaFileNamePart.Substring(
                        FILE_PART_SEPERATOR_SCHEMA_MIN.Length,
                        schemaFileNamePart.IndexOf(FILE_PART_SEPERATOR_MAX) - FILE_PART_SEPERATOR_SCHEMA_MIN.Length)));

                    var maxSchema = double.Parse(ReplaceHyphenWithPeriod(schemaFileNamePart.Substring(schemaFileNamePart.IndexOf(FILE_PART_SEPERATOR_MAX) + FILE_PART_SEPERATOR_MAX.Length)));

                    // Check if the schema version is between the min and max allowed
                    if (schemaVersion.HasValue && (minSchema > schemaVersion || schemaVersion > maxSchema))
                    {
                        continue;
                    }

                    var templateFileNamePart = fileNameParts[FILE_PART_TEMPLATE].Split('.')[0].ToLower();

                    var minTemplate = double.Parse(ReplaceHyphenWithPeriod(templateFileNamePart.Substring(
                        FILE_PART_SEPERATOR_TEMPLATE_MIN.Length,
                        templateFileNamePart.IndexOf(FILE_PART_SEPERATOR_MAX) - FILE_PART_SEPERATOR_TEMPLATE_MIN.Length)));

                    var maxTemplate = double.Parse(ReplaceHyphenWithPeriod(
                        templateFileNamePart.Substring(templateFileNamePart.IndexOf(FILE_PART_SEPERATOR_MAX) + FILE_PART_SEPERATOR_MAX.Length)));

                    // Check if the template version is between the min and max allowed
                    if (templateVersion.HasValue && (minTemplate > templateVersion || templateVersion > maxTemplate))
                    {
                        continue;
                    }

                    int? fileModelVersion = null;

                    if (fileName.Contains(FILE_PART_SEPERATOR_MODEL_VERSION))
                    {
                        var modelFileNamePart = fileNameParts[FILE_PART_MODEL].Split('.')[0].ToLower();
                        fileModelVersion = int.Parse(modelFileNamePart.Substring(
                            FILE_PART_SEPERATOR_MODEL_VERSION.Length,
                            modelFileNamePart.Length - FILE_PART_SEPERATOR_MODEL_VERSION.Length));

                        // If the files model version doesnt match the requested one, continue through the loop
                        // If no specific version of a model was requested, treat it as if it was version 1 to not break
                        // existing compatibility
                        if (fileModelVersion != (modelVersion ?? 1))
                        {
                            continue;
                        }
                    }

                    matchingFilePaths.Add(new TemplateFileDetails
                    {
                        FilePath = filePath,
                        MinSchemaVersion = minSchema,
                        MaxSchemaVersion = maxSchema,
                        MinTemplateVersion = minTemplate,
                        MaxTemplateVersion = maxTemplate,
                        ModelVersion = fileModelVersion,
                        Scope = scopeFileNamePart
                    });
                }
                catch (Exception ex)
                {
                    _loggerService?.LogError(ex, ex.Message);
                }
            }

            var match = matchingFilePaths
                .OrderByDescending(file => file.MaxTemplateVersion)
                .ThenByDescending(file => file.MaxSchemaVersion)
                .ThenByDescending(file => file.ModelVersion ?? int.MinValue)
                .FirstOrDefault();

            return new UiModelJsonResponse
            {
                TemplateFileDetails = match
            };
        }

        private string[] GetModelFilenames(string fundingViewTypeString, string fundingPeriodCode)
        {
            var allFilePaths = _modelFileStoreService.GetModelFilenames($"{fundingViewTypeString}/{fundingPeriodCode}");
            return allFilePaths ?? _modelFileStoreService.GetModelFilenames(fundingViewTypeString);
        }

        private string ReplaceHyphenWithPeriod(string numberWithHyphen)
        {
            return numberWithHyphen.Replace("-", ".");
        }

        private async Task<string> GetUiModelJsonFromCosmos(
            FundingViewType fundingViewType,
            FundingViewScope fundingViewScope,
            Publication publication,
            PreviewLayoutModel previewLayoutModel)
        {
            if (previewLayoutModel?.IsValidPreviewMode == true &&
                previewLayoutModel.FundingViewScope == fundingViewScope &&
                (publication == null || publication.FundingStreamId == previewLayoutModel.FundingStreamId))
            {
                var dbPreviewLayout = await _layoutManagementService.GetLayoutAsync(previewLayoutModel.LayoutId);

                if (dbPreviewLayout == null)
                {
                    throw new Exception(
                        $"Preview Layout with id '{previewLayoutModel.LayoutId}', used for '{fundingViewType}' '{fundingViewScope}' could not be found");
                }

                return !string.IsNullOrEmpty(dbPreviewLayout.LayoutJsonData) ? dbPreviewLayout.LayoutJsonData : JsonConvert.SerializeObject(dbPreviewLayout.Data);
            }

            var publicationLayout = publication?.PublicationLayouts?.FirstOrDefault(pl => pl.FundingViewType == fundingViewType
                && pl.FundingViewScope == fundingViewScope);
            var publicationLayoutId = publicationLayout?.LayoutId;

            if (string.IsNullOrEmpty(publicationLayoutId))
            {
                _loggerService?.LogError($"Layout with id '{publicationLayoutId}', used for '{fundingViewType}' '{fundingViewScope}' could not be found");
                return null;
            }

            // Retrieve from Cosmos DB
            var dbLayout = await _layoutManagementService.GetLayoutAsync(publicationLayoutId);

            if (dbLayout == null)
            {
                _loggerService?.LogError($"Layout with id '{publicationLayoutId}', used for '{fundingViewType}' '{fundingViewScope}' could not be found");
                return null;
            }

            if (dbLayout.DeletedDateTime != null)
            {
                _loggerService?.LogError($"Layout with id '{publicationLayoutId}', used for '{fundingViewType}' '{fundingViewScope}' has been deleted");
                return null;
            }

            return !string.IsNullOrEmpty(dbLayout.LayoutJsonData) ? dbLayout.LayoutJsonData : JsonConvert.SerializeObject(dbLayout.Data);
        }

        private class UiModelJsonResponse
        {
            public TemplateFileDetails TemplateFileDetails { get; set; }

            public string Json { get; set; }
        }

        private async Task<UiModelJsonResponse> GetUiModelJson(
            string fundingStreamCode,
            string fundingPeriodCode,
            double? schemaVersion,
            double? templateVersion,
            int? modelVersion,
            FundingViewType fundingViewType,
            FundingViewScope fundingViewScope,
            Publication publication,
            PreviewLayoutModel previewLayoutModel)
        {
            var cosmosLayout = await GetUiModelJsonFromCosmos(fundingViewType, fundingViewScope, publication, previewLayoutModel);

            if (!string.IsNullOrEmpty(cosmosLayout))
            {
                return new UiModelJsonResponse
                {
                    Json = cosmosLayout
                };
            }

            // Else fetch from file system as previously
            var uiModelResponse = GetRelevantTemplateFilePath(
                fundingStreamCode,
                fundingPeriodCode,
                schemaVersion,
                templateVersion,
                modelVersion,
                fundingViewType,
                fundingViewScope);

            if (string.IsNullOrEmpty(uiModelResponse?.TemplateFileDetails?.FilePath))
            {
                throw new FileNotFoundException($"{fundingViewType.ToString()} - No template found for fundingStreamCode: {fundingStreamCode} SchemaVersion: {schemaVersion}"
                    + $" templateVersion: {templateVersion} modelVersion {modelVersion} fundingViewScope: {fundingViewScope}");
            }

            if (!_modelFileStoreService.Exists(uiModelResponse?.TemplateFileDetails?.FilePath))
            {
                throw new FileNotFoundException($"Model not found for fundingStreamCode: {fundingStreamCode} schemaVersion: {schemaVersion} templateVersion: {templateVersion}"
                    + $" modelVersion: {modelVersion} fundingViewType: {fundingViewType.ToString()} fundingViewScope: {fundingViewScope}");
            }

            var response = _modelFileStoreService.ReadFileAsString(uiModelResponse?.TemplateFileDetails?.FilePath);
            uiModelResponse.Json = response;

            return uiModelResponse;
        }

        /// <summary>
        /// Get the maximum template schema version for specified FundingViewType (e.g. UI or spreadsheet).
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. FY-2122).</param>
        /// <param name="fundingViewType">The funding view type (e.g. Spreadsheet).</param>
        /// <returns>A string that will hold min and max version numbers.</returns>
        private int? GetMaximumTemplateSchemaVersion(string fundingStreamCode, string fundingPeriodCode, FundingViewType fundingViewType)
        {
            var allFilePaths = GetModelFilenames(fundingViewType.ToString(), fundingPeriodCode);
            int? maxVersion = null;

            foreach (var filePath in allFilePaths)
            {
                try
                {
                    var fileNameParts = Path.GetFileName(filePath).Split('_');

                    if (fileNameParts.Length < 4)
                    {
                        continue;
                    }

                    var fundingStreamCodeFileNamePart = fileNameParts.First();

                    // Filter by funding stream code matching
                    if (!fundingStreamCodeFileNamePart.Equals(fundingStreamCode, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    if (fileNameParts.Length < 5)
                    {
                        continue;
                    }

                    var modelFileNamePart = fileNameParts[3].Split('.')[0].ToLower();

                    var fileModelVersion = int.Parse(modelFileNamePart.Substring("ModelVersion".Length, modelFileNamePart.Length - "ModelVersion".Length));

                    if (!maxVersion.HasValue || fileModelVersion > maxVersion.Value)
                    {
                        maxVersion = fileModelVersion;
                    }
                }
                catch (Exception ex)
                {
                    _loggerService?.LogError(ex, string.Empty);
                }
            }

            return maxVersion;
        }

        #endregion Private helper methods
    }
}