using Moq;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
using PDS.VYF.Services.Implementations.InfraServices.DataApiClientServices;
using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

namespace PDS.VYF.Services.Tests.Implementations.InfraServices.DataApiClientServices
{
    /// <summary>
    /// The parent API client services tests.
    /// </summary>
    [TestClass, TestCategory("Unit")]
    public class ParentApiClientServicesTests
    {
        private MockRepository mockRepository;

        private Mock<IHttpApiService> mockHttpApiService;
        private Mock<IFundingStreamSettingsServices> mockFundingStreamSettingsServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentApiClientServicesTests"/> class.
        /// </summary>
        public ParentApiClientServicesTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockHttpApiService = this.mockRepository.Create<IHttpApiService>();
            this.mockFundingStreamSettingsServices = this.mockRepository.Create<IFundingStreamSettingsServices>();
        }

        /// <summary>
        /// Searches the parent state under test expected behavior.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task SearchParent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var parentApiClientServices = this.CreateParentApiClientServices();
            var parentDataApiRequest = new ParentSearchApiRequestModel(true, "12345678");
            bool setLoggedInFundingStreamPeriods = true;
            var fundingStreamPeriod = new List<string>()
            {
                "GAG-AC-2425"
            };

            mockHttpApiService.Setup(s => s.PostRequest<List<LoggedInParentModel>>(
                    $"Parent/SearchParent",
                    It.IsAny<string>(),
                    "application/json"))
                .ReturnsAsync(new List<LoggedInParentModel>());

            mockFundingStreamSettingsServices.Setup(s => s.GetLoggedInFundingStreamPeriod(true)).ReturnsAsync(fundingStreamPeriod);

            // Act
            var actual = await parentApiClientServices.SearchParent(
                parentDataApiRequest,
                setLoggedInFundingStreamPeriods,
                fundingStreamPeriod);

            // Assert
            this.mockRepository.VerifyAll();
        }

        /// <summary>
        /// Determines whether [is parent state under test expected behavior].
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task IsParent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var parentApiClientServices = this.CreateParentApiClientServices();
            string strUKPRN = "12345678";
            bool setLoggedInFundingStreamPeriods = true;
            var fundingStreamPeriod = new List<string>()
            {
                "GAG-AC-2425"
            };

            mockHttpApiService.Setup(s => s.PostRequest<bool>(
                    $"Parent/IsParent/{strUKPRN}",
                    It.IsAny<string>(),
                    "application/json"))
                .ReturnsAsync(true);

            mockFundingStreamSettingsServices.Setup(s => s.GetLoggedInFundingStreamPeriod(true)).ReturnsAsync(fundingStreamPeriod);

            // Act
            var actual = await parentApiClientServices.IsParent(
                strUKPRN,
                setLoggedInFundingStreamPeriods,
                fundingStreamPeriod);

            // Assert
            actual.Equals(true);
            this.mockRepository.VerifyAll();
        }

        /// <summary>
        /// Determines whether [is my child state under test expected behavior].
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task IsMyChild_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var parentApiClientServices = this.CreateParentApiClientServices();
            string parentUKPRN = "12345678";
            string childUKPRN = "10009999";
            bool setLoggedInFundingStreamPeriods = true;
            var fundingStreamPeriod = new List<string>()
            {
                "GAG-AC-2425"
            };

            mockHttpApiService.Setup(s => s.PostRequest<bool>(
                    $"Parent/IsMyChild/{parentUKPRN}/{childUKPRN}",
                    It.IsAny<string>(),
                    "application/json"))
                .ReturnsAsync(true);

            mockFundingStreamSettingsServices.Setup(s => s.GetLoggedInFundingStreamPeriod(true)).ReturnsAsync(fundingStreamPeriod);

            // Act
            var actual = await parentApiClientServices.IsMyChild(
                parentUKPRN,
                childUKPRN,
                setLoggedInFundingStreamPeriods,
                fundingStreamPeriod);

            // Assert
            actual.Equals(true);
            this.mockRepository.VerifyAll();
        }

        private ParentApiClientServices CreateParentApiClientServices()
        {
            return new ParentApiClientServices(
                this.mockHttpApiService.Object,
                this.mockFundingStreamSettingsServices.Object);
        }
    }
}
