using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Helper;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    /// <summary>
    /// The ProviderTypeSubTypeHelperTests class.
    /// </summary>
    [TestClass, TestCategory("Unit")]
    public class ProviderTypeSubTypeHelperTests
    {
        [TestMethod, TestCategory("Unit")]
        [DataRow("Schoo", true)]
        [DataRow(null, false)]
        [DataRow(ProviderTypeExternal.LocalAuthorityMaintainedSchool, true)]
        [DataRow(ProviderTypeExternal.SpecialSchool, true)]
        [DataRow("unknown", false)]
        public void IsSchoolProviderType_ReturnsExpectedValue(string providerType, bool expected)
        {
            // Act
            var actual = providerType.IsSchoolProviderType();

            // Assert
            actual.Should().Be(expected);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("Schoo", false)]
        [DataRow(null, false)]
        [DataRow(ProviderTypeExternal.StoreTypeAcademy, true)]
        [DataRow(ProviderTypeExternal.Academy, true)]
        [DataRow(ProviderTypeExternal.IndependentSchool, true)]
        [DataRow(ProviderTypeExternal.FreeSchool, true)]
        public void IsAcademyProviderType_ReturnsExpectedValue(string providerType, bool expected)
        {
            // Act
            var actual = providerType.IsAcademyProviderType();

            // Assert
            actual.Should().Be(expected);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(ProviderTypeExternal.University, true)]
        [DataRow(ProviderTypeExternal.SixteenToNineteen, true)]
        [DataRow(null, false)]
        [DataRow(ProviderTypeExternal.OtherType, true)]
        [DataRow(ProviderTypeExternal.StoreTypeFurtherEducation, true)]
        [DataRow(ProviderTypeExternal.College, true)]
        [DataRow("unknown", false)]
        public void IsFurtherEducationProviderType_ReturnsExpectedValue(string providerType, bool expected)
        {
            // Act
            var actual = providerType.IsFurtherEducationProviderType();

            // Assert
            actual.Should().Be(expected);
        }


        [TestMethod, TestCategory("Unit")]
        [DataRow("Schoo", false)]
        [DataRow(null, false)]
        [DataRow(ProviderTypeExternal.LocalAuthority, true)]
        [DataRow(ProviderTypeExternal.StoreTypeLocalAuthority, true)]
        [DataRow("unknown", false)]
        public void IsLocalAuthorityProviderType_ReturnsExpectedValue(string providerType, bool expected)
        {
            // Act
            var actual = providerType.IsLocalAuthorityProviderType();

            // Assert
            actual.Should().Be(expected);
        }


        [TestMethod, TestCategory("Unit")]
        [DataRow("Schoo", false)]
        [DataRow(null, false)]
        [DataRow(ProviderTypeExternal.StoreTypeNonProgrammeFundedProvider, true)]
        [DataRow(ProviderTypeExternal.NonProgrammeFundedProvider, true)]
        [DataRow("unknown", false)]
        public void IsNonProgrammeFundedProviderType_ReturnsExpectedValue(string providerType, bool expected)
        {
            // Act
            var actual = providerType.IsNonProgrammeFundedProviderType();

            // Assert
            actual.Should().Be(expected);
        }


        [TestMethod, TestCategory("Unit")]
        [DataRow("Schoo", false)]
        [DataRow(null, false)]
        [DataRow(ProviderTypeExternal.StoreTypeSpecialPost16Subtype, true)]
        [DataRow(ProviderTypeExternal.SpecialPost16Subtype, true)]
        public void IsSpecialPost16ProviderType_ReturnsExpectedValue(string providerSubType, bool expected)
        {
            // Act
            var actual = providerSubType.IsSpecialPost16ProviderType();

            // Assert
            actual.Should().Be(expected);
        }
    }
}