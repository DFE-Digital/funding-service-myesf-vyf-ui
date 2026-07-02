namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// An interface that provides operations to utilise UI models.
    /// </summary>
    public interface IModelFileStoreService
    {
        /// <summary>
        /// Get the filenames for all models.
        /// </summary>
        /// <param name="subFolderPath">A sub folder path to look for models in.</param>
        /// <returns>A list of filenames as a string array.</returns>
        string[] GetModelFilenames(string subFolderPath);

        /// <summary>
        /// Read a model file as a string.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>The template file as a string.</returns>
        string ReadFileAsString(string path);

        /// <summary>
        /// Read a model file or resource into a byte array.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>A template file or resource as a byte array.</returns>
        byte[] ReadFileAsByteArray(string path);

        /// <summary>
        /// Is there a file at the given path?.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>True if the path is a file.</returns>
        bool Exists(string path);
    }
}