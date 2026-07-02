using Newtonsoft.Json;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Implementations
{
    /// <summary>
    /// The EF Core Db Publication repository.
    /// </summary>
    /// <seealso cref="IPublicationRepository" />
    public class PublicationRepository : Repository<Publication>, IPublicationRepository
    {
        private readonly ILoggerAdapter<Repository<Publication>> _loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="loggerService">The logger service.</param>
        public PublicationRepository(
            Context dbContext,
            ILoggerAdapter<Repository<Publication>> loggerService)
            : base(dbContext, loggerService)
        {
            _loggerService = loggerService;
        }

        /// <inheritdoc/>
        public async Task<Publication> CreatePublication(Publication publication)
        {
            var inputPublication = publication;
            publication.LastUpdatedAt = DateTime.Now;
            publication.CreatedAt = DateTime.Now;

            var createResult = await AddAsync(publication);
            _loggerService?.LogInformation(Message(inputPublication, createResult, nameof(CreatePublication)));

            return createResult;
        }

        /// <inheritdoc/>
        public async Task<bool> DeletePublication(Publication publication)
        {
            var deleteResult = await RemoveAsync(publication.Id);
            var result = deleteResult > 0;
            _loggerService?.LogInformation($"Publication {nameof(DeletePublication)} action for {GetPublicationData(publication)} had result {result}");

            return result;
        }

        /// <inheritdoc/>
        public async Task<IList<Publication>> GetPublications(int fundingStreamId)
        {
            var publications = await GetAllAsync(
                pub => pub.FundingStreamId == fundingStreamId,
                includeProperties: nameof(Publication.FundingStream));

            return publications?.ToList();
        }

        /// <inheritdoc/>
        public async Task<bool> UpdatePublication(Publication publication)
        {
            var changes = new Dictionary<string, string>();
            var dbPublication = await GetDatabasePublication(publication);

            if (dbPublication == null)
            {
                return false;
            }

            GetChanges(publication, dbPublication, changes);
            dbPublication.LastUpdatedAt = DateTime.Now;

            var saveAffectedRecords = await SaveChangesAsync();

            var result = saveAffectedRecords > 0;

            if (result)
            {
                _loggerService?.LogInformation(
                    $"Publication for funding stream {dbPublication.FundingStreamId} ({dbPublication.FundingStream?.FundingStreamCode ?? "?"}) updated by {dbPublication.LastUpdatedBy} for the following properties {string.Join(",", changes.Select(x => $"{x.Key} => {x.Value}"))}");
            }
            else
            {
                _loggerService?.LogInformation(
                    $"Publication for funding stream {dbPublication.FundingStreamId} ({dbPublication.FundingStream?.FundingStreamCode ?? "?"}) didn't update");
            }

            return result;
        }

        private static void GetChanges(Publication newPublication, Publication originalPublication, Dictionary<string, string> changes)
        {
            if (originalPublication.PublishedDate != newPublication.PublishedDate)
            {
                changes.Add(
                    nameof(Publication.PublishedDate),
                    $"{originalPublication.PublishedDate.ToShortDateString()} => {newPublication.PublishedDate.ToShortDateString()}");
                originalPublication.PublishedDate = newPublication.PublishedDate;
            }

            if (originalPublication.CutOffDate != newPublication.CutOffDate)
            {
                changes.Add(
                    nameof(Publication.CutOffDate),
                    $"{originalPublication.CutOffDate?.ToShortDateString()} => {newPublication.CutOffDate?.ToShortDateString()}");
                originalPublication.CutOffDate = newPublication.CutOffDate;
            }

            if (originalPublication.Status != newPublication.Status)
            {
                changes.Add(nameof(originalPublication.Status), $"{originalPublication.Status} => {newPublication.Status}");
                originalPublication.Status = newPublication.Status;
            }

            if (originalPublication.FundingPeriodCode != newPublication.FundingPeriodCode)
            {
                changes.Add(
                    nameof(originalPublication.FundingPeriodCode),
                    $"{originalPublication.FundingPeriodCode} => {newPublication.FundingPeriodCode}");
                originalPublication.FundingPeriodCode = newPublication.FundingPeriodCode;
            }

            if (originalPublication.Description != newPublication.Description)
            {
                changes.Add(nameof(originalPublication.Description), $"{originalPublication.Description} => {newPublication.Description}");
                originalPublication.Description = newPublication.Description;
            }

            if (originalPublication.SpreadsheetModelVersion != newPublication.SpreadsheetModelVersion)
            {
                changes.Add(
                    nameof(originalPublication.SpreadsheetModelVersion),
                    $"{originalPublication.SpreadsheetModelVersion} => {newPublication.SpreadsheetModelVersion}");
                originalPublication.SpreadsheetModelVersion = newPublication.SpreadsheetModelVersion;
            }

            if (originalPublication.UIModelVersion != newPublication.UIModelVersion)
            {
                changes.Add(
                    nameof(originalPublication.UIModelVersion),
                    $"{originalPublication.UIModelVersion} => {newPublication.UIModelVersion}");
                originalPublication.UIModelVersion = newPublication.UIModelVersion;
            }
        }

        private static string Message(Publication publication, Publication actionResult, string actionName)
        {
            return $"Publication update {actionName} for {GetPublicationData(publication)} had result {GetPublicationData(actionResult)}";
        }

        private static string GetPublicationData(Publication publication)
        {
            if (publication == null)
            {
                return "null";
            }

            return JsonConvert.SerializeObject(
                publication,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        private async Task<Publication> GetDatabasePublication(Publication publication)
        {
            return await GetAsync(publication.Id);
        }
    }
}