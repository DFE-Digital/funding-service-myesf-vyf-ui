using Microsoft.AspNetCore.Mvc.ModelBinding;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Binders
{
    /// <summary>
    /// Binds the model as per Filter values chosen on Layout Management Page.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder" />
    public class FilterTypeBinder : IModelBinder
    {
        private const string PostAction = WebRequestMethods.Http.Post;
        private const string OpenState = ".Open.";

        /// <inheritdoc />
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var request = bindingContext.HttpContext.Request;
            var filterType = new LayoutFilter()
            {
                FundingStreams = new List<Filter>(),
                FundingViewTypes = new List<Filter>(),
                FundingViewScopes = new List<Filter>(),
                PageNumber = 1
            };

            if (!request.Method.ToUpper().Equals(PostAction))
            {
                bindingContext.Result = ModelBindingResult.Success(filterType);
                return Task.CompletedTask;
            }

            var requestValues = request.Form;


            foreach (var key in requestValues.Keys.Where(o => o.StartsWith("Model.FilterType.FundingStreams") && !o.Contains(OpenState)))
            {
                requestValues.TryGetValue(key, out var fundingStreams);
                foreach (var item in fundingStreams.ToList())
                {
                    filterType.FundingStreams.Add(new Filter
                    {
                        Id = Convert.ToInt32(item),
                        Selected = true
                    });
                }
            }

            foreach (var key in requestValues.Keys.Where(o => o.StartsWith("Model.FilterType.FundingViewTypes") && !o.Contains(OpenState)))
            {
                requestValues.TryGetValue(key, out var fundingViewTypes);
                foreach (var item in fundingViewTypes.ToList())
                {
                    filterType.FundingViewTypes.Add(new Filter
                    {
                        Id = Convert.ToInt32(item),
                        Selected = true
                    });
                }
            }

            foreach (var key in requestValues.Keys.Where(o => o.StartsWith("Model.FilterType.FundingViewScopes") && !o.Contains(OpenState)))
            {
                requestValues.TryGetValue(key, out var fundingViewScopes);
                foreach (var item in fundingViewScopes.ToList())
                {
                    filterType.FundingViewScopes.Add(new Filter
                    {
                        Id = Convert.ToInt32(item),
                        Selected = true
                    });
                }
            }


            var pageNumberKey = requestValues.Keys.FirstOrDefault(o => o.StartsWith("Pagination.CurrentPage") && !o.Contains(OpenState));

            if (pageNumberKey != null)
            {
                requestValues.TryGetValue(pageNumberKey, out var pageNumber);

                int.TryParse(pageNumber, out int pageNo);

                filterType.PageNumber = pageNo;
            }


            var paginationNext = requestValues.Keys.FirstOrDefault(o => o.StartsWith("Pagination.Next") && !o.Contains(OpenState));

            if (paginationNext != null)
            {
                filterType.PageNumber = filterType.PageNumber + 1;
            }

            var paginationPrevious = requestValues.Keys.FirstOrDefault(o => o.StartsWith("Pagination.Previous") && !o.Contains(OpenState));

            if (paginationPrevious != null)
            {
                filterType.PageNumber = filterType.PageNumber - 1;
            }

            var updateFilters = requestValues.Keys.FirstOrDefault(o => o.StartsWith("Model.FilterType.UpdateFilters") && !o.Contains(OpenState));

            if (updateFilters != null)
            {
                filterType.PageNumber = 1;
            }

            var requestedPage = requestValues.Keys.FirstOrDefault(o => o.StartsWith("Pagination.PageNumber") && !o.Contains(OpenState));
            if (requestedPage != null)
            {
                requestValues.TryGetValue(requestedPage, out var pageNumber);

                int.TryParse(pageNumber, out int pageNo);

                filterType.PageNumber = pageNo;
            }

            bindingContext.Result = ModelBindingResult.Success(filterType);
            return Task.CompletedTask;
        }
    }
}