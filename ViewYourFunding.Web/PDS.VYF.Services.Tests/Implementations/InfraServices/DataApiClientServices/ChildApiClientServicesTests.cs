using Moq;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
using PDS.VYF.Services.Implementations.InfraServices.DataApiClientServices;
using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

namespace PDS.VYF.Services.Tests.Implementations.InfraServices.DataApiClientServices
{
    /// <summary>
    /// The Child API client Services Test.
    /// </summary>
    [TestClass, TestCategory("Unit")]
    public class ChildApiClientServicesTests
    {
        private MockRepository mockRepository;

        private Mock<IHttpApiService> mockHttpApiService;
        private Mock<IFundingStreamSettingsServices> mockFundingStreamSettingsServices;

        public ChildApiClientServicesTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockHttpApiService = this.mockRepository.Create<IHttpApiService>();
            this.mockFundingStreamSettingsServices = this.mockRepository.Create<IFundingStreamSettingsServices>();
        }

        [TestMethod]
        public async Task SearchChild_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var childApiClientServices = this.CreateChildApiClientServices();
            var childSearchApiRequest = new ChildSearchApiRequestModel(true, "12345678");
            bool setLoggedInFundingStreamPeriods = true;
            var fundingStreamPeriod = new List<string>()
            {
                "GAG-AC-2425"
            };

            mockHttpApiService.Setup(s => s.PostRequest<List<LoggedInChildModel>>(
                    $"Child/SearchChild",
                    It.IsAny<string>(),
                    "application/json"))
                .ReturnsAsync(new List<LoggedInChildModel>());

            mockFundingStreamSettingsServices.Setup(s => s.GetLoggedInFundingStreamPeriod(false)).ReturnsAsync(fundingStreamPeriod);

            // Act
            var actual = await childApiClientServices.SearchChild(
                childSearchApiRequest,
                setLoggedInFundingStreamPeriods,
                fundingStreamPeriod);

            // Assert
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task SearchChildrenOfAParent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var childApiClientServices = this.CreateChildApiClientServices();
            string parentUKPRN = "12345678";
            ChildSearchApiRequestModel childRequest = new()
            {
                ListOfUKPRNs = new List<string>() { "12345678" },
                HasToBeLatestFunding = true,
            };
            bool setLoggedInFundingStreamPeriods = true;
            var fundingStreamPeriod = new List<string>()
            {
                "GAG-AC-2425"
            };

            mockHttpApiService.Setup(s => s.PostRequest<List<LoggedInChildModel>>(
                    $"Child/SearchChildrenOfAParent/{parentUKPRN}",
                    It.IsAny<string>(),
                    "application/json"))
                .ReturnsAsync(new List<LoggedInChildModel>());

            mockFundingStreamSettingsServices.Setup(s => s.GetLoggedInFundingStreamPeriod(false)).ReturnsAsync(fundingStreamPeriod);

            // Act
            var result = await childApiClientServices.SearchChildrenOfAParent(
                parentUKPRN,
                childRequest,
                setLoggedInFundingStreamPeriods,
                fundingStreamPeriod);

            // Assert
            this.mockRepository.VerifyAll();
        }


        [TestMethod]
        public async Task IsLatestStatement_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var childApiClientServices = this.CreateChildApiClientServices();
            string id = "GAG-AC-2425-12345678-2_0";
            var childRequest = new ChildSearchApiRequestModel(true, "12345678");
            bool setLoggedInFundingStreamPeriods = true;
            var fundingStreamPeriod = new List<string>()
            {
                "GAG-AC-2425"
            };

            mockHttpApiService.Setup(s => s.PostRequest<bool>(
                    $"Child/IsLatestStatement/{id}",
                    It.IsAny<string>(),
                    "application/json"))
                .ReturnsAsync(true);

            mockFundingStreamSettingsServices.Setup(s => s.GetLoggedInFundingStreamPeriod(false)).ReturnsAsync(fundingStreamPeriod);

            // Act
            var actual = await childApiClientServices.IsLatestStatement(
                id,
                childRequest,
                setLoggedInFundingStreamPeriods,
                fundingStreamPeriod);

            // Assert
            actual.Equals(true);
            this.mockRepository.VerifyAll();
        }

        private ChildApiClientServices CreateChildApiClientServices()
        {
            return new ChildApiClientServices(
                this.mockHttpApiService.Object,
                this.mockFundingStreamSettingsServices.Object);
        }
    }
}
