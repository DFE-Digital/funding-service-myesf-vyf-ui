using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// Interface exposing methods to manage funding documents (e.g. (spreadsheets).
    /// </summary>
    public interface IFundingDocumentService
    {
        /// <summary>
        /// Gets the funding documents (spreadsheets) for the given parameters.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code for which to retrieve spreadsheets.</param>
        /// <param name="fundingStreamName">The funding stream Name for which to retrieve spreadsheets.</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. FY-1920).</param>
        /// <param name="publications">The publications to limit to (optional).</param>
        /// <param name="preferredDocumentFormat">The preferred document format to ensure output is ordered correctly.</param>
        /// <returns>A read-only collection of the matching funding documents.</returns>
        Task<IReadOnlyCollection<FundingDocument>> GetFundingDocuments(
            string fundingStreamCode,
            string fundingStreamName,
            string fundingPeriodCode,
            ICollection<Publication> publications = null,
            string preferredDocumentFormat = "ods");
    }
}