using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Attributes;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GlobalSettingModels = PDS.ViewYourFunding.Web.Models.GlobalSetting;
using SettingEditType = PDS.ViewYourFunding.Web.Enums.SettingEditType;
using User = Pds.Core.Common.Identity.Models.User;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class GlobalSettingsControllerTests
    {
        private readonly IMapper _mapper;

        public GlobalSettingsControllerTests()
        {
            _mapper = GetMapper();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetExternalViewYourFundingSetting_SettingExists_ReturnsOkResultAndValueOfTrue()
        {
            // Arrange
            const int typeId = 6;
            const string settingValue = "TRUE";

            var mockGlobalSettingService = new Mock<IGlobalSettingService>();
            var mockMapper = new Mock<IMapper>();

            var serviceGlobalSetting = GetServiceGlobalSetting(typeId, settingValue);
            var modelGlobalSetting = GetWebGlobalSetting(typeId, settingValue);

            mockMapper.Setup(x => x.Map<GlobalSettingModels.GlobalSetting>(serviceGlobalSetting)).Returns(modelGlobalSetting);

            mockGlobalSettingService.Setup(x => x.GetFirstOrDefault(typeId))
                .ReturnsAsync(serviceGlobalSetting);

            var controller = new GlobalSettingsController(mockGlobalSettingService.Object, mockMapper.Object, null, null, null, null, new MemoryCacheService(null, 0), null, null, null);

            // Act
            var response = await controller.GetExternalViewYourFundingSetting();

            // Assert
            response.Should().Be(settingValue);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetExternalViewYourFundingSetting_SettingDoesNotExists_ReturnsNotFoundObjectResultAndNotFoundMessage()
        {
            // Arrange
            var mockGlobalSettingService = new Mock<IGlobalSettingService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new GlobalSettingsController(mockGlobalSettingService.Object, mockMapper.Object, null, null, null, null, new MemoryCacheService(null, 0), null, null, null);

            // Act
            var response = await controller.GetExternalViewYourFundingSetting();

            // Assert
            response.Should().Be("Setting type 6 not found.");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetExternalViewUrlSetting_SettingExists_ReturnsOkResultAndValueOfFalse()
        {
            // Arrange
            const int typeId = 5;
            const string settingValue = "FALSE";

            var mockGlobalSettingService = new Mock<IGlobalSettingService>();
            var mockMapper = new Mock<IMapper>();

            var serviceGlobalSetting = GetServiceGlobalSetting(typeId, settingValue);
            var modelGlobalSetting = GetWebGlobalSetting(typeId, settingValue);

            mockMapper.Setup(x => x.Map<GlobalSettingModels.GlobalSetting>(serviceGlobalSetting)).Returns(modelGlobalSetting);

            mockGlobalSettingService.Setup(x => x.GetFirstOrDefault(typeId))
                .ReturnsAsync(serviceGlobalSetting);

            var controller = new GlobalSettingsController(mockGlobalSettingService.Object, mockMapper.Object, null, null, null, null, new MemoryCacheService(null, 0), null, null, null);

            // Act
            var response = await controller.GetExternalViewUrlSetting();

            // Assert
            response.Should().Be(settingValue);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetExternalViewUrlSetting_SettingDoesNotExists_ReturnsNotFoundObjectResultAndNotFoundMessage()
        {
            // Arrange
            var mockGlobalSettingService = new Mock<IGlobalSettingService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new GlobalSettingsController(mockGlobalSettingService.Object, mockMapper.Object, null, null, null, null, new MemoryCacheService(null, 0), null, null, null);

            // Act
            var response = await controller.GetExternalViewUrlSetting();

            // Assert
            response?.Should().Be(null);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetLoggedInViewYourFundingSetting_SettingExists_ReturnsOkResultAndValueOfTrue()
        {
            // Arrange
            const int typeId = 2;
            const string settingValue = "TRUE";

            var mockGlobalSettingService = new Mock<IGlobalSettingService>();
            var mockMapper = new Mock<IMapper>();

            var serviceGlobalSetting = GetServiceGlobalSetting(typeId, settingValue);
            var modelGlobalSetting = GetWebGlobalSetting(typeId, settingValue);

            mockMapper.Setup(x => x.Map<GlobalSettingModels.GlobalSetting>(serviceGlobalSetting)).Returns(modelGlobalSetting);

            mockGlobalSettingService.Setup(x => x.GetFirstOrDefault(typeId))
                .ReturnsAsync(serviceGlobalSetting);

            var controller = new GlobalSettingsController(mockGlobalSettingService.Object, mockMapper.Object, null, null, null, null, new MemoryCacheService(null, 0), null, null, null);

            // Act
            var response = await controller.GetLoggedInAdminViewYourFundingSetting();

            // Assert
            response.Should().Be(settingValue);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetLoggedInViewYourFundingSetting_SettingDoesNotExists_ReturnsNotFoundObjectResultAndNotFoundMessage()
        {
            // Arrange
            var mockGlobalSettingService = new Mock<IGlobalSettingService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new GlobalSettingsController(mockGlobalSettingService.Object, mockMapper.Object, null, null, null, null, new MemoryCacheService(null, 0), null, null, null);

            // Act
            var response = await controller.GetLoggedInAdminViewYourFundingSetting();

            // Assert
            response.Should().Be("Setting type 2 not found.");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetLoggedInViewUrlSetting_SettingExists_ReturnsOkResultAndUrlValue()
        {
            // Arrange
            const int typeId = 1;
            const string settingValue = "LoggedInViewUrl";

            var mockGlobalSettingService = new Mock<IGlobalSettingService>();
            var mockMapper = new Mock<IMapper>();

            var serviceGlobalSetting = GetServiceGlobalSetting(typeId, settingValue);
            var modelGlobalSetting = GetWebGlobalSetting(typeId, settingValue);

            mockMapper.Setup(x => x.Map<GlobalSettingModels.GlobalSetting>(serviceGlobalSetting)).Returns(modelGlobalSetting);

            mockGlobalSettingService.Setup(x => x.GetFirstOrDefault(typeId))
                .ReturnsAsync(serviceGlobalSetting);

            var controller = new GlobalSettingsController(mockGlobalSettingService.Object, mockMapper.Object, null, null, null, null, new MemoryCacheService(null, 0), null, null, null);

            // Act
            var response = await controller.GetLoggedInAdminViewUrlSetting();

            // Assert
            response.Should().BeOfType(typeof(string));
            response.Should().Be(settingValue);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetLoggedInViewUrlSetting_SettingDoesNotExists_ReturnsNotFoundObjectResultAndNotFoundMessage()
        {
            // Arrange
            var mockGlobalSettingService = new Mock<IGlobalSettingService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new GlobalSettingsController(mockGlobalSettingService.Object, mockMapper.Object, null, null, null, null, new MemoryCacheService(null, 0), null, null, null);

            // Act
            var response = await controller.GetLoggedInAdminViewUrlSetting();

            // Assert
            response.Should().Be(null);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetLoggedInProviderViewYourFundingSetting_SettingExists_ReturnsOkResultAndValueOfTrue()
        {
            // Arrange
            const int typeId = 4;
            const string settingValue = "TRUE";

            var mockGlobalSettingService = new Mock<IGlobalSettingService>();
            var mockMapper = new Mock<IMapper>();

            var serviceGlobalSetting = GetServiceGlobalSetting(typeId, settingValue);
            var modelGlobalSetting = GetWebGlobalSetting(typeId, settingValue);

            mockMapper.Setup(x => x.Map<GlobalSettingModels.GlobalSetting>(serviceGlobalSetting)).Returns(modelGlobalSetting);

            mockGlobalSettingService.Setup(x => x.GetFirstOrDefault(typeId))
                .ReturnsAsync(serviceGlobalSetting);

            var controller = new GlobalSettingsController(mockGlobalSettingService.Object, mockMapper.Object, null, null, null, null, new MemoryCacheService(null, 0), null, null, null);

            // Act
            var response = await controller.GetLoggedInProviderViewYourFundingSetting();

            // Assert
            response.Should().Be(settingValue);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetLoggedInProviderViewYourFundingSetting_SettingDoesNotExists_ReturnsNotFoundObjectResultAndNotFoundMessage()
        {
            // Arrange
            var mockGlobalSettingService = new Mock<IGlobalSettingService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new GlobalSettingsController(mockGlobalSettingService.Object, mockMapper.Object, null, null, null, null, new MemoryCacheService(null, 0), null, null, null);

            // Act
            var response = await controller.GetLoggedInProviderViewYourFundingSetting();

            // Assert
            response.Should().Be("Setting type 4 not found.");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetLoggedInProviderViewUrlSetting_SettingExists_ReturnsOkResultAndUrlValue()
        {
            // Arrange
            const int typeId = 3;
            const string settingValue = "LoggedInViewUrl";

            var mockGlobalSettingService = new Mock<IGlobalSettingService>();
            var mockMapper = new Mock<IMapper>();

            var serviceGlobalSetting = GetServiceGlobalSetting(typeId, settingValue);
            var modelGlobalSetting = GetWebGlobalSetting(typeId, settingValue);

            mockMapper.Setup(x => x.Map<GlobalSettingModels.GlobalSetting>(serviceGlobalSetting)).Returns(modelGlobalSetting);

            mockGlobalSettingService.Setup(x => x.GetFirstOrDefault(typeId))
                .ReturnsAsync(serviceGlobalSetting);

            var controller = new GlobalSettingsController(mockGlobalSettingService.Object, mockMapper.Object, null, null, null, null, new MemoryCacheService(null, 0), null, null, null);

            // Act
            var response = await controller.GetLoggedInProviderViewUrlSetting();

            // Assert
            response.Should().BeOfType(typeof(string));
            response.Should().Be(settingValue);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetLoggedInProviderViewUrlSetting_SettingDoesNotExists_ReturnsNotFoundObjectResultAndNotFoundMessage()
        {
            // Arrange
            var mockGlobalSettingService = new Mock<IGlobalSettingService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new GlobalSettingsController(mockGlobalSettingService.Object, mockMapper.Object, null, null, null, null, new MemoryCacheService(null, 0), null, null, null);

            // Act
            var response = await controller.GetLoggedInProviderViewUrlSetting();

            // Assert
            response.Should().Be(null);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(1234, true)]
        [DataRow(5678, false)]
        public async Task IsMultipleAcademyTrust_ExpectedResult(int ukPrn, bool expectedResult)
        {
            // Arrange
            var controller = new GlobalSettingsController(
                GetGlobalSettingService_ForMATToggleValue().Object,
                _mapper,
                GetIdentityService().Object,
                GetUserJourneyService().Object,
                null,
                GetFundingApiService_MATResult(expectedResult).Object,
                new MemoryCacheService(null, 0),
                GetFundingViewService().Object,
                null,
                null);

            // Act
            var response = await controller.IsMultipleAcademyTrust(ukPrn);

            // Assert
            response.Should().Be(expectedResult);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(5678, false)]
        public async Task IsMultipleAcademyTrust_MAT_ToggledOff_ExpectedResult(int ukPrn, bool expectedResult)
        {
            // Arrange
            var controller = new GlobalSettingsController(
                GetGlobalSettingService_ForMATToggleValue("false").Object,
                _mapper,
                GetIdentityService().Object,
                GetUserJourneyService().Object,
                null,
                GetFundingApiService_MATResult(expectedResult).Object,
                new MemoryCacheService(null, 0),
                GetFundingViewService().Object,
                null,
                null);

            // Act
            var response = await controller.IsMultipleAcademyTrust(ukPrn);

            // Assert
            response.Should().Be(expectedResult);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(0, false)]
        public async Task IsMultipleAcademyTrust_InvalidUkprn_ExpectedResult(int ukPrn, bool expectedResult)
        {
            // Arrange
            var controller = new GlobalSettingsController(
                null,
                _mapper,
                GetIdentityService().Object,
                GetUserJourneyService().Object,
                null,
                GetFundingApiService_MATResult(expectedResult).Object,
                new MemoryCacheService(null, 0),
                GetFundingViewService().Object,
                null,
                null);

            // Act
            var response = await controller.IsMultipleAcademyTrust(ukPrn);

            // Assert
            response.Should().Be(expectedResult);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(92929, false)]
        public async Task IsMultipleAcademyTrust_ThrowsException_ExpectedResult(int ukPrn, bool expectedResult)
        {
            // Arrange
            var controller = new GlobalSettingsController(
                GetGlobalSettingService_ForMATToggleValue().Object,
                _mapper,
                GetIdentityService().Object,
                GetUserJourneyService().Object,
                null,
                GetFundingApiService__FundingSearch_ThrowsException().Object,
                new MemoryCacheService(null, 0),
                GetFundingViewService().Object,
                GetLogger().Object,
                null);

            // Act
            var response = await controller.IsMultipleAcademyTrust(ukPrn);

            // Assert
            response.Should().Be(expectedResult);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetInfoForLoggedInMultipleAcademyTrust_VYFToggleOn_ExpectedResult()
        {
            const int typeId = GlobalSettingTypeConstants.DisplayLoggedInMultipleAcademyTrustViewTypeId;
            const string settingValue = "TRUE";

            var mockGlobalSettingService = new Mock<IGlobalSettingService>();
            var mockMapper = new Mock<IMapper>();

            var serviceGlobalSetting = GetServiceGlobalSetting(typeId, settingValue);
            var modelGlobalSetting = GetWebGlobalSetting(typeId, settingValue);

            mockMapper.Setup(x => x.Map<GlobalSettingModels.GlobalSetting>(serviceGlobalSetting)).Returns(modelGlobalSetting);

            mockGlobalSettingService.Setup(x => x.GetFirstOrDefault(typeId))
                .ReturnsAsync(serviceGlobalSetting);

            // Arrange
            var controller = new GlobalSettingsController(
                mockGlobalSettingService.Object,
                mockMapper.Object,
                GetIdentityService().Object,
                GetUserJourneyService().Object,
                null,
                GetFundingApiService_MATResult(true).Object,
                new MemoryCacheService(null, 0),
                GetFundingViewService().Object,
                null,
                GetHttpApiService().Object);

            var expectedViewModel = new GlobalSettingModels.InfoForLoggedInProvider
            {
                FundingsExists = true,
                ToggledOn = true,
                NewFundingsNotRead = 10,
                UpdatedFundingsNotRead = 20,
                Path = null
            };

            // Act
            var actual = await controller.GetInfoForLoggedInMultipleAcademyTrust("10060391", "user1");

            // Assert
            actual
                .Should().BeOfType<ActionResult<GlobalSettingModels.InfoForLoggedInProvider>>()
                .Which.Value.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetInfoForLoggedInMultipleAcademyTrust_VYFToggleOff_ExpectedResult()
        {
            const int typeId = GlobalSettingTypeConstants.DisplayLoggedInMultipleAcademyTrustViewTypeId;
            const string settingValue = "FALSE";

            var mockGlobalSettingService = new Mock<IGlobalSettingService>();
            var mockMapper = new Mock<IMapper>();

            var serviceGlobalSetting = GetServiceGlobalSetting(typeId, settingValue);
            var modelGlobalSetting = GetWebGlobalSetting(typeId, settingValue);

            mockMapper.Setup(x => x.Map<GlobalSettingModels.GlobalSetting>(serviceGlobalSetting)).Returns(modelGlobalSetting);

            mockGlobalSettingService.Setup(x => x.GetFirstOrDefault(typeId))
                .ReturnsAsync(serviceGlobalSetting);

            // Arrange
            var controller = new GlobalSettingsController(
                mockGlobalSettingService.Object,
                mockMapper.Object,
                GetIdentityService().Object,
                GetUserJourneyService().Object,
                null,
                GetFundingApiService_MATResult(true).Object,
                new MemoryCacheService(null, 0),
                GetFundingViewService().Object,
                null,
                GetHttpApiService().Object);

            var expectedViewModel = new GlobalSettingModels.InfoForLoggedInProvider
            {
                ToggledOn = false
            };

            // Act
            var actual = await controller.GetInfoForLoggedInMultipleAcademyTrust("10060391", "user1");

            // Assert
            actual
                .Should().BeOfType<ActionResult<GlobalSettingModels.InfoForLoggedInProvider>>()
                .Which.Value.Should().BeEquivalentTo(expectedViewModel);
        }

        /// <summary>
        /// Return service domain global setting object.
        /// </summary>
        /// <param name="typeId">Type Id to assign to global setting object.</param>
        /// <param name="settingValue">Setting value to assign to global setting object.</param>
        /// <returns>Service domain global setting object.</returns>
        private static GlobalSetting GetServiceGlobalSetting(int typeId, string settingValue)
        {
            return new GlobalSetting()
            {
                Id = 6,
                Type = typeId,
                Description = "Is view your funding public view available?",
                EditType = GlobalSetting.SettingEditType.Bool,
                ReadOnly = false,
                Value = settingValue,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        private static IMapper GetMapper()
        {
            var config = new TypeAdapterConfig();
            config.ConfigureWebMappings();
            return new Mapper(config);
        }

        /// <summary>
        /// Return Web domain global setting object.
        /// </summary>
        /// <param name="typeId">Type Id to assign to global setting object.</param>
        /// <param name="settingValue">Setting value to assign to global setting object.</param>
        /// <returns>Web domain global setting object.</returns>
        private static GlobalSettingModels.GlobalSetting GetWebGlobalSetting(int typeId, string settingValue)
        {
            return new GlobalSettingModels.GlobalSetting()
            {
                Id = 1,
                Type = typeId,
                Description = "Is View Your Funding External area available",
                EditType = SettingEditType.Bool,
                ReadOnly = false,
                Value = settingValue,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        private Mock<IFundingApiService> GetFundingApiService_MATResult(bool validMat = true)
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
                                "PSG-AY-1920-10072822-2_0", "PSG-AY-1920-10072811-2_0"
                            }
                        }
                    }
                });

            fundingApiService
                .Setup(fas => fas.GetUserFundingViewCount(It.IsAny<string>(), It.IsAny<List<FundingVersionDetail>>()))
                .ReturnsAsync(new UserFundingViewCountResponse
                {
                    UnreadNewFundings = 10,
                    UnreadUpdatedFundings = 20
                });

            return fundingApiService;
        }

        private Mock<IFundingApiService> GetFundingApiService__FundingSearch_ThrowsException()
        {
            var fundingApiService = new Mock<IFundingApiService>(MockBehavior.Strict);

            fundingApiService
                .Setup(fas => fas.SearchFunding(It.IsAny<FundingApiSearchRequestObject>()))
                .Throws(new Exception());

            return fundingApiService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService()
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

        private Mock<IGlobalSettingService> GetGlobalSettingService_ForMATToggleValue(string valueReturned = "true")
        {
            var globalSettingsMock = new Mock<IGlobalSettingService>(MockBehavior.Strict);
            globalSettingsMock
                .Setup(gs => gs.GetFirstOrDefault(GlobalSettingTypeConstants.DisplayLoggedInMultipleAcademyTrustViewTypeId))
                .ReturnsAsync(new GlobalSetting { Value = valueReturned });

            return globalSettingsMock;
        }

        private Mock<ILoggerAdapter<GlobalSettingsController>> GetLogger()
        {
            var loggerService = new Mock<ILoggerAdapter<GlobalSettingsController>>();

            loggerService.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()));

            return loggerService;
        }

        private FundingStream GetGagFundingStream()
        {
            return new FundingStream
            {
                FundingStreamCode = "GAG",
                FundingStreamName = "General annual grant",
                FundingStreamNameWithinSentence = "General annual grant",
                RelevantForProviders_LoggedIn = true,
                RelevantForOrganisations_LoggedIn = true,
                RelevantForOrganisations_Public = true,
                Active = true,
                SettingValues = new List<SettingValue>
                {
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "FinancialYear"
                        },
                        Value = "202122"
                    },
                    new SettingValue
                    {
                        Setting = new SettingType
                        {
                            SettingName = "UseStaticData"
                        },
                        Value = "true"
                    }
                },
                Publications = new List<Publication>
                {
                    new Publication
                    {
                        FundingPeriodCode = "FY-2122",
                        PublishedDate = new DateTime(2030, 1, 1),
                        Status = PublicationStatus.Published
                    }
                },
                NextPayments = new List<NextPayment>()
            };
        }

        private Mock<IClaimsBasedIdentityService> GetIdentityService()
        {
            var identityService = new Mock<IClaimsBasedIdentityService>(MockBehavior.Strict);
            identityService
                .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User { Ukprn = 10053512 });

            return identityService;
        }

        private Mock<IFundingViewService> GetFundingViewService()
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

        private Mock<IHttpApiService> GetHttpApiService()
        {
            var httpApiService = new Mock<IHttpApiService>(MockBehavior.Strict);
            httpApiService
                .Setup(x => x.PostRequestToUserFundingView<UserFundingViewCountResponse>(
                    $"user/GetUserFundingViewCount",
                    It.IsAny<string>(),
                    "application/json"))
                .ReturnsAsync(new UserFundingViewCountResponse
                {
                    UnreadNewFundings = 10,
                    UnreadUpdatedFundings = 20
                });
            return httpApiService;
        }
    }
}