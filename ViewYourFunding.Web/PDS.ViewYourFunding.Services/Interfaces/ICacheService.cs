using PDS.ViewYourFunding.Services.Cache;
using System;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// Interface ICachingService.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Executes the specific action asynchronously.
        /// </summary>
        /// <typeparam name="T">The action result type.</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="action">The action.</param>
        /// <param name="cacheExpirationPolicy">Type of the cache expiration policy.</param>
        /// <param name="timeToLiveTimeSpan">The specified time to live time span.</param>
        /// <returns>The action result.</returns>
        Task<T> AddOrGetExistingResultAsync<T>(string cacheKey, Func<Task<T>> action, CacheExpirationPolicy cacheExpirationPolicy, TimeSpan? timeToLiveTimeSpan = null);

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <typeparam name="T">The action result type.</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="action">The action.</param>
        /// <param name="cacheExpirationPolicy">Type of the cache expiration policy.</param>
        /// <param name="timeToLiveTimeSpan">The specified time to live time span.</param>
        /// <returns>The action result.</returns>
        T AddOrGetExistingResult<T>(string cacheKey, Func<T> action, CacheExpirationPolicy cacheExpirationPolicy, TimeSpan? timeToLiveTimeSpan = null);

        /// <summary>
        /// Clears the cache.
        /// </summary>
        void ClearCache();

        /// <summary>
        /// Removes a cache item by key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        void RemoveCacheItem(string key);
    }
}
