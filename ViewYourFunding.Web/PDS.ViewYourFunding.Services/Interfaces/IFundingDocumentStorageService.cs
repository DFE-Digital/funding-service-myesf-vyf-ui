using PDS.ViewYourFunding.Services.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// An interface to provide methods to save or fetch saved funding documents (e.g. spreadsheets) and the meta information around it.
    /// </summary>
    public interface IFundingDocumentStorageService
    {
        /// <summary>
        /// Get filenames filtered by the criteria passed in.
        /// </summary>
        /// <param name="fundingPeriodCode">Funding period code.</param>
        /// <param name="fundingStreamCode">Funding stream code.</param>
        /// <returns>A list of matching filenames.</returns>
        List<string> GetFilenames(string fundingPeriodCode, string fundingStreamCode);

        /// <summary>
        /// Get the file from storage as a stream.
        /// </summary>
        /// <param name="filename">Filename to look up.</param>
        /// <returns>File as a stream.</returns>
        Task<Stream> GetFile(string filename);

        /// <summary>
        /// Get the web path of an existing funding document.
        /// </summary>
        /// <param name="filename">Filename to look up.</param>
        /// <returns>The web path as a string, along with other meta information.</returns>
        Task<FundingDocumentMetaResponse> GetMetadata(string filename);

        /// <summary>
        /// Upload a funding document to a seperate service.
        /// </summary>
        /// <param name="filename">The filename to save the funding document to.</param>
        /// <param name="fundingDocument">The funding document to upload (as a byte array).</param>
        /// <param name="publishedDate">Date the document was published.</param>
        /// <param name="cutoffDate">The cut off date (ignore data after this).</param>
        /// <param name="fileExtension">Extension of the file.</param>
        /// <param name="fundingPeriodCode">Funding period code.</param>
        /// <param name="fundingStreamCode">Funding stream code.</param>
        /// <returns>The Funding document meta response.</returns>
        Task<FundingDocumentMetaResponse> Upload(
            string filename,
            byte[] fundingDocument,
            DateTime publishedDate,
            DateTime cutoffDate,
            string fileExtension,
            string fundingPeriodCode,
            string fundingStreamCode);

        /// <summary>
        /// Delete a funding document.
        /// </summary>
        /// <param name="filename">The filename to delete.</param>
        /// <returns><code>true</code> if the delete was successful, otherwise. <code>false</code>.</returns>
        Task<bool> Delete(string filename);
    }
}