using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    /// <summary>
    /// The QueryFilterHelperTests class.
    /// </summary>
    [TestClass]
    public class QueryFilterHelperTests
    {
        #region Tests

        /// <summary>
        /// Builds the query filter should match expected.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="expected">The expected.</param>
        [TestMethod, TestCategory("Unit")]
        [DynamicData(nameof(BuildQueryFilterShouldMatchExpectedSource))]
        public void BuildQueryFilter_ShouldMatchExpected(IEnumerable<IFundingApiSearchProviderFunding> input, QueryFilter expected)
        {
            // Act
            var actual = QueryFilterHelper.BuildQueryFilter(input.ToList());

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        /// <summary>
        /// Filters the search results should match expected.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="appliedFilter">The applied filter.</param>
        /// <param name="expected">The expected.</param>
        [TestMethod, TestCategory("Unit")]
        [DynamicData(nameof(FilterSearchResultsShouldMatchExpectedSource))]
        public void FilterSearchResults_ShouldMatchExpected(IEnumerable<IFundingApiSearchProviderFunding> input, QueryFilter appliedFilter, IEnumerable<IFundingApiSearchProviderFunding> expected)
        {
            // Act
            var actual = QueryFilterHelper.FilterSearchResults(appliedFilter, input.ToList());

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        #endregion


        #region Mock Data Member Helpers

        /// <summary>
        /// Gets the build query filter should match expected source.
        /// </summary>
        /// <value>
        /// The build query filter should match expected source.
        /// </value>
        private static IEnumerable<object[]> BuildQueryFilterShouldMatchExpectedSource =>
            new List<object[]>
            {
                new object[]
                {
                    GetMockProviderResults(10),
                    GetAppliedQueryFilter(10)
                },
                new object[]
                {
                    GetMockProviderResults(8),
                    GetAppliedQueryFilter(8)
                }
            };

        /// <summary>
        /// Gets the filter search results should match expected source.
        /// </summary>
        /// <value>
        /// The filter search results should match expected source.
        /// </value>
        private static IEnumerable<object[]> FilterSearchResultsShouldMatchExpectedSource =>
            new List<object[]>
            {
                new object[]
                {
                    GetMockProviderResults(10, 2),
                    GetAppliedQueryFilter(),
                    GetMockProviderResults(2)
                },
                new object[]
                {
                    GetMockProviderResults(8, establishmentMaxNameCount: 3),
                    GetAppliedQueryFilter(),
                    GetMockProviderResults(3)
                }
            };

        #endregion


        #region Mock Data Helpers

        private static IEnumerable<IFundingApiSearchProviderFunding> GetMockProviderResults(
            int resultCount,
            int laMaxNameCount = 100,
            int establishmentMaxNameCount = 100)
        {
            return Enumerable.Range(0, resultCount).Select(provider =>
                new FundingApiSearchProviderFunding
                {
                    OrganisationName = nameof(FundingApiSearchProviderFunding.OrganisationName),
                    ParentName = nameof(FundingApiSearchProviderFunding.ParentName),
                    ParentPrimaryIdentifier = laMaxNameCount > provider ? nameof(FundingApiSearchProviderFunding.ParentPrimaryIdentifier) : string.Empty,
                    ParentProviderType = GroupingType.LocalAuthority,
                    ProviderType = establishmentMaxNameCount > provider ? ProviderTypeExternal.Academy : ProviderTypeExternal.LocalAuthorityMaintainedSchool
                });
        }

        private static QueryFilter GetAppliedQueryFilter(int count = 1)
        {
            return new QueryFilter
            {
                Filters = new List<SearchResultsFilter>
                {
                    new SearchResultsFilter
                    {
                        Key = SearchFilterConstants.LocalAuthorityFilterKey,
                        Open = true,
                        SearchEnabled = true,
                        Title = SearchFilterConstants.LocalAuthorityFilterTitle,
                        Values = new List<SearchFilterValue>
                        {
                            new SearchFilterValue
                            {
                                Count = count,
                                Value = nameof(FundingApiSearchProviderFunding.ParentPrimaryIdentifier),
                                Title = nameof(FundingApiSearchProviderFunding.ParentName),
                                ParentKey = SearchFilterConstants.LocalAuthorityFilterKey
                            }
                        }
                    },
                    new SearchResultsFilter
                    {
                        Key = SearchFilterConstants.EstablishmentTypeFilterKey,
                        Open = true,
                        Title = SearchFilterConstants.EstablishmentTypeFilterTitle,
                        Values = new List<SearchFilterValue>
                        {
                            new SearchFilterValue
                            {
                                Count = count,
                                Value = ProviderTypeInternal.Academy,
                                Title = ProviderTypeInternal.Academy,
                                ParentKey = SearchFilterConstants.EstablishmentTypeFilterKey
                            }
                        }
                    }
                }
            };
        }

        #endregion
    }
}
