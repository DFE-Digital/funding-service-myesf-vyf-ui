using FluentAssertions;
using Moq;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
using PDS.VYF.Services.Implementations.AppServices;
using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;
using PDS.VYF.Services.Tests.Mocks.InfraServices.DataApiClientServices;
using PDS.VYF.Services.Tests.Mocks.OtherServices;

namespace PDS.VYF.Services.Tests.Implementations.AppServices
{
    /// <summary>
    /// The test class for ChildOrParentNameServices.
    /// </summary>
    [TestClass, TestCategory("Unit")]
    public class ChildOrParentNameServicesTests
    {
        private readonly MockParentApiClientServices mockParentApiClientServices = new();
        private readonly MockChildApiClientServices mockChildApiClientServices = new();
        private readonly Mock<ILoggerAdapter<ModelFundingViewService>> mockLogger = new();
        private readonly ChildOrParentNameServices childOrParentNameServices;

        private readonly MockCacheService mockCacheService = new();
        private MockRepository mockRepository;
        private Mock<IFundingStreamSettingsServices> mockFundingStreamSettingsServices;


        /// <summary>
        /// Initializes a new instance of the <see cref="ChildOrParentNameServicesTests"/> class.
        /// The test publication date.
        /// </summary>
        public ChildOrParentNameServicesTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockFundingStreamSettingsServices = mockRepository.Create<IFundingStreamSettingsServices>();

            this.childOrParentNameServices = new ChildOrParentNameServices(
                                                    mockLogger.Object,
                                                    mockCacheService.Object,
                                                    mockChildApiClientServices.Object,
                                                    mockParentApiClientServices.Object,
                                                    mockFundingStreamSettingsServices.Object);
        }

        /// <summary>
        /// Gets the parent or child name internal should return from API call result success.
        /// </summary>
        /// <param name="ukprn">The ukprn.</param>
        /// <param name="isParent">if set to <c>true</c> [is parent].</param>
        /// <param name="childName">Name of the child.</param>
        /// <param name="parentName">Name of the parent.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        [DataRow("12345678", true, "Test Child Provider Name", "Test Parent Org Name")]
        [DataRow("12340000", false, "Test Child Provider Name", "Test Parent Org Name")]
        [DataRow("12345678", true, "Test Child Provider Name", "")]
        [DataRow("12345678", true, "Test Child Provider Name", " ")]
        [DataRow("12345678", true, "Test Child Provider Name", null)]
        [DataRow("12345678", true, "", null)]
        [DataRow("12345678", true, " ", null)]
        [DataRow("12345678", true, null, null)]
        [DataRow("12340000", false, "", "Test Parent Org Name")]
        [DataRow("12340000", false, " ", "Test Parent Org Name")]
        [DataRow("12340000", false, null, "Test Parent Org Name")]
        [DataRow("12340000", false, null, "")]
        [DataRow("12340000", false, null, " ")]
        [DataRow("12340000", false, null, null)]
        public async Task GetParentOrChildNameInternal_ShouldReturnFromApiCallResult_Success(string ukprn, bool isParent, string childName, string parentName)
        {
            // Arrange
            string expectedResult;
            int childApiSearchCount = 0;
            int parentApiSearchCount = 0;

            if (isParent)
            {
                parentApiSearchCount += 1;
                if (!string.IsNullOrWhiteSpace(parentName))
                {
                    expectedResult = parentName;
                }
                else
                {
                    childApiSearchCount += 1;
                    expectedResult = childName ?? string.Empty;
                }
            }
            else
            {
                childApiSearchCount += 1;
                if (!string.IsNullOrWhiteSpace(childName))
                {
                    expectedResult = childName;
                }
                else
                {
                    parentApiSearchCount += 1;
                    expectedResult = parentName ?? string.Empty;
                }
            }

            mockParentApiClientServices.SetupIsParent(ukprn, isParent);
            mockParentApiClientServices.SetupSearchParent(new List<LoggedInParentModel>() { new() { GroupName = parentName } });
            mockChildApiClientServices.SetupSearchChild(new List<LoggedInChildModel>() { new() { OrganisationName = childName } });

            // Act
            var result = await childOrParentNameServices.GetParentOrChildNameInternal(ukprn, isParent);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
            mockParentApiClientServices.VerifySearchParent(parentApiSearchCount);
            mockChildApiClientServices.VerifySearchChild(childApiSearchCount);
        }

        /// <summary>
        /// Gets the parent or child name should return name from cache success.
        /// </summary>
        /// <param name="ukprn">The ukprn.</param>
        /// <param name="isParent">if set to <c>true</c> [is parent].</param>
        /// <param name="childName">Name of the child.</param>
        /// <param name="parentName">Name of the parent.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        [DataRow("12345678", true, "Test Child Provider Name", "Test Parent Org Name")]
        [DataRow("12340000", false, "Test Child Provider Name", "Test Parent Org Name")]
        [DataRow("12345678", true, "Test Child Provider Name", "")]
        [DataRow("12345678", true, "Test Child Provider Name", " ")]
        [DataRow("12345678", true, "Test Child Provider Name", null)]
        [DataRow("12345678", true, "", null)]
        [DataRow("12345678", true, " ", null)]
        [DataRow("12345678", true, null, null)]
        [DataRow("12340000", false, "", "Test Parent Org Name")]
        [DataRow("12340000", false, " ", "Test Parent Org Name")]
        [DataRow("12340000", false, null, "Test Parent Org Name")]
        [DataRow("12340000", false, null, "")]
        [DataRow("12340000", false, null, " ")]
        [DataRow("12340000", false, null, null)]
        public async Task GetParentOrChildName_ShouldReturnNameFromCache_Success(string ukprn, bool isParent, string childName, string parentName)
        {
            // Arrange
            string expectedResult;
            int childApiSearchCount = 0;
            int parentApiSearchCount = 0;

            if (isParent)
            {
                if (!string.IsNullOrWhiteSpace(parentName))
                {
                    expectedResult = parentName;
                }
                else
                {
                    expectedResult = childName ?? string.Empty;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(childName))
                {
                    expectedResult = childName;
                }
                else
                {
                    expectedResult = parentName ?? string.Empty;
                }
            }

            var periodcode = new List<string>
            {
                "GAG-AC-2425", "GAG-AC-2526", "GAG-AC-2627"
            };

            mockFundingStreamSettingsServices.Setup(s => s.GetEmailEnabledFundingStreamPeriod()).ReturnsAsync(periodcode);

            mockParentApiClientServices.SetupIsParent(ukprn, isParent);
            mockChildApiClientServices.SetupSearchChild(new List<LoggedInChildModel>() { new() { OrganisationName = "Dummy Child name" } });
            mockParentApiClientServices.SetupSearchParent(new List<LoggedInParentModel> { new() { GroupName = "Dummy Parent name" } });
            mockCacheService.SetupAddOrGetExistingResultAsync($"ChildOrParentName-{ukprn}", ViewYourFunding.Services.Cache.CacheExpirationPolicy.Sliding, expectedResult);

            // Act
            var result = await childOrParentNameServices.GetParentOrChildName(ukprn);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
            mockChildApiClientServices.VerifySearchChild(childApiSearchCount);
            mockParentApiClientServices.VerifySearchParent(parentApiSearchCount);
            mockCacheService.VerifyAddOrGetExistingResultAsync<string>($"ChildOrParentName-{ukprn}", ViewYourFunding.Services.Cache.CacheExpirationPolicy.Sliding, 1);
        }
    }
}
