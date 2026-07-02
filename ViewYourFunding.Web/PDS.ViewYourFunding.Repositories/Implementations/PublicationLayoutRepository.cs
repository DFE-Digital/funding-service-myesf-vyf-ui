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
    /// The EF Core Db Publication layout repository.
    /// </summary>
    /// <seealso cref="IPublicationLayoutRepository" />
    public class PublicationLayoutRepository : Repository<PublicationLayout>, IPublicationLayoutRepository
    {
        private readonly ILoggerAdapter<Repository<PublicationLayout>> _loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationLayoutRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="loggerService">The logger service.</param>
        public PublicationLayoutRepository(
            Context dbContext,
            ILoggerAdapter<Repository<PublicationLayout>> loggerService)
            : base(dbContext, loggerService)
        {
            _loggerService = loggerService;
        }

        /// <inheritdoc/>
        public async Task<PublicationLayout> CreatePublicationLayout(PublicationLayout publicationLayout)
        {
            var inputPublicationLayout = publicationLayout;
            publicationLayout.LastUpdatedAt = DateTime.Now;
            publicationLayout.CreatedAt = DateTime.Now;

            var createResult = await AddAsync(publicationLayout);
            _loggerService?.LogInformation(Message(inputPublicationLayout, createResult, nameof(CreatePublicationLayout)));

            return createResult;
        }

        /// <inheritdoc/>
        public async Task<bool> DeletePublicationLayout(PublicationLayout publicationLayout)
        {
            var deleteResult = await RemoveAsync(publicationLayout.Id);
            var result = deleteResult > 0;
            _loggerService?.LogInformation($"Publication layout {nameof(DeletePublicationLayout)} action for {GetPublicationLayoutData(publicationLayout)} had result {result}");

            return result;
        }

        /// <inheritdoc/>
        public async Task<IList<PublicationLayout>> GetPublicationLayouts(int publicationId)
        {
            var publicationLayouts = await GetAllAsync(pubLayout => pubLayout.PublicationId == publicationId);
            return publicationLayouts?.ToList();
        }

        /// <inheritdoc/>
        public async Task<bool> UpdatePublicationLayout(PublicationLayout publicationLayout)
        {
            var changes = new Dictionary<string, string>();
            var dbPublicationLayout = await GetDatabasePublicationLayout(publicationLayout);

            if (dbPublicationLayout == null)
            {
                return false;
            }

            GetChanges(publicationLayout, dbPublicationLayout, changes);
            dbPublicationLayout.LastUpdatedAt = DateTime.Now;

            var saveAffectedRecords = await SaveChangesAsync();
            var result = saveAffectedRecords > 0;

            if (result)
            {
                _loggerService?.LogInformation(
                    $"Publication layout {dbPublicationLayout.Id} for publication {dbPublicationLayout.PublicationId} updated by {dbPublicationLayout.LastUpdatedBy}");
            }
            else
            {
                _loggerService?.LogInformation(
                    $"Publication layout {dbPublicationLayout.Id} for publication {dbPublicationLayout.PublicationId} didn't update");
            }

            return result;
        }

        private static void GetChanges(PublicationLayout newPublicationLayout, PublicationLayout originalPublicationLayout, Dictionary<string, string> changes)
        {
            if (newPublicationLayout?.LayoutId == originalPublicationLayout?.LayoutId)
            {
                return;
            }

            changes.Add(
                nameof(originalPublicationLayout.LayoutId),
                $"{originalPublicationLayout.LayoutId} => {newPublicationLayout.LayoutId}");

            originalPublicationLayout.LayoutId = newPublicationLayout.LayoutId;
        }

        private static string Message(PublicationLayout publicationLayout, PublicationLayout actionResult, string actionName)
        {
            return $"Publication layout update {actionName} for {GetPublicationLayoutData(publicationLayout)} had result {GetPublicationLayoutData(actionResult)}";
        }

        private static string GetPublicationLayoutData(PublicationLayout publicationLayout)
        {
            if (publicationLayout == null)
            {
                return "null";
            }

            return JsonConvert.SerializeObject(
                publicationLayout,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        private async Task<PublicationLayout> GetDatabasePublicationLayout(PublicationLayout publicationLayout)
        {
            return await GetAsync(publicationLayout.Id);
        }
    }
}