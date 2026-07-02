using FluentAssertions;
using Moq;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Attributes;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using PDS.VYF.Services.Abstracts.InfraServices.FilesServices;
using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
using PDS.VYF.Services.Implementations.AppServices;
using PDS.VYF.Services.Implementations.InfraServices.FilesServices;
using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;

namespace PDS.VYF.Services.Tests.Implementations.AppServices
{
    /// <summary>
    /// The shared funding view services tests.
    /// </summary>
    [TestClass, TestCategory("Unit")]
    public class SharedFundingViewServicesTests
    {
        private MockRepository mockRepository;

        private Mock<IComponentService> mockComponentService;
        private Mock<IComponentConfigurationService> mockComponentConfigurationService;
        private Mock<IUIModelFilesServices> mockUIModelFilesServices;
        private Mock<ICacheService> mockCacheService;
        private Mock<IGlobalSettingsService> mockGlobalSettingsService;
        private Mock<IModelFileStoreService> mockModelFileStoreService;
        private Mock<ILoggerAdapter<ModelFundingViewService>> mockLoggerAdapter;

        /// <summary>
        /// The test model group identifier.
        /// </summary>
        private const string TestModelGroupId = "test-funding-amount";

        /// <summary>
        /// The test json property name.
        /// </summary>
        private const string TestJsonPropertyName = "fundingAmount";

        /// <summary>
        /// The test funding amount.
        /// </summary>
        private const decimal TestFundingAmount = 1234.56m;

        /// <summary>
        /// The test funding value.
        /// </summary>
        private static readonly string TestFundingValue =
            "{ \"" + TestJsonPropertyName + "\": " + TestFundingAmount.ToString() + " }";

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedFundingViewServicesTests"/> class.
        /// </summary>
        public SharedFundingViewServicesTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockComponentService = this.mockRepository.Create<IComponentService>();
            this.mockComponentConfigurationService = this.mockRepository.Create<IComponentConfigurationService>();
            this.mockUIModelFilesServices = this.mockRepository.Create<IUIModelFilesServices>();
            this.mockCacheService = this.mockRepository.Create<ICacheService>();
            this.mockGlobalSettingsService = this.mockRepository.Create<IGlobalSettingsService>();
            this.mockModelFileStoreService = this.mockRepository.Create<IModelFileStoreService>();
            this.mockLoggerAdapter = this.mockRepository.Create<ILoggerAdapter<ModelFundingViewService>>();
        }

        /// <summary>
        /// Gets the funding view data state under test expected behavior.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetFundingViewData_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var sharedFundingViewServices = this.CreateSharedFundingViewServices();

            var config = new FundingStream
            {
                FundingStreamCode = "GAG",
                FundingStreamName = "General annual grant",
            };

            var data = new ProviderFundingApiSearchResponse
            {
                ProviderFunding = new List<FundingApiSearchProviderFunding>
                {
                    new FundingApiSearchProviderFunding
                    {
                        OrganisationName = "Dummy Child name",
                        TotalAmount = Convert.ToDouble(12345678),
                        StatusChangedDate = new DateTime(2030, 01, 01),
                        FundingValue = TestFundingValue
                    }
                }
            };

            var viewDataRequest = new ChildSummaryViewDataRequestModel()
            {
                UkprnFromLoggedInUser = "12340000",
                UserId = "ravimekala4074495ravi.mekala@education.gov.uk",
                FundingStreamCode = "GAG",
                FundingPeriodCode = "AC-2425",
                SchemaVersion = "1.2",
                TemplateVersion = "2.0",
                FundingViewType = FundingViewType.ViewData,
                FundingViewScope = FundingViewScope.LoggedInProviderSummary,
                PublicationDate = new DateTime(2030, 01, 01),
                IsLatestOrFinalFundingForYear = true,
                IsCurrentYear = true,
                SelectedTab = string.Empty,
                IsALoggedInView = true,
                SelectedVarianceOption = VarianceSelectionOption.NoComparison,
                FundingStreamConfig = config,
                ProviderFundingData = data
            };

            mockModelFileStoreService.Setup(mfss => mfss.GetModelFilenames(It.IsAny<string>()))
                .Returns(new[]
                {
                     Environment.CurrentDirectory + @"/Views/FundingUIModels/ViewData/GAG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProviderSummary.json"
                });

            var uIModelFilesServices = new UIModelFilesServices(
                this.mockLoggerAdapter.Object,
                this.mockModelFileStoreService.Object,
                this.mockCacheService.Object);

            var uiModelFileResponseInternal = uIModelFilesServices.GetUiModelInternal(viewDataRequest);

            mockUIModelFilesServices.Setup(s => s.GetUiModel(It.IsAny<ChildSummaryViewDataRequestModel>())).ReturnsAsync(uiModelFileResponseInternal.Result);

            mockGlobalSettingsService.Setup(s => s.GetValueAsBool(GlobalSettingTypeConstants.ShowDataTypeId)).ReturnsAsync(true);

            mockGlobalSettingsService.Setup(s => s.GetValueAsBool(GlobalSettingTypeConstants.DisplaySelectorsTypeId)).ReturnsAsync(false);

