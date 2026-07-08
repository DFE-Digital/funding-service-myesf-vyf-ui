using Moq;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Models;
using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
using PDS.VYF.Services.Implementations.AppServices;
using PDS.VYF.Services.Models.ApiModels;
using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;
using PDS.VYF.Services.Tests.Mocks.InfraServices.DataApiClientServices;

namespace PDS.VYF.Services.Tests.Implementations.AppServices
{
    /// <summary>
    /// The LoggedInApiServicesTests class.
    /// </summary>
    [TestClass, TestCategory("Unit")]
    public class LoggedInApiServicesTests
    {
        private readonly MockParentApiClientServices mockParentApiClientServices = new();
        private readonly MockChildApiClientServices mockChildApiClientServices = new();
        private MockRepository mockRepository;
        private Mock<ILoggerAdapter<LoggedInApiServices>> mockLoggerAdapter;
        private Mock<IGlobalSettingsService> mockGlobalSettingsService;
        private Mock<IFundingStreamSettingsServices> mockFundingStreamSettingsServices;
        private Mock<IUserCountApiClientServices> mockUserCountApiClientServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggedInApiServicesTests"/> class.
        /// </summary>
        public LoggedInApiServicesTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockLoggerAdapter = this.mockRepository.Create<ILoggerAdapter<LoggedInApiServices>>();
            this.mockGlobalSettingsService = this.mockRepository.Create<IGlobalSettingsService>();
            this.mockFundingStreamSettingsServices = this.mockRepository.Create<IFundingStreamSettingsServices>();
            this.mockUserCountApiClientServices = this.mockRepository.Create<IUserCountApiClientServices>();
        }

        [TestMethod]
        public async Task GetInfoForLoggedIn_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var loggedInApiServices = this.CreateLoggedInApiServices();
            string ukprn = "12345678";
            string principal = "ravimekala4074495ravi.mekala@education.gov.uk";
            string scheme = "https";
            string host = "myesf.local:44375";

            mockParentApiClientServices.SetupIsParent(ukprn, true);

            mockGlobalSettingsService.Setup(s => s.GetFirstOrDefault(GlobalSettingTypeConstants.DisplayLoggedInMultipleAcademyTrustViewTypeId)).
                ReturnsAsync(new ViewYourFunding.Services.Models.GlobalSetting { Value = "True" });

            mockGlobalSettingsService.Setup(s => s.GetFirstOrDefault(GlobalSettingTypeConstants.UrlForLoggedInMultipleAcademyTrustViewTypeId)).
                ReturnsAsync(new ViewYourFunding.Services.Models.GlobalSetting { Value = "/view-latest-funding/pre-16-16-19-statements/parent" });

            mockGlobalSettingsService.Setup(s => s.GetFirstOrDefault(GlobalSettingTypeConstants.PublicFacingUrlLeftPart)).
                ReturnsAsync(new ViewYourFunding.Services.Models.GlobalSetting { Value = null });

            ParentSearchApiRequestModel parentRequest = new()
            {
                HasToBeLatestFunding = true,
                ListOfUKPRNs = new List<string>() { "12345679" },
            };

            ChildSearchApiRequestModel childRequest = new()
            {
                ListOfUKPRNs = new List<string>() { "12345678" },
                HasToBeLatestFunding = true,
            };


            var periodcode = new List<string>
            {
                "GAG-AC-2425", "GAG-AC-2526", "GAG-AC-2627"
            };

            Dictionary<string, PDS.ViewYourFunding.Services.Models.FundingStream>? fundingStream = new()
            {
                {
                    "GAG", new PDS.ViewYourFunding.Services.Models.FundingStream
                    {
                        FundingStreamName = "General annual grant",
                        SettingValues = new List<ViewYourFunding.Services.Models.SettingValue>
                        {
                            new ViewYourFunding.Services.Models.SettingValue
                            {
                                Setting = new SettingType
                                {
                                    SettingName = "DigitalStatementsGoLiveDate"
                                },
                                Value = "17/05/2024"
                            }
                        }
                    }
                },
            };
            mockFundingStreamSettingsServices.Setup(s => s.GetLoggedInFundingStreams(It.IsAny<bool>())).ReturnsAsync(fundingStream);

            mockFundingStreamSettingsServices.Setup(s => s.GetEmailEnabledFundingStreamPeriod()).ReturnsAsync(periodcode);

            mockChildApiClientServices.SetupLatestFundingPeriod(new List<string>()
            {
                "GAG-AC-2526"
            });

            var exptectedLoggedInInfoResponse = new LoggedInInfo
            {
                FundingsExists = true,
                NewFundingsNotRead = 1,
                Path = "https://myesf.local:44375/view-latest-funding/pre-16-16-19-statements/parent",
                ToggledOn = true,
                UpdatedFundingsNotRead = 0
            };

            List<LoggedInChildModel>? loggedInChildModel = new()
            {
                new LoggedInChildModel()
                {
                    FundingStreamCode = "GAG",
                    StatusChangedDateOnly = new DateTime(2024, 08, 28),
                    StatementType = "New",
                    Id = "GAG-AC-2425-12345678-1_0"
                }
            };

            mockChildApiClientServices.Setup(s => s.SearchChildrenOfAParent(It.IsAny<string>(), It.IsAny<ChildSearchApiRequestModel>(), It.IsAny<bool>(), It.IsAny<List<string>?>()))
                .ReturnsAsync(loggedInChildModel);

            List<ChildStatementModel> childStatementModel = new()
            {
                new ChildStatementModel()
                {
                    StatementType = "New",
                    ChildId = "GAG-AC-2425-12345678-1_0"
                }
            };

            UserViewCountResponse userViewCountResponse = new()
            {
                NewCount = 1,
                UpdatedCount = 0
            };

            mockUserCountApiClientServices.Setup(s => s.GetUserViewCount(It.IsAny<UserViewCountRequestModel>())).ReturnsAsync(userViewCountResponse);

            // Act
            var actual = await loggedInApiServices.GetInfoForLoggedIn(
                ukprn,
                principal,
                scheme,
                host);

            // Assert
            actual.Equals(exptectedLoggedInInfoResponse);
            this.mockRepository.VerifyAll();
        }

        private LoggedInApiServices CreateLoggedInApiServices()
        {
            return new LoggedInApiServices(
                this.mockLoggerAdapter.Object,
                this.mockParentApiClientServices.Object,
                this.mockChildApiClientServices.Object,
                this.mockGlobalSettingsService.Object,
                this.mockFundingStreamSettingsServices.Object,
                this.mockUserCountApiClientServices.Object);
        }
    }
}
