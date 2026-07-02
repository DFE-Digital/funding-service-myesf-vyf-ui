using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Dataset Provider list Helper.
    /// </summary>
    public static class DatasetResultListHelper
    {
        /// <summary>
        /// Refines the based on dataset.
        /// </summary>
        /// <param name="relevantFundingsQuery">The relevant fundings query.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="providerFundingType">Type of the provider funding.</param>
        /// <returns>The refined List.</returns>
        public static List<IFundingApiSearchProviderFunding> RefineBasedOnDataset(
            this IEnumerable<IFundingApiSearchProviderFunding> relevantFundingsQuery,
            UiModelDataset dataset,
            Type providerFundingType)
        {
            List<IFundingApiSearchProviderFunding> relevantProviderFunding;
            if (!string.IsNullOrEmpty(dataset.OrderBy))
            {
                var property = PropertyInfoHelper.GetProperty(providerFundingType, dataset.OrderBy);
                if (!string.IsNullOrEmpty(dataset.ThenBy))
                {
                    var propertyThenBy = PropertyInfoHelper.GetProperty(providerFundingType, dataset.ThenBy);

                    relevantProviderFunding = relevantFundingsQuery
                        .OrderBy(funding => property.GetValue(funding, null))
                        .ThenBy(funding => propertyThenBy.GetValue(funding, null))
                        .Take(dataset.Take ?? int.MaxValue).ToList();
                }
                else
                {
                    relevantProviderFunding = relevantFundingsQuery
                        .OrderBy(funding => property.GetValue(funding, null))
                        .Take(dataset.Take ?? int.MaxValue).ToList();
                }
            }
            else if (!string.IsNullOrEmpty(dataset.OrderByDescending))
            {
                var property = PropertyInfoHelper.GetProperty(providerFundingType, dataset.OrderByDescending);

                if (!string.IsNullOrEmpty(dataset.ThenBy))
                {
                    var propertyThenBy = PropertyInfoHelper.GetProperty(providerFundingType, dataset.ThenBy);

                    relevantProviderFunding = relevantFundingsQuery
                        .OrderByDescending(funding => property.GetValue(funding, null))
                        .ThenBy(funding => propertyThenBy.GetValue(funding, null))
                        .Take(dataset.Take ?? int.MaxValue).ToList();
                }
                else
                {
                    relevantProviderFunding = relevantFundingsQuery
                        .OrderByDescending(funding => property.GetValue(funding, null))
                        .Take(dataset.Take ?? int.MaxValue).ToList();
                }
            }
            else
            {
                relevantProviderFunding = relevantFundingsQuery
                    .Take(dataset.Take ?? int.MaxValue).ToList();
            }

            if (dataset.GroupByOrganisationUkprn)
            {
                var providerFundingGroups = relevantProviderFunding
                    .OrderByDescending(provFunding => provFunding.StatusChangedDate)
                    .GroupBy(provFunding => new { provFunding.OrganisationUkprn }).ToList();

                relevantProviderFunding = providerFundingGroups
                    .Select(providerFundingGroup => providerFundingGroup.First())
                    .ToList();
            }

            if (dataset.PrependRows != null)
            {
                var relevantFundingTemp = dataset.PrependRows.Select(prependRow => (IFundingApiSearchProviderFunding)new FundingApiSearchProviderFunding()).ToList();
                relevantFundingTemp.AddRange(relevantProviderFunding);

                relevantProviderFunding = relevantFundingTemp;
            }

            if (dataset.AppendRows != null)
            {
                relevantProviderFunding.AddRange(dataset.AppendRows.Select(prependRow => (IFundingApiSearchProviderFunding)new FundingApiSearchProviderFunding()).ToList());
            }

            return relevantProviderFunding;
        }

        /// <summary>
        /// Refines the based on dataset.
        /// </summary>
        /// <param name="relevantFundingsQuery">The relevant fundings query.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="fundingType">Type of the funding.</param>
        /// <returns>he refined List.</returns>
        public static List<IFundingApiSearchFunding> RefineBasedOnDataset(
            this IEnumerable<IFundingApiSearchFunding> relevantFundingsQuery,
            UiModelDataset dataset,
            Type fundingType)
        {
            List<IFundingApiSearchFunding> relevantFunding;

            if (!string.IsNullOrEmpty(dataset.OrderBy))
            {
                var property = PropertyInfoHelper.GetProperty(fundingType, dataset.OrderBy);
                if (!string.IsNullOrEmpty(dataset.ThenBy))
                {
                    var propertyThenBy = PropertyInfoHelper.GetProperty(fundingType, dataset.ThenBy);

                    relevantFunding = relevantFundingsQuery
                        .OrderBy(funding => property.GetValue(funding, null))
                        .ThenBy(funding => propertyThenBy.GetValue(funding, null))
                        .Take(dataset.Take ?? int.MaxValue).ToList();
                }
                else
                {
                    relevantFunding = relevantFundingsQuery
                        .OrderBy(funding => property.GetValue(funding, null))
                        .Take(dataset.Take ?? int.MaxValue).ToList();
                }
            }
            else if (!string.IsNullOrEmpty(dataset.OrderByDescending))
            {
                var property = PropertyInfoHelper.GetProperty(fundingType, dataset.OrderByDescending);

                if (!string.IsNullOrEmpty(dataset.ThenBy))
                {
                    var propertyThenBy = PropertyInfoHelper.GetProperty(fundingType, dataset.ThenBy);

                    relevantFunding = relevantFundingsQuery
                        .OrderByDescending(funding => property.GetValue(funding, null))
                        .ThenBy(funding => propertyThenBy.GetValue(funding, null))
                        .Take(dataset.Take ?? int.MaxValue).ToList();
                }
                else
                {
                    relevantFunding = relevantFundingsQuery
                        .OrderByDescending(funding => property.GetValue(funding, null))
                        .Take(dataset.Take ?? int.MaxValue).ToList();
                }
            }
            else
            {
                relevantFunding = relevantFundingsQuery
                    .Take(dataset.Take ?? int.MaxValue).ToList();
            }

            if (dataset.GroupByOrganisationUkprn)
            {
                var fundingGroups = relevantFunding
                    .OrderByDescending(provFunding => provFunding.StatusChangedDate)
                    .GroupBy(provFunding => new { provFunding.GroupUkprn }).ToList();

                relevantFunding = fundingGroups
                    .Select(providerFundingGroup => providerFundingGroup.First())
                    .ToList();
            }

            if (dataset.PrependRows != null)
            {
                var relevantFundingTemp =
                    dataset.PrependRows.Select(prependRow => (IFundingApiSearchFunding)prependRow).ToList(); // Shallow copy
                relevantFundingTemp.AddRange(relevantFunding);

                relevantFunding = relevantFundingTemp;
            }

            if (dataset.AppendRows != null)
            {
                relevantFunding.AddRange(dataset.AppendRows);
            }

            return relevantFunding;
        }
    }
}