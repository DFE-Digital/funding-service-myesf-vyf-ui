using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using FundingViewData = PDS.ViewYourFunding.Services.DTOs.FundingViewData;
using ProviderTypeInternal = PDS.ViewYourFunding.Services.Constants.ProviderTypeInternal;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Helper class for Query Filter related methods.
    /// </summary>
    public static class QueryFilterHelper
    {
        /// <summary>
        /// Builds the query filter for the given funding view data.
        /// </summary>
        /// <param name="fundingViewData">The funding view data.</param>
        /// <returns>The built query filter.</returns>
        public static QueryFilter BuildQueryFilter(FundingViewData fundingViewData)
        {
            var establishmentFilterValues = fundingViewData?.FundingSubData?
                .Select(funding =>
                {
                    var providerType = ProviderTypeInternal.FromExternal(funding.ProviderType, funding.ProviderSubType);
                    return new KeyValuePair<string, string>(providerType, providerType);
                })
                .Where(filterValue => filterValue.Key != null)
                .ToList();

            var establishmentTypeFilter = BuildFilter(
                SearchFilterConstants.EstablishmentTypeFilterTitle,
                SearchFilterConstants.EstablishmentTypeFilterKey,
                false,
                establishmentFilterValues);

            return new QueryFilter
            {
                Filters = new List<SearchResultsFilter>
                {
                    establishmentTypeFilter
                }
            };
        }

        /// <summary>
        /// Builds the query filter.
        /// </summary>
        /// <param name="searchProviderFunding">The search provider funding list.</param>
        /// <returns>The Query filter based on the search provider funding list.</returns>
        public static QueryFilter BuildQueryFilter(List<IFundingApiSearchProviderFunding> searchProviderFunding)
        {
            var localAuthorityFilterValues = searchProviderFunding
                .Select(funding => new KeyValuePair<string, string>(funding.ParentPrimaryIdentifier, funding.ParentName))
                .ToList();

            var localAuthorityFilter = BuildFilter(
                SearchFilterConstants.LocalAuthorityFilterTitle,
                SearchFilterConstants.LocalAuthorityFilterKey,
                true,
                localAuthorityFilterValues);

            var establishmentFilterValues = searchProviderFunding
                .Select(funding =>
                {
                    var providerType = ProviderTypeInternal.FromExternal(funding.ProviderType, funding.ProviderSubType);
                    return new KeyValuePair<string, string>(providerType, providerType);
                })
                .Where(filterValue => filterValue.Key != null)
                .ToList();

            var establishmentFilter = BuildFilter(
                SearchFilterConstants.EstablishmentTypeFilterTitle,
                SearchFilterConstants.EstablishmentTypeFilterKey,
                false,
                establishmentFilterValues);

            return new QueryFilter
            {
                Filters = new List<SearchResultsFilter>
                {
                    localAuthorityFilter,
                    establishmentFilter
                }
            };
        }

        /// <summary>
        /// Filters the search results.
        /// </summary>
        /// <param name="selectedFilters">The selected filters.</param>
        /// <param name="viewModelProviderResults">The view model provider results.</param>
        /// <returns>A filtered search results list.</returns>
        public static List<IFundingApiSearchProviderFunding> FilterSearchResults(QueryFilter selectedFilters, List<IFundingApiSearchProviderFunding> viewModelProviderResults)
        {
            var localAuthorityFilter = selectedFilters.Filters.FirstOrDefault(filter =>
                filter.Key.Equals(
                    SearchFilterConstants.LocalAuthorityFilterKey,
                    StringComparison.CurrentCultureIgnoreCase));

            var establishmentTypeFilter = selectedFilters.Filters.FirstOrDefault(filter =>
                filter.Key.Equals(
                    SearchFilterConstants.EstablishmentTypeFilterKey,
                    StringComparison.CurrentCultureIgnoreCase));

            var filterResultSet = new List<IFundingApiSearchProviderFunding>();

            foreach (var funding in viewModelProviderResults)
            {
                var localAuthorityFilterIsSatisfied =
                    localAuthorityFilter == null ||
                    localAuthorityFilter.Values.Any(
                        searchFilterValue => searchFilterValue.Value.Equals(
                            funding.ParentPrimaryIdentifier));

                var establishmentTypeFilterIsSatisfied =
                    establishmentTypeFilter == null ||
                    establishmentTypeFilter.Values.Any(
                        searchFilterValue => searchFilterValue.Value.RemoveWhitespace().Equals(
                            ProviderTypeInternal.FromExternal(funding.ProviderType, funding.ProviderSubType).RemoveWhitespace()));

                if (localAuthorityFilterIsSatisfied && establishmentTypeFilterIsSatisfied)
                {
                    filterResultSet.Add(funding);
                }
            }

            viewModelProviderResults = filterResultSet;
            return viewModelProviderResults;
        }

        /// <summary>
        /// Applies the filters to the sub-data of the given funding view data.
        /// </summary>
        /// <param name="selectedFilters">The filters to apply.</param>
        /// <param name="fundingViewData">The funding view data.</param>
        /// <returns>The funding view data, with its sub-data filtered.</returns>
        public static FundingViewData ApplyFilters(QueryFilter selectedFilters, FundingViewData fundingViewData)
        {
            if (selectedFilters?.Filters?.Any() != true)
            {
                return fundingViewData;
            }

            var fundingSubData = fundingViewData.FundingSubData.ToList();

            foreach (var filter in selectedFilters.Filters)
            {
                fundingSubData = ApplyFilter(fundingSubData, filter);
            }

            fundingViewData.FundingSubData = fundingSubData;
            return fundingViewData;
        }

        /// <summary>
        /// Builds a filter out of the given parameters.
        /// </summary>
        /// <param name="filterTitle">The filter title.</param>
        /// <param name="filterKey">The filter key.</param>
        /// <param name="searchEnabled">Whether or not the filter should have a search box.</param>
        /// <param name="filterValues">A list of values for which to calculate the filters.</param>
        /// <returns>The built filter object.</returns>
        private static SearchResultsFilter BuildFilter(
            string filterTitle, string filterKey, bool searchEnabled, List<KeyValuePair<string, string>> filterValues)
        {
            return new SearchResultsFilter
            {
                Title = filterTitle,
                Key = filterKey,
                Open = true,
                SearchEnabled = searchEnabled,
                Values = filterValues?
                    .GroupBy(filterValue => filterValue.Key.RemoveWhitespace())
                    .Select(filterValueGroup => new SearchFilterValue
                    {
                        Title = filterValueGroup.First().Value,
                        Value = filterValueGroup.Key,
                        ParentKey = filterKey,
                        Count = filterValueGroup.Count()
                    })
                    .ToList()
            };
        }

        /// <summary>
        /// Filters the provided funding data using the given filter.
        /// </summary>
        /// <param name="fundingData">The funding data to filter.</param>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>The filtered list of funding.</returns>
        private static List<IFundingApiSearchProviderFunding> ApplyFilter(
            List<IFundingApiSearchProviderFunding> fundingData, SearchResultsFilter filter)
        {
            if (filter.Key.Equals(SearchFilterConstants.EstablishmentTypeFilterKey, StringComparison.CurrentCultureIgnoreCase))
            {
                var filterValueSelector = new Func<IFundingApiSearchProviderFunding, string>(funding => ProviderTypeInternal.FromExternal(funding.ProviderType, funding.ProviderSubType));
                return ApplyFilter(fundingData, filter, filterValueSelector);
            }

            return fundingData;
        }

        /// <summary>
        /// Filters the provided funding data using the given filter and selector function.
        /// </summary>
        /// <param name="fundingData">The funding data to filter.</param>
        /// <param name="filter">The filter to apply.</param>
        /// <param name="filterValueSelector">The function to determine the filter's value for the given funding.</param>
        /// <returns>The filtered list of funding.</returns>
        private static List<IFundingApiSearchProviderFunding> ApplyFilter(
            List<IFundingApiSearchProviderFunding> fundingData, SearchResultsFilter filter, Func<IFundingApiSearchProviderFunding, string> filterValueSelector)
        {
            if (filter?.Values?.Any() != true)
            {
                return fundingData;
            }

            return fundingData
                .Where(funding =>
                {
                    var fundingFilterValue = filterValueSelector(funding).RemoveWhitespace();
                    return filter.Values.Any(filterValue => filterValue.Value.Equals(fundingFilterValue));
                })
                .ToList();
        }
    }
}
