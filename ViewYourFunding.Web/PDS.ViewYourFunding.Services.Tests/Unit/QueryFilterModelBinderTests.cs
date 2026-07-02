using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Services.Binders;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    /// <summary>
    /// The QueryFilterModelBinderTests class.
    /// </summary>
    [TestClass]
    public class QueryFilterModelBinderTests
    {
        /// <summary>
        /// The mock default model binding context.
        /// </summary>
        private Mock<DefaultModelBindingContext> _mockDefaultModelBindingContext;

        #region Tests

        /// <summary>
        /// Queries the filter model binder expected model.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="expected">The expected.</param>
        [TestMethod, TestCategory("Unit")]
        [DynamicData(nameof(QueryFilterModelBinderExpectedModelSource))]
        public void QueryFilterModelBinder_ExpectedModel(Dictionary<string, string[]> input, QueryFilter expected)
        {
            // arrange
            var queryFilterModelBinder = new QueryFilterBinder();
            _mockDefaultModelBindingContext = GetBindingContext(input);

            // act
            var result = queryFilterModelBinder.BindModelAsync(_mockDefaultModelBindingContext.Object);

            // assert
            result.IsCompletedSuccessfully.Should().BeTrue();

            // _mockDefaultModelBindingContext.Object.Result.Model.Should().BeSameAs(expected);
        }

        #endregion


        #region Mock Data Member Helpers

        /// <summary>
        /// Gets the query filter model binder expected model source.
        /// </summary>
        /// <value>
        /// The query filter model binder expected model source.
        /// </value>
        private static IEnumerable<object[]> QueryFilterModelBinderExpectedModelSource =>
            new List<object[]>
            {
                new object[]
                {
                    new Dictionary<string, string[]>
                    {
                        { "QueryFilter.Filters.LocalAuthority", new[] { nameof(FundingApiSearchProviderFunding.ParentPrimaryIdentifier), nameof(FundingApiSearchProviderFunding.ParentPrimaryIdentifier) } },
                        { "QueryFilter.Filters.EstablishmentType", new[] { nameof(FundingApiSearchProviderFunding.ProviderType), nameof(FundingApiSearchProviderFunding.ProviderType) } }
                    }, GetAppliedQueryFilter(2)
                },
                new object[]
                {
                    new Dictionary<string, string[]>
                    {
                        { "QueryFilter.Filters.LocalAuthority", new[] { nameof(FundingApiSearchProviderFunding.ParentPrimaryIdentifier), nameof(FundingApiSearchProviderFunding.ParentPrimaryIdentifier), nameof(FundingApiSearchProviderFunding.ParentPrimaryIdentifier) } },
                        { "QueryFilter.Filters.EstablishmentType", new[] { nameof(FundingApiSearchProviderFunding.ProviderType), nameof(FundingApiSearchProviderFunding.ProviderType), nameof(FundingApiSearchProviderFunding.ProviderType) } }
                    },
                    GetAppliedQueryFilter(3)
                },
                new object[]
                {
                    new Dictionary<string, string[]>(),
                    new QueryFilter
                    {
                        Filters = new List<SearchResultsFilter>()
                    }
                }
            };

        #endregion


        #region Mock Data Helpers

        /// <summary>
        /// Gets the applied query filter.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>The QueryFilter.</returns>
        private static QueryFilter GetAppliedQueryFilter(int count)
        {
            return new QueryFilter
            {
                Filters = new List<SearchResultsFilter>
                {
                    new SearchResultsFilter
                    {
                        Key = SearchFilterConstants.LocalAuthorityFilterKey,
                        Values = GetAppliedLocalAuthorityQueryFilters(count)
                    },
                    new SearchResultsFilter
                    {
                        Key = SearchFilterConstants.EstablishmentTypeFilterKey,
                        Values = GetAppliedEstablishmentTypeQueryFilters(count)
                    }
                }
            };
        }

        /// <summary>
        /// Gets the applied local authority query filters.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>The SearchFilterValue list.</returns>
        private static List<SearchFilterValue> GetAppliedLocalAuthorityQueryFilters(int count)
        {
            return Enumerable.Range(0, count).Select(filter =>
                new SearchFilterValue
                {
                    Value = nameof(FundingApiSearchProviderFunding.ParentPrimaryIdentifier),
                    Selected = true
                })
            .ToList();
        }

        /// <summary>
        /// Gets the applied establishment type query filters.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>The SearchFilterValue list.</returns>
        private static List<SearchFilterValue> GetAppliedEstablishmentTypeQueryFilters(int count)
        {
            return Enumerable.Range(0, count).Select(filter =>
                new SearchFilterValue
                {
                    Value = nameof(FundingApiSearchProviderFunding.ProviderType),
                    Selected = true
                })
            .ToList();
        }
        #endregion


        #region Context Helpers

        /// <summary>
        /// Setups the controller and binding context.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The ModelBindingContext.</returns>
        private static ModelBindingContext SetupControllerAndBindingContext(Dictionary<string, StringValues> input)
        {
            var bindingContext = new DefaultModelBindingContext();

            var bindingSource = new BindingSource(string.Empty, string.Empty, false, false);

            bindingContext.ValueProvider = new FormValueProvider(bindingSource, new FormCollection(input), CultureInfo.DefaultThreadCurrentCulture);

            return bindingContext;
        }

        /// <summary>
        /// Fakes the HTTP context.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The HttpContext.</returns>
        private static HttpContext FakeHttpContext(Dictionary<string, string[]> data)
        {
            var httpContext = new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();

            // Convert string[] to StringValues for FormCollection
            var formData = data.ToDictionary(kvp => kvp.Key, kvp => new StringValues(kvp.Value));

            request.Setup(c => c.Form).Returns(new FormCollection(formData));
            httpContext.Setup(x => x.Request).Returns(request.Object);
            httpContext.Setup(x => x.Request.Method).Returns("Post");

            return httpContext.Object;
        }


        /// <summary>
        /// Gets the binding context.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The DefaultModelBindingContext.</returns>
        private Mock<DefaultModelBindingContext> GetBindingContext(Dictionary<string, string[]> data)
        {
            var mockBindingContext = new Mock<DefaultModelBindingContext>();
            mockBindingContext.Setup(x => x.HttpContext).Returns(FakeHttpContext(data));
            return mockBindingContext;
        }
        #endregion
    }
}
