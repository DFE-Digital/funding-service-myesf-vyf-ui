using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    /// <summary>
    /// The FundingViewDataGeneratorTests class.
    /// </summary>
    [TestClass]
    public class FundingViewDataGeneratorTests
    {
        /// <summary>
        /// The test entity name.
        /// </summary>
        private const string TestEntityName = "TESTLA";

        /// <summary>
        /// The test provider entity name.
        /// </summary>
        private const string TestProviderEntityName = "TESTProvider";

        /// <summary>
        /// The test funding total.
        /// </summary>
        private const double TestFundingTotal = 12345678;

        /// <summary>
        /// The test external publication date.
        /// </summary>
        private static readonly DateTime TestExternalPublicationDate = new DateTime(2019, 10, 01);

        /// <summary>
        /// The test model group identifier.
        /// </summary>
        private const string TestModelGroupId = "test-funding-amount";

        /// <summary>
        /// The test json property name.
        /// </summary>
        private const string TestJsonPropertyName = "fundingAmount";

        /// <summary>
        /// The test funding amount.
        /// </summary>
        private const decimal TestFundingAmount = 1234.56m;

        /// <summary>
        /// The test funding value.
        /// </summary>
        private static readonly string TestFundingValue =
            "{ \"" + TestJsonPropertyName + "\": " + TestFundingAmount.ToString() + " }";

        /// <summary>
        /// Generates from funding data returns correct result.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Generate_FromFundingDataSimple_ReturnsCorrectResult()
        {
            // Arrange
            var uiModel = new UiModel
            {
                Dataset = new List<UiModelDataset>
                {
                    new UiModelDataset
                    {
                        DatasetName = "funding"
                    }
                },
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup
                    {
                        Id = TestModelGroupId,
                        Selector = $"$..{TestJsonPropertyName}"
                    }
                }
            };

            var data = new FundingApiSearchResponse
            {
                Funding = new List<FundingApiSearchFunding>
                {
                    new FundingApiSearchFunding
                    {
                        GroupName = TestEntityName,
                        TotalAmount = TestFundingTotal,
                        StatusChangedDate = TestExternalPublicationDate,
                        FundingValue = TestFundingValue,
                        GroupingType = "LocalAuthority"
                    }
                }
            };

            var config = new FundingStream
            {
                FundingStreamCode = "DSG",
                FundingStreamName = "Dedicated schools grant"
            };

            var componentService = new ComponentService(null, null);

            var generator = new FundingViewDataGenerator(
                uiModel,
                "FY-2021",
                data,
                null,
                DateTime.Now,
                null,
                config,
                null,
                null,
                true,
                true,
                null,
                null,
                null,
                componentService,
                new ComponentConfigurationService(componentService, GetBasePathService().Object),
                false,
                false,
                VarianceSelectionOption.NoComparison,
                false,
                false,
                true);

            // Act
            var actual = generator.Generate();

            // Assert
            actual.Should().NotBeNull();
        }

        /// <summary>
        /// Generates from funding data returns correct result.
        /// </summary>
        /// <param name="numberFormat">The number format to use.</param>
        /// <param name="expected">The expected result.</param>
        [DataRow("GBCurrencyWithoutTrailingZeroes", "£1,234.56")]
        [DataRow("GBCurrency", "£1,234.56")]
        [DataRow("TwoDPWithoutTrailingZeroes", "1,234.56")]
        [DataRow("PercentageWith1DecimalPlace", "1,234.6%")]
        [DataRow("PercentageWith2DecimalPlaces", "1,234.56%")]
        [TestMethod, TestCategory("Unit")]
        public void Generate_FromFundingDataSimpleWithNumberFormats_ReturnsCorrectResult(string numberFormat, string expected)
        {
            // Arrange
            var uiModel = new UiModel
            {
                Dataset = new List<UiModelDataset>
                {
                    new UiModelDataset
                    {
                        DatasetName = "funding"
                    }
                },
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup
                    {
                        Id = TestModelGroupId,
                        Selector = $"$..{TestJsonPropertyName}",
                        Datastyle = new UiModelStyle
                        {
                            NumberFormat = numberFormat
                        }
                    }
                }
            };

            var data = new FundingApiSearchResponse
            {
                Funding = new List<FundingApiSearchFunding>
                {
                    new FundingApiSearchFunding
                    {
                        GroupName = TestEntityName,
                        TotalAmount = TestFundingTotal,
                        StatusChangedDate = TestExternalPublicationDate,
                        FundingValue = TestFundingValue,
                        GroupingType = "LocalAuthority"
                    }
                }
            };

            var config = new FundingStream
            {
                FundingStreamCode = "DSG",
                FundingStreamName = "Dedicated schools grant"
            };

            var generator = new FundingViewDataGenerator(
                uiModel,
                "FY-2021",
                data,
                null,
                DateTime.Now,
                null,
                config,
                null,
                null,
                true,
                true,
                null,
                null,
                null,
                new ComponentService(null, null),
                new ComponentConfigurationService(new ComponentService(null, null), GetBasePathService().Object),
                false,
                false,
                VarianceSelectionOption.NoComparison,
                false,
                false,
                true);

            // Act
            var actual = generator.Generate();

            // Assert
            actual.Components.First().Values.First().Should().Be(expected);
        }

        /// <summary>
        /// Generates from funding data returns correct result.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Generate_FromFundingDataIntemiediateDSG_ReturnsCorrectComponent()
        {
            // Arrange
            var uiModel = new UiModel
            {
                Dataset = new List<UiModelDataset>
                {
                    new UiModelDataset
                    {
                        DatasetName = "funding"
                    }
                },
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup
                    {
                        Id = TestModelGroupId,
                        Selector = $"$..{TestJsonPropertyName}"
                    }
                }
            };

            var data = new FundingApiSearchResponse
            {
                Funding = new List<FundingApiSearchFunding>
                {
                    new FundingApiSearchFunding
                    {
                        GroupName = TestEntityName,
                        TotalAmount = TestFundingTotal,
                        StatusChangedDate = TestExternalPublicationDate,
                        FundingValue = TestFundingValue,
                        GroupingType = "LocalAuthority"
                    }
                }
            };

            var componentService = new ComponentService(null, null);

            var config = new FundingStream
            {
                FundingStreamCode = "DSG",
                FundingStreamName = "Dedicated schools grant",
                FundingStreamNameWithinSentence = "dedicated schools grant",
                FundingStreamCodePubliclyKnown = true
            };

            var generator = new FundingViewDataGenerator(
                uiModel,
                "FY-2021",
                data,
                null,
                new DateTime(2010, 7, 4),
                null,
                config,
                null,
                null,
                true,
                true,
                null,
                null,
                null,
                componentService,
                new ComponentConfigurationService(componentService, GetBasePathService().Object),
                false,
                false,
                VarianceSelectionOption.NoComparison,
                false,
                false,
                true);

            var expectedFundingData = new FundingApiSearchFunding
            {
                TotalAmount = 12345678,
                FundingValue = @"{ ""fundingAmount"": 1234.56 }"
            };

            var expectedMeta = new ComponentConfiguration
            {
                AllDatasetsData = new List<List<IFundingApiSearch>>
                {
                    new List<IFundingApiSearch>
                    {
                        expectedFundingData
                    }
                },
                ProviderType = "LocalAuthority",
                ProviderSubType = "LocalAuthority",
                BasePath = "Base Path",
                UrlForLoggedInView = "Url For Logged In Provider Path",
                FundingStreamName = "Dedicated schools grant",
                FundingStreamNameWithinSentence = "dedicated schools grant",
                FundingPeriodCode = "FY-2021",
                FundingStreamCode = "DSG",
                OpeningDay = 1,
                OpeningMonth = 1,
                OpeningYear = 1,
                Year1 = 2020,
                Year2 = 2021,
                ShortFundingStreamNameHtml = "<abbr title=\"Dedicated schools grant\">DSG</abbr>",
                ExpandedFundingStreamNameHtml = "Dedicated schools grant (<abbr title=\"Dedicated schools grant\">DSG</abbr>)",
                FundingStreamPathPart = "dedicated-schools-grant",
                IsCurrentYear = true,
                YearType = YearTypeName.FinancialYear,
                YearTypeCode = YearTypeCode.FinancialYear,
                IsLatestOrFinalFundingForYear = true,
                NoNextPaymentForOrganisationText = "There are no more scheduled payments for this organisation.",
                NoNextPaymentForTheYearText = "There are no more scheduled payments for financial year 2020 to 2021.",
                PublishedDate = new DateTime(2010, 7, 4),
                ShowNameAbbreviation = true,
                LocalAuthorityName = "TESTLA",
                LocalAuthorityNameOverride = "TESTLA",
                FundingStreamCodeOrName = "DSG",
                AsOfMonth = "Sept",
                AsOfYear = "2020",
                ProviderName = "TESTLA",
                IsLocalAuthority = true,
                Data = expectedFundingData,
                Variables = new Dictionary<string, object>
                {
                    { "censusYear", 2020 },
                    { "R06Year1", 2019 },
                    { "R06Year2", 2020 }
                },
                ProviderOpenDate = DateTime.MinValue,
                ProviderOpenDateddMMMMyyyy = DateTime.MinValue.ToString("dd MMMM yyyy"),
                Year0 = 2019,
                OriginalModel = new UiModel
                {
                    Dataset = new List<UiModelDataset>
                    {
                        new UiModelDataset
                        {
                            DatasetName = "funding"
                        }
                    },
                    Groups = new List<UiModelGroup>
                    {
                        new UiModelGroup
                        {
                            Id = "test-funding-amount",
                            Selector = "$..fundingAmount"
                        }
                    }
                },
                DaysInYear = DateTime.Now.NumberOfDaysInYear(),
                StatusChangedDate = new DateTime(2019, 10, 1),
                StatusChangedDateUiFormatted = new DateTime(2019, 10, 1).ToDateDisplay(),
                VarianceSelectionOption = VarianceSelectionOption.NoComparison
            };

            var expected = new List<Component>
            {
                new Component(expectedMeta)
                {
                    Id = "test-funding-amount",
                    Values = new List<object>
                    {
                        1234.56
                    },
                    Title = string.Empty,
                    AlternativeTitle = string.Empty,
                    OriginalGroup = new UiModelGroup
                    {
                        Id = "test-funding-amount",
                        Selector = "$..fundingAmount"
                    },
                    VarianceValueFormatted = string.Empty
                }
            };

            // Act
            var actual = generator.Generate();

            // Assert
            actual.Components.Should().HaveCountGreaterThanOrEqualTo(expected.Count);
            actual.Components.Should().BeEquivalentTo(expected);
        }

        /// <summary>
        /// Generates from funding data returns correct result.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Generate_FromFundingDataIntemiediatePSG_ReturnsCorrectComponent()
        {
            // Arrange
            var uiModel = new UiModel
            {
                Dataset = new List<UiModelDataset>
                {
                    new UiModelDataset
                    {
                        DatasetName = "funding"
                    }
                },
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup
                    {
                        Id = TestModelGroupId,
                        Selector = $"$..{TestJsonPropertyName}"
                    }
                }
            };

            var data = new FundingApiSearchResponse
            {
                Funding = new List<FundingApiSearchFunding>
                {
                    new FundingApiSearchFunding
                    {
                        GroupName = TestEntityName,
                        TotalAmount = TestFundingTotal,
                        StatusChangedDate = TestExternalPublicationDate,
                        FundingValue = TestFundingValue,
                        GroupingType = "LocalAuthority"
                    }
                }
            };

            var componentService = new ComponentService(null, null);

            var config = new FundingStream
            {
                FundingStreamCode = "PSG",
                FundingStreamName = "PE and sport premium"
            };

            var generator = new FundingViewDataGenerator(
                uiModel,
                "AY-1920",
                data,
                null,
                new DateTime(2010, 7, 4),
                null,
                config,
                null,
                null,
                true,
                true,
                null,
                null,
                null,
                componentService,
                new ComponentConfigurationService(componentService, GetBasePathService().Object),
                false,
                false,
                VarianceSelectionOption.NoComparison,
                false,
                false,
                true);

            var expectedFundingData = new FundingApiSearchFunding
            {
                TotalAmount = 12345678,
                FundingValue = @"{ ""fundingAmount"": 1234.56 }"
            };

            var expectedMeta = new ComponentConfiguration
            {
                AllDatasetsData = new List<List<IFundingApiSearch>>
                {
                    new List<IFundingApiSearch>
                    {
                        expectedFundingData
                    }
                },
                ProviderType = "LocalAuthority",
                ProviderSubType = "LocalAuthority",
                BasePath = "Base Path",
                UrlForLoggedInView = "Url For Logged In Provider Path",
                FundingStreamName = "PE and sport premium",
                FundingPeriodCode = "AY-1920",
                FundingStreamCode = "PSG",
                OpeningDay = 1,
                OpeningMonth = 1,
                OpeningYear = 1,
                Year1 = 2019,
                Year2 = 2020,
                ShortFundingStreamNameHtml = "PE and sport premium",
                ExpandedFundingStreamNameHtml = "PE and sport premium",
                FundingStreamPathPart = "pe-and-sport-premium",
                IsCurrentYear = true,
                YearType = YearTypeName.AcademicYear,
                YearTypeCode = YearTypeCode.AcademicYear,
                IsLatestOrFinalFundingForYear = true,
                NoNextPaymentForOrganisationText = "There are no more scheduled payments for this organisation.",
                NoNextPaymentForTheYearText = "There are no more scheduled payments for academic year 2019 to 2020.",
                PublishedDate = new DateTime(2010, 7, 4),
                LocalAuthorityName = "TESTLA",
                LocalAuthorityNameOverride = "TESTLA",
                FundingStreamCodeOrName = "PE and sport premium",
                AsOfMonth = "September",
                AsOfYear = "2019",
                ProviderName = "TESTLA",
                IsLocalAuthority = true,
                FundingStreamNameWithinSentence = "PE and sport premium",
                Data = expectedFundingData,
                Variables = new Dictionary<string, object>
                {
                    { "censusYear", 2019 },
                    { "R06Year1", 2018 },
                    { "R06Year2", 2019 }
                },
                ProviderOpenDate = DateTime.MinValue,
                ProviderOpenDateddMMMMyyyy = DateTime.MinValue.ToString("dd MMMM yyyy"),
                Year0 = 2018,
                OriginalModel = new UiModel
                {
                    Dataset = new List<UiModelDataset>
                    {
                        new UiModelDataset
                        {
                            DatasetName = "funding"
                        }
                    },
                    Groups = new List<UiModelGroup>
                    {
                        new UiModelGroup
                        {
                            Id = "test-funding-amount",
                            Selector = "$..fundingAmount"
                        }
                    }
                },
                DaysInYear = DateTime.Now.NumberOfDaysInYear(),
                StatusChangedDate = new DateTime(2019, 10, 1),
                StatusChangedDateUiFormatted = new DateTime(2019, 10, 1).ToDateDisplay(),
                VarianceSelectionOption = VarianceSelectionOption.NoComparison
            };

            var expected = new List<Component>
            {
                new Component(expectedMeta)
                {
                    Id = "test-funding-amount",
                    Values = new List<object>
                    {
                        1234.56
                    },
                    Title = string.Empty,
                    AlternativeTitle = string.Empty,
                    OriginalGroup = new UiModelGroup
                    {
                        Id = "test-funding-amount",
                        Selector = "$..fundingAmount"
                    },
                    VarianceValueFormatted = string.Empty
                }
            };

            // Act
            var actual = generator.Generate();

            // Assert
            actual.Components.Should().HaveCountGreaterThanOrEqualTo(expected.Count);
            actual.Components.Should().BeEquivalentTo(expected);
        }

        /// <summary>
        /// Generates from funding data returns correct result.
        /// </summary>
        /// <param name="secondFundingValue">The second funding value.</param>
        /// <param name="expectedVariance">The expected variance.</param>
        /// <param name="valueFormat">The value format.</param>
        /// <param name="expectedVarianceFormatted">The expected variance formatted.</param>
        [TestMethod, TestCategory("Unit")]
        [DataRow("5235.58", "-4001.02", "GBCurrencyWithoutTrailingZeroes", "£4,001.02")]
        [DataRow("1234.56", "0", "", "0")]
        [DataRow("1135.58", "98.98000000000002", "GBCurrencyWithoutDecimalPlace", "£99")]
        public void Generate_FromFundingDataGAG_Variance_ReturnsCorrectComponent(
            string secondFundingValue,
            string expectedVariance,
            string valueFormat,
            string expectedVarianceFormatted)
        {
            // Arrange
            var expectedVarianceValue = Convert.ToDouble(expectedVariance);
            var uiModel = new UiModel
            {
                Dataset = new List<UiModelDataset>
                {
                    new UiModelDataset
                    {
                        DatasetName = "providerFunding"
                    }
                },
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup
                    {
                        Id = TestModelGroupId,
                        Selector = $"$..{TestJsonPropertyName}",
                        EnableVariance = true,
                        Datastyle = new UiModelStyle
                        {
                            NumberFormat = valueFormat
                        }
                    }
                }
            };

            var secondFundingValueJson = "{ \"" + TestJsonPropertyName + "\": " + Convert.ToDecimal(secondFundingValue) + " }";

            var data = new ProviderFundingApiSearchResponse
            {
                ProviderFunding = new List<FundingApiSearchProviderFunding>
                {
                    new FundingApiSearchProviderFunding
                    {
                        OrganisationName = TestProviderEntityName,
                        TotalAmount = Convert.ToDouble(TestFundingTotal),
                        StatusChangedDate = TestExternalPublicationDate,
                        FundingValue = TestFundingValue
                    },
                    new FundingApiSearchProviderFunding
                    {
                        OrganisationName = TestProviderEntityName,
                        TotalAmount = Convert.ToDouble(TestFundingTotal),
                        StatusChangedDate = TestExternalPublicationDate.AddDays(10),
                        FundingValue = secondFundingValueJson
                    }
                }
            };

            var componentService = new ComponentService(null, null);

            var config = new FundingStream
            {
                FundingStreamCode = "GAG",
                FundingStreamName = "General Annual Grant"
            };

            var generator = new FundingViewDataGenerator(
                uiModel,
                "AY-2021",
                null,
                data,
                new DateTime(2020, 10, 4),
                null,
                config,
                null,
                null,
                true,
                true,
                null,
                null,
                null,
                componentService,
                new ComponentConfigurationService(componentService, GetBasePathService().Object),
                false,
                false,
                VarianceSelectionOption.FinalStatementPreviousYear,
                false,
                false,
                true);

            var expectedFundingData1 = new FundingApiSearchFunding
            {
                TotalAmount = 12345678,
                FundingValue = @"{ ""fundingAmount"": 1234.56 }"
            };

            var expectedFundingData2 = new FundingApiSearchFunding
            {
                TotalAmount = 12345678,
                FundingValue = @"{ ""fundingAmount"": " + secondFundingValue + " }"
            };

            var expectedMeta = new ComponentConfiguration
            {
                AllDatasetsData = new List<List<IFundingApiSearch>>
                {
                    new List<IFundingApiSearch>
                    {
                        expectedFundingData1,
                        expectedFundingData2
                    }
                },
                LocalAuthorityName = "TESTProvider",
                LocalAuthorityNameOverride = "TESTProvider",
                StatusChangedDate = new DateTime(2019, 10, 01),
                StatusChangedDateUiFormatted = new DateTime(2019, 10, 1).ToDateDisplay(),
                BasePath = "Base Path",
                UrlForLoggedInView = "Url For Logged In Provider Path",
                FundingStreamName = "General Annual Grant",
                FundingPeriodCode = "AY-2021",
                FundingStreamCode = "GAG",
                OpeningDay = 1,
                OpeningMonth = 1,
                OpeningYear = 1,
                Year1 = 2020,
                Year2 = 2021,
                ShortFundingStreamNameHtml = "General Annual Grant",
                ExpandedFundingStreamNameHtml = "General Annual Grant",
                FundingStreamPathPart = "general-annual-grant",
                IsCurrentYear = true,
                VarianceSelectionOption = VarianceSelectionOption.FinalStatementPreviousYear,
                YearType = YearTypeName.AcademicYear,
                YearTypeCode = YearTypeCode.AcademicYear,
                IsLatestOrFinalFundingForYear = true,
                NoNextPaymentForOrganisationText = "There are no more scheduled payments for this organisation.",
                NoNextPaymentForTheYearText = "There are no more scheduled payments for academic year 2020 to 2021.",
                PublishedDate = new DateTime(2020, 10, 4),
                Variables = new Dictionary<string, object>
                {
                    { "censusYear", 2020 },
                    { "R06Year1", 2019 },
                    { "R06Year2", 2020 }
                },
                FundingStreamCodeOrName = "General Annual Grant",
                AsOfMonth = "September",
                AsOfYear = "2020",
                ProviderName = TestProviderEntityName,
                IsLocalAuthority = false,
                FundingStreamNameWithinSentence = "General Annual Grant",
                Data = expectedFundingData1,
                ProviderOpenDate = DateTime.MinValue,
                ProviderOpenDateddMMMMyyyy = DateTime.MinValue.ToString("dd MMMM yyyy"),
                Year0 = 2019,
                OriginalModel = new UiModel
                {
                    Dataset = new List<UiModelDataset>
                    {
                        new UiModelDataset
                        {
                            DatasetName = "providerFunding"
                        }
                    },
                    Groups = new List<UiModelGroup>
                    {
                        new UiModelGroup
                        {
                            Id = "test-funding-amount",
                            Selector = "$..fundingAmount",
                            EnableVariance = true,
                            Datastyle = new UiModelStyle
                            {
                                NumberFormat = valueFormat
                            }
                        }
                    }
                },
                DaysInYear = DateTime.Now.NumberOfDaysInYear()
            };

            var expectedValue = valueFormat switch
            {
                "GBCurrencyWithoutTrailingZeroes" => (object)"£1,234.56",
                "GBCurrencyWithoutDecimalPlace" => "£1,235",
                _ => 1234.56
            };

            var expected = new List<Component>
            {
                new Component(expectedMeta)
                {
                    Id = "test-funding-amount",
                    Values = new List<object>
                    {
                        expectedValue
                    },
                    UnevaluatedValues = new List<object>
                    {
                        expectedValue
                    },
                    Title = string.Empty,
                    AlternativeTitle = string.Empty,
                    OriginalGroup = new UiModelGroup
                    {
                        Id = "test-funding-amount",
                        Selector = "$..fundingAmount",
                        Datastyle = new UiModelStyle
                        {
                            NumberFormat = valueFormat
                        },
                        EnableVariance = true
                    },
                    VarianceValueFormatted = expectedVarianceFormatted,
                    EnableVariance = true,
                    VarianceValue = expectedVarianceValue,
                }
            };

            // Act
            var actual = generator.Generate();

            // Assert
            actual.Components.Should().HaveCountGreaterThanOrEqualTo(expected.Count);
            actual.Components.Should().BeEquivalentTo(expected);
        }

        /// <summary>
        /// Generates from funding data returns correct result.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Generate_FromFundingDataComplexDSG_ReturnsCorrectComponent()
        {
            // Arrange
            var uiModel = new UiModel
            {
                Dataset = new List<UiModelDataset>
                {
                    new UiModelDataset
                    {
                        DatasetName = "funding"
                    }
                },
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup
                    {
                        Id = "SomeVarTest",
                        Expression = "@Var1"
                    },
                    new UiModelGroup
                    {
                        Id = "SomeVarTest2",
                        Expression = "@Var2"
                    },
                    new UiModelGroup
                    {
                        Id = "SomeVarTest3",
                        Expression = "@Var3"
                    },
                    new UiModelGroup
                    {
                        Id = "SomeVarTest4",
                        Expression = "@Var4"
                    },
                    new UiModelGroup
                    {
                        Id = "SomeVarTest5",
                        Expression = "@Var5"
                    },
                    new UiModelGroup
                    {
                        Id = "SomeVarTest6",
                        Expression = "@Var6"
                    },
                    new UiModelGroup
                    {
                        Id = "SomeVarTest7",
                        Expression = "@Var6=!NO!"
                    },
                    new UiModelGroup
                    {
                        Id = TestModelGroupId,
                        Selector = $"$..{TestJsonPropertyName}"
                    }
                },
                Variables = new List<UIModelVariable>
                {
                    new UIModelVariable
                    {
                        Name = "Var1",
                        Value = "HELLO"
                    },
                    new UIModelVariable
                    {
                        Name = "Var2",
                        Expression = "1=1"
                    },
                    new UIModelVariable
                    {
                        Name = "Var3",
                        Selector = $"$..{TestJsonPropertyName}"
                    },
                    new UIModelVariable
                    {
                        Name = "Var4",
                        Expression = "@Var2=true"
                    },
                    new UIModelVariable
                    {
                        Name = "Var5",
                        Expression = "@Var4=true",
                        TrueResponse = "YES",
                        FalseResponse = "NO"
                    },
                    new UIModelVariable
                    {
                        Name = "Var6",
                        Expression = "@Var4=false",
                        TrueResponse = "!YES!",
                        FalseResponse = "!NO!"
                    }
                }
            };

            var data = new FundingApiSearchResponse
            {
                Funding = new List<FundingApiSearchFunding>
                {
                    new FundingApiSearchFunding
                    {
                        GroupName = TestEntityName,
                        TotalAmount = TestFundingTotal,
                        StatusChangedDate = TestExternalPublicationDate,
                        FundingValue = TestFundingValue,
                        GroupingType = "LocalAuthority"
                    }
                }
            };

            var componentService = new ComponentService(null, null);

            var config = new FundingStream
            {
                FundingStreamCode = "DSG",
                FundingStreamName = "Dedicated schools grant",
                FundingStreamNameWithinSentence = "dedicated schools grant",
                FundingStreamCodePubliclyKnown = true
            };

            var generator = new FundingViewDataGenerator(
                uiModel,
                "FY-2021",
                data,
                null,
                new DateTime(2010, 7, 4),
                null,
                config,
                null,
                null,
                true,
                true,
                null,
                null,
                null,
                componentService,
                new ComponentConfigurationService(componentService, GetBasePathService().Object),
                false,
                false,
                VarianceSelectionOption.NoComparison,
                false,
                false,
                true);

            var expectedFundingData = new FundingApiSearchFunding
            {
                TotalAmount = 12345678,
                FundingValue = @"{ ""fundingAmount"": 1234.56 }"
            };

            var expectedMeta = new ComponentConfiguration
            {
                AllDatasetsData = new List<List<IFundingApiSearch>>
                {
                    new List<IFundingApiSearch>
                    {
                        expectedFundingData
                    }
                },
                ProviderType = "LocalAuthority",
                ProviderSubType = "LocalAuthority",
                BasePath = "Base Path",
                UrlForLoggedInView = "Url For Logged In Provider Path",
                FundingStreamName = "Dedicated schools grant",
                FundingStreamNameWithinSentence = "dedicated schools grant",
                FundingPeriodCode = "FY-2021",
                FundingStreamCode = "DSG",
                Year1 = 2020,
                Year2 = 2021,
                OpeningDay = 1,
                OpeningMonth = 1,
                OpeningYear = 1,
                ShortFundingStreamNameHtml = "<abbr title=\"Dedicated schools grant\">DSG</abbr>",
                ExpandedFundingStreamNameHtml = "Dedicated schools grant (<abbr title=\"Dedicated schools grant\">DSG</abbr>)",
                FundingStreamPathPart = "dedicated-schools-grant",
                IsCurrentYear = true,
                YearType = YearTypeName.FinancialYear,
                YearTypeCode = YearTypeCode.FinancialYear,
                IsLatestOrFinalFundingForYear = true,
                NoNextPaymentForOrganisationText = "There are no more scheduled payments for this organisation.",
                NoNextPaymentForTheYearText = "There are no more scheduled payments for financial year 2020 to 2021.",
                PublishedDate = new DateTime(2010, 7, 4),
                ShowNameAbbreviation = true,
                LocalAuthorityName = "TESTLA",
                LocalAuthorityNameOverride = "TESTLA",
                Variables = new Dictionary<string, object>
                {
                    { "Var1", "HELLO" },
                    { "Var2", true },
                    { "Var3", 1234.56 },
                    { "Var4", true },
                    { "Var5", "YES" },
                    { "Var6", "!NO!" },
                    { "censusYear", 2020 },
                    { "R06Year1", 2019 },
                    { "R06Year2", 2020 }
                },
                FundingStreamCodeOrName = "DSG",
                AsOfMonth = "Sept",
                AsOfYear = "2020",
                ProviderName = "TESTLA",
                IsLocalAuthority = true,
                Data = expectedFundingData,
                ProviderOpenDate = DateTime.MinValue,
                ProviderOpenDateddMMMMyyyy = DateTime.MinValue.ToString("dd MMMM yyyy"),
                Year0 = 2019,
                OriginalModel = new UiModel
                {
                    Dataset = new List<UiModelDataset>
                    {
                        new UiModelDataset
                        {
                            DatasetName = "funding"
                        }
                    },
                    Groups = new List<UiModelGroup>
                    {
                        new UiModelGroup
                        {
                            Id = "SomeVarTest",
                            Expression = "@Var1"
                        },
                        new UiModelGroup
                        {
                            Id = "SomeVarTest2",
                            Expression = "@Var2"
                        },
                        new UiModelGroup
                        {
                            Id = "SomeVarTest3",
                            Expression = "@Var3"
                        },
                        new UiModelGroup
                        {
                            Id = "SomeVarTest4",
                            Expression = "@Var4"
                        },
                        new UiModelGroup
                        {
                            Id = "SomeVarTest5",
                            Expression = "@Var5"
                        },
                        new UiModelGroup
                        {
                            Id = "SomeVarTest6",
                            Expression = "@Var6"
                        },
                        new UiModelGroup
                        {
                            Id = "SomeVarTest7",
                            Expression = "@Var6=!NO!"
                        },
                        new UiModelGroup
                        {
                            Id = TestModelGroupId,
                            Selector = $"$..{TestJsonPropertyName}"
                        }
                    },
                    Variables = new List<UIModelVariable>
                    {
                        new UIModelVariable
                        {
                            Name = "Var1",
                            Value = "HELLO",
                            Evaluated = true
                        },
                        new UIModelVariable
                        {
                            Name = "Var2",
                            Expression = "1=1",
                            Evaluated = true
                        },
                        new UIModelVariable
                        {
                            Name = "Var3",
                            Selector = $"$..{TestJsonPropertyName}",
                            Evaluated = true
                        },
                        new UIModelVariable
                        {
                            Name = "Var4",
                            Expression = "@Var2=true",
                            Evaluated = true
                        },
                        new UIModelVariable
                        {
                            Name = "Var5",
                            Expression = "@Var4=true",
                            TrueResponse = "YES",
                            FalseResponse = "NO",
                            Evaluated = true
                        },
                        new UIModelVariable
                        {
                            Name = "Var6",
                            Expression = "@Var4=false",
                            TrueResponse = "!YES!",
                            FalseResponse = "!NO!",
                            Evaluated = true
                        }
                    }
                },
                DaysInYear = DateTime.Now.NumberOfDaysInYear(),
                StatusChangedDate = new DateTime(2019, 10, 1),
                StatusChangedDateUiFormatted = new DateTime(2019, 10, 1).ToDateDisplay(),
                VarianceSelectionOption = VarianceSelectionOption.NoComparison
            };

            var expected = new List<Component>
            {
                new Component(expectedMeta)
                {
                    Id = "SomeVarTest",
                    Values = new List<object>
                    {
                        "HELLO"
                    },
                    Title = string.Empty,
                    AlternativeTitle = string.Empty,
                    OriginalGroup = new UiModelGroup
                    {
                        Id = "SomeVarTest",
                        Expression = "@Var1"
                    },
                    VarianceValueFormatted = string.Empty
                },
                new Component(expectedMeta)
                {
                    Id = "SomeVarTest2",
                    Values = new List<object>
                    {
                        true
                    },
                    Title = string.Empty,
                    AlternativeTitle = string.Empty,
                    OriginalGroup = new UiModelGroup
                    {
                        Id = "SomeVarTest2",
                        Expression = "@Var2"
                    },
                    VarianceValueFormatted = string.Empty
                },
                new Component(expectedMeta)
                {
                    Id = "SomeVarTest3",
                    Values = new List<object>
                    {
                        1234.56
                    },
                    Title = string.Empty,
                    AlternativeTitle = string.Empty,
                    OriginalGroup = new UiModelGroup
                    {
                        Id = "SomeVarTest3",
                        Expression = "@Var3"
                    },
                    VarianceValueFormatted = string.Empty
                },
                new Component(expectedMeta)
                {
                    Id = "SomeVarTest4",
                    Values = new List<object>
                    {
                        true
                    },
                    Title = string.Empty,
                    AlternativeTitle = string.Empty,
                    OriginalGroup = new UiModelGroup
                    {
                        Id = "SomeVarTest4",
                        Expression = "@Var4"
                    },
                    VarianceValueFormatted = string.Empty
                },
                new Component(expectedMeta)
                {
                    Id = "SomeVarTest5",
                    Values = new List<object>
                    {
                        "YES"
                    },
                    Title = string.Empty,
                    AlternativeTitle = string.Empty,
                    OriginalGroup = new UiModelGroup
                    {
                        Id = "SomeVarTest5",
                        Expression = "@Var5"
                    },
                    VarianceValueFormatted = string.Empty
                },
                new Component(expectedMeta)
                {
                    Id = "SomeVarTest6",
                    Values = new List<object>
                    {
                        "!NO!"
                    },
                    Title = string.Empty,
                    AlternativeTitle = string.Empty,
                    OriginalGroup = new UiModelGroup
                    {
                        Id = "SomeVarTest6",
                        Expression = "@Var6"
                    },
                    VarianceValueFormatted = string.Empty
                },
                new Component(expectedMeta)
                {
                    Id = "SomeVarTest7",
                    Values = new List<object>
                    {
                        true
                    },
                    Title = string.Empty,
                    AlternativeTitle = string.Empty,
                    OriginalGroup = new UiModelGroup
                    {
                        Id = "SomeVarTest7",
                        Expression = "@Var6=!NO!"
                    },
                    VarianceValueFormatted = string.Empty
                },
                new Component(expectedMeta)
                {
                    Id = "test-funding-amount",
                    Values = new List<object>
                    {
                        1234.56
                    },
                    Title = string.Empty,
                    AlternativeTitle = string.Empty,
                    OriginalGroup = new UiModelGroup
                    {
                        Id = "test-funding-amount",
                        Selector = "$..fundingAmount"
                    },
                    VarianceValueFormatted = string.Empty
                }
            };

            // Act
            var actual = generator.Generate();

            // Assert
            actual.Components.Should().HaveCountGreaterThanOrEqualTo(expected.Count);
            actual.Components.Should().BeEquivalentTo(expected);
        }

        /// <summary>
        /// Generates from provider funding data returns correct result.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void Generate_FromProviderFundingData_ReturnsCorrectResult()
        {
            // Arrange
            var uiModel = new UiModel
            {
                Dataset = new List<UiModelDataset>
                {
                    new UiModelDataset
                    {
                        DatasetName = "providerfunding"
                    }
                },
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup
                    {
                        Id = TestModelGroupId,
                        Selector = $"$..{TestJsonPropertyName}"
                    }
                }
            };

            var data = new ProviderFundingApiSearchResponse
            {
                ProviderFunding = new List<FundingApiSearchProviderFunding>
                {
                    new FundingApiSearchProviderFunding
                    {
                        OrganisationName = TestEntityName,
                        TotalAmount = Convert.ToDouble(TestFundingTotal),
                        StatusChangedDate = TestExternalPublicationDate,
                        FundingValue = TestFundingValue
                    }
                }
            };

            var componentService = new ComponentService(null, null);

            var config = new FundingStream
            {
                FundingStreamCode = "DSG",
                FundingStreamName = "Dedicated schools grant",
            };

            var generator = new FundingViewDataGenerator(
                uiModel,
                "FY-2021",
                null,
                data,
                DateTime.Now,
                null,
                config,
                null,
                null,
                true,
                true,
                null,
                null,
                null,
                componentService,
                new ComponentConfigurationService(componentService, GetBasePathService().Object),
                false,
                false,
                VarianceSelectionOption.NoComparison,
                false,
                false,
                true);

            // Act
            var actual = generator.Generate();

            // Assert
            actual.Should().NotBeNull();
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
