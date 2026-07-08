using FluentAssertions;
using Moq;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models;
using PDS.VYF.Services.Abstracts.AppServices;
using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
using PDS.VYF.Services.Enums;
using PDS.VYF.Services.Implementations.AppServices;
using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;
using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;
using PDS.VYF.Services.Models.ResponseModels.ViewDataResponseModels;
using PDS.VYF.Services.Tests.Mocks.InfraServices.DataApiClientServices;
using PDS.VYF.Services.Tests.Mocks.OtherServices;
using System.Data;

namespace PDS.VYF.Services.Tests.Implementations.AppServices
{
    [TestClass, TestCategory("Unit")]
    public class ChildFundingViewServicesTests
    {
        /// <summary>
        /// The test publication date.
        /// </summary>
        private static readonly DateTime TestPublicationDate = new DateTime(2030, 01, 01);

        private readonly MockCacheService mockCacheService = new();
        private readonly MockChildApiClientServices mockChildApiClientServices = new();
        private MockRepository mockRepository;
        private Mock<ISharedFundingViewServices> mockSharedFundingViewServices;
        private Mock<ILoggerAdapter<ModelFundingViewService>> mockLoggerAdapter;
        private Mock<IParentApiClientServices> mockParentApiClientServices;
        private Mock<IFundingStreamSettingsServices> mockFundingStreamSettingsServices;
        private Mock<IUserCountApiClientServices> mockUserCountApiClientServices;

        public ChildFundingViewServicesTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockSharedFundingViewServices = this.mockRepository.Create<ISharedFundingViewServices>();
            this.mockLoggerAdapter = this.mockRepository.Create<ILoggerAdapter<ModelFundingViewService>>();
            this.mockParentApiClientServices = this.mockRepository.Create<IParentApiClientServices>();
            this.mockFundingStreamSettingsServices = this.mockRepository.Create<IFundingStreamSettingsServices>();
            this.mockUserCountApiClientServices = this.mockRepository.Create<IUserCountApiClientServices>();
        }

        [TestMethod]
        public async Task GetChildSummaryViewDataInternal_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var childFundingViewServices = this.CreateChildFundingViewServices();

            var request = new ChildSummaryViewDataRequestModel()
            {
                UkprnFromLoggedInUser = "12340000",
                UserId = "ravimekala4074495ravi.mekala@education.gov.uk",
            };

            var fundingStream = new FundingStream
            {
                Publications = new List<Publication>
                    {
                        new Publication
                        {
                            PublishedDate = new DateTime(2030, 1, 1),
                            FundingPeriodCode = "AC-2425",
                            IsLatest = true,
                            Status = ViewYourFunding.Services.Enums.PublicationStatus.Published
                        }
                    },
                SettingValues = new List<ViewYourFunding.Services.Models.SettingValue>
                        {
                            new ViewYourFunding.Services.Models.SettingValue
                            {
                                Setting = new SettingType
                                {
                                    SettingName = "DigitalStatementsGoLiveDate"
                                },
                                Value = "17/05/2024"
                            }
                        }
            };


            mockChildApiClientServices.SetupSearchChild(new List<LoggedInChildModel>()
            {
                new ()
                {
                    OrganisationName = "Dummy Child name",
                    FundingStreamCode = "GAG",
                    FundingPeriodCode = "AC-2425",
                    SchemaVersion = "1.0",
                    TemplateVersion = "1.0",
                    Id = "GAG-AC-2425-12345678-1_0",
                    StatusChangedDate = new DateTime(2024, 08, 28),
                    StatusChangedDateOnly = new DateTime(2024, 08, 28),
                }
            });

