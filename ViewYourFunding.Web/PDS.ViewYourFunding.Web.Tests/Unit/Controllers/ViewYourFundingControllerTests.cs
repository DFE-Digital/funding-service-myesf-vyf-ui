using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Web.Models;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Attributes;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.RequestObjects;
using PDS.ViewYourFunding.Services.ResponseObjects;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Extensions;
using PDS.ViewYourFunding.Web.Models.Request;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using PDS.ViewYourFunding.Web.Tests.Constants;
using PDS.ViewYourFunding.Web.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Model = PDS.ViewYourFunding.Web.Models.FundingStream;
using User = Pds.Core.Common.Identity.Models.User;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers
{
    /// <summary>
    /// The VYF controller tests.
    /// </summary>
    [TestClass]
    public class ViewYourFundingControllerTests
    {
        #region Constants

        /// <summary>
        /// The value of the radio input for 'PE and sport'.
        /// </summary>
        private const string OptionValue_WhichAllocation_PSG = "PSG";

        /// <summary>
        /// The test funding year from.
        /// </summary>
        private const int TestFundingYearFrom = 2020;

        /// <summary>
        /// The test funding year to.
        /// </summary>
        private const int TestFundingYearTo = 2021;

        /// <summary>
        /// The search term.
        /// </summary>
        private const string SearchTerm = "searchTerm";

        /// <summary>
        /// The organisation ukprn.
        /// </summary>
        private const string OrganisationUkprn = nameof(IFundingApiSearchProviderFunding.OrganisationUkprn);

        /// <summary>
        /// Gets the first organisation ukprn.
        /// </summary>
        private string FirstOrganisationUkprn
        {
            get
            {
                return $"{OrganisationUkprn}0";
            }
        }

        /// <summary>
        /// The organisation name.
        /// </summary>
        private const string OrganisationName = nameof(IFundingApiSearchProviderFunding.OrganisationName);

        /// <summary>
        /// The DSG funding stream name.
        /// </summary>
        private const string DSGFundingStreamName = "Dedicated schools grant";

        /// <summary>
        /// The PSG funding stream name.
        /// </summary>
        private const string PSGFundingStreamName = "PE and sport premium";

        /// <summary>
        /// The test feedback link.
        /// </summary>
        private const string TestFeedbackLink = "http://www.vyf-survey.com/survey";

        /// <summary>
        /// The test contact us link.
        /// </summary>
        private const string TestContactUsLink = "http://www.vyf-contactus.com/contactus";

        /// <summary>
        /// The test search term.
        /// </summary>
        private const string TestSearchTerm = "the search term";

        /// <summary>
        /// The PSG publication date.
        /// </summary>
        private const string PSGPublicationDateSetting = "01/01/2020";

        /// <summary>
        /// The DSG year setting.
        /// </summary>
        private const string DSGYearSetting = "202021";

        /// <summary>
        /// The PSG year setting.
        /// </summary>
        private const string PSGYearSetting = "201920";

        /// <summary>
        /// The PSG funding period code.
        /// </summary>
        private const string PSGFundingPeriodCode = "AY-1920";

        /// <summary>
        /// The DSG funding period code.
        /// </summary>
        private const string DSGFundingPeriodCode = "FY-2021";

        /// <summary>
        /// The next payment date setting.
        /// </summary>
        private const string NextPaymentDateSetting = "01/06/2020";

        /// <summary>
        /// The next payment date setting maintained.
        /// </summary>
        private const string NextPaymentDateSettingMaintained = "02/06/2020";

        /// <summary>
        /// The next payment date setting academies.
        /// </summary>
        private const string NextPaymentDateSettingAcademies = "01/07/2020";

        /// <summary>
        /// The next payment date setting NMSS.
        /// </summary>
        private const string NextPaymentDateSettingNMSS = "01/08/2020";

        /// <summary>
        /// The test local authority code.
        /// </summary>
        private const string TestLocalAuthorityCode = "0";

        /// <summary>
        /// The test local authority name.
        /// </summary>
        private const string TestLocalAuthorityName = "Test LA";

        /// <summary>
        /// The test funding total.
        /// </summary>
        private const decimal TestFundingTotal = 12345678;

        /// <summary>
        /// The test publication date.
        /// </summary>
        private static readonly DateTime TestPublicationDate = new DateTime(2019, 12, 31);

        /// <summary>
        /// The test status changed date.
        /// </summary>
        private static readonly DateTime TestStatusChangedDate = new DateTime(2019, 12, 31);

        /// <summary>
        /// The test publication description.
        /// </summary>
        private const string TestPublicationDescription = nameof(TestPublicationDescription);

        /// <summary>
        /// As of allocations year.
        /// </summary>
        private const string AsOfAllocationsYear = "2019";

        /// <summary>
        /// As of allocations month.
        /// </summary>
        private const string AsOfAllocationsMonth = "September";

        /// <summary>
        /// The not logged in user.
        /// </summary>
        private static readonly User NotLoggedInUser = new User
        {
            IsAuthenticated = false
        };

        /// <summary>
        /// The logged in user.
        /// </summary>
        private static readonly User LoggedInUser = new User
        {
            Ukprn = 12345678,
            ProviderName = "Test Provider",
            Email = "test@test.com",
            IsAuthenticated = true,
            IsExternalUser = true
        };

        /// <summary>
        /// The test funding spreadsheets.
        /// </summary>
        private IEnumerable<FundingDocument> _nationalFundingAllocationSpreadsheets;

        /// <summary>
        /// Test provider name to be used for logged in providers.
        /// </summary>
        private const string ProviderNameFromProviderFunding = "Provider Name Obtained From Provider Funding Organisation Name";

        /// <summary>
        /// Test provider name to be obtained for MAT/LA etc.
        /// </summary>
        private const string ProviderNameFromFunding = "Provider Name Obtained From Funding Group Name";

        #endregion


        #region Start action tests

        /// <summary>
        /// Starts the view model matches expected.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Start_ViewModelMatchesExpected()
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();
            var fundingApiServiceMock = GetFundingApiService_SingleMatchingProviderFundingResult(
                user.Ukprn.ToString(),
                FundingStreamCode.DSG);

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                fundingApiServiceMock,
                settingsServiceMock,
                null);

            var expectedViewModel = new StartPageViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = ProviderNameFromProviderFunding,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                FundingStreams = new List<Model.FundingStream>
        {
            new Model.FundingStream
            {
                Id = 2,
                Active = true,
                FundingStreamCode = "DSG",
                FundingStreamName = "Dedicated schools grant",
                FundingStreamCodePubliclyKnown = true,
                FundingStreamNameWithinSentence = "dedicated schools grant",
                Publications = new List<Publication>
                {
                    new Publication
                    {
                        Description = "TestPublicationDescription",
                        FundingPeriodCode = "FY-2021",
                        FundingStreamId = 0,
                        IsLatest = true,
                        PublishedDate = new DateTime(2019, 12, 31),
                        Status = PublicationStatus.Published
                    }
                },
                NextPayments = new List<NextPayment>(),
                SettingValues = new List<SettingValue>
                {
                    new SettingValue
                    {
                        CreatedAt = DateTime.MaxValue,
                        FundingStreamId = 2,
                        Id = 0,
                        LastUpdatedAt = DateTime.MaxValue,
                        LastUpdatedBy = "System",
                        Setting = new SettingType
                        {
                            SettingName = "FinancialYear"
                        },
                        SettingId = 2,
                        Value = "202021"
                    }
                },
                RelevantForNational = true,
                RelevantForOrganisations_LoggedIn = true,
                RelevantForOrganisations_Public = true,
                RelevantForProviders_LoggedIn = false,
                RelevantForProviders_Public = false
            },
            new Model.FundingStream
            {
                Id = 1,
                Active = true,
                FundingStreamCode = "PSG",
                FundingStreamName = "PE and sport premium",
                FundingStreamNameWithinSentence = "PE and sport premium",
                Publications = new List<Publication>
                {
                    new Publication
                    {
                        Description = "TestPublicationDescription",
                        FundingPeriodCode = "AY-1920",
                        FundingStreamId = 0,
                        IsLatest = true,
                        PublishedDate = new DateTime(2019, 12, 31),
                        Status = PublicationStatus.Published
                    }
                },
                NextPayments = new List<NextPayment>(),
                SettingValues = new List<SettingValue>
                {
                    new SettingValue
                    {
                        CreatedAt = DateTime.MaxValue,
                        FundingStreamId = 1,
                        Id = 0,
                        LastUpdatedAt = DateTime.MaxValue,
                        LastUpdatedBy = "System",
                        Setting = new SettingType
                        {
                            SettingName = "AcademicYear"
                        },
                        SettingId = 1,
                        Value = "201920"
                    },
                    new SettingValue
                    {
                        CreatedAt = DateTime.MaxValue,
                        FundingStreamId = 1,
                        Id = 0,
                        LastUpdatedAt = DateTime.MaxValue,
                        LastUpdatedBy = "System",
                        Setting = new SettingType
                        {
                            SettingName = "ProviderDownloadSizeInBytes"
                        },
                        SettingId = 2,
                        Value = "5000"
                    }
                },
                RelevantForNational = true,
                RelevantForOrganisations_LoggedIn = true,
                RelevantForOrganisations_Public = true,
                RelevantForProviders_LoggedIn = true,
                RelevantForProviders_Public = true
            }
        }
            };

            // Act
            var actual = await controller.Start();

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<StartPageViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(info => info.Path.EndsWith("PublicationLayouts"))
                        .Excluding(info => info.Path.EndsWith("Setting.SettingValues"))
                        .Excluding(option => option.FundingStreams[0].SettingValues[0].CreatedAt)
                        .Excluding(option => option.FundingStreams[0].SettingValues[0].LastUpdatedAt)
                        .Excluding(option => option.FundingStreams[1].SettingValues[0].CreatedAt)
                        .Excluding(option => option.FundingStreams[1].SettingValues[0].LastUpdatedAt));
        }

        #endregion


        #region ViewingChoice[Chosen] action tests

        /// <summary>
        /// Viewings the choice view model matches expected no error.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ViewingChoice_ViewModelMatchesExpected_NoError()
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetFundingApiService_SingleMatchingFundingResult(user.Ukprn.ToString(), FundingStreamCode.DSG);
            var settingsServiceMock = GetMockSettingsService();
            var controller = GetViewYourFundingController(securityServiceMock, null, fundingApiServiceMock, settingsServiceMock);

            var expectedViewModel = new ViewingChoiceViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = ProviderNameFromFunding,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                ValidationError = false
            };

            // Act
            var actual = await controller.ViewingChoice();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ViewingChoiceViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        /// <summary>
        /// Viewings the choice view model matches expected error.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ViewingChoice_ViewModelMatchesExpected_Error()
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var controller = GetViewYourFundingController(securityServiceMock, null, GetFundingApiService_SingleMatchingProviderFundingResult(user.Ukprn?.ToString(), FundingStreamCode.DSG), GetMockSettingsService());

            var expectedViewModel = new ViewingChoiceViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = ProviderNameFromProviderFunding,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                ValidationError = true
            };

            // Act
            var actual = await controller.ViewingChoice(true);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ViewingChoiceViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        /// <summary>
        /// Viewings the choice chosen for organisation redirects to find an organisation action.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void ViewingChoiceChosen_ForOrganisation_RedirectsToFindAnOrganisationAction()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var controller = GetViewYourFundingController(securityServiceMock);

            // Act
            var actual = controller.ViewingChoiceChosen(ViewYourFundingConstants.OptionValue_ViewingChoice_FindAnOrganisation);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_FindAnOrganisation);
        }

        /// <summary>
        /// Viewings the choice chosen for national redirects to which allocation action.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void ViewingChoiceChosen_ForNational_RedirectsToWhichAllocationAction()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var controller = GetViewYourFundingController(securityServiceMock);

            // Act
            var actual = controller.ViewingChoiceChosen(ViewYourFundingConstants.OptionValue_ViewingChoice_WhichAllocation);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_WhichAllocation);
        }

        /// <summary>
        /// Viewings the choice chosen for null redirects to viewing choice action with error.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void ViewingChoiceChosen_ForNull_RedirectsToViewingChoiceAction_WithError()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var controller = GetViewYourFundingController(securityServiceMock);

            // Act
            var actual = controller.ViewingChoiceChosen(null);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ViewingChoice);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("error", true);
        }

        /// <summary>
        /// Viewings the choice chosen for empty redirects to viewing choice action with error.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void ViewingChoiceChosen_ForEmpty_RedirectsToViewingChoiceAction_WithError()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var controller = GetViewYourFundingController(securityServiceMock);

            // Act
            var actual = controller.ViewingChoiceChosen(string.Empty);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ViewingChoice);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("error", true);
        }

        #endregion


        #region WhichAllocation[Chosen] action tests

        /// <summary>
        /// Whiches the allocation view model matches expected no error.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task WhichAllocation_ViewModelMatchesExpected_NoError()
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();
            var fundingApiServiceMock = GetFundingApiService_SingleMatchingProviderFundingResult(user.Ukprn.ToString(), FundingStreamCode.DSG);
            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                fundingApiServiceMock,
                settingsServiceMock,
                null);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                        {
                            new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                        }, "someAuthTypeName"))
                }
            };

            var expectedViewModel = new WhichAllocationViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = ProviderNameFromProviderFunding,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                ValidationError = false,
                LatestYear = GetLatestYear()
            };

            // Act
            var actual = await controller.WhichAllocation();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<WhichAllocationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        /// <summary>
        /// Which allocation view model matches expected error.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task WhichAllocation_ViewModelMatchesExpected_Error()
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingApiServiceMock = GetFundingApiService_SingleMatchingFundingResult(user.Ukprn.ToString(), FundingStreamCode.DSG);
            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                mockFundingApiServiceMock,
                settingsServiceMock,
                null);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                        {
                            new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                        }, "someAuthTypeName"))
                }
            };

            var expectedViewModel = new WhichAllocationViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = ProviderNameFromFunding,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                ValidationError = true,
                LatestYear = GetLatestYear()
            };

            // Act
            var actual = await controller.WhichAllocation(true);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<WhichAllocationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        /// <summary>
        /// Whiches the allocation chosen for null redirects to which allocation action with error.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task WhichAllocationChosen_ForNull_RedirectsToWhichAllocationAction_WithError()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();
            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                null,
                settingsServiceMock,
                null);

            // Act
            var actual = await controller.WhichAllocationChosen(null);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_WhichAllocation);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("error", true);
        }

        /// <summary>
        /// Whiches the allocation chosen for empty redirects to which allocation action with error.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task WhichAllocationChosen_ForEmpty_RedirectsToWhichAllocationAction_WithError()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();
            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                null,
                settingsServiceMock,
                null);

            // Act
            var actual = await controller.WhichAllocationChosen(string.Empty);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_WhichAllocation);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("error", true);
        }

        /// <summary>
        /// 'Which allocation chosen' redirects to NationalFundingAllocation action.
        /// </summary>
        /// <param name="fundingStreamCode">The code for the funding Stream.</param>
        /// <param name="fundingStreamName">The name for the funding stream.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <param name="yearFrom">The start year of the funding year.</param>
        /// <param name="yearTo">The end year of the funding year.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        [DataRow(FundingStreamCode.DSG, DSGFundingStreamName, DSGFundingPeriodCode, TestFundingYearFrom, TestFundingYearTo, DisplayName = "DSG Chosen")]
        [DataRow(FundingStreamCode.PEAndSport, PSGFundingStreamName, PSGFundingPeriodCode, TestFundingYearFrom - 1, TestFundingYearTo - 1, DisplayName = "PSG Chosen")]
        public async Task WhichAllocationChosen_RedirectsToNationalFundingAllocation(
            string fundingStreamCode,
            string fundingStreamName,
            string fundingPeriodCode,
            int yearFrom,
            int yearTo)
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();
            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                null,
                settingsServiceMock,
                null);

            // Act
            var actual = await controller.WhichAllocationChosen(fundingStreamCode);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_NationalFundingAllocation);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("yearFrom", yearFrom);
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("yearTo", yearTo);
        }

        /// <summary>
        /// Whiches the allocation chosen for PSG redirects to PSG action.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task WhichAllocationChosen_ForPSG_RedirectsToPSGAction()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();
            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                null,
                settingsServiceMock,
                null);

            // Act
            var actual = await controller.WhichAllocationChosen(OptionValue_WhichAllocation_PSG);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_NationalFundingAllocation);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("yearFrom", 2019);
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("yearTo", 2020);
        }

        #endregion


        #region National Funding Allocation action tests

        /// <summary>
        /// National Funding Allocation view model matches expected for n spreadsheets.
        /// </summary>
        /// <param name="documentCount">The document count.</param>
        /// <param name="fundingStreamCode">The code for the funding Stream.</param>
        /// <param name="fundingStreamName">The name for the funding stream.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <param name="yearFrom">The start year of the funding year.</param>
        /// <param name="yearTo">The end year of the funding year.</param>
        /// <param name="canUseShortCode">Can we use the short code (PSG false, DSG true).</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        [DataRow(1, FundingStreamCode.DSG, DSGFundingStreamName, DSGFundingPeriodCode, TestFundingYearFrom, TestFundingYearTo, true, DisplayName = "One document")]
        [DataRow(1, FundingStreamCode.PEAndSport, PSGFundingStreamName, PSGFundingPeriodCode, TestFundingYearFrom - 1, TestFundingYearTo - 1, false, DisplayName = "One document")]
        [DataRow(10, FundingStreamCode.DSG, DSGFundingStreamName, DSGFundingPeriodCode, TestFundingYearFrom, TestFundingYearTo, true, DisplayName = "Many documents")]
        [DataRow(10, FundingStreamCode.PEAndSport, PSGFundingStreamName, PSGFundingPeriodCode, TestFundingYearFrom - 1, TestFundingYearTo - 1, false, DisplayName = "Many documents")]
        public async Task NationalFundingAllocation_ViewModelMatchesExpected_ForNSpreadsheets(
            int documentCount,
            string fundingStreamCode,
            string fundingStreamName,
            string fundingPeriodCode,
            int yearFrom,
            int yearTo,
            bool canUseShortCode)
        {
            // Arrange
            var user = LoggedInUser;

            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();

            var fundingViewServiceMock = GetNationalFundingAllocationMockFundingViewService(
                fundingStreamCode,
                fundingStreamName,
                fundingPeriodCode,
                yearFrom,
                yearTo);

            var fundingDocumentServiceMock = GetNationalFundingAllocationMockFundingDocumentService(
                fundingStreamCode,
                fundingStreamName,
                fundingPeriodCode,
                yearFrom,
                yearTo,
                documentCount);

            _nationalFundingAllocationSpreadsheets = GetNationalFundingAllocationMockFundingDocuments(
                fundingStreamCode,
                fundingStreamName,
                yearFrom,
                yearTo,
                documentCount,
                false);

            var controller = GetViewYourFundingController(
                securityServiceMock,
                fundingDocumentServiceMock,
                GetFundingApiService_SingleMatchingProviderFundingResult(user.Ukprn.ToString(), FundingStreamCode.PEAndSport),
                settingsServiceMock,
                fundingViewServiceMock);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                    {
                                    new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                    }, "someAuthTypeName"))
                }
            };

            var historicYears = GetHistoricYears(yearFrom, yearTo);
            var latestHistoricYears = historicYears.Where(historicYear => historicYear.yearFrom != yearFrom).ToList();

            var pageData = new Dictionary<string, object>
            {
                {
                    "currentYearFrom",
                    yearFrom
                },
                {
                    "currentYearTo",
                    yearTo
                },
                {
                    "LatestHistoricYears",
                    latestHistoricYears
                },
                {
                    "HistoricYears",
                    historicYears
                },
                {
                    "FutureYears",
                    new List<(int, int)>()
                },
                {
                    "CurrentYearsBeforeThisYear",
                    new List<(int, int)>()
                }
            };

            var expectedViewModel = new NationalFundingAllocationViewModel
            {
                FundingStreamCode = fundingStreamCode,
                FundingStreamName = fundingStreamName,
                YearFrom = yearFrom,
                YearTo = yearTo,
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                Spreadsheets = (IReadOnlyCollection<FundingDocument>)_nationalFundingAllocationSpreadsheets,
                ValidationError = false,
                ValidationErrorMessage = string.Empty,
                HistoricYears = historicYears,
                LatestHistoricYears = latestHistoricYears,
                FundingViewData = new FundingViewData
                {
                    EntityName = string.Empty,
                    PublicationDate = TestPublicationDate,
                    FundingValues = new Dictionary<string, object> { { "TestFundingTotal", 12345678 } },
                    Components = new List<Component>
                    {
                        new Component(new ComponentConfiguration())
                        {
                            ComponentConfiguration =
                            {
                                FundingPeriodCode = fundingPeriodCode,
                                FundingStreamCode = fundingStreamCode,
                                FundingStreamName = fundingStreamName,
                                IsCurrentYear = false,
                                IsLatestOrFinalFundingForYear = false,
                                OrganisationClosed = false,
                                ShowNameAbbreviation = false,
                                Year1 = yearFrom,
                                Year2 = yearTo
                            },
                            Title = "Test Heading",
                            Type = ComponentType.Heading_Large,
                            PageData = pageData
                        }
                    }
                },
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = ProviderNameFromProviderFunding,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                CanUseShortCode = canUseShortCode
            };

            // Act
            var actual = await controller.NationalFundingAllocation(fundingStreamCode, yearFrom, yearTo);

            // Assert
            actual.Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<NationalFundingAllocationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            var actualModel = ((ViewResult)actual).Model as NationalFundingAllocationViewModel;
            actualModel.Spreadsheets.Should().HaveCount(expectedViewModel.Spreadsheets.Count);

            fundingDocumentServiceMock.Verify();
        }

        /// <summary>
        /// National Funding Allocation with no funding configuration returns expected result.
        /// </summary>
        /// <param name="documentCount">The number of documents to generate.</param>
        /// <param name="fundingStreamCode">The code for the funding Stream.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <param name="yearFrom">The start year of the funding year.</param>
        /// <param name="yearTo">The end year of the funding year.</param>
        /// <returns>Task.</returns>
        [DataRow(0, FundingStreamCode.DSG, DSGFundingPeriodCode, TestFundingYearFrom, TestFundingYearTo)]
        [DataRow(0, FundingStreamCode.PEAndSport, PSGFundingPeriodCode, TestFundingYearFrom - 1, TestFundingYearTo - 1)]
        [TestMethod, TestCategory("Unit")]
        public async Task NationalFundingAllocation_NoFundingConfiguration_MatchesExpected(
            int documentCount,
            string fundingStreamCode,
            string fundingPeriodCode,
            int yearFrom,
            int yearTo)
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var userJourneyServiceMock = GetMockUserJourneyServiceNoFundingStreams();

            var fundingViewServiceMock = GetNationalFundingAllocationMockFundingViewService(
                fundingStreamCode,
                null,
                fundingPeriodCode,
                yearFrom,
                yearTo);

            var fundingDocumentServiceMock = GetNationalFundingAllocationMockFundingDocumentService(
                fundingStreamCode,
                null,
                fundingPeriodCode,
                yearFrom,
                yearTo,
                0);

            _nationalFundingAllocationSpreadsheets = GetNationalFundingAllocationMockFundingDocuments(
                fundingStreamCode,
                null,
                yearFrom,
                yearTo,
                documentCount,
                false);

            var controller = GetViewYourFundingController(
                securityServiceMock,
                fundingDocumentServiceMock,
                GetMockFundingApiService(string.Empty),
                userJourneyServiceMock,
                fundingViewServiceMock);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                        {
                            new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                        }, "someAuthTypeName"))
                }
            };

            var expectedViewModel = new NationalFundingAllocationViewModel
            {
                FundingStreamCode = fundingStreamCode,
                FundingStreamName = null,
                YearFrom = yearFrom,
                YearTo = yearTo,
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                ValidationError = true,
                ValidationErrorMessage = $"A problem occurred during processing. No funding stream configurations were found for funding stream '{fundingStreamCode}'.",
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = null,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                }
            };

            var actual = await controller.NationalFundingAllocation(fundingStreamCode, yearFrom, yearTo);

            // Assert
            actual.Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<NationalFundingAllocationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            userJourneyServiceMock.Verify();
        }

        /// <summary>
        /// National Funding Allocation with no spreadsheets returns expected result.
        /// </summary>
        /// <param name="fundingStreamCode">The code for the funding Stream.</param>
        /// <param name="fundingStreamName">The name for the funding stream.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <param name="yearFrom">The start year of the funding year.</param>
        /// <param name="yearTo">The end year of the funding year.</param>
        /// <param name="canUseShortCode">Can we use the short code (true for DSG, false for PSG).</param>
        /// <returns>Task.</returns>
        [DataRow(FundingStreamCode.DSG, DSGFundingStreamName, DSGFundingPeriodCode, TestFundingYearFrom, TestFundingYearTo, true)]
        [DataRow(FundingStreamCode.PEAndSport, PSGFundingStreamName, PSGFundingPeriodCode, TestFundingYearFrom - 1, TestFundingYearTo - 1, false)]
        [TestMethod, TestCategory("Unit")]
        public async Task NationalFundingAllocation_NoSpreadsheets_MatchesExpected(
            string fundingStreamCode,
            string fundingStreamName,
            string fundingPeriodCode,
            int yearFrom,
            int yearTo,
            bool canUseShortCode)
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();
            var fundingApiServiceMock = GetFundingApiService_SingleMatchingProviderFundingResult(user.Ukprn.ToString(), FundingStreamCode.DSG);
            var fundingViewServiceMock = GetNationalFundingAllocationMockFundingViewService(
                fundingStreamCode,
                fundingStreamName,
                fundingPeriodCode,
                yearFrom,
                yearTo);

            var fundingDocumentServiceMock = GetNationalFundingAllocationMockFundingDocumentServiceNoDocuments(
                fundingStreamCode,
                fundingStreamName,
                fundingPeriodCode);

            var controller = GetViewYourFundingController(
                securityServiceMock,
                fundingDocumentServiceMock,
                fundingApiServiceMock,
                settingsServiceMock,
                fundingViewServiceMock);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                    {
                                    new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                    }, "someAuthTypeName"))
                }
            };

            var expectedViewModel = new NationalFundingAllocationViewModel
            {
                FundingStreamCode = fundingStreamCode,
                FundingStreamName = fundingStreamName,
                YearFrom = yearFrom,
                YearTo = yearTo,
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                Spreadsheets = null,
                ValidationError = true,
                ValidationErrorMessage = $"A problem occurred during processing. No spreadsheets were found for funding stream code '{fundingStreamCode}', funding stream name '{fundingStreamName}' and funding period code '{fundingPeriodCode}'.",
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = ProviderNameFromProviderFunding,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                CanUseShortCode = canUseShortCode
            };

            // Act
            var actual = await controller.NationalFundingAllocation(fundingStreamCode, yearFrom, yearTo);

            // Assert
            actual.Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<NationalFundingAllocationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            fundingDocumentServiceMock.Verify();
        }

        /// <summary>
        /// National Funding Allocation with no publications returns expected result.
        /// </summary>
        /// <param name="documentCount">The number of documents to generate.</param>
        /// <param name="fundingStreamCode">The code for the funding Stream.</param>
        /// <param name="fundingStreamName">The name for the funding stream.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <param name="yearFrom">The start year of the funding year.</param>
        /// <param name="yearTo">The end year of the funding year.</param>
        /// <returns>Task.</returns>
        [DataRow(0, FundingStreamCode.DSG, DSGFundingStreamName, DSGFundingPeriodCode, TestFundingYearFrom, TestFundingYearTo)]
        [DataRow(0, FundingStreamCode.PEAndSport, PSGFundingStreamName, PSGFundingPeriodCode, TestFundingYearFrom - 1, TestFundingYearTo - 1)]
        [TestMethod, TestCategory("Unit")]
        public async Task NationalFundingAllocation_NoPublications_MatchesExpected(
            int documentCount,
            string fundingStreamCode,
            string fundingStreamName,
            string fundingPeriodCode,
            int yearFrom,
            int yearTo)
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var userJourneyServiceMock = GetMockUserJourneyServiceNoPublications();
            var fundingApiServiceMock = GetFundingApiService_SingleMatchingFundingResult(user.Ukprn.ToString(), FundingStreamCode.DSG);

            var fundingViewServiceMock = GetNationalFundingAllocationMockFundingViewService(
                fundingStreamCode,
                fundingStreamName,
                fundingPeriodCode,
                yearFrom,
                yearTo);

            var fundingDocumentServiceMock = GetNationalFundingAllocationMockFundingDocumentService(
                fundingStreamCode,
                fundingStreamName,
                fundingPeriodCode,
                yearFrom,
                yearTo,
                0);

            _nationalFundingAllocationSpreadsheets = GetNationalFundingAllocationMockFundingDocuments(
                fundingStreamCode,
                fundingStreamName,
                yearFrom,
                yearTo,
                documentCount,
                false);

            var controller = GetViewYourFundingController(
                securityServiceMock,
                fundingDocumentServiceMock,
                fundingApiServiceMock,
                userJourneyServiceMock,
                fundingViewServiceMock);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                    {
                                    new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                    }, "someAuthTypeName"))
                }
            };

            var expectedViewModel = new NationalFundingAllocationViewModel
            {
                FundingStreamCode = fundingStreamCode,
                FundingStreamName = fundingStreamName,
                YearFrom = yearFrom,
                YearTo = yearTo,
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                ValidationError = true,
                ValidationErrorMessage = $"A problem occurred during processing. No publications were found for funding stream '{fundingStreamCode}'.",
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = null,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                }
            };

            var actual = await controller.NationalFundingAllocation(fundingStreamCode, yearFrom, yearTo);

            // Assert
            actual.Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<NationalFundingAllocationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            userJourneyServiceMock.Verify();
        }

        /// <summary>
        /// National Funding Allocation with no matching spreadsheets for publications returns expected result.
        /// </summary>
        /// <param name="documentCount">The number of documents to generate.</param>
        /// <param name="fundingStreamCode">The code for the funding Stream.</param>
        /// <param name="fundingStreamName">The name for the funding stream.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <param name="yearFrom">The start year of the funding year.</param>
        /// <param name="yearTo">The end year of the funding year.</param>
        /// <returns>Task.</returns>
        [DataRow(1, FundingStreamCode.DSG, DSGFundingStreamName, DSGFundingPeriodCode, TestFundingYearFrom, TestFundingYearTo)]
        [DataRow(1, FundingStreamCode.PEAndSport, PSGFundingStreamName, PSGFundingPeriodCode, TestFundingYearFrom - 1, TestFundingYearTo - 1)]
        [TestMethod, TestCategory("Unit")]
        public async Task NationalFundingAllocation_NoMatchingSpreadSheetsForPublications_MatchesExpected(
            int documentCount,
            string fundingStreamCode,
            string fundingStreamName,
            string fundingPeriodCode,
            int yearFrom,
            int yearTo)
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var userJourneyServiceMock = GetMockUserJourneyServiceNoMatchingPublications();

            var fundingViewServiceMock = GetNationalFundingAllocationMockFundingViewService(
                fundingStreamCode,
                fundingStreamName,
                fundingPeriodCode,
                yearFrom,
                yearTo);

            var fundingDocumentServiceMock = GetNationalFundingAllocationMockFundingDocumentService(
                fundingStreamCode,
                fundingStreamName,
                fundingPeriodCode,
                yearFrom,
                yearTo,
                documentCount);

            _nationalFundingAllocationSpreadsheets = GetNationalFundingAllocationMockFundingDocuments(
                fundingStreamCode,
                fundingStreamName,
                yearFrom,
                yearTo,
                documentCount,
                false);

            var controller = GetViewYourFundingController(
                securityServiceMock,
                fundingDocumentServiceMock,
                GetFundingApiService_SingleMatchingProviderFundingResult(user.Ukprn.ToString(), FundingStreamCode.DSG),
                userJourneyServiceMock,
                fundingViewServiceMock);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                    {
                                    new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                    }, "someAuthTypeName"))
                }
            };

            var expectedViewModel = new NationalFundingAllocationViewModel
            {
                FundingStreamCode = fundingStreamCode,
                FundingStreamName = fundingStreamName,
                YearFrom = yearFrom,
                YearTo = yearTo,
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                Spreadsheets = null,
                ValidationError = true,
                ValidationErrorMessage = $"A problem occurred during processing. No Publications were found with a published date matching any of the Spreadsheet published dates for funding stream '{fundingStreamCode}'.",
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = ProviderNameFromProviderFunding,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                }
            };

            var actual = await controller.NationalFundingAllocation(fundingStreamCode, yearFrom, yearTo);

            // Assert
            actual.Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<NationalFundingAllocationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            userJourneyServiceMock.Verify();
        }

        /// <summary>
        /// National Funding Allocation no exception should be thrown for zero spreadsheets.
        /// </summary>
        /// <param name="fundingStreamCode">The code for the funding Stream.</param>
        /// <param name="fundingStreamName">The name for the funding stream.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <param name="yearFrom">The start year of the funding year.</param>
        /// <param name="yearTo">The end year of the funding year.</param>
        [DataRow(FundingStreamCode.DSG, DSGFundingStreamName, DSGFundingPeriodCode, TestFundingYearFrom, TestFundingYearTo)]
        [DataRow(FundingStreamCode.PEAndSport, PSGFundingStreamName, PSGFundingPeriodCode, TestFundingYearFrom - 1, TestFundingYearTo - 1)]
        [TestMethod, TestCategory("Unit")]
        public void NationalFundingAllocation_NoExceptionShouldBeThrown_ForZeroSpreadsheets(
            string fundingStreamCode,
            string fundingStreamName,
            string fundingPeriodCode,
            int yearFrom,
            int yearTo)
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();
            var fundingViewServiceMock = GetNationalFundingAllocationMockFundingViewService(
                fundingStreamCode,
                fundingStreamName,
                fundingPeriodCode,
                yearFrom,
                yearTo);

            var fundingDocumentServiceMock = GetMockFundingDocumentService(
                fundingStreamCode,
                fundingPeriodCode,
                yearFrom,
                yearTo,
                0);

            var controller = GetViewYourFundingController(
                securityServiceMock,
                fundingDocumentServiceMock,
                GetFundingApiService_SingleMatchingProviderFundingResult(user.Ukprn.ToString(), FundingStreamCode.DSG),
                settingsServiceMock,
                fundingViewServiceMock);

            // Act
            Func<Task> act = async () => await controller.NationalFundingAllocation(fundingStreamCode, yearFrom, yearTo);

            // Assert
            act.Should().NotThrowAsync<Exception>();
        }

        #endregion


        #region FindAnOrganisation action tests

        /// <summary>
        /// Finds an organisation view model matches expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task FindAnOrganisation_ViewModelMatchesExpected()
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var controller = GetViewYourFundingController(securityServiceMock, null, GetFundingApiService_SingleMatchingFundingResult(user.Ukprn?.ToString(), FundingStreamCode.PEAndSport), GetMockSettingsService());

            var expectedViewModel = new FindAnOrganisationViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = ProviderNameFromFunding,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink
            };

            // Act
            var actual = await controller.FindAnOrganisation();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FindAnOrganisationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        /// <summary>
        /// Finds an organisation view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task FindAnOrganisation_ViewModelMatchesExpected_NotLoggedIn()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(string.Empty);
            var settingsServiceMock = GetMockSettingsService();
            var controller = GetViewYourFundingController(securityServiceMock, null, fundingApiServiceMock, settingsServiceMock);

            var expectedViewModel = new FindAnOrganisationViewModel
            {
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                CurrentUser = new CurrentUserViewModel
                {
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                }
            };

            // Act
            var actual = await controller.FindAnOrganisation();

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FindAnOrganisationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        #endregion


        #region Provider search action tests

        /// <summary>
        /// Providers the results no results view model matches expected logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderSearch_NoResults_MatchesExpected_LoggedIn()
        {
            // Arrange
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, 0);
            var mockSettingsService = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();

            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);

            var controller = GetViewYourFundingController(
                securityServiceMock,
                mockDocumentService,
                mockFundingSearchService,
                mockSettingsService,
                mockFundingViewService);

            // Act
            var actual = await controller.ProviderSearch(SearchTerm);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ProviderNoResults);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the results no results view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderSearch_NoResults_MatchesExpected_NotLoggedIn()
        {
            // Arrange
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, 0);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService = GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            // Act
            var actual = await controller.ProviderSearch(SearchTerm);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ProviderNoResults);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the results one result view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderSearch_OneResult_MatchesExpected_NotLoggedIn()
        {
            // Arrange
            const int resultCount = 1;
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, resultCount);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                mockDocumentService,
                mockFundingSearchService,
                mockSettingsService,
                mockFundingViewService);

            // Act
            var actual = await controller.ProviderSearch(SearchTerm);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ProviderStatement);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the results empty search returns redirect to route.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderSearch_EmptySearch_ReturnsRedirectToRoute()
        {
            // Arrange
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, 0);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            // Act
            var actual = await controller.ProviderSearch(string.Empty);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_FindAnOrganisation);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("validationErrorInputId", "provider");
        }

        /// <summary>
        /// Providers the results many results view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderSearch_ManyResults_MatchesExpected_NotLoggedIn()
        {
            // Arrange
            const int resultCount = 10;
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, resultCount);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            // Act
            var actual = await controller.ProviderSearch(SearchTerm);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ProviderDidYouMean);
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain(SearchTerm, SearchTerm);

            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        #endregion


        #region Provider Did you mean action tests

        /// <summary>
        /// Providers the results no results view model matches expected logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderResults_NoResults_ViewModelMatchesExpected_LoggedIn()
        {
            // Arrange
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, 0);
            var mockSettingsService = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();

            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);

            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                mockDocumentService,
                mockFundingSearchService,
                mockSettingsService,
                mockFundingViewService);

            // Act
            var actual = await controller.ProviderDidYouMean(SearchTerm, new QueryFilterViewModel());

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ProviderNoResults);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the results no results view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderResults_NoResults_ViewModelMatchesExpected_NotLoggedIn()
        {
            // Arrange
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, 0);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService = GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            // Act
            var actual = await controller.ProviderDidYouMean(SearchTerm, new QueryFilterViewModel());

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ProviderNoResults);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the results one result view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderResults_OneResult_ViewModelMatchesExpected_NotLoggedIn()
        {
            // Arrange
            const int resultCount = 1;
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, resultCount);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                mockDocumentService,
                mockFundingSearchService,
                mockSettingsService,
                mockFundingViewService);

            // Act
            var actual = await controller.ProviderDidYouMean(SearchTerm, new QueryFilterViewModel());

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ProviderStatement);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the results empty search returns redirect to route.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderResults_EmptySearch_ReturnsRedirectToRoute()
        {
            // Arrange
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, 0);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            // Act
            var actual = await controller.ProviderDidYouMean(string.Empty, new QueryFilterViewModel());

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_FindAnOrganisation);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("validationErrorInputId", "provider");
        }

        /// <summary>
        /// Providers the results many results view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderResults_ManyResults_ViewModelMatchesExpected_NotLoggedIn()
        {
            // Arrange
            const int resultCount = 10;
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, resultCount);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();
            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            var expectedViewModel = new ProviderResultsViewModel
            {
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                CurrentUser = new CurrentUserViewModel
                {
                    FirstName = NotLoggedInUser.FirstName,
                    LastName = NotLoggedInUser.LastName,
                    FullName = NotLoggedInUser.FullName
                },
                ProviderResults = GetMockProviderResults(resultCount).ToList(),
                SearchTerm = SearchTerm,
                BackToTopLinkMinimumCount = 25,
                QueryFilterViewModel = new QueryFilterViewModel
                {
                    SearchTerm = SearchTerm,
                    QueryFilter = QueryFilterHelper.BuildQueryFilter(GetMockProviderResults(resultCount).ToList()),
                    RouteName = ViewYourFundingConstants.RouteName_ProviderDidYouMean
                }
            };

            var appliedFilterViewModel = new QueryFilterViewModel
            {
                SearchTerm = SearchTerm
            };

            // Act
            var actual = await controller.ProviderDidYouMean(SearchTerm, appliedFilterViewModel);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderResultsViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        #endregion


        #region Provider Filter Results action tests

        /// <summary>
        /// Providers the filter results many results no filter view model matches expected logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderFilterResults_ManyResults_NoFilter_ViewModelMatchesExpected_LoggedIn()
        {
            // Arrange
            const int resultCount = 10;
            var securityServiceMock = GetMockSecurityService(LoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, resultCount);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();
            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            var expectedViewModel = GetExpectedProviderResultsViewModel(LoggedInUser, resultCount);

            var appliedFilterViewModel = new QueryFilterViewModel
            {
                SearchTerm = SearchTerm,
                RouteName = ViewYourFundingConstants.RouteName_ProviderDidYouMean
            };

            // Act
            var actual = await controller.ProviderDidYouMean(SearchTerm, appliedFilterViewModel);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderResultsViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Exactly(3));
        }

        /// <summary>
        /// Providers the filter results many results no filter view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderFilterResults_ManyResults_NoFilter_ViewModelMatchesExpected_NotLoggedIn()
        {
            // Arrange
            const int resultCount = 10;
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, resultCount);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();
            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            var expectedViewModel = GetExpectedProviderResultsViewModel(NotLoggedInUser, resultCount);
            var appliedFilterViewModel = new QueryFilterViewModel
            {
                SearchTerm = SearchTerm,
                RouteName = ViewYourFundingConstants.RouteName_ProviderDidYouMean
            };

            // Act
            var actual = await controller.ProviderDidYouMean(SearchTerm, appliedFilterViewModel);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderResultsViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the filter results many results null filter view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderFilterResults_ManyResults_NullFilter_ViewModelMatchesExpected_NotLoggedIn()
        {
            // Arrange
            const int resultCount = 10;
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, resultCount);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();
            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
                 securityServiceMock,
                 mockDocumentService,
                 mockFundingSearchService,
                 mockSettingsService,
                 mockFundingViewService);

            var expectedViewModel = GetExpectedProviderResultsViewModel(NotLoggedInUser, resultCount);
            var appliedFilterViewModel = new QueryFilterViewModel
            {
                SearchTerm = SearchTerm,
                QueryFilter = null,
                RouteName = ViewYourFundingConstants.RouteName_ProviderDidYouMean
            };

            // Act
            var actual = await controller.ProviderDidYouMean(SearchTerm, appliedFilterViewModel);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderResultsViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the filter results many results filters view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderFilterResults_ManyResults_Filters_ViewModelMatchesExpected_NotLoggedIn()
        {
            // Arrange
            const int resultCount = 10;
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, resultCount);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();
            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            var expectedViewModel = GetExpectedProviderResultsViewModel(NotLoggedInUser, resultCount);

            var appliedFilterViewModel = new QueryFilterViewModel
            {
                QueryFilter = GetAppliedQueryFilter(),
                SearchTerm = SearchTerm,
                RouteName = ViewYourFundingConstants.RouteName_ProviderDidYouMean
            };

            expectedViewModel.QueryFilterViewModel.QueryFilter.Merge(appliedFilterViewModel.QueryFilter);

            // Act
            var actual = await controller.ProviderDidYouMean(SearchTerm, appliedFilterViewModel);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderResultsViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the filter results many results filters view model matches expected logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderFilterResults_ManyResults_Filters_ViewModelMatchesExpected_LoggedIn()
        {
            // Arrange
            const int resultCount = 10;
            var securityServiceMock = GetMockSecurityService(LoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, resultCount);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();
            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            var expectedViewModel = GetExpectedProviderResultsViewModel(LoggedInUser, resultCount);

            var appliedFilterViewModel = new QueryFilterViewModel
            {
                QueryFilter = GetAppliedQueryFilter(),
                SearchTerm = SearchTerm,
                RouteName = ViewYourFundingConstants.RouteName_ProviderDidYouMean
            };

            expectedViewModel.QueryFilterViewModel.QueryFilter.Merge(appliedFilterViewModel.QueryFilter);

            // Act
            var actual = await controller.ProviderDidYouMean(SearchTerm, appliedFilterViewModel);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderResultsViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Exactly(3));
        }

        #endregion


        #region ProviderNoResults action tests

        /// <summary>
        /// Providers the no results no results view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderNoResults_NoResults_ViewModelMatchesExpected_NotLoggedIn()
        {
            // Arrange
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, 0);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                mockDocumentService,
                mockFundingSearchService,
                mockSettingsService,
                mockFundingViewService);


            var expectedViewModel = new ProviderNoResultsViewModel
            {
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                CurrentUser = new CurrentUserViewModel
                {
                    FirstName = NotLoggedInUser.FirstName,
                    LastName = NotLoggedInUser.LastName,
                    FullName = NotLoggedInUser.FullName
                },
                SearchTerm = SearchTerm
            };

            // Act
            var actual = await controller.ProviderNoResults(SearchTerm);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderNoResultsViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        #endregion


        #region ProviderStatement action tests

        /// <summary>
        /// Providers the statement no results view model matches expected logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderStatement_NoResults_ViewModelMatchesExpected_LoggedIn()
        {
            // Arrange
            const int resultCount = 0;
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, resultCount);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            var actual = await controller.ProviderStatement(OrganisationUkprn);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ProviderDidYouMean);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the statement no results view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderStatement_NoResults_ViewModelMatchesExpected_NotLoggedIn()
        {
            // Arrange
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, 0);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            // Act
            var actual = await controller.ProviderStatement(OrganisationUkprn);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ProviderDidYouMean);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the statement one result view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderStatement_OneResult_ViewModelMatchesExpected_NotLoggedIn()
        {
            // Arrange
            const int resultCount = 2;
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, resultCount);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            var expectedViewModel = GetExpectedProviderStatementViewModel(NotLoggedInUser);

            // Act
            var actual = await controller.ProviderStatement(FirstOrganisationUkprn);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the statement one result view model matches expected academies not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderStatement_OneResult_ViewModelMatchesExpected_Academies_NotLoggedIn()
        {
            // Arrange
            const int resultCount = 2;
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(
                SearchTerm,
                0,
                resultCount,
                providerType: "Academies");
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);

            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
              securityServiceMock,
              mockDocumentService,
              mockFundingSearchService,
              mockSettingsService,
              mockFundingViewService);

            var expectedViewModel = GetExpectedProviderStatementViewModel(
                NotLoggedInUser,
                paymentDateSettingName: SettingName.NextAllocationPaymentDateAcademies);

            // Act
            var actual = await controller.ProviderStatement(FirstOrganisationUkprn);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the statement one result view model matches expected NMSS not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderStatement_OneResult_ViewModelMatchesExpected_NMSS_NotLoggedIn()
        {
            // Arrange
            const int resultCount = 2;
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(
                SearchTerm,
                0,
                resultCount,
                providerType: "NMSS");
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            var expectedViewModel = GetExpectedProviderStatementViewModel(
                NotLoggedInUser,
                paymentDateSettingName: SettingName.NextAllocationPaymentDateNMSS);

            // Act
            var actual = await controller.ProviderStatement(FirstOrganisationUkprn);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the statement one result with search term view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderStatement_OneResult_WithSearchTerm_ViewModelMatchesExpected_NotLoggedIn()
        {
            // Arrange
            const int resultCount = 2;
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, resultCount);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                mockDocumentService,
                mockFundingSearchService,
                mockSettingsService,
                mockFundingViewService);

            var expectedViewModel = GetExpectedProviderStatementViewModel(NotLoggedInUser, SearchTerm);

            // Act
            var actual = await controller.ProviderStatement(FirstOrganisationUkprn, SearchTerm);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        /// <summary>
        /// Providers the statement one result view model matches expected logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderStatement_OneResult_ViewModelMatchesExpected_LoggedIn()
        {
            // Arrange
            const int resultCount = 2;
            var securityServiceMock = GetMockSecurityService(LoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, resultCount);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                mockDocumentService,
                mockFundingSearchService,
                mockSettingsService,
                mockFundingViewService);

            var expectedViewModel = GetExpectedProviderStatementViewModel(LoggedInUser);

            // Act
            var actual = await controller.ProviderStatement(FirstOrganisationUkprn);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Exactly(3));
        }

        /// <summary>
        /// Providers the statement empty search view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderStatement_EmptySearch_ViewModelMatchesExpected_NotLoggedIn()
        {
            // Arrange
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, 0);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
                 securityServiceMock,
                 mockDocumentService,
                 mockFundingSearchService,
                 mockSettingsService,
                 mockFundingViewService);

            // Act
            var actual = await controller.ProviderStatement(string.Empty);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ProviderDidYouMean);
            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Never);
        }

        /// <summary>
        /// Providers the statement many results view model matches expected not logged in.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderStatement_ManyResults_ViewModelMatchesExpected_NotLoggedIn()
        {
            // Arrange
            const int resultCount = 2;
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, resultCount);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               mockDocumentService,
               mockFundingSearchService,
               mockSettingsService,
               mockFundingViewService);

            var expectedViewModel = GetExpectedProviderStatementViewModel(NotLoggedInUser);

            // Act
            var actual = await controller.ProviderStatement(FirstOrganisationUkprn);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            mockFundingSearchService.Verify(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
        }

        #endregion


        #region LocalAuthoritySearch action tests

        /// <summary>
        /// Locals the authority search no results.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthoritySearch_NoResults()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(TestSearchTerm, 0);
            var settingsServiceMock = GetMockSettingsService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                fundingApiServiceMock,
                settingsServiceMock,
                null);

            // Act
            var actual = await controller.LocalAuthoritySearch(TestSearchTerm);

            // Assert
            fundingApiServiceMock.Verify();
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_LocalAuthorityNoResults);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("searchTerm", TestSearchTerm);
        }

        /// <summary>
        /// Locals the authority search single result.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthoritySearch_SingleResult()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(TestSearchTerm, 1);
            var settingsServiceMock = GetMockSettingsService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               null,
               fundingApiServiceMock,
               settingsServiceMock,
               null);

            // Act
            var actual = await controller.LocalAuthoritySearch(TestSearchTerm);

            // Assert
            fundingApiServiceMock.Verify();
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_LocalAuthorityStatement);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("localAuthorityCode", "0");
        }

        /// <summary>
        /// Locals the authority search multiple results.
        /// </summary>
        /// <param name="resultCount">The result count.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(100)]
        [DataRow(999)]
        public async Task LocalAuthoritySearch_MultipleResults(int resultCount)
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(TestSearchTerm, resultCount);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                fundingApiServiceMock,
                settingsServiceMock,
                null);

            // Act
            var actual = await controller.LocalAuthoritySearch(TestSearchTerm);

            // Assert
            fundingApiServiceMock.Verify();
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_LocalAuthorityDidYouMean)
                ;

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("searchTerm", TestSearchTerm);
        }

        /// <summary>
        /// Las the search results empty search returns redirect to route.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LaSearchResults_EmptySearch_ReturnsRedirectToRoute()
        {
            // Arrange
            var securityServiceMock = GetMockSecurityService(NotLoggedInUser);
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, 0, 0);
            var mockSettingsService = GetMockSettingsService();
            var mockDocumentService =
                GetMockFundingDocumentService(FundingStreamCode.PEAndSport, PSGFundingPeriodCode, 2019, 2020, 1);
            var mockFundingViewService = GetMockFundingViewService();
            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                mockDocumentService,
                mockFundingSearchService,
                mockSettingsService,
                mockFundingViewService);

            // Act
            var actual = await controller.LocalAuthoritySearch(string.Empty);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_FindAnOrganisation);
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("validationErrorInputId", "laSearch");
        }

        #endregion


        #region LocalAuthorityNoResults action tests

        /// <summary>
        /// Locals the authority no results view model matches expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityNoResults_ViewModelMatchesExpected()
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var mockSettingsService = GetMockSettingsService();
            var mockFundingApiService = GetFundingApiService_SingleMatchingFundingResult(user.Ukprn.ToString(), FundingStreamCode.PEAndSport);

            var controller = GetViewYourFundingController(securityServiceMock, null, mockFundingApiService, mockSettingsService);

            var expectedViewModel = new LocalAuthorityNoResultsViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = ProviderNameFromFunding,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                SearchTerm = TestSearchTerm
            };

            // Act
            var actual = await controller.LocalAuthorityNoResults(TestSearchTerm);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityNoResultsViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        #endregion


        #region LocalAuthorityDidYouMean action tests

        /// <summary>
        /// Locals the authority did you mean when multiple results view model matches expected.
        /// </summary>
        /// <param name="resultCount">The result count.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(100)]
        [DataRow(999)]
        public async Task LocalAuthorityDidYouMean_WhenMultipleResults_ViewModelMatchesExpected(int resultCount)
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(TestSearchTerm, resultCount);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();
            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                fundingApiServiceMock,
                settingsServiceMock,
                mockFundingViewService);

            var expectedViewModel = new LocalAuthorityDidYouMeanViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                BackToTopLinkMinimumCount = 25,
                ContactUsLink = TestContactUsLink,
                SearchTerm = TestSearchTerm,
                LocalAuthorities = Enumerable.Range(0, resultCount)
                            .ToDictionary(k => k.ToString(), k => k.ToString()).ToList()
            };

            // Act
            var actual = await controller.LocalAuthorityDidYouMean(TestSearchTerm);

            // Assert
            fundingApiServiceMock.Verify();
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityDidYouMeanViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        /// <summary>
        /// Locals the authority did you mean when no results redirects to no results.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityDidYouMean_WhenNoResults_RedirectsToNoResults()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(TestSearchTerm, 0);
            var settingsServiceMock = GetMockSettingsService();
            var fundingViewServiceMock = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                fundingApiServiceMock,
                settingsServiceMock,
                fundingViewServiceMock);

            // Act
            var actual = await controller.LocalAuthorityDidYouMean(TestSearchTerm);

            // Assert
            fundingApiServiceMock.Verify();
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_LocalAuthorityNoResults);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("searchTerm", TestSearchTerm);
        }

        /// <summary>
        /// Locals the authority did you mean when single result redirects to statement.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityDidYouMean_WhenSingleResult_RedirectsToStatement()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(TestSearchTerm, 1);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                fundingApiServiceMock,
                settingsServiceMock,
                mockFundingViewService);

            // Act
            var actual = await controller.LocalAuthorityDidYouMean(TestSearchTerm);

            // Assert
            fundingApiServiceMock.Verify();
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_LocalAuthorityStatement);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("localAuthorityCode", "0");
        }

        #endregion


        #region Provider PSG Funding breakdown action tests

        /// <summary>
        /// Providers the PSG funding breakdown view model matches expected.
        /// </summary>
        /// <param name="organisationUkprn">The organisation ukprn.</param>
        /// <param name="resultCount">The result count.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        [DataRow(OrganisationUkprn, 1)]
        [DataRow(OrganisationUkprn, 2)]
        [DataRow(OrganisationUkprn, 3)]
        public async Task ProviderPSGFundingBreakdown_ViewModelMatchesExpected(string organisationUkprn, int resultCount)
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, null, resultCount);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                mockFundingSearchService,
                settingsServiceMock,
                mockFundingViewService);

            var expectedViewModel = new ProviderFundingBreakdownViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = null,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                YearFrom = 2019,
                YearTo = 2020,
                FundingStreamName = PSGFundingStreamName,
                FundingStatus = FundingBreakdownStatus.Latest,
                NotLatestCssClass = string.Empty,
                ProviderStatementSection = GetProviderStatementSectionViewModel(1, true).First(),
                PublicationDate = TestPublicationDate,
                FundingViewData = new FundingViewData
                {
                    EntityName = "OrganisationName",
                    PublicationDate = new DateTime(2019, 12, 31),
                    TotalAmount = 12345678M,
                    FundingValues = new Dictionary<string, object>(),
                    FundingSubData = new List<IFundingApiSearchProviderFunding>()
                }
            };

            // Mapster converts null collections to empty collections
            if (expectedViewModel.ProviderStatementSection?.FundingStreamConfiguration?.Publications != null)
            {
                foreach (var publication in expectedViewModel.ProviderStatementSection.FundingStreamConfiguration.Publications)
                {
                    publication.PublicationLayouts ??= new List<PublicationLayout>();
                }
            }

            if (expectedViewModel.ProviderStatementSection?.FundingStreamConfiguration?.SettingValues != null)
            {
                foreach (var settingValue in expectedViewModel.ProviderStatementSection.FundingStreamConfiguration.SettingValues)
                {
                    if (settingValue.Setting != null)
                    {
                        settingValue.Setting.SettingValues ??= new List<SettingValue>();
                    }
                }
            }

            // Act
            var actual = await controller.ProviderFundingBreakdown(
                "pe-and-sport-premium",
                FirstOrganisationUkprn,
                DateTimeExtensions.ToRouteParameterString(TestPublicationDate),
                2019,
                2020);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderFundingBreakdownViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(option => option.ProviderStatementSection.FundingStreamConfiguration.SettingValues[0].CreatedAt)
                        .Excluding(option => option.ProviderStatementSection.FundingStreamConfiguration.SettingValues[0].LastUpdatedAt));

            mockFundingSearchService.Verify(
                x => x.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false),
                Times.Exactly(3));
        }

        /// <summary>
        /// Providers the PSG funding breakdown with search term view model matches expected.
        /// </summary>
        /// <param name="organisationUkprn">The organisation ukprn.</param>
        /// <param name="resultCount">The result count.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        [DataRow(OrganisationUkprn, 1)]
        [DataRow(OrganisationUkprn, 2)]
        [DataRow(OrganisationUkprn, 3)]
        public async Task ProviderPSGFundingBreakdown_WithSearchTerm_ViewModelMatchesExpected(string organisationUkprn, int resultCount)
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, null, resultCount);
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
                 securityServiceMock,
                 null,
                 mockFundingSearchService,
                 settingsServiceMock,
                 mockFundingViewService);

            var expectedViewModel = new ProviderFundingBreakdownViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = null,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                YearFrom = 2019,
                YearTo = 2020,
                FundingStreamName = PSGFundingStreamName,
                FundingStatus = FundingBreakdownStatus.Latest,
                NotLatestCssClass = string.Empty,
                SearchTerm = SearchTerm,
                ProviderStatementSection = GetProviderStatementSectionViewModel(1, true, SearchTerm).First(),
                PublicationDate = TestPublicationDate,
                FundingViewData = new FundingViewData
                {
                    EntityName = "OrganisationName",
                    PublicationDate = new DateTime(2019, 12, 31),
                    TotalAmount = 12345678M,
                    FundingValues = new Dictionary<string, object>(),
                    FundingSubData = new List<IFundingApiSearchProviderFunding>()
                }
            };

            // Act
            var actual = await controller.ProviderFundingBreakdown(
                "pe-and-sport-premium",
                FirstOrganisationUkprn,
                DateTimeExtensions.ToRouteParameterString(TestPublicationDate),
                2019,
                2020,
                SearchTerm);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderFundingBreakdownViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(info =>
                            info.Path.EndsWith("PublicationLayouts"))
                        .Excluding(info =>
                            info.Path.EndsWith("Setting.SettingValues"))
                        .Excluding(option =>
                            option.ProviderStatementSection.FundingStreamConfiguration.SettingValues[0].CreatedAt)
                        .Excluding(option =>
                            option.ProviderStatementSection.FundingStreamConfiguration.SettingValues[0].LastUpdatedAt));

            mockFundingSearchService.Verify(
                x => x.SearchProviderFunding(
                    It.IsAny<FundingApiSearchRequestObject>(),
                    false),
                Times.Exactly(3));
        }

        /// <summary>
        /// Providers the PSG funding breakdown redirect expected for empty or null or invalid organisation uk PRN.
        /// </summary>
        /// <param name="documentCount">The document count.</param>
        /// <param name="organisationUkprn">The organisation ukprn.</param>
        /// <param name="resultCount">The result count.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        [DataRow(0, "", 0)]
        [DataRow(1, null, 0)]
        [DataRow(10, "52332532", 0)]
        public async Task ProviderPSGFundingBreakdown_RedirectExpected_ForEmptyOrNullOrInvalid_OrganisationUkPrn(int documentCount, string organisationUkprn, int resultCount)
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingDocumentServiceMock = GetMockFundingDocumentService(
                FundingStreamCode.PEAndSport, "AY-1920", 2019, 2020, documentCount);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, null, resultCount);

            var controller = GetViewYourFundingController(
                 securityServiceMock,
                 null,
                 mockFundingSearchService,
                 settingsServiceMock,
                 null);

            // Act
            var actual = await controller.ProviderFundingBreakdown("pe-and-sport-premium", organisationUkprn, DateTimeExtensions.ToRouteParameterString(TestPublicationDate), 2019, 2020);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ProviderDidYouMean);
            if (!string.IsNullOrWhiteSpace(organisationUkprn))
            {
                mockFundingSearchService.Verify(x => x.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
            }
        }

        #endregion


        #region Provider PSG 2018 Funding Breakdown allocation action tests

        /// <summary>
        /// Providers the PSG funding breakdown201819 view model matches expected.
        /// </summary>
        /// <param name="documentCount">The document count.</param>
        /// <param name="organisationUkprn">The organisation ukprn.</param>
        /// <param name="resultCount">The result count.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        [DataRow(1, OrganisationUkprn, 1)]
        [DataRow(10, OrganisationUkprn, 1)]
        public async Task ProviderPSGFundingBreakdown201819_ViewModelMatchesExpected(int documentCount, string organisationUkprn, int resultCount)
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var mockFundingViewService = GetMockFundingViewService();

            var settingsServiceMock = GetMockSettingsService();
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, null, resultCount);
            var fundingDocumentServiceMock = GetMockFundingDocumentService(
                FundingStreamCode.PEAndSport, "AY-1819", 2018, 2019, documentCount);

            var controller = GetViewYourFundingController(
                securityServiceMock,
                fundingDocumentServiceMock,
                mockFundingSearchService,
                settingsServiceMock,
                mockFundingViewService);

            var expectedViewModel = new ProviderHistorySingleYearViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = null,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                OrganisationName = OrganisationName,
                OrganisationUkprn = FirstOrganisationUkprn,
                FundingStreamName = "PE and sport premium",
                YearFrom = 2018,
                YearTo = 2019,
                LatestYearFrom = 2019,
                LatestYearTo = 2020,
                FundingStreamCode = "PSG",
                FundingViewData = new FundingViewData
                {
                    EntityName = "OrganisationName",
                    PublicationDate = new DateTime(2019, 12, 31),
                    TotalAmount = 12345678M,
                    FundingSubData = new List<IFundingApiSearchProviderFunding>()
                }
            };

            // Act
            var actual = await controller.ProviderHistorySingleYear("pe-and-sport-premium", 2018, 2019, FirstOrganisationUkprn);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderHistorySingleYearViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel, options => options.Excluding(model => model.Spreadsheets));

            mockFundingSearchService.Verify(x => x.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Exactly(3));
        }

        /// <summary>
        /// Providers the PSG funding breakdown201819 with search term view model matches expected.
        /// </summary>
        /// <param name="documentCount">The document count.</param>
        /// <param name="organisationUkprn">The organisation ukprn.</param>
        /// <param name="resultCount">The result count.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        [DataRow(1, OrganisationUkprn, 1)]
        [DataRow(10, OrganisationUkprn, 1)]
        public async Task ProviderPSGFundingBreakdown201819_WithSearchTerm_ViewModelMatchesExpected(int documentCount, string organisationUkprn, int resultCount)
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var mockFundingViewService = GetMockFundingViewService();

            var settingsServiceMock = GetMockSettingsService();
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, null, resultCount);
            var fundingDocumentServiceMock = GetMockFundingDocumentService(
                FundingStreamCode.PEAndSport, "AY-1819", 2018, 2019, documentCount);

            var controller = GetViewYourFundingController(
                securityServiceMock,
                fundingDocumentServiceMock,
                mockFundingSearchService,
                settingsServiceMock,
                mockFundingViewService);

            var expectedViewModel = new ProviderHistorySingleYearViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = null,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                OrganisationName = OrganisationName,
                OrganisationUkprn = FirstOrganisationUkprn,
                FundingStreamName = "PE and sport premium",
                FundingStreamCode = "PSG",
                YearFrom = 2018,
                YearTo = 2019,
                LatestYearFrom = 2019,
                LatestYearTo = 2020,
                SearchTerm = SearchTerm,
                FundingViewData = new FundingViewData
                {
                    EntityName = "OrganisationName",
                    PublicationDate = new DateTime(2019, 12, 31),
                    TotalAmount = 12345678M,
                    FundingSubData = new List<IFundingApiSearchProviderFunding>()
                }
            };

            // Act
            var actual = await controller.ProviderHistorySingleYear("pe-and-sport-premium", 2018, 2019, FirstOrganisationUkprn, SearchTerm);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderHistorySingleYearViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel, options => options.Excluding(model => model.Spreadsheets));

            mockFundingSearchService.Verify(x => x.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Exactly(3));
        }

        /// <summary>
        /// Providers the PSG funding breakdown201819 redirect expected for empty or null or invalid organisation uk PRN.
        /// </summary>
        /// <param name="documentCount">The document count.</param>
        /// <param name="organisationUkprn">The organisation ukprn.</param>
        /// <param name="resultCount">The result count.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        [DataRow(0, "", 0)]
        [DataRow(1, null, 0)]
        [DataRow(10, "52332532", 0)]
        public async Task ProviderPSGFundingBreakdown201819_RedirectExpected_ForEmptyOrNullOrInvalid_OrganisationUkPrn(int documentCount, string organisationUkprn, int resultCount)
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingDocumentServiceMock = GetMockFundingDocumentService(
                FundingStreamCode.PEAndSport, "AY-1920", 2019, 2020, documentCount);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingSearchService = GetMockFundingApiService(SearchTerm, null, resultCount);

            var controller = GetViewYourFundingController(
                securityServiceMock,
                fundingDocumentServiceMock,
                mockFundingSearchService,
                settingsServiceMock,
                null);

            // Act
            var actual = await controller.ProviderHistorySingleYear("pe-and-sport-premium", 2018, 2019, organisationUkprn);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ProviderDidYouMean);
            if (!string.IsNullOrWhiteSpace(organisationUkprn))
            {
                mockFundingSearchService.Verify(x => x.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false), Times.Once);
            }
        }

        #endregion


        #region Provider PSG Allocation History action tests

        /// <summary>
        /// Providers the PSG allocation history view model matches expected.
        /// </summary>
        /// <param name="organisationUkprn">The organisation ukprn.</param>
        /// <param name="resultCount">The result count.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        [DataRow(OrganisationUkprn, 1)]
        [DataRow(OrganisationUkprn, 2)]
        [DataRow(OrganisationUkprn, 3)]
        public async Task ProviderPSGAllocationHistory_ViewModelMatchesExpected(string organisationUkprn, int resultCount)
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();
            var fundingApiService = GetMockFundingApiService(string.Empty, providerSearchResultCount: 1);

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                fundingApiService,
                settingsServiceMock,
                mockFundingViewService);

            var expectedViewModel = new ProviderHistoryViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                OrganisationUkprn = OrganisationUkprn,
                OrganisationName = OrganisationName,
                FundingStreamConfiguration = ExpectedFundingStreamConfiguration[FundingStreamCode.PEAndSport],
                SecondaryContentTitle = ExpectedFundingStreamConfiguration[FundingStreamCode.PEAndSport].FundingStreamName,
                FundingPeriodPublications = GetExpectedPSGPublications(),
                FundingViewData = new FundingViewData
                {
                    EntityName = "OrganisationName",
                    PublicationDate = new DateTime(2019, 12, 31),
                    TotalAmount = 12345678M,
                    FundingSubData = new List<IFundingApiSearchProviderFunding>()
                }
            };

            // Act
            var actual = await controller.ProviderHistory(
                "pe-and-sport-premium",
                organisationUkprn);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderHistoryViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(info => info.Path.EndsWith("PublicationLayouts"))
                        .Excluding(info => info.Path.EndsWith("Setting.SettingValues"))
                        .Excluding(info => info.Path.EndsWith("CurrentUser.ProviderName"))
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].CreatedAt)
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].LastUpdatedAt));
        }

        /// <summary>
        /// Providers the PSG allocation history with search term view model matches expected.
        /// </summary>
        /// <param name="organisationUkprn">The organisation ukprn.</param>
        /// <param name="resultCount">The result count.</param>
        [TestMethod, TestCategory("Unit")]
        [DataRow(OrganisationUkprn, 1)]
        [DataRow(OrganisationUkprn, 2)]
        [DataRow(OrganisationUkprn, 3)]
        public async Task ProviderPSGAllocationHistory_WithSearchTerm_ViewModelMatchesExpected(string organisationUkprn, int resultCount)
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();
            var fundingApiService = GetMockFundingApiService(string.Empty, providerSearchResultCount: 1);

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                fundingApiService,
                settingsServiceMock,
                mockFundingViewService);

            var expectedViewModel = new ProviderHistoryViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = null,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                OrganisationUkprn = organisationUkprn,
                OrganisationName = OrganisationName,
                SearchTerm = SearchTerm,
                FundingStreamConfiguration = ExpectedFundingStreamConfiguration[FundingStreamCode.PEAndSport],
                SecondaryContentTitle = ExpectedFundingStreamConfiguration[FundingStreamCode.PEAndSport].FundingStreamName,
                FundingPeriodPublications = GetExpectedPSGPublications(),
                FundingViewData = new FundingViewData
                {
                    EntityName = "OrganisationName",
                    PublicationDate = new DateTime(2019, 12, 31),
                    TotalAmount = 12345678M,
                    FundingSubData = new List<IFundingApiSearchProviderFunding>()
                }
            };

            // Mapster converts null collections to empty collections
            if (expectedViewModel.FundingStreamConfiguration?.Publications != null)
            {
                foreach (var publication in expectedViewModel.FundingStreamConfiguration.Publications)
                {
                    publication.PublicationLayouts ??= new List<PublicationLayout>();
                }
            }

            if (expectedViewModel.FundingStreamConfiguration?.SettingValues != null)
            {
                foreach (var settingValue in expectedViewModel.FundingStreamConfiguration.SettingValues)
                {
                    if (settingValue.Setting != null)
                    {
                        settingValue.Setting.SettingValues ??= new List<SettingValue>();
                    }
                }
            }

            // Act
            var actual = await controller.ProviderHistory(
                "pe-and-sport-premium",
                organisationUkprn,
                SearchTerm);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderHistoryViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].CreatedAt)
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].LastUpdatedAt));
        }

        /// <summary>
        /// Providers the PSG allocation history for empty or null organisation uk PRN redirect expected.
        /// </summary>
        /// <param name="organisationUkprn">The organisation ukprn.</param>
        /// <param name="resultCount">The result count.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        [DataRow("", 0)]
        [DataRow(null, 0)]
        public async Task ProviderPSGAllocationHistory_ForEmptyOrNullOrganisationUkPrn_RedirectExpected(string organisationUkprn, int resultCount)
        {
            // Arrange
            var user = LoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                null,
                settingsServiceMock,
                mockFundingViewService);

            // Act
            var actual = await controller.ProviderHistory("pe-and-sport-premium", organisationUkprn);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_ProviderDidYouMean);
        }

        #endregion


        #region LocalAuthorityStatement action tests

        /// <summary>
        /// Locals the authority statement when not accessed from search view model matches expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityStatement_WhenNotAccessedFromSearch_ViewModelMatchesExpected()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(TestSearchTerm);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();
            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
                 securityServiceMock,
                 null,
                 fundingApiServiceMock,
                 settingsServiceMock,
                 mockFundingViewService);

            var expectedViewModel = new LocalAuthorityStatementViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                LocalAuthorityCode = TestLocalAuthorityCode,
                LocalAuthorityName = TestLocalAuthorityName,
                FundingViewData = ExpectedLocalAuthorityFundingData(TestLocalAuthorityCode),
                FundingStreamConfiguration = ExpectedFundingStreamConfiguration
            };

            // Act
            var actual = await controller.LocalAuthorityStatement(TestLocalAuthorityCode);

            // Assert
            fundingApiServiceMock.Verify();

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityStatementViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(vm => vm.FundingDocuments)
                        .Excluding(info => info.Path.EndsWith("PublicationLayouts"))
                        .Excluding(info => info.Path.EndsWith("Setting.SettingValues"))
                        .Excluding(option => option.FundingStreamConfiguration["DSG"].SettingValues[0].CreatedAt)
                        .Excluding(option => option.FundingStreamConfiguration["DSG"].SettingValues[0].LastUpdatedAt)
                        .Excluding(option => option.FundingStreamConfiguration["PSG"].SettingValues[0].CreatedAt)
                        .Excluding(option => option.FundingStreamConfiguration["PSG"].SettingValues[0].LastUpdatedAt));

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityStatementViewModel>()
                .Which.FundingDocuments.Should().HaveCount(2);
        }

        /// <summary>
        /// Locals the authority statement when accessed from search view model matches expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityStatement_WhenAccessedFromSearch_ViewModelMatchesExpected()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(TestSearchTerm);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();

            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                fundingApiServiceMock,
                settingsServiceMock,
                mockFundingViewService);

            var expectedViewModel = new LocalAuthorityStatementViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                LocalAuthorityCode = TestLocalAuthorityCode,
                LocalAuthorityName = TestLocalAuthorityName,
                FundingViewData = ExpectedLocalAuthorityFundingData(TestLocalAuthorityCode),
                FundingStreamConfiguration = ExpectedFundingStreamConfiguration,
                SearchTerm = TestSearchTerm
            };

            // Act
            var actual = await controller.LocalAuthorityStatement(
                TestLocalAuthorityCode,
                TestSearchTerm);

            // Assert
            fundingApiServiceMock.Verify();

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityStatementViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(vm => vm.FundingDocuments)
                        .Excluding(info => info.Path.EndsWith("PublicationLayouts"))
                        .Excluding(info => info.Path.EndsWith("Setting.SettingValues"))
                        .Excluding(option => option.FundingStreamConfiguration["DSG"].SettingValues[0].CreatedAt)
                        .Excluding(option => option.FundingStreamConfiguration["DSG"].SettingValues[0].LastUpdatedAt)
                        .Excluding(option => option.FundingStreamConfiguration["PSG"].SettingValues[0].CreatedAt)
                        .Excluding(option => option.FundingStreamConfiguration["PSG"].SettingValues[0].LastUpdatedAt));

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityStatementViewModel>()
                .Which.FundingDocuments.Should().HaveCount(2);
        }

        #endregion


        #region LocalAuthorityPSGBreakdown action tests

        /// <summary>
        /// Locals the authority PSG breakdown when not accessed from search view model matches expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityPSGBreakdown_WhenNotAccessedFromSearch_ViewModelMatchesExpected()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(TestSearchTerm);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();
            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                fundingApiServiceMock,
                settingsServiceMock,
                mockFundingViewService);

            var expectedViewModel = new FundingBreakdownViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                LocalAuthorityCode = TestLocalAuthorityCode,
                LocalAuthorityName = TestLocalAuthorityName,
                FundingViewData = ExpectedLocalAuthorityFundingData(TestLocalAuthorityCode)[$"AY-1920-{FundingStreamCode.PEAndSport}"],
                FundingStreamConfiguration = ExpectedFundingStreamConfiguration[FundingStreamCode.PEAndSport],
                YearFrom = 2019,
                YearTo = 2020,
                IsCurrentYear = true,
                IsLatestOrFinalFundingForYear = true,
                AsOfMonth = AsOfAllocationsMonth,
                AsOfYear = AsOfAllocationsYear,
                ImportExportAdjustmentYear1 = 2019,
                ImportExportAdjustmentYear2 = 2018,
                InitialIndicativeOrEmpty = "indicative",
                QueryFilterViewModel = new QueryFilterViewModel
                {
                    RouteName = ViewYourFundingConstants.RouteName_LocalAuthorityFundingBreakdown,
                    SearchTerm = null,
                    QueryFilter = new QueryFilter
                    {
                        Filters = new List<SearchResultsFilter>
                {
                    new SearchResultsFilter
                    {
                        Key = SearchFilterConstants.EstablishmentTypeFilterKey,
                        Title = SearchFilterConstants.EstablishmentTypeFilterTitle,
                        Open = true,
                        SearchEnabled = false,
                        Values = new List<SearchFilterValue>()
                    }
                }
                    }
                },
                FundingPeriodCode = "AY-1920"
            };

            // Act
            var actual = await controller.LocalAuthorityFundingBreakdown(
                new LocalAuthorityFundingBreakdownRequest
                {
                    LocalAuthorityCode = TestLocalAuthorityCode,
                    PublishedDate = DateTimeExtensions.ToRouteParameterString(TestPublicationDate),
                    SearchTerm = null,
                    YearFrom = 2019,
                    YearTo = 2020,
                    FundingStreamName = "pe-and-sport-premium"
                },
                null);

            // Assert
            fundingApiServiceMock.Verify();

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingBreakdownViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(vm => vm.Document)
                        .Excluding(info => info.Path.EndsWith("PublicationLayouts"))
                        .Excluding(info => info.Path.EndsWith("Setting.SettingValues"))
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].CreatedAt)
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].LastUpdatedAt));
        }

        /// <summary>
        /// Locals the authority PSG breakdown when accessed from search view model matches expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityPSGBreakdown_WhenAccessedFromSearch_ViewModelMatchesExpected()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(TestSearchTerm);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();
            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               null,
               fundingApiServiceMock,
               settingsServiceMock,
               mockFundingViewService);

            var expectedViewModel = new FundingBreakdownViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                LocalAuthorityCode = TestLocalAuthorityCode,
                LocalAuthorityName = TestLocalAuthorityName,
                FundingViewData = ExpectedLocalAuthorityFundingData(TestLocalAuthorityCode)[$"AY-1920-{FundingStreamCode.PEAndSport}"],
                FundingStreamConfiguration = ExpectedFundingStreamConfiguration[FundingStreamCode.PEAndSport],
                SearchTerm = TestSearchTerm,
                YearFrom = 2019,
                YearTo = 2020,
                IsCurrentYear = true,
                IsLatestOrFinalFundingForYear = true,
                AsOfMonth = AsOfAllocationsMonth,
                AsOfYear = AsOfAllocationsYear,
                ImportExportAdjustmentYear1 = 2019,
                ImportExportAdjustmentYear2 = 2018,
                InitialIndicativeOrEmpty = "indicative",
                QueryFilterViewModel = new QueryFilterViewModel
                {
                    RouteName = ViewYourFundingConstants.RouteName_LocalAuthorityFundingBreakdown,
                    SearchTerm = TestSearchTerm,
                    QueryFilter = new QueryFilter
                    {
                        Filters = new List<SearchResultsFilter>
                {
                    new SearchResultsFilter
                    {
                        Key = SearchFilterConstants.EstablishmentTypeFilterKey,
                        Title = SearchFilterConstants.EstablishmentTypeFilterTitle,
                        Open = true,
                        SearchEnabled = false,
                        Values = new List<SearchFilterValue>()
                    }
                }
                    }
                },
                FundingPeriodCode = "AY-1920"
            };

            // Act
            var actual = await controller.LocalAuthorityFundingBreakdown(
                new LocalAuthorityFundingBreakdownRequest
                {
                    LocalAuthorityCode = TestLocalAuthorityCode,
                    PublishedDate = DateTimeExtensions.ToRouteParameterString(TestPublicationDate),
                    SearchTerm = TestSearchTerm,
                    YearFrom = 2019,
                    YearTo = 2020,
                    FundingStreamName = "pe-and-sport-premium"
                },
                null);

            // Assert
            fundingApiServiceMock.Verify();

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingBreakdownViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(vm => vm.Document)
                        .Excluding(info => info.Path.EndsWith("PublicationLayouts"))
                        .Excluding(info => info.Path.EndsWith("Setting.SettingValues"))
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].CreatedAt)
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].LastUpdatedAt));
        }

        #endregion


        #region LocalAuthorityPSGBreakdown201819 action tests

        /// <summary>
        /// Locals the authority PSG breakdown201819 when not accessed from search view model matches expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityPSGBreakdown201819_WhenNotAccessedFromSearch_ViewModelMatchesExpected()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingDocumentServiceMock = GetMockFundingDocumentService(
                FundingStreamCode.PEAndSport, "AY-1819", 2018, 2019, 1);
            var fundingApiServiceMock = GetMockFundingApiService(TestSearchTerm, fundingSearchResultCount: 1);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();
            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                fundingDocumentServiceMock,
                fundingApiServiceMock,
                settingsServiceMock,
                mockFundingViewService);

            var expectedViewModel = new LocalAuthorityHistorySingleYearViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                LocalAuthorityCode = TestLocalAuthorityCode,
                LocalAuthorityName = TestLocalAuthorityName,
                FundingStreamName = "PE and sport premium",
                FundingStreamCode = "PSG",
                YearFrom = 2018,
                YearTo = 2019,
                LatestYearFrom = 2019,
                LatestYearTo = 2020,
                FundingViewData = new FundingViewData
                {
                    EntityName = "Test LA",
                    PublicationDate = new DateTime(2019, 12, 31),
                    TotalAmount = 12345678M,
                    FundingSubData = new List<IFundingApiSearchProviderFunding>(),
                    FundingStreamCode = "PSG",
                    FundingPeriodCode = "AY-1920",
                    LocalAuthorityName = "Test LA"
                }
            };

            // Act
            var actual = await controller.LocalAuthorityHistorySingleYear("pe-and-sport-premium", 2018, 2019, TestLocalAuthorityCode, null);

            // Assert
            fundingApiServiceMock.Verify();

            actual
                .Should().BeOfType<ViewResult>()
               .Which.Model.Should().BeOfType<LocalAuthorityHistorySingleYearViewModel>()
               .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options.Excluding(vm => vm.Spreadsheets));
        }

        /// <summary>
        /// Locals the authority PSG breakdown201819 when accessed from search view model matches expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityPSGBreakdown201819_WhenAccessedFromSearch_ViewModelMatchesExpected()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingDocumentServiceMock = GetMockFundingDocumentService(
                FundingStreamCode.PEAndSport, "AY-1819", 2018, 2019, 1);
            var fundingApiServiceMock = GetMockFundingApiService(TestSearchTerm, fundingSearchResultCount: 1);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();
            var mockConfigService = GetMockConfigService();

            var controller = GetViewYourFundingController(
                 securityServiceMock,
                 fundingDocumentServiceMock,
                 fundingApiServiceMock,
                 settingsServiceMock,
                 mockFundingViewService);

            var expectedViewModel = new LocalAuthorityHistorySingleYearViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                LocalAuthorityCode = TestLocalAuthorityCode,
                LocalAuthorityName = TestLocalAuthorityName,
                FundingStreamName = "PE and sport premium",
                FundingStreamCode = "PSG",
                YearFrom = 2018,
                YearTo = 2019,
                LatestYearFrom = 2019,
                LatestYearTo = 2020,
                SearchTerm = TestSearchTerm,
                FundingViewData = new FundingViewData
                {
                    EntityName = "Test LA",
                    PublicationDate = new DateTime(2019, 12, 31),
                    TotalAmount = 12345678M,
                    FundingSubData = new List<IFundingApiSearchProviderFunding>(),
                    FundingStreamCode = "PSG",
                    FundingPeriodCode = "AY-1920",
                    LocalAuthorityName = "Test LA"
                },
            };

            // Act
            var actual = await controller.LocalAuthorityHistorySingleYear("pe-and-sport-premium", 2018, 2019, TestLocalAuthorityCode, TestSearchTerm);

            // Assert
            fundingApiServiceMock.Verify();

            actual
                .Should().BeOfType<ViewResult>()
               .Which.Model.Should().BeOfType<LocalAuthorityHistorySingleYearViewModel>()
               .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options.Excluding(vm => vm.Spreadsheets));
        }

        #endregion


        #region LocalAuthorityDSGHistory action tests

        /// <summary>
        /// Locals the authority DSG history when not accessed from search view model matches expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityHistory_WhenNotAccessedFromSearch_ViewModelMatchesExpected()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(
                TestLocalAuthorityCode,
                providerSearchResultCount: 1,
                fundingSearchResultCount: 1,
                fundingPeriodCode: DSGFundingPeriodCode);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                fundingApiServiceMock,
                settingsServiceMock,
                mockFundingViewService);

            var expectedViewModel = new LocalAuthorityHistoryViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                LocalAuthorityCode = TestLocalAuthorityCode,
                LocalAuthorityName = TestLocalAuthorityName,
                SecondaryContentTitle = "Dedicated schools grant (<abbr title=\"Dedicated schools grant\">DSG</abbr>)",
                FundingStreamConfiguration = ExpectedFundingStreamConfiguration[FundingStreamCode.DSG],
                FundingPeriodPublications = GetExpectedDSGPublications(),
                FundingStreamName = "dedicated-schools-grant",
                FundingViewData = new FundingViewData
                {
                    EntityName = "Test LA",
                    PublicationDate = new DateTime(2019, 12, 31),
                    TotalAmount = 12345678M,
                    FundingValues = new Dictionary<string, object>
            {
                { "TestFundingTotal", 12345678M }
            },
                    FundingStreamCode = "DSG",
                    FundingPeriodCode = "FY-2021",
                    LocalAuthorityName = "Test LA"
                }
            };

            // Act
            var actual = await controller.LocalAuthorityHistory(
                TestLocalAuthorityCode,
                "dedicated-schools-grant",
                null);

            // Assert
            fundingApiServiceMock.Verify();

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityHistoryViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(info => info.Path.EndsWith("PublicationLayouts"))
                        .Excluding(info => info.Path.EndsWith("Setting.SettingValues"))
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].CreatedAt)
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].LastUpdatedAt));
        }

        /// <summary>
        /// Locals the authority DSG history when accessed from search view model matches expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityHistory_WhenAccessedFromSearch_ViewModelMatchesExpected()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(
                TestLocalAuthorityCode,
                providerSearchResultCount: 1,
                fundingSearchResultCount: 1,
                fundingPeriodCode: DSGFundingPeriodCode);

            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               null,
               fundingApiServiceMock,
               settingsServiceMock,
               mockFundingViewService);

            var expectedViewModel = new LocalAuthorityHistoryViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                LocalAuthorityCode = TestLocalAuthorityCode,
                LocalAuthorityName = TestLocalAuthorityName,
                SecondaryContentTitle = "Dedicated schools grant (<abbr title=\"Dedicated schools grant\">DSG</abbr>)",
                FundingStreamConfiguration = ExpectedFundingStreamConfiguration[FundingStreamCode.DSG],
                SearchTerm = TestSearchTerm,
                FundingPeriodPublications = GetExpectedDSGPublications(),
                FundingStreamName = "dedicated-schools-grant",
                FundingViewData = new FundingViewData
                {
                    EntityName = "Test LA",
                    PublicationDate = new DateTime(2019, 12, 31),
                    TotalAmount = 12345678M,
                    FundingValues = new Dictionary<string, object>
            {
                { "TestFundingTotal", 12345678M }
            },
                    FundingStreamCode = "DSG",
                    FundingPeriodCode = "FY-2021",
                    LocalAuthorityName = "Test LA"
                },
            };

            // Act
            var actual = await controller.LocalAuthorityHistory(
                TestLocalAuthorityCode,
                "dedicated-schools-grant",
                TestSearchTerm);

            // Assert
            fundingApiServiceMock.Verify();

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityHistoryViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(info => info.Path.EndsWith("PublicationLayouts"))
                        .Excluding(info => info.Path.EndsWith("Setting.SettingValues"))
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].CreatedAt)
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].LastUpdatedAt));
        }

        #endregion


        #region LocalAuthorityPSGHistory action tests

        /// <summary>
        /// Locals the authority PSG history when not accessed from search view model matches expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityPSGHistory_WhenNotAccessedFromSearch_ViewModelMatchesExpected()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(
                TestLocalAuthorityCode,
                providerSearchResultCount: 1,
                fundingSearchResultCount: 1);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                fundingApiServiceMock,
                settingsServiceMock,
                mockFundingViewService);

            var expectedViewModel = new LocalAuthorityHistoryViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                LocalAuthorityCode = TestLocalAuthorityCode,
                LocalAuthorityName = TestLocalAuthorityName,
                SecondaryContentTitle = "PE and sport premium",
                FundingStreamConfiguration = ExpectedFundingStreamConfiguration[FundingStreamCode.PEAndSport],
                FundingPeriodPublications = GetExpectedPSGPublications(),
                FundingViewData = new FundingViewData
                {
                    EntityName = "Test LA",
                    PublicationDate = new DateTime(2019, 12, 31),
                    TotalAmount = 12345678M,
                    FundingSubData = new List<IFundingApiSearchProviderFunding>(),
                    FundingStreamCode = "PSG",
                    FundingPeriodCode = "AY-1920",
                    LocalAuthorityName = "Test LA"
                },
                FundingStreamName = "pe-and-sport-premium"
            };

            // Act
            var actual = await controller.LocalAuthorityHistory(
                TestLocalAuthorityCode,
                "pe-and-sport-premium",
                null);

            // Assert
            fundingApiServiceMock.Verify();

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityHistoryViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(info => info.Path.EndsWith("PublicationLayouts"))
                        .Excluding(info => info.Path.EndsWith("Setting.SettingValues"))
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].CreatedAt)
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].LastUpdatedAt));
        }

        /// <summary>
        /// Locals the authority PSG history when accessed from search view model matches expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityPSGHistory_WhenAccessedFromSearch_ViewModelMatchesExpected()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var fundingApiServiceMock = GetMockFundingApiService(
                TestLocalAuthorityCode,
                providerSearchResultCount: 1,
                fundingSearchResultCount: 1);
            var settingsServiceMock = GetMockSettingsService();
            var mockFundingViewService = GetMockFundingViewService();

            var controller = GetViewYourFundingController(
               securityServiceMock,
               null,
               fundingApiServiceMock,
               settingsServiceMock,
               mockFundingViewService);

            var expectedViewModel = new LocalAuthorityHistoryViewModel
            {
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                },
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                LocalAuthorityCode = TestLocalAuthorityCode,
                LocalAuthorityName = TestLocalAuthorityName,
                SecondaryContentTitle = "PE and sport premium",
                FundingStreamConfiguration = ExpectedFundingStreamConfiguration[FundingStreamCode.PEAndSport],
                SearchTerm = TestSearchTerm,
                FundingPeriodPublications = GetExpectedPSGPublications(),
                FundingViewData = new FundingViewData
                {
                    EntityName = "Test LA",
                    PublicationDate = new DateTime(2019, 12, 31),
                    TotalAmount = 12345678M,
                    FundingSubData = new List<IFundingApiSearchProviderFunding>(),
                    FundingStreamCode = "PSG",
                    FundingPeriodCode = "AY-1920",
                    LocalAuthorityName = "Test LA"
                },
                FundingStreamName = "pe-and-sport-premium"
            };

            // Act
            var actual = await controller.LocalAuthorityHistory(
                TestLocalAuthorityCode,
                "pe-and-sport-premium",
                TestSearchTerm);

            // Assert
            fundingApiServiceMock.Verify();

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityHistoryViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(info => info.Path.EndsWith("PublicationLayouts"))
                        .Excluding(info => info.Path.EndsWith("Setting.SettingValues"))
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].CreatedAt)
                        .Excluding(option => option.FundingStreamConfiguration.SettingValues[0].LastUpdatedAt));
        }

        #endregion


        #region ProviderSpreadsheetDownload tests

        /// <summary>
        /// Providers the spreadsheet download for valid published date returns expected file.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task ProviderSpreadsheetDownload_ForValidPublishedDate_ReturnsExpectedFile()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();

            var mockFundingViewService = new Mock<IFundingViewService>();
            var expectedFileContent = new byte[1024];

            mockFundingViewService
                .Setup(s => s.GenerateFundingDocument(
                    It.IsAny<Services.Models.FundingStream>(),
                    PSGFundingPeriodCode,
                    It.IsAny<DateTime>(),
                    It.IsAny<Publication>(),
                    FundingViewType.Spreadsheet,
                    FundingViewScope.Provider,
                    new[] { FileFormat.ODS },
                    It.Is<SearchFilter[]>(f => f.Length == 1
                        && f.First().PropertyName == SearchFilterPropertyName.Ukprn
                        && f.First().PropertyValue == nameof(ProviderSpreadsheetDownloadRequest.Ukprn)),
                    null,
                    false,
                    false,
                    true,
                    It.IsAny<IFundingApiSearchFunding[]>(),
                    It.IsAny<IFundingApiSearchProviderFunding[]>(),
                    false,
                    false))
                .ReturnsAsync(new List<FundingDocumentMeta>
                {
                    new FundingDocumentMeta
                    {
                        Data = expectedFileContent,
                        Filename = nameof(FundingDocumentMeta.Filename)
                    }
                })
                .Verifiable();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                null,
                settingsServiceMock,
                mockFundingViewService);

            // Act
            var actual = await controller.ProviderSpreadsheetDownload(new ProviderSpreadsheetDownloadRequest
            {
                FundingStreamCode = FundingStreamCode.PEAndSport,
                PublishedDate = DateTimeExtensions.ToRouteParameterString(TestPublicationDate),
                Format = FundingDocumentFileType.Spreadsheet_OpenFormat,
                Ukprn = nameof(ProviderSpreadsheetDownloadRequest.Ukprn),
                YearFrom = 2019,
                YearTo = 2020,
                YearTypeCode = YearTypeCode.AcademicYear
            });

            // Assert
            mockFundingViewService.Verify();
            actual.Should().NotBeNull().And.BeOfType<FileContentResult>();

            var actualFileContentResult = (FileContentResult)actual;
            actualFileContentResult.ContentType.Should().Be("application/vnd.oasis.opendocument.spreadsheet; charset=utf-8");
            actualFileContentResult.FileDownloadName.Should().Be(nameof(FundingDocumentMeta.Filename));
            actualFileContentResult.FileContents.Should().BeEquivalentTo(expectedFileContent);
        }

        /// <summary>
        /// Providers the spreadsheet download for invalid published date throws exception.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void ProviderSpreadsheetDownload_ForInvalidPublishedDate_ThrowsException()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                null,
                settingsServiceMock,
                null);

            // Act
            Func<Task> act = async () => await controller.ProviderSpreadsheetDownload(new ProviderSpreadsheetDownloadRequest
            {
                FundingStreamCode = FundingStreamCode.PEAndSport,
                PublishedDate = DateTimeExtensions.ToRouteParameterString(DateTime.ParseExact(PSGPublicationDateSetting, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                        .AddDays(1))
            });

            // Assert
            act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        /// <summary>
        /// Providers the spreadsheet download for invalid published date throws exception.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void ProviderSpreadsheetDownload_ForNonPublicFundingStream_ThrowsInvalidOperationException()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                null,
                settingsServiceMock,
                null);

            // Act
            Func<Task> act = async () => await controller.ProviderSpreadsheetDownload(new ProviderSpreadsheetDownloadRequest
            {
                FundingStreamCode = "GAG"
            });

            // Assert
            act.Should().ThrowAsync<InvalidOperationException>();
        }

        #endregion


        #region NationalSpreadsheetDownload tests

        /// <summary>
        /// Providers the spreadsheet download for valid published date returns expected file.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task NationalSpreadsheetDownload_ForValidPublishedDate_ReturnsExpectedFile()
        {
            // Arrange
            var user = NotLoggedInUser;
            var securityServiceMock = GetMockSecurityService(user);
            var settingsServiceMock = GetMockSettingsService();

            var mockFundingViewService = new Mock<IFundingViewService>();
            var expectedFileContent = new byte[1024];

            mockFundingViewService
                .Setup(s => s.GenerateFundingDocument(
                    It.IsAny<Services.Models.FundingStream>(),
                    PSGFundingPeriodCode,
                    It.IsAny<DateTime>(),
                    It.IsAny<Publication>(),
                    FundingViewType.Spreadsheet,
                    FundingViewScope.National,
                    new[] { FileFormat.ODS },
                    null,
                    null,
                    false,
                    false,
                    true,
                    It.IsAny<IFundingApiSearchFunding[]>(),
                    It.IsAny<IFundingApiSearchProviderFunding[]>(),
                    false,
                    false))
                .ReturnsAsync(new List<FundingDocumentMeta>
                {
                    new FundingDocumentMeta
                    {
                        Data = expectedFileContent,
                        Filename = nameof(FundingDocumentMeta.Filename)
                    }
                })
                .Verifiable();

            var controller = GetViewYourFundingController(
                securityServiceMock,
                null,
                null,
                settingsServiceMock,
                mockFundingViewService);

            // Act
            var actual = await controller.NationalSpreadsheetDownload(new NationalSpreadsheetDownloadRequest
            {
                FundingStreamCode = FundingStreamCode.PEAndSport,
                PublishedDate = DateTimeExtensions.ToRouteParameterString(TestPublicationDate),
                Format = FundingDocumentFileType.Spreadsheet_OpenFormat,
                YearFrom = 2019,
                YearTo = 2020,
                YearTypeCode = YearTypeCode.AcademicYear
            });

            // Assert
            mockFundingViewService.Verify();
            actual.Should().NotBeNull().And.BeOfType<FileContentResult>();

            var actualFileContentResult = (FileContentResult)actual;
            actualFileContentResult.ContentType.Should().Be("application/vnd.oasis.opendocument.spreadsheet; charset=utf-8");
            actualFileContentResult.FileDownloadName.Should().Be(nameof(FundingDocumentMeta.Filename));
            actualFileContentResult.FileContents.Should().BeEquivalentTo(expectedFileContent);
        }

        #endregion


        #region Mock Data Helpers

        /// <summary>
        /// Gets the expected DSG publications.
        /// </summary>
        /// <returns>The expected DSG publications.</returns>
        private static List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>> GetExpectedDSGPublications()
        {
            return new List<KeyValuePair<(int, int), List<Publication>>>
                {
                    new KeyValuePair<(int, int), List<Publication>>(
                        (2020, 2021), new List<Publication>
                        {
                            new Publication
                            {
                                PublishedDate = TestPublicationDate,
                                Description = TestPublicationDescription,
                                FundingPeriodCode = DSGFundingPeriodCode,
                                Status = PublicationStatus.Published,
                                IsLatest = true
                            }
                        }),
                    new KeyValuePair<(int, int), List<Publication>>(
                        (2019, 2020), new List<Publication>()),
                    new KeyValuePair<(int, int), List<Publication>>(
                        (2018, 2019), new List<Publication>()),
                    new KeyValuePair<(int, int), List<Publication>>(
                        (2017, 2018), new List<Publication>())
                };
        }

        /// <summary>
        /// Gets the expected PSG publications.
        /// </summary>
        /// <returns>The expected PSG publications.</returns>
        private static List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>> GetExpectedPSGPublications()
        {
            return new List<KeyValuePair<(int, int), List<Publication>>>
                {
                    new KeyValuePair<(int, int), List<Publication>>(
                        (2019, 2020), new List<Publication>
                        {
                            new Publication
                            {
                                PublishedDate = TestPublicationDate,
                                Description = TestPublicationDescription,
                                FundingPeriodCode = PSGFundingPeriodCode,
                                Status = PublicationStatus.Published,
                                IsLatest = true
                            }
                        }),
                    new KeyValuePair<(int, int), List<Publication>>(
                        (2018, 2019), new List<Publication>()),
                    new KeyValuePair<(int, int), List<Publication>>(
                        (2017, 2018), new List<Publication>()),
                    new KeyValuePair<(int, int), List<Publication>>(
                        (2016, 2017), new List<Publication>())
                };
        }

        /// <summary>
        /// Gets the mock funding documents.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="documentCount">The document count.</param>
        /// <param name="rewritePath">if set to <c>true</c> [rewrite path].</param>
        /// <returns>The FundingDocument collection.</returns>
        private IReadOnlyCollection<FundingDocument> GetMockFundingDocuments(
            string fundingStreamCode,
            int yearFrom,
            int yearTo,
            int documentCount,
            bool rewritePath = true)
        {
            return Enumerable.Range(0, documentCount).Select(documentIndex =>
                new FundingDocument
                {
                    FileExtension = FundingDocumentFileType.Spreadsheet_OpenFormat,
                    DocumentPublishedDate = TestPublicationDate,
                    FileSizeBytes = 5000,
                    FilePath = rewritePath ?
                        $"/api/funding/DownloadSpreadsheet/{documentIndex + 1}/{TestPublicationDate.ToString("yyyy-MM-dd")}"
                        : $"/path/to/spreadsheet/{documentIndex + 1}",
                    FundingStreamCode = fundingStreamCode,
                    YearFrom = yearFrom,
                    YearTo = yearTo
                })
            .ToList();
        }

        /// <summary>
        /// Gets the National Funding Allocation mock funding documents.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code.</param>
        /// <param name="fundingStreamName">The funding stream name.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="documentCount">The document count.</param>
        /// <param name="rewritePath">if set to <c>true</c> [rewrite path].</param>
        /// <returns>The FundingDocument collection.</returns>
        private IReadOnlyCollection<FundingDocument> GetNationalFundingAllocationMockFundingDocuments(
            string fundingStreamCode,
            string fundingStreamName,
            int yearFrom,
            int yearTo,
            int documentCount,
            bool rewritePath = true)
        {
            var fundingDocuments = Enumerable.Range(0, documentCount).Select(documentIndex =>
                new FundingDocument
                {
                    FileExtension = FundingDocumentFileType.Spreadsheet_OpenFormat,
                    DocumentPublishedDate = TestPublicationDate,
                    FileSizeBytes = 5000,
                    FilePath = rewritePath ?
                        $"/api/funding/DownloadSpreadsheet/{documentIndex + 1}/{TestPublicationDate.ToString("yyyy-MM-dd")}"
                        : $"/path/to/spreadsheet/{documentIndex + 1}",
                    FundingStreamCode = fundingStreamCode,
                    FundingStreamName = fundingStreamName,
                    YearFrom = yearFrom,
                    YearTo = yearTo,
                    IsLatestDocument = true,
                    IsFinal = false
                })
            .ToList();

            return fundingDocuments;
        }

        /// <summary>
        /// Gets the mock provider results.
        /// </summary>
        /// <param name="resultCount">The result count.</param>
        /// <param name="schoolType">Type of the school.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <returns>The IFundingApiSearchProviderFunding list.</returns>
        private IEnumerable<IFundingApiSearchProviderFunding> GetMockProviderResults(int resultCount, string schoolType = "Maintained", string fundingPeriodCode = PSGFundingPeriodCode)
        {
            string providerType = ProviderTypeExternal.LocalAuthorityMaintainedSchool;
            string providerSubType = string.Empty;

            if (schoolType.EndsWith("NMSS"))
            {
                providerType = ProviderTypeExternal.SpecialSchool;
                providerSubType = ProviderSubTypeExternal.NonMaintainedSpecialSchool;
            }
            else if (schoolType.EndsWith("Academies"))
            {
                providerType = ProviderTypeExternal.Academy;
            }

            var instanceNumber = 0;

            return Enumerable.Range(0, resultCount).Select(provider =>
                new FundingApiSearchProviderFunding
                {
                    OrganisationName = nameof(FundingApiSearchProviderFunding.OrganisationName),
                    ParentName = nameof(FundingApiSearchProviderFunding.ParentName),
                    ParentPrimaryIdentifier = nameof(FundingApiSearchProviderFunding.ParentPrimaryIdentifier),
                    ParentProviderType = GroupingType.LocalAuthority,
                    OrganisationUkprn = $"{OrganisationUkprn}{instanceNumber++}",
                    ProviderType = providerType,
                    ProviderSubType = providerSubType,
                    FundingPeriodCode = fundingPeriodCode,
                    FundingStreamCode = FundingStreamCode.PEAndSport,
                    StatusChangedDate = TestStatusChangedDate,
                    GroupingReason = GroupingReason.Information
                });
        }

        /// <summary>
        /// Gets the mock funding results.
        /// </summary>
        /// <param name="resultCount">The result count.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <returns>The IFundingApiSearchFunding list.</returns>
        private IEnumerable<IFundingApiSearchFunding> GetMockFundingResults(int resultCount, string fundingPeriodCode = PSGFundingPeriodCode)
        {
            return Enumerable.Range(0, resultCount).Select(provider =>
                new FundingApiSearchFunding
                {
                    GroupName = TestLocalAuthorityName,
                    GroupingType = GroupingType.LocalAuthority,
                    GroupCode = TestLocalAuthorityCode,
                    FundingPeriodCode = fundingPeriodCode,
                    FundingStreamCode = FundingStreamCode.PEAndSport,
                    StatusChangedDate = TestStatusChangedDate,
                    GroupingReason = GroupingReason.Information
                });
        }

        /// <summary>
        /// Gets the expected provider statement view model.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="paymentDateSettingName">Name of the payment date setting.</param>
        /// <returns>The ProviderStatementViewModel.</returns>
        private ProviderStatementViewModel GetExpectedProviderStatementViewModel(
            User user,
            string searchTerm = null,
            string paymentDateSettingName = "")
        {
            if (string.IsNullOrEmpty(paymentDateSettingName))
            {
                paymentDateSettingName = SettingName.NextAllocationPaymentDateMaintainedSchools;
            }

            var userViewModel = user.IsAuthenticated
                ? new CurrentUserViewModel
                {
                    IsExternalUser = LoggedInUser.IsExternalUser,
                    IsLoggedIn = LoggedInUser.IsAuthenticated,
                    ProviderName = null,
                    Ukprn = LoggedInUser.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                }
                : new CurrentUserViewModel
                {
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                };

            return new ProviderStatementViewModel
            {
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                CurrentUser = userViewModel,
                SearchTerm = searchTerm,
                FundingViewData = new Dictionary<string, FundingViewData>
                {
                    {
                        "PSG", new FundingViewData
                        {
                            EntityName = "OrganisationName",
                            PublicationDate = new DateTime(2019, 12, 31),
                            TotalAmount = 12345678M,
                            FundingSubData = new List<IFundingApiSearchProviderFunding>()
                        }
                    }
                },
                OrganisationUkprn = "OrganisationUkprn0"
            };
        }

        /// <summary>
        /// Gets the provider statement section view model.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="isFundingBreakdown">if set to <c>true</c> [is funding breakdown].</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="paymentDateSettingName">Name of the payment date setting.</param>
        /// <returns>The list of ProviderStatementSectionViewModel.</returns>
        private IEnumerable<ProviderStatementSectionViewModel> GetProviderStatementSectionViewModel(
            int count,
            bool isFundingBreakdown = false,
            string searchTerm = null,
            string paymentDateSettingName = "")
        {
            if (string.IsNullOrEmpty(paymentDateSettingName))
            {
                paymentDateSettingName = SettingName.NextAllocationPaymentDateMaintainedSchools;
            }

            return Enumerable.Range(0, count).Select(provider =>
                new ProviderStatementSectionViewModel
                {
                    ProviderResult = GetMockProviderResults(1, paymentDateSettingName).First(),
                    CurrentAsOfYear = AsOfAllocationsYear,
                    CurrentAsOfMonth = AsOfAllocationsMonth,
                    CurrentYearStart = 2019,
                    CurrentYearEnd = 2020,
                    NextPaymentDate = ExpectedFundingStreamConfiguration[FundingStreamCode.PEAndSport]
                                      .NextPayments?.FirstOrDefault()?.NextPaymentDate,
                    NoNextPaymentForTheYearText = "There are no more scheduled payments for academic year 2019 to 2020.",
                    SearchTerm = searchTerm,
                    Document = new FundingDocument
                    {
                        FileExtension = FundingDocumentFileType.Spreadsheet_OpenFormat,
                        FileSizeBytes = 5000,
                        FilePath = isFundingBreakdown ? null : "/path/to/spreadsheet/1",
                        YearFrom = 2019,
                        YearTo = 2020,
                        FundingStreamCode = FundingStreamCode.PEAndSport
                    },
                    FundingStreamConfiguration = ExpectedFundingStreamConfiguration[FundingStreamCode.PEAndSport]
                });
        }

        /// <summary>
        /// Gets the expected provider results view model.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="resultCount">The result count.</param>
        /// <returns>The ProviderResultsViewModel.</returns>
        private ProviderResultsViewModel GetExpectedProviderResultsViewModel(User user, int resultCount)
        {
            var userViewModel = user.IsAuthenticated
                ? new CurrentUserViewModel
                {
                    IsExternalUser = LoggedInUser.IsExternalUser,
                    IsLoggedIn = LoggedInUser.IsAuthenticated,
                    ProviderName = null,
                    Ukprn = LoggedInUser.Ukprn,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName
                }
                : new CurrentUserViewModel();

            var expectedViewModel = new ProviderResultsViewModel
            {
                FeedbackLink = TestFeedbackLink,
                ContactUsLink = TestContactUsLink,
                CurrentUser = userViewModel,
                ProviderResults = GetMockProviderResults(resultCount).ToList(),
                SearchTerm = SearchTerm,
                BackToTopLinkMinimumCount = 25,
                QueryFilterViewModel = new QueryFilterViewModel
                {
                    SearchTerm = SearchTerm,
                    QueryFilter = QueryFilterHelper.BuildQueryFilter(GetMockProviderResults(resultCount).ToList()),
                    RouteName = ViewYourFundingConstants.RouteName_ProviderDidYouMean
                }
            };
            return expectedViewModel;
        }

        /// <summary>
        /// Gets the applied query filter.
        /// </summary>
        /// <returns>The QueryFilter.</returns>
        private QueryFilter GetAppliedQueryFilter()
        {
            return new QueryFilter
            {
                Filters = new List<SearchResultsFilter>
                {
                    new SearchResultsFilter
                    {
                        Key = SearchFilterConstants.LocalAuthorityFilterKey,
                        Open = true,
                        Title = SearchFilterConstants.LocalAuthorityFilterTitle,
                        Values = new List<SearchFilterValue>
                        {
                            new SearchFilterValue
                            {
                                Count = 1,
                                Value = nameof(FundingApiSearchProviderFunding.ParentPrimaryIdentifier),
                                Selected = true,
                                Title = nameof(SearchFilterValue.Title),
                                ParentKey = SearchFilterConstants.LocalAuthorityFilterKey
                            }
                        }
                    },
                    new SearchResultsFilter
                    {
                        Key = SearchFilterConstants.EstablishmentTypeFilterKey,
                        Open = true,
                        Title = SearchFilterConstants.EstablishmentTypeFilterTitle,
                        Values = new List<SearchFilterValue>
                        {
                            new SearchFilterValue
                            {
                                Count = 1,
                                Value = ProviderTypeInternal.MaintainedSchool,
                                Selected = true,
                                Title = ProviderTypeInternal.MaintainedSchool,
                                ParentKey = SearchFilterConstants.EstablishmentTypeFilterKey
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Expecteds the publications.
        /// </summary>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <returns>The Publication.</returns>
        private List<Publication> ExpectedPublications(string fundingPeriodCode)
            => new List<Publication>
            {
                new Publication
                {
                    PublishedDate = TestPublicationDate,
                    Description = TestPublicationDescription,
                    FundingPeriodCode = fundingPeriodCode,
                    Status = PublicationStatus.Published,
                    IsLatest = true
                }
            };

        /// <summary>
        /// Expecte none matchin publications.
        /// </summary>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <returns>The Publication.</returns>
        private List<Publication> ExpectedNonMatchingPublications(string fundingPeriodCode)
            => new List<Publication>
            {
                new Publication
                {
                    PublishedDate = new DateTime(2020, 1, 1),
                    Description = TestPublicationDescription,
                    FundingPeriodCode = fundingPeriodCode,
                    Status = PublicationStatus.Published,
                    IsLatest = true
                }
            };


        /// <summary>
        /// Gets the expected funding stream configuration.
        /// </summary>
        /// <value>
        /// The expected funding stream configuration.
        /// </value>
        private Dictionary<string, Model.FundingStream> ExpectedFundingStreamConfiguration
            => new Dictionary<string, Model.FundingStream>
            {
                {
                    FundingStreamCode.DSG, new Model.FundingStream
                    {
                        FundingStreamCode = FundingStreamCode.DSG,
                        FundingStreamName = "Dedicated schools grant",
                        FundingStreamNameWithinSentence = "dedicated schools grant",
                        FundingStreamCodePubliclyKnown = true,
                        Publications = ExpectedPublications(DSGFundingPeriodCode),
                        NextPayments = new List<NextPayment>(),
                        SettingValues = ExpectedSettingValues(FundingStreamCode.DSG),
                        RelevantForNational = true,
                        RelevantForOrganisations_Public = true,
                        RelevantForOrganisations_LoggedIn = true,
                        Active = true,
                        Id = 2
                    }
                },
                {
                    FundingStreamCode.PEAndSport, new Model.FundingStream
                    {
                        FundingStreamCode = FundingStreamCode.PEAndSport,
                        FundingStreamName = "PE and sport premium",
                        FundingStreamNameWithinSentence = "PE and sport premium",
                        FundingStreamCodePubliclyKnown = false,
                        Publications = ExpectedPublications(PSGFundingPeriodCode),
                        NextPayments = new List<NextPayment>(),
                        SettingValues = ExpectedSettingValues(FundingStreamCode.PEAndSport),
                        RelevantForNational = true,
                        RelevantForOrganisations_Public = true,
                        RelevantForOrganisations_LoggedIn = true,
                        RelevantForProviders_LoggedIn = true,
                        RelevantForProviders_Public = true,
                        Active = true,
                        Id = 1
                    }
                }
            };

        public List<SettingValue> ExpectedSettingValues(string fundingStreamCode)
        {
            var returnList = new List<SettingValue>();

            if (fundingStreamCode == FundingStreamCode.DSG)
            {
                returnList.Add(new SettingValue
                {
                    CreatedAt = DateTime.MaxValue,
                    FundingStreamId = 2,
                    Id = 0,
                    LastUpdatedAt = DateTime.MaxValue,
                    LastUpdatedBy = "System",
                    SettingId = 2,
                    Setting = new SettingType
                    {
                        SettingName = "FinancialYear",
                    },
                    Value = "202021"
                });
            }
            else
            {
                returnList.Add(new SettingValue
                {
                    CreatedAt = DateTime.MaxValue,
                    FundingStreamId = 1,
                    Id = 0,
                    LastUpdatedAt = DateTime.MaxValue,
                    LastUpdatedBy = "System",
                    SettingId = 1,
                    Setting = new SettingType
                    {
                        SettingName = "AcademicYear",
                    },
                    Value = "201920"
                });

                returnList.Add(new SettingValue
                {
                    CreatedAt = DateTime.MaxValue,
                    FundingStreamId = 1,
                    Id = 0,
                    LastUpdatedAt = DateTime.MaxValue,
                    LastUpdatedBy = "System",
                    SettingId = 2,
                    Setting = new SettingType
                    {
                        SettingName = "ProviderDownloadSizeInBytes",
                    },
                    Value = "5000"
                });
            }

            return returnList;
        }

        /// <summary>
        /// Gets the next payment dates.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code.</param>
        /// <returns>The next payment dates.</returns>
        private Dictionary<string, DateTime> GetNextPaymentDates(string fundingStreamCode)
        {
            var returnDictionary = new Dictionary<string, DateTime>();

            if (fundingStreamCode == FundingStreamCode.DSG)
            {
                returnDictionary.Add(
                    SettingName.NextAllocationPaymentDate,
                    DateTime.ParseExact(
                        NextPaymentDateSetting,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None));
            }
            else
            {
                returnDictionary.Add(
                    SettingName.NextAllocationPaymentDateMaintainedSchools,
                    DateTime.ParseExact(
                        NextPaymentDateSettingMaintained,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None));

                returnDictionary.Add(
                    SettingName.NextAllocationPaymentDateAcademies,
                    DateTime.ParseExact(
                        NextPaymentDateSettingAcademies,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None));

                returnDictionary.Add(
                    SettingName.NextAllocationPaymentDateNMSS,
                    DateTime.ParseExact(
                        NextPaymentDateSettingNMSS,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None));
            }

            return returnDictionary;
        }

        /// <summary>
        /// Expected National Funding Allocation funding data.
        /// </summary>
        /// <returns>The FundingViewData.</returns>
        private Dictionary<string, FundingViewData> ExpectedNationalFundingAllocationFundingData(
            string fundingStreamCode,
            string fundingStreamName,
            string fundingPeriodCode,
            int yearFrom,
            int yearTo)
            => new Dictionary<string, FundingViewData>
            {
                {
                    fundingStreamCode,
                    new FundingViewData
                    {
                        Components = new List<Component>
                        {
                            new Component(new ComponentConfiguration
                            {
                                FundingStreamCode = fundingStreamCode,
                                FundingStreamName = fundingStreamName,
                                FundingPeriodCode = fundingPeriodCode,
                                Year1 = yearFrom,
                                Year2 = yearTo
                            })
                            {
                                Type = ComponentType.Heading_Large,
                                Title = "Test Heading"
                            }
                        },

                        EntityName = string.Empty,
                        PublicationDate = TestPublicationDate,
                        FundingValues = new Dictionary<string, object>
                        {
                            { nameof(TestFundingTotal), TestFundingTotal }
                        }
                    }
                },
            };

        /// <summary>
        /// Expecteds the local authority funding data.
        /// </summary>
        /// <param name="localAuthorityCode">The local authority code.</param>
        /// <returns>The FundingViewData.</returns>
        private Dictionary<string, FundingViewData> ExpectedLocalAuthorityFundingData(string localAuthorityCode)
            => new Dictionary<string, FundingViewData>
            {
                {
                    $"{DSGFundingPeriodCode}-{FundingStreamCode.DSG}",
                    new FundingViewData
                    {
                        EntityName = TestLocalAuthorityName,
                        PublicationDate = TestPublicationDate,
                        TotalAmount = TestFundingTotal,
                        FundingValues = new Dictionary<string, object>
                        {
                            { nameof(TestFundingTotal), TestFundingTotal }
                        },
                        FundingStreamCode = FundingStreamCode.DSG,
                        FundingPeriodCode = DSGFundingPeriodCode,
                        LocalAuthorityName = TestLocalAuthorityName
                    }
                },
                {
                    $"{PSGFundingPeriodCode}-{FundingStreamCode.PEAndSport}",
                    new FundingViewData
                    {
                        EntityName = TestLocalAuthorityName,
                        PublicationDate = TestPublicationDate,
                        TotalAmount = TestFundingTotal,
                        FundingValues = new Dictionary<string, object>(),
                        FundingSubData = new List<IFundingApiSearchProviderFunding>(),
                        FundingStreamCode = FundingStreamCode.PEAndSport,
                        FundingPeriodCode = PSGFundingPeriodCode,
                        LocalAuthorityName = TestLocalAuthorityName
                    }
                },
            };

        /// <summary>
        /// Expecteds the provider funding data.
        /// </summary>
        /// <param name="ukprn">The ukprn.</param>
        /// <returns>The FundingViewData.</returns>
        private Dictionary<string, FundingViewData> ExpectedProviderFundingData(string ukprn)
            => new Dictionary<string, FundingViewData>
            {
                {
                    FundingStreamCode.PEAndSport,
                    new FundingViewData
                    {
                        EntityName = OrganisationName,
                        PublicationDate = TestPublicationDate,
                        TotalAmount = TestFundingTotal,
                        FundingValues = new Dictionary<string, object>(),
                        FundingSubData = new List<IFundingApiSearchProviderFunding>()
                    }
                },
            };

        /// <summary>
        /// Gets the publish date dictionary.
        /// </summary>
        /// <param name="numberOfDates">The number of dates.</param>
        /// <returns>The publish dates.</returns>
        private Dictionary<int, IDictionary<DateTime, string>> GetPublishDateDictionary(int numberOfDates)
        {
            var dictionary = new Dictionary<int, IDictionary<DateTime, string>>();
            var data = Enumerable.Range(0, numberOfDates)
                .Select(offset => TestPublicationDate.AddDays(offset))
                .ToDictionary(date => date, date => date == TestPublicationDate
                    ? TestPublicationDescription
                    : "New allocations published for the academic year 2019 to 2020");

            dictionary.Add(2019, data);

            return dictionary;
        }

        #endregion


        #region Get Controller Helpers

        /// <summary>
        /// Gets the view your funding controller.
        /// </summary>
        /// <param name="securityServiceMock">The security service mock.</param>
        /// <param name="mockFundingDocumentService">The mock funding document service.</param>
        /// <param name="mockFundingApiService">The mock funding API service.</param>
        /// <param name="mockSettingsService">The mock settings service.</param>
        /// <param name="mockFundingViewService">The mock funding view service.</param>
        /// <returns>The ViewYourFundingController.</returns>
        private ViewYourFundingController GetViewYourFundingController(
            Mock<IClaimsBasedIdentityService> securityServiceMock = null,
            Mock<IFundingDocumentService> mockFundingDocumentService = null,
            Mock<IFundingApiService> mockFundingApiService = null,
            Mock<IUserJourneyService> mockSettingsService = null,
            Mock<IFundingViewService> mockFundingViewService = null,
            Mock<IGlobalSettingService> mockGlobalSettingService = null)
        {
            if (mockGlobalSettingService == null)
            {
                mockGlobalSettingService = CommonMocks.GlobalSettingService();
            }

            var controller = new ViewYourFundingController(
                new ComponentService(null, null),
                securityServiceMock?.Object,
                mockFundingDocumentService?.Object,
                mockFundingApiService?.Object,
                mockSettingsService?.Object,
                mockFundingViewService?.Object,
                GetMapper(),
                new MemoryCacheService(null, 0),
                mockGlobalSettingService?.Object,
                GetMockConfigService().Object);

            return controller;
        }

        #endregion


        #region Mock Service Helpers

        private IMapper GetMapper()
        {
            var config = new TypeAdapterConfig();
            config.ConfigureWebMappings();
            return new Mapper(config);
        }

        private Mock<IClaimsBasedIdentityService> GetMockSecurityService(User user)
        {
            var mockSecurityService = new Mock<IClaimsBasedIdentityService>();

            mockSecurityService
                .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);
            return mockSecurityService;
        }

        private Mock<IFundingApiService> GetFundingApiService_SingleMatchingProviderFundingResult(string ukprnForFunding, string fundingstreamCode)
        {
            var fundingApiService = new Mock<IFundingApiService>(MockBehavior.Strict);
            var fundingValue = "123.45";
            var providerfundings = new List<IFundingApiSearchProviderFunding>
            {
                new FundingApiSearchProviderFunding
                {
                    FundingValue = fundingValue,
                    OrganisationUkprn = ukprnForFunding,
                    OrganisationName = ProviderNameFromProviderFunding,
                    FundingStreamCode = fundingstreamCode,
                    ParentProviderType = GroupingType.LocalAuthority
                }
            };
            var fundings = new List<IFundingApiSearchFunding>
            {
                new FundingApiSearchFunding
                {
                    FundingValue = fundingValue,
                    FundingStreamCode = fundingstreamCode
                }
            };

            fundingApiService
                .Setup(fas => fas.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false))
                .ReturnsAsync(new ProviderFundingApiSearchResponse { ProviderFunding = providerfundings });

            fundingApiService
                .Setup(fas => fas.SearchFunding(It.IsAny<FundingApiSearchRequestObject>()))
                .ReturnsAsync(new FundingApiSearchResponse { Funding = fundings });

            return fundingApiService;
        }

        private Mock<IFundingApiService> GetFundingApiService_SingleMatchingFundingResult(string ukprnForFunding, string fundingstreamCode)
        {
            var fundingApiService = new Mock<IFundingApiService>(MockBehavior.Strict);
            var fundingValue = "123.45";
            var providerfundings = new List<IFundingApiSearchProviderFunding>
            {
                new FundingApiSearchProviderFunding
                {
                    FundingValue = fundingValue,
                    FundingStreamCode = fundingstreamCode,
                    ParentProviderType = GroupingType.LocalAuthority
                }
            };
            var fundings = new List<IFundingApiSearchFunding>
            {
                new FundingApiSearchFunding
                {
                    FundingValue = fundingValue,
                    GroupUkprn = ukprnForFunding,
                    GroupName = ProviderNameFromFunding,
                    FundingStreamCode = fundingstreamCode
                }
            };

            fundingApiService
                .Setup(fas => fas.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false))
                .ReturnsAsync(new ProviderFundingApiSearchResponse { ProviderFunding = providerfundings });

            fundingApiService
                .Setup(fas => fas.SearchFunding(It.IsAny<FundingApiSearchRequestObject>()))
                .ReturnsAsync(new FundingApiSearchResponse { Funding = fundings });

            return fundingApiService;
        }

        private Mock<IFundingDocumentService> GetNationalFundingAllocationMockFundingDocumentService(
            string fundingStreamCode,
            string fundingStreamName,
            string fundingPeriodCode,
            int yearFrom,
            int yearTo,
            int documentCount)
        {
            var mockFundingDocumentService = new Mock<IFundingDocumentService>();

            //arrange
            mockFundingDocumentService
                .Setup(s => s.GetFundingDocuments(
                    fundingStreamCode,
                    fundingStreamName,
                    fundingPeriodCode,
                    It.IsAny<ICollection<Publication>>(),
                    It.IsAny<string>()))
                .ReturnsAsync(GetNationalFundingAllocationMockFundingDocuments(
                    fundingStreamCode,
                    fundingStreamName,
                    yearFrom,
                    yearTo,
                    documentCount,
                    false))
                .Verifiable();

            return mockFundingDocumentService;
        }

        private Mock<IFundingDocumentService> GetNationalFundingAllocationMockFundingDocumentServiceNoDocuments(
            string fundingStreamCode,
            string fundingStreamName,
            string fundingPeriodCode)
        {
            var mockFundingDocumentService = new Mock<IFundingDocumentService>();

            //arrange
            mockFundingDocumentService
                .Setup(s => s.GetFundingDocuments(
                    fundingStreamCode,
                    fundingStreamName,
                    fundingPeriodCode,
                    It.IsAny<ICollection<Publication>>(),
                    It.IsAny<string>()))
                .ReturnsAsync((IReadOnlyCollection<FundingDocument>)null)
                .Verifiable();

            return mockFundingDocumentService;
        }

        private Mock<IFundingDocumentService> GetMockFundingDocumentService(
             string fundingStreamCode,
             string fundingPeriodCode,
             int yearFrom,
             int yearTo,
             int documentCount)
        {
            var mockFundingDocumentService = new Mock<IFundingDocumentService>();

            //arrange
            mockFundingDocumentService
                .Setup(s => s.GetFundingDocuments(
                    fundingStreamCode,
                    It.IsAny<string>(),
                    fundingPeriodCode,
                    It.IsAny<ICollection<Publication>>(),
                    It.IsAny<string>()))
                .ReturnsAsync(GetMockFundingDocuments(
                    fundingStreamCode,
                    yearFrom,
                    yearTo,
                    documentCount,
                    false))
                .Verifiable();

            return mockFundingDocumentService;
        }

        private Mock<IFundingViewService> GetNationalFundingAllocationMockFundingViewService(
            string fundingStreamCode,
            string fundingStreamName,
            string fundingPeriodCode,
            int yearFrom,
            int yearTo)
        {
            var mockViewService = new Mock<IFundingViewService>();

            mockViewService
                .Setup(s => s.GenerateFundingViewData(
                    It.IsAny<IComponentService>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<FundingStream[]>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<Publication>(),
                    It.IsAny<int?>(),
                    It.Is<FundingViewScope>(scope => scope.ToString().StartsWith("National")),
                    It.IsAny<Dictionary<ComponentType, Defaults>>(),
                    It.IsAny<FundingDocument>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<SearchFilter[]>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<IFundingApiSearchFunding[]>(),
                    It.IsAny<IFundingApiSearchProviderFunding[]>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<PreviewLayoutModel>(),
                    It.IsAny<VarianceSelectionOption>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(ExpectedNationalFundingAllocationFundingData(
                                fundingStreamCode,
                                fundingStreamName,
                                fundingPeriodCode,
                                yearFrom,
                                yearTo)[fundingStreamCode]);

            return mockViewService;
        }

        private Mock<IFundingViewService> GetMockFundingViewService()
        {
            var mockViewService = new Mock<IFundingViewService>();

            mockViewService
                .Setup(s => s.GenerateFundingViewData(
                    It.IsAny<IComponentService>(),
                    DSGFundingPeriodCode,
                    It.IsAny<string>(),
                    It.Is<FundingStream[]>(x => x.Length == 1 && x[0].FundingStreamCode == FundingStreamCode.DSG),
                    It.IsAny<DateTime>(),
                    It.IsAny<Publication>(),
                    It.IsAny<int?>(),
                    It.Is<FundingViewScope>(scope => scope.ToString().StartsWith("Organisation")),
                    It.IsAny<Dictionary<ComponentType, Defaults>>(),
                    It.IsAny<FundingDocument>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<SearchFilter[]>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<IFundingApiSearchFunding[]>(),
                    It.IsAny<IFundingApiSearchProviderFunding[]>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<PreviewLayoutModel>(),
                    It.IsAny<VarianceSelectionOption>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(ExpectedLocalAuthorityFundingData(TestLocalAuthorityCode)[$"{DSGFundingPeriodCode}-{FundingStreamCode.DSG}"]);

            mockViewService
                .Setup(s => s.GenerateFundingViewData(
                    It.IsAny<IComponentService>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<FundingStream[]>(x => x.Length == 1 && x[0].FundingStreamCode == FundingStreamCode.PEAndSport),
                    It.IsAny<DateTime>(),
                    It.IsAny<Publication>(),
                    It.IsAny<int?>(),
                    It.Is<FundingViewScope>(scope => scope.ToString().StartsWith("Organisation")),
                    It.IsAny<Dictionary<ComponentType, Defaults>>(),
                    It.IsAny<FundingDocument>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<SearchFilter[]>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<IFundingApiSearchFunding[]>(),
                    It.IsAny<IFundingApiSearchProviderFunding[]>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<PreviewLayoutModel>(),
                    It.IsAny<VarianceSelectionOption>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(ExpectedLocalAuthorityFundingData(TestLocalAuthorityCode)[$"{PSGFundingPeriodCode}-{FundingStreamCode.PEAndSport}"]);

            mockViewService
                .Setup(s => s.GenerateFundingViewData(
                    It.IsAny<IComponentService>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.Is<FundingStream[]>(x => x.Length == 1 && x[0].FundingStreamCode == FundingStreamCode.PEAndSport),
                    It.IsAny<DateTime>(),
                    It.IsAny<Publication>(),
                    It.IsAny<int?>(),
                    It.Is<FundingViewScope>(scope => scope.ToString().StartsWith("Provider")),
                    It.IsAny<Dictionary<ComponentType, Defaults>>(),
                    It.IsAny<FundingDocument>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<SearchFilter[]>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<IFundingApiSearchFunding[]>(),
                    It.IsAny<IFundingApiSearchProviderFunding[]>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<PreviewLayoutModel>(),
                    It.IsAny<VarianceSelectionOption>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(ExpectedProviderFundingData(OrganisationUkprn)[FundingStreamCode.PEAndSport]);

            mockViewService
                .Setup(s => s.GetDataRequirements(
                    It.IsAny<FundingStream>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<FundingViewScope>(),
                    It.IsAny<SearchFilter[]>(),
                    It.IsAny<IFundingApiSearchFunding>(),
                    It.IsAny<IFundingApiSearchProviderFunding>(),
                    It.IsAny<string>()))
                .Returns(new List<FundingApiSearchRequestObject>());

            return mockViewService;
        }

        private Mock<IUserJourneyService> GetMockUserJourneyServiceNoFundingStreams()
        {
            var mockSettingsService = new Mock<IUserJourneyService>();
            mockSettingsService
               .Setup(s => s.GetFundingStreams())
               .ReturnsAsync(new List<Services.Models.FundingStream>());

            return mockSettingsService;
        }

        private Mock<IUserJourneyService> GetMockUserJourneyServiceNoPublications()
        {
            var mockSettingsService = new Mock<IUserJourneyService>();

            mockSettingsService
               .Setup(s => s.GetFundingStreams())
               .ReturnsAsync(new List<Services.Models.FundingStream>
               {
                   new Services.Models.FundingStream
                   {
                     Id = 1,
                     Active = true,
                     FundingStreamCode = "PSG",
                     FundingStreamName = "PE and sport premium",
                     SettingValues = new List<SettingValue>()
                     {
                         new SettingValue
                         {
                             FundingStreamId = 1,
                             SettingId = 1,
                             Value = "201920",
                             CreatedAt = DateTime.Now,
                             LastUpdatedAt = DateTime.Now,
                             LastUpdatedBy = "System",
                             Setting = new SettingType
                             {
                                 SettingName = "AcademicYear"
                             }
                         }
                     },
                     RelevantForNational = true,
                     RelevantForOrganisations_LoggedIn = true,
                     RelevantForOrganisations_Public = true,
                     RelevantForProviders_LoggedIn = true,
                     RelevantForProviders_Public = true
                   },
                   new Services.Models.FundingStream
                   {
                     Id = 2,
                     Active = true,
                     FundingStreamCode = "DSG",
                     FundingStreamName = "Dedicated schools grant",
                     SettingValues = new List<SettingValue>()
                     {
                         new SettingValue
                         {
                             FundingStreamId = 2,
                             SettingId = 2,
                             Value = "202021",
                             CreatedAt = DateTime.Now,
                             LastUpdatedAt = DateTime.Now,
                             LastUpdatedBy = "System",
                             Setting = new SettingType
                             {
                                 SettingName = "FinancialYear"
                             }
                         }
                     },
                     RelevantForNational = true,
                     RelevantForOrganisations_LoggedIn = true,
                     RelevantForOrganisations_Public = true,
                     RelevantForProviders_LoggedIn = false,
                     RelevantForProviders_Public = false
                   }
               });

            return mockSettingsService;
        }

        private Mock<IUserJourneyService> GetMockUserJourneyServiceNoMatchingPublications()
        {
            var mockSettingsService = new Mock<IUserJourneyService>();

            mockSettingsService
               .Setup(s => s.GetFundingStreams())
               .ReturnsAsync(new List<Services.Models.FundingStream>
               {
                   new Services.Models.FundingStream
                   {
                     Id = 1,
                     Active = true,
                     FundingStreamCode = "PSG",
                     FundingStreamName = "PE and sport premium",
                     SettingValues = new List<SettingValue>()
                     {
                         new SettingValue
                         {
                             FundingStreamId = 1,
                             SettingId = 1,
                             Value = "201920",
                             CreatedAt = DateTime.Now,
                             LastUpdatedAt = DateTime.Now,
                             LastUpdatedBy = "System",
                             Setting = new SettingType
                             {
                                 SettingName = "AcademicYear"
                             }
                         }
                     },
                     Publications = ExpectedNonMatchingPublications(PSGFundingPeriodCode),
                     RelevantForNational = true,
                     RelevantForOrganisations_LoggedIn = true,
                     RelevantForOrganisations_Public = true,
                     RelevantForProviders_LoggedIn = true,
                     RelevantForProviders_Public = true
                   },
                   new Services.Models.FundingStream
                   {
                     Id = 2,
                     Active = true,
                     FundingStreamCode = "DSG",
                     FundingStreamName = "Dedicated schools grant",
                     SettingValues = new List<SettingValue>()
                     {
                         new SettingValue
                         {
                             FundingStreamId = 2,
                             SettingId = 2,
                             Value = "202021",
                             CreatedAt = DateTime.Now,
                             LastUpdatedAt = DateTime.Now,
                             LastUpdatedBy = "System",
                             Setting = new SettingType
                             {
                                 SettingName = "FinancialYear"
                             }
                         }
                     },
                     Publications = ExpectedNonMatchingPublications(DSGFundingPeriodCode),
                     RelevantForNational = true,
                     RelevantForOrganisations_LoggedIn = true,
                     RelevantForOrganisations_Public = true,
                     RelevantForProviders_LoggedIn = true,
                     RelevantForProviders_Public = true
                   }
               });

            return mockSettingsService;
        }

        private Mock<IUserJourneyService> GetMockSettingsService()
        {
            var mockSettingsService = new Mock<IUserJourneyService>();
            mockSettingsService
               .Setup(s => s.GetFundingStreams())
               .ReturnsAsync(new List<Services.Models.FundingStream>
               {
                   new Services.Models.FundingStream
                   {
                     Id = 1,
                     Active = true,
                     FundingStreamCode = "PSG",
                     FundingStreamName = "PE and sport premium",
                     FundingStreamNameWithinSentence = "PE and sport premium",
                     SettingValues = new List<SettingValue>()
                     {
                         new SettingValue
                         {
                             FundingStreamId = 1,
                             SettingId = 1,
                             Value = "201920",
                             CreatedAt = DateTime.Now,
                             LastUpdatedAt = DateTime.Now,
                             LastUpdatedBy = "System",
                             Setting = new SettingType
                             {
                                 SettingName = "AcademicYear"
                             }
                         },
                         new SettingValue
                         {
                             FundingStreamId = 1,
                             SettingId = 2,
                             Value = "5000",
                             CreatedAt = DateTime.MaxValue,
                             LastUpdatedAt = DateTime.MaxValue,
                             LastUpdatedBy = "System",
                             Setting = new SettingType
                             {
                                 SettingName = "ProviderDownloadSizeInBytes"
                             }
                         }
                     },
                     Publications = ExpectedPublications(PSGFundingPeriodCode),
                     NextPayments = new List<NextPayment>(),
                     FundingStreamCodePubliclyKnown = false,
                     RelevantForNational = true,
                     RelevantForOrganisations_LoggedIn = true,
                     RelevantForOrganisations_Public = true,
                     RelevantForProviders_LoggedIn = true,
                     RelevantForProviders_Public = true
                   },
                   new Services.Models.FundingStream
                   {
                     Id = 2,
                     Active = true,
                     FundingStreamCode = "DSG",
                     FundingStreamName = "Dedicated schools grant",
                     FundingStreamNameWithinSentence = "dedicated schools grant",
                     SettingValues = new List<SettingValue>()
                     {
                         new SettingValue
                         {
                             FundingStreamId = 2,
                             SettingId = 2,
                             Value = "202021",
                             CreatedAt = DateTime.Now,
                             LastUpdatedAt = DateTime.Now,
                             LastUpdatedBy = "System",
                             Setting = new SettingType
                             {
                                 SettingName = "FinancialYear"
                             }
                         }
                     },
                     Publications = ExpectedPublications(DSGFundingPeriodCode),
                     NextPayments = new List<NextPayment>(),
                     FundingStreamCodePubliclyKnown = true,
                     RelevantForNational = true,
                     RelevantForOrganisations_LoggedIn = true,
                     RelevantForOrganisations_Public = true,
                     RelevantForProviders_LoggedIn = false,
                     RelevantForProviders_Public = false
                   }
               });

            return mockSettingsService;
        }

        private Mock<IFundingApiService> GetMockFundingApiService(
            string searchTerm,
            int? localAuthoritySearchResultCount = null,
            int? providerSearchResultCount = null,
            int? fundingSearchResultCount = null,
            string providerType = "Maintained",
            string fundingPeriodCode = PSGFundingPeriodCode)
        {
            var mockFundingApiService = new Mock<IFundingApiService>();

            if (localAuthoritySearchResultCount != null)
            {
                mockFundingApiService
                    .Setup(s => s.SearchLocalAuthorities(
                        It.Is<FundingApiSearchLocalAuthoritiesRequest>(r =>
                            r.SearchTerm == searchTerm &&

                            r.FundingStreamConfiguration.Any(c =>
                                c.Key == FundingStreamCode.PEAndSport &&
                                c.Value.FundingStreamCode == FundingStreamCode.PEAndSport) &&

                            r.FundingStreamConfiguration.Any(c =>
                                c.Key == FundingStreamCode.DSG &&
                                c.Value.FundingStreamCode == FundingStreamCode.DSG))))
                    .ReturnsAsync(new FundingApiSearchLocalAuthoritiesResponse
                    {
                        LocalAuthorities = Enumerable.Range(0, localAuthoritySearchResultCount.Value)
                            .ToDictionary(k => k.ToString(), k => k.ToString())
                    })
                    .Verifiable();
            }

            if (providerSearchResultCount != null)
            {
                mockFundingApiService
                    .Setup(s => s.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false))
                    .ReturnsAsync(new ProviderFundingApiSearchResponse
                    {
                        ProviderFunding = GetMockProviderResults(providerSearchResultCount.Value, providerType, fundingPeriodCode)
                    })
                    .Verifiable();
            }

            if (fundingSearchResultCount != null)
            {
                mockFundingApiService
                    .Setup(s => s.SearchFunding(It.IsAny<FundingApiSearchRequestObject>()))
                    .ReturnsAsync(new FundingApiSearchResponse
                    {
                        Funding = GetMockFundingResults(fundingSearchResultCount.Value, fundingPeriodCode)
                    })
                    .Verifiable();
            }

            return mockFundingApiService;
        }

        /// <summary>
        /// Gets the mock configuration service.
        /// </summary>
        /// <returns>The ApplicationConfiguration options.</returns>
        private Mock<IOptions<ApplicationConfiguration>> GetMockConfigService()
        {
            var mockConfigService = new Mock<IOptions<ApplicationConfiguration>>();

            mockConfigService.Setup(x => x.Value)
                .Returns(new ApplicationConfiguration
                {
                    ContactUsLink = "http://www.vyf-contactus.com/contactus",
                    FeedbackLink = "http://www.vyf-survey.com/survey",
                    TerminatedLocalAuthority = new TerminatedLocalAuthority
                    {
                        FundingPeriodCode = string.Empty,
                        LocalAuthorityCode = string.Empty
                    }
                });

            return mockConfigService;
        }

        private List<FundingStreamCurrentYearViewModel> GetLatestYear()
        {
            return new List<FundingStreamCurrentYearViewModel>
            {
                new FundingStreamCurrentYearViewModel
                {
                     FundingStreamCode = "PSG",
                     FundingStreamName = "PE and sport premium",
                     YearStart = 2019,
                     YearEnd = 2020
                },
                new FundingStreamCurrentYearViewModel
                {
                     FundingStreamCode = "DSG",
                     FundingStreamName = "Dedicated schools grant",
                     YearStart = 2020,
                     YearEnd = 2021
                }
            };
        }

        private List<(int yearFrom, int yearTo)> GetHistoricYears(int currentYearFrom, int currentYearTo)
        {
            var currentAndHistoricYears = new List<(int, int)>();

            for (var numberOfYearsAgo = 1;
                numberOfYearsAgo <= ViewYourFundingConstants.NumberOfYearsOfHistoricAllocationsToShow;
                numberOfYearsAgo++)
            {
                var historicYearFrom = currentYearFrom - numberOfYearsAgo;

                currentAndHistoricYears.Add((historicYearFrom, historicYearFrom + 1));
            }

            return currentAndHistoricYears;
        }

        #endregion
    }
}