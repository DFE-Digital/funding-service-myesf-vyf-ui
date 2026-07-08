using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Web.Components.Areas.Lists.DTOs;
using Pds.Core.Web.Components.Areas.Lists.Models;
using Pds.Core.Web.Models;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Attributes;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Enums;
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
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using User = Pds.Core.Common.Identity.Models.User;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers.LoggedIn
{
    [TestClass]
    public class MultipleAcademyTrustControllerUnitTests
    {
        private readonly string _contactUsLink;
        private readonly string _feedbackLink;

        public MultipleAcademyTrustControllerUnitTests()
        {
            var appConfig = new ApplicationConfiguration();
            _contactUsLink = appConfig.ContactUsLink;
            _feedbackLink = appConfig.FeedbackLinkForLoggedInView;
        }

        [TestMethod, TestCategory("Unit")]
        public async Task MatProviderStatement_NoFundingProviderFundings_ResultExpected()
        {
            // Arrange
            var controller = new MultipleAcademyTrustController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_NoFundingStreams().Object,
                GetMapper(),
                GetFundingApiService_SinglePSGProvider().Object,
                new Mock<IFundingViewService>(MockBehavior.Strict).Object,
                GetGlobalSettingService().Object,
                new MemoryCacheService(null, 0),
                GetAppConfigService().Object);
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
            var actual = await controller.MultipleAcademyTrustStatement(Mock.Of<ListRequest>());

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(LoggedInConstants.RouteName_ProviderStatement);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task MatProviderStatement_NoFundingStreams_MockFundingViewService_ResultExpected()
        {
            // Arrange
            var controller = new MultipleAcademyTrustController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_NoFundingStreams().Object,
                GetMapper(),
                GetFundingApiService_GetFundingProviderFundings().Object,
                new Mock<IFundingViewService>(MockBehavior.Strict).Object,
                GetGlobalSettingService().Object,
                new MemoryCacheService(null, 0),
                GetAppConfigService().Object);
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

            var expectedViewModel = new MultipleAcademyTrustStatementViewModel
            {
                BackToTopLinkMinimumCount = 25,
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingViewData = new Dictionary<string, FundingViewData>(),
                CurrentUser = new CurrentUserViewModel
                {
                    Ukprn = 10053512
                },
                ListItems = Enumerable.Empty<MatStatementListItem>(),
                FilterCategories = GetListFilters(Enumerable.Empty<MatStatementListItem>(), new Dictionary<string, FundingStream>()),
                HomeLink = "/",
                ChoicePageLink = "/choose-a-statement-type",
            };

            // Act
            var actual = await controller.MultipleAcademyTrustStatement(Mock.Of<ListRequest>());

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<MultipleAcademyTrustStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task MatProviderStatement_Invalid_MatStatus_ResultExpected()
        {
            // Arrange
            var controller = new MultipleAcademyTrustController(
                GetComponentService_Uncalled().Object,
                GetIdentityService_NonMatUser().Object,
                GetUserJourneyService_NoFundingStreams().Object,
                GetMapper(),
                GetFundingApiService_GetFundingProviderFundings(validMat: false).Object,
                new Mock<IFundingViewService>(MockBehavior.Strict).Object,
                GetGlobalSettingService().Object,
                new MemoryCacheService(null, 0),
                GetAppConfigService().Object);
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
            var actual = await controller.MultipleAcademyTrustStatement(Mock.Of<ListRequest>());

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(LoggedInConstants.RouteName_ProviderStatement);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task MatProviderStatement_OnlyGAG_MockFundingViewService_ResultExpected()
        {
            // Arrange
            var controller = new MultipleAcademyTrustController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyGAG().Object,
                GetMapper(),
                GetFundingApiService_GetFundingProviderFundings(true).Object,
                GetFundingViewService_PSGAndGAG().Object,
                GetGlobalSettingService().Object,
                new MemoryCacheService(null, 0),
                GetAppConfigService().Object);
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

            var expectedFundingViewData = new Dictionary<string, FundingViewData>
            {
                {
                    "GAG-",
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
            };

            var expectedListItems = expectedFundingViewData.Select((kvp, index) =>
            {
                var sharedData = kvp.Value.Components.FirstOrDefault()?.PageData;
                var data = sharedData != null ? sharedData.ToDictionary(entry => entry.Key, entry => entry.Value) : new Dictionary<string, object>();
                data.Add("AccordionInstanceNumber", index + 1);

                return new MatStatementListItem
                {
                    Header = kvp.Value.Components[0],
                    Body = kvp.Value.Components[1],
                    FundingViewData = kvp.Value,
                    Data = data
                };
            });

            var expectedViewModel = new MultipleAcademyTrustStatementViewModel
            {
                BackToTopLinkMinimumCount = 25,
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingViewData = expectedFundingViewData,
                CurrentUser = new CurrentUserViewModel
                {
                    Ukprn = 10053512,
                    ProviderName = "Abbey View Primary Academy"
                },
                ListItems = expectedListItems,
                FilterCategories = GetListFilters(expectedListItems, new Dictionary<string, FundingStream>
                {
                    { "GAG", new FundingStream { FundingStreamName = "General annual grant" } }
                }),
                HomeLink = "/",
                ChoicePageLink = "/choose-a-statement-type",
            };

            // Act
            var actual = await controller.MultipleAcademyTrustStatement(Mock.Of<ListRequest>());

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<MultipleAcademyTrustStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod, TestCategory("Integration")]
        public async Task MatProviderStatement_OnlyGAG_RealFundingViewService_ResultExpected()
        {
            // Arrange
            var modelFileStoreService = new Mock<IModelFileStoreService>(MockBehavior.Strict);
            modelFileStoreService
                .Setup(mfss => mfss.GetModelFilenames(It.IsAny<string>()))
                .Returns(new[]
                {
                    "GAG_SchemaMin0-0Max100-0_TemplateMin0-0Max100-0_LoggedInMatProviderSummary.json"
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
                GetFundingApiService_GetFundingProviderFundings(true).Object,
                null,
                null,
                new MemoryCacheService(null, 0));

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

            var controller = new MultipleAcademyTrustController(
                componentService.Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyGAG().Object,
                GetMapper(),
                GetFundingApiService_GetFundingProviderFundings(true).Object,
                realFundingViewService,
                GetGlobalSettingService().Object,
                new MemoryCacheService(null, 0),
                GetAppConfigService().Object);
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

            var providerResults = GetRealData();

            var localAuths = new[]
            {
                Mock.Of<IFundingApiSearchProviderFunding>(pf =>
                    pf.OrganisationName == "St Aidan's Church of England High School" &&
                    pf.LocalAuthorityName == "Southwark")
            };

            var expectedListItems = providerResults.Select((kvp, index) =>
            {
                var fundingViewData = kvp.Value;

                fundingViewData.LocalAuthorityName = localAuths.FirstOrDefault(funding => funding.OrganisationName == fundingViewData.EntityName)?.ParentName;

                var sharedData = kvp.Value.Components.FirstOrDefault()?.PageData;
                var data = sharedData != null ? sharedData.ToDictionary(entry => entry.Key, entry => entry.Value) : new Dictionary<string, object>();
                data.Add("AccordionInstanceNumber", index + 1);

                return new MatStatementListItem
                {
                    Header = kvp.Value.Components[0],
                    Body = kvp.Value.Components[1],
                    FundingViewData = fundingViewData,
                    Data = data
                };
            });

            var expectedViewModel = new MultipleAcademyTrustStatementViewModel
            {
                BackToTopLinkMinimumCount = 25,
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingViewData = GetRealData(),
                CurrentUser = new CurrentUserViewModel
                {
                    Ukprn = 10053512,
                    ProviderName = "Abbey View Primary Academy"
                },
                ListItems = expectedListItems,
                FilterCategories = GetListFilters(expectedListItems, new Dictionary<string, FundingStream>
                {
                    { "GAG", new FundingStream { FundingStreamName = "General annual grant" } }
                }),
                HomeLink = "/",
                ChoicePageLink = "/choose-a-statement-type",
            };

            // Act
            var actual = await controller.MultipleAcademyTrustStatement(Mock.Of<ListRequest>());

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<MultipleAcademyTrustStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task MatProviderStatement_OnlyPSG_MockFundingViewService_ResultExpected()
        {
            // Arrange
            var controller = new MultipleAcademyTrustController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_OnlyPSG().Object,
                GetMapper(),
                GetFundingApiService_GetFundingProviderFundings().Object,
                GetFundingViewService_PSGAndGAG().Object,
                GetGlobalSettingService().Object,
                new MemoryCacheService(null, 0),
                GetAppConfigService().Object);
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

            var expectedFundingViewData = new Dictionary<string, FundingViewData>
            {
                {
                    "PSG-",
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
            };

            var expectedListItems = expectedFundingViewData.Select((kvp, index) =>
            {
                var sharedData = kvp.Value.Components.FirstOrDefault()?.PageData;
                var data = sharedData != null ? sharedData.ToDictionary(entry => entry.Key, entry => entry.Value) : new Dictionary<string, object>();
                data.Add("AccordionInstanceNumber", index + 1);

                return new MatStatementListItem
                {
                    Header = kvp.Value.Components[0],
                    Body = kvp.Value.Components[1],
                    FundingViewData = kvp.Value,
                    Data = data
                };
            });

            var expectedViewModel = new MultipleAcademyTrustStatementViewModel
            {
                BackToTopLinkMinimumCount = 25,
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingViewData = expectedFundingViewData,
                CurrentUser = new CurrentUserViewModel
                {
                    Ukprn = 10053512
                },
                ListItems = expectedListItems,
                FilterCategories = GetListFilters(expectedListItems, new Dictionary<string, FundingStream>
                {
                    { "PSG", new FundingStream { FundingStreamName = "PE and sport premium" } },
                }),
                HomeLink = "/",
                ChoicePageLink = "/choose-a-statement-type",
            };

            // Act
            var actual = await controller.MultipleAcademyTrustStatement(Mock.Of<ListRequest>());

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<MultipleAcademyTrustStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task MatProviderStatement_GAGAndPSG_MockFundingViewService_ResultExpected()
        {
            // Arrange
            var controller = new MultipleAcademyTrustController(
                GetComponentService_Uncalled().Object,
                GetIdentityService().Object,
                GetUserJourneyService_PSGAndGAG().Object,
                GetMapper(),
                GetFundingApiService_GetFundingProviderFundings().Object,
                GetFundingViewService_PSGAndGAG().Object,
                GetGlobalSettingService().Object,
                new MemoryCacheService(null, 0),
                GetAppConfigService().Object);
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

            var expectedFundingViewData = new Dictionary<string, FundingViewData>
            {
                {
                    "GAG-",
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
                    "PSG-",
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
            };

            var expectedListItems = expectedFundingViewData.Select((kvp, index) =>
            {
                var sharedData = kvp.Value.Components.FirstOrDefault()?.PageData;
                var data = sharedData != null ? sharedData.ToDictionary(entry => entry.Key, entry => entry.Value) : new Dictionary<string, object>();
                data.Add("AccordionInstanceNumber", index + 1);

                return new MatStatementListItem
                {
                    Header = kvp.Value.Components[0],
                    Body = kvp.Value.Components[1],
                    FundingViewData = kvp.Value,
                    Data = data
                };
            });

            var expectedViewModel = new MultipleAcademyTrustStatementViewModel
            {
                BackToTopLinkMinimumCount = 25,
                ContactUsLink = _contactUsLink,
                FeedbackLink = _feedbackLink,
                FundingViewData = expectedFundingViewData,
                CurrentUser = new CurrentUserViewModel
                {
                    Ukprn = 10053512,
                    ProviderName = "Abbey View Primary Academy"
                },
                ListItems = expectedListItems,
                FilterCategories = GetListFilters(expectedListItems, new Dictionary<string, FundingStream>
                {
                    { "GAG", new FundingStream { FundingStreamName = "General annual grant" } },
                    { "PSG", new FundingStream { FundingStreamName = "PE and sport premium" } },
                }),
                HomeLink = "/",
                ChoicePageLink = "/choose-a-statement-type",
            };

            // Act
            var actual = await controller.MultipleAcademyTrustStatement(Mock.Of<ListRequest>());

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<MultipleAcademyTrustStatementViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        private static IEnumerable<IFilterCategoryViewModel> GetListFilters(
            IEnumerable<MatStatementListItem> listItems,
            IDictionary<string, FundingStream> fundingStreams)
        {
            var academies = listItems.Select(item => item.FundingViewData.EntityName).Distinct();
            var fundingTypes = listItems.Select(item => item.FundingViewData.FundingStreamCode).Distinct();
            var localAuthorities = listItems.Select(item => item.FundingViewData.LocalAuthorityName).Distinct();

            var academiesFilter = new ListFilterCategoryViewModel
            {
                Key = "academy",
                Title = "Filter by academy",
                Groups = Enumerable.Empty<ListGroupFilterViewModel>(),
                Values = academies.Select(academy => new FilterValueViewModel
                {
                    Title = academy,
                    Value = academy,
                    Count = listItems.Count(item => item.FundingViewData.EntityName == academy),
                    Selected = false
                })
            };

            var fundingTypesFilter = new ListFilterCategoryViewModel
            {
                Key = "fundingType",
                Title = "Filter by funding type",
                Groups = Enumerable.Empty<ListGroupFilterViewModel>(),
                Values = fundingTypes.Select(fundingType => new FilterValueViewModel
                {
                    Title = fundingStreams[fundingType].FundingStreamName,
                    Value = fundingType,
                    Count = listItems.Count(item => item.FundingViewData.FundingStreamCode == fundingType),
                    Selected = false
                })
            };

            var localAuthorityFilter = new ListFilterCategoryViewModel
            {
                Key = "localAuthority",
                Title = "Filter by local authority",
                Groups = Enumerable.Empty<ListGroupFilterViewModel>(),
                Values = localAuthorities.Select(la => new FilterValueViewModel
                {
                    Title = la,
                    Value = la,
                    Count = listItems.Count(item => item.FundingViewData.LocalAuthorityName == la),
                    Selected = false
                })
            };

            return new[]
            {
                academiesFilter,
                fundingTypesFilter,
                localAuthorityFilter
            };
        }

        private static IMapper GetMapper()
        {
            return new MapperConfiguration(x => x.AddProfile(new WebAutoMapperProfile())).CreateMapper();
        }

        private static FundingStream GetGagFundingStream()
        {
            return new FundingStream
            {
                FundingStreamCode = "GAG",
                FundingStreamName = "General annual grant",
                FundingStreamNameWithinSentence = "General annual grant",
                RelevantForProviders_LoggedIn = true,
                RelevantForOrganisations_Public = true,
                RelevantForOrganisations_LoggedIn = true,
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
                    }
                },
                Publications = new List<Publication>
                {
                    new Publication
                    {
                        FundingPeriodCode = "AC-2122",
                        PublishedDate = new DateTime(2030, 1, 1),
                        Status = PublicationStatus.Published
                    }
                },
                NextPayments = new List<NextPayment>()
            };
        }

        private static Dictionary<string, FundingViewData> GetRealData()
        {
            var result = new Dictionary<string, FundingViewData>
            {
                {
                    "GAG-10072811",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy",
                        EntityPrimaryIdentifier = "10072811",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Free schools",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10072812",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy 2",
                        EntityPrimaryIdentifier = "10072812",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Free schools",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10072813",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy 3",
                        EntityPrimaryIdentifier = "10072813",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Free schools",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        FundingStreamCode = "GAG",
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10072814",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy 4",
                        EntityPrimaryIdentifier = "10072814",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Free schools",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10072815",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy 5",
                        EntityPrimaryIdentifier = "10072815",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Free schools",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10072816",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy 6",
                        EntityPrimaryIdentifier = "10072816",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Free schools",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10072817",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy 7",
                        EntityPrimaryIdentifier = "10072817",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Free schools",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10072818",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy 8",
                        EntityPrimaryIdentifier = "10072818",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Free schools",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10072819",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy 9",
                        EntityPrimaryIdentifier = "10072819",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Free schools",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10072820",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy 10",
                        EntityPrimaryIdentifier = "10072820",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Free schools",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10072821",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy 11",
                        EntityPrimaryIdentifier = "10072821",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Free schools",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10072822",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy 12",
                        EntityPrimaryIdentifier = "10072822",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        EntitySubType = "Free schools",
                        FundingStreamCode = "GAG",
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10072823",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy 13",
                        EntityPrimaryIdentifier = "10072823",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Free schools",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        FundingStreamCode = "GAG",
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10072824",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy 14",
                        EntityPrimaryIdentifier = "10072824",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        EntitySubType = "Free schools",
                        FundingStreamCode = "GAG",
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10072825",
                    new FundingViewData
                    {
                        EntityName = "Abbey View Primary Academy 15",
                        EntityPrimaryIdentifier = "10072825",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Free schools",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        FundingStreamCode = "GAG",
                        TotalAmount = 436228.22M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10084320",
                    new FundingViewData
                    {
                        EntityName = "Shrewsbury Academy",
                        EntityPrimaryIdentifier = "10084320",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Academy sponsor led",
                        FundingStreamCode = "GAG",
                        TotalAmount = 7312439.57M,
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10047220",
                    new FundingViewData
                    {
                        EntityName = "Harris Academy Tottenham",
                        EntityPrimaryIdentifier = "10047220",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Academy sponsor led",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 7312439.57M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10021072",
                    new FundingViewData
                    {
                        EntityName = "Belvedere Academy",
                        EntityPrimaryIdentifier = "10021072",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Academy sponsor led",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 413390.84M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10061450",
                    new FundingViewData
                    {
                        EntityName = "Great Barr Academy",
                        EntityPrimaryIdentifier = "10061450",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Academy sponsor led",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 413390.84M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10047466",
                    new FundingViewData
                    {
                        EntityName = "Wombourne High School",
                        EntityPrimaryIdentifier = "10047466",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Academy sponsor led",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 413390.84M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10081419",
                    new FundingViewData
                    {
                        EntityName = "Springfield Primary Academy",
                        EntityPrimaryIdentifier = "10081419",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Academy sponsor led",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 413390.84M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10067283",
                    new FundingViewData
                    {
                        EntityName = "All Saints CofE Academy Denstone",
                        EntityPrimaryIdentifier = "10067283",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Academy converter",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 413390.84M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10078119",
                    new FundingViewData
                    {
                        EntityName = "Brinscall Primary School",
                        EntityPrimaryIdentifier = "10078119",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Academies",
                        EntitySubType = "Free schools special",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 413390.84M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10021055",
                    new FundingViewData
                    {
                        EntityName = "Bradford Academy",
                        EntityPrimaryIdentifier = "10021055",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Free Schools",
                        EntitySubType = "Free schools",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 413390.84M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10034949",
                    new FundingViewData
                    {
                        EntityName = "Simon Balle All-Through School",
                        EntityPrimaryIdentifier = "10034949",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Special schools",
                        EntitySubType = "Academy special sponsor led",
                        FundingStreamCode = "GAG",
                        InYearOpener = false,
                        IsIndicativeFunding = false,
                        TotalAmount = 7312439.57M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10038354",
                    new FundingViewData
                    {
                        EntityName = "Bowland High",
                        EntityPrimaryIdentifier = "10038354",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Special schools",
                        EntitySubType = "Academy special sponsor led",
                        FundingStreamCode = "GAG",
                        InYearOpener = false,
                        IsIndicativeFunding = false,
                        TotalAmount = 2740800M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                },
                {
                    "GAG-10034857",
                    new FundingViewData
                    {
                        EntityName = "St Aidan's Church of England High School",
                        EntityPrimaryIdentifier = "10034857",
                        EntityAlternativeIdentifier = null,
                        EntityType = "Special schools",
                        EntitySubType = "Academy special sponsor led",
                        FundingStreamCode = "GAG",
                        InYearOpener = true,
                        IsIndicativeFunding = false,
                        TotalAmount = 2740800M,
                        Components = new List<Component>
                        {
                            new Component(null) { Type = ComponentType.Accordion_Title },
                            new Component(null) { Type = ComponentType.Accordion_Panel }
                        },
                        PublicationDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2122"
                    }
                }
            };

            return result;
        }

        private static FundingStream GetPsgFundingStream()
        {
            return new FundingStream
            {
                FundingStreamCode = "PSG",
                FundingStreamName = "PE and sport premium",
                RelevantForProviders_LoggedIn = true,
                RelevantForOrganisations_Public = true,
                RelevantForOrganisations_LoggedIn = true,
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
                .ReturnsAsync(new User { Ukprn = 10053512 });

            return identityService;
        }

        private Mock<IClaimsBasedIdentityService> GetIdentityService_NonMatUser()
        {
            var identityService = new Mock<IClaimsBasedIdentityService>(MockBehavior.Strict);
            identityService
                .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User { Ukprn = 10002555 });

            return identityService;
        }

        private Mock<IGlobalSettingService> GetGlobalSettingService()
        {
            return CommonMocks.GlobalSettingService();
        }

        private Mock<IOptions<ApplicationConfiguration>> GetAppConfigService()
        {
            var appConfigService = new Mock<IOptions<ApplicationConfiguration>>(MockBehavior.Strict);
            appConfigService
                .Setup(s => s.Value)
                .Returns(new ApplicationConfiguration());

            return appConfigService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_OnlyGAG()
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

        private Mock<IFundingApiService> GetFundingApiService_GetFundingProviderFundings(bool forMat = false, bool validMat = true)
        {
            var fundingApiService = new Mock<IFundingApiService>(MockBehavior.Strict);

            fundingApiService
                .Setup(fas => fas.SearchFunding(It.IsAny<FundingApiSearchRequestObject>()))
                .ReturnsAsync(new FundingApiSearchResponse
                {
                    Funding = new List<IFundingApiSearchFunding>
                    {
                        new FundingApiSearchFunding
                        {
                            GroupingType = validMat ? "AcademyTrust" : string.Empty,
                            ProviderFundings = new List<string>
                            {
                                forMat ? "PSG-AY-1920-10035320-2_0" : "PSG-AY-1920-10081072-2_0"
                            },
                        }
                    }
                });

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
                            ParentName = "Local auth name",
                            FundingPeriodCode = "AY-1920"
                        },
                        new FundingApiSearchProviderFunding
                        {
                            OrganisationName = "A School",
                            FundingStreamCode = "PSG",
                            ParentProviderType = "AcademyTrust",
                            FundingPeriodCode = "AY-1920"
                        }
                    }
                });

            return fundingApiService;
        }

        private Mock<IFundingApiService> GetFundingApiService_SinglePSGProvider()
        {
            var fundingApiService = new Mock<IFundingApiService>(MockBehavior.Strict);
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
                            ParentName = "Local auth name",
                            FundingPeriodCode = "AY-1920"
                        },
                        new FundingApiSearchProviderFunding
                        {
                            OrganisationName = "A School",
                            FundingStreamCode = "PSG",
                            ParentProviderType = "AcademyTrust",
                            FundingPeriodCode = "AY-1920"
                        }
                    }
                });

            fundingApiService
                .Setup(fas => fas.SearchFunding(It.IsAny<FundingApiSearchRequestObject>()))
                .ReturnsAsync(new FundingApiSearchResponse
                {
                    Funding = new List<IFundingApiSearchFunding>
                    {
                        new FundingApiSearchFunding
                        {
                            GroupingType = "AcademyTrust",
                            ProviderFundings = new List<string>()
                        }
                    }
                });


            return fundingApiService;
        }

        private Mock<IComponentService> GetComponentService_Uncalled()
        {
            var componentService = new Mock<IComponentService>(MockBehavior.Strict);
            return componentService;
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

            var fundingStream = new FundingApiSearchFundingStream();

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
                        FundingStreams = new[] { fundingStream }, SearchTerm = string.Empty
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

            return fundingViewService;
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
    }
}