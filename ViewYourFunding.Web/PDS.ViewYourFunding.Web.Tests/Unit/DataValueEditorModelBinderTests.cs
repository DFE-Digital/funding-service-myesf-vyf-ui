using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Web.Areas.Admin.Binders;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit;
using System.Collections.Generic;
using System.Globalization;

namespace PDS.ViewYourFunding.Web.Tests.Unit
{
    [TestClass]
    public class DataValueEditorModelBinderTests
    {
        /// <summary>
        /// The mock default model binding context.
        /// </summary>
        private Mock<DefaultModelBindingContext> _mockDefaultModelBindingContext;

        #region Tests

        [TestMethod, TestCategory("Unit")]
        [DynamicData(nameof(GetDataValueEditorModelBinderExpectedModelSource))]
        public void BindModelAsync_ExpectedModel(Dictionary<string, StringValues> input, DataTypeBaseEdit expected)
        {
            // arrange
            var dataValueEditorBinder = new DataValueEditorBinder();
            _mockDefaultModelBindingContext = GetBindingContext(input, expected);

            // act
            var result = dataValueEditorBinder.BindModelAsync(_mockDefaultModelBindingContext.Object);

            // assert
            result.IsCompletedSuccessfully.Should().BeTrue();
            _mockDefaultModelBindingContext.Object.ModelMetadata.ModelType.Name.Should().BeEquivalentTo(expected.GetType().Name);
        }

        #endregion


        #region Mock Data Member Helpers

        /// <summary>
        /// Gets the get data value editor model binder expected model source.
        /// </summary>
        /// <value>
        /// The get data value editor model binder expected model source.
        /// </value>
        private static IEnumerable<object[]> GetDataValueEditorModelBinderExpectedModelSource =>
            new List<object[]>
            {
                new object[]
                {
                    GetFormData(nameof(SettingEditType.String), "test"),
                    new StringTypeEdit()
                },
                new object[]
                {
                    GetFormData(nameof(SettingEditType.Int), "2"),
                    new IntTypeEdit()
                }, new object[]
                {
                    GetFormData(nameof(SettingEditType.Time), "00:00"),
                    new TimeTypeEdit()
                }, new object[]
                {
                    GetFormData(nameof(SettingEditType.DateTime), "22/10/2000 12:20"),
                    new DateTimeTypeEdit()
                }, new object[]
                {
                    GetFormData(nameof(SettingEditType.Date), "12/10/2008"),
                    new DateTypeEdit()
                }, new object[]
                {
                    GetFormData(nameof(SettingEditType.Bool), "TRUE"),
                    new BoolTypeEdit()
                }
            };

        private static Dictionary<string, StringValues> GetFormData(string settingEditType, string currentValue)
        {
            return new Dictionary<string, StringValues>
            {
                { nameof(SettingEditType), new[] { settingEditType } },
                { nameof(DataTypeBaseEdit.DataTypeId), new[] { "1" } },
                { nameof(DataTypeBaseEdit.Description), new[] { nameof(DataTypeBaseEdit.Description) } },
                { nameof(DataTypeBaseEdit.CurrentValue), new[] { currentValue } }
            };
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
        private static HttpContext FakeHttpContext(Dictionary<string, StringValues> data)
        {
            var httpContext = new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();
            request.Setup(c => c.Form).Returns(new FormCollection(data));

            httpContext.Setup(x => x.Request).Returns(request.Object);
            httpContext.Setup(x => x.Request.Method).Returns("Post");
            return httpContext.Object;
        }

        /// <summary>
        /// Gets the binding context.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="dataTypeBaseEdit">The base class of data edit types.</param>
        /// <returns>The DefaultModelBindingContext.</returns>
        private static Mock<DefaultModelBindingContext> GetBindingContext(Dictionary<string, StringValues> data, DataTypeBaseEdit dataTypeBaseEdit)
        {
            var mockBindingContext = new Mock<DefaultModelBindingContext>();
            mockBindingContext.Setup(x => x.HttpContext).Returns(FakeHttpContext(data));
            mockBindingContext.Setup(x => x.ModelMetadata).Returns(GetTypeBaseEdit(dataTypeBaseEdit));
            mockBindingContext.Setup(x => x.ValidationState).Returns(new ValidationStateDictionary());
            return mockBindingContext;
        }

        private static ModelMetadata GetTypeBaseEdit(DataTypeBaseEdit dataTypeBaseEdit)
        {
            var empty = new EmptyModelMetadataProvider();
            var modelMetadata = empty.GetMetadataForType(dataTypeBaseEdit.GetType());
            return modelMetadata;
        }

        #endregion
    }
}