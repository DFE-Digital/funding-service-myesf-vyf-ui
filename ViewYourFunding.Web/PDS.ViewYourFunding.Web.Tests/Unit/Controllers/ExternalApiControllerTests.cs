using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Identity.Claims.Interfaces;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Attributes;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Extensions;
using PDS.ViewYourFunding.Web.Interfaces;
using PDS.ViewYourFunding.Web.Models;
using PDS.ViewYourFunding.Web.Models.Request;
using PDS.ViewYourFunding.Web.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class ExternalApiControllerTests
    {
        [TestMethod, TestCategory("Unit")]
        public async Task RenderHtml_AllMocked_RendersCorrectly()
        {
            // Arrange
            var requestModel = new RenderHtmlRequest
            {
                ProviderFundingId = "ABC-AY-1920-12345678-1_0",
                CutOffDate = new DateTime(2030, 1, 1),
                FundingPeriodCode = "AY-1920",
                FundingStreamCode = "ABC",
                LayoutId = "027e20f1-cb41-4ddb-a9db-a6f23e41a49b",
                Ukprn = "12345678"
            };

            var fundingViewData = new FundingViewData
            {
                FundingStreamCode = "ABC",
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

            var controller = new ExternalApiController(
                GetComponentService().Object,
                GetIdentityService().Object,
                GetAppConfigService().Object,
                GetUserJourneyService().Object,
                GetMapper(),
                GetFundingApiService_SingleABCProvider().Object,
                GetFundingViewService(fundingViewData).Object,
                GetRazorViewToStringRenderer(new RenderHtmlViewModel { FundingViewData = fundingViewData }).Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object);

            var expectedContent = "ABC1";

            // Act
            var actual = await controller.RenderHtml(requestModel);

            // Assert
            actual
                .Should().BeOfType<ContentResult>()
            .Which.Content.Should().Be(expectedContent);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task RenderHtml_AllMockedComplexLayoutLookup_RendersCorrectly()
        {
            // Arrange
            var requestModel = new RenderHtmlRequest
            {
                ProviderFundingId = "ABC-AY-1920-12345678-1_0",
                CutOffDate = new DateTime(2030, 1, 1),
                FundingPeriodCode = "AY-1920",
                FundingStreamCode = "ABC",
                LayoutId = "027e20f1-cb41-4ddb-a9db-a6f23e41a49b",
                Ukprn = "12345678"
            };

            var fundingViewData = new FundingViewData
            {
                FundingStreamCode = "ABC",
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

            var layoutModel = new LayoutModel
            {
                Data = new Dictionary<string, object>
                 {
                     {
                        "groups", new List<object>
                         {
                            new Dictionary<string, object>
                            {
                                { "type", "AccordionTitle" }
                            },
                            new Dictionary<string, object>
                            {
                                { "type", "AccordionPanel" }
                            }
                         }
                     }
                 }
            };

            var fundingApiService = GetFundingApiService_SingleABCProvider().Object;
            var userJourneyService = GetUserJourneyService().Object;
            var cosmosDbService = new Mock<ICosmosDbService<LayoutModel>>(MockBehavior.Strict);

            cosmosDbService
                .Setup(x => x.GetAsync("027e20f1-cb41-4ddb-a9db-a6f23e41a49b"))
                .ReturnsAsync(layoutModel);

            cosmosDbService
                .Setup(x => x.RunQueryAsync<Dictionary<string, object>>(@"SELECT *
                FROM c
                where (IS_DEFINED(c.FundingViewType) = false or c.FundingViewType = null)
                and (IS_DEFINED(c.LayoutType) = true and c.LayoutType != null and c.LayoutType != '')"))
                .ReturnsAsync(new List<Dictionary<string, object>>());

            var controller = new ExternalApiController(
                GetComponentService().Object,
                GetIdentityService().Object,
                GetAppConfigService().Object,
                userJourneyService,
                GetMapper(),
                fundingApiService,
                new ModelFundingViewService(
                    GetGlobalSettingService().Object,
                    null,
                    new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                    null,
                    null,
                    fundingApiService,
                    new Mock<IComponentFactory>(MockBehavior.Strict).Object,
                    new LayoutManagementService(cosmosDbService.Object, null),
                    new MemoryCacheService(null, 0)),
                GetRazorViewToStringRenderer(new RenderHtmlViewModel { FundingViewData = fundingViewData }).Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object);

            var expectedContent = "ABC1";

            // Act
            var actual = await controller.RenderHtml(requestModel);

            // Assert
            actual
                .Should().BeOfType<ContentResult>()
            .Which.Content.Should().Be(expectedContent);
        }

        [TestMethod, TestCategory("Unit")]
        public void RenderFile_WhenRequestIsNull_ThrowsException()
        {
            // Arrange
            var controller = new ExternalApiController(null, null, null, null, null, null, null, null, null, null);

            // Act
            Func<Task> act = async () => await controller.GetFile(null);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage($"Request object should not be null");
        }

        [TestMethod, TestCategory("Unit")]
        public void GetFile_WhenFundingStreamIsNotActive_ThrowsException()
        {
            // Arrange
            var controller = new ExternalApiController(null, null, null, GetUserJourneyService().Object, null, null, null, null, null, null);

            // Act
            Func<Task> act = async () => await controller.GetFile(new RenderFileRequest { FundingStreamCode = "DEF" });

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage($"Cannot find an active funding stream with the code 'DEF'");
        }

        [TestMethod, TestCategory("Unit")]
        public void GetFile_WhenNoFunding_ThrowsException()
        {
            // Arrange
            var requestModel = new RenderFileRequest
            {
                ProviderFundingId = "ABC-AY-1920-12345678-1_0",
                CutOffDate = new DateTime(2030, 1, 1),
                PublicationDate = new DateTime(2020, 1, 1),
                FundingPeriodCode = "AY-1920",
                FundingStreamCode = "ABC",
                LayoutId = "027e20f1-cb41-4ddb-a9db-a6f23e41a49b",
                FileName = "File Name"
            };

            var content = new byte[1024];
            var returnModel = new List<FundingDocumentMeta> { new FundingDocumentMeta { Data = content } };

            var fundingViewService = new Mock<IFundingViewService>(MockBehavior.Strict);
            fundingViewService
                .Setup(fvs => fvs.GenerateFundingDocument(
                    It.IsAny<FundingStream>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<Publication>(),
                    It.IsAny<FundingViewType>(),
                    It.IsAny<FundingViewScope>(),
                    It.IsAny<FileFormat[]>(),
                    null,
                    It.IsAny<PreviewLayoutModel>(),
                    false,
                    false,
                    true,
                    It.IsAny<IFundingApiSearchFunding[]>(),
                    It.IsAny<IFundingApiSearchProviderFunding[]>(),
                    false,
                    true))
                .ReturnsAsync(returnModel);

            var controller = new ExternalApiController(
                GetComponentService().Object,
                GetIdentityService().Object,
                GetAppConfigService().Object,
                GetUserJourneyService().Object,
                GetMapper(),
                GetFundingApiService_NotFound().Object,
                fundingViewService.Object,
                null,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object);


            // Act
            Func<Task> act = async () => await controller.GetFile(requestModel);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("Unable to find data from indices.");
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFile_AllMocked_RendersCorrectly()
        {
            // Arrange
            var requestModel = new RenderFileRequest
            {
                ProviderFundingId = "ABC-AY-1920-12345678-1_0",
                CutOffDate = new DateTime(2030, 1, 1),
                PublicationDate = new DateTime(2020, 1, 1),
                FundingPeriodCode = "AY-1920",
                FundingStreamCode = "ABC",
                LayoutId = "027e20f1-cb41-4ddb-a9db-a6f23e41a49b",
                FileName = "File Name"
            };

            var content = new byte[1024];
            var returnModel = new List<FundingDocumentMeta> { new FundingDocumentMeta { Data = content } };

            var fundingViewService = new Mock<IFundingViewService>(MockBehavior.Strict);
            fundingViewService
                .Setup(fvs => fvs.GenerateFundingDocument(
                    It.IsAny<FundingStream>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<Publication>(),
                    It.IsAny<FundingViewType>(),
                    It.IsAny<FundingViewScope>(),
                    It.IsAny<FileFormat[]>(),
                    null,
                    It.IsAny<PreviewLayoutModel>(),
                    false,
                    false,
                    true,
                    It.IsAny<IFundingApiSearchFunding[]>(),
                    It.IsAny<IFundingApiSearchProviderFunding[]>(),
                    false,
                    true))
                .ReturnsAsync(returnModel);

            var controller = new ExternalApiController(
                GetComponentService().Object,
                GetIdentityService().Object,
                GetAppConfigService().Object,
                GetUserJourneyService().Object,
                GetMapper(),
                GetFundingApiService_SingleABCProvider().Object,
                fundingViewService.Object,
                null,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object);

            // Act
            var actual = await controller.GetFile(requestModel);

            // Assert
            actual
                .Should().BeOfType<FileContentResult>()
            .Which.FileContents.Should().BeEquivalentTo(content);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(null, null, null)]
        [DataRow(null, "false", null)]
        [DataRow(null, null, "true")]
        [DataRow("false", null, "true")]
        [DataRow("false", "false", "false")]
        [DataRow("false", "true", "false")]
        [DataRow("true", "false", "true")]
        [DataRow("true", "true", "true")]
        public async Task GetAutoPullFundingStreams(string fs1AutoPullValue, string fs2AutoPullValue, string fs3AutoPullValue)
        {
            // Arrange
            var expectedAutoPullFundingStreamResults = GetAutoPullFundingStreams_GenerateExpectedResults(fs1AutoPullValue, fs2AutoPullValue, fs3AutoPullValue);

            var fundingViewData = new FundingViewData
            {
                FundingStreamCode = "ABC",
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

            var controller = new ExternalApiController(
                GetComponentService().Object,
                GetIdentityService().Object,
                GetAppConfigService().Object,
                GetUserJourneyService_AutoPull(fs1AutoPullValue, fs2AutoPullValue, fs3AutoPullValue).Object,
                GetMapper(),
                GetFundingApiService_SingleABCProvider().Object,
                GetFundingViewService(fundingViewData).Object,
                GetRazorViewToStringRenderer(new RenderHtmlViewModel { FundingViewData = fundingViewData }).Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object);

            // Act
            var actual = await controller.GetAutoPullConfiguredFundingStreams();

            // Assert
            actual
                .Should().BeOfType<List<GetAutoPullFundingStreamResult>>()
            .Which.Should().BeEquivalentTo(expectedAutoPullFundingStreamResults);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(null, null, null, null, null)]
        [DataRow(null, "FY-2324", null, null, null)]
        [DataRow("AC-2425", null, "", null, null)]
        [DataRow("AC-2425", "FY-2324", "AC-2324", null, null)]
        [DataRow("AC-2425", "FY-2324", "AC-2324", "AC-2425", null)]
        [DataRow("AC-2425", "FY-2324", "AC-2324", "AC-2425", "AC-2425")]
        [DataRow("AC-2425,AC-2526", "FY-2324,AC-2526", "AC-2324,AC-2526", "AC-2425,AC-2526", "AC-2425,AC-2526")]
        public async Task GetEmailEnabledFundingStreamAndPeriods_Tests(string fundingPeriodsGAG, string fundingPeriods1619, string fundingPeriod1416, string fundingPeriodDSG, string fundingPeriodDisabled)
        {
            // Arrange
            var expectedResult = GetEmailEnabledFundingStreamAndPeriodsResult_ExpectedResult(fundingPeriodsGAG, fundingPeriods1619, fundingPeriod1416, fundingPeriodDSG);

            var fundingViewData = new FundingViewData
            {
                FundingStreamCode = "ABC",
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

            var controller = new ExternalApiController(
                GetComponentService().Object,
                GetIdentityService().Object,
                GetAppConfigService().Object,
                GetUserJourneyService_EmailEnabledFundingStreams(fundingPeriodsGAG, fundingPeriods1619, fundingPeriod1416, fundingPeriodDSG, fundingPeriodDisabled).Object,
                GetMapper(),
                GetFundingApiService_SingleABCProvider().Object,
                GetFundingViewService(fundingViewData).Object,
                GetRazorViewToStringRenderer(new RenderHtmlViewModel { FundingViewData = fundingViewData }).Object,
                new MemoryCacheService(null, 0),
                GetGlobalSettingService().Object);

            // Act
            var actual = await controller.GetEmailEnabledFundingStreamAndPeriods();

            // Assert
            actual
                .Should().BeOfType<List<EmailEnabledFundingStreamAndPeriodsResult>>()
            .Which.Should().BeEquivalentTo(expectedResult);
        }

        private Mock<IRazorViewToStringRenderer> GetRazorViewToStringRenderer(RenderHtmlViewModel viewModel)
        {
            var renderer = new Mock<IRazorViewToStringRenderer>(MockBehavior.Strict);
            renderer
                .Setup(x => x.RenderViewToStringAsync(
                    "Shared/RenderHtml",
                    It.Is<RenderHtmlViewModel>(m =>
                        m.FundingViewData != null &&
                        m.FundingViewData.Components != null &&
                        m.FundingViewData.Components.Count == viewModel.FundingViewData.Components.Count)))
                .ReturnsAsync("ABC1");

            return renderer;
        }

        private Mock<IFundingViewService> GetFundingViewService(FundingViewData returnModel)
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
                .ReturnsAsync(returnModel);

            return fundingViewService;
        }

        private Mock<IFundingApiService> GetFundingApiService_SingleABCProvider()
        {
            var fundingApiService = new Mock<IFundingApiService>(MockBehavior.Strict);
            fundingApiService
                .Setup(fas => fas.GetProviderFunding(It.IsAny<string>()))
                .ReturnsAsync(new FundingApiSearchProviderFunding
                {
                    Id = "id",
                    OrganisationName = "A School",
                    FundingStreamCode = "ABC",
                    ParentProviderType = "LocalAuthority",
                    FundingPeriodCode = "AY-1920"
                });

            return fundingApiService;
        }

        private Mock<IFundingApiService> GetFundingApiService_NotFound()
        {
            var fundingApiService = new Mock<IFundingApiService>(MockBehavior.Strict);
            fundingApiService
                .Setup(fas => fas.GetProviderFunding(It.IsAny<string>()))
                .ReturnsAsync((FundingApiSearchProviderFunding)null);

            return fundingApiService;
        }

        private Mock<IComponentService> GetComponentService()
        {
            var componentService = new Mock<IComponentService>(MockBehavior.Strict);
            componentService
                .Setup(x => x.GetComponent(
                    It.IsAny<UiModelGroup>(),
                    It.IsAny<ComponentConfiguration>(),
                    It.IsAny<ComponentConfiguration>(),
                    It.IsAny<Dictionary<ComponentType, Defaults>>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Returns(new Component(null)
                {
                });

            return componentService;
        }

        private Mock<IClaimsBasedIdentityService> GetIdentityService()
        {
            var identityService = new Mock<IClaimsBasedIdentityService>(MockBehavior.Strict);
            identityService
                .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new Pds.Core.Common.Identity.Models.User());

            return identityService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService()
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>
                {
                    GetABCFundingStream()
                });

            return userJourneyService;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_AutoPull(string fsAutoPullConfig1, string fsAutoPullConfig2, string fsAutoPullConfig3)
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(GetCustomFundingStreams_AutoPullSetting(fsAutoPullConfig1, fsAutoPullConfig2, fsAutoPullConfig3));

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

        private FundingStream GetABCFundingStream()
        {
            return new FundingStream
            {
                FundingStreamCode = "ABC",
                FundingStreamName = "A B C grant",
                FundingStreamNameWithinSentence = "A bc grant",
                RelevantForProviders_LoggedIn = true,
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
                        Value = "FY-2122"
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

        private IMapper GetMapper()
        {
            var config = new TypeAdapterConfig();
            config.ConfigureWebMappings();
            return new Mapper(config);
        }

        private Mock<IOptions<ApplicationConfiguration>> GetAppConfigService()
        {
            var appConfigService = new Mock<IOptions<ApplicationConfiguration>>(MockBehavior.Strict);
            appConfigService
                .Setup(s => s.Value)
                .Returns(new ApplicationConfiguration());

            return appConfigService;
        }

        private Mock<IGlobalSettingService> GetGlobalSettingService()
        {
            return CommonMocks.GlobalSettingService();
        }

        private List<FundingStream> GetCustomFundingStreams_AutoPullSetting(string fsAutoPullSetting1, string fsAutoPullSetting2, string fsAutoPullSetting3)
        {
            FundingStream fs1 = CustomFundingStream_AutoPullSetting("ABC", "A B C grant", "A bc grant", fsAutoPullSetting1);
            FundingStream fs2 = CustomFundingStream_AutoPullSetting("DEF", "D E F grant", "D ef grant", fsAutoPullSetting2);
            FundingStream fs3 = CustomFundingStream_AutoPullSetting("GHI", "G H I grant", "G hi grant", fsAutoPullSetting3);

            return new List<FundingStream>
            {
                fs1,
                fs2,
                fs3
            };
        }

        private FundingStream CustomFundingStream_AutoPullSetting(string fsCode, string fsName, string fsNameSentence, string autoPullSetting)
        {
            FundingStream fs = new FundingStream
            {
                FundingStreamCode = fsCode,
                FundingStreamName = fsName,
                FundingStreamNameWithinSentence = fsNameSentence,
                RelevantForProviders_LoggedIn = true,
                RelevantForOrganisations_Public = true,
                Active = true
            };
            if (autoPullSetting != null)
            {
                fs.SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                            SettingId = 1,
                            CreatedAt = new DateTime(2020, 10, 1),
                            Value = autoPullSetting,
                            Setting = new SettingType
                            {
                                SettingName = "UseAutoPull",
                                SettingDescription = "SettingDescription1",
                                ValueDataType = SettingValueDataType.Bool,
                                ValuesAreEditable = false,
                            }
                        }
                    };
            }

            return fs;
        }

        private List<GetAutoPullFundingStreamResult> GetAutoPullFundingStreams_GenerateExpectedResults(string fs1AutoPullValue, string fs2AutoPullValue, string fs3AutoPullValue)
        {
            var getAutoPullFundingStreamResults = new List<GetAutoPullFundingStreamResult>();
            if (fs1AutoPullValue == "true")
            {
                var result = new GetAutoPullFundingStreamResult
                {
                    FundingStreamCode = "ABC",
                    FundingStreamName = "A B C grant"
                };
                getAutoPullFundingStreamResults.Add(result);
            }

            if (fs2AutoPullValue == "true")
            {
                var result = new GetAutoPullFundingStreamResult
                {
                    FundingStreamCode = "DEF",
                    FundingStreamName = "D E F grant"
                };
                getAutoPullFundingStreamResults.Add(result);
            }

            if (fs3AutoPullValue == "true")
            {
                var result = new GetAutoPullFundingStreamResult
                {
                    FundingStreamCode = "GHI",
                    FundingStreamName = "G H I grant"
                };
                getAutoPullFundingStreamResults.Add(result);
            }

            return getAutoPullFundingStreamResults;
        }

        private List<EmailEnabledFundingStreamAndPeriodsResult> GetEmailEnabledFundingStreamAndPeriodsResult_ExpectedResult(string fundingPeriodsGAG, string fundingPeriods1619, string fundingPeriod1416, string fundingPeriodDSG)
        {
            List<EmailEnabledFundingStreamAndPeriodsResult> expectedResult = new ();

            if (!string.IsNullOrWhiteSpace(fundingPeriodsGAG))
            {
                expectedResult.Add(new EmailEnabledFundingStreamAndPeriodsResult { FundingStreamCode = "GAG", FundingStreamName = "General Annual Grant", FundingPeriods = fundingPeriodsGAG.Split(",").ToList(), HasChildViewEnabled = true, HasParentViewEnabled = true, DigitalStatementsGoLiveDate = null });
            }

            if (!string.IsNullOrWhiteSpace(fundingPeriod1416))
            {
                expectedResult.Add(new EmailEnabledFundingStreamAndPeriodsResult { FundingStreamCode = "1416", FundingStreamName = "1416 Grant", FundingPeriods = fundingPeriod1416.Split(",").ToList(), HasChildViewEnabled = true, HasParentViewEnabled = false, DigitalStatementsGoLiveDate = null });
            }

            if (!string.IsNullOrWhiteSpace(fundingPeriodDSG))
            {
                expectedResult.Add(new EmailEnabledFundingStreamAndPeriodsResult { FundingStreamCode = "DSG", FundingStreamName = "DSG Grant", FundingPeriods = fundingPeriodDSG.Split(",").ToList(), HasChildViewEnabled = false, HasParentViewEnabled = true, DigitalStatementsGoLiveDate = null });
            }

            return expectedResult;
        }

        private Mock<IUserJourneyService> GetUserJourneyService_EmailEnabledFundingStreams(string fundingPeriodsGAG, string fundingPeriods1619, string fundingPeriod1416, string fundingPeriodDSG, string fundingPeriodDisabled)
        {
            var userJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);
            userJourneyService
                .Setup(s => s.GetFundingStreams())
                .ReturnsAsync(GetCustomFundingStreams_EmailEnabledFundingStreams(fundingPeriodsGAG, fundingPeriods1619, fundingPeriod1416, fundingPeriodDSG, fundingPeriodDisabled));

            return userJourneyService;
        }

        private FundingStream CustomFundingStream_EmailEnabledFundingStreams(string fsCode, string fsName, string fsNameSentence, string fundingPeriods, bool loggedInProvider, bool loggedInOrg, bool isActive = true)
        {
            FundingStream fs = new FundingStream
            {
                FundingStreamCode = fsCode,
                FundingStreamName = fsName,
                FundingStreamNameWithinSentence = fsNameSentence,
                RelevantForProviders_LoggedIn = loggedInProvider,
                RelevantForOrganisations_LoggedIn = loggedInOrg,
                Active = isActive,
            };
            if (fundingPeriods != null)
            {
                fs.SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                            SettingId = 1,
                            CreatedAt = new DateTime(2020, 10, 1),
                            Value = fundingPeriods,
                            Setting = new SettingType
                            {
                                SettingName = "EmailEnabledFundingPeriods",
                                SettingDescription = "SettingDescription1",
                                ValueDataType = SettingValueDataType.String,
                                ValuesAreEditable = false,
                            }
                        }
                    };
            }

            return fs;
        }

        private List<FundingStream> GetCustomFundingStreams_EmailEnabledFundingStreams(string fundingPeriodsGAG, string fundingPeriods1619, string fundingPeriod1416, string fundingPeriodDSG, string fundingPeriodDisabled)
        {
            FundingStream fs1 = CustomFundingStream_EmailEnabledFundingStreams("GAG", "General Annual Grant", "General Annual Grant", fundingPeriodsGAG, true, true);
            FundingStream fs2 = CustomFundingStream_EmailEnabledFundingStreams("1619", "1619 Grant", "1619 Grant", fundingPeriods1619, false, false);
            FundingStream fs3 = CustomFundingStream_EmailEnabledFundingStreams("1416", "1416 Grant", "1416 Grant", fundingPeriod1416, true, false);
            FundingStream fs4 = CustomFundingStream_EmailEnabledFundingStreams("DSG", "DSG Grant", "DSG Grant", fundingPeriodDSG, false, true);
            FundingStream fs5 = CustomFundingStream_EmailEnabledFundingStreams("DIS", "Disabled Grant", "Disabled Grant", fundingPeriodDSG, false, true, false);

            return new List<FundingStream>
            {
                fs1,
                fs2,
                fs3,
                fs4,
                fs5
            };
        }
    }
}