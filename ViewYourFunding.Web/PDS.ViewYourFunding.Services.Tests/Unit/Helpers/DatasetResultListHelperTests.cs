using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    [TestClass, TestCategory("Unit")]
    public class DatasetResultListHelperTests
    {
        [TestMethod, TestCategory("Unit")]
        [DynamicData(nameof(RefineBasedOnDatasetFundingSource), DynamicDataSourceType.Method)]
        public void RefineBasedOnDataset_Funding_ReturnsExpectedResult(
            IEnumerable<FundingApiSearchFunding> input,
            UiModelDataset dataset,
            Type fundingType,
            IList<FundingApiSearchFunding> expectedValue)
        {
            // Arrange/Act
            var result = input.RefineBasedOnDataset(dataset, fundingType);

            // Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        [TestMethod]
        [DynamicData(nameof(RefineBasedOnDatasetProviderFundingSource), DynamicDataSourceType.Method)]
        public void RefineBasedOnDataset_ProviderFunding_ReturnsExpectedResult(
            IEnumerable<FundingApiSearchProviderFunding> input,
            UiModelDataset dataset,
            Type fundingType,
            IList<FundingApiSearchProviderFunding> expectedValue)
        {
            // Arrange/Act
            var result = input.RefineBasedOnDataset(dataset, fundingType);

            // Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        private static IEnumerable<object[]> RefineBasedOnDatasetProviderFundingSource()
            =>
            new List<object[]>
            {
                new object[]
                {
                    FundingApiSearchProviderFundings,
                    new UiModelDataset
                    {
                        OrderBy = nameof(FundingApiSearchProviderFunding.OrganisationName),
                        GroupByOrganisationUkprn = true
                    },
                    typeof(IFundingApiSearchProviderFunding),
                    FundingApiSearchProviderFundings.Where(provider => provider.FundingVersion.Equals("2")).ToList()
                },
                new object[]
                {
                    FundingApiSearchProviderFundings,
                    new UiModelDataset
                    {
                        OrderBy = nameof(FundingApiSearchProviderFunding.OrganisationName),
                        ThenBy = nameof(IFundingApiSearchProviderFunding.FundingVersion),
                        GroupByOrganisationUkprn = true
                    },
                    typeof(IFundingApiSearchProviderFunding),
                    FundingApiSearchProviderFundings
                        .Where(provider => provider.FundingVersion.Equals("2")).ToList()
                },
                new object[]
                {
                    FundingApiSearchProviderFundings,
                    new UiModelDataset
                    {
                        OrderByDescending = nameof(FundingApiSearchProviderFunding.OrganisationName),
                    },
                    typeof(IFundingApiSearchProviderFunding),
                    FundingApiSearchProviderFundings
                        .OrderByDescending(prov => prov.OrganisationName)
                        .ToList()
                },
                new object[]
                {
                    FundingApiSearchProviderFundings,
                    new UiModelDataset
                    {
                        OrderByDescending = nameof(FundingApiSearchProviderFunding.OrganisationName),
                        ThenBy = nameof(FundingApiSearchProviderFunding.FundingVersion)
                    },
                    typeof(IFundingApiSearchProviderFunding),
                    FundingApiSearchProviderFundings
                        .OrderByDescending(prov => prov.OrganisationName)
                        .ThenBy(prov => prov.FundingVersion)
                        .ToList()
                },
                new object[]
                {
                    FundingApiSearchProviderFundings,
                    new UiModelDataset
                    {
                        PrependRows = new List<UiModelFundingApiSearchFunding>(),
                        AppendRows = new List<UiModelFundingApiSearchFunding>()
                    },
                    typeof(IFundingApiSearchProviderFunding),
                    FundingApiSearchProviderFundings
                },
            };

        private static IEnumerable<object[]> RefineBasedOnDatasetFundingSource() =>
            new List<object[]>
            {
                new object[]
                {
                    FundingApiSearchFundings,
                    new UiModelDataset
                    {
                        PrependRows = new List<UiModelFundingApiSearchFunding>(),
                        AppendRows = new List<UiModelFundingApiSearchFunding>()
                    },
                    typeof(IFundingApiSearchFunding),
                    FundingApiSearchFundings
                },
                new object[]
                {
                    FundingApiSearchFundings,
                    new UiModelDataset
                    {
                        OrderBy = nameof(FundingApiSearchFunding.GroupName),
                        ThenBy = nameof(IFundingApiSearchFunding.FundingVersion),
                        GroupByOrganisationUkprn = true
                    },
                    typeof(IFundingApiSearchFunding),
                    FundingApiSearchFundings
                        .Where(provider => provider.FundingVersion.Equals("2"))
                        .ToList()
                },
                new object[]
                {
                    FundingApiSearchFundings,
                    new UiModelDataset
                    {
                        OrderBy = nameof(FundingApiSearchFunding.GroupName),
                        GroupByOrganisationUkprn = true
                    },
                    typeof(IFundingApiSearchFunding),
                    FundingApiSearchFundings
                        .Where(provider => provider.FundingVersion.Equals("2"))
                        .ToList()
                },
                new object[]
                {
                    FundingApiSearchFundings,
                    new UiModelDataset
                    {
                        OrderByDescending = nameof(FundingApiSearchFunding.GroupName),
                        ThenBy = nameof(FundingApiSearchFunding.FundingVersion)
                    },
                    typeof(IFundingApiSearchFunding),
                    FundingApiSearchFundings
                        .OrderByDescending(funding => funding.GroupName)
                        .ThenBy(prov => prov.FundingVersion)
                        .ToList()
                },
                new object[]
                {
                    FundingApiSearchFundings,
                    new UiModelDataset
                    {
                        OrderByDescending = nameof(FundingApiSearchFunding.GroupName)
                    },
                    typeof(IFundingApiSearchFunding),
                    FundingApiSearchFundings
                        .OrderByDescending(funding => funding.GroupName)
                        .ToList()
                },
                new object[]
                {
                    FundingApiSearchFundings,
                    new UiModelDataset(),
                    typeof(IFundingApiSearchFunding),
                    FundingApiSearchFundings
                },
                new object[]
                {
                    FundingApiSearchFundings,
                    new UiModelDataset
                    {
                        OrderBy = nameof(FundingApiSearchFunding.GroupName),
                        ThenBy = nameof(FundingApiSearchFunding.FundingVersion)
                    },
                    typeof(IFundingApiSearchFunding),
                    FundingApiSearchFundings
                        .OrderBy(funding => funding.GroupName)
                        .ThenBy(prov => prov.FundingVersion)
                        .ToList()
                },
            };


        private static List<FundingApiSearchProviderFunding> FundingApiSearchProviderFundings
            => new List<FundingApiSearchProviderFunding>
            {
                new FundingApiSearchProviderFunding
                {
                    OrganisationUkprn = "1",
                    FundingVersion = "1",
                    OrganisationName = "Org 1",
                    StatusChangedDate = new DateTime(2021, 4, 12)
                }, new FundingApiSearchProviderFunding
                {
                    OrganisationUkprn = "1",
                    FundingVersion = "2",
                    OrganisationName = "Org 1",
                    StatusChangedDate = new DateTime(2021, 4, 15)
                },
                new FundingApiSearchProviderFunding
                {
                    OrganisationUkprn = "2",
                    FundingVersion = "1",
                    OrganisationName = "Org 2",
                    StatusChangedDate = new DateTime(2021, 4, 12)
                },
                new FundingApiSearchProviderFunding
                {
                    OrganisationUkprn = "2",
                    FundingVersion = "2",
                    OrganisationName = "Org 2",
                    StatusChangedDate = new DateTime(2021, 4, 15)
                },
                new FundingApiSearchProviderFunding
                {
                    OrganisationUkprn = "3",
                    FundingVersion = "1",
                    OrganisationName = "Org 3",
                    StatusChangedDate = new DateTime(2021, 4, 12)
                },
                new FundingApiSearchProviderFunding
                {
                    OrganisationUkprn = "3",
                    FundingVersion = "2",
                    OrganisationName = "Org 3",
                    StatusChangedDate = new DateTime(2021, 4, 15)
                }
            };

        private static List<FundingApiSearchFunding> FundingApiSearchFundings
            => new List<FundingApiSearchFunding>
            {
                new FundingApiSearchFunding
                {
                    GroupUkprn = "1",
                    FundingVersion = "1",
                    GroupName = "Org 1",
                    StatusChangedDate = new DateTime(2021, 4, 12)
                }, new FundingApiSearchFunding
                {
                    GroupUkprn = "1",
                    FundingVersion = "2",
                    GroupName = "Org 1",
                    StatusChangedDate = new DateTime(2021, 4, 15)
                },
                new FundingApiSearchFunding
                {
                    GroupUkprn = "2",
                    FundingVersion = "1",
                    GroupName = "Org 2",
                    StatusChangedDate = new DateTime(2021, 4, 12)
                },
                new FundingApiSearchFunding
                {
                    GroupUkprn = "2",
                    FundingVersion = "2",
                    GroupName = "Org 2",
                    StatusChangedDate = new DateTime(2021, 4, 15)
                },
                new FundingApiSearchFunding
                {
                    GroupUkprn = "3",
                    FundingVersion = "1",
                    GroupName = "Org 3",
                    StatusChangedDate = new DateTime(2021, 4, 12)
                },
                new FundingApiSearchFunding
                {
                    GroupUkprn = "3",
                    FundingVersion = "2",
                    GroupName = "Org 3",
                    StatusChangedDate = new DateTime(2021, 4, 15)
                }
            };
    }
}