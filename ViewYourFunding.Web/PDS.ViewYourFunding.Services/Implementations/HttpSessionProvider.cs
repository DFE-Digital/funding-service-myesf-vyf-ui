using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PDS.ViewYourFunding.Services.Interfaces;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// Http Session Provider.
    /// </summary>
    /// <typeparam name="T">The class of the data.</typeparam>
    /// <seealso cref="ISessionStorage{T}" />
    public class HttpSessionProvider<T> : ISessionStorage<T>
    {
        private readonly IHttpContextAccessor _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSessionProvider{T}"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public HttpSessionProvider(IHttpContextAccessor session)
        {
            _session = session;
        }

        /// <summary>
        /// Gets the session key.
        /// </summary>
        /// <value>
        /// The session key.
        /// </value>
        private static string SessionKey => typeof(T).FullName;

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>The data.</returns>
        public T Get()
        {
            var data = _session.HttpContext.Session.GetString(SessionKey);
            return data == null ? default : JsonConvert.DeserializeObject<T>(data);
        }

        /// <summary>
        /// Removes this instance.
        /// </summary>
        public void Remove()
        {
            _session.HttpContext.Session.Remove(SessionKey);
        }

        /// <summary>
        /// Saves the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Save(T data)
        {
            _session.HttpContext.Session.SetString(SessionKey, JsonConvert.SerializeObject(data));
        }
    }
}
