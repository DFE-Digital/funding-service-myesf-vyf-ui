using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Web.Areas.Admin.Binders;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Tests.Unit
{
    /// <summary>
    /// The FilterTypeBinderTests class.
    /// </summary>
    [TestClass]
    public class FilterTypeBinderTests
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
        [TestMethod, TestCategory("Unit")]
        [DynamicData(nameof(FilterTypeBinderExpectedModelSource))]
        public void FilterTypeBinder_ExpectedModel(Dictionary<string, string[]> input)
        {
            // arrange
            var filterTypeBinder = new FilterTypeBinder();
            _mockDefaultModelBindingContext = GetBindingContext(input);

            // act
            var result = filterTypeBinder.BindModelAsync(_mockDefaultModelBindingContext.Object);

            // assert
            result.IsCompletedSuccessfully.Should().BeTrue();
        }

        #endregion

        #region Mock Data Member Helpers

        /// <summary>
        /// Gets the query filter model binder expected model source.
        /// </summary>
        /// <value>
        /// The query filter model binder expected model source.
        /// </value>
        private static IEnumerable<object[]> FilterTypeBinderExpectedModelSource =>
            new List<object[]>
            {
                new object[]
                {
                    new Dictionary<string, string[]>
                    {
                        { "Model.FilterType.FundingStreams", new[] { "1" } },
                        { "Model.FilterType.FundingViewTypes", new[] { "0" } },
                        { "Model.FilterType.FundingViewScopes", new[] { "0" } }
                    }
                },
                new object[]
                {
                    new Dictionary<string, string[]>
                    {
                        { "Model.FilterType.FundingStreams", new[] { "2" } },
                        { "Model.FilterType.FundingViewTypes", new[] { "1" } },
                        { "Model.FilterType.FundingViewScopes", new[] { "0" } }
                    }
                }
            };

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