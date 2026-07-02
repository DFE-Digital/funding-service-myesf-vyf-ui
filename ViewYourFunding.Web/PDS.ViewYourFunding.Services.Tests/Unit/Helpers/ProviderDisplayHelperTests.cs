using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Helper;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    [TestClass]
    public class ProviderDisplayHelperTests
    {
        [TestMethod, TestCategory("Unit")]
        public void GetSearchResultDisplayName_AllDataAvailable_ShouldReturnSchoolNameWithTownAndPostcode()
        {
            // Arrange
            var organisationName = "A";
            var organisationTown = "B";
            var organisationPostcode = "C";

            // Act
            var actual = ProviderDisplayHelper.GetSearchResultDisplayName(organisationName, organisationTown, organisationPostcode);

            // Assert
            actual.Should().BeEquivalentTo("A (B, C)");
        }

        [TestMethod, TestCategory("Unit")]
        public void GetSearchResultDisplayName_NoPostcodeOrTown_ShouldReturnSchoolNameOnly()
        {
            // Arrange
            var organisationName = "A";
            var organisationTown = (string)null;
            var organisationPostcode = (string)null;

            // Act
            var actual = ProviderDisplayHelper.GetSearchResultDisplayName(organisationName, organisationTown, organisationPostcode);

            // Assert
            actual.Should().BeEquivalentTo("A");
        }

        [TestMethod, TestCategory("Unit")]
        public void GetSearchResultDisplayName_NoPostcode_ShouldReturnSchoolNameWithTownOnly()
        {
            // Arrange
            var organisationName = "A";
            var organisationTown = "B";
            var organisationPostcode = (string)null;

            // Act
            var actual = ProviderDisplayHelper.GetSearchResultDisplayName(organisationName, organisationTown, organisationPostcode);

            // Assert
            actual.Should().BeEquivalentTo("A (B)");
        }

        [TestMethod, TestCategory("Unit")]
        public void GetSearchResultDisplayName_NoTown_ShouldReturnSchoolNameWithPostcodeOnly()
        {
            // Arrange
            var organisationName = "A";
            var organisationTown = (string)null;
            var organisationPostcode = "C";

            // Act
            var actual = ProviderDisplayHelper.GetSearchResultDisplayName(organisationName, organisationTown, organisationPostcode);

            // Assert
            actual.Should().BeEquivalentTo("A (C)");
        }
    }
}