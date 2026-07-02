namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// Session Storage interface.
    /// </summary>
    /// <typeparam name="T">The class of the data.</typeparam>
    public interface ISessionStorage<T>
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>The data.</returns>
        T Get();

        /// <summary>
        /// Saves the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        void Save(T data);

        /// <summary>
        /// Removes this instance.
        /// </summary>
        void Remove();
    }
}