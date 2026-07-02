using Microsoft.Extensions.Caching.Memory;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Cache;
using PDS.ViewYourFunding.Services.Interfaces;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// The Cache service class which uses Polly Caching to provide results from cache where available.
    /// https://github.com/App-vNext/Polly/wiki/Cache
    /// Implements the <see cref="ICacheService" />.
    /// </summary>
    /// <seealso cref="ICacheService" />
    public class MemoryCacheService : ICacheService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILoggerAdapter<MemoryCacheService> _logger;

        /// <summary>
        /// The memory cache.
        /// </summary>
        private readonly MemoryCache _memoryCache;

        /// <summary>
        /// The memory cache provider.
        /// </summary>
        private readonly MemoryCacheProvider _memoryCacheProvider;

        /// <summary>
        /// The policy registry.
        /// </summary>
        private readonly PolicyRegistry _policyRegistry;

        /// <summary>
        /// The global cache time to live time span.
        /// </summary>
        private readonly TimeSpan _globalCacheTimeToLiveTimeSpan;

        /// <summary>
        /// The policy name.
        /// </summary>
        private const string PolicyName = "ViewYourFunding";

        /// <summary>
        /// The async policy name.
        /// </summary>
        private const string AsyncPolicyName = "ViewYourFunding_Async";

        /// <summary>
        /// The cache keys.
        /// </summary>
        private IList<string> _cacheKeys = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="globalTimeToLiveMinutes">The configured global time to live minutes value.</param>
        public MemoryCacheService(ILoggerAdapter<MemoryCacheService> logger, int globalTimeToLiveMinutes)
        {
            _logger = logger;

            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _memoryCacheProvider = new MemoryCacheProvider(_memoryCache);
            _policyRegistry = new PolicyRegistry();
            _globalCacheTimeToLiveTimeSpan =
                TimeSpan.FromMinutes(globalTimeToLiveMinutes);
            AddPolicyNamesToRegistry();
        }

        /// <summary>
        /// Add or Get the existing action result from cache.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="action">The action.</param>
        /// <param name="cacheExpirationPolicy">Type of the cache policy.</param>
        /// <param name="timeToLiveTimeSpan">The specified time to live time span.</param>
        /// <returns>The action result.</returns>
        public T AddOrGetExistingResult<T>(
            string cacheKey,
            Func<T> action,
            CacheExpirationPolicy cacheExpirationPolicy,
            TimeSpan? timeToLiveTimeSpan)
        {
            AddToCacheKeys(cacheKey);
            switch (cacheExpirationPolicy)
            {
                case CacheExpirationPolicy.Absolute:
                case CacheExpirationPolicy.Sliding:
                    var cachePolicy = _policyRegistry.Get<ISyncPolicy>(PolicyName);
                    var contextData = GetContextData(cacheExpirationPolicy, timeToLiveTimeSpan);
                    return cachePolicy.Execute(pollyContext => action(), new Context(cacheKey, contextData));
                default:
                    // Executes the action without applying any caching logic.
                    return Policy.NoOp<T>().Execute(action);
            }
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void ClearCache()
        {
            foreach (var item in _cacheKeys)
            {
                _memoryCache.Remove(item);
            }

            _cacheKeys = new List<string>();
        }

        /// <summary>
        /// Add or Get the existing action result from cache asynchronously.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="action">The action.</param>
        /// <param name="cacheExpirationPolicy">Type of the cache policy.</param>
        /// <param name="timeToLiveTimeSpan">The specified time to live time span.</param>
        /// <returns>The action result.</returns>
        public async Task<T> AddOrGetExistingResultAsync<T>(
            string cacheKey,
            Func<Task<T>> action,
            CacheExpirationPolicy cacheExpirationPolicy,
            TimeSpan? timeToLiveTimeSpan)
        {
            AddToCacheKeys(cacheKey);
            switch (cacheExpirationPolicy)
            {
                case CacheExpirationPolicy.Absolute:
                case CacheExpirationPolicy.Sliding:
                    var cachePolicy = _policyRegistry.Get<IAsyncPolicy>(AsyncPolicyName);
                    var contextData = GetContextData(cacheExpirationPolicy, timeToLiveTimeSpan);
                    return await cachePolicy.ExecuteAsync(pollyContext => action(), new Context(cacheKey, contextData));
                default:
                    // Executes the action without applying any caching logic.
                    return await Policy.NoOpAsync<T>().ExecuteAsync(action);
            }
        }

        /// <inheritdoc/>
        public void RemoveCacheItem(string key)
        {
            _memoryCache.Remove(key);
            _cacheKeys.Remove(key);
        }

        /// <summary>
        /// Adds to cache keys list if not in the list.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        private void AddToCacheKeys(string cacheKey)
        {
            if (!_cacheKeys.Contains(cacheKey))
            {
                _cacheKeys.Add(cacheKey);
            }
        }

        /// <summary>
        /// Gets the context data.
        /// </summary>
        /// <param name="cacheExpirationPolicy">Type of the cache policy.</param>
        /// <param name="actionTimeSpan">The action time span.</param>
        /// <returns>The dictionary for context data.</returns>
        private Dictionary<string, object> GetContextData(CacheExpirationPolicy cacheExpirationPolicy, TimeSpan? actionTimeSpan)
        {
            var contextData = new Dictionary<string, object>
            {
                [ContextualTtl.TimeSpanKey] = actionTimeSpan ?? _globalCacheTimeToLiveTimeSpan,
                [ContextualTtl.SlidingExpirationKey] = cacheExpirationPolicy == CacheExpirationPolicy.Sliding
            };
            return contextData;
        }

        /// <summary>
        /// Adds the policy names to registry.
        /// </summary>
        private void AddPolicyNamesToRegistry()
        {
            if (!_policyRegistry.ContainsKey(PolicyName))
            {
                _policyRegistry.Add(PolicyName, GetContextualPolicy());
            }

            if (!_policyRegistry.ContainsKey(AsyncPolicyName))
            {
                _policyRegistry.Add(AsyncPolicyName, GetContextualPolicyAsync());
            }
        }

        /// <summary>
        /// Gets the contextual policy asynchronously.
        /// </summary>
        /// <returns>The Cache Policy.</returns>
        private IAsyncPolicy GetContextualPolicyAsync()
        {
            return Policy.CacheAsync(
                _memoryCacheProvider,
                new ContextualTtl(),
                new DefaultCacheKeyStrategy(),
                onCachePut: (context, key) => LogInformation($"VYF CACHING : Caching '{key}'."),
                onCacheGet: (context, key) => { },
                onCachePutError: (context, key, exception) => _logger?.LogError(exception, exception.Message),
                onCacheGetError: (context, key, exception) => _logger?.LogError(exception, exception.Message),
                onCacheMiss: (context, key) => LogInformation($"VYF CACHING : Cache miss for '{key}'."));
        }

        /// <summary>
        /// Gets the contextual policy.
        /// </summary>
        /// <returns>The Cache Policy.</returns>
        private ISyncPolicy GetContextualPolicy()
        {
            return Policy.Cache(
                _memoryCacheProvider,
                new ContextualTtl(),
                new DefaultCacheKeyStrategy(),
                onCachePut: (context, key) => LogInformation($"VYF CACHING : Caching '{key}'."),
                onCacheGet: (context, key) => { },
                onCachePutError: (context, key, exception) => _logger?.LogError(exception, exception.Message),
                onCacheGetError: (context, key, exception) => _logger?.LogError(exception, exception.Message),
                onCacheMiss: (context, key) => LogInformation($"VYF CACHING : Cache miss for '{key}'."));
        }

        private void LogInformation(string message)
        {
            _logger?.LogInformation(message);
        }
    }
}