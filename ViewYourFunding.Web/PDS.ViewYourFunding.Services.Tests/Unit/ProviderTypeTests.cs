using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Constants;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    /// <summary>
    /// The ProviderTypeTests class.
    /// </summary>
    [TestClass]
    public class ProviderTypeTests
    {
        /// <summary>
        /// Froms the external returns expected value.
        /// </summary>
        /// <param name="externalType">Type of the external.</param>
        /// <param name="externalSubType">Type of the external sub.</param>
        /// <param name="expectedInternalType">Expected type of the internal.</param>
        [TestMethod, TestCategory("Unit")]
        [DataRow(null, null, ProviderTypeInternal.Default)]
        [DataRow(null, ProviderSubTypeExternal.NonMaintainedSpecialSchool, ProviderTypeInternal.NonMaintainedSpecialSchool)]
        [DataRow(ProviderTypeExternal.SpecialSchool, ProviderSubTypeExternal.NonMaintainedSpecialSchool, ProviderTypeInternal.NonMaintainedSpecialSchool)]
        [DataRow(ProviderTypeExternal.Academy, ProviderSubTypeExternal.NonMaintainedSpecialSchool, ProviderTypeInternal.NonMaintainedSpecialSchool)]
        [DataRow(ProviderTypeExternal.Academy, null, ProviderTypeInternal.Academy)]
        [DataRow(ProviderTypeExternal.FreeSchool, null, ProviderTypeInternal.Academy)]
        [DataRow(ProviderTypeExternal.SpecialSchool, null, ProviderTypeInternal.MaintainedSchool)]
        [DataRow(ProviderTypeExternal.LocalAuthorityMaintainedSchool, null, ProviderTypeInternal.MaintainedSchool)]
        public void FromExternal_ReturnsExpectedValue(string externalType, string externalSubType, string expectedInternalType)
        {
            // Arrange Act
            var actual = ProviderTypeInternal.FromExternal(externalType, externalSubType);

            // Assert
            actual.Should().Be(expectedInternalType);
        }
    }
}