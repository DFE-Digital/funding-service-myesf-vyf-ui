using Microsoft.AspNetCore.Mvc;
using PDS.ViewYourFunding.Services.Binders;
using PDS.ViewYourFunding.Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// The query filter class.
    /// </summary>
    [ModelBinder(BinderType = typeof(QueryFilterBinder))]
    public class QueryFilter
    {
        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        public List<SearchResultsFilter> Filters { get; set; } = new List<SearchResultsFilter>();

        /// <summary>
        /// Merges the specified selected values with the current query filter values.
        /// </summary>
        /// <param name="selectedValues">The selected values.</param>
        public void Merge(QueryFilter selectedValues)
        {
            if (selectedValues == null)
            {
                // open the first filter when used the first time.
                Filters.First().Open = true;
                return;
            }

            //for each filter in the other queryFilter
            foreach (var filter in selectedValues.Filters)
            {
                //Find a filter that has the same key as the other QueryFilters filter from our end and assign that to matchingFilter
                var matchingFilter = Filters.FirstOrDefault(o => o.Key.Equals(filter.Key, StringComparison.InvariantCultureIgnoreCase));

                if (matchingFilter != null)
                {
                    //our filter open equals what ever their filter open equals.
                    matchingFilter.Open = filter.Open;

                    //iterate through all of the filter values inside the other filter
                    foreach (var filterValue in filter.Values)
                    {
                        //When two values are found on both filters, we turn selected in to true in this end.
                        var match = matchingFilter.Values.FirstOrDefault(o => o.Value.RemoveWhitespace().Equals(filterValue.Value.RemoveWhitespace(), StringComparison.InvariantCultureIgnoreCase));
                        if (match != null)
                        {
                            matchingFilter.Open = true;
                            match.Selected = true;
                        }
                    }
                }
            }
        }
    }
}
