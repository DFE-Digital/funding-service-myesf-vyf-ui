using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FundingDocumentMetaResponse = PDS.ViewYourFunding.Services.DTOs.FundingDocumentMetaResponse;

namespace PDS.ViewYourFunding.Services.Implementations.FundingView
{
    /// <summary>
    /// An implementation of the funding document storage service using Azure Blob Storage.
    /// </summary>
    public class AzureBlobStorageFundingDocumentService : IFundingDocumentStorageService
    {
        private readonly string _serviceName;
        private readonly string _apiKey;
        private readonly string _containerName;
        private readonly ILoggerAdapter<AzureBlobStorageFundingDocumentService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureBlobStorageFundingDocumentService"/> class.
        /// Construct a service to talk to blob storage.
        /// </summary>
        /// <param name="serviceName">The Azure blob storage name.</param>
        /// <param name="apiKey">The Azure blob storage key.</param>
        /// <param name="containerName">The Azure blob storage container name.</param>
        /// <param name="logger">The logger.</param>
        public AzureBlobStorageFundingDocumentService(string serviceName, string apiKey, string containerName, ILoggerAdapter<AzureBlobStorageFundingDocumentService> logger)
        {
            _serviceName = serviceName;
            _apiKey = apiKey;
            _containerName = containerName;
            _logger = logger;
        }

        /// <summary>
        /// Get filenames filtered by the criteria passed in.
        /// </summary>
        /// <param name="fundingPeriodCode">Funding period code.</param>
        /// <param name="fundingStreamCode">Funding stream code.</param>
        /// <returns>A list of matching filenames.</returns>
        public List<string> GetFilenames(string fundingPeriodCode, string fundingStreamCode)
        {
            var blobs = GetBlobFilenames();
            var prefix = FilenameHelper.BuildFundingDocumentFilenamePrefix(fundingStreamCode, fundingPeriodCode);

            return blobs.Where(filename => filename.Name.StartsWith(prefix))
                .Select(filename => filename.Name)
                .Where(filename =>
                {
                    var parts = FilenameHelper.GetComponentsFromFilename(filename);
                    return parts != null;
                })
                .ToList();
        }

        /// <summary>
        /// Get the web path of an existing funding document.
        /// </summary>
        /// <param name="filename">Filename to look up.</param>
        /// <returns>The web path as a string, along with other meta information.</returns>
        public async Task<FundingDocumentMetaResponse> GetMetadata(string filename)
        {
            var cloudBlockBlob = GetBlockBlob(filename);
            var exists = await cloudBlockBlob.ExistsAsync();

            if (!exists)
            {
                return null;
            }

            await cloudBlockBlob.FetchAttributesAsync();

            return new FundingDocumentMetaResponse
            {
                FilePath = cloudBlockBlob.Uri.AbsoluteUri,
                FileSizeBytes = cloudBlockBlob.Properties.Length,
                DocumentPublishedDate = cloudBlockBlob.Metadata.ContainsKey("PublishedDate")
                    && DateTime.TryParse(cloudBlockBlob.Metadata["PublishedDate"], out var documentPublishedDate) ? documentPublishedDate : DateTime.MinValue,
                FileExtension = cloudBlockBlob.Metadata.TryGetValue("FileExtension", out var fileExtension) ? fileExtension : null,
                FundingPeriodCode = cloudBlockBlob.Metadata.TryGetValue("FundingPeriodCode", out var fundingPeriodCode) ? fundingPeriodCode : null,
                FundingStreamCode = cloudBlockBlob.Metadata.TryGetValue("FundingStreamCode", out var fundingStreamCode) ? fundingStreamCode : null,
                CutOffDate = cloudBlockBlob.Metadata.TryGetValue("CutOffDate", out var cutoffDateStr)
                    && DateTime.TryParse(cutoffDateStr, out var cutoffDate) ? cutoffDate : DateTime.MaxValue,
                CreatedDate = cloudBlockBlob.Metadata.TryGetValue("CreatedDateUTC", out var createdDateStr)
                              && DateTime.TryParse(createdDateStr, out var createdDate) ? createdDate.ToUniversalTime() : (DateTime?)null,
                ModifiedDate = cloudBlockBlob.Properties.LastModified?.DateTime
            };
        }

