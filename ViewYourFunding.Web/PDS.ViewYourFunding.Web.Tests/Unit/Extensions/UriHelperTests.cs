using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Web.Helpers;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Extensions
{
    [TestClass]
    [TestCategory("Unit")]
    public class UriHelperTests
    {
        [TestMethod]
        [DataRow("domain:80", "id=1", "domain:80?id=1")]
        [DataRow("domain:8080", "id=1", "domain:8080?id=1")]
        [DataRow("www.domain:8080", "id=1", "www.domain:8080?id=1")]
        [DataRow("https://www.domain:8080", "id=1", "https://www.domain:8080/?id=1")]
        [DataRow("http://www.domain:8080", "id=1", "http://www.domain:8080/?id=1")]
        [DataRow("http://domain:8080", "id=1", "http://domain:8080/?id=1")]
        [DataRow("http://domain:8080?id=1", "parameter=1", "http://domain:8080/?id=1&parameter=1")]
        [DataRow("http://domain:8080?id=1", "parameter1=1&parameter2=1", "http://domain:8080/?id=1&parameter1=1&parameter2=1")]
        [DataRow("http://domain:8080?id=1&parameter1=1", "parameter2=1", "http://domain:8080/?id=1&parameter1=1&parameter2=1")]
        [DataRow("http://domain?id=1", "parameter=1", "http://domain/?id=1&parameter=1")]
        [DataRow("http://domain:8080", "id=1", "http://domain:8080/?id=1")]
        public void BuildUri_ReturnsExpectedUri(string uri, string parameters, string expectedUri)
        {
            // Arrange / Act
            var actual = UriHelper.BuildUri(uri, parameters);

            // Assert
            actual.Should().Be(expectedUri);
        }
    }
}