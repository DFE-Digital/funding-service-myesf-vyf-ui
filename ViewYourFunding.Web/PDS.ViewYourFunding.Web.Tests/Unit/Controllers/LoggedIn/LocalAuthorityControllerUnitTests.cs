using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Utils;
using Pds.Core.Web.Models;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Attributes;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Implementations.Hacks;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Controllers;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Exceptions;
using PDS.ViewYourFunding.Web.Extensions;
using PDS.ViewYourFunding.Web.Models.Request;
using PDS.ViewYourFunding.Web.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LocalAuthorityBreakdownViewModel = PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.LocalAuthorityBreakdownViewModel;
using LocalAuthorityFundingBreakdownRequest = PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.Requests.LocalAuthorityFundingBreakdownRequest;
using LocalAuthorityRecoupmentDetailRequest = PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.Requests.LocalAuthorityRecoupmentDetailRequest;
using LocalAuthorityRecoupmentDetailViewModel = PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.LocalAuthorityRecoupmentDetailViewModel;
using User = Pds.Core.Common.Identity.Models.User;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers.LoggedIn
{
    [TestClass]
    public class LocalAuthorityControllerUnitTests
    {
        private static readonly LocalAuthorityFundingBreakdownRequest LocalAuthorityFundingBreakdown1619Request = new LocalAuthorityFundingBreakdownRequest
        {
            Ukprn = "10072811"
        };

        private static readonly LocalAuthorityRecoupmentDetailRequest LocalAuthorityRecoupmentDetailRequest = new LocalAuthorityRecoupmentDetailRequest
        {
            Ukprn = "10072811"
        };

        private static readonly Local1619_FakeApiService Local1619FakeApiService = new Local1619_FakeApiService(null);
        private static readonly LocalLAREC_FakeApiService LocalLAREC_FakeApiService = new LocalLAREC_FakeApiService(null);

        private readonly string _contactUsLink;
        private readonly string _feedbackLink;

        public LocalAuthorityControllerUnitTests()
        {
            var appConfig = new ApplicationConfiguration();
            _contactUsLink = appConfig.ContactUsLink;
            _feedbackLink = appConfig.FeedbackLinkForLoggedInView;
        }

        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityBreakdown_MockFundingViewService_ResultExpected()
        {
            // Arrange
            LocalAuthorityFundingBreakdown1619Request.FundingStreamNamePathPart = "16-to-19-funding";
            LocalAuthorityFundingBreakdown1619Request.PublishedDate = new DateTime(2030, 1, 1).ToString("d-M-yyyy"); //DateTime.MinValue.ToString("d-M-yyyy");
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Truro And Penwith College",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_Only1619().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(isLaSsf: true, isLa: true).Object,
                GetFundingViewService_Only1619().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            var expectedViewModel = new LocalAuthorityBreakdownViewModel
            {
                OrganisationName = "Hertfordshire",
                ChoicePageLink = "/choose-a-statement-type",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                Funding = GetFunding(),
                FundingStream = Get1619FundingStream(),
                OrganisationUkPrn = "10072811",
                FundingViewData = new FundingViewData
                {
                    EntityName = "Hertfordshire",
                    FundingStreamCode = "1619",
                    TotalAmount = 2906249.75M,
                    Components = new List<Component>
                    {
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Title
                        },
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Panel
                        }
                    }
                },
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName
                }
            };

            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";
            expectedViewModel.YearFrom = 2021;
            expectedViewModel.YearTo = 2022;

            // Act
            var actual = await controller.LocalAuthorityFundingBreakdown(LocalAuthorityFundingBreakdown1619Request);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityBreakdownViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod, TestCategory("Unit")]
        public void LocalAuthorityBreakdown_InvalidFundingStream_ThrowsExpectedResult()
        {
            // Arrange
            LocalAuthorityFundingBreakdown1619Request.FundingStreamNamePathPart = "dummy";
            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_Only1619().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(isLaSsf: true, isLa: true).Object,
                GetFundingViewService_Only1619().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            // Act
            Func<Task> act = async () => { await controller.LocalAuthorityFundingBreakdown(LocalAuthorityFundingBreakdown1619Request); };

            // Assert
            act.Should().ThrowAsync<RequestException>().WithMessage($"Funding stream not found for {LocalAuthorityFundingBreakdown1619Request.FundingStreamNamePathPart}");
        }

        [TestMethod, TestCategory("Unit")]
        public void LocalAuthorityBreakdown_NoFundingResults_ThrowsExpectedResult()
        {
            // Arrange
            LocalAuthorityFundingBreakdown1619Request.FundingStreamNamePathPart = "16-to-19-funding";
            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_Only1619(false).Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                GetFundingViewService_Only1619().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            // Act
            Func<Task> act = async () => { await controller.LocalAuthorityFundingBreakdown(LocalAuthorityFundingBreakdown1619Request); };

            // Assert
            act.Should().ThrowAsync<RequestException>().WithMessage($"No funding data found for {LocalAuthorityFundingBreakdown1619Request.Ukprn}");
        }

        [TestMethod, TestCategory("Unit")]
        public void LocalAuthorityBreakdown_NoGroupType_ThrowsExpectedResult()
        {
            // Arrange
            LocalAuthorityFundingBreakdown1619Request.FundingStreamNamePathPart = "16-to-19-funding";
            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_Only1619(false).Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(true).Object,
                GetFundingViewService_Only1619().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            // Act
            Func<Task> act = async () => { await controller.LocalAuthorityFundingBreakdown(LocalAuthorityFundingBreakdown1619Request); };

            // Assert
            act.Should().ThrowAsync<RequestException>().WithMessage($"No expected grouping type data found for {LocalAuthorityFundingBreakdown1619Request.Ukprn}");
        }

        [TestMethod, TestCategory("Unit")]
        public void LocalAuthorityHistory_NoFundingStreams_MockFundingViewService_ThrowsExpectedResult()
        {
            // Arrange
            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_NoFundingStreams().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                new Mock<IFundingViewService>(MockBehavior.Strict).Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[]
                        {
                            new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                        }, "someAuthTypeName"))
                }
            };

            var fundingStreamNamePathPart = "invalid-name-part";

            // Act
            Func<Task> act = async () => await controller.LocalAuthorityHistory("10072811", fundingStreamNamePathPart);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage($"There are no funding streams for {fundingStreamNamePathPart} for 10072811.");
        }

        [TestMethod, TestCategory("Unit")]
        public void LocalAuthorityHistory_NoPublications_MockFundingViewService_ThrowsExpectedResult()
        {
            // Arrange
            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_Only1619_NoPublications().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                new Mock<IFundingViewService>(MockBehavior.Strict).Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[]
                        {
                            new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                        }, "someAuthTypeName"))
                }
            };

            var fundingStreamCode = "1619";
            var fundingStreamNamePathPart = "16-to-19-funding";

            // Act
            Func<Task> act = async () => await controller.LocalAuthorityHistory("10072811", fundingStreamNamePathPart);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage($"There are no publications for funding stream {fundingStreamCode}");
        }

        [TestMethod, TestCategory("Unit")]
        public void LocalAuthorityHistory_Only1619_MockFundingViewService_ThrowsExpectedResult()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Test Provider",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_Only1619().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(false, false, true).Object,
                GetFundingViewService_Only1619().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[]
                        {
                            new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                        }, "someAuthTypeName"))
                }
            };

            var fundingStreamNamePathPart = "16-to-19-funding";


            // Act
            Func<Task> act = async () => await controller.LocalAuthorityHistory("10004801", fundingStreamNamePathPart);

            // Assert
            act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("You are not authorised to perform this action.");
        }


        [TestMethod, TestCategory("Unit")]
        public async Task LocalAuthorityHistory_Only1619_MockFundingViewService_ResultAsExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Truro And Penwith College",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_Only1619().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(false, false, true).Object,
                GetFundingViewService_Only1619().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[]
                            {
                        new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                            },
                            "someAuthTypeName"))
                }
            };

            var fundingPeriodVM2021 = new LocalAuthorityFundingViewModel
            {
                FundingPeriodCode = "AS-2021",
                IsFinal = true,
                IsLatest = false,
                StatusChangedDate = new DateTime(2020, 1, 10),
                VariationReason = "Initial allocation."
            };

            var fundingPeriodVM2022 = new LocalAuthorityFundingViewModel
            {
                FundingPeriodCode = "AS-2122",
                IsFinal = false,
                IsLatest = true,
                StatusChangedDate = new DateTime(2021, 1, 10),
                VariationReason = "Initial allocation."
            };

            var expectedViewModel = new LocalAuthorityHistoryViewModel
            {
                ChoicePageLink = "/choose-a-statement-type",
                OrganisationName = "Hertfordshire",
                OrganisationUkprn = "10072811",
                SecondaryContentTitle = "16 to 19 funding",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingPeriodPublications = new List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>>(),
                FundingPeriodLocalAuthorityFundings =
                    new List<KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>>
                    {
                new KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>(
                    (2021, 2022),
                    new List<LocalAuthorityFundingViewModel> { fundingPeriodVM2022 }),
                new KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>(
                    (2020, 2021),
                    new List<LocalAuthorityFundingViewModel> { fundingPeriodVM2021 })
                    },
                FundingViewData = new FundingViewData
                {
                    FundingStreamCode = "1619",
                    TotalAmount = 2906249.75M,
                    EntityName = "Hertfordshire",
                    Components = new List<Component>
            {
                new Component(null)
                {
                    Type = ComponentType.Accordion_Title,
                    PageData = new Dictionary<string, object>
                    {
                        {
                            "FundingPeriodPublications",
                            new List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>>()
                        },
                        {
                            "FundingPeriodLocalAuthorityFundings",
                            new List<KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>>
                            {
                                new KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>(
                                    (2021, 2022),
                                    new List<LocalAuthorityFundingViewModel> { fundingPeriodVM2022 }),
                                new KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>(
                                    (2020, 2021),
                                    new List<LocalAuthorityFundingViewModel> { fundingPeriodVM2021 })
                            }
                        }
                    }
                },
                new Component(null)
                {
                    Type = ComponentType.Accordion_Panel
                }
            }
                },
                FundingStream = new Web.Areas.Admin.Models.FundingStream.FundingStream
                {
                    FundingStreamCode = "1619",
                    FundingStreamName = "16 to 19 funding",
                    FundingStreamNameWithinSentence = "16 to 19 funding",
                    RelevantForProviders_LoggedIn = true,
                    RelevantForOrganisations_LoggedIn = true,
                    RelevantForOrganisations_Public = true,
                    HistoryIndependentOfPublications = true,
                    Active = true,
                    SettingValues = new List<SettingValue>
            {
                new SettingValue
                {
                    Setting = new SettingType
                    {
                        SettingName = SettingName.AcademyAndSchoolAcademicYear
                    },
                    Value = "202122"
                },
                new SettingValue
                {
                    Setting = new SettingType
                    {
                        SettingName = "UseStaticData"
                    },
                    Value = "True"
                },
                new SettingValue
                {
                    Setting = new SettingType
                    {
                        SettingName = "ParentProviderType"
                    },
                    Value = "LocalAuthority"
                }
            },
                    NextPayments = new List<NextPayment>(),
                    Publications = new List<Publication>
            {
                new Publication
                {
                    PublishedDate = new DateTime(2030, 1, 1),
                    FundingPeriodCode = "AS-2122",
                    IsLatest = true,
                    Status = PublicationStatus.Published
                }
            }
                },
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName
                }
            };
            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";

            var fundingStreamNamePathPart = "16-to-19-funding";

            // Act
            var actual = await controller.LocalAuthorityHistory(
                "10072811",
                fundingStreamNamePathPart);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityHistoryViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(info => info.Path.EndsWith("PublicationLayouts"))
                        .Excluding(info => info.Path.EndsWith("Setting.SettingValues")));
        }

        [TestMethod, TestCategory("Integration")]
        public async Task LocalAuthorityHistory_Only1619_RealFundingViewService_ResultAsExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Truro And Penwith College",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var modelFileStoreService = new Mock<IModelFileStoreService>(MockBehavior.Strict);
            modelFileStoreService
                .Setup(mfss => mfss.GetModelFilenames(It.IsAny<string>()))
                .Returns(new[]
                {
            "1619_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInOrganisationHistory.json"
                });

            modelFileStoreService
                .Setup(mfss => mfss.Exists(It.IsAny<string>()))
                .Returns(true);

            modelFileStoreService
                .Setup(mfss => mfss.ReadFileAsString(It.IsAny<string>()))
                .Returns(@"{""dataset"": [{""datasetName"":""providerFunding"",""expression"": ""ParentProviderType=LocalAuthority&GroupingReason=Information""}], ""groups"": [{""type"":""HeadingAlternativeTitle"",},{""type"":""HtmlParagraph""}]}");

            var realFundingViewService = new ModelFundingViewService(
                GetGlobalSettingService().Object,
                modelFileStoreService.Object,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                null,
                null,
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                null,
                null,
                new MemoryCacheService(null, 0));

            var componentService = new Mock<IComponentService>(MockBehavior.Strict);
            componentService
                .Setup(cs => cs.GetComponent(
                    It.Is<UiModelGroup>(grp => grp.Type == "HeadingAlternativeTitle"),
                    It.IsAny<ComponentConfiguration>(),
                    It.IsAny<ComponentConfiguration>(),
                    It.IsAny<Dictionary<ComponentType, Defaults>>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Returns(new Component(null)
                {
                    Type = ComponentType.Accordion_Title
                });

            componentService
                .Setup(cs => cs.GetComponent(
                    It.Is<UiModelGroup>(grp => grp.Type == "HtmlParagraph"),
                    It.IsAny<ComponentConfiguration>(),
                    It.IsAny<ComponentConfiguration>(),
                    It.IsAny<Dictionary<ComponentType, Defaults>>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Returns(new Component(null)
                {
                    Type = ComponentType.Accordion_Panel
                });

            var controller = new LocalAuthorityController(
                componentService.Object,
                GetIdentityService().Object,
                GetUserJourneyService_Only1619().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                realFundingViewService,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);

            var fundingPeriodVM2021 = new LocalAuthorityFundingViewModel
            {
                FundingPeriodCode = "AS-2021",
                IsFinal = true,
                IsLatest = false,
                StatusChangedDate = new DateTime(2020, 1, 10),
                VariationReason = "Initial allocation."
            };

            var fundingPeriodVM2022 = new LocalAuthorityFundingViewModel
            {
                FundingPeriodCode = "AS-2122",
                IsFinal = false,
                IsLatest = true,
                StatusChangedDate = new DateTime(2021, 1, 10),
                VariationReason = "Initial allocation."
            };

            var expectedViewModel = new LocalAuthorityHistoryViewModel
            {
                ChoicePageLink = "/choose-a-statement-type",
                OrganisationName = "Hertfordshire",
                OrganisationUkprn = "10072811",
                SecondaryContentTitle = "16 to 19 funding",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingPeriodPublications = new List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>>(),
                FundingPeriodLocalAuthorityFundings =
                    new List<KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>>
                    {
                new KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>(
                    (2021, 2022),
                    new List<LocalAuthorityFundingViewModel> { fundingPeriodVM2022 }),
                new KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>(
                    (2020, 2021),
                    new List<LocalAuthorityFundingViewModel> { fundingPeriodVM2021 })
                    },
                FundingViewData = new FundingViewData
                {
                    FundingStreamCode = "1619",
                    TotalAmount = 1.23M,
                    EntityPrimaryIdentifier = "10007063",
                    EntityAlternativeIdentifier = "1234",
                    EntityType = "Furth",
                    EntitySubType = "11ACA",
                    EntityName = "Truro and Penwith College",
                    FundingPeriodCode = "AS-2122",
                    PublicationDate = new DateTime(2030, 1, 1),
                    InYearOpener = false,
                    IsIndicativeFunding = false,
                    Components = new List<Component>
            {
                new Component(null)
                {
                    Type = ComponentType.Accordion_Title,
                    PageData = new Dictionary<string, object>
                    {
                        {
                            "FundingPeriodPublications",
                            new List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>>()
                        },
                        {
                            "FundingPeriodLocalAuthorityFundings",
                            new List<KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>>
                            {
                                new KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>(
                                    (2021, 2022),
                                    new List<LocalAuthorityFundingViewModel> { fundingPeriodVM2022 }),
                                new KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>(
                                    (2020, 2021),
                                    new List<LocalAuthorityFundingViewModel> { fundingPeriodVM2021 })
                            }
                        }
                    }
                },
                new Component(null)
                {
                    Type = ComponentType.Accordion_Panel
                }
            }
                },
                FundingStream = new Web.Areas.Admin.Models.FundingStream.FundingStream
                {
                    FundingStreamCode = "1619",
                    FundingStreamName = "16 to 19 funding",
                    FundingStreamNameWithinSentence = "16 to 19 funding",
                    RelevantForProviders_LoggedIn = true,
                    RelevantForOrganisations_LoggedIn = true,
                    RelevantForOrganisations_Public = true,
                    HistoryIndependentOfPublications = true,
                    Active = true,
                    SettingValues = new List<SettingValue>
            {
                new SettingValue
                {
                    Setting = new SettingType
                    {
                        SettingName = SettingName.AcademyAndSchoolAcademicYear
                    },
                    Value = "202122"
                },
                new SettingValue
                {
                    Setting = new SettingType
                    {
                        SettingName = "UseStaticData"
                    },
                    Value = "True"
                },
                new SettingValue
                {
                    Setting = new SettingType
                    {
                        SettingName = "ParentProviderType"
                    },
                    Value = "LocalAuthority"
                }
            },
                    NextPayments = new List<NextPayment>(),
                    Publications = new List<Publication>
            {
                new Publication
                {
                    PublishedDate = new DateTime(2030, 1, 1),
                    FundingPeriodCode = "AS-2122",
                    IsLatest = true,
                    Status = PublicationStatus.Published
                }
            }
                },
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName
                }
            };
            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";

            var fundingStreamNamePathPart = "16-to-19-funding";

            // Act
            var actual = await controller.LocalAuthorityHistory(
                "10072811",
                fundingStreamNamePathPart);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityHistoryViewModel>()
                .Which.Should().BeEquivalentTo(
                    expectedViewModel,
                    options => options
                        .Excluding(info => info.Path.EndsWith("PublicationLayouts"))
                        .Excluding(info => info.Path.EndsWith("Setting.SettingValues")));
        }

        [TestMethod, TestCategory("Unit")]
        public void LARecoupmentHistory_NoRelevantFundingStreams_MockFundingViewService_ThrowsExpectedResult()
        {
            // Arrange
            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_NoFundingStreams().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                new Mock<IFundingViewService>(MockBehavior.Strict).Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[]
                        {
                            new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                        }, "someAuthTypeName"))
                }
            };

            var fundingStreamNamePathPart = "invalid-name-part";

            // Act
            Func<Task> act = async () => await controller.LARecoupmentHistory("10007063", fundingStreamNamePathPart);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage($"There are no funding streams for {fundingStreamNamePathPart} for 10007063.");
        }

        [TestMethod, TestCategory("Unit")]
        public void LARecoupmentHistory_NoLARECFundingStreams_MockFundingViewService_ThrowsExpectedResult()
        {
            // Arrange
            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_Only1619_NoPublications().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                new Mock<IFundingViewService>(MockBehavior.Strict).Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[]
                        {
                            new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                        }, "someAuthTypeName"))
                }
            };

            var laRecoupmentFundingStreamCode = "LAREC";
            var fundingStreamNamePathPart = "16-to-19-funding";

            // Act
            Func<Task> act = async () => await controller.LARecoupmentHistory("10072811", fundingStreamNamePathPart);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage($"There are no funding streams available for this local authority with ukprn: 10072811 for funding stream code: {laRecoupmentFundingStreamCode}");
        }

        [TestMethod, TestCategory("Unit")]
        public void LARecoupmentHistory_NoFundings_MockFundingViewService_ThrowsExpectedResult()
        {
            // Arrange
            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyLarec_NoPublications().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                new Mock<IFundingViewService>(MockBehavior.Strict).Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[]
                        {
                            new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                        }, "someAuthTypeName"))
                }
            };

            var laRecoupmentFundingStreamCode = "LAREC";
            var fundingStreamNamePathPart = "16-to-19-funding";
            var ukprn = "10072811";

            // Act
            Func<Task> act = async () => await controller.LARecoupmentHistory(ukprn, fundingStreamNamePathPart);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage($"There are no recoupment reports for this local authority with ukprn: {ukprn} for funding stream code: {laRecoupmentFundingStreamCode}");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task LARecoupmentHistory_OnlyLarec_MockFundingViewService_ResultAsExpected()
        {
            // Arrange
            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyLarec().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(false, false, true).Object,
                GetFundingViewService_OnlyLarec().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[]
                        {
                            new Claim("http://sfs-sfa.gov.uk/claims/principal", "something")
                        }, "someAuthTypeName"))
                }
            };

            var expectedViewModel = new LARecoupmentHistoryViewModel
            {
                ChoicePageLink = "/choose-a-statement-type",
                OrganisationName = null,
                OrganisationUkprn = "10072811",
                SecondaryContentTitle = "LA recoupment",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingStream = new Web.Areas.Admin.Models.FundingStream.FundingStream
                {
                    FundingStreamCode = "LAREC",
                    FundingStreamName = "LA recoupment",
                    FundingStreamNameWithinSentence = "LA recoupment",
                    RelevantForProviders_LoggedIn = true,
                    RelevantForOrganisations_LoggedIn = true,
                    RelevantForOrganisations_Public = true,
                    HistoryIndependentOfPublications = true,
                    Active = true,
                    Publications = new List<Publication>
                    {
                        new Publication
                        {
                            PublishedDate = new DateTime(2030, 1, 1),
                            FundingPeriodCode = "FY-2223",
                            IsLatest = true,
                            Status = PublicationStatus.Published
                        }
                    }
                }
            };
            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";

            var fundingStreamNamePathPart = "16-to-19-funding";


            // Act
            var actual = await controller.LARecoupmentHistory("10072811", fundingStreamNamePathPart);

            // Assert
            actual.Should().NotBeNull();
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LARecoupmentHistoryViewModel>().Which.FundingStream.FundingStreamCode
                .Should().BeEquivalentTo(expectedViewModel.FundingStream.FundingStreamCode);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task OrganisationSpreadsheetDownload_WithValidData_ReturnsExpectedFile()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10004801,
                ProviderName = "TEST PROVIDER",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };
            var mockFundingViewService = new Mock<IFundingViewService>();
            var settingsServiceMock = GetMockSettingsService();
            var expectedFileContent = new byte[1024];

            mockFundingViewService
                .Setup(s => s.GenerateFundingDocument(
                    It.IsAny<FundingStream>(),
                    "AY-1920",
                    It.IsAny<DateTime>(),
                    It.IsAny<Publication>(),
                    FundingViewType.Spreadsheet,
                    FundingViewScope.LoggedInOrganisationSsf,
                    new[] { FileFormat.CSV },
                    It.Is<SearchFilter[]>(f => f.Length == 1
                        && f.First().PropertyName == SearchFilterPropertyName.Ukprn
                        && f.First().PropertyValue == nameof(OrganisationSpreadsheetDownloadRequest.Ukprn)),
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

            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                settingsServiceMock.Object,
                null,
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(false, false, true).Object,
                mockFundingViewService.Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService("http://www.example.org/").Object,
                GetSystemProvider_WithSetUp().Object);

            // Act
            var actual = await controller.OrganisationSpreadsheetDownload(new OrganisationSpreadsheetDownloadRequest
            {
                FundingStreamCode = "1619",
                PublishedDate = DateTimeExtensions.ToRouteParameterString(new DateTime(2019, 12, 31)),
                Format = FundingDocumentFileType.Spreadsheet_CSVFormat,
                Ukprn = nameof(OrganisationSpreadsheetDownloadRequest.Ukprn),
                YearFrom = 2019,
                YearTo = 2020,
                YearTypeCode = YearTypeCode.AcademicYear,
                LocalAuthorityCode = "123"
            });

            // Assert
            mockFundingViewService.Verify();
            actual.Should().NotBeNull().And.BeOfType<FileContentResult>();

            var actualFileContentResult = (FileContentResult)actual;
            actualFileContentResult.ContentType.Should().Be("text/csv; charset=utf-8");
            actualFileContentResult.FileDownloadName.Should().Be(nameof(FundingDocumentMeta.Filename));
            actualFileContentResult.FileContents.Should().BeEquivalentTo(expectedFileContent);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task RecoupmentSummary_WithValidData_ResultAsExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Truro And Penwith Academy",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true,
                Roles = new List<string> { "BusinessAllocations_1416", "ViewRecoupmentReports" }
            };

            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyLarec().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(isLaSsf: true, isLa: true).Object,
                GetFundingViewService_OnlyLarec().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            var expectedViewModel = new LocalAuthorityRecoupmentSummaryViewModel
            {
                ChoicePageLink = "/choose-a-statement-type",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                OrganisationUkPrn = "10072811",
                FundingViewData = new Dictionary<string, FundingViewData>
                {
                    {
                        "LAREC-1", new FundingViewData
                        {
                            EntityName = "Hertfordshire",
                            FundingStreamCode = "LAREC",
                            TotalAmount = 2906249.75M,
                            Components = new List<Component>
                            {
                                new Component(null)
                                {
                                    Type = ComponentType.Accordion_Title
                                },
                                new Component(null)
                                {
                                    Type = ComponentType.Accordion_Panel
                                }
                            }
                        }
                    }
                },
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName
                }
            };

            expectedViewModel.HomeLink = "/";

            // Act
            var actual = await controller.LocalAuthorityRecoupmentSummary(user.Ukprn.ToString());

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityRecoupmentSummaryViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task RecoupmentDetail_WithValidData_ResultAsExpected()
        {
            // Arrange
            LocalAuthorityRecoupmentDetailRequest.FundingStreamCode = "LAREC";
            LocalAuthorityRecoupmentDetailRequest.PublishedDate = new DateTime(2030, 1, 1).ToString("d-M-yyyy"); //DateTime.MinValue.ToString("d-M-yyyy");
            LocalAuthorityRecoupmentDetailRequest.YearFrom = 2022;
            LocalAuthorityRecoupmentDetailRequest.YearTo = 2023;
            LocalAuthorityRecoupmentDetailRequest.Tab = "anomalies";
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Truro And Penwith Academy",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true,
                Roles = new List<string> { "BusinessAllocations_1416", "ViewRecoupmentReports" }
            };

            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyLarec().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(isLaSsf: true, isLa: true).Object,
                GetFundingViewService_OnlyLarec().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            var expectedViewModel = new LocalAuthorityRecoupmentDetailViewModel
            {
                OrganisationName = "Hertfordshire",
                ChoicePageLink = "/choose-a-statement-type",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                Funding = GetLarecFunding(),
                FundingStream = GetLarecFundingStream(),
                OrganisationUkPrn = "10072811",
                Document = GetLarecDocument(),
                FundingViewData = new FundingViewData
                {
                    EntityName = "Hertfordshire",
                    FundingStreamCode = "LAREC",
                    TotalAmount = 2906249.75M,
                    Components = new List<Component>
                    {
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Title
                        },
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Panel
                        }
                    }
                },
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName
                }
            };

            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";
            expectedViewModel.YearFrom = 2022;
            expectedViewModel.YearTo = 2023;
            expectedViewModel.Tab = "anomalies";
            expectedViewModel.PublishedDate = new DateTime(2030, 1, 1).ToString("d-M-yyyy");

            // Act
            var actual = await controller.LocalAuthorityRecoupmentDetail(LocalAuthorityRecoupmentDetailRequest);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LocalAuthorityRecoupmentDetailViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod, TestCategory("Unit")]
        public void RecoupmentDetail_InvalidFundingStream_ThrowsExpectedResult()
        {
            // Arrange
            LocalAuthorityRecoupmentDetailRequest.FundingStreamCode = "dummy";
            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyLarec().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(isLaSsf: true, isLa: true).Object,
                GetFundingViewService_OnlyLarec().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            // Act
            Func<Task> act = async () => { await controller.LocalAuthorityRecoupmentDetail(LocalAuthorityRecoupmentDetailRequest); };

            // Assert
            act.Should().ThrowAsync<RequestException>().WithMessage($"Funding stream not found for {LocalAuthorityRecoupmentDetailRequest.FundingStreamCode}");
        }

        [TestMethod, TestCategory("Unit")]
        public void RecoupmentDetail_NoFundingStreams_ThrowsExpectedResult()
        {
            // Arrange
            LocalAuthorityRecoupmentDetailRequest.FundingStreamCode = "dummy";
            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_NoFundingStreams().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(isLaSsf: true, isLa: true).Object,
                GetFundingViewService_OnlyLarec().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            // Act
            Func<Task> act = async () => { await controller.LocalAuthorityRecoupmentDetail(LocalAuthorityRecoupmentDetailRequest); };

            // Assert
            act.Should().ThrowAsync<RequestException>().WithMessage("Funding streams not found for logged in organisation");
        }


        [TestMethod, TestCategory("Unit")]
        public void RecoupmentDetail_InvalidPublication_ThrowsExpectedResult()
        {
            // Arrange
            LocalAuthorityRecoupmentDetailRequest.FundingStreamCode = "LAREC";
            LocalAuthorityRecoupmentDetailRequest.PublishedDate = new DateTime(2030, 1, 1).ToString("d-M-yyyy"); //DateTime.MinValue.ToString("d-M-yyyy");
            LocalAuthorityRecoupmentDetailRequest.YearFrom = 2022;
            LocalAuthorityRecoupmentDetailRequest.YearTo = 2023;
            LocalAuthorityRecoupmentDetailRequest.Tab = "anomalies";
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Truro And Penwith Academy",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true,
                Roles = new List<string> { "BusinessAllocations_1416", "ViewRecoupmentReports" }
            };

            var controller = new LocalAuthorityController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyLarec(true, false).Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(isLaSsf: true, isLa: true).Object,
                GetFundingViewService_OnlyLarec().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            // Act
            Func<Task> act = async () => { await controller.LocalAuthorityRecoupmentDetail(LocalAuthorityRecoupmentDetailRequest); };

            // Assert
            act.Should().ThrowAsync<RequestException>().WithMessage($"There are no publications for funding stream {LocalAuthorityRecoupmentDetailRequest.FundingStreamCode}");
        }

        private static IMapper GetMapper()
        {
            var config = new TypeAdapterConfig();
            config.ConfigureWebMappings();
            return new Mapper(config);
        }

        private static FundingStream Get1619FundingStream(
            bool useStaticData = true,
            bool validPublication = true)
        {
            return new FundingStream
            {
                FundingStreamCode = "1619",
                FundingStreamName = "16 to 19 funding",
                FundingStreamNameWithinSentence = "16 to 19 funding",
                RelevantForProviders_LoggedIn = true,
                RelevantForOrganisations_LoggedIn = true,
                RelevantForOrganisations_Public = true,
                HistoryIndependentOfPublications = true,
                Active = true,
                SettingValues = new List<SettingValue>
                {
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = SettingName.AcademyAndSchoolAcademicYear
                        },
                        Value = "202122",
                    },
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "UseStaticData"
                        },
                        Value = useStaticData.ToString(),
                    },
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "ParentProviderType"
                        },
                        Value = "LocalAuthority",
                    }
                },
                Publications = validPublication
                    ? new List<Publication>
                    {
                        new Publication
                        {
                            FundingPeriodCode = "AS-2122",
                            PublishedDate = new DateTime(2030, 1, 1),
                            Status = PublicationStatus.Published
                        }
                    }
                    : null,
                NextPayments = new List<NextPayment>()
            };
        }

        private static FundingDocument GetLarecDocument()
        {
            return new FundingDocument
            {
                DocumentPublishedDate = new DateTime(2030, 1, 1),
                FileExtension = "ods",
                FundingStreamCode = "LAREC",
                IsLatestDocument = false,
                YearFrom = 2022,
                YearTo = 2023,
            };
        }

        private static FundingStream GetLarecFundingStream(
           bool useStaticData = true,
           bool validPublication = true)
        {
            return new FundingStream
            {
                FundingStreamCode = "LAREC",
                FundingStreamName = "LA recoupment",
                FundingStreamNameWithinSentence = "LA recoupment",
                RelevantForProviders_LoggedIn = true,
                RelevantForOrganisations_LoggedIn = true,
                RelevantForOrganisations_Public = true,
                HistoryIndependentOfPublications = true,
                Active = true,
                SettingValues = new List<SettingValue>
                {
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = SettingName.FinancialYear
                        },
                        Value = "202223",
                    },
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "UseStaticData"
                        },
                        Value = useStaticData.ToString(),
                    },
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "ParentProviderType"
                        },
                        Value = "LocalAuthority",
                    }
                },
                Publications = validPublication
                    ? new List<Publication>
                    {
                        new Publication
                        {
                            FundingPeriodCode = "FY-2223",
                            PublishedDate = new DateTime(2030, 1, 1),
                            Status = PublicationStatus.Published,
                            IsLatest = true
                        }
                    }
                    : null,
                NextPayments = new List<NextPayment>()
            };
        }

        private static FundingStream GetLarecFundingStream_NoPublications(
        bool useStaticData = true,
        bool validPublication = true)
        {
            return new FundingStream
            {
                FundingStreamCode = "LAREC",
                FundingStreamName = "LA recoupment",
                FundingStreamNameWithinSentence = "LA recoupment",
                RelevantForProviders_LoggedIn = true,
                RelevantForOrganisations_LoggedIn = true,
                RelevantForOrganisations_Public = true,
                HistoryIndependentOfPublications = true,
                Active = true,
                SettingValues = new List<SettingValue>
                {
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = SettingName.FinancialYear
                        },
                        Value = "202223",
                    },
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "UseStaticData"
                        },
                        Value = useStaticData.ToString(),
                    },
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "ParentProviderType"
                        },
                        Value = "LocalAuthority",
                    }
                },
                Publications = null,
                NextPayments = new List<NextPayment>()
            };
        }

        private Mock<IClaimsBasedIdentityService> GetIdentityService()
        {
            var identityService = new Mock<IClaimsBasedIdentityService>(MockBehavior.Strict);
            identityService
                .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User
                {
                    Ukprn = 10072811,
                    ProviderName = "TEST PROVIDER",
                    Email = "test@test.com",
                    IsAuthenticated = true,
                    IsExternalUser = true
                });

            return identityService;
        }

        private Mock<IOptions<ApplicationConfiguration>> GetAppConfigService(string homeLink = "/")
        {
            var appConfig = new ApplicationConfiguration();
            appConfig.LoggedInProviderHomeLink = homeLink;

            var appConfigService = new Mock<IOptions<ApplicationConfiguration>>(MockBehavior.Strict);
            appConfigService
                .Setup(s => s.Value)
                .Returns(appConfig);

            return appConfigService;
        }

        private Mock<IFundingApiService> GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(bool isMatValid = false, bool isLaSsf = false, bool isLa = false)
        {
            var fundingApiService = new Mock<IFundingApiService>(MockBehavior.Strict);

            fundingApiService
                .Setup(fas => fas.SearchFunding(It.IsAny<FundingApiSearchRequestObject>()))
                .ReturnsAsync(GetFundingApiSearchResponse(isMatValid, isLaSsf, isLa));

            fundingApiService
                .Setup(fas => fas.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false))
                .ReturnsAsync(new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = new List<FundingApiSearchProviderFunding>
                    {
                        new FundingApiSearchProviderFunding
                        {
                            OrganisationName = "A good school",
                            FundingStreamCode = "GAG",
                            ParentProviderType = "AcademyTrust",
                            FundingPeriodCode = "AY-1920",
                            GroupingReason = GroupingReason.Information,
                            StatusChangedDate = new DateTime(2019, 10, 31)
                        }
                    }
                });

            fundingApiService.Setup(fas => fas.HasUserVisitedFunding(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            return fundingApiService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_Only1619(
            bool useStaticData = true,
            bool validPublication = true)
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>
                {
                    Get1619FundingStream(useStaticData, validPublication)
                });

            return userJourneyService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_OnlyLarec(
            bool useStaticData = true,
            bool validPublication = true)
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>
                {
                    GetLarecFundingStream(useStaticData, validPublication)
                });

            return userJourneyService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_OnlyLarec_NoPublications(
        bool useStaticData = true,
        bool validPublication = true)
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>
                {
                    GetLarecFundingStream_NoPublications(useStaticData, validPublication)
                });

            return userJourneyService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_NoFundingStreams(
            bool useStaticData = true,
            bool validPublication = true)
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>());

            return userJourneyService;
        }

        private FundingApiSearchResponse GetFundingApiSearchResponse(bool valid = false, bool isLaSsf = false, bool isLa = false)
        {
            if (isLaSsf || isLa)
            {
                var result = new List<IFundingApiSearchFunding>();
                if (isLaSsf)
                {
                    result.Add(new FundingApiSearchFunding
                    {
                        GroupingType = "LocalAuthoritySsf"
                    });
                }

                if (isLa)
                {
                    result.Add(new FundingApiSearchFunding
                    {
                        GroupingType = "LocalAuthority"
                    });
                }

                return new FundingApiSearchResponse
                {
                    Funding = result
                };
            }

            if (!valid)
            {
                return null;
            }

            return new FundingApiSearchResponse
            {
                Funding = new List<IFundingApiSearchFunding>
                {
                    new FundingApiSearchFunding
                    {
                        GroupingType = "AcademyTrust",
                        ProviderFundings = new List<string>
                        {
                            "ukprn1", "ukprn2", "10072811"
                        }
                    }
                }
            };
        }

        private Mock<IComponentService> GetComponentService_Uncalled()
        {
            var componentService = new Mock<IComponentService>(MockBehavior.Strict);
            return componentService;
        }

        private Mock<ISystemProvider> GetSystemProvider_WithSetUp()
        {
            var systemProvider = GetSystemProvider();

            systemProvider.Setup(sp => sp.DateTime.Now()).Returns(DateTime.Now);

            return systemProvider;
        }

        private Mock<IFundingViewService> GetFundingViewService_Only1619()
        {
            var fundingViewService = new Mock<IFundingViewService>(MockBehavior.Strict);
            fundingViewService
                .Setup(fvs => fvs.GenerateFundingViewData(
                    It.IsAny<IComponentService>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<FundingStream[]>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<Publication>(),
                    It.IsAny<int?>(),
                    It.IsAny<FundingViewScope>(),
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
                .ReturnsAsync(new FundingViewData
                {
                    EntityName = "Hertfordshire",
                    FundingStreamCode = "1619",
                    TotalAmount = 2906249.75M,
                    Components = new List<Component>
                    {
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Title
                        },
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Panel
                        }
                    }
                });

            fundingViewService
                .Setup(fvs => fvs.GetDataRequirements(
                    It.IsAny<FundingStream>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<FundingViewScope>(),
                    It.IsAny<SearchFilter[]>(),
                    It.IsAny<IFundingApiSearchFunding>(),
                    It.IsAny<IFundingApiSearchProviderFunding>(),
                    It.IsAny<string>()))
                .Returns(new List<FundingApiSearchRequestObject>
                {
                    new FundingApiSearchRequestObject
                    {
                        Type = "Funding",
                        FundingStreams = new[] { new FundingApiSearchFundingStream() }, SearchTerm = string.Empty
                    }
                });

            return fundingViewService;
        }

        private Mock<IFundingViewService> GetFundingViewService_OnlyLarec()
        {
            var fundingViewService = new Mock<IFundingViewService>(MockBehavior.Strict);
            fundingViewService
                .Setup(fvs => fvs.GenerateFundingViewData(
                    It.IsAny<IComponentService>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<FundingStream[]>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<Publication>(),
                    It.IsAny<int?>(),
                    It.IsAny<FundingViewScope>(),
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
                .ReturnsAsync(new FundingViewData
                {
                    EntityName = "Hertfordshire",
                    FundingStreamCode = "LAREC",
                    TotalAmount = 2906249.75M,
                    Components = new List<Component>
                    {
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Title
                        },
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Panel
                        }
                    }
                });

            fundingViewService
                .Setup(fvs => fvs.GetDataRequirements(
                    It.IsAny<FundingStream>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<FundingViewScope>(),
                    It.IsAny<SearchFilter[]>(),
                    It.IsAny<IFundingApiSearchFunding>(),
                    It.IsAny<IFundingApiSearchProviderFunding>(),
                    It.IsAny<string>()))
                .Returns(new List<FundingApiSearchRequestObject>
                {
                    new FundingApiSearchRequestObject
                    {
                        Type = "Funding",
                        FundingStreams = new[] { new FundingApiSearchFundingStream() }, SearchTerm = string.Empty
                    }
                });

            return fundingViewService;
        }

        private Mock<IGlobalSettingService> GetGlobalSettingService()
        {
            return CommonMocks.GlobalSettingService();
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
                    PublishedDate = new DateTime(2019, 12, 31),
                    Description = "TestPublicationDescription",
                    FundingPeriodCode = fundingPeriodCode,
                    Status = PublicationStatus.Published,
                    IsLatest = true
                }
            };

        private IFundingApiSearchFunding GetFunding()
        {
            return Local1619FakeApiService.GetFunding("ForContract").GetAwaiter().GetResult();
        }

        private IFundingApiSearchFunding GetLarecFunding()
        {
            return LocalLAREC_FakeApiService.GetFunding("LAREC_Static_LA_V2").GetAwaiter().GetResult();
        }

        private Mock<IUserJourneyService> GetUserJourneyService_NoFundingStreams()
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>());

            return userJourneyService;
        }

        private Mock<ISystemProvider> GetSystemProvider()
        {
            var systemProvider = new Mock<ISystemProvider>(MockBehavior.Strict);

            return systemProvider;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_Only1619_NoPublications()
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            var fundingStream = Get1619FundingStream();
            fundingStream.Publications = new List<Publication>();

            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>
                {
                    fundingStream
                });

            return userJourneyService;
        }

        private Mock<IBasePathService> GetBasePathService()
        {
            var basePathService = new Mock<IBasePathService>(MockBehavior.Strict);
            basePathService
                .Setup(s => s.GetApplicationBasePath())
                .Returns("Base Path");
            basePathService
                .Setup(s => s.GetUrlForLoggedInProviderPath())
                .Returns("Url For Logged In Provider Path");

            return basePathService;
        }

        private Mock<IFundingApiService> GetFundingApiService_WithUserVisitNotOccuredDetails()
        {
            var fundingApiService = new Mock<IFundingApiService>(MockBehavior.Strict);

            fundingApiService.Setup(fas => fas.HasUserVisitedFunding(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            fundingApiService.Setup(fas => fas.AddUserFundingViewDetail(It.IsAny<AddUserFundingViewRequest>())).Returns(Task.CompletedTask);

            return fundingApiService;
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
                     FundingStreamCode = "1619",
                     FundingStreamName = "16 to 19 funding",
                     FundingStreamNameWithinSentence = "1619",
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
                                 SettingName = "OrganisationDownloadSizeInBytes"
                             }
                         }
                     },
                     Publications = ExpectedPublications("AY-1920"),
                     NextPayments = new List<NextPayment>(),
                     FundingStreamCodePubliclyKnown = false,
                     RelevantForNational = true,
                     RelevantForOrganisations_LoggedIn = true,
                     RelevantForOrganisations_Public = true,
                     RelevantForProviders_LoggedIn = true,
                     RelevantForProviders_Public = true
                   }
               });

            return mockSettingsService;
        }
    }
}