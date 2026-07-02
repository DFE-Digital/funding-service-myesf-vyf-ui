using System;

namespace PDS.ViewYourFunding.Services.Attributes
{
    /// <summary>
    /// An attribute to determine the cache key.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class CacheKeyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheKeyAttribute"/> class.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        public CacheKeyAttribute(string cacheKey)
        {
            CacheKey = cacheKey;
        }

        /// <summary>
        /// Gets or sets the cache key.
        /// </summary>
        public string CacheKey { get; set; }
    }
}