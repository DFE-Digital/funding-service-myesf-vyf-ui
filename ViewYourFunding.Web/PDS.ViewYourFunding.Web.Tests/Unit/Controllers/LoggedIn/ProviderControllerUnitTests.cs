using AutoMapper;
using FluentAssertions;
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
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Controllers;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.Requests;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Exceptions;
using PDS.ViewYourFunding.Web.Models.Request;
using PDS.ViewYourFunding.Web.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using User = Pds.Core.Common.Identity.Models.User;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers.LoggedIn
{
    [TestClass, TestCategory("Unit")]
    public class ProviderControllerUnitTests
    {
        private readonly string _contactUsLink;
        private readonly string _feedbackLink;

        /// <summary>
        /// Test provider name to be used for logged in providers.
        /// </summary>
        private const string ProviderNameFromProviderFunding = "Provider name obtained from provider funding organisation name";

        /// <summary>
        /// Test provider name to be obtained for MAT/LA etc.
        /// </summary>
        private const string ProviderNameFromFunding = "Provider name obtained from funding group name";

        public ProviderControllerUnitTests()
        {
            var appConfig = new ApplicationConfiguration();
            _contactUsLink = appConfig.ContactUsLink;
            _feedbackLink = appConfig.FeedbackLinkForLoggedInView;
        }

        [TestMethod]
        public async Task ProviderStatement_NoFundingStreams_MockFundingViewService_ResultExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = null,
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var controller = new ProviderController(
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

            var expectedViewModel = new ProviderStatementViewModel
            {
                ChoicePageLink = "/choose-a-statement-type",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingViewData = new Dictionary<string, FundingViewData>(),
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName
                },
                DisplayNoAllocationMessage = true,
            };
            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";

            // Act
            var actual = await controller.ProviderStatement();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod]
        public async Task ProviderStatement_OnlyGAG_MockFundingViewService_ResultExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Abbey View Primary Academy",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyGAG().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                GetFundingViewService_OnlyGAG().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);
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

            var expectedViewModel = new ProviderStatementViewModel
            {
                ProviderUrn = "--",
                OrganisationName = "Abbey View Primary Academy",
                ChoicePageLink = "/choose-a-statement-type",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                ProviderFundingViewData = new Dictionary<string, FundingViewData>
                {
                    {
                        "GAG",
                        new FundingViewData
                        {
                            FundingStreamCode = "GAG",
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
                },
                DisplayNoAllocationMessage = false
            };
            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";

            // Act
            var actual = await controller.ProviderStatement();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod]
        public async Task ProviderStatement_OnlyGAG_RealFundingViewService_ResultExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Abbey View Primary Academy",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var modelFileStoreService = new Mock<IModelFileStoreService>(MockBehavior.Strict);
            modelFileStoreService
                .Setup(mfss => mfss.GetModelFilenames(It.IsAny<string>()))
                .Returns(new[]
                {
                    "GAG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProviderSummary.json"
                });

            modelFileStoreService
                .Setup(mfss => mfss.Exists(It.IsAny<string>()))
                .Returns(true);

            modelFileStoreService
                .Setup(mfss => mfss.ReadFileAsString(It.IsAny<string>()))
                .Returns(@"{""dataset"": [{""datasetName"":""providerFunding"",""expression"": ""ParentProviderType=AcademyTrust&GroupingReason=Information""}], ""groups"": [{""type"":""AccordionTitle"",},{""type"":""AccordionPanel""}]}");

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

            var controller = new ProviderController(
                GetComponentService_Basic().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyGAG().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                realFundingViewService,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);

            var expectedViewModel = new ProviderStatementViewModel
            {
                ProviderUrn = "--",
                OrganisationName = "Abbey View Primary Academy",
                ChoicePageLink = "/choose-a-statement-type",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                ProviderFundingViewData = new Dictionary<string, FundingViewData>
                {
                    {
                        "GAG",
                        new FundingViewData
                        {
                            EntityName = "Abbey View Primary Academy",
                            EntityPrimaryIdentifier = "10072811",
                            EntityAlternativeIdentifier = "8252042",
                            EntityType = "Academies",
                            EntitySubType = "Free schools",
                            FundingStreamCode = "GAG",
                            TotalAmount = 0,
                            InYearOpener = false,
                            IsIndicativeFunding = false,
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
                            },
                            PublicationDate = new DateTime(2030, 1, 1),
                            FundingPeriodCode = "AC-2122"
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
                },
                DisplayNoAllocationMessage = false
            };
            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";

            // Act
            var actual = await controller.ProviderStatement();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod]
        public async Task ProviderStatement_OnlyPSG_MockFundingViewService_ResultExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "A school",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyPSG().Object,
                GetMapper(),
                GetFundingApiService_SinglePSGProvider().Object,
                GetFundingViewService_OnlyPSG().Object,
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
                            new[] { new Claim("http://sfs-sfa.gov.uk/claims/principal", "something") },
                            "someAuthTypeName"))
                }
            };

            var expectedViewModel = new ProviderStatementViewModel
            {
                OrganisationName = "A School",
                ChoicePageLink = "/choose-a-statement-type",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                ProviderFundingViewData = new Dictionary<string, FundingViewData>
                {
                    {
                        "PSG",
                        new FundingViewData
                        {
                            FundingStreamCode = "PSG",
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
                    ProviderName = null,
                    Ukprn = user.Ukprn,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName
                }
            };
            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";

            // Act
            var actual = await controller.ProviderStatement();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod]
        public async Task ProviderStatement_GAGAndPSG_MockFundingViewService_ResultExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Abbey View Primary Academy",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_PSGAndGAG().Object,
                GetMapper(),
                GetFundingApiService_SinglePSGProvider().Object,
                GetFundingViewService_PSGAndGAG().Object,
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

            var expectedViewModel = new ProviderStatementViewModel
            {
                ProviderUrn = "--",
                OrganisationName = "Abbey View Primary Academy",
                ChoicePageLink = "/choose-a-statement-type",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                ProviderFundingViewData = new Dictionary<string, FundingViewData>
                {
                    {
                        "GAG",
                        new FundingViewData
                        {
                            FundingStreamCode = "GAG",
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
                    },
                    {
                        "PSG",
                        new FundingViewData
                        {
                            FundingStreamCode = "PSG",
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
                },
                DisplayNoAllocationMessage = false
            };
            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";

            // Act
            var actual = await controller.ProviderStatement();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod]
        public async Task ProviderStatement_LocalAuthoritySsf_MockFundingViewService_ResultExpected()
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

            var controller = new ProviderController(
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

            var expectedViewModel = new ProviderStatementViewModel
            {
                OrganisationName = "Hertfordshire",
                ChoicePageLink = "/choose-a-statement-type",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingViewData = new Dictionary<string, FundingViewData>
                {
                    {
                        "1619",
                        new FundingViewData
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
                        }
                    }
                },
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = null,
                    Ukprn = user.Ukprn,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName
                },
                DisplayNoAllocationMessage = false
            };
            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";

            // Act
            var actual = await controller.ProviderStatement();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod]
        public async Task ProviderStatement_LocalAuthority_MockFundingViewService_ResultExpected()
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

            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_Only1619().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(isLa: true).Object,
                GetFundingViewService_Only1619().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            var expectedViewModel = new ProviderStatementViewModel
            {
                OrganisationName = "Hertfordshire",
                ChoicePageLink = "/choose-a-statement-type",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingViewData = new Dictionary<string, FundingViewData>
                {
                    {
                        "1619",
                        new FundingViewData
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
                        }
                    }
                },
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = null,
                    Ukprn = user.Ukprn,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName
                },
                DisplayNoAllocationMessage = false
            };
            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";

            // Act
            var actual = await controller.ProviderStatement();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod]
        public async Task ProviderStatement_GAGAndPSGViaChoicePage_MockFundingViewService_ResultExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Abbey View Primary Academy",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_PSGAndGAG().Object,
                GetMapper(),
                GetFundingApiService_SinglePSGProvider().Object,
                GetFundingViewService_PSGAndGAG().Object,
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

            var expectedViewModel = new ProviderStatementViewModel
            {
                ProviderUrn = "--",
                OrganisationName = "Abbey View Primary Academy",
                ViaChoicePage = true,
                ChoicePageLink = "/choose-a-statement-type",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                ProviderFundingViewData = new Dictionary<string, FundingViewData>
                {
                    {
                        "GAG",
                        new FundingViewData
                        {
                            FundingStreamCode = "GAG",
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
                    },
                    {
                        "PSG",
                        new FundingViewData
                        {
                            FundingStreamCode = "PSG",
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
                },
                DisplayNoAllocationMessage = false
            };
            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";

            // Act
            var actual = await controller.ProviderStatement(true);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod]
        public async Task ProviderStatement_GAGAndPSGViaChoicePageAbsoluteLink_MockFundingViewService_ResultExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Abbey View Primary Academy",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_PSGAndGAG().Object,
                GetMapper(),
                GetFundingApiService_SinglePSGProvider().Object,
                GetFundingViewService_PSGAndGAG().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService("http://www.example.org/").Object,
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

            var expectedViewModel = new ProviderStatementViewModel
            {
                ProviderUrn = "--",
                OrganisationName = "Abbey View Primary Academy",
                ViaChoicePage = true,
                ChoicePageLink = "http://www.example.org/choose-a-statement-type",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                ProviderFundingViewData = new Dictionary<string, FundingViewData>
                {
                    {
                        "GAG",
                        new FundingViewData
                        {
                            FundingStreamCode = "GAG",
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
                    },
                    {
                        "PSG",
                        new FundingViewData
                        {
                            FundingStreamCode = "PSG",
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
                },
                DisplayNoAllocationMessage = false
            };
            expectedViewModel.HomeLink = "http://www.example.org/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "http://www.example.org/";

            // Act
            var actual = await controller.ProviderStatement(true);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod]
        public async Task VarianceSelection_OnlyCurrentYear_ResultExpected()
        {
            // Arrange
            var fundingViewService = new ModelFundingViewService(
                GetGlobalSettingService().Object,
                GetModelFileStoreService_GaG().Object,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                null,
                null,
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                null,
                null,
                new MemoryCacheService(null, 0));

            var controller = new ProviderController(
                GetComponentService_Basic().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyGAG().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                fundingViewService,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            var expectedOptions = new List<VarianceSelectionOption> { VarianceSelectionOption.PreviousStatementCurrentYear, VarianceSelectionOption.NoComparison };

            // Act
            var actual = await controller.VarianceSelection(new ProviderFundingBreakdownRequest
            {
                Ukprn = "10087061",
                FundingStreamNamePathPart = "general-annual-grant",
                PublishedDate = "1-8-2021",
                YearFrom = 2021,
                YearTo = 2022
            });

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<VarianceSelectionViewModel>()
                .Which.Options.Should().BeEquivalentTo(expectedOptions);
        }

        [TestMethod]
        public async Task VarianceSelection_WithPreviousYear_ResultExpected()
        {
            // Arrange
            var fundingViewService = new ModelFundingViewService(
                GetGlobalSettingService().Object,
                GetModelFileStoreService_GaG().Object,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                null,
                null,
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                null,
                null,
                new MemoryCacheService(null, 0));

            var controller = new ProviderController(
                GetComponentService_Basic().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyGAG(true).Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                fundingViewService,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            var expectedOptions = new List<VarianceSelectionOption> { VarianceSelectionOption.FinalStatementPreviousYear, VarianceSelectionOption.PreviousStatementCurrentYear, VarianceSelectionOption.NoComparison };

            // Act
            var actual = await controller.VarianceSelection(new ProviderFundingBreakdownRequest
            {
                Ukprn = "10034857",
                FundingStreamNamePathPart = "general-annual-grant",
                PublishedDate = "1-8-2021",
                YearFrom = 2021,
                YearTo = 2022
            });

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<VarianceSelectionViewModel>()
                .Which.Options.Should().BeEquivalentTo(expectedOptions);
        }

        [TestMethod]
        public async Task VarianceSelection_NoOptions_ResultExpected()
        {
            // Arrange
            var fundingViewService = new ModelFundingViewService(
                GetGlobalSettingService().Object,
                GetModelFileStoreService_GaG().Object,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                null,
                null,
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                null,
                null,
                new MemoryCacheService(null, 0));

            var controller = new ProviderController(
                GetComponentService_Basic().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyGAG().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                fundingViewService,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            // Act
            var actual = await controller.VarianceSelection(new ProviderFundingBreakdownRequest
            {
                Ukprn = "10038354",
                FundingStreamNamePathPart = "general-annual-grant",
                PublishedDate = "1-6-2021",
                YearFrom = 2021,
                YearTo = 2022
            });

            // Assert
            actual
                 .Should().BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should().Be("ProviderFundingBreakDown");
        }

        [TestMethod]
        public void VarianceSelection_WhenNoMatchingFundingStream_ThrowsException()
        {
            // Arrange
            var fundingViewService = new ModelFundingViewService(
                GetGlobalSettingService().Object,
                GetModelFileStoreService_GaG().Object,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                null,
                null,
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                null,
                null,
                new MemoryCacheService(null, 0));

            var controller = new ProviderController(
                GetComponentService_Basic().Object,
                GetIdentityService().Object,
                GetUserJourneyService_NoFundingStreams().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                fundingViewService,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            var providerFundingBreakdownRequest = new ProviderFundingBreakdownRequest
            {
                Ukprn = "10038354",
                FundingStreamNamePathPart = "general-annual-grant",
                PublishedDate = "1-6-2021",
                YearFrom = 2021,
                YearTo = 2022
            };

            // Act
            Func<Task> act = async () => await controller.VarianceSelection(providerFundingBreakdownRequest);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage($"Funding stream not found for {providerFundingBreakdownRequest.FundingStreamNamePathPart}");
        }

        [TestMethod]
        public void ProviderFundingBreakDown_NoFundingStreamCode_ResultExpected()
        {
            // Arrange
            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_NoFundingStreams().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                GetFundingViewService_OnlyGAG().Object,
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

            var providerFundingBreakdownRequest = new ProviderFundingBreakdownRequest
            {
                Ukprn = "10072811",
                FundingStreamNamePathPart = nameof(ProviderFundingBreakdownRequest.FundingStreamNamePathPart)
            };

            // Act
            Func<Task> act = async () => await controller.ProviderFundingBreakDown(providerFundingBreakdownRequest);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage($"Funding stream not found for {providerFundingBreakdownRequest.FundingStreamNamePathPart}");
        }

        // TODO - This test should be uncommented when the temporary fix to grant MAT GAG access unconditionally is removed (Method: ProviderController.IsPartOfMAT).
        // [TestMethod]
        public void ProviderFundingBreakDown_GAG_MatUserAccess_OrgNotPartOfMat_ResultExpected()
        {
            // Arrange
            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService_MatUser().Object,
                GetUserJourneyService_OnlyGAG().Object,
                GetMapper(),
                GetFundingApiService_NoResults().Object,
                GetFundingViewService_OnlyGAG().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);
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

            var providerFundingBreakdownRequest = new ProviderFundingBreakdownRequest
            {
                Ukprn = "12345678",
                FundingStreamNamePathPart = "general-annual-grant"
            };

            // Act
            Func<Task> act = async () => await controller.ProviderFundingBreakDown(providerFundingBreakdownRequest);

            // Assert
            act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("You are not authorised to perform this action.");
        }

        [TestMethod]
        public void ProviderFundingBreakDown_PSG_MatUserAccess_OrgNotPartOfMat_ResultExpected()
        {
            // Arrange
            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService_MatUser().Object,
                GetUserJourneyService_OnlyPSG().Object,
                GetMapper(),
                GetFundingApiService_NoResults().Object,
                GetFundingViewService_OnlyPSG().Object,
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

            var providerFundingBreakdownRequest = new ProviderFundingBreakdownRequest
            {
                Ukprn = "12345678",
                FundingStreamNamePathPart = "pe-and-sport-premium"
            };

            // Act
            Func<Task> act = async () => await controller.ProviderFundingBreakDown(providerFundingBreakdownRequest);

            // Assert
            act.Should().NotThrowAsync();
        }

        [TestMethod]
        public void ProviderFundingBreakDown_NoProviderFunding_ResultExpected()
        {
            // Arrange
            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyPSG().Object,
                GetMapper(),
                GetFundingApiService_NoResults().Object,
                GetFundingViewService_OnlyGAG().Object,
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

            var providerFundingBreakdownRequest = new ProviderFundingBreakdownRequest
            {
                Ukprn = "10072811",
                FundingStreamNamePathPart = "pe-and-sport-premium",
                PublishedDate = "1-1-2020",
                YearFrom = 2019,
                YearTo = 2020
            };

            // Act
            Func<Task> act = async () => await controller.ProviderFundingBreakDown(providerFundingBreakdownRequest);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage($"Provider funding not found for {providerFundingBreakdownRequest.FundingStreamNamePathPart}");
        }

        [TestMethod]
        public void ProviderFundingBreakDown_NoPublications_ResultExpected()
        {
            // Arrange
            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyPSG_NoPublications().Object,
                GetMapper(),
                GetFundingApiService_SinglePSGProvider().Object,
                GetFundingViewService_OnlyGAG().Object,
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

            var providerFundingBreakdownRequest = new ProviderFundingBreakdownRequest
            {
                Ukprn = "10072811",
                FundingStreamNamePathPart = "pe-and-sport-premium"
            };

            // Act
            Func<Task> act = async () => await controller.ProviderFundingBreakDown(providerFundingBreakdownRequest);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage($"Publications not found for {GetPsgFundingStream().FundingStreamName}");
        }

        [TestMethod]
        public async Task ProviderFundingBreakDown_RealFundingViewService_ResultExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Abbey View Primary Academy",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var fundingViewService = new ModelFundingViewService(
                GetGlobalSettingService().Object,
                GetModelFileStoreService_GaG().Object,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                null,
                null,
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                null,
                null,
                new MemoryCacheService(null, 0));

            var controller = new ProviderController(
                GetComponentService_Basic().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyGAG().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                fundingViewService,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            var expectedViewModel = new ProviderFundingBreakdownViewModel
            {
                ProviderUrn = "--",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                ChoicePageLink = "/choose-a-statement-type",
                FundingStream = GetGagFundingStream(),
                OrganisationName = "Abbey View Primary Academy",
                OrganisationUkPrn = "10072811",
                FundingViewData = new FundingViewData
                {
                    EntityName = "Abbey View Primary Academy",
                    EntityPrimaryIdentifier = "10072811",
                    EntityAlternativeIdentifier = "8252042",
                    EntitySubType = "Free schools",
                    EntityType = "Academies",
                    PublicationDate = DateTime.Today,
                    FundingStreamCode = "GAG",
                    TotalAmount = 0,
                    InYearOpener = false,
                    IsIndicativeFunding = false,
                    Components = new List<Component>
                    {
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Title,
                            PageData = new Dictionary<string, object>
                            {
                                { "FileSizeBytes", "201KB" },
                                { "FileExtension", "CSV" }
                            }
                        },
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Panel
                        }
                    },
                    FundingPeriodCode = "AC-2122"
                },
                PublishedDate = "1-6-2021",
                FundingStreamNamePathPart = "general-annual-grant",
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName
                },
                YearFrom = 2021,
                YearTo = 2022
            };
            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";

            // Act
            var actual = await controller.ProviderFundingBreakDown(new ProviderFundingBreakdownRequest
            {
                Ukprn = "10072811",
                FundingStreamNamePathPart = "general-annual-grant",
                PublishedDate = "1-6-2021",
                YearFrom = 2021,
                YearTo = 2022
            });

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderFundingBreakdownViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod]
        public async Task ProviderFundingBreakDown_FundingViewService_ResultExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10087061,
                ProviderName = "Test Provider",
                Email = "Abbey View Primary Academy",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var fundingViewService = new ModelFundingViewService(
                GetGlobalSettingService().Object,
                GetIndicativeModelFileStoreService_GaG().Object,
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                null,
                null,
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                null,
                null,
                new MemoryCacheService(null, 0));

            var controller = new ProviderController(
                GetComponentService_Basic().Object,
                GetIdentityServiceSetupForIndicativeStatementUser().Object,
                GetUserJourneyService_OnlyGAG().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                fundingViewService,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);

            var expectedViewModel = new ProviderFundingBreakdownViewModel
            {
                ProviderUrn = "--",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                ChoicePageLink = "/choose-a-statement-type",
                FundingStream = GetGagFundingStream(),
                OrganisationName = "Alanbrooke School",
                OrganisationUkPrn = "10087061",
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = user.IsExternalUser,
                    IsLoggedIn = user.IsAuthenticated,
                    ProviderName = user.ProviderName,
                    Ukprn = user.Ukprn,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName
                },
                YearFrom = 2021,
                YearTo = 2022
            };
            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";

            // Act
            var actual = await controller.ProviderFundingBreakDown(new ProviderFundingBreakdownRequest
            {
                Ukprn = "10087061",
                FundingStreamNamePathPart = "general-annual-grant",
                PublishedDate = "1-6-2021",
                YearFrom = 2021,
                YearTo = 2022
            });

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderFundingBreakdownViewModel>()
                .Which.OrganisationName.Should().BeEquivalentTo(expectedViewModel.OrganisationName);
        }

        [TestMethod]
        public void ProviderHistory_NoFundingStreams_MockFundingViewService_ResultExpected()
        {
            // Arrange
            var controller = new ProviderController(
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

            var fundingStreamNamePathPart = "general-annual-grant";

            // Act
            Func<Task> act = async () => await controller.ProviderHistory("10072811", fundingStreamNamePathPart);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage($"There is no funding stream code for funding stream {fundingStreamNamePathPart}.");
        }

        [TestMethod]
        public void ProviderHistory_NoPublications_MockFundingViewService_ResultExpected()
        {
            // Arrange
            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyGAG_NoPublications().Object,
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

            var fundingStreamCode = "GAG";
            var fundingStreamNamePathPart = "general-annual-grant";

            // Act
            Func<Task> act = async () => await controller.ProviderHistory("10072811", fundingStreamNamePathPart);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage($"There are no publications for funding stream {fundingStreamCode}");
        }

        // Note: This test is commented out as it will not work until the following line of code in the BaseFundingController is removed as it is bypassing the Mock fundingApiService and not throwing the expected exception
        // var apiService = fundingStreamCode == "GAG" ? new LocalGAG_FakeApiService() : _fundingApiService; // TODO - eventually remove this line
        // [TestMethod]
        public void ProviderHistory_NoProviderFunding_MockFundingViewService_ResultExpected()
        {
            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyGAGNoPayments().Object,
                GetMapper(),
                GetFundingApiService_NoResults().Object,
                GetFundingViewService_OnlyGAGProviderHistory().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);

            var fundingStreamCode = "GAG";
            var fundingStreamNamePathPart = "general-annual-grant";

            // Act
            Func<Task> act = async () => await controller.ProviderHistory("12345678", fundingStreamNamePathPart);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage($"There are no publications for funding stream {fundingStreamCode}");
        }

        [TestMethod]
        public async Task ProviderHistory_OnlyGAG_MockFundingViewService_ResultExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Abbey View Primary Academy",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyGAG().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                GetFundingViewService_OnlyGAG().Object,
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

            var expectedViewModel = new ProviderHistoryViewModel
            {
                ProviderUrn = "--",
                ChoicePageLink = "/choose-a-statement-type",
                OrganisationName = "Abbey View Primary Academy",
                OrganisationUkprn = "10072811",
                SecondaryContentTitle = "General annual grant",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingPeriodPublications = new List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>>(),
                FundingPeriodProviderFundings = new List<KeyValuePair<(int yearFrom, int yearTo), List<ProviderFundingViewModel>>>
                {
                    new KeyValuePair<(int, int), List<ProviderFundingViewModel>>(
                        (2021, 2022), new List<ProviderFundingViewModel>
                        {
                            new ProviderFundingViewModel
                            {
                                StatusChangedDate = new DateTime(2021, 6, 1),
                                FundingPeriodCode = "AC-2122",
                                IsLatest = true,
                                IsFinal = false,
                                VariationReason = "Initial allocation.",
                                GroupingReason = "Information"
                            }
                        }),
                    new KeyValuePair<(int, int), List<ProviderFundingViewModel>>(
                        (2023, 2024), new List<ProviderFundingViewModel>
                        {
                            new ProviderFundingViewModel
                            {
                                StatusChangedDate = new DateTime(2023, 6, 1),
                                FundingPeriodCode = "AC-2324",
                                IsLatest = false,
                                IsFinal = true,
                                VariationReason = "Initial allocation.",
                                GroupingReason = "Information"
                            }
                        })
                },
                FundingViewData = new FundingViewData
                {
                    FundingStreamCode = "GAG",
                    TotalAmount = 2906249.75M,
                    Components = new List<Component>
                    {
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Panel
                        },
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
                                    "FundingPeriodProviderFundings",
                                    new List<KeyValuePair<(int yearFrom, int yearTo), List<ProviderFundingViewModel>>>
                                    {
                                        new KeyValuePair<(int, int), List<ProviderFundingViewModel>>(
                                            (2021, 2022), new List<ProviderFundingViewModel>
                                            {
                                                new ProviderFundingViewModel
                                                {
                                                    StatusChangedDate = new DateTime(2021, 6, 1),
                                                    FundingPeriodCode = "AC-2122",
                                                    IsLatest = true,
                                                    IsFinal = false,
                                                    VariationReason = "Initial allocation.",
                                                    GroupingReason = "Information"
                                                }
                                            }),
                                        new KeyValuePair<(int, int), List<ProviderFundingViewModel>>(
                                            (2023, 2024), new List<ProviderFundingViewModel>
                                            {
                                                new ProviderFundingViewModel
                                                {
                                                    StatusChangedDate = new DateTime(2023, 6, 1),
                                                    FundingPeriodCode = "AC-2324",
                                                    IsLatest = false,
                                                    IsFinal = true,
                                                    VariationReason = "Initial allocation.",
                                                    GroupingReason = "Information"
                                                }
                                            })
                                    }
                                }
                            },
                        },
                    }
                },
                FundingStream = new Web.Areas.Admin.Models.FundingStream.FundingStream
                {
                    FundingStreamCode = "GAG",
                    FundingStreamName = "General annual grant",
                    FundingStreamNameWithinSentence = "General annual grant",
                    RelevantForProviders_LoggedIn = true,
                    RelevantForOrganisations_Public = true,
                    HistoryIndependentOfPublications = true,
                    Active = true,
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "AcademyAcademicYear"
                            },
                            Value = "202122"
                        },
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "UseStaticData"
                            },
                            Value = "true",
                        },
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "ParentProviderType"
                            },
                            Value = "AcademyTrust",
                        },
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "ProviderDownloadSizeInBytes"
                            },
                            Value = "206000",
                        },
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "FundingDocumentFileType"
                            },
                            Value = "CSV",
                        }
                    },
                    NextPayments = new List<NextPayment>(),
                    Publications = new List<Publication>
                    {
                        new Publication
                        {
                            PublishedDate = new DateTime(2030, 1, 1),
                            FundingPeriodCode = "AC-2122",
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

            var fundingStreamNamePathPart = "general-annual-grant";

            // Act
            var actual = await controller.ProviderHistory("10072811", fundingStreamNamePathPart);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderHistoryViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod]
        public async Task ProviderHistory_OnlyGAG_RealFundingViewService_ResultExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Abbey View Primary Academy",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var modelFileStoreService = new Mock<IModelFileStoreService>(MockBehavior.Strict);
            modelFileStoreService
                .Setup(mfss => mfss.GetModelFilenames(It.IsAny<string>()))
                .Returns(new[]
                {
                    "GAG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProviderHistory.json"
                });

            modelFileStoreService
                .Setup(mfss => mfss.Exists(It.IsAny<string>()))
                .Returns(true);

            modelFileStoreService
                .Setup(mfss => mfss.ReadFileAsString(It.IsAny<string>()))
                .Returns(@"{""dataset"": [{""datasetName"":""providerFunding"",""expression"": ""ParentProviderType=AcademyTrust&GroupingReason=Information""}], ""groups"": [{""type"":""HeadingTitleWithFundingStream"",},{""type"":""HtmlParagraph""}]}");

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
                    It.Is<UiModelGroup>(grp => grp.Type == "HeadingTitleWithFundingStream"),
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

            var controller = new ProviderController(
                componentService.Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyGAG().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation().Object,
                realFundingViewService,
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

            var expectedViewModel = new ProviderHistoryViewModel
            {
                ProviderUrn = "--",
                ChoicePageLink = "/choose-a-statement-type",
                OrganisationName = "Abbey View Primary Academy",
                OrganisationUkprn = "10072811",
                SecondaryContentTitle = "General annual grant",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingPeriodPublications = new List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>>(),
                FundingPeriodProviderFundings = new List<KeyValuePair<(int yearFrom, int yearTo), List<ProviderFundingViewModel>>>
                {
                    new KeyValuePair<(int, int), List<ProviderFundingViewModel>>(
                        (2021, 2022), new List<ProviderFundingViewModel>
                        {
                            new ProviderFundingViewModel
                            {
                                StatusChangedDate = new DateTime(2021, 6, 1),
                                FundingPeriodCode = "AC-2122",
                                IsLatest = true,
                                IsFinal = false,
                                VariationReason = "Initial allocation.",
                                GroupingReason = "Information"
                            }
                        }),
                    new KeyValuePair<(int, int), List<ProviderFundingViewModel>>(
                        (2023, 2024), new List<ProviderFundingViewModel>
                        {
                            new ProviderFundingViewModel
                            {
                                StatusChangedDate = new DateTime(2023, 6, 1),
                                FundingPeriodCode = "AC-2324",
                                IsLatest = false,
                                IsFinal = true,
                                VariationReason = "Initial allocation.",
                                GroupingReason = "Information"
                            }
                        })
                },
                FundingViewData = new FundingViewData
                {
                    EntityName = "Abbey View Primary Academy",
                    EntityPrimaryIdentifier = "10072811",
                    EntityAlternativeIdentifier = "8252042",
                    EntityType = "Academies",
                    EntitySubType = "Free schools",
                    FundingStreamCode = "GAG",
                    TotalAmount = 0,
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
                                    "FundingPeriodProviderFundings",
                                    new List<KeyValuePair<(int yearFrom, int yearTo), List<ProviderFundingViewModel>>>
                                    {
                                        new KeyValuePair<(int, int), List<ProviderFundingViewModel>>(
                                            (2021, 2022), new List<ProviderFundingViewModel>
                                            {
                                                new ProviderFundingViewModel
                                                {
                                                    StatusChangedDate = new DateTime(2021, 6, 1),
                                                    FundingPeriodCode = "AC-2122",
                                                    IsLatest = true,
                                                    IsFinal = false,
                                                    VariationReason = "Initial allocation.",
                                                    GroupingReason = "Information"
                                                }
                                            }),
                                        new KeyValuePair<(int, int), List<ProviderFundingViewModel>>(
                                            (2023, 2024), new List<ProviderFundingViewModel>
                                            {
                                                new ProviderFundingViewModel
                                                {
                                                    StatusChangedDate = new DateTime(2023, 6, 1),
                                                    FundingPeriodCode = "AC-2324",
                                                    IsLatest = false,
                                                    IsFinal = true,
                                                    VariationReason = "Initial allocation.",
                                                    GroupingReason = "Information"
                                                }
                                            })
                                    }
                                }
                            },
                        },
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Panel
                        }
                    },
                    FundingPeriodCode = "AC-2122"
                },
                FundingStream = new Web.Areas.Admin.Models.FundingStream.FundingStream
                {
                    FundingStreamCode = "GAG",
                    FundingStreamName = "General annual grant",
                    FundingStreamNameWithinSentence = "General annual grant",
                    RelevantForProviders_LoggedIn = true,
                    RelevantForOrganisations_Public = true,
                    HistoryIndependentOfPublications = true,
                    Active = true,
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "AcademyAcademicYear"
                            },
                            Value = "202122"
                        },
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "UseStaticData"
                            },
                            Value = "true",
                        },
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "ParentProviderType"
                            },
                            Value = "AcademyTrust",
                        },
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "ProviderDownloadSizeInBytes"
                            },
                            Value = "206000",
                        },
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "FundingDocumentFileType"
                            },
                            Value = "CSV",
                        }
                    },
                    NextPayments = new List<NextPayment>(),
                    Publications = new List<Publication>
                    {
                        new Publication
                        {
                            PublishedDate = new DateTime(2030, 1, 1),
                            FundingPeriodCode = "AC-2122",
                            IsLatest = true,
                            Status = PublicationStatus.Published
                        }
                    }
                },
                CurrentUser = new CurrentUserViewModel
                {
                    IsExternalUser = true,
                    IsLoggedIn = true,
                    ProviderName = "Abbey View Primary Academy",
                    Ukprn = 10072811
                }
            };
            expectedViewModel.HomeLink = "/";
            expectedViewModel.BreadCrumbItems[0].ExplicitUrl = "/";

            var fundingStreamNamePathPart = "general-annual-grant";

            // Act
            var actual = await controller.ProviderHistory("10072811", fundingStreamNamePathPart);

            // Assert
            actual
                  .Should().BeOfType<ViewResult>()
                 .Which.Model.Should().BeOfType<ProviderHistoryViewModel>()
                 .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        // TODO - This test should be uncommented when the temporary fix to grant MAT GAG access unconditionally is removed (Method: ProviderController.IsPartOfMAT).
        // [TestMethod]
        public void ProviderHistory_OnlyGAG_MatUserAccess_OrgNotPartOfMat_ResultExpected()
        {
            // Arrange
            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService_MatUser().Object,
                GetUserJourneyService_OnlyGAG().Object,
                GetMapper(),
                GetFundingApiService_NoResults().Object,
                GetFundingViewService_OnlyGAG().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);
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

            var expectedViewModel = new ProviderHistoryViewModel
            {
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingViewData = null,
                CurrentUser = new CurrentUserViewModel()
            };

            var fundingStreamNamePathPart = "general-annual-grant";

            // Act
            Func<Task> act = async () => await controller.ProviderHistory("12345678", fundingStreamNamePathPart);

            // Assert
            act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("You are not authorised to perform this action.");
        }

        [TestMethod]
        public void ProviderHistory_OnlyPSG_MatUserAccess_OrgNotPartOfMat_ResultExpected()
        {
            // Arrange
            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService_MatUser().Object,
                GetUserJourneyService_OnlyPSG().Object,
                GetMapper(),
                GetFundingApiService_NoResults().Object,
                GetFundingViewService_OnlyPSG().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);
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

            var expectedViewModel = new ProviderHistoryViewModel
            {
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingViewData = null,
                CurrentUser = new CurrentUserViewModel()
            };

            var fundingStreamNamePathPart = "pe-and-sport-premium";

            // Act
            Func<Task> act = async () => await controller.ProviderHistory("12345678", fundingStreamNamePathPart);

            // Assert
            act.Should().NotThrowAsync();
        }

        [TestMethod]
        public async Task ProviderHistory_OnlyGAG_MatUserAccess_OrgIsPartOfMat_MockFundingViewService_ResultExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10053512,
                ProviderName = "Abbey View Primary Academy",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService_MatUser().Object,
                GetUserJourneyService_OnlyGAG().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(true).Object,
                GetFundingViewService_OnlyGAG().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider().Object);
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

            var expectedViewModel = new ProviderHistoryViewModel
            {
                FromMatStatementsPage = true,
                ProviderUrn = "--",
                ChoicePageLink = "/choose-a-statement-type",
                OrganisationName = "Abbey View Primary Academy",
                OrganisationUkprn = "10053512",
                SecondaryContentTitle = "General annual grant",
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingPeriodPublications = new List<KeyValuePair<(int yearFrom, int yearTo), List<Publication>>>(),
                FundingPeriodProviderFundings = new List<KeyValuePair<(int yearFrom, int yearTo), List<ProviderFundingViewModel>>>
                {
                    new KeyValuePair<(int, int), List<ProviderFundingViewModel>>(
                        (2021, 2022), new List<ProviderFundingViewModel>
                        {
                            new ProviderFundingViewModel
                            {
                                StatusChangedDate = new DateTime(2021, 6, 1),
                                FundingPeriodCode = "AC-2122",
                                IsLatest = true,
                                IsFinal = false,
                                VariationReason = "Initial allocation.",
                                GroupingReason = "Information"
                            }
                        }),
                    new KeyValuePair<(int, int), List<ProviderFundingViewModel>>(
                        (2023, 2024), new List<ProviderFundingViewModel>
                        {
                            new ProviderFundingViewModel
                            {
                                StatusChangedDate = new DateTime(2023, 6, 1),
                                FundingPeriodCode = "AC-2324",
                                IsLatest = false,
                                IsFinal = true,
                                VariationReason = "Initial allocation.",
                                GroupingReason = "Information"
                            }
                        })
                },
                FundingViewData = new FundingViewData
                {
                    FundingStreamCode = "GAG",
                    TotalAmount = 2906249.75M,
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
                                    "FundingPeriodProviderFundings",
                                    new List<KeyValuePair<(int yearFrom, int yearTo), List<ProviderFundingViewModel>>>
                                    {
                                        new KeyValuePair<(int, int), List<ProviderFundingViewModel>>(
                                            (2021, 2022), new List<ProviderFundingViewModel>
                                            {
                                                new ProviderFundingViewModel
                                                {
                                                    StatusChangedDate = new DateTime(2021, 6, 1),
                                                    FundingPeriodCode = "AC-2122",
                                                    IsLatest = true,
                                                    IsFinal = false,
                                                    VariationReason = "Initial allocation.",
                                                    GroupingReason = "Information"
                                                }
                                            }),
                                        new KeyValuePair<(int, int), List<ProviderFundingViewModel>>(
                                            (2023, 2024), new List<ProviderFundingViewModel>
                                            {
                                                new ProviderFundingViewModel
                                                {
                                                    StatusChangedDate = new DateTime(2023, 6, 1),
                                                    FundingPeriodCode = "AC-2324",
                                                    IsLatest = false,
                                                    IsFinal = true,
                                                    VariationReason = "Initial allocation.",
                                                    GroupingReason = "Information"
                                                }
                                            })
                                    }
                                }
                            },
                        },
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Panel
                        }
                    }
                },
                FundingStream = new Web.Areas.Admin.Models.FundingStream.FundingStream
                {
                    FundingStreamCode = "GAG",
                    FundingStreamName = "General annual grant",
                    FundingStreamNameWithinSentence = "General annual grant",
                    RelevantForProviders_LoggedIn = true,
                    RelevantForOrganisations_Public = true,
                    HistoryIndependentOfPublications = true,
                    Active = true,
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "AcademyAcademicYear"
                            },
                            Value = "202122"
                        },
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "UseStaticData"
                            },
                            Value = "true",
                        },
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "ParentProviderType"
                            },
                            Value = "AcademyTrust",
                        },
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "ProviderDownloadSizeInBytes"
                            },
                            Value = "206000",
                        },
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "FundingDocumentFileType"
                            },
                            Value = "CSV",
                        }
                    },
                    NextPayments = new List<NextPayment>(),
                    Publications = new List<Publication>
                    {
                        new Publication
                        {
                            PublishedDate = new DateTime(2030, 1, 1),
                            FundingPeriodCode = "AC-2122",
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

            var fundingStreamNamePathPart = "general-annual-grant";

            // Act
            var actual = await controller.ProviderHistory("10053512", fundingStreamNamePathPart);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<ProviderHistoryViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod]
        public async Task ProviderStatement_WhenRefererIsMat_ResultExpected()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "Abbey View Primary Academy",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };

            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyGAG().Object,
                GetMapper(),
                GetFundingApiService_SearchFunding_HasUserVisitedFundingInvocation(true).Object,
                GetFundingViewService_OnlyGAG().Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService().Object,
                GetSystemProvider_WithSetUp().Object);
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

            // Act
            var actual = await controller.ProviderStatement(false);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().BeSameAs(LoggedInConstants.RouteName_MultipleAcademyTrustStatement);
        }

        #region ProviderSpreadsheetDownload tests

        /// <summary>
        /// Providers the spreadsheet download for valid published date returns expected file.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task ProviderSpreadsheetDownload_ForValidPublishedDate_ReturnsExpectedFile()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
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
                    It.IsAny<FundingViewScope>(),
                    new[] { FileFormat.ODS },
                    It.Is<SearchFilter[]>(f => f.Length == 1
                        && f.First().PropertyName == SearchFilterPropertyName.Ukprn
                        && f.First().PropertyValue == "10072811"),
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

            var fundingApiMockService = GetFundingApiService_SingleMatchingProviderFundingResult(user.Ukprn.ToString(), "GAG");
            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                settingsServiceMock.Object,
                null,
                fundingApiMockService.Object,
                mockFundingViewService.Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService("http://www.example.org/").Object,
                GetSystemProvider_WithSetUp().Object);

            // Act
            var actual = await controller.ProviderSpreadsheetDownload(new ProviderSpreadsheetDownloadRequest
            {
                Id = "GAG_AY1920_1234567_1_0",
                FundingStreamCode = "GAG",
                PublishedDate = DateTimeExtensions.ToRouteParameterString(new DateTime(2019, 12, 31)),
                Format = FundingDocumentFileType.Spreadsheet_OpenFormat,
                Ukprn = "10072811",
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
        /// Providers the spreadsheet download where no provider funding is found throws exception.
        /// </summary>
        [TestMethod]
        public void ProviderSpreadsheetDownload_WhereNoProviderFundingFound_ThrowsException()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "TEST PROVIDER",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };
            var mockFundingViewService = new Mock<IFundingViewService>();
            var settingsServiceMock = GetMockSettingsService();

            var fundingApiMockService = GetFundingApiService_SingleMatchingFundingResult(user.Ukprn.ToString(), "GAG");
            fundingApiMockService
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
                            StatusChangedDate = new DateTime(2019, 10, 31),
                            Id = "GAG" + "-" + user.Ukprn.ToString()
                        }
                    }
                });
            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                settingsServiceMock.Object,
                null,
                fundingApiMockService.Object,
                mockFundingViewService.Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService("http://www.example.org/").Object,
                GetSystemProvider_WithSetUp().Object);

            var publishedDate = DateTimeExtensions.ToRouteParameterString(new DateTime(2019, 12, 31));

            // Act
            Func<Task> actual = async () => await controller.ProviderSpreadsheetDownload(new ProviderSpreadsheetDownloadRequest
            {
                Id = "GAG_AY1920_1234567_1_0",
                FundingStreamCode = "GAG",
                PublishedDate = publishedDate,
                Format = FundingDocumentFileType.Spreadsheet_OpenFormat,
                Ukprn = "10072811",
                YearFrom = 2019,
                YearTo = 2020,
                YearTypeCode = YearTypeCode.AcademicYear
            });

            // Assert
            actual.Should().ThrowAsync<Exception>().WithMessage($"There is no provider funding for the status change date {publishedDate.ToRouteParameterDate()} for provider with ukprn {user.Ukprn}");
        }

        [TestMethod]
        public async Task ProviderSpreadsheetDownloadNMSS_ForValidPublishedDate_ReturnsExpectedFile()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10015031,
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
                    "AY-2122",
                    It.IsAny<DateTime>(),
                    It.IsAny<Publication>(),
                    FundingViewType.Spreadsheet,
                    It.IsAny<FundingViewScope>(),
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

            var fundingApiMockService = GetFundingApiService_SingleMatchingFundingResult(user.Ukprn.ToString(), "NMSS");
            fundingApiMockService
                .Setup(fas => fas.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false))
                .ReturnsAsync(new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = new List<FundingApiSearchProviderFunding>
                    {
                        new FundingApiSearchProviderFunding
                        {
                            OrganisationName = "A good school",
                            FundingStreamCode = "NMSS",
                            ParentProviderType = "AcademyTrust",
                            FundingPeriodCode = "NMSS-2122",
                            GroupingReason = GroupingReason.Information,
                            StatusChangedDate = new DateTime(2019, 12, 31),
                            Id = "NMSS" + "-" + user.Ukprn.ToString()
                        }
                    }
                });
            var controller = new ProviderController(
                GetComponentService_Basic().Object,
                GetIdentityService().Object,
                settingsServiceMock.Object,
                null,
                fundingApiMockService.Object,
                mockFundingViewService.Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService("http://www.example.org/").Object,
                GetSystemProvider_WithSetUp().Object);

            // Act
            var actual = await controller.ProviderSpreadsheetDownload(new ProviderSpreadsheetDownloadRequest
            {
                Id = "NMSS_AY2122_1234567_1_0",
                FundingStreamCode = "NMSS",
                PublishedDate = DateTimeExtensions.ToRouteParameterString(new DateTime(2019, 12, 31)),
                Format = FundingDocumentFileType.Spreadsheet_OpenFormat,
                Ukprn = nameof(ProviderSpreadsheetDownloadRequest.Ukprn),
                YearFrom = 2021,
                YearTo = 2022,
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
        [TestMethod]
        public void ProviderSpreadsheetDownload_ForInvalidPublishedDate_ThrowsException()
        {
            // Arrange
            var user = new User
            {
                Ukprn = 10072811,
                ProviderName = "TEST PROVIDER",
                Email = "test@test.com",
                IsAuthenticated = true,
                IsExternalUser = true
            };
            var settingsServiceMock = GetMockSettingsService();
            var fundingApiServiceMock = GetFundingApiService_SingleMatchingFundingResult(user.Ukprn.ToString(), "NMSS");
            var controller = new ProviderController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                settingsServiceMock.Object,
                null,
                fundingApiServiceMock.Object,
                null,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object,
                GetAppConfigService("http://www.example.org/").Object,
                null);

            var downloadRequest = new ProviderSpreadsheetDownloadRequest
            {
                Id = "GAG_AY1920_1234567_1_0",
                FundingStreamCode = "GAG",
                PublishedDate = DateTimeExtensions.ToRouteParameterString(DateTime.ParseExact("01/01/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None)
                        .AddDays(1))
            };

            // Act
            Func<Task> act = async () => await controller.ProviderSpreadsheetDownload(downloadRequest);

            // Assert
            act.Should().ThrowAsync<RequestException>().WithMessage($"There are no publications for the date {downloadRequest.PublishedDate}");
        }

        #endregion

        private static IMapper GetMapper()
        {
            return new MapperConfiguration(x => x.AddProfile(new WebAutoMapperProfile())).CreateMapper();
        }

        private static FundingStream GetGagFundingStream(bool needPreviousPublication = false)
        {
            var publications = new List<Publication>
            {
                    new Publication
                    {
                        FundingPeriodCode = "AC-2122",
                        PublishedDate = new DateTime(2030, 1, 1),
                        Status = PublicationStatus.Published
                    }
            };

            if (needPreviousPublication)
            {
                publications.Add(
                new Publication
                {
                    FundingPeriodCode = "AC-2021",
                    PublishedDate = new DateTime(2030, 1, 1),
                    Status = PublicationStatus.Published
                });
            }

            return new FundingStream
            {
                FundingStreamCode = "GAG",
                FundingStreamName = "General annual grant",
                FundingStreamNameWithinSentence = "General annual grant",
                RelevantForProviders_LoggedIn = true,
                RelevantForOrganisations_Public = true,
                HistoryIndependentOfPublications = true,
                Active = true,
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
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "UseStaticData"
                        },
                        Value = "true",
                    },
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "ParentProviderType"
                        },
                        Value = "AcademyTrust",
                    },
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "ProviderDownloadSizeInBytes"
                        },
                        Value = "206000",
                    },
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "FundingDocumentFileType"
                        },
                        Value = "CSV",
                    }
                },
                Publications = publications,
                NextPayments = new List<NextPayment>()
            };
        }

        private static FundingStream Get1619FundingStream()
        {
            return new FundingStream
            {
                FundingStreamCode = "1619",
                FundingStreamName = "16 to 19",
                FundingStreamNameWithinSentence = "16 to 19",
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
                        Value = "true",
                    },
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "ParentProviderType"
                        },
                        Value = "AcademyTrust",
                    }
                },
                Publications = new List<Publication>
                {
                    new Publication
                    {
                        FundingPeriodCode = "AS-2122",
                        PublishedDate = new DateTime(2030, 1, 1),
                        Status = PublicationStatus.Published
                    }
                },
                NextPayments = new List<NextPayment>()
            };
        }

        private static FundingStream GetPsgFundingStream()
        {
            return new FundingStream
            {
                FundingStreamCode = "PSG",
                FundingStreamName = "PE and sport premium",
                RelevantForProviders_LoggedIn = true,
                RelevantForOrganisations_Public = true,
                Active = true,
                SettingValues = new List<SettingValue>
                {
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "AcademicYear"
                        },
                        Value = "201920"
                    }
                },
                Publications = new List<Publication>
                {
                    new Publication
                    {
                        FundingPeriodCode = "AY-1920",
                        PublishedDate = new DateTime(2020, 1, 1),
                        Status = PublicationStatus.Published
                    }
                }
            };
        }

        private Mock<IUserJourneyService> GetUserJourneyService_NoFundingStreams()
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>());

            return userJourneyService;
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

        private Mock<IClaimsBasedIdentityService> GetIdentityServiceSetupForIndicativeStatementUser()
        {
            var identityService = new Mock<IClaimsBasedIdentityService>(MockBehavior.Strict);
            identityService
                .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User
                {
                    Ukprn = 10087061,
                    ProviderName = "TEST PROVIDER",
                    Email = "test@test.com",
                    IsAuthenticated = true,
                    IsExternalUser = true
                });

            return identityService;
        }

        private Mock<IClaimsBasedIdentityService> GetIdentityService_MatUser()
        {
            var identityService = new Mock<IClaimsBasedIdentityService>(MockBehavior.Strict);
            identityService
                .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User
                {
                    Ukprn = 10053512,
                    ProviderName = "MAT PROVIDER",
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

            fundingApiService.Setup(fas => fas.HasUserVisitedFunding(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            return fundingApiService;
        }

        private Mock<IFundingApiService> GetFundingApiService_WithUserVisitNotOccuredDetails()
        {
            var fundingApiService = new Mock<IFundingApiService>(MockBehavior.Strict);

            fundingApiService.Setup(fas => fas.HasUserVisitedFunding(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            fundingApiService.Setup(fas => fas.AddUserFundingViewDetail(It.IsAny<AddUserFundingViewRequest>())).Returns(Task.CompletedTask);

            return fundingApiService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_Only1619()
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>
                {
                    Get1619FundingStream()
                });

            return userJourneyService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_OnlyGAG(bool needPreviousPublication = false)
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>
                {
                    GetGagFundingStream(needPreviousPublication)
                });

            return userJourneyService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_OnlyGAGNoPayments()
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>
                {
                    GetGagFundingStream()
                });

            return userJourneyService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_PSGAndGAG()
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>
                {
                    GetPsgFundingStream(),
                    GetGagFundingStream()
                });

            return userJourneyService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_OnlyPSG()
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>
                {
                    GetPsgFundingStream()
                });

            return userJourneyService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_OnlyGAG_NoPublications()
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            var fundingStream = GetGagFundingStream();
            fundingStream.Publications = new List<Publication>();

            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>
                {
                    fundingStream
                });

            return userJourneyService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_OnlyPSG_NoPublications()
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            var fundingStream = GetPsgFundingStream();
            fundingStream.Publications = new List<Publication>();

            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>
                {
                    fundingStream
                });

            return userJourneyService;
        }

        private Mock<IFundingApiService> GetFundingApiService_SinglePSGProvider()
        {
            var fundingApiService = new Mock<IFundingApiService>(MockBehavior.Strict);

            fundingApiService.Setup(fas => fas.HasUserVisitedFunding(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            fundingApiService
                .Setup(fas => fas.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false))
                .ReturnsAsync(new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = new List<FundingApiSearchProviderFunding>
                    {
                        new FundingApiSearchProviderFunding
                        {
                            OrganisationName = "A School",
                            FundingStreamCode = "PSG",
                            ParentProviderType = "LocalAuthority",
                            FundingPeriodCode = "AY-1920",
                            GroupingReason = "Information"
                        }
                    }
                });

            fundingApiService
                .Setup(fas => fas.SearchFunding(It.IsAny<FundingApiSearchRequestObject>()))
                .ReturnsAsync(GetFundingApiSearchResponse());

            return fundingApiService;
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

        private Mock<IFundingApiService> GetFundingApiService_NoResults()
        {
            var fundingApiService = new Mock<IFundingApiService>(MockBehavior.Strict);
            fundingApiService
                .Setup(fas => fas.SearchProviderFunding(It.IsAny<FundingApiSearchRequestObject>(), false))
                .ReturnsAsync((IFundingApiSearchResponseProviderFunding)null);

            fundingApiService
                .Setup(fas => fas.SearchFunding(It.IsAny<FundingApiSearchRequestObject>()))
                .ReturnsAsync((IFundingApiSearchResponseFunding)null);

            return fundingApiService;
        }

        private Mock<IFundingApiService> GetFundingApiService_SingleFundingResult()
        {
            var fundingApiService = new Mock<IFundingApiService>(MockBehavior.Strict);
            var fundingValue = "123.45";
            var providerfundings = new List<IFundingApiSearchProviderFunding>
            {
                new FundingApiSearchProviderFunding
                {
                    FundingValue = fundingValue
                }
            };
            var fundings = new List<IFundingApiSearchFunding>
            {
                new FundingApiSearchFunding
                {
                    FundingValue = fundingValue
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


        private Mock<IComponentService> GetComponentService_Uncalled()
        {
            var componentService = new Mock<IComponentService>(MockBehavior.Strict);
            return componentService;
        }

        private Mock<ISystemProvider> GetSystemProvider()
        {
            var systemProvider = new Mock<ISystemProvider>(MockBehavior.Strict);

            return systemProvider;
        }

        private Mock<ISystemProvider> GetSystemProvider_WithSetUp()
        {
            var systemProvider = new Mock<ISystemProvider>(MockBehavior.Strict);

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

        private Mock<IFundingViewService> GetFundingViewService_OnlyGAG()
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
                    FundingStreamCode = "GAG",
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

        private Mock<IFundingViewService> GetFundingViewService_OnlyGAGProviderHistory()
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
                    FundingStreamCode = "GAG",
                    TotalAmount = 2906249.75M,
                    Components = new List<Component>
                    {
                        new Component(null)
                        {
                            Type = ComponentType.Heading_TitleWithFundingStream
                        },
                        new Component(null)
                        {
                            Type = ComponentType.Html_Paragraph
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

        private Mock<IFundingViewService> GetFundingViewService_OnlyPSG()
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
                    FundingStreamCode = "PSG",
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

        private Mock<IFundingViewService> GetFundingViewService_PSGAndGAG()
        {
            var fundingViewService = new Mock<IFundingViewService>(MockBehavior.Strict);
            fundingViewService
                .Setup(fvs => fvs.GenerateFundingViewData(
                    It.IsAny<IComponentService>(),
                    It.IsAny<string>(),
                    "PSG",
                    It.Is<FundingStream[]>(fsc => fsc.Length >= 1),
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
                    FundingStreamCode = "PSG",
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
                .Setup(fvs => fvs.GenerateFundingViewData(
                    It.IsAny<IComponentService>(),
                    It.IsAny<string>(),
                    "GAG",
                    It.Is<FundingStream[]>(fsc => fsc.Length >= 1),
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
                    FundingStreamCode = "GAG",
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

        private Mock<IModelFileStoreService> GetModelFileStoreService_GaG()
        {
            var modelFileStoreService = new Mock<IModelFileStoreService>(MockBehavior.Strict);
            modelFileStoreService
                .Setup(mfss => mfss.GetModelFilenames(It.IsAny<string>()))
                .Returns(new[]
                {
                    "GAG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInProvider.json"
                });

            modelFileStoreService
                .Setup(mfss => mfss.Exists(It.IsAny<string>()))
                .Returns(true);

            modelFileStoreService
                .Setup(mfss => mfss.ReadFileAsString(It.IsAny<string>()))
                .Returns(@"{""dataset"": [{""datasetName"":""providerFunding"",""expression"": ""ParentProviderType=AcademyTrust&GroupingReason=Information""}], ""groups"": [{""type"":""AccordionTitle"",},{""type"":""AccordionPanel""}]}");

            return modelFileStoreService;
        }

        private Mock<IModelFileStoreService> GetIndicativeModelFileStoreService_GaG()
        {
            var modelFileStoreService = new Mock<IModelFileStoreService>(MockBehavior.Strict);
            modelFileStoreService
                .Setup(mfss => mfss.GetModelFilenames(It.IsAny<string>()))
                .Returns(new[]
                {
                    "GAG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInIndicativeProvider.json"
                });

            modelFileStoreService
                .Setup(mfss => mfss.Exists(It.IsAny<string>()))
                .Returns(true);

            modelFileStoreService
                .Setup(mfss => mfss.ReadFileAsString(It.IsAny<string>()))
                .Returns(@"{""dataset"": [{""datasetName"":""providerFunding"",""expression"": ""ParentProviderType=AcademyTrust&GroupingReason=Indicative""}], ""groups"": [{""type"":""Heading/TitleWithDate"",},{""type"":""AccordionTitle""}]}");

            return modelFileStoreService;
        }

        private Mock<IGlobalSettingService> GetGlobalSettingService()
        {
            return CommonMocks.GlobalSettingService();
        }

        private Mock<IComponentService> GetComponentService_Basic()
        {
            var componentService = new Mock<IComponentService>(MockBehavior.Strict);
            componentService
                .Setup(cs => cs.GetComponent(
                    It.Is<UiModelGroup>(grp => grp.Type == "AccordionTitle"),
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
                    It.Is<UiModelGroup>(grp => grp.Type == "AccordionPanel"),
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
            componentService
                .Setup(cs => cs.GetComponent(
                    It.Is<UiModelGroup>(grp => grp.Type == "Heading/TitleWithDate"),
                    It.IsAny<ComponentConfiguration>(),
                    It.IsAny<ComponentConfiguration>(),
                    It.IsAny<Dictionary<ComponentType, Defaults>>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Returns(new Component(null)
                {
                    Type = ComponentType.Heading_TitleWithDate
                });

            return componentService;
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
                     FundingStreamCode = "GAG",
                     FundingStreamName = "General Annual Grant",
                     FundingStreamNameWithinSentence = "GAG",
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
                         },
                         new SettingValue
                         {
                             FundingStreamId = 1,
                             SettingId = 2,
                             Value = "AcademyTrust",
                             CreatedAt = DateTime.MaxValue,
                             LastUpdatedAt = DateTime.MaxValue,
                             LastUpdatedBy = "System",
                             Setting = new SettingType
                             {
                                 SettingName = "ParentProviderType"
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
                   },
                   new Services.Models.FundingStream
                   {
                     Id = 2,
                     Active = true,
                     FundingStreamCode = "NMSS",
                     FundingStreamName = "Non-Maintained Special School",
                     FundingStreamNameWithinSentence = "NMSS",
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
                         },
                         new SettingValue
                         {
                             FundingStreamId = 1,
                             SettingId = 2,
                             Value = "AcademyTrust",
                             CreatedAt = DateTime.MaxValue,
                             LastUpdatedAt = DateTime.MaxValue,
                             LastUpdatedBy = "System",
                             Setting = new SettingType
                             {
                                 SettingName = "ParentProviderType"
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
                    ParentProviderType = GroupingType.AcademyTrust,
                    StatusChangedDate = new DateTime(2019, 12, 31),
                    GroupingReason = GroupingReason.Information,
                    Id = fundingstreamCode + "-" + ukprnForFunding
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

            fundingApiService
               .Setup(fas => fas.HasUserVisitedFunding(It.IsAny<string>(), It.IsAny<string>()))
               .ReturnsAsync(false);

            fundingApiService
                .Setup(fas => fas.AddUserFundingViewDetail(It.IsAny<AddUserFundingViewRequest>()))
                .Returns(Task.CompletedTask);

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

            fundingApiService
                .Setup(fas => fas.HasUserVisitedFunding(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            fundingApiService
                .Setup(fas => fas.AddUserFundingViewDetail(It.IsAny<AddUserFundingViewRequest>()))
                .Returns(Task.CompletedTask);

            return fundingApiService;
        }
    }
}