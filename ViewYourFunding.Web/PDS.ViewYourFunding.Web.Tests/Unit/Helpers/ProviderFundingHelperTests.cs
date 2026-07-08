using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Helpers;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Helpers
{
    [TestClass, TestCategory("Unit")]
    public class ProviderFundingHelperTests
    {
        [TestMethod]
        public void GroupFundingsByYear_OneIndicativeStatement_VariationReasonUpdatedCorrectly()
        {
            // Arrange
            var yearsToInclude = new List<(int yearFrom, int yearTo)>
            {
                (2020, 2021)
            };
            var providerFundings = new List<IFundingApiSearchProviderFunding>
            {
                new FundingApiSearchProviderFunding
                {
                    FundingPeriodCode = "AC-2021",
                    VariationReason = "Sample reason",
                    GroupingReason = GroupingReason.Indicative,
                    StatusChangedDate = new DateTime(2021, 04, 01)
                }
            };
            var fundingStream = new FundingStream
            {
                SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                            SettingId = 1,
                            CreatedAt = new DateTime(2020, 10, 1),
                            Value = "202122",
                            Setting = new SettingType
                            {
                                SettingName = "AcademyAcademicYear",
                                SettingDescription = "SettingDescription1",
                                ValueDataType = SettingValueDataType.String,
                                ValuesAreEditable = false,
                            }
                        }
                    }
            };

            var expected = new List<KeyValuePair<(int yearFrom, int yearTo), List<IFundingApiSearchProviderFunding>>>
            {
                new KeyValuePair<(int yearFrom, int yearTo), List<IFundingApiSearchProviderFunding>>((2020, 2021), new List<IFundingApiSearchProviderFunding>
            {
                new FundingApiSearchProviderFunding
                {
                    FundingPeriodCode = "AC-2021",
                    VariationReason = "Indicative allocation.",
                    GroupingReason = GroupingReason.Indicative,
                    StatusChangedDate = new DateTime(2021, 04, 01)
                }
            })
            };

            // Act
            var act = ProviderFundingHelper.GroupFundingsByYear(yearsToInclude, providerFundings, fundingStream);

            // Assert
            act.Should().BeOfType<List<KeyValuePair<(int yearFrom, int yearTo), List<IFundingApiSearchProviderFunding>>>>().And.BeEquivalentTo(expected);
        }

        [TestMethod]
        public void GroupFundingsByYear_TwoIndicativeStatements_VariationReasonsUpdatedCorrectly()
        {
            // Arrange
            var yearsToInclude = new List<(int yearFrom, int yearTo)>
            {
                (2020, 2021)
            };
            var providerFundings = new List<IFundingApiSearchProviderFunding>
            {
                new FundingApiSearchProviderFunding
                {
                    FundingPeriodCode = "AC-2021",
                    VariationReason = "Sample reason",
                    GroupingReason = GroupingReason.Indicative,
                    StatusChangedDate = new DateTime(2021, 04, 01)
                },
                new FundingApiSearchProviderFunding
                {
                    FundingPeriodCode = "AC-2021",
                    VariationReason = "Sample reason",
                    GroupingReason = GroupingReason.Indicative,
                    StatusChangedDate = new DateTime(2021, 05, 01)
                }
            };
            var fundingStream = new FundingStream
            {
                SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                            SettingId = 1,
                            CreatedAt = new DateTime(2020, 10, 1),
                            Value = "202122",
                            Setting = new SettingType
                            {
                                SettingName = "AcademyAcademicYear",
                                SettingDescription = "SettingDescription1",
                                ValueDataType = SettingValueDataType.String,
                                ValuesAreEditable = false,
                            }
                        }
                    }
            };

            var expected = new List<KeyValuePair<(int yearFrom, int yearTo), List<IFundingApiSearchProviderFunding>>>
            {
                new KeyValuePair<(int yearFrom, int yearTo), List<IFundingApiSearchProviderFunding>>((2020, 2021), new List<IFundingApiSearchProviderFunding>
            {
                new FundingApiSearchProviderFunding
                {
                    FundingPeriodCode = "AC-2021",
                    VariationReason = "Revised indicative allocation.",
                    GroupingReason = GroupingReason.Indicative,
                    StatusChangedDate = new DateTime(2021, 05, 01)
                },
                new FundingApiSearchProviderFunding
                {
                    FundingPeriodCode = "AC-2021",
                    VariationReason = "Indicative allocation.",
                    GroupingReason = GroupingReason.Indicative,
                    StatusChangedDate = new DateTime(2021, 04, 01)
                }
            })
            };

            // Act
            var act = ProviderFundingHelper.GroupFundingsByYear(yearsToInclude, providerFundings, fundingStream);

            // Assert
            act.Should().BeOfType<List<KeyValuePair<(int yearFrom, int yearTo), List<IFundingApiSearchProviderFunding>>>>().And.BeEquivalentTo(expected);
        }
    }
}
