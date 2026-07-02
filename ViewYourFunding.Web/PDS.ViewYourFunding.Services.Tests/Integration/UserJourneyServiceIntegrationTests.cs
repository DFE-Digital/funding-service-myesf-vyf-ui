using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Repositories.Implementations;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Config;
using PDS.ViewYourFunding.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Integration
{
    [TestClass]
    public class UserJourneyServiceIntegrationTests
    {
        [TestMethod, TestCategory("Integration")]
        public async Task GetFundingStream_WithSetupData_HasCorrectInfo()
        {
            // Arrange
            var settingsService = new UserJourneyService(
                new MemoryCacheService(null, 0),
                GetMapper(),
                null,
                GetFundingStreamRepository());

            // Act
            var fundingStream = await settingsService.GetFundingStream("FSC1");

            // Assert
            fundingStream.FundingStreamName.Should().Be("FSC1 Name");
        }

        private static Context GetContext(string name)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(name).Options;
            return new Context(options);
        }

        /// <summary>
        /// Get Auto-mapper configuration.
        /// </summary>
        /// <returns>Mapper configuration.</returns>
        private static IMapper GetMapper()
        {
            return new MapperConfiguration(x => x.AddProfile(new ServicesAutoMapperProfile())).CreateMapper();
        }

        private IFundingStreamRepository GetFundingStreamRepository()
        {
            var databaseName = MethodBase.GetCurrentMethod().Name;
            var context = GetContext(databaseName);
            var fundingStreams = GetFundingStreamData();

            // Create in-memory database.
            context.AddRange(fundingStreams);
            context.SaveChanges();

            return new FundingStreamRepository(context, null);
        }

        private IMapper GetImapper()
        {
            return new MapperConfiguration(x => x.AddProfile(new ServicesAutoMapperProfile())).CreateMapper();
        }

        private IList<FundingStream> GetFundingStreamData()
        {
            return new List<FundingStream>
            {
                new FundingStream
                {
                    Id = 1,
                    FundingStreamCode = "FSC1",
                    FundingStreamName = "FSC1 Name",
                    Publications = new List<Publication>
                    {
                        new Publication
                        {
                            Description = "Publication1",
                            PublishedDate = new DateTime(2019, 10, 10),
                            CutOffDate = new DateTime(2020, 01, 01),
                            FundingPeriodCode = "AY-1920",
                            SpreadsheetModelVersion = 2,
                            UIModelVersion = 2,
                            Status = PublicationStatus.Published
                        },
                        new Publication
                        {
                            Description = "Publication2",
                            PublishedDate = new DateTime(2019, 10, 11),
                            FundingPeriodCode = "FY-2021",
                            SpreadsheetModelVersion = 1,
                            UIModelVersion = 2,
                            Status = PublicationStatus.Published
                        }
                    },
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                            CreatedAt = new DateTime(2020, 10, 1),
                            Value = "SettingValue1",
                            Setting = new Setting
                            {
                                SettingName = "SettingName1",
                                SettingDescription = "SettingDescription1",
                                ValueDataType = SettingValueDataType.String,
                                ValuesAreEditable = false
                            }
                        }
                    },
                    Active = true,
                    LastUpdatedBy = string.Empty
                },
                new FundingStream
                {
                    Id = 2,
                    FundingStreamCode = "FSC2",
                    FundingStreamName = "FSC2 Name",
                    Publications = new List<Publication>
                    {
                        new Publication
                        {
                            Description = "Publication1",
                            PublishedDate = new DateTime(2019, 10, 10)
                        },
                        new Publication
                        {
                            Description = "Publication2",
                            PublishedDate = new DateTime(2019, 10, 11)
                        },
                        new Publication
                        {
                            Description = "Publication3",
                            PublishedDate = new DateTime(2019, 10, 12)
                        }
                    },
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                            CreatedAt = new DateTime(2020, 10, 1),
                            Value = "SettingValue1",
                            Setting = new Setting
                            {
                                SettingName = "SettingName1",
                                SettingDescription = "SettingDescription1",
                                ValueDataType = SettingValueDataType.String,
                                ValuesAreEditable = false
                            }
                        },
                        new SettingValue
                        {
                            CreatedAt = new DateTime(2020, 10, 1),
                            Value = "SettingValue2",
                            Setting = new Setting
                            {
                                SettingName = "SettingName2",
                                SettingDescription = "SettingDescription2",
                                ValueDataType = SettingValueDataType.String,
                                ValuesAreEditable = false
                            }
                        }
                    },
                    Active = true,
                    LastUpdatedBy = string.Empty
                }
            };
        }
    }
}