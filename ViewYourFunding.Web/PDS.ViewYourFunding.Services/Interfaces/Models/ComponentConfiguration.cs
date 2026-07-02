using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Interfaces.Models
{
    /// <summary>
    /// Gets or sets the component configuration.
    /// </summary>
    public class ComponentConfiguration
    {
        private CalculationAndFundingLinesCollection _calculationAndFundingLines;
        private IFundingApiSearch _dataJson;
        private FundingValueNested_1_0 _fundingValue_1_0;
        private FundingValueNested_1_1 _fundingValue_1_1;
        private FundingValueNested_1_2 _fundingValue_1_2;
        private JObject _fundingValueJObject;
        private Dictionary<string, object> _constantDefinedVariables;
        private bool showSelectors;

        /// <summary>
        /// Gets or sets the base path of the service.
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// Gets or sets the url for logged in view.
        /// </summary>
        public string UrlForLoggedInView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we can use the abbreviated name.
        /// </summary>
        public bool ShowNameAbbreviation { get; set; }

        /// <summary>
        /// Gets or sets the primary identfier (e.g. an LACode for an LA).
        /// </summary>
        public string PrimaryIdentifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we are dealing with a local authority.
        /// </summary>
        public bool IsLocalAuthority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is an indicative funding.
        /// </summary>
        public bool IsIndicativeFunding { get; set; }

        /// <summary>
        /// Gets or sets the previous year.
        /// </summary>
        public int Year0 { get; set; }

        /// <summary>
        /// Gets or sets the first year.
        /// </summary>
        public int Year1 { get; set; }

        /// <summary>
        /// Gets or sets the second year.
        /// </summary>
        public int Year2 { get; set; }

        /// <summary>
        /// Gets or sets the funding stream code (e.g. DSG).
        /// </summary>
        public string FundingStreamCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream code (e.g. DSG) or name if its not relevant.
        /// </summary>
        public string FundingStreamCodeOrName { get; set; }

        /// <summary>
        /// Gets or sets the funding stream name (e.g. PE and sport premium).
        /// </summary>
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets the full name of this funding stream as used in a sentence (which may mean it it lowercased), e.g. 'PE and Sport or dedicated schools grant'.
        /// </summary>
        public string FundingStreamNameWithinSentence { get; set; }

        /// <summary>
        /// Gets or sets the short funding stream html name (e.g. <abbr>DSG</abbr>).
        /// </summary>
        public string ShortFundingStreamNameHtml { get; set; }

        /// <summary>
        /// Gets or sets the long funding stream html name (e.g. Dedicated schools grant (<abbr>DSG</abbr>)).
        /// </summary>
        public string ExpandedFundingStreamNameHtml { get; set; }

        /// <summary>
        /// Gets or sets the funding period code (e.g. AY-1920).
        /// </summary>
        public string FundingPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the year type (e.g. Academic).
        /// </summary>
        public string YearType { get; set; }

        /// <summary>
        /// Gets or sets the year type code (e.g. AY).
        /// </summary>
        public string YearTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the funding stream path part (e.g. dedicated-schools-grant).
        /// </summary>
        public string FundingStreamPathPart { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we originally entered the site via the allocation type page (pre or post 16).
        /// </summary>
        public bool ViaChoicePage { get; set; }

        /// <summary>
        /// Gets or sets the published date.
        /// </summary>
        public DateTime PublishedDate { get; set; }

        /// <summary>
        /// Gets or sets the variance selection option.
        /// </summary>
        public VarianceSelectionOption VarianceSelectionOption { get; set; }

        /// <summary>
        /// Gets or sets the variance display message.
        /// </summary>
        public string VarianceMessage { get; set; }

        /// <summary>
        /// Gets or sets the publication ui model version (integer or null).
        /// </summary>
        public int? PublicationUiModelVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the organisation is closed or not.
        /// </summary>
        public bool OrganisationClosed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the next payment date.
        /// </summary>
        public DateTime? NextPaymentDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the text to use if there is no next payment for the year.
        /// </summary>
        public string NoNextPaymentForTheYearText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the text to use if there is no next payment for the organisation.
        /// </summary>
        public string NoNextPaymentForOrganisationText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the funding document.
        /// </summary>
        public FundingDocument FundingDocument { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether the funding is the final or latest year.
        /// </summary>
        public bool IsLatestOrFinalFundingForYear { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the date relates to the current year.
        /// </summary>
        public bool IsCurrentYear { get; set; }

        /// <summary>
        /// Gets or sets the variables.
        /// </summary>
        public Dictionary<string, object> Variables { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets a value indicating the first tab.
        /// </summary>
        public string InitialTab { get; set; }

        /// <summary>
        /// Gets or sets a value containing the search term.
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets a value containing the 'as of' year.
        /// </summary>
        public string AsOfYear { get; set; }

        /// <summary>
        /// Gets or sets a value containing the 'as of' month.
        /// </summary>
        public string AsOfMonth { get; set; }

        /// <summary>
        /// Gets or sets a value containing the local authority name.
        /// </summary>
        public string LocalAuthorityName { get; set; }

        /// <summary>
        /// Gets or sets a value containing the local authority name override for incorrect parent name.
        /// </summary>
        public string LocalAuthorityNameOverride { get; set; }

        /// <summary>
        /// Gets or sets a value containing the local authority name.
        /// </summary>
        public string ProviderLocalAuthorityName { get; set; }

        /// <summary>
        /// Gets or sets a value containing the local authority code.
        /// </summary>
        public string LocalAuthorityCode { get; set; }

        /// <summary>
        /// Gets or sets a value containing the provider name.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets a value containing the provider establishment number.
        /// </summary>
        public string ProviderEstablishmentNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the view is for logged in users only.
        /// </summary>
        public bool IsALoggedInView { get; set; }

        /// <summary>
        /// Gets or sets a value containing the data as a JSON object.
        /// </summary>
        public IFundingApiSearch Data
        {
            get
            {
                return _dataJson;
            }

            set
            {
                _dataJson = value;
                _calculationAndFundingLines = null;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the institution contains primary age pupils.
        /// </summary>
        public bool ProviderIsPrimaryInstitution { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the institution contains secondary age pupils.
        /// </summary>
        public bool ProviderIsSecondaryInstitution { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the institution contains both primary and secondary age pupils (and potentially from nusery age [3] up to 6th form [19]).
        /// </summary>
        public bool ProviderIsAllThroughInstitution { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is an in year opener.
        /// </summary>
        /// <value>
        /// True if it is an in year opener.
        /// </value>
        public bool InYearOpener { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a current year opener.
        /// </summary>
        /// <value>
        /// True if it is a current year opener.
        /// </value>
        public bool IsCurrentYearOpener { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is an academy converter or new provision in year opener.
        /// Opened between 31st August Year 1 and 31st July Year 2.
        /// </summary>
        /// <value>
        /// True if it is an academy converter or new provision in year opener.
        /// </value>
        public bool IsAcademyConverterOrNewProvisionInYearOpener { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a post april in year opener.
        /// </summary>
        /// <value>
        /// True if it is a post april in year opener.
        /// </value>
        public bool IsPostAprilOpener { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the day the institution was opened.
        /// </summary>
        public int? OpeningDay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the month the institution was opened.
        /// </summary>
        public int? OpeningMonth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the year the institution was opened.
        /// </summary>
        public int? OpeningYear { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a mainstream academy.
        /// </summary>
        /// <value>
        /// True if it is a mainstream academy.
        /// </value>
        public bool IsMainstreamAcademy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a sponsored mainstream academy.
        /// </summary>
        /// <value>
        /// True if it is a sponsored mainstream academy.
        /// </value>
        public bool IsMainstreamAcademySponsored { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a mainstream free school.
        /// </summary>
        /// <value>
        /// True if it is a mainstream free school.
        /// </value>
        public bool IsMainstreamFreeSchool { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a special academy.
        /// </summary>
        /// <value>
        /// True if it is a special academy.
        /// </value>
        public bool IsSpecialAcademy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a special free school.
        /// </summary>
        /// <value>
        /// True if it is a special free school.
        /// </value>
        public bool IsSpecialFreeSchool { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is an academy provider type.
        /// </summary>
        /// <value>
        /// True if it is an academy provider type.
        /// </value>
        public bool IsAcademyProviderType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a school provider type.
        /// </summary>
        /// <value>
        /// True if it is a school provider type.
        /// </value>
        public bool IsSchoolProviderType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a local authority provider type.
        /// </summary>
        /// <value>
        /// True if it is a local authority provider type.
        /// </value>
        public bool IsLocalAuthorityProviderType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a further education provider type.
        /// </summary>
        /// <value>
        /// True if it is a further education provider type.
        /// </value>
        public bool IsFurtherEducationProviderType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a special post 16 provider type.
        /// </summary>
        /// <value>
        /// True if it is a special post 16 provider type.
        /// </value>
        public bool IsSpecialPost16ProviderType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a non programme funded provider type.
        /// </summary>
        /// <value>
        /// True if it is a non programme funded provider type.
        /// </value>
        public bool IsNonProgrammeFundedProviderType { get; set; }

        /// <summary>
        /// Gets or sets a value showing the open date of the provider.
        /// </summary>
        public DateTime? ProviderOpenDate { get; set; }

        /// <summary>
        /// Gets or sets a value showing the open date of the provider, as a formatted string.
        /// </summary>
        public string ProviderOpenDateddMMMMyyyy { get; set; }

        /// <summary>
        /// Gets or sets a value showing the open days start date of the provider.
        /// </summary>
        public DateTime? ProviderOpenDaysStartDate { get; set; }

        /// <summary>
        /// Gets or sets the provider upin.
        /// </summary>
        public string ProviderUpin { get; set; }

        /// <summary>
        /// Gets or sets the provider URN.
        /// </summary>
        public string ProviderUrn { get; set; }

        /// <summary>
        /// Gets or sets the provider type.
        /// </summary>
        public string ProviderType { get; set; }

        /// <summary>
        /// Gets or sets the provider sub type.
        /// </summary>
        public string ProviderSubType { get; set; }

        /// <summary>
        /// Gets or sets the region name.
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// Gets or sets the original model.
        /// </summary>
        public UiModel OriginalModel { get; set; }

        /// <summary>
        /// Gets or sets the number of days in the current year.
        /// </summary>
        public int DaysInYear { get; set; }

        /// <summary>
        /// Gets or sets the first dataset's data.
        /// </summary>
        public List<List<IFundingApiSearch>> AllDatasetsData { get; set; }

        /// <summary>
        /// Gets or sets the date and time this allocation was published.
        /// </summary>
        public DateTime? StatusChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the date the allocation was published, as a UI formatted string.
        /// </summary>
        public string StatusChangedDateUiFormatted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property that determines 'statement spec' mode is set.
        /// </summary>
        public bool ShowStatementSpecification { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property that determines whether data is drawn.
        /// </summary>
        public bool ShowData { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the output is of HTML format.
        /// </summary>
        public bool IsHtml { get; set; } = true;

        /// <summary>
        /// Gets or sets the schema version.
        /// </summary>
        public double SchemaVersion { get; set; }

        /// <summary>
        /// Gets or sets the entity group ukprn.
        /// </summary>
        public string EntityGroupUkprn { get; set; }

        /// <summary>
        ///  Gets or sets the constant defined variables.
        /// </summary>
        public Dictionary<string, object> ConstantDefinedVariables
        {
            get
            {
                if (_constantDefinedVariables == null)
                {
                    _constantDefinedVariables = BaseViewYourFundingRenderer.GetConstantDefinedVariables(this);
                }

                return _constantDefinedVariables;
            }

            set
            {
                _constantDefinedVariables = value;
            }
        }

        /// <summary>
        /// Gets or sets the calculation and funding lines.
        /// </summary>
        public CalculationAndFundingLinesCollection CalculationAndFundingLines
        {
            get
            {
                return _calculationAndFundingLines ??=
                    ComponentHelper.ConvertJObjectToCalculationsAndFundingLinesCollection(
                    GetFundingValueTemplate());
            }

            set => _calculationAndFundingLines = value;
        }

        /// <summary>
        /// Gets the funding distribution periods. Work Around.
        /// </summary>
        /// <value>
        /// The funding distribution periods.
        /// </value>
        public IDictionary<string, double> FundingDistributionPeriods
        {
            get
            {
                var fundingDistributionPeriods = new Dictionary<string, double>();
                var paymentFundingLinesWithDistributionPeriods =
                    CalculationAndFundingLines.FundingLines
                    .Where(fundingLine =>
                        fundingLine.Value.IsPaymentType && fundingLine.Value.DistributionPeriods?.Any() == true);

                var distributionPeriodGroups =
                    paymentFundingLinesWithDistributionPeriods.SelectMany(fundingLine => fundingLine.Value.DistributionPeriods).ToList()
                    .GroupBy(distributionPeriod => new { distributionPeriod.DistributionPeriodId });

                foreach (var group in distributionPeriodGroups)
                {
                    var groupTotal = group.Sum(distributionPeriod => distributionPeriod.Value);

                    fundingDistributionPeriods.Add(
                        group.Key.DistributionPeriodId,
                        groupTotal);
                }

                return fundingDistributionPeriods;
            }
        }

        /// <summary>
        /// Gets the funding value Data.
        /// </summary>
        public FundingValueNested_1_0 FundingValue_1_0
        {
            get
            {
                if (_fundingValue_1_0 == null)
                {
                    if (Data == null ||
                        string.IsNullOrEmpty(Data.FundingValue) ||
                        Data.FundingValue.Equals(DataValueConstants.NullObjectValue, StringComparison.InvariantCulture))
                    {
                        _fundingValue_1_0 = new FundingValueNested_1_0();
                    }
                    else
                    {
                        _fundingValue_1_0 = JsonConvert.DeserializeObject<FundingValueNested_1_0>(Data.FundingValue);
                    }

                    if (Data != null)
                    {
                        _fundingValue_1_0.TotalValue = Data.TotalAmount;
                    }
                }

                return _fundingValue_1_0;
            }
        }

        /// <summary>
        /// Gets the funding value Data.
        /// </summary>
        public FundingValueNested_1_1 FundingValue_1_1
        {
            get
            {
                if (_fundingValue_1_1 == null)
                {
                    if (Data == null ||
                        string.IsNullOrEmpty(Data.FundingValue) ||
                        Data.FundingValue.Equals(DataValueConstants.NullObjectValue, StringComparison.InvariantCulture))
                    {
                        _fundingValue_1_1 = new FundingValueNested_1_1();
                    }
                    else
                    {
                        _fundingValue_1_1 = JsonConvert.DeserializeObject<FundingValueNested_1_1>(Data.FundingValue);
                    }

                    if (Data != null)
                    {
                        _fundingValue_1_1.TotalValue = Data.TotalAmount;
                    }
                }

                return _fundingValue_1_1;
            }
        }

        /// <summary>
        /// Gets the funding value Data.
        /// </summary>
        public FundingValueNested_1_2 FundingValue_1_2
        {
            get
            {
                if (_fundingValue_1_2 == null)
                {
                    if (Data == null ||
                        string.IsNullOrEmpty(Data.FundingValue) ||
                        Data.FundingValue.Equals(DataValueConstants.NullObjectValue, StringComparison.InvariantCulture))
                    {
                        _fundingValue_1_2 = new FundingValueNested_1_2();
                    }
                    else
                    {
                        _fundingValue_1_2 = JsonConvert.DeserializeObject<FundingValueNested_1_2>(Data.FundingValue);
                    }

                    if (Data != null)
                    {
                        _fundingValue_1_2.TotalValue = Data.TotalAmount;
                    }
                }

                return _fundingValue_1_2;
            }
        }


        /// <summary>
        /// Gets the funding value Data.
        /// </summary>
        public JObject FundingValueJObject
        {
            get
            {
                if (_fundingValueJObject == null && !string.IsNullOrEmpty(Data?.FundingValue) && Data.FundingValue != DataValueConstants.NullObjectValue)
                {
                    _fundingValueJObject = JObject.Parse(Data.FundingValue);
                }

                return _fundingValueJObject;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether selectors should be shown (used for debugging).
        /// </summary>
        public bool ShowSelectors
        {
            get
            {
                return showSelectors || ShowStatementSpecification;
            }

            set
            {
                showSelectors = value;
            }
        }

        /// <summary>
        /// Gets or sets the the context configuration cache.
        /// </summary>
        public Dictionary<string, ComponentConfiguration> ContextConfigurationCache { get; set; } = new Dictionary<string, ComponentConfiguration>();

        /// <summary>
        /// Gets or sets the URL for logged in local authority view.
        /// </summary>
        /// <value>
        /// The URL for logged in local authority view.
        /// </value>
        public string UrlForLoggedInLocalAuthorityView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the IsSecondYearInYearOpener configuration value.
        /// </summary>
        public bool IsSecondYearInYearOpener { get; set; }

        private IFundingValueNested GetFundingValueTemplate()
        {
            return SchemaVersion switch
            {
                1.2 => (IFundingValueNested)FundingValue_1_2,
                1.1 => FundingValue_1_1,
                _ => FundingValue_1_0
            };
        }
    }
}