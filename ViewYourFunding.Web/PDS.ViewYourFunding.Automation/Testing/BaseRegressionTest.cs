using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Testing
{
    /// <summary>
    /// The BaseRegressionTest class.
    /// </summary>
    public class BaseRegressionTest
    {
        #region Common properties

        /// <summary>
        /// The display view your funding.
        /// </summary>
        public bool DisplayViewYourFunding = false;

        #endregion


        #region Life cycle

        #endregion

        [TestInitialize]
        public void SetUp()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

            Driver.Initialise();

            // Handle Cookie message before running relevant test.
            StartPage.Open();
            StartPage.ClickAcceptCookiesButton();
            StartPage.ClickHideCookieMessageButton();
        }

        /// <summary>
        /// Tears down.
        /// </summary>
        [TestCleanup]
        public void TearDown()
        {
            Driver.Instance?.Close();
            Driver.Instance?.Quit();
        }

        #region Helpers


        /// <summary>
        /// Adds the document to azure file share.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="shareKey">The share key.</param>
        /// <returns>>A <see cref="Task"/> representing the asynchronous operation.>.</returns>
        public async Task AddDocumentToAzureFileShare(string fileName, Stream stream, string shareKey)
        {
            var shareRef = GetShare(shareKey);
            var file = shareRef.GetFileReference(fileName);

            var exists = await file.ExistsAsync();

            if (!exists)
            {
                stream.Seek(0, SeekOrigin.Begin);
                await file.UploadFromStreamAsync(stream);
            }
        }

        /// <summary>
        /// Removes the document from azure file share.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="shareKey">The share key.</param>
        /// <returns>>A <see cref="Task"/> representing the asynchronous operation.>.</returns>
        public async Task RemoveDocumentFromAzureFileShare(string fileName, string shareKey)
        {
            var shareRef = GetShare(shareKey);
            var file = shareRef.GetFileReference(fileName);

            var exists = await file.ExistsAsync();

            if (exists)
            {
                await file.DeleteAsync();
            }
        }

        /// <summary>
        /// Gets the share.
        /// </summary>
        /// <param name="shareKey">The share key.</param>
        /// <returns>The CloudFileDirectory result.</returns>
        private CloudFileDirectory GetShare(string shareKey)
        {
            var connectionString = ConfigurationManager.AppSettings["AzureFileStorageInternal_ConnectionString"];

            var cloudClient = CloudStorageAccount.Parse(connectionString);
            var cloudFileClient = cloudClient.CreateCloudFileClient();
            var share = cloudFileClient.GetShareReference(shareKey);

            return share.GetRootDirectoryReference();
        }

        #endregion


        #region Builders


        #endregion

    }
}