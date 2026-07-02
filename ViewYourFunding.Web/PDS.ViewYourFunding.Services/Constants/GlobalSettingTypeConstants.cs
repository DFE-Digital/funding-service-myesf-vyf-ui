using PDS.ViewYourFunding.Services.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Constants
{
    /// <summary>
    /// The Global Settings Type constants class.
    /// </summary>
    public static class GlobalSettingTypeConstants
    {
        /// <summary>
        /// The setting type id that represents the URL for the logged-in admin view.
        /// </summary>
        public const int UrlForLoggedInAdminViewTypeId = 1;

        /// <summary>
        /// The setting type id that is used to determine whether the View Your Funding area is available when admin is logged in.
        /// </summary>
        public const int DisplayViewYourFundingLoggedInAdminViewTypeId = 2;

        /// <summary>
        /// The setting type id that represents the URL for the logged-in provider view.
        /// </summary>
        public const int UrlForLoggedInProviderViewTypeId = 3;

        /// <summary>
        /// The setting type id that is used to determine whether the View Your Funding area is available when provider is logged in.
        /// </summary>
        public const int DisplayViewYourFundingLoggedInProviderViewTypeId = 4;

        /// <summary>
        /// The setting type id that represents the URL for the external view.
        /// </summary>
        public const int UrlForExternalViewTypeId = 5;

        /// <summary>
        /// The setting type id used to determine whether the External View Your Funding area is available.
        /// </summary>
        public const int DisplayViewYourFundingExternalViewTypeId = 6;

        /// <summary>
        /// The setting type id that represents the URL for the logged-in Multiple Academy Trust view.
        /// </summary>
        public const int UrlForLoggedInMultipleAcademyTrustViewTypeId = 7;

        /// <summary>
        /// The setting type id that is used to determine whether the View Your Funding area is available when a MAT is logged in.
        /// </summary>
        public const int DisplayLoggedInMultipleAcademyTrustViewTypeId = 8;

        /// <summary>
        /// The setting type id that is used to determine whether the different views will show selectors or not (only for testing purpose).
        /// </summary>
        [CacheKey("DisplaySelectorsTypeId")]
        public const int DisplaySelectorsTypeId = 9;

        /// <summary>
        /// The setting type id that represents the public facing url left part (e.g. https://example.org).
        /// </summary>
        public const int PublicFacingUrlLeftPart = 12;

        /// <summary>
        /// The setting type id that is used to determine whether the different views will show as a statement specification or not (only for testing purpose).
        /// </summary>
        [CacheKey("DisplayStatementSpecificationTypeId")]
        public const int DisplayStatementSpecificationTypeId = 13;

        /// <summary>
        /// The setting type id that is used to determine whether the different views will show data (only ever false for testing purpose).
        /// </summary>
        [CacheKey("ShowDataTypeId")]
        public const int ShowDataTypeId = 14;

        /// <summary>
        /// Get the cache key.
        /// </summary>
        /// <param name="id">The type id.</param>
        /// <returns>A cache key string or null.</returns>
        public static string GetCacheKey(int id)
        {
            var cacheKeyDictionary = GetCacheKeyAttributes();
            return cacheKeyDictionary.ContainsKey(id) ? cacheKeyDictionary[id] : null;
        }

        /// <summary>
        /// Get the cache key attributes.
        /// </summary>
        /// <returns>The dictionary.</returns>
        private static Dictionary<int, string> GetCacheKeyAttributes()
        {
            if (_cacheKeyDictionary != null)
            {
                return _cacheKeyDictionary;
            }

            var returnDictionary = new Dictionary<int, string>();

            var constantsType = typeof(GlobalSettingTypeConstants);
            var cacheKeyType = typeof(CacheKeyAttribute);

            var constantFields = constantsType.GetFields();

            foreach (var constantField in constantFields)
            {
                var constantValueObject = constantField.GetValue(null);

                if (!(constantValueObject is int constantInt))
                {
                    continue;
                }

                var cacheKeyAttribute = constantField.GetCustomAttributes(cacheKeyType, false)?.FirstOrDefault();

                if (cacheKeyAttribute == null)
                {
                    continue;
                }

                returnDictionary.Add(constantInt, (cacheKeyAttribute as CacheKeyAttribute).CacheKey);
            }

            _cacheKeyDictionary = returnDictionary;
            return returnDictionary;
        }

        /// <summary>
        /// The cache key dictionary.
        /// </summary>
        private static Dictionary<int, string> _cacheKeyDictionary;
    }
}