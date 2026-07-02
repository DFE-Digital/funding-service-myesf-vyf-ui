using Newtonsoft.Json;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Implementations.FundingView
{
    /// <summary>
    /// All the data required to represent a spreadsheet.
    /// </summary>
    public class Spreadsheet : ISpreadsheet
    {
        #region Internal properties

        /// <summary>
        /// Gets or sets the global setting service.
        /// </summary>
        internal IGlobalSettingService GlobalSettingService { get; set; }

        /// <summary>
        /// Gets the service used to retrieve resources.
        /// </summary>
        internal IModelFileStoreService ModelFileStoreService { get; private set; }

        /// <summary>
        /// Gets or sets the Component Configuration Service.
        /// </summary>
        internal IComponentConfigurationService ComponentConfigurationService { get; set; }

        /// <summary>
        /// Gets the funding period code (e.g. DSG).
        /// </summary>
        internal string FundingPeriodCode { get; private set; }

        /// <summary>
        /// Gets the period start year (e.g. 2019).
        /// </summary>
        internal int PeriodStartYear { get; private set; }

        /// <summary>
        /// Gets the period end year  (e.g. 2020).
        /// </summary>
        internal int PeriodEndYear { get; private set; }

        /// <summary>
        /// Gets the data to plug into the spreadsheet.
        /// </summary>
        internal IFundingApiSearchResponseFunding FundingData { get; private set; }

        /// <summary>
        /// Gets the funding data to plug into the spreadsheet.
        /// </summary>
        internal IFundingApiSearchResponseProviderFunding ProviderFundingData { get; private set; }

        /// <summary>
        /// Gets the funding stream for the data on the spreadsheet.
        /// </summary>
        internal FundingStream FundingStream { get; private set; }

        /// <summary>
        /// Gets the UI model/template used to generate the spreadsheet.
        /// </summary>
        internal UiModel UiModel { get; private set; }

        /// <summary>
        /// Gets the publication date to give on the spreadsheet.
        /// </summary>
        internal DateTime PublicationDate { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show selectors.
        /// </summary>
        internal bool ShowSelectors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property that determines 'statement spec' mode is set.
        /// </summary>
        internal bool ShowStatementSpecification { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property that determines whether data is drawn.
        /// </summary>
        internal bool ShowData { get; set; } = true;

        #endregion Internal properties

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// Create an instance to hold all the data needed to create a spreadsheet.
        /// </summary>
        /// <param name="globalSettingService"> The service used to retrieve global settings.</param>
        /// <param name="modelFileStoreService"> The service used to retrieve resources.</param>
        /// <param name="componentConfigurationService">The Component Configuration Service.</param>
        /// <param name="uiModel">The UI model/template used to generate the spreadsheet.</param>
        /// <param name="fundingData">The data to plug into the spreadsheet.</param>
        /// <param name="providerFundingData">The provider funding data plug into the spreadsheet.</param>
        /// <param name="fundingStream">The funding stream config.</param>
        /// <param name="fundingPeriodCode">A funding period code (e.g. DSG)..</param>
        /// <param name="periodStartYear">The period start year (e.g. 2019).</param>
        /// <param name="periodEndYear">The period end year  (e.g. 2020).</param>
        /// <param name="publicationDate">The publication date to give on the spreadsheet.</param>
        /// <param name="asOfMonth">'As of' month (e.g. Apr).</param>
        /// <param name="asOfYear">'As of' year (e.g. 2010).</param>
        /// <param name="showSelectors">Whether to show selectors or not.</param>
        /// <param name="showStatementSpecification">Whether to show statement specification or not.</param>
        /// <param name="showData">Whether to show data or not.</param>
        public Spreadsheet(
            IGlobalSettingService globalSettingService,
            IModelFileStoreService modelFileStoreService,
            IComponentConfigurationService componentConfigurationService,
            UiModel uiModel,
            IFundingApiSearchResponseFunding fundingData,
            IFundingApiSearchResponseProviderFunding providerFundingData,
            FundingStream fundingStream,
            string fundingPeriodCode,
            int periodStartYear,
            int periodEndYear,
            DateTime publicationDate,
            string asOfMonth,
            string asOfYear,
            bool showSelectors,
            bool showStatementSpecification,
            bool showData)
        {
            GlobalSettingService = globalSettingService;
            ModelFileStoreService = modelFileStoreService;
            ComponentConfigurationService = componentConfigurationService;
            FundingPeriodCode = fundingPeriodCode;
            PeriodStartYear = periodStartYear;
            PeriodEndYear = periodEndYear;
            FundingData = fundingData;
            ProviderFundingData = providerFundingData;
            FundingStream = fundingStream;
            UiModel = uiModel;
            PublicationDate = publicationDate;

            Worksheets = new Dictionary<string, IWorksheet>();
            Images = new Dictionary<string, byte[]>();
            AsOfMonth = asOfMonth;
            AsOfYear = asOfYear;

            ShowSelectors = showSelectors;
            ShowStatementSpecification = showStatementSpecification;
            ShowData = showData;
        }

        #region Public properties

        /// <summary>
        /// Gets or sets worksheets inside this spreadsheet.
        /// </summary>
        [JsonConverter(typeof(ConcreteTypeConverter<Dictionary<string, Worksheet>>))]
        public Dictionary<string, IWorksheet> Worksheets { get; set; }

        /// <summary>
        /// Gets or sets image resources used in this spreadsheet.
        /// </summary>
        public Dictionary<string, byte[]> Images { get; set; }

        /// <summary>
        /// Gets or sets 'As of' month (e.g. Apr).
        /// </summary>
        public string AsOfMonth { get; set; }

        /// <summary>
        /// Gets or sets 'As of' year (e.g. 2010).
        /// </summary>
        public string AsOfYear { get; set; }

        #endregion Public properties

        /// <summary>
        /// Build a spreadsheet.
        /// </summary>
        public void Build()
        {
            if (UiModel.Groups != null)
            {
                foreach (var sheetGroup in UiModel.Groups)
                {
                    AddWorksheet(sheetGroup);
                }
            }
        }

        /// <summary>
        /// Add a worksheet to the spreadsheet.
        /// </summary>
        /// <param name="sheetGroup">Definition of how to draw the worksheet.</param>
        private void AddWorksheet(UiModelGroup sheetGroup)
        {
            var worksheet = new Worksheet(this, sheetGroup, PublicationDate, ComponentConfigurationService, GlobalSettingService);
            var hasData = worksheet.Draw();

            if (!sheetGroup.OnlyShowIfData || hasData)
            {
                Worksheets.Add(worksheet.Title, worksheet);
            }
        }
    }
}