            mockGlobalSettingsService.Setup(s => s.GetValueAsBool(GlobalSettingTypeConstants.DisplayStatementSpecificationTypeId)).ReturnsAsync(false);

            mockComponentConfigurationService.Setup(s => s.GetFundingProperties(It.IsAny<FundingApiSearchProviderFunding>(), It.IsAny<UiModelDataset>())).Returns(It.IsAny<FundingProperties>());

            var expectedFundingData = new FundingApiSearchFunding
            {
                TotalAmount = 12345678,
                FundingValue = @"{ ""fundingAmount"": 1234.56 }"
            };

            var expectedMeta = new ComponentConfiguration
            {
                AllDatasetsData = new List<List<IFundingApiSearch>>
                {
                    new List<IFundingApiSearch>
                    {
                        expectedFundingData
                    }
                },
                ProviderType = "Academies",
                ProviderSubType = "Academy sponsor led",
                BasePath = "Base Path",
                UrlForLoggedInView = "Url For Logged In Provider Path",
                FundingStreamName = "General annual grant",
                FundingStreamNameWithinSentence = "deneral annual grant",
                FundingPeriodCode = "AC-2425",
                FundingStreamCode = "GAG",
                OpeningDay = 1,
                OpeningMonth = 1,
                OpeningYear = 1,
                Year1 = 2024,
                Year2 = 2025,
                ShortFundingStreamNameHtml = "<abbr title=\"General annual grant\">DSG</abbr>",
                ExpandedFundingStreamNameHtml = "General annual grant (<abbr title=\"General annual grant\">GAG</abbr>)",
                FundingStreamPathPart = "general annual grant",
                IsCurrentYear = true,
                YearType = "financial year",
                IsLatestOrFinalFundingForYear = true,
                NoNextPaymentForOrganisationText = "There are no more scheduled payments for this organisation.",
                NoNextPaymentForTheYearText = "There are no more scheduled payments for financial year 2024 to 2025.",
                PublishedDate = new DateTime(2030, 1, 1),
                ShowNameAbbreviation = true,
                LocalAuthorityName = "TESTLA",
                LocalAuthorityNameOverride = "TESTLA",
                FundingStreamCodeOrName = "GAG",
                AsOfMonth = "Sept",
                AsOfYear = "2024",
                ProviderName = "TESTLA",
                IsLocalAuthority = true,
                Data = expectedFundingData,
                Variables = new Dictionary<string, object>
                {
                    { "censusYear", 2024 },
                    { "R06Year1", 2024 },
                    { "R06Year2", 2025 }
                },
                ProviderOpenDate = DateTime.MinValue,
                Year0 = 2024,
                OriginalModel = new UiModel
                {
                    Dataset = new List<UiModelDataset>
                    {
                        new UiModelDataset
                        {
                            DatasetName = "funding"
                        }
                    },
                    Groups = new List<UiModelGroup>
                    {
                        new UiModelGroup
                        {
                            Id = "test-funding-amount",
                            Selector = "$..fundingAmount"
                        }
                    }
                },
                DaysInYear = DateTime.Now.NumberOfDaysInYear(),
                StatusChangedDate = new DateTime(2024, 9, 9),
                VarianceSelectionOption = VarianceSelectionOption.NoComparison
            };

            var expected = new List<Component>
            {
                new Component(expectedMeta)
                {
                    Id = "test-funding-amount",
                    Values = new List<object>
                    {
                        1234.56
                    },
                    Title = string.Empty,
                    AlternativeTitle = string.Empty,
                    OriginalGroup = new UiModelGroup
                    {
                        Id = "test-funding-amount",
                        Selector = "$..fundingAmount"
                    },
                    VarianceValueFormatted = string.Empty
                }
            };

            mockComponentConfigurationService.Setup(s => s.GetComponentConfiguration(
                It.IsAny<FundingProperties>(),
                It.IsAny<IFundingApiSearch>(),
                It.IsAny<UiModelDataset>(),
                It.IsAny<List<List<IFundingApiSearch>>>(),
                It.IsAny<UiModel>(),
                It.IsAny<FundingStream>(),
                It.IsAny<FundingDocument>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<VarianceSelectionOption>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<bool>())).Returns(expectedMeta);

            mockComponentService
                .Setup(cs => cs.GetComponent(
                    It.IsAny<UiModelGroup>(),
                    It.IsAny<ComponentConfiguration>(),
                    It.IsAny<ComponentConfiguration>(),
                    It.IsAny<Dictionary<ComponentType, Defaults>>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Returns(expected.First());

            // Act
            var actual = await sharedFundingViewServices.GetFundingViewData(
                viewDataRequest);

            // Assert
            this.mockRepository.VerifyAll();
            actual.Should().NotBeNull();

            actual.Components.Should().HaveCountGreaterThanOrEqualTo(expected.Count);
        }

        private SharedFundingViewServices CreateSharedFundingViewServices()
        {
            return new SharedFundingViewServices(
                this.mockComponentService.Object,
                this.mockComponentConfigurationService.Object,
                this.mockUIModelFilesServices.Object,
                this.mockCacheService.Object,
                this.mockGlobalSettingsService.Object);
        }
    }
}
