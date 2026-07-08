using Microsoft.EntityFrameworkCore;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    public abstract class DataContextTestBase
    {
        public const string DoNotUseSettingNameAndDescription = "donotuseregressionsetting";

        private const string DoNotUseSettingTypeSettingNameAndDescription = "donotuseregressionsettingtype";

        /// <summary>
        /// Allows you to obtain the method or property name of the caller to the method.
        /// Using reflection, MethodBase.GetCurrentMethod().Name returned MoveNext.
        /// </summary>
        /// <param name="name">Caller member name.</param>
        /// <returns>Name of property or method.</returns>
        public static string GetAsyncMethodName([CallerMemberName] string name = null)
        {
            return name;
        }

        /// <summary>
        /// Create DbContext for in-memory database.
        /// </summary>
        /// <param name="name">Database name.</param>
        /// <returns>DbContext.</returns>
        public static Context GetContext(string name)
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(name).Options;
            return new Context(options);
        }

        public static IList<FundingStream> GetFundingStreamData()
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
                            SettingId = 1,
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
                    NextPaymentTypes = new List<NextPaymentType>
                    {
                        new NextPaymentType
                        {
                            TypeCode = "test21",
                            Description = "test21 desc",
                            LastUpdatedBy = string.Empty
                        },
                        new NextPaymentType
                        {
                            TypeCode = "test22",
                            Description = "test22 desc",
                            LastUpdatedBy = string.Empty
                        }
                    },
                    NextPayments = new List<NextPayment>
                    {
                        new NextPayment
                        {
                           NextPaymentDate = new DateTime(1, 1, 1),
                           LastUpdatedBy = string.Empty
                        },
                        new NextPayment
                        {
                            NextPaymentDate = new DateTime(1, 1, 1),
                            LastUpdatedBy = string.Empty
                        }
                    },
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
                            SettingId = 2,
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
                            SettingId = 3,
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
                    NextPaymentTypes = new List<NextPaymentType>
                    {
                        new NextPaymentType
                        {
                            TypeCode = "test2",
                            Description = "test2 desc",
                            LastUpdatedBy = string.Empty
                        },
                        new NextPaymentType
                        {
                            TypeCode = "test3",
                            Description = "test3 desc",
                            LastUpdatedBy = string.Empty
                        }
                    },
                    NextPayments = new List<NextPayment>
                    {
                        new NextPayment
                        {
                           NextPaymentDate = new DateTime(1, 1, 1),
                           LastUpdatedBy = string.Empty
                        },
                        new NextPayment
                        {
                            NextPaymentDate = new DateTime(1, 1, 1),
                            LastUpdatedBy = string.Empty
                        }
                    },
                    LastUpdatedBy = string.Empty
                }
            };
        }

        public static SettingValue GetSettingValue()
        {
            return new SettingValue
            {
                FundingStreamId = 1,
                Value = "test"
            };
        }

        public static Setting GetRegressionSettingValueTestSettingType()
        {
            return new Setting
            {
                ValueDataType = SettingValueDataType.String,
                SettingDescription = DoNotUseSettingNameAndDescription,
                SettingName = DoNotUseSettingNameAndDescription
            };
        }

        public static Setting GetRegressionSettingType()
        {
            return new Setting
            {
                ValueDataType = SettingValueDataType.String,
                SettingDescription = DoNotUseSettingTypeSettingNameAndDescription,
                SettingName = DoNotUseSettingTypeSettingNameAndDescription
            };
        }
    }
}