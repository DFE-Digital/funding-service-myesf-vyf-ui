using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Threading.Tasks;
using ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// The Publication Spreadsheet Meta data Service class.
    /// </summary>
    /// <seealso cref="IPublicationSpreadsheetMetaDataService" />
    public class PublicationSpreadsheetMetaDataService : IPublicationSpreadsheetMetaDataService
    {
        /// <summary>
        /// The GMT standard time identifier.
        /// </summary>
        private const string BritishTimeId = "GMT Standard Time";

        /// <summary>
        /// The funding document storage service.
        /// </summary>
        private readonly IFundingDocumentStorageService _fundingDocumentStorageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationSpreadsheetMetaDataService"/> class.
        /// </summary>
        /// <param name="fundingDocumentStorageService">The funding document storage service.</param>
        public PublicationSpreadsheetMetaDataService(
            IFundingDocumentStorageService fundingDocumentStorageService)
        {
            _fundingDocumentStorageService = fundingDocumentStorageService;
        }

        /// <inheritdoc />
        public async Task<PublicationSpreadsheetMetaData> GetPublicationSpreadsheetMetaDataAsync(Publication publication)
        {
            var filename = $"{publication.FundingStream.FundingStreamCode}_" +
                           $"{publication.FundingPeriodCode}_" +
                           $"{publication.PublishedDate.ToIncrementalDateFormat(true)}.ods";

            var result = await _fundingDocumentStorageService.GetMetadata(filename);

            var createdDateTime = result?.CreatedDate ?? result?.ModifiedDate;

            DateTime? britishLocalTime = null;

            try
            {
                var britishTimeZone = TimeZoneInfo.FindSystemTimeZoneById(BritishTimeId);
                if (createdDateTime.HasValue)
                {
                    britishLocalTime = TimeZoneInfo.ConvertTimeFromUtc(createdDateTime.Value, britishTimeZone);
                }
            }
            catch
            {
                var localTime = DateTime.Now;
                var localTimeUtc = DateTime.Now.ToUniversalTime();

                var offSet = localTime - localTimeUtc;
                if (createdDateTime.HasValue)
                {
                    britishLocalTime = createdDateTime + offSet;
                }
            }

            return new PublicationSpreadsheetMetaData
            {
                CreatedDateTime = britishLocalTime
            };
        }
    }
}