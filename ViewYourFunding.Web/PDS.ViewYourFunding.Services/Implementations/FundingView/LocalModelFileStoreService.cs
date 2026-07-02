using PDS.ViewYourFunding.Services.Interfaces;
using System;
using System.IO;

namespace PDS.ViewYourFunding.Services.Implementations.FundingView
{
    /// <summary>
    /// A local filesystem implementation that provides operations to utilise UI models.
    /// </summary>
    public class LocalModelFileStoreService : IModelFileStoreService
    {
        private readonly string _rootDirectoryPath;
        private readonly string _folderPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalModelFileStoreService"/> class.
        /// Setup a local file storage service, recording the root directory path to lookup files from.
        /// </summary>
        /// <param name="rootDirectoryPath">The path of the root directory to use.</param>
        /// <param name="folderPath">The folder path.</param>
        public LocalModelFileStoreService(string rootDirectoryPath, string folderPath = "Views/FundingUIModels")
        {
            _rootDirectoryPath = rootDirectoryPath;
            _folderPath = folderPath;
        }

        /// <summary>
        /// Get the filenames for all models.
        /// </summary>
        /// <param name="subFolderPath">A sub folder path to look for models in.</param>
        /// <returns>A list of filenames as a string array.</returns>
        public string[] GetModelFilenames(string subFolderPath)
        {
            var folderPath = Path.Combine(_rootDirectoryPath, _folderPath, subFolderPath);

            if (!Directory.Exists(folderPath))
            {
                return null;
            }

            return Directory.GetFiles(folderPath);
        }

        /// <summary>
        /// Read a model file as a string.
        /// </summary>
        /// <param name="path">>The path to the file.</param>
        /// <returns>A template file or resource as a byte array.</returns>
        public byte[] ReadFileAsByteArray(string path)
        {
            try
            {
                return File.ReadAllBytes(Path.Combine(_rootDirectoryPath, _folderPath, path));
            }
            catch (Exception ex)
            {
                throw new Exception($"File '{path}' could not be loaded", ex);
            }
        }

        /// <summary>
        /// Read a model file or resource into a byte array.
        /// </summary>
        /// <param name="path">>The path to the file.</param>
        /// <returns>The template file as a string.</returns>
        public string ReadFileAsString(string path)
        {
            return File.ReadAllText(path, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Is there a file at the given path?.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>True if the path is a file.</returns>
        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}