using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    /// <summary>
    /// The FundingApiServiceTests class.
    /// </summary>
    /// <seealso cref="ViewYourFunding.Services.Implementations.FundingApiService" />
    [TestClass]
    public class FundingApiServiceTests : FundingApiService
    {
        private static Mock<IHttpApiService> _mockHttpApiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingApiServiceTests"/> class.
        /// </summary>
        public FundingApiServiceTests() : base(Http().Object, new LoggerAdapter<FundingApiService>(null))
        {
        }

        /// <summary>
        /// Searches the funding only filtered on cutoff date should return at least one result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task SearchFunding_OnlyFilteredOnCutoffDate_ShouldReturnAtLeastOneResult()
        {
            // Arrange
            var cutoffDate = new DateTime(2010, 12, 21);
            var fundingStream = new FundingApiSearchFundingStream { BeforeDateTime = cutoffDate };
            var requestObj = new FundingApiSearchRequestObject { FundingStreams = new[] { fundingStream }, SearchTerm = "abc" };

            // Act
            var actual = await SearchFunding(requestObj);

            // Assert
            actual.Should().NotBeNull();
            actual.Funding.Should().NotBeNull().And.HaveCountGreaterThan(0);
        }

        /// <summary>
        /// Searches the provider funding unfiltered except cutoff date should return at least one result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task SearchProviderFunding_UnfilteredExceptCutoffDate_ShouldReturnAtLeastOneResult()
        {
            // Arrange
            var cutoffDate = new DateTime(2010, 12, 21);
            var fundingStream = new FundingApiSearchFundingStream { BeforeDateTime = cutoffDate };
            var requestObj = new FundingApiSearchRequestObject { FundingStreams = new[] { fundingStream }, SearchTerm = "def" };

            // Act
            var actual = await SearchProviderFunding(requestObj);

            // Assert
            actual.Should().NotBeNull();
            actual.ProviderFunding.Should().NotBeNull().And.HaveCountGreaterThan(0);
        }

        [DataRow("St Joseph")]
        [DataRow("St. Joseph")]
        [TestMethod, TestCategory("Unit")]
        public async Task SearchProviderFunding_VarietySearchTerms_ShouldMatch(string searchTerm)
        {
            // Arrange
            var cutoffDate = new DateTime(2099, 01, 01);
            var fundingStream = new FundingApiSearchFundingStream { BeforeDateTime = cutoffDate };
            var requestObj = new FundingApiSearchRequestObject { FundingStreams = new[] { fundingStream }, SearchTerm = searchTerm };

            // Act
            var actual = await SearchProviderFunding(requestObj);

            // Assert
            actual.Should().NotBeNull();

            actual.ProviderFunding.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
        }

        [DataRow("---")]
        [DataRow("-@$")]
        [TestMethod, TestCategory("Unit")]
        public async Task SearchProviderFunding_VarietySearchTerms_ShouldNotMatch(string searchTerm)
        {
            // Arrange
            var cutoffDate = new DateTime(2099, 01, 01);
            var fundingStream = new FundingApiSearchFundingStream { BeforeDateTime = cutoffDate };
            var requestObj = new FundingApiSearchRequestObject { FundingStreams = new[] { fundingStream }, SearchTerm = searchTerm };

            // Act
            var actual = await SearchProviderFunding(requestObj);

            // Assert
            actual.Should().BeNull();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetProviderFunding_ForAnId_ReturnsResult()
        {
            // Arrange
            var id = "provider funding id";

            // Act
            var actual = await GetProviderFunding(id);

            // Assert
            actual.Should().NotBeNull();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetProviderFunding_ForNull_ReturnsNull()
        {
            // Act
            var actual = await GetProviderFunding(null);

            // Assert
            actual.Should().BeNull();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task HasUserVisitedFunding_ReturnsResult()
        {
            // Arrange
            var userId = "user id";
            var fundingId = "funding id";

            // Act
            var actual = await HasUserVisitedFunding(userId, fundingId);

            // Assert
            actual.Should().BeTrue();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task AddUserFundingViewDetail_ReturnsResult()
        {
            // Arrange
            var userId = "user id";
            var fundingId = "funding id";

            var addUserFundingViewRequest = new AddUserFundingViewRequest { UserId = userId, FundingId = fundingId };

            // Act
            await AddUserFundingViewDetail(addUserFundingViewRequest);

            // Assert
            _mockHttpApiService.Verify(
                s => s.PostRequestToUserFundingView<bool?>(
                    $"user/AddUserFundingViewDetail",
                    JsonConvert.SerializeObject(addUserFundingViewRequest),
                    "application/json"), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetUserFundingViewCount_ReturnsResult()
        {
            // Arrange
            var userId = "user id";
            var fundingIds = new List<FundingVersionDetail>
            {
                new FundingVersionDetail { FundingId = "funding1", StatementChannelVersion = 1 },
                new FundingVersionDetail { FundingId = "funding2", StatementChannelVersion = 2 }
            };

            var expected = new UserFundingViewCountResponse
            { UserId = "user 1", UnreadNewFundings = 10, UnreadUpdatedFundings = 20 };

            // Act
            var actual = await GetUserFundingViewCount(userId, fundingIds);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        /// <summary>
        /// HTTPs this instance.
        /// </summary>
        /// <returns>The Mock IHttpApiService.</returns>
        private static Mock<IHttpApiService> Http()
        {
            _mockHttpApiService = new Mock<IHttpApiService>();

            _mockHttpApiService.Setup(s => s.PostRequest<FundingApiSearchResponse>(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new FundingApiSearchResponse
                {
                    Funding = new List<IFundingApiSearchFunding>
                    {
                        new FundingApiSearchFunding
                        {
                            //...
                        }
                    },
                    ProviderFunding = null
                });

            _mockHttpApiService.Setup(s => s.PostRequest<ProviderFundingApiSearchResponse>(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = new List<IFundingApiSearchProviderFunding>
                    {
                        new FundingApiSearchProviderFunding
                        {
                            //...
                        }
                    }
                });

            _mockHttpApiService.Setup(s => s.PostRequestToUserFundingView<bool?>(
                    $"user/AddUserFundingViewDetail",
                    It.IsAny<string>(),
                    "application/json"))
                .ReturnsAsync(true);

            _mockHttpApiService.Setup(s => s.PostRequestToUserFundingView<UserFundingViewCountResponse>(
                    $"user/GetUserFundingViewCount",
                    It.IsAny<string>(),
                    "application/json"))
                .ReturnsAsync(new UserFundingViewCountResponse { UserId = "user 1", UnreadNewFundings = 10, UnreadUpdatedFundings = 20 });

            _mockHttpApiService.Setup(s => s.GetRequestSingleResult<FundingApiSearchProviderFunding>(
                    It.IsAny<string>()))
                .ReturnsAsync(new FundingApiSearchProviderFunding());

            _mockHttpApiService.Setup(s => s.GetRequestSingleResult<bool>(
                    It.IsAny<string>()))
                .ReturnsAsync(true);

            _mockHttpApiService.Setup(s => s.GetResponseFromUserFundingView<bool>(
                    It.IsAny<string>()))
                .ReturnsAsync(true);

            return _mockHttpApiService;
        }
    }
}