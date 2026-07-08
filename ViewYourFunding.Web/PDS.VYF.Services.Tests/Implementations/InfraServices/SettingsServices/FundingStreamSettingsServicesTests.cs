using AutoMapper;
using Moq;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Cache;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
using PDS.VYF.Services.Implementations.InfraServices.SettingsServices;

namespace PDS.VYF.Services.Tests.Implementations.InfraServices.SettingsServices
{
    [TestClass]
    public class FundingStreamSettingsServicesTests
    {
        private MockRepository mockRepository;
        private Mock<ILoggerAdapter<IUserJourneyService>> mockLoggerAdapter;
        private Mock<IMapper> mockMapper;
        private Mock<ICacheService> mockCacheService;
        private Mock<IFundingStreamRepository> mockFundingStreamRepo;
        private Mock<IFundingStreamSettingsServices> mockFundingStreamSettingsServices;

        public FundingStreamSettingsServicesTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockFundingStreamSettingsServices = this.mockRepository.Create<IFundingStreamSettingsServices>();
            this.mockLoggerAdapter = this.mockRepository.Create<ILoggerAdapter<IUserJourneyService>>();
            this.mockMapper = new Mock<IMapper>();
            this.mockCacheService = this.mockRepository.Create<ICacheService>();
            this.mockFundingStreamRepo = this.mockRepository.Create<IFundingStreamRepository>();
        }

