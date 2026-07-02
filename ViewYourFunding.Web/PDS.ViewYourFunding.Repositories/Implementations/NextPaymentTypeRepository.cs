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
    /// The EF Core Db next payment type repository.
    /// </summary>
    /// <seealso cref="INextPaymentTypeRepository" />
    public class NextPaymentTypeRepository : Repository<NextPaymentType>, INextPaymentTypeRepository
    {
        private readonly ILoggerAdapter<Repository<NextPaymentType>> _loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPaymentTypeRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="loggerService">The logger service.</param>
        public NextPaymentTypeRepository(
            Context dbContext,
            ILoggerAdapter<Repository<NextPaymentType>> loggerService)
            : base(dbContext, loggerService)
        {
            _loggerService = loggerService;
        }

        /// <inheritdoc/>
        public async Task<NextPaymentType> CreateNextPaymentType(NextPaymentType nextPaymentType)
        {
            nextPaymentType.LastUpdatedAt = DateTime.Now;
            nextPaymentType.CreatedAt = DateTime.Now;
            var createResult = await AddAsync(nextPaymentType);
            _loggerService?.LogInformation(Message(nextPaymentType, createResult, nameof(CreateNextPaymentType)));

            return createResult;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteNextPaymentType(NextPaymentType nextPaymentType)
        {
            var deleteResult = await RemoveAsync(nextPaymentType.Id);
            var result = deleteResult > 0;
            _loggerService?.LogInformation(
                $"Next Payment {nameof(DeleteNextPaymentType)} action for {GetNextPaymentTypeData(nextPaymentType)} had result {result}");

            return result;
        }

        /// <inheritdoc/>
        public async Task<IList<NextPaymentType>> GetNextPaymentTypes(int fundingStreamId)
        {
            var nextPaymentTypes = await GetAllAsync(
                nextPayment => nextPayment.FundingStreamId == fundingStreamId,
                includeProperties: nameof(NextPaymentType.FundingStream));

            return nextPaymentTypes?.ToList();
        }

        /// <inheritdoc/>
        public async Task<IList<NextPaymentType>> GetAllNextPaymentTypes()
        {
            var nextPaymentTypes = await GetAllAsync(
                includeProperties: nameof(NextPaymentType.FundingStream));

            return nextPaymentTypes?.ToList();
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateNextPaymentType(NextPaymentType nextPaymentType)
        {
            var changes = new Dictionary<string, string>();
            var dbNextPaymentType = await GetDatabaseNextPaymentType(nextPaymentType);
            if (dbNextPaymentType == null)
            {
                return false;
            }

            GetChanges(nextPaymentType, dbNextPaymentType, changes);
            dbNextPaymentType.LastUpdatedAt = DateTime.Now;

            var saveAffectedRecords = await SaveChangesAsync();

            var result = saveAffectedRecords > 0;
            if (result)
            {
                _loggerService?.LogInformation(
                    $"Next Payment type for funding stream {dbNextPaymentType.FundingStreamId} ({dbNextPaymentType.FundingStream?.FundingStreamCode ?? "?"}) updated by {dbNextPaymentType.LastUpdatedBy} for the following properties {string.Join(",", changes.Select(x => $"{x.Key} => {x.Value}"))}");
            }
            else
            {
                _loggerService?.LogInformation(
                    $"Next Payment type  for funding stream {dbNextPaymentType.FundingStreamId} ({dbNextPaymentType.FundingStream?.FundingStreamCode ?? "?"}) didn't update");
            }

            return result;
        }

        private static void GetChanges(NextPaymentType nextPaymentType, NextPaymentType dbNextPaymentType, Dictionary<string, string> changes)
        {
            if (dbNextPaymentType.Description != nextPaymentType.Description)
            {
                changes.Add(nameof(dbNextPaymentType.Description), $"{dbNextPaymentType.Description} => {nextPaymentType.Description}");
                dbNextPaymentType.Description = nextPaymentType.Description;
            }

            if (dbNextPaymentType.TypeCode != nextPaymentType.TypeCode)
            {
                changes.Add(nameof(dbNextPaymentType.TypeCode), $"{dbNextPaymentType.TypeCode} => {nextPaymentType.TypeCode}");
                dbNextPaymentType.TypeCode = nextPaymentType.TypeCode;
            }
        }

        private static string Message(NextPaymentType nextPaymentType, NextPaymentType actionResult, string actionName)
        {
            return $"Next Payment Type update {actionName} for {GetNextPaymentTypeData(nextPaymentType)} had result {GetNextPaymentTypeData(actionResult)}";
        }

        private static string GetNextPaymentTypeData(NextPaymentType nextPaymentType)
        {
            if (nextPaymentType == null)
            {
                return "null";
            }

            return JsonConvert.SerializeObject(
                nextPaymentType,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        private async Task<NextPaymentType> GetDatabaseNextPaymentType(NextPaymentType nextPaymentType)
        {
            var dbNextPaymentType = await GetAsync(nextPaymentType.Id);
            return dbNextPaymentType;
        }
    }
}