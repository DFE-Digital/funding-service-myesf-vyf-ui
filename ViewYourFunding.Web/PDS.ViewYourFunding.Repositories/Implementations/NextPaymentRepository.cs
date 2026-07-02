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
    /// <seealso cref="INextPaymentRepository" />
    public class NextPaymentRepository : Repository<NextPayment>, INextPaymentRepository
    {
        private readonly ILoggerAdapter<Repository<NextPayment>> _loggerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPaymentRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="loggerService">The logger service.</param>
        public NextPaymentRepository(
            Context dbContext,
            ILoggerAdapter<Repository<NextPayment>> loggerService)
            : base(dbContext, loggerService)
        {
            _loggerService = loggerService;
        }

        /// <inheritdoc/>
        public async Task<NextPayment> CreateNextPayment(NextPayment nextPayment)
        {
            nextPayment.LastUpdatedAt = DateTime.Now;
            nextPayment.CreatedAt = DateTime.Now;
            var createResult = await AddAsync(nextPayment);
            _loggerService?.LogInformation(Message(nextPayment, createResult, nameof(CreateNextPayment)));

            return createResult;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteNextPayment(NextPayment nextPayment)
        {
            var deleteResult = await RemoveAsync(nextPayment.Id);
            var result = deleteResult > 0;
            _loggerService?.LogInformation(
                $"Next Payment {nameof(DeleteNextPayment)} action for {GetNextPaymentData(nextPayment)} had result {result}");

            return result;
        }

        /// <inheritdoc/>
        public async Task<IList<NextPayment>> GetNextPayments(int fundingStreamId)
        {
            var nextPayments = await GetAllAsync(
                nextPayment => nextPayment.FundingStreamId == fundingStreamId,
                includeProperties: $"{nameof(NextPayment.FundingStream)}, {nameof(NextPayment.NextPaymentType)}");

            return nextPayments?.ToList();
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateNextPayment(NextPayment nextPayment)
        {
            var changes = new Dictionary<string, string>();
            var dbNextPayment = await GetDatabaseNextPayment(nextPayment);
            if (dbNextPayment == null)
            {
                return false;
            }

            GetChanges(nextPayment, dbNextPayment, changes);
            dbNextPayment.LastUpdatedAt = DateTime.Now;

            var saveAffectedRecords = await SaveChangesAsync();

            var result = saveAffectedRecords > 0;
            if (result)
            {
                _loggerService?.LogInformation(
                    $"Next Payment for funding stream {dbNextPayment.FundingStreamId} ({dbNextPayment.FundingStream?.FundingStreamCode ?? "?"}) updated by {dbNextPayment.LastUpdatedBy} for the following properties {string.Join(",", changes.Select(x => $"{x.Key} => {x.Value}"))}");
            }
            else
            {
                _loggerService?.LogInformation(
                    $"Next Payment for funding stream {dbNextPayment.FundingStreamId} ({dbNextPayment.FundingStream?.FundingStreamCode ?? "?"}) didn't update");
            }

            return result;
        }

        private static string Message(NextPayment nextPayment, NextPayment actionResult, string actionName)
        {
            return $"Next Payment Type update {actionName} for {GetNextPaymentData(nextPayment)} had result {GetNextPaymentData(actionResult)}";
        }

        private static string GetNextPaymentData(NextPayment nextPaymentType)
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

        private static void GetChanges(NextPayment nextPayment, NextPayment dbNextPayment, Dictionary<string, string> changes)
        {
            if (dbNextPayment.NextPaymentDate != nextPayment.NextPaymentDate)
            {
                changes.Add(
                    nameof(dbNextPayment.NextPaymentDate),
                    $"{dbNextPayment.NextPaymentDate.ToShortDateString()} => {nextPayment.NextPaymentDate.ToShortDateString()}");
                dbNextPayment.NextPaymentDate = nextPayment.NextPaymentDate;
            }

            if (dbNextPayment.NextPaymentTypeId != nextPayment.NextPaymentTypeId)
            {
                changes.Add(nameof(dbNextPayment.NextPaymentTypeId), $"{dbNextPayment.NextPaymentTypeId} => {nextPayment.NextPaymentTypeId}");
                dbNextPayment.NextPaymentTypeId = nextPayment.NextPaymentTypeId;
            }

            if (dbNextPayment.FundingPeriodCode != nextPayment.FundingPeriodCode)
            {
                changes.Add(nameof(dbNextPayment.FundingPeriodCode), $"{dbNextPayment.FundingPeriodCode} => {nextPayment.FundingPeriodCode}");
                dbNextPayment.FundingPeriodCode = nextPayment.FundingPeriodCode;
            }

            if (dbNextPayment.Active != nextPayment.Active)
            {
                changes.Add(nameof(dbNextPayment.Active), $"{dbNextPayment.Active} => {nextPayment.Active}");
                dbNextPayment.Active = nextPayment.Active;
            }
        }

        private async Task<NextPayment> GetDatabaseNextPayment(NextPayment nextPayment)
        {
            var dbPublication = await GetAsync(nextPayment.Id);
            return dbPublication;
        }
    }
}