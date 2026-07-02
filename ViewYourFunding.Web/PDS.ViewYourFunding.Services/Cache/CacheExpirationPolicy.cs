namespace PDS.ViewYourFunding.Services.Cache
{
    /// <summary>
    /// The CacheExpirationPolicy class - Time based expiration strategies.
    /// </summary>
    public enum CacheExpirationPolicy
    {
        /// <summary>
        /// The absolute expiration.
        /// </summary>
        Absolute,

        /// <summary>
        /// The sliding expiration.
        /// </summary>
        Sliding,

        /// <summary>
        /// Bypass the caching.
        /// </summary>
        Bypass
    }
}
