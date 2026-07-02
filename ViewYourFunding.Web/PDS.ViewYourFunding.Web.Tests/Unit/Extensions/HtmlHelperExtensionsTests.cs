using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Web.Helpers;
using System.Text.Encodings.Web;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Extensions
{
    /// <summary>
    /// The HtmlHelperExtensionsTests class.
    /// </summary>
    [TestClass]
    public class HtmlHelperExtensionsTests
    {
        /// <summary>
        /// Datas the attribute returns correct value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expectedValue">The expected value.</param>
        [TestMethod, TestCategory("Unit")]
        [DataRow("a", "b", "data-a=\"b\"")]
        [DataRow("A", "b", "data-a=\"b\"")]
        [DataRow("a", "B", "data-a=\"B\"")]
        [DataRow("A", "B", "data-a=\"B\"")]
        [DataRow("test-DaTa-KEY", "TEST-DATA-Value", "data-test-data-key=\"TEST-DATA-Value\"")]
        public void DataAttribute_ReturnsCorrectValue(string key, string value, string expectedValue)
        {
            // Arrange
            var htmlHelper = new HtmlHelper(new Mock<IHtmlGenerator>().Object, new Mock<ICompositeViewEngine>().Object, new Mock<IModelMetadataProvider>().Object, new Mock<IViewBufferScope>().Object, HtmlEncoder.Default, UrlEncoder.Default);

            //// Act
            var actual = htmlHelper.DataAttribute(key, value);

            //// Assert
            actual.ToString().Should().Be(expectedValue);
        }
    }
}
