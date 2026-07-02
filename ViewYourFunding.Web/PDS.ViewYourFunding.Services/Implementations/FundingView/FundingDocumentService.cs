using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations.FundingView
{
    /// <summary>
    /// A service to use to get the current funding documents.
    /// </summary>
    public class FundingDocumentService : IFundingDocumentService
    {
        private readonly IFundingDocumentStorageService _fundingDocumentStoreService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingDocumentService"/> class.
        /// Construct an instance of FundingDocumentService.
        /// </summary>
        /// <param name="fundingDocumentStoreService">How should the generated funding documents get stored.</param>
        public FundingDocumentService(IFundingDocumentStorageService fundingDocumentStoreService)
        {
            _fundingDocumentStoreService = fundingDocumentStoreService;
        }

        /// <summary>
        /// Get the funding documents from their storage.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code for which to retrieve funding documents (e.g. spreadsheets).</param>
        /// <param name="fundingStreamName">The funding stream name for which to retrieve funding documents (e.g. spreadsheets).</param>
        /// <param name="fundingPeriodCode">The period code to look at (e.g. AY-1920).</param>
        /// <param name="publications">Publications to filter to (optional).</param>
        /// <param name="preferredDocumentFormat">The preferred document format to ensure output is ordered correctly.</param>
        /// <returns>An enumerated collection of funding documents.</returns>
        public async Task<IReadOnlyCollection<FundingDocument>> GetFundingDocuments(
            string fundingStreamCode,
            string fundingStreamName,
            string fundingPeriodCode,
            ICollection<Publication> publications = null,
            string preferredDocumentFormat = "ods")
        {
            var returnList = new List<FundingDocument>();
            var filePaths = _fundingDocumentStoreService.GetFilenames(fundingPeriodCode, fundingStreamCode);

            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(fundingPeriodCode);

            foreach (var filename in filePaths)
            {
                var result = await _fundingDocumentStoreService.GetMetadata(filename);

                returnList.Add(new FundingDocument
                {
                    DocumentPublishedDate = result.DocumentPublishedDate,
                    FileExtension = result.FileExtension,
                    FilePath = result.FilePath,
                    FileSizeBytes = result.FileSizeBytes,
                    FundingStreamCode = result.FundingStreamCode,
                    FundingStreamName = fundingStreamName,
                    YearFrom = yearFrom,
                    YearTo = yearTo,
                    CutOffDate = result.CutOffDate,
                    IsFinal = false
                });
            }

            // Only return the file with the latest cut-off date for each distinct published date.
            var filteredReturnList = returnList
                .GroupBy(fundingDocument => fundingDocument.DocumentPublishedDate)
                .SelectMany(group => group.Where(fundingDocument => fundingDocument.CutOffDate == group.Max(maxFundingDocument => maxFundingDocument.CutOffDate)));

            if (publications != null)
            {
                filteredReturnList = filteredReturnList
                    .Where(fundingDocument => publications.Any(publication => publication.PublishedDate == fundingDocument.DocumentPublishedDate));
            }

            var newest = filteredReturnList
                .OrderByDescending(fundingDocument => fundingDocument.DocumentPublishedDate)
                .ThenBy(fundingDocument => fundingDocument.FileExtension == preferredDocumentFormat ? 0 : 1)
                .FirstOrDefault();

            if (newest != null)
            {
                newest.IsFinal = true;
            }

            return filteredReturnList.ToList();
        }
    }
}