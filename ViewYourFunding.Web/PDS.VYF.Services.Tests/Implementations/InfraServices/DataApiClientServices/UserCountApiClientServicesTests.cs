using Moq;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.VYF.Services.Implementations.InfraServices.DataApiClientServices;
using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

namespace PDS.VYF.Services.Tests.Implementations.InfraServices.DataApiClientServices
{
    [TestClass, TestCategory("Unit")]
    public class UserCountApiClientServicesTests
    {
        private MockRepository mockRepository;

        private Mock<IHttpApiService> mockHttpApiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCountApiClientServicesTests"/> class.
        /// </summary>
        public UserCountApiClientServicesTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockHttpApiService = this.mockRepository.Create<IHttpApiService>();
        }

        /// <summary>
        /// Gets the user view count state under test expected behavior.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetUserViewCount_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userCountApiClientServices = this.CreateUserCountApiClientServices();
            var userViewCountRequest = new UserViewCountRequestModel
            {
                UserId = "ravimekala4074495ravi.mekala@education.gov.uk",
                ChildStatements = new List<ChildStatementModel>()
            };

            var expectedUserViewCountResponse = new UserViewCountResponse
            {
                NewCount = 0,
                UpdatedCount = 0,
            };

            mockHttpApiService.Setup(s => s.PostRequestToUserFundingView<UserViewCountResponse>(
                    $"UserView/GetUserViewCount",
                    It.IsAny<string>(),
                    "application/json"))
                .ReturnsAsync(new UserViewCountResponse());

            // Act
            var actual = await userCountApiClientServices.GetUserViewCount(
                userViewCountRequest);

            // Assert
            actual.Equals(expectedUserViewCountResponse);
            this.mockRepository.VerifyAll();
        }

        /// <summary>
        /// Determines whether [has user visited state under test expected behavior].
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task HasUserVisited_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userCountApiClientServices = this.CreateUserCountApiClientServices();
            string userId = "ravimekala4074495ravi.mekala@education.gov.uk";
            string providerFundingId = "GAG-AC-2425-12345678-1_0";

            mockHttpApiService.Setup(s => s.GetResponseFromUserFundingView<bool>(
                    $"UserView/HasUserVisited/{userId}/{providerFundingId}"))
                .ReturnsAsync(true);

            // Act
            var actual = await userCountApiClientServices.HasUserVisited(
                userId,
                providerFundingId);

            // Assert
            actual.Equals(true);
            this.mockRepository.VerifyAll();
        }

        /// <summary>
        /// Adds the user visited information state under test expected behavior.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task AddUserVisitedInfo_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userCountApiClientServices = this.CreateUserCountApiClientServices();
            string userId = "ravimekala4074495ravi.mekala@education.gov.uk";
            string providerFundingId = "GAG-AC-2425-12345678-1_0";

            mockHttpApiService.Setup(s => s.PostRequestToUserFundingView<bool>(
                    $"UserView/AddUserVisitedInfo/{userId}/{providerFundingId}",
                    string.Empty,
                    "application/json"))
                .ReturnsAsync(true);

            // Act
            var actual = await userCountApiClientServices.AddUserVisitedInfo(
                userId,
                providerFundingId);

            // Assert
            actual.Equals(true);
            this.mockRepository.VerifyAll();
        }

        private UserCountApiClientServices CreateUserCountApiClientServices()
        {
            return new UserCountApiClientServices(
                this.mockHttpApiService.Object);
        }
    }
}
