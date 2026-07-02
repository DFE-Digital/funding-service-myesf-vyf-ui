using PDS.ViewYourFunding.Services.Constants;
using System;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// The Provider Subtype helper class.
    /// </summary>
    public static class ProviderTypeSubTypeHelper
    {
        /// <summary>
        /// Determines whether this is a special post16 provider type.
        /// </summary>
        /// <param name="providerSubtype">The provider subtype.</param>
        /// <returns>
        ///   True if is a special post16 provider type.
        /// </returns>
        public static bool IsSpecialPost16ProviderType(this string providerSubtype)
        {
            return StringValueCompare(ProviderTypeExternal.StoreTypeSpecialPost16Subtype, providerSubtype) ||
                   StringValueCompare(ProviderTypeExternal.SpecialPost16Subtype, providerSubtype);
        }

        /// <summary>
        /// Determines whether this is a non programme funded provider type.
        /// </summary>
        /// <param name="providerType">The provider type.</param>
        /// <returns>
        ///   True if is a non programme funded provider type.
        /// </returns>
        public static bool IsNonProgrammeFundedProviderType(this string providerType)
        {
            return StringValueCompare(ProviderTypeExternal.StoreTypeNonProgrammeFundedProvider, providerType) ||
                   StringValueCompare(ProviderTypeExternal.NonProgrammeFundedProvider, providerType);
        }

        /// <summary>
        /// Determines whether this is a school provider type.
        /// </summary>
        /// <param name="providerType">The provider type.</param>
        /// <returns>
        ///   True if is a special post16 provider type.
        /// </returns>
        public static bool IsSchoolProviderType(this string providerType)
        {
            return StringValueCompare(ProviderTypeExternal.StoreTypeSchool, providerType) ||
                   StringValueCompare(ProviderTypeExternal.LocalAuthorityMaintainedSchool, providerType) ||
                   StringValueCompare(ProviderTypeExternal.SpecialSchool, providerType);
        }

        /// <summary>
        /// Determines whether this is an academy provider type.
        /// </summary>
        /// <param name="providerType">The provider type.</param>
        /// <returns>
        ///   True if is an academy provider type.
        /// </returns>
        public static bool IsAcademyProviderType(this string providerType)
        {
            return StringValueCompare(ProviderTypeExternal.StoreTypeAcademy, providerType) ||
                   StringValueCompare(ProviderTypeExternal.Academy, providerType) ||
                   StringValueCompare(ProviderTypeExternal.FreeSchool, providerType) ||
                   StringValueCompare(ProviderTypeExternal.IndependentSchool, providerType);
        }

        /// <summary>
        /// Determines whether this is a further education provider type.
        /// </summary>
        /// <param name="providerType">The provider type.</param>
        /// <returns>
        ///   True if is a further education provider type.
        /// </returns>
        public static bool IsFurtherEducationProviderType(this string providerType)
        {
            return StringValueCompare(ProviderTypeExternal.StoreTypeFurtherEducation, providerType) ||
                   StringValueCompare(ProviderTypeExternal.College, providerType) ||
                   StringValueCompare(ProviderTypeExternal.University, providerType) ||
                   StringValueCompare(ProviderTypeExternal.OtherType, providerType) ||
                   StringValueCompare(ProviderTypeExternal.SixteenToNineteen, providerType);
        }

        /// <summary>
        /// Determines whether this is a local authority provider type.
        /// </summary>
        /// <param name="providerType">The provider type.</param>
        /// <returns>
        ///   True if is a local authority provider type.
        /// </returns>
        public static bool IsLocalAuthorityProviderType(this string providerType)
        {
            return StringValueCompare(ProviderTypeExternal.StoreTypeLocalAuthority, providerType) ||
                   StringValueCompare(ProviderTypeExternal.LocalAuthority, providerType);
        }

        private static bool StringValueCompare(string source, string destination)
        {
            return source.Equals(destination, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
