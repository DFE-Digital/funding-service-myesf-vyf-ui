using Microsoft.AspNetCore.Mvc.ModelBinding;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Binders
{
    /// <summary>
    /// The Query Filter Model binder.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder" />
    public class QueryFilterBinder : IModelBinder
    {
        private const string PostAction = WebRequestMethods.Http.Post;
        private const string QueryFilterPrefix = "QueryFilter.";
        private const string OpenState = ".Open.";

        /// <summary>
        /// Attempts to bind a model.
        /// </summary>
        /// <param name="bindingContext">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingContext" />.</param>
        /// <returns>
        /// <para>
        /// A <see cref="T:System.Threading.Tasks.Task" /> which will complete when the model binding process completes.
        /// </para>
        /// <para>
        /// If model binding was successful, the <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingContext.Result" /> should have
        /// <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingResult.IsModelSet" /> set to <c>true</c>.
        /// </para>
        /// <para>
        /// A model binder that completes successfully should set <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingContext.Result" /> to
        /// a value returned from <see cref="M:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingResult.Success(System.Object)" />.
        /// </para>
        /// </returns>
        /// <exception cref="ArgumentNullException">bindingContext.</exception>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var queryFilter = new QueryFilter();

            var request = bindingContext.HttpContext.Request;

            if (!request.Method.ToUpper().Equals(PostAction))
            {
                return Task.CompletedTask;
            }

            var requestValues = request.Form;
            foreach (var key in requestValues.Keys.Where(o => o.StartsWith(QueryFilterPrefix) && !o.Contains(OpenState)))
            {
                requestValues.TryGetValue(key, out var values);

                var filter = new SearchResultsFilter { Key = key.Split('.').Last(), Values = new List<SearchFilterValue>() };

                foreach (var item in values.ToList())
                {
                    filter.Values.Add(new SearchFilterValue
                    {
                        Value = item,
                        Selected = true
                    });
                }

                queryFilter.Filters.Add(filter);
            }

            foreach (var openSetting in requestValues.Keys.Where(o => o.StartsWith(QueryFilterPrefix) && o.Contains(OpenState)))
            {
                var filterKey = openSetting.Split('.').Last();
                var filter = queryFilter.Filters.FirstOrDefault(o => o.Key == filterKey);
                if (filter == null)
                {
                    filter = new SearchResultsFilter { Key = filterKey, Values = new List<SearchFilterValue>() };
                    queryFilter.Filters.Add(filter);
                }

                requestValues.TryGetValue(openSetting, out var openValue);
                filter.Open = openValue.FirstOrDefault()?.Equals(true.ToString(), StringComparison.InvariantCultureIgnoreCase) == true;
            }

            bindingContext.Result = ModelBindingResult.Success(queryFilter);
            return Task.CompletedTask;
        }
    }
}
