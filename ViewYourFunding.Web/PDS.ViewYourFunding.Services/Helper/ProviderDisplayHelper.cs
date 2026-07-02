namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Helper methods for provider display related tasks.
    /// </summary>
    public static class ProviderDisplayHelper
    {
        /// <summary>
        /// Get the Search Result Display Name (e.g. 'School A (Mansfield, NG17 0AB)').
        /// </summary>
        /// <param name="organisationName">The organisation/provider name (e.g. School A).</param>
        /// <param name="organisationTown">The organisation/provider town name (e.g. Mansfield).</param>
        /// <param name="organisationPostcode">The organisation/provider postcode (e.g. NG17 0AB).</param>
        /// <returns>A formatted string.</returns>
        public static string GetSearchResultDisplayName(string organisationName, string organisationTown, string organisationPostcode)
        {
            var hasTownInformation = !string.IsNullOrEmpty(organisationTown);
            var hasPostCodeInformation = !string.IsNullOrEmpty(organisationPostcode);

            if (!hasTownInformation && !hasPostCodeInformation)
            {
                return organisationName;
            }

            if (!hasPostCodeInformation)
            {
                return $"{organisationName} ({organisationTown})";
            }

            if (!hasTownInformation)
            {
                return $"{organisationName} ({organisationPostcode})";
            }

            return $"{organisationName} ({organisationTown}, {organisationPostcode})";
        }
    }
}