using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Common.Identity.Models;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Web.Models;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Interfaces;
using PDS.ViewYourFunding.Web.Models.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class ErrorControllerUnitTests
    {
        #region Private fields

        private readonly string _contactUsLink;
        private readonly string _serviceNowLink;

        #endregion


        #region Mock Services

        private readonly Mock<IOptions<ApplicationConfiguration>> _mockConfigurationService;

        private readonly Mock<IClaimsBasedIdentityService> _mockSecurityService;

        private readonly Mock<IUserRoleAuthorizationService> _mockUserRoleAuthorizeService;

        private readonly Mock<IUserJourneyService> _mockUserJourneyService;

        private readonly Mock<IMapper> _mockMapper;

        private readonly Mock<IFundingApiService> _mockFundingApiService;

        private readonly Mock<IFundingViewService> _mockFundingViewService;

        private readonly Mock<ICacheService> _mockCacheService;

        private readonly Mock<IGlobalSettingService> _mockGlobalSettingService;

        #endregion


        #region Constructor

        public ErrorControllerUnitTests()
        {
            _mockConfigurationService = new Mock<IOptions<ApplicationConfiguration>>(MockBehavior.Strict);
            _mockSecurityService = new Mock<IClaimsBasedIdentityService>(MockBehavior.Strict);
            _mockUserRoleAuthorizeService = new Mock<IUserRoleAuthorizationService>(MockBehavior.Strict);
            _mockUserJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            _mockMapper = new Mock<IMapper>(MockBehavior.Strict);
            _mockFundingApiService = new Mock<IFundingApiService>(MockBehavior.Strict);
            _mockFundingViewService = new Mock<IFundingViewService>(MockBehavior.Strict);
            _mockCacheService = new Mock<ICacheService>(MockBehavior.Strict);
            _mockGlobalSettingService = new Mock<IGlobalSettingService>(MockBehavior.Strict);
            var appConfig = new ApplicationConfiguration();
            _serviceNowLink = appConfig.ServiceNowLink;
            _contactUsLink = appConfig.ContactUsLink;
        }

        #endregion


        #region Action Tests

        [TestMethod, TestCategory("Unit")]
        [DataRow(true)]
        [DataRow(false)]
        public async Task Index_ResultExpected(bool isExternalUser)
        {
            // Arrange
            var controller = GetErrorController("test", isExternalUser);
            var expectedViewModel = GetExpectedErrorPageViewModel_Index(isExternalUser);

            // Act
            var actual = await controller.Index();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ErrorPageViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("test", "A", true, null)]
        [DataRow("pre-prod", "A", true, null)]
        [DataRow("prod", "A", true, null)]
        [DataRow("prod", "A", false, new string[] { "MYESF Admin" })]
        [DataRow("prod", "A", true, null)]
        [DataRow("prod", "BB", false, new string[] { "View as provider" })]
        [DataRow("prod", "BB", true, new string[] { "View allocation statements" })]
        [DataRow("prod", "CCC", false, new string[] { "View as provider" })]
        [DataRow("prod", "CCC", true, new string[] { "View recoupment reports" })]
        [DataRow("prod", "D", false, new string[] { "MYESF Admin", "Allocations Administrator 1416", "Allocations Administrator 1619" })]
        [DataRow("prod", "D", true, null)]
        public async Task StatusCode403_DSIRequiredUserRoles_ExpectedResult(string dsiUrl, string encryptedUrl, bool isExternalUser, string[] expectedUserRoles)
        {
            // Arrange
            var controller = GetErrorController(dsiUrl, isExternalUser);

            var expectedViewModel = GetExpectedErrorPageViewModel_StatusCode403_DSIRequiredUserRoles(dsiUrl, isExternalUser, expectedUserRoles?.ToList());

            // Act
            var actual = await controller.StatusCode403_DSIRequiredUserRoles(encryptedUrl);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ErrorPageViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        #endregion


        #region Private helpers

        private ErrorPageViewModel GetExpectedErrorPageViewModel_Index(bool externalUser)
        {
            var expectedViewModel = new ErrorPageViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    Ukprn = 12345678,
                    ProviderName = "Test Org",
                    IsExternalUser = externalUser,
                    IsLoggedIn = !externalUser,
                },
                ContactUsLink = externalUser ? _contactUsLink : _serviceNowLink,
                ErrorPartialViewName = "_ErrorHelpDetails"
            };
            expectedViewModel.StatusCode = 0;

            return expectedViewModel;
        }

        private ErrorPageViewModel GetExpectedErrorPageViewModel_StatusCode403_DSIRequiredUserRoles(string dsiUrl, bool externalUser, List<string> userRoles)
        {
            var expectedViewModel = new ErrorPageViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    Ukprn = 12345678,
                    ProviderName = "Test Org",
                    IsExternalUser = externalUser,
                    IsLoggedIn = !externalUser,
                },
                RequiredUserRolesForUrl = userRoles,
                ErrorPartialViewName = "StatusCodes/_403DSIRequiredUserRoles",
                DfeSignInUrl = dsiUrl
            };
            expectedViewModel.StatusCode = 403;

            return expectedViewModel;
        }

        private ErrorController GetErrorController(string dsiUrl, bool isExternalUser)
        {
            SetupMockServices(dsiUrl, isExternalUser);
            var controller = new ErrorController(
                _mockSecurityService.Object,
                _mockConfigurationService.Object,
                _mockUserRoleAuthorizeService.Object,
                _mockUserJourneyService.Object,
                _mockMapper.Object,
                _mockFundingApiService.Object,
                _mockFundingViewService.Object,
                _mockCacheService.Object,
                _mockGlobalSettingService.Object);

            return controller;
        }

        private void SetupMockServices(string dsiUrl, bool isExternalUser)
        {
            var appConfig = new ApplicationConfiguration();
            appConfig.DfeSignInUrl = dsiUrl;
            _mockConfigurationService.Setup(x => x.Value)
                .Returns(appConfig);

            _mockSecurityService.Setup(x => x.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User
                {
                    Ukprn = 12345678,
                    IsAuthenticated = !isExternalUser,
                    IsExternalUser = isExternalUser
                });

            _mockUserRoleAuthorizeService.Setup(x => x.DecryptUrlParameterToRequiredUserRoleNames("A", false)).Returns(new List<string> { "MYESF Admin" });
            _mockUserRoleAuthorizeService.Setup(x => x.DecryptUrlParameterToRequiredUserRoleNames("A", true)).Returns((List<string>)null);
            _mockUserRoleAuthorizeService.Setup(x => x.DecryptUrlParameterToRequiredUserRoleNames("BB", false)).Returns(new List<string> { "View as provider" });
            _mockUserRoleAuthorizeService.Setup(x => x.DecryptUrlParameterToRequiredUserRoleNames("BB", true)).Returns(new List<string> { "View allocation statements" });
            _mockUserRoleAuthorizeService.Setup(x => x.DecryptUrlParameterToRequiredUserRoleNames("CCC", false)).Returns(new List<string> { "View as provider" });
            _mockUserRoleAuthorizeService.Setup(x => x.DecryptUrlParameterToRequiredUserRoleNames("CCC", true)).Returns(new List<string> { "View recoupment reports" });
            _mockUserRoleAuthorizeService.Setup(x => x.DecryptUrlParameterToRequiredUserRoleNames("D", false)).Returns(new List<string> { "MYESF Admin", "Allocations Administrator 1416", "Allocations Administrator 1619" });
            _mockUserRoleAuthorizeService.Setup(x => x.DecryptUrlParameterToRequiredUserRoleNames("D", true)).Returns((List<string>)null);

            _mockUserJourneyService.Setup(x => x.GetFundingStreams()).ReturnsAsync(
                new List<FundingStream>
                {
                    new FundingStream
                       {
                            FundingStreamName = "GAG",
                            FundingStreamCode = "GAG",
                            SettingValues = new List<SettingValue>
                            {
                                new SettingValue
                                {
                                    Setting = new SettingType
                                    {
                                        SettingName = "AcademyAcademicYear"
                                    },
                                    Value = "202122",
                                },
                            },
                            Publications = new List<Publication>
                            {
                                    new Publication
                                    {
                                        FundingPeriodCode = "AC-2122",
                                        PublishedDate = new DateTime(2030, 1, 1),
                                        Status = PublicationStatus.Published
                                    }
                            }
                       }
                });
            _mockFundingApiService.Setup(x => x.SearchFunding(It.IsAny<FundingApiSearchRequestObject>())).ReturnsAsync(
                new FundingApiSearchResponse
                {
                    Funding = new List<IFundingApiSearchFunding>
                    {
                        new FundingApiSearchFunding
                        {
                            GroupUkprn = "12345678",
                            GroupName = "Test Org",
                        }
                    }
                });
        }
        #endregion
    }
}