        [TestMethod]
        public async Task GetEmailEnabledFundingStreamPeriod_StateUnderTest_ExpectedBehavior()
        {
            var expectedFSCodes = new List<string>
            {
                "FS1-AC-2021,FS1-AC-2022"
            };

            var fundingStreams = new List<FundingStream>
            {
                new FundingStream
                {
                    FundingStreamCode = "FS1",
                    Active = true,
                    RelevantForProviders_LoggedIn = true,
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                             Setting = new SettingType
                                {
                                    SettingName = "EmailEnabledFundingPeriods"
                                },
                             Value = "AC-2021,AC-2022"
                        }
                    }
                }
            };

            var service = CreateServiceWithMockedCache(fundingStreams);

            // Act
            var actual = await service.GetEmailEnabledFundingStreamPeriod();
            CollectionAssert.AreEqual(expectedFSCodes, actual);

            // Assert
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetEmailEnabledFundingStreamPeriod_StateUnderTest_WithNullEmailEnabledFundingPeriodsValue_ExpectedBehavior()
        {
            var expectedFSCodes = new List<string>
            {
            };

            var fundingStreams = new List<FundingStream>
            {
                new FundingStream
                {
                    FundingStreamCode = "FS1",
                    Active = true,
                    RelevantForProviders_LoggedIn = true,
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                             Setting = new SettingType
                                {
                                    SettingName = "EmailEnabledFundingPeriods"
                                },
                             Value = string.Empty
                        }
                    }
                }
            };

            var service = CreateServiceWithMockedCache(fundingStreams);

            // Act
            var actual = await service.GetEmailEnabledFundingStreamPeriod();
            CollectionAssert.AreEqual(expectedFSCodes, actual);

            // Assert
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetEmailEnabledFundingStreamPeriod_StateUnderTest_WithWhiteSpaceEmailEnabledFundingPeriodsValueNoFundingStreamCode_ExpectedBehavior()
        {
            var expectedFSCodes = new List<string>
            {
            };

            var fundingStreams = new List<FundingStream>
            {
                new FundingStream
                {
                    FundingStreamCode = " ",
                    Active = true,
                    RelevantForProviders_LoggedIn = true,
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                             Setting = new SettingType
                                {
                                    SettingName = "EmailEnabledFundingPeriods"
                                },
                             Value = " "
                        }
                    }
                }
            };

            var service = CreateServiceWithMockedCache(fundingStreams);

            // Act
            var actual = await service.GetEmailEnabledFundingStreamPeriod();
            CollectionAssert.AreEqual(expectedFSCodes, actual);

            // Assert
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetEmailEnabledFundingStreamPeriod_StateUnderTest_WithWhiteSpaceEmailEnabledFundingPeriodsValueWithFundingStreamCode_ExpectedBehavior()
        {
            var expectedFSCodes = new List<string>
            {
            };

            var fundingStreams = new List<FundingStream>
            {
                new FundingStream
                {
                    FundingStreamCode = "GAG",
                    Active = true,
                    RelevantForProviders_LoggedIn = true,
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                             Setting = new SettingType
                                {
                                    SettingName = "EmailEnabledFundingPeriods"
                                },
                             Value = " "
                        }
                    }
                }
            };

            var service = CreateServiceWithMockedCache(fundingStreams);

            // Act
            var actual = await service.GetEmailEnabledFundingStreamPeriod();
            CollectionAssert.AreEqual(expectedFSCodes, actual);

            // Assert
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetEmailEnabledFundingStreamPeriod_StateUnderTest_WithFundingStreamWithNoFundingStreamCodeValue_ExpectedBehavior()
        {
            var expectedFSCodes = new List<string> { string.Empty };

            var fundingStreams = new List<FundingStream>
            {
                new FundingStream
                {
                    Active = true,
                    RelevantForProviders_LoggedIn = true,
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                             Setting = new SettingType
                                {
                                    SettingName = "EmailEnabledFundingPeriods"
                                },
                             Value = "AC-2021,AC-2022"
                        }
                    }
                }
            };

            var service = CreateServiceWithMockedCache(fundingStreams);

            // Act
            var actual = await service.GetEmailEnabledFundingStreamPeriod();
            CollectionAssert.AreEqual(expectedFSCodes, actual);

            // Assert
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetEmailEnabledFundingStreamPeriod_StateUnderTest_WithMultipleStreams_ExpectedBehavior()
        {
            var expectedFSCodes = new List<string>
            {
                "FS1-AC-2023,FS1-AC-2024", "FS2-AS-2023"
            };

            var fundingStreams = new List<FundingStream>
            {
                new FundingStream
                {
                    FundingStreamCode = "FS1",
                    Active = true,
                    RelevantForProviders_LoggedIn = true,
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                             Setting = new SettingType
                                {
                                    SettingName = "EmailEnabledFundingPeriods"
                                },
                             Value = "AC-2023,AC-2024"
                        }
                    }
                },
                new FundingStream
                {
                    FundingStreamCode = "FS2",
                    Active = true,
                    RelevantForProviders_LoggedIn = true,
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                             Setting = new SettingType
                                {
                                    SettingName = "EmailEnabledFundingPeriods"
                                },
                             Value = "AS-2023"
                        }
                    }
                }
            };

            var service = CreateServiceWithMockedCache(fundingStreams);

            // Act
            var actual = await service.GetEmailEnabledFundingStreamPeriod();
            CollectionAssert.AreEqual(expectedFSCodes, actual);

            // Assert
            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetEmailEnabledFundingStreamPeriod_StateUnderTest_WithNoValidFundingStreams_ExpectedBehavior()
        {
            var expectedFSCodes = new List<string>
            {
            };

            var fundingStreams = new List<FundingStream>
            {
                new FundingStream
                {
                },
            };

            var service = CreateServiceWithMockedCache(fundingStreams);

            // Act
            var actual = await service.GetEmailEnabledFundingStreamPeriod();
            CollectionAssert.AreEqual(expectedFSCodes, actual);

            // Assert
            this.mockRepository.VerifyAll();
        }

        private FundingStreamSettingsServices CreateServiceWithMockedCache(IList<FundingStream> fundingStreams)
        {
            var cacheMock = new Mock<ICacheService>();
            cacheMock.Setup(c => c.AddOrGetExistingResultAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IList<FundingStream>>>>(),
                CacheExpirationPolicy.Absolute,
                It.IsAny<TimeSpan>()))
                .ReturnsAsync(fundingStreams);

            return new FundingStreamSettingsServices(
                cacheMock.Object,
                Mock.Of<IMapper>(),
                Mock.Of<ILoggerAdapter<IUserJourneyService>>(),
                Mock.Of<IFundingStreamRepository>());
        }
    }
}
