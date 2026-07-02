namespace PDS.ViewYourFunding.Services.Constants
{
    /// <summary>
    /// Constants class for internal provider types.
    /// </summary>
    public static class ProviderTypeInternal
    {
        /// <summary>
        /// The academy provider type.
        /// </summary>
        public const string Academy = "Academies";

        /// <summary>
        /// The non-maintained special schools provider type.
        /// </summary>
        public const string NonMaintainedSpecialSchool = "Non-maintained special schools";

        /// <summary>
        /// The maintained schools provider type.
        /// </summary>
        public const string MaintainedSchool = "Maintained schools";

        /// <summary>
        /// The default value to use if the provider type is unknown.
        /// </summary>
        public const string Default = "Unknown";

        /// <summary>
        /// Get the internal provider type for the given external provider type and sub-type.
        /// </summary>
        /// <param name="providerTypeExternal">The external provider type.</param>
        /// <param name="providerSubTypeExternal">The external provider sub-type.</param>
        /// <returns>The internal provider type for the given external provider type and sub-type.</returns>
        public static string FromExternal(string providerTypeExternal, string providerSubTypeExternal)
        {
            if (providerSubTypeExternal == ProviderSubTypeExternal.NonMaintainedSpecialSchool)
            {
                return NonMaintainedSpecialSchool;
            }

            if (providerTypeExternal == ProviderTypeExternal.Academy
                || (!string.IsNullOrEmpty(providerTypeExternal) && providerTypeExternal.Equals(ProviderTypeExternal.FreeSchool, System.StringComparison.CurrentCultureIgnoreCase)))
            {
                return Academy;
            }

            if (providerTypeExternal == ProviderTypeExternal.SpecialSchool
                || providerTypeExternal == ProviderTypeExternal.LocalAuthorityMaintainedSchool)
            {
                return MaintainedSchool;
            }

            return Default;
        }
    }
}