            var result = new PDS.ViewYourFunding.Services.DTOs.FundingViewData
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
            };

            var expectedViewModel = new ChildSummaryViewDataResponseModel
            {
                HasFundingDataExists = true,
                OrganizationName = "Dummy Child name",
                FundingViewData = new Dictionary<string, FundingViewData>
                {
                    {
                        "GAG-AC-2425-12345678-1_0",
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
                }
            };

            var fundingStreamPeriod = new List<string>()
            {
                "GAG-AC-2425"
            };

            expectedViewModel.StatementVisitInfo.Add("GAG-AC-2425-12345678-1_0", StatementVisitInfoEnum.UpdatedUnread);

            var periodcode = new List<string>
            {
                "GAG-AC-2425", "GAG-AC-2526", "GAG-AC-2627"
            };

            mockFundingStreamSettingsServices.Setup(s => s.GetEmailEnabledFundingStreamPeriod()).ReturnsAsync(periodcode);

            mockChildApiClientServices.SetupLatestFundingPeriod(new List<string>()
            {
                "GAG-AC-2526"
            });

            mockFundingStreamSettingsServices.Setup(s => s.GetFundingStream("GAG")).ReturnsAsync(fundingStream);

            mockUserCountApiClientServices.Setup(s => s.HasUserVisited(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            mockSharedFundingViewServices.Setup(s => s.GetFundingViewData(It.IsAny<ChildSummaryViewDataRequestModel>())).ReturnsAsync(result);

            // Act
            var actual = await childFundingViewServices.GetChildSummaryViewDataInternal(
                request);

            // Assert
            this.mockRepository.VerifyAll();

            actual.Should().BeOfType<ChildSummaryViewDataResponseModel>()
            .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod]
        public async Task GetChildSummaryViewData_StateUnderTestFromCache_ExpectedBehavior()
        {
            // Arrange
            var childFundingViewServices = this.CreateChildFundingViewServices();

            var request = new ChildSummaryViewDataRequestModel()
            {
                UkprnFromLoggedInUser = "12340000",
                UserId = "ravimekala4074495ravi.mekala@education.gov.uk",
            };

            mockChildApiClientServices.SetupSearchChild(new List<LoggedInChildModel>()
            {
                new ()
                {
                    OrganisationName = "Dummy Child name",
                    FundingStreamCode = "GAG",
                    FundingPeriodCode = "AC-2425",
                    SchemaVersion = "1.0",
                    TemplateVersion = "1.0",
                    Id = "GAG-AC-2425-12345678-1_0",
                    StatusChangedDate = new DateTime(2024, 08, 28),
                    StatusChangedDateOnly = new DateTime(2024, 08, 28),
                }
            });

            var expectedResult = new ChildSummaryViewDataResponseModel
            {
                HasFundingDataExists = true,
                OrganizationName = "Dummy Child name",
                FundingViewData = new Dictionary<string, FundingViewData>
                {
                    {
                        "GAG-AC-2425-12345678-1_0",
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
                }
            };

            expectedResult.StatementVisitInfo.Add("GAG-AC-2425-12345678-1_0", StatementVisitInfoEnum.UpdatedUnread);

            mockCacheService.SetupAddOrGetExistingResultAsync($"Child-SummaryPage-ViewData-ravimekala4074495ravi.mekala@education.gov.uk-12340000-False", ViewYourFunding.Services.Cache.CacheExpirationPolicy.Sliding, expectedResult);

            // Act
            var actual = await childFundingViewServices.GetChildSummaryViewData(
                request);

            // Assert
            this.mockRepository.VerifyAll();

            actual.Should().BeOfType<ChildSummaryViewDataResponseModel>()
            .Which.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public async Task GetChildDetailedViewDataInternal_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var childFundingViewServices = this.CreateChildFundingViewServices();

            var fundingStream = new FundingStream
            {
                Publications = new List<Publication>
                    {
                        new Publication
                        {
                            PublishedDate = new DateTime(2030, 1, 1),
                            FundingPeriodCode = "AC-2425",
                            IsLatest = true,
                            Status = ViewYourFunding.Services.Enums.PublicationStatus.Published
                        }
                    },
                SettingValues = new List<ViewYourFunding.Services.Models.SettingValue>
                        {
                            new ViewYourFunding.Services.Models.SettingValue
                            {
                                Setting = new SettingType
                                {
                                    SettingName = "DigitalStatementsGoLiveDate"
                                },
                                Value = "17/05/2024"
                            }
                        }
            };

            var request = new ChildDetailedViewDataRequestModel()
            {
                UkprnFromRoute = "12345678",
                UkprnFromLoggedInUser = "12345678",
                FundingStreamNamePathPart = "general-annual-grant",
                PublishedDate = "01-01-2030",
                YearFrom = 2024,
                YearTo = 2025,
                FundingStreamCode = "GAG",
                FundingPeriodCode = "AC-2425",
                FundingStreamConfig = fundingStream,
                UserId = "ravimekala4074495ravi.mekala@education.gov.uk"
            };

            mockChildApiClientServices.SetupSearchChild(new List<LoggedInChildModel>()
            {
                new ()
                {
                    OrganisationName = "Dummy Child name",
                    FundingStreamCode = "GAG",
                    FundingPeriodCode = "AC-2425",
                    SchemaVersion = "1.0",
                    TemplateVersion = "1.0",
                    Id = "GAG-AC-2425-12345678-1_0",
                    StatusChangedDate = new DateTime(2024, 08, 28),
                    StatusChangedDateOnly = new DateTime(2024, 08, 28),
                }
            });

            var result = new PDS.ViewYourFunding.Services.DTOs.FundingViewData
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
            };

            var expectedResult = new ChildDetailedViewDataResponseModel()
            {
                HasFundingDataExists = true,
                OrganizationName = "Dummy Child name",
                FundingViewData = new FundingViewData
                {
                    FundingStreamCode = "GAG",
                    PublicationDate = DateTime.MinValue,
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
            };

            mockFundingStreamSettingsServices.Setup(s => s.GetEmailEnabledFundingStreamPeriod()).ReturnsAsync(new List<string> { "GAG-AC-2627, GAG-AC-2526" });

            mockChildApiClientServices.Setup(s => s.LatestFundingPeriod(It.IsAny<ChildSearchApiRequestModel>())).ReturnsAsync(new List<string> { "GAG-AC-2627" });

            mockChildApiClientServices.Setup(s => s.IsLatestStatement(It.IsAny<string>(), It.IsAny<ChildSearchApiRequestModel>(), It.IsAny<bool>(), It.IsAny<List<string>?>())).ReturnsAsync(true);

            mockSharedFundingViewServices.Setup(s => s.GetFundingViewData(It.IsAny<ChildDetailedViewDataRequestModel>())).ReturnsAsync(result);

            mockUserCountApiClientServices.Setup(s => s.AddUserVisitedInfo(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<bool>());

            // Act
            var actual = await childFundingViewServices.GetChildDetailedViewDataInternal(
                request);

            // Assert
            this.mockRepository.VerifyAll();
            actual.Should().BeOfType<ChildDetailedViewDataResponseModel>()
            .Which.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public async Task GetChildDetailedViewData_StateUnderTestFromCache_ExpectedBehavior()
        {
            // Arrange
            var childFundingViewServices = this.CreateChildFundingViewServices();
            var request = new ChildDetailedViewDataRequestModel()
            {
                UkprnFromRoute = "12345678",
                UkprnFromLoggedInUser = "12345678",
                FundingStreamNamePathPart = "general-annual-grant",
                PublishedDate = "01-01-2030",
                YearFrom = 2024,
                YearTo = 2025,
                FundingStreamCode = "GAG",
                FundingPeriodCode = "AC-2425",
                PublicationDate = new DateTime(2030, 01, 01),
                UserId = "testUserId@gov.uk",
            };

            var fundingStreamCodeAndName = new Dictionary<string, string>();
            fundingStreamCodeAndName.Add("GAG", "General annual grant");

            var fundingStream = new FundingStream
            {
                FundingStreamCode = "GAG",
                FundingStreamName = "General annual grant",
                Publications = new List<Publication>
                    {
                        new Publication
                        {
                            PublishedDate = new DateTime(2030, 1, 1),
                            FundingPeriodCode = "AC-2425",
                            IsLatest = true,
                            Status = ViewYourFunding.Services.Enums.PublicationStatus.Published
                        }
                    },

                SettingValues = new List<ViewYourFunding.Services.Models.SettingValue>
                {
                    new ViewYourFunding.Services.Models.SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "AcademyAcademicYear"
                        },
                        Value = "202425"
                    }
                }
            };

            mockFundingStreamSettingsServices.Setup(s => s.GetFundingStreamCodeAndName(It.IsAny<bool>())).ReturnsAsync(fundingStreamCodeAndName);

            mockFundingStreamSettingsServices.Setup(s => s.GetFundingStream(It.IsAny<string>())).ReturnsAsync(fundingStream);

            mockChildApiClientServices.SetupSearchChild(new List<LoggedInChildModel>()
            {
                new ()
                {
                    OrganisationName = "Dummy Child name",
                    FundingStreamCode = "GAG",
                    FundingPeriodCode = "AC-2425",
                    SchemaVersion = "1.0",
                    TemplateVersion = "1.0",
                    Id = "GAG-AC-2425-12345678-1_0",
                    StatusChangedDate = new DateTime(2024, 08, 28),
                    StatusChangedDateOnly = new DateTime(2024, 08, 28),
                }
            });

            mockChildApiClientServices
                .Setup(a => a.GetChildComparison(It.IsAny<ChildComparisonRequest>()))
                .ReturnsAsync(new Dictionary<ComparisonTypeEnum, ChildComparisonResponse>());

            var fundingViewDataResponse = new PDS.ViewYourFunding.Services.DTOs.FundingViewData
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
            };

            var expectedResult = new ChildDetailedViewDataResponseModel()
            {
                HasFundingDataExists = true,
                OrganizationName = "Dummy Child name",
                FundingViewData = new FundingViewData
                {
                    FundingStreamCode = "GAG",
                    PublicationDate = DateTime.MinValue,
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
            };

            mockChildApiClientServices.Setup(s => s.IsLatestStatement(It.IsAny<string>(), It.IsAny<ChildSearchApiRequestModel>(), It.IsAny<bool>(), It.IsAny<List<string>?>())).ReturnsAsync(true);

            mockCacheService.SetupAddOrGetExistingResultAsync($"ChildDetailedViewData-GAG-AC-2425-12345678-{request.PublicationDate?.ToString("yyyy-MM-dd")}-False-{"testUserId@gov.uk"}-NoComparison", ViewYourFunding.Services.Cache.CacheExpirationPolicy.Sliding, expectedResult);

            // Act
            var actual = await childFundingViewServices.GetChildDetailedViewData(
                request);

            // Assert
            this.mockRepository.VerifyAll();
            actual.Should().BeOfType<ChildDetailedViewDataResponseModel>()
                .Which.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public async Task GetChildHistoryViewDataInternal_StateUnderTestMultiple_ExpectedBehavior()
        {
            // Arrange
            var childFundingViewServices = this.CreateChildFundingViewServices();
            var request = new ChildHistoryViewDataRequestModel()
            {
                UkprnFromRoute = "12345678",
                UkprnFromLoggedInUser = "12345678",
                FundingStreamNamePathPart = "general-annual-grant",
                FundingStreamCode = "GAG",
                FundingPeriodCode = "AC-2425",
                DigitalGoLiveDate = new DateTime(2024, 01, 01),
                FundingStreamConfig = new FundingStream
                {
                    HistoryIndependentOfPublications = true
                }
            };

            var periodcode = new List<string>
            {
                "GAG-AC-2425", "GAG-AC-2526", "GAG-AC-2627"
            };

            mockChildApiClientServices.SetupLatestFundingPeriod(new List<string>()
            {
                "GAG-AC-2526"
            });


            mockChildApiClientServices.SetupSearchChild(new List<LoggedInChildModel>()
            {
                new ()
                {
                    OrganisationName = "Dummy Child name",
                    FundingStreamCode = "GAG",
                    FundingPeriodCode = "AC-2425",
                    SchemaVersion = "1.2",
                    TemplateVersion = "2.0",
                    Id = "GAG-AC-2425-12345678-2_0",
                    StatusChangedDate = new DateTime(2024, 08, 28),
                    StatusChangedDateOnly = new DateTime(2024, 08, 28),
                    IsLatest = true,
                    ProviderUrn = "123456",
                    YearFrom = 2024,
                    YearTo = 2025,
                    InYearOpener = false
                },
                new ()
                {
                    OrganisationName = "Dummy Child name",
                    FundingStreamCode = "GAG",
                    FundingPeriodCode = "AC-2425",
                    SchemaVersion = "1.2",
                    TemplateVersion = "2.0",
                    Id = "GAG-AC-2425-12345678-1_0",
                    StatusChangedDate = new DateTime(2024, 07, 28),
                    StatusChangedDateOnly = new DateTime(2024, 07, 28),
                    IsLatest = true,
                    ProviderUrn = "123456",
                    YearFrom = 2024,
                    YearTo = 2025,
                    InYearOpener = false
                }
            });

            var result = new PDS.ViewYourFunding.Services.DTOs.FundingViewData
            {
                FundingStreamCode = "GAG",
                TotalAmount = 2906249.75M,
                Components = new List<Component>
                {
                    new Component(null)
                    {
                        Type = ComponentType.Accordion_Panel,
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
                                    new KeyValuePair<(int, int), List<ProviderFundingViewModel>>((2024, 2025), new List<ProviderFundingViewModel>
                                    {
                                        new ProviderFundingViewModel
                                        {
                                            StatusChangedDate = new DateTime(2024, 08, 28),
                                            FundingPeriodCode = "AC-2425",
                                            IsLatest = true,
                                            IsFinal = false,
                                            VariationReason = "Revised allocation.",
                                            GroupingReason = "Payment"
                                        },
                                        new ProviderFundingViewModel
                                        {
                                            StatusChangedDate = new DateTime(2024, 07, 28),
                                            FundingPeriodCode = "AC-2425",
                                            IsLatest = false,
                                            IsFinal = false,
                                            VariationReason = "Initial allocation.",
                                            GroupingReason = "Information"
                                        }
                                    })
                                }
                            }
                        },
                    }
                }
            };

            mockSharedFundingViewServices.Setup(s => s.GetFundingViewData(It.IsAny<ChildHistoryViewDataRequestModel>())).ReturnsAsync(result);

            var expectedModel = new ChildHistoryViewDataResponseModel()
            {
                HasFundingDataExists = true,
                HasUserHaveRightAccess = true,
                IsValidUrl = true,
                OrganizationName = "Dummy Child name",
                OrganizationUrn = "123456",
                FundingViewData = new FundingViewData
                {
                    FundingStreamCode = "GAG",
                    PublicationDate = DateTime.MinValue,
                    TotalAmount = 2906249.75M,
                    Components = new List<Component>
                    {
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Panel,
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
                                        new KeyValuePair<(int, int), List<ProviderFundingViewModel>>((2024, 2025), new List<ProviderFundingViewModel>
                                        {
                                            new ProviderFundingViewModel
                                            {
                                                StatusChangedDate = new DateTime(2024, 08, 28),
                                                FundingPeriodCode = "AC-2425",
                                                IsLatest = true,
                                                IsFinal = false,
                                                VariationReason = "Revised allocation.",
                                                GroupingReason = "Payment"
                                            },
                                            new ProviderFundingViewModel
                                            {
                                                StatusChangedDate = new DateTime(2024, 07, 28),
                                                FundingPeriodCode = "AC-2425",
                                                IsLatest = false,
                                                IsFinal = false,
                                                VariationReason = "Initial allocation.",
                                                GroupingReason = "Information"
                                            }
                                        })
                                    }
                                }
                            },
                        }
                    }
                },
            };

            var childHistoryViewDataResponse = new ChildHistoryViewDataResponseModel()
            {
                HasUserHaveRightAccess = true,
                IsValidUrl = true
            };

            // Act
            var actual = await childFundingViewServices.GetChildHistoryViewDataInternal(
                request, childHistoryViewDataResponse);

            // Assert
            this.mockRepository.VerifyAll();
            actual.Should().BeOfType<ChildHistoryViewDataResponseModel>().
                Which.Should().BeEquivalentTo(expectedModel);
        }

        [TestMethod]
        public async Task GetChildHistoryViewDataInternal_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var childFundingViewServices = this.CreateChildFundingViewServices();
            var request = new ChildHistoryViewDataRequestModel()
            {
                UkprnFromRoute = "12345678",
                UkprnFromLoggedInUser = "12345678",
                FundingStreamNamePathPart = "general-annual-grant",
                FundingStreamCode = "GAG",
                FundingPeriodCode = "AC-2425",
                DigitalGoLiveDate = new DateTime(2024, 01, 01),
                FundingStreamConfig = new FundingStream
                {
                    HistoryIndependentOfPublications = true
                }
            };

            var periodcode = new List<string>
            {
                "GAG-AC-2425", "GAG-AC-2526", "GAG-AC-2627"
            };

            mockChildApiClientServices.SetupLatestFundingPeriod(new List<string>()
            {
                "GAG-AC-2526"
            });

            mockChildApiClientServices.SetupSearchChild(new List<LoggedInChildModel>()
            {
                new ()
                {
                    OrganisationName = "Dummy Child name",
                    FundingStreamCode = "GAG",
                    FundingPeriodCode = "AC-2425",
                    SchemaVersion = "1.2",
                    TemplateVersion = "2.0",
                    Id = "GAG-AC-2425-12345678-1_0",
                    StatusChangedDate = new DateTime(2024, 07, 28),
                    StatusChangedDateOnly = new DateTime(2024, 07, 28),
                    IsLatest = true,
                    ProviderUrn = "123456",
                    YearFrom = 2024,
                    YearTo = 2025,
                    InYearOpener = false
                }
            });

            var result = new PDS.ViewYourFunding.Services.DTOs.FundingViewData
            {
                FundingStreamCode = "GAG",
                TotalAmount = 2906249.75M,
                Components = new List<Component>
                {
                    new Component(null)
                    {
                        Type = ComponentType.Accordion_Panel,
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
                                    new KeyValuePair<(int, int), List<ProviderFundingViewModel>>((2024, 2025), new List<ProviderFundingViewModel>
                                    {
                                        new ProviderFundingViewModel
                                        {
                                            StatusChangedDate = new DateTime(2024, 07, 28),
                                            FundingPeriodCode = "AC-2425",
                                            IsLatest = true,
                                            IsFinal = false,
                                            VariationReason = "Initial allocation.",
                                            GroupingReason = "Information"
                                        }
                                    })
                                }
                            }
                        },
                    }
                }
            };

            mockSharedFundingViewServices.Setup(s => s.GetFundingViewData(It.IsAny<ChildHistoryViewDataRequestModel>())).ReturnsAsync(result);

            var expectedModel = new ChildHistoryViewDataResponseModel()
            {
                HasFundingDataExists = true,
                HasUserHaveRightAccess = true,
                IsValidUrl = true,
                OrganizationName = "Dummy Child name",
                OrganizationUrn = "123456",
                FundingViewData = new FundingViewData
                {
                    FundingStreamCode = "GAG",
                    PublicationDate = DateTime.MinValue,
                    TotalAmount = 2906249.75M,
                    Components = new List<Component>
                    {
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Panel,
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
                                        new KeyValuePair<(int, int), List<ProviderFundingViewModel>>((2024, 2025), new List<ProviderFundingViewModel>
                                        {
                                            new ProviderFundingViewModel
                                            {
                                                StatusChangedDate = new DateTime(2024, 07, 28),
                                                FundingPeriodCode = "AC-2425",
                                                IsLatest = true,
                                                IsFinal = false,
                                                VariationReason = "Initial allocation.",
                                                GroupingReason = "Information"
                                            }
                                        })
                                    }
                                }
                            },
                        }
                    }
                },
            };

            var childHistoryViewDataResponse = new ChildHistoryViewDataResponseModel()
            {
                HasUserHaveRightAccess = true,
                IsValidUrl = true
            };

            // Act
            var actual = await childFundingViewServices.GetChildHistoryViewDataInternal(
                request, childHistoryViewDataResponse);

            // Assert
            this.mockRepository.VerifyAll();
            actual.Should().BeOfType<ChildHistoryViewDataResponseModel>()
                .Which.Should().BeEquivalentTo(expectedModel);
        }

        [TestMethod]
        public async Task GetChildHistoryViewData_StateUnderTestFromCache_ExpectedBehavior()
        {
            // Arrange
            var childFundingViewServices = this.CreateChildFundingViewServices();
            var request = new ChildHistoryViewDataRequestModel()
            {
                UkprnFromRoute = "12345678",
                UkprnFromLoggedInUser = "12345678",
                FundingStreamNamePathPart = "general-annual-grant",
                FundingStreamCode = "GAG",
                FundingPeriodCode = "AC-2425",
                FundingStreamConfig = new FundingStream
                {
                    HistoryIndependentOfPublications = true
                }
            };

            var fundingStreamCodeAndName = new Dictionary<string, string>();
            fundingStreamCodeAndName.Add("GAG", "General annual grant");

            var fundingStream = new FundingStream
            {
                HistoryIndependentOfPublications = true,
                Publications = new List<Publication>
                {
                    new Publication
                    {
                        PublishedDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2425",
                        IsLatest = true,
                        Status = ViewYourFunding.Services.Enums.PublicationStatus.Published
                    }
                }
            };

            mockFundingStreamSettingsServices.Setup(s => s.GetFundingStreamCodeAndName(It.IsAny<bool>())).ReturnsAsync(fundingStreamCodeAndName);

            mockFundingStreamSettingsServices.Setup(s => s.GetFundingStream("GAG")).ReturnsAsync(fundingStream);

            var periodcode = new List<string>
            {
                "GAG-AC-2425", "GAG-AC-2526", "GAG-AC-2627"
            };

            mockFundingStreamSettingsServices.Setup(s => s.GetEmailEnabledFundingStreamPeriod()).ReturnsAsync(periodcode);

            mockChildApiClientServices.Setup(s => s.LatestFundingPeriod(It.IsAny<ChildSearchApiRequestModel>())).ReturnsAsync(new List<string> { "GAG-AC-2425" });

            mockChildApiClientServices.SetupSearchChild(new List<LoggedInChildModel>()
            {
                new ()
                {
                    OrganisationName = "Dummy Child name",
                    FundingStreamCode = "GAG",
                    FundingPeriodCode = "AC-2425",
                    SchemaVersion = "1.2",
                    TemplateVersion = "2.0",
                    Id = "GAG-AC-2425-12345678-1_0",
                    StatusChangedDate = new DateTime(2024, 07, 28),
                    StatusChangedDateOnly = new DateTime(2024, 07, 28),
                    IsLatest = true,
                    ProviderUrn = "123456",
                    YearFrom = 2024,
                    YearTo = 2025
                }
            });

            var expectedModel = new ChildHistoryViewDataResponseModel()
            {
                HasFundingDataExists = true,
                HasUserHaveRightAccess = true,
                IsValidUrl = true,
                OrganizationName = "Dummy Child name",
                OrganizationUrn = "123456",
                FundingViewData = new FundingViewData
                {
                    FundingStreamCode = "GAG",
                    PublicationDate = DateTime.MinValue,
                    TotalAmount = 2906249.75M,
                    Components = new List<Component>
                    {
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Panel,
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
                                        new KeyValuePair<(int, int), List<ProviderFundingViewModel>>((2024, 2025), new List<ProviderFundingViewModel>
                                        {
                                            new ProviderFundingViewModel
                                            {
                                                StatusChangedDate = new DateTime(2024, 07, 28),
                                                FundingPeriodCode = "AC-2425",
                                                IsLatest = true,
                                                IsFinal = false,
                                                VariationReason = "Initial allocation.",
                                                GroupingReason = "Information"
                                            }
                                        })
                                    }
                                }
                            },
                        }
                    }
                },
            };

            mockCacheService.SetupAddOrGetExistingResultAsync($"Child-HistoryPage-ViewData-GAG-AC-2425-12345678-12345678-False", ViewYourFunding.Services.Cache.CacheExpirationPolicy.Sliding, expectedModel);

            // Act
            var actual = await childFundingViewServices.GetChildHistoryViewData(request);

            // Assert
            this.mockRepository.VerifyAll();
            actual.Should().BeOfType<ChildHistoryViewDataResponseModel>().
                Which.Should().BeEquivalentTo(expectedModel);
        }

        [TestMethod]
        public async Task GetChildHistoryViewData_StateUnderTestMultipleFromCache_ExpectedBehavior()
        {
            // Arrange
            var childFundingViewServices = this.CreateChildFundingViewServices();
            var request = new ChildHistoryViewDataRequestModel()
            {
                UkprnFromRoute = "12345678",
                UkprnFromLoggedInUser = "12345678",
                FundingStreamNamePathPart = "general-annual-grant",
                FundingStreamCode = "GAG",
                FundingPeriodCode = "AC-2425",
                FundingStreamConfig = new FundingStream
                {
                    HistoryIndependentOfPublications = true
                }
            };

            var fundingStreamCodeAndName = new Dictionary<string, string>();
            fundingStreamCodeAndName.Add("GAG", "General annual grant");

            var fundingStream = new FundingStream
            {
                HistoryIndependentOfPublications = true,
                Publications = new List<Publication>
                {
                    new Publication
                    {
                        PublishedDate = new DateTime(2030, 1, 1),
                        FundingPeriodCode = "AC-2425",
                        IsLatest = true,
                        Status = ViewYourFunding.Services.Enums.PublicationStatus.Published
                    }
                }
            };

            mockFundingStreamSettingsServices.Setup(s => s.GetFundingStreamCodeAndName(It.IsAny<bool>())).ReturnsAsync(fundingStreamCodeAndName);

            mockFundingStreamSettingsServices.Setup(s => s.GetFundingStream("GAG")).ReturnsAsync(fundingStream);

            var periodcode = new List<string>
            {
                "GAG-AC-2425", "GAG-AC-2526", "GAG-AC-2627"
            };

            mockFundingStreamSettingsServices.Setup(s => s.GetEmailEnabledFundingStreamPeriod()).ReturnsAsync(periodcode);

            mockChildApiClientServices.Setup(s => s.LatestFundingPeriod(It.IsAny<ChildSearchApiRequestModel>())).ReturnsAsync(new List<string> { "GAG-AC-2425" });

            mockChildApiClientServices.SetupSearchChild(new List<LoggedInChildModel>()
            {
                new ()
                {
                    OrganisationName = "Dummy Child name",
                    FundingStreamCode = "GAG",
                    FundingPeriodCode = "AC-2425",
                    SchemaVersion = "1.2",
                    TemplateVersion = "2.0",
                    Id = "GAG-AC-2425-12345678-2_0",
                    StatusChangedDate = new DateTime(2024, 08, 28),
                    StatusChangedDateOnly = new DateTime(2024, 08, 28),
                    IsLatest = true,
                    ProviderUrn = "123456",
                    YearFrom = 2024,
                    YearTo = 2025
                },
                new ()
                {
                    OrganisationName = "Dummy Child name",
                    FundingStreamCode = "GAG",
                    FundingPeriodCode = "AC-2425",
                    SchemaVersion = "1.2",
                    TemplateVersion = "2.0",
                    Id = "GAG-AC-2425-12345678-1_0",
                    StatusChangedDate = new DateTime(2024, 07, 28),
                    StatusChangedDateOnly = new DateTime(2024, 07, 28),
                    IsLatest = true,
                    ProviderUrn = "123456",
                    YearFrom = 2024,
                    YearTo = 2025
                }
            });

            var expectedModel = new ChildHistoryViewDataResponseModel()
            {
                HasFundingDataExists = true,
                HasUserHaveRightAccess = true,
                IsValidUrl = true,
                OrganizationName = "Dummy Child name",
                OrganizationUrn = "123456",
                FundingViewData = new FundingViewData
                {
                    FundingStreamCode = "GAG",
                    PublicationDate = DateTime.MinValue,
                    TotalAmount = 2906249.75M,
                    Components = new List<Component>
                    {
                        new Component(null)
                        {
                            Type = ComponentType.Accordion_Panel,
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
                                        new KeyValuePair<(int, int), List<ProviderFundingViewModel>>((2024, 2025), new List<ProviderFundingViewModel>
                                        {
                                            new ProviderFundingViewModel
                                            {
                                                StatusChangedDate = new DateTime(2024, 08, 28),
                                                FundingPeriodCode = "AC-2425",
                                                IsLatest = true,
                                                IsFinal = false,
                                                VariationReason = "Revised allocation.",
                                                GroupingReason = "Payment"
                                            },
                                            new ProviderFundingViewModel
                                            {
                                                StatusChangedDate = new DateTime(2024, 07, 28),
                                                FundingPeriodCode = "AC-2425",
                                                IsLatest = false,
                                                IsFinal = false,
                                                VariationReason = "Initial allocation.",
                                                GroupingReason = "Information"
                                            }
                                        })
                                    }
                                }
                            },
                        }
                    }
                },
            };

            mockCacheService.SetupAddOrGetExistingResultAsync($"Child-HistoryPage-ViewData-GAG-AC-2425-12345678-12345678-False", ViewYourFunding.Services.Cache.CacheExpirationPolicy.Sliding, expectedModel);

            // Act
            var actual = await childFundingViewServices.GetChildHistoryViewData(request);

            // Assert
            this.mockRepository.VerifyAll();
            actual.Should().BeOfType<ChildHistoryViewDataResponseModel>().
                Which.Should().BeEquivalentTo(expectedModel);
        }

        [TestMethod]
        public async Task GetCurrentAndHistoricFundingStreamPeriodsForGAG_ReturnsMultipleFundingPeriods()
        {
            // Arrange
            var childFundingViewServices = this.CreateChildFundingViewServices();

            var request = new ChildSearchApiRequestModel()
            {
                FundingStreamPeriods = new List<string>()
                {
                    "GAG-AC-2627",
                }
            };

            var fundingStream = new FundingStream
            {
                Publications = new List<Publication>
                    {
                        new Publication
                        {
                            PublishedDate = new DateTime(2030, 1, 1),
                            FundingPeriodCode = "AC-2627",
                            FundingStreamId = 1,
                            IsLatest = true,
                            Status = ViewYourFunding.Services.Enums.PublicationStatus.Published
                        }
                    },
                SettingValues = new List<ViewYourFunding.Services.Models.SettingValue>
                    {
                        new ViewYourFunding.Services.Models.SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "DigitalIYOStatementsGoLiveDate"
                            },
                            Value = "01/01/2025"
                        }
                    }
            };

            // Expected
            var expectedFundingStreamAndPeriods = new List<string>()
            {
                { "GAG-AC-2526" },
            };

            var periodcode = new List<string>
            {
                "GAG-AC-2425", "GAG-AC-2526", "GAG-AC-2627"
            };

            mockFundingStreamSettingsServices.Setup(s => s.GetEmailEnabledFundingStreamPeriod()).ReturnsAsync(periodcode);

            mockChildApiClientServices.SetupLatestFundingPeriod(new List<string>()
            {
                "GAG-AC-2526"
            });

            mockFundingStreamSettingsServices.Setup(s => s.GetFundingStream(It.IsAny<string>())).ReturnsAsync(fundingStream);

            // Act
            var actual = await childFundingViewServices.GetCurrentAndHistoricFundingStreamPeriods(request);

            // Assert
            this.mockRepository.VerifyAll();

            actual.Should().BeEquivalentTo(expectedFundingStreamAndPeriods);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("1619-AS-2526", "1619-AS-2526")]
        public async Task GetCurrentAndHistoricFundingStreamPeriodsFor1619_ReturnsSingleFundingPeriods(string fundingStreamPeriod, string expectedFundingStreamPeriod)
        {
            // Arrange
            var childFundingViewServices = this.CreateChildFundingViewServices();

            var request = new ChildSearchApiRequestModel()
            {
                FundingStreamPeriods = new List<string>()
                {
                    fundingStreamPeriod,
                }
            };

            var fundingStream = new FundingStream
            {
                Publications = new List<Publication>
                    {
                        new Publication
                        {
                            PublishedDate = new DateTime(2030, 1, 1),
                            FundingPeriodCode = "AS-2526",
                            FundingStreamId = 1,
                            IsLatest = true,
                            Status = ViewYourFunding.Services.Enums.PublicationStatus.Published
                        }
                    },
                SettingValues = new List<ViewYourFunding.Services.Models.SettingValue>
                    {
                        new ViewYourFunding.Services.Models.SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = string.Empty
                            },
                            Value = null
                        }
                    }
            };

            // Expected
            var expectedFundingStreamAndPeriods = new List<string>()
            {
                expectedFundingStreamPeriod,
            };

            var periodcode = new List<string>
            {
                "1619-AS-2425", "1619-AS-2526", "1619-AS-2627"
            };

            mockFundingStreamSettingsServices.Setup(s => s.GetEmailEnabledFundingStreamPeriod()).ReturnsAsync(periodcode);

            mockChildApiClientServices.SetupLatestFundingPeriod(new List<string>()
            {
                "1619-AS-2526"
            });

            mockFundingStreamSettingsServices.Setup(s => s.GetFundingStream(It.IsAny<string>())).ReturnsAsync(fundingStream);

            // Act
            var actual = await childFundingViewServices.GetCurrentAndHistoricFundingStreamPeriods(request);

            // Assert
            this.mockRepository.VerifyAll();

            actual.Should().BeEquivalentTo(expectedFundingStreamAndPeriods);
        }

        private ChildFundingViewServices CreateChildFundingViewServices()
        {
            return new ChildFundingViewServices(
                this.mockSharedFundingViewServices.Object,
                this.mockLoggerAdapter.Object,
                this.mockCacheService.Object,
                this.mockChildApiClientServices.Object,
                this.mockParentApiClientServices.Object,
                this.mockFundingStreamSettingsServices.Object,
                this.mockUserCountApiClientServices.Object);
        }
    }
}