        /// <summary>
        /// Get the file from storage as a stream.
        /// </summary>
        /// <param name="filename">Filename to look up.</param>
        /// <returns>File as a stream.</returns>
        public async Task<Stream> GetFile(string filename)
        {
            var cloudBlockBlob = GetBlockBlob(filename);
            var inputStream = new MemoryStream();

            await cloudBlockBlob.DownloadToStreamAsync(inputStream);
            inputStream.Position = 0;

            return inputStream;
        }

        /// <summary>
        /// Upload a funding document into Azure Blob storage.
        /// </summary>
        /// <param name="filename">The filename to save the funding document to.</param>
        /// <param name="fundingDocument">The funding document to upload (as a byte array).</param>
        /// <param name="publishedDate">Date the document was published.</param>
        /// <param name="cutoffDate">The cut off date (ignore data after this).</param>
        /// <param name="fileExtension">Extension of the file.</param>
        /// <param name="fundingPeriodCode">Funding period code.</param>
        /// <param name="fundingStreamCode">Funding stream code.</param>
        /// <returns>The path to the uploaded file and other metadata.</returns>
        public async Task<FundingDocumentMetaResponse> Upload(
            string filename,
            byte[] fundingDocument,
            DateTime publishedDate,
            DateTime cutoffDate,
            string fileExtension,
            string fundingPeriodCode,
            string fundingStreamCode)
        {
            _logger?.LogInformation("DEBUG1 Upload() start");

            var cloudBlockBlob = GetBlockBlob(filename);
            var createdDate = DateTime.UtcNow;

            cloudBlockBlob.Metadata.Add("CutoffDate", cutoffDate.ToGBFormat());
            cloudBlockBlob.Metadata.Add("PublishedDate", publishedDate.ToGBFormat());
            cloudBlockBlob.Metadata.Add("FileExtension", fileExtension);
            cloudBlockBlob.Metadata.Add("FundingPeriodCode", fundingPeriodCode);
            cloudBlockBlob.Metadata.Add("FundingStreamCode", fundingStreamCode);
            cloudBlockBlob.Metadata.Add("CreatedDateUTC", createdDate.ToUTCZFormat());

            await cloudBlockBlob.UploadFromByteArrayAsync(fundingDocument, 0, fundingDocument.Length);

            var result = new FundingDocumentMetaResponse
            {
                FilePath = cloudBlockBlob.Uri.AbsoluteUri,
                FileSizeBytes = fundingDocument.LongLength,
                DocumentPublishedDate = publishedDate,
                FileExtension = fileExtension,
                FundingPeriodCode = fundingPeriodCode,
                FundingStreamCode = fundingStreamCode,
                CutOffDate = cutoffDate,
                CreatedDate = createdDate
            };

            _logger?.LogInformation("DEBUG1 Upload() finished");

            return result;
        }

        /// <summary>
        /// Delete a funding document.
        /// </summary>
        /// <param name="filename">The filename to delete.</param>
        /// <returns><code>true</code> if the delete was successful, otherwise. <code>false</code>.</returns>
        public async Task<bool> Delete(string filename)
        {
            var cloudBlockBlob = GetBlockBlob(filename);

            return await cloudBlockBlob.DeleteIfExistsAsync();
        }

        /// <summary>
        /// Get a reference to a blob block on Azure.
        /// </summary>
        /// <param name="filename">Filename to look up.</param>
        /// <returns>A Cloud Block Blob object.</returns>
        private CloudBlockBlob GetBlockBlob(string filename)
        {
            var storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                    _serviceName,
                    _apiKey), true);

            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(_containerName);

            return cloudBlobContainer.GetBlockBlobReference(filename);
        }

        /// <summary>
        /// Get the blob filenames in a container.
        /// </summary>
        /// <returns>A list of the filenames in a container.</returns>
        private List<CloudBlockBlob> GetBlobFilenames()
        {
            var storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                    _serviceName,
                    _apiKey), true);

            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(_containerName);

            //return cloudBlobContainer.ListBlobs().Select(blob => (CloudBlockBlob)blob).ToList();
            var segment = Task.Run(async () => await cloudBlobContainer.ListBlobsSegmentedAsync(null)).GetAwaiter().GetResult();
            var list = new List<IListBlobItem>();
            list.AddRange(segment.Results);
            return list.Select(blob => (CloudBlockBlob)blob).ToList();
        }
    